using EQX.Core.InOut;
using EQX.Core.Robot;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Robot;
using PIFilmAutoDetachCleanMC.Defines.Process.Step._07.RobotLoadProcess;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;
using PIFilmAutoDetachCleanMC.Recipe;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class RobotUnloadProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly IRobot _robotUnload;
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly RobotUnloadRecipe _robotUnloadRecipe;
        private readonly IDInputDevice _robotUnloadInput;
        private readonly IDOutputDevice _robotUnloadOutput;
        private readonly DieHardK180Plasma _plasma;
        private readonly CWorkData _workData;
        private readonly MachineStatus _machineStatus;


        private bool IsPlasmaPrepare { get; set; } = false;

        private IDOutput GlassVacOnOff1 => _devices.Outputs.UnloadRobotVac1OnOff;
        private IDOutput GlassVacOnOff2 => _devices.Outputs.UnloadRobotVac2OnOff;
        private IDOutput GlassVacOnOff3 => _devices.Outputs.UnloadRobotVac3OnOff;
        private IDOutput GlassVacOnOff4 => _devices.Outputs.UnloadRobotVac4OnOff;

        private bool GlassVac1 => _devices.Inputs.UnloadRobotVac1.Value;
        private bool GlassVac2 => _devices.Inputs.UnloadRobotVac2.Value;
        private bool GlassVac3 => _devices.Inputs.UnloadRobotVac3.Value;
        private bool GlassVac4 => _devices.Inputs.UnloadRobotVac4.Value;
        private ICylinder Cyl1 => _devices.Cylinders.UnloadRobotCyl1UpDown;
        private ICylinder Cyl2 => _devices.Cylinders.UnloadRobotCyl2UpDown;
        private ICylinder Cyl3 => _devices.Cylinders.UnloadRobotCyl3UpDown;
        private ICylinder Cyl4 => _devices.Cylinders.UnloadRobotCyl4UpDown;

        private bool IsCylindersUp => Cyl1.IsBackward && Cyl2.IsBackward && Cyl3.IsBackward && Cyl4.IsBackward;
        private bool IsCylindersDown => Cyl1.IsForward && Cyl2.IsForward && Cyl3.IsForward && Cyl4.IsForward;

        private bool GlassDetect1 => _devices.Inputs.UnloadRobotDetect1.Value;
        private bool GlassDetect2 => _devices.Inputs.UnloadRobotDetect2.Value;
        private bool GlassDetect3 => _devices.Inputs.UnloadRobotDetect3.Value;
        private bool GlassDetect4 => _devices.Inputs.UnloadRobotDetect4.Value;

        private int PlasmaSpeed => _robotUnloadRecipe.RobotPlasmaSpeed;
        private int LowSpeed => _robotUnloadRecipe.RobotSpeedLow;
        private int HightSpeed => _robotUnloadRecipe.RobotSpeedHigh;


        private bool SendCommand(ERobotCommand command, int lowSpeed, int highSpeed, string[] paras = null)
        {
            if (paras == null || paras.Count() == 0)
            {
                _robotUnload.SendCommand(RobotHelpers.MotionCommands(command, lowSpeed, highSpeed));
            }
            else
            {
                _robotUnload.SendCommand(RobotHelpers.MotionCommands(command, lowSpeed, highSpeed, paras));
            }

            Wait(5000, () => _robotUnload.ReadResponse(RobotHelpers.MotionRspStart(command)));
            return !WaitTimeOutOccurred;
        }
        #endregion

        #region Inputs
        private IDInput PeriRDY => _devices.Inputs.UnloadRobPeriRdy;
        private IDInput StopMess => _devices.Inputs.UnloadRobStopmess;
        private IDInput ProACT => _devices.Inputs.UnloadRobProAct;
        #endregion

        #region Outputs
        private IDOutput DrivesOn => _devices.Outputs.UnloadRobDrivesOn;
        private IDOutput ConfMess => _devices.Outputs.UnloadRobConfMess;
        private IDOutput ExtStart => _devices.Outputs.UnloadRobExtStart;
        private IDOutput DrivesOff => _devices.Outputs.UnloadRobDrivesOff;
        private IDOutput MoveEnable => _devices.Outputs.UnloadRobMoveEnable;
        #endregion

        #region Flags
        private bool FlagUnloadAlignRequestUnload
        {
            get
            {
                return _robotUnloadInput[(int)ERobotUnloadProcessInput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD];
            }
        }

        private bool FlagRobotUnloadDoneReceived
        {
            get
            {
                return _robotUnloadInput[(int)ERobotUnloadProcessInput.UNLOAD_ALIGN_UNLOAD_DONE_RECEIVED];
            }
        }

        private bool FlagRobotUnloadPickDone
        {
            set
            {
                _robotUnloadOutput[(int)ERobotUnloadProcessOutput.ROBOT_UNLOAD_PICK_DONE] = value;
            }
        }

        private bool FlagMachineRequestPlace
        {
            get
            {
                return _robotUnloadInput[(int)ERobotUnloadProcessInput.MACHINE_REQUEST_PLACE];
            }
        }
        #endregion

        #region Constructor
        public RobotUnloadProcess(Devices devices,
            CommonRecipe commonRecipe,
            RobotUnloadRecipe robotUnloadRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("RobotUnloadInput")] IDInputDevice robotUnloadInput,
            [FromKeyedServices("RobotUnloadOutput")] IDOutputDevice robotUnloadOutput,
            [FromKeyedServices("RobotUnload")] IRobot robotUnload,
            DieHardK180Plasma plasma,
            CWorkData workData)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _robotUnloadRecipe = robotUnloadRecipe;
            _machineStatus = machineStatus;
            _robotUnloadInput = robotUnloadInput;
            _robotUnloadOutput = robotUnloadOutput;
            _robotUnload = robotUnload;
            _plasma = plasma;
            _workData = workData;
        }
        #endregion

        #region Override Methods
        public override bool ProcessToOrigin()
        {
            switch ((ERobotUnloadToOriginStep)Step.OriginStep)
            {
                case ERobotUnloadToOriginStep.Start:
                    Log.Debug("To Origin Start");
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.ReConnectIfRequired:
                    if (!_robotUnload.IsConnected)
                    {
                        Log.Debug("Robot is not connected, trying to reconnect.");
                        _robotUnload.Connect();
                    }

                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.CheckConnection:
                    if (!_robotUnload.IsConnected)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Connect_Fail);
                        break;
                    }

                    Log.Debug("Robot is connected.");
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.EnableOutput:
                    Log.Debug("Enable Output MoveEnable, DriverOff");
                    DrivesOff.Value = true;
                    MoveEnable.Value = true;
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotMotionOnCheck:
                    if (!PeriRDY.Value)
                    {
                        Log.Debug("Driver ON");
                        DrivesOn.Value = true;
                        Step.OriginStep++;
                        break;
                    }

                    Step.OriginStep = (int)ERobotUnloadToOriginStep.RobotStopMessCheck;
                    break;
                case ERobotUnloadToOriginStep.RobotMotionOn_Delay:
                    Wait(3000, () => PeriRDY.Value);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotMotionOn_Disable:
                    Log.Debug("Driver OFF");
                    DrivesOn.Value = false;
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotStopMessCheck:
                    if (StopMess.Value)
                    {
                        Log.Debug("Confirm message.");
                        ConfMess.Value = true;
                        Step.OriginStep++;
                        break;
                    }

                    Step.OriginStep = (int)ERobotLoadToOriginStep.RobotExtStart_Enable;
                    break;
                case ERobotUnloadToOriginStep.RobotConfMess_Delay:
                    Wait(500);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotConfMess_Disable:
                    Log.Debug("Disable Confirm message.");
                    ConfMess.Value = false;
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotExtStart_Enable:
                    Log.Debug("Enable External Start.");
                    ExtStart.Value = true;
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotExtStart_Delay:
                    Wait(500);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotExtStart_Disable:
                    Log.Debug("Disable External Start.");
                    ExtStart.Value = false;
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.RobotProgramStart_Check:
#if SIMULATION
                    SimulationInputSetter.SetSimInput(ProACT, true);
#endif
                    if (!ProACT.Value)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Programing_Not_Running);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(ProACT, false);
#endif
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.SendPGMStart:
                    _robotUnload.SendCommand(RobotHelpers.PCPGMStart);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.SendPGMStart_Check:
                    if (_robotUnload.ReadResponse(5000, "RobotReady,0\r\n"))
                    {
                        Log.Debug("Robot PGM Start Success.");
                        Step.OriginStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotLoad_No_Ready_Response);
                    break;
                case ERobotUnloadToOriginStep.SetModel:
                    Log.Debug("Set Model: " + _robotUnloadRecipe.Model);
                    _robotUnload.SendCommand(RobotHelpers.SetModel(_robotUnloadRecipe.Model));
                    Step.OriginStep++;
                    break;
                case ERobotUnloadToOriginStep.SetModel_Check:
                    if (_robotUnload.ReadResponse(5000, $"select,{_robotUnloadRecipe.Model},0\n\r"))
                    {
                        Log.Debug("Set Model: " + _robotUnloadRecipe.Model + " success");
                        Step.OriginStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotLoad_SetModel_Fail);
                    break;
                case ERobotUnloadToOriginStep.End:
                    if (ProcessStatus == EProcessStatus.ToOriginDone)
                    {
                        Thread.Sleep(10);
                        break;
                    }

                    Log.Debug("To Origin End");
                    ProcessStatus = EProcessStatus.ToOriginDone;
                    break;
            }
            return true;
        }

        public override bool ProcessOrigin()
        {
            switch ((ERobotUnloadOriginStep)Step.OriginStep)
            {
                case ERobotUnloadOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.Cylinder_Up:
                    Log.Debug("Cylinders Up");
                    CylinderContact(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylindersUp);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.Cylinder_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Cylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinders Up Done");
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.RobotHomePosition:
                    Log.Debug("Check Home Positon RobotUnload");
                    _robotUnload.SendCommand(RobotHelpers.HomePositionCheck);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.RobotHomePosition_Check:
                    if (_robotUnload.ReadResponse(5000, "Robot in home,0\r\n"))
                    {
                        Log.Debug("Robot In Home .");
                        Step.OriginStep = (int)ERobotUnloadOriginStep.End;
                        break;
                    }

                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.RobotSeqHome:
                    Log.Debug("Check Sequence Home RobotUnload");
                    _robotUnload.SendCommand(RobotHelpers.SeqHomeCheck);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.RobotSeqHome_Check:
                    if (_robotUnload.ReadResponse(5000, "Home safely,0\r\n"))
                    {
                        Log.Debug("Robot Unload Home Safely");
                        Step.OriginStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotUnload_Home_Manual_By_TeachingPendant);
                    break;
                case ERobotUnloadOriginStep.Robot_Origin:
                    Log.Debug("Start Origin Robot Unload");
                    Log.Debug($"Send Robot Motion Command {ERobotCommand.HOME}");
                    if (SendCommand(ERobotCommand.HOME, 10, 30))
                    {
                        Wait((int)(_commonRecipe.MotionOriginTimeout * 1000), () => _robotUnload.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.HOME)));
                        Step.OriginStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotUnload_SendMotionCommand_Fail);
                    break;
                case ERobotUnloadOriginStep.Robot_Origin_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotUnload_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Unload Origin done");
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.End:
                    Log.Debug("Origin End");
                    Step.OriginStep++;
                    ProcessStatus = EProcessStatus.OriginDone;
                    break;
                default:
                    Wait(10);
                    break;
            }
            return true;
        }

        public override bool ProcessRun()
        {
            switch (Sequence)
            {
                case ESequence.Stop:
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    IsWarning = false;
                    Sequence = ESequence.Stop;
                    break;
                case ESequence.InConveyorLoad:
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.CSTTilt:
                    break;
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.OutConveyorUnload:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    break;
                case ESequence.VinylClean:
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    break;
                case ESequence.RobotPlaceFixtureToAlign:
                    break;
                case ESequence.FixtureAlign:
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    break;
                case ESequence.TransferFixtureLoad:
                    break;
                case ESequence.Detach:
                    break;
                case ESequence.TransferFixtureUnload:
                    break;
                case ESequence.DetachUnload:
                    break;
                case ESequence.RemoveFilm:
                    break;
                case ESequence.RemoveFilmThrow:
                    break;
                case ESequence.GlassTransferPick:
                    break;
                case ESequence.GlassTransferPlace:
                    break;
                case ESequence.AlignGlassLeft:
                    break;
                case ESequence.AlignGlassRight:
                    break;
                case ESequence.TransferInShuttleLeftPick:
                    break;
                case ESequence.TransferInShuttleRightPick:
                    break;
                case ESequence.WETCleanLeftLoad:
                    break;
                case ESequence.WETCleanRightLoad:
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanRight:
                    break;
                case ESequence.WETCleanLeftUnload:
                    break;
                case ESequence.WETCleanRightUnload:
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.TransferRotationRight:
                    break;
                case ESequence.AFCleanLeftLoad:
                    break;
                case ESequence.AFCleanRightLoad:
                    break;
                case ESequence.AFCleanLeft:
                    break;
                case ESequence.AFCleanRight:
                    break;
                case ESequence.AFCleanLeftUnload:
                    break;
                case ESequence.AFCleanRightUnload:
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    break;
                case ESequence.UnloadTransferRightPlace:
                    break;
                case ESequence.UnloadAlignGlass:
                    break;
                case ESequence.UnloadRobotPick:
                    Sequence_UnloadRobotPick();
                    break;
                case ESequence.UnloadRobotPlasma:
                    Sequence_UnloadRobotPlasma();
                    break;
                case ESequence.UnloadRobotPlace:
                    Sequence_UnloadRobotPlace();
                    break;
            }

            return true;
        }
        #endregion

        #region Private Methods
        private void VacuumOnOff(bool bOnOff)
        {
            GlassVacOnOff1.Value = bOnOff;
            GlassVacOnOff2.Value = bOnOff;
            GlassVacOnOff3.Value = bOnOff;
            GlassVacOnOff4.Value = bOnOff;
#if SIMULATION
            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac1, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac2, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac3, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac4, bOnOff);

            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect1, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect2, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect3, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect4, bOnOff);
#endif
        }

        private void CylinderContact(bool bContact)
        {
            //Cylinder Down
            if (bContact)
            {
                Cyl1.Forward();
                Cyl2.Forward();
                Cyl3.Forward();
                Cyl4.Forward();
            }
            //Cylinder Up
            else
            {
                Cyl1.Backward();
                Cyl2.Backward();
                Cyl3.Backward();
                Cyl4.Backward();
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((ERobotUnloadAutoRunStep)Step.RunStep)
            {
                case ERobotUnloadAutoRunStep.Start:
                    Log.Debug("AutoRun Start");
                    Step.RunStep++;
                    break;
                case ERobotUnloadAutoRunStep.GlassVac_Check:
                    if ((GlassVac1 || GlassVac2 || GlassVac3 || GlassVac4) && _machineStatus.IsDryRunMode == false)
                    {
                        PlasmaPrepare();
                        Log.Info("Sequence Unload Robot Plasma");
                        Sequence = ESequence.UnloadRobotPlasma;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadAutoRunStep.End:
                    Log.Info("Unload Robot Pick");
                    Sequence = ESequence.UnloadRobotPick;
                    break;
            }
        }

        private void Sequence_UnloadRobotPick()
        {
            switch ((ERobotUnloadPickStep)Step.RunStep)
            {
                case ERobotUnloadPickStep.Start:
                    Log.Debug("Unload Robot Pick Start");
                    Step.RunStep++;
                    Log.Debug("Wait Unload Align Request Unload");
                    break;
                case ERobotUnloadPickStep.Robot_Move_ReadyPickPosition:
                    Log.Debug("Robot Move Ready Pick Position");
                    if (SendCommand(ERobotCommand.S1_RDY, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotUnload.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S1_RDY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotUnload_SendMotionCommand_Fail);
                    break;
                case ERobotUnloadPickStep.Robot_Move_ReadyPickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotUnload_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S1_RDY} Done");
                    Log.Debug("Wait Unload Align Request Unload");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Wait_UnloadAlignRequestUnload:
                    if (FlagUnloadAlignRequestUnload == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Robot_Move_PickPosition:
                    Log.Debug("Robot Move Pick Position");
                    if (SendCommand(ERobotCommand.S1_RDY_PP, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotUnload.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S1_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotUnload_SendMotionCommand_Fail);
                    break;
                case ERobotUnloadPickStep.Robot_Move_PickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotUnload_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S1_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Cylinder_Down:
                    Log.Debug("Cylinders Down");
                    CylinderContact(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylindersDown);
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Cylinder_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Cylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinders Down Done");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    VacuumOnOff(true);
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => (GlassVac1 && GlassVac2 && GlassVac3 && GlassVac4) || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Vacuum_On_Fail);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.GlassDetect_Check:
                    Log.Debug("Glass Detect Check");
                    if ((GlassDetect1 == false || GlassDetect2 == false || GlassDetect3 == false || GlassDetect4 == false) && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Pick_Fail);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Set_FlagRobotPickDone:
                    Log.Debug("Set Flag Robot Pick Done");
                    FlagRobotUnloadPickDone = true;
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Wait_UnloadAlign_PickDoneReceived:
                    if (FlagRobotUnloadDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Robot Pick Done");
                    FlagRobotUnloadPickDone = false;
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Robot_MoveBack_ReadyPickPosition:
                    Log.Debug("Robot Move Back Ready Pick Position");
                    if (SendCommand(ERobotCommand.S1_PP_RDY, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotUnload.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S1_PP_RDY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotUnload_SendMotionCommand_Fail);
                    break;
                case ERobotUnloadPickStep.Robot_MoveBack_ReadyPickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotUnload_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S1_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Plasma_Prepare:
                    PlasmaPrepare();
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.End:
                    Log.Debug("Unload Robot Pick End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Unload Robot Plasma");
                    Sequence = ESequence.UnloadRobotPlasma;
                    break;
            }
        }

        private void PlasmaPrepare()
        {
            int plasmaPrepareStep = 0;

            Task plasmaPrepareTask = Task.Run(async () =>
            {
                bool running = true;

                while (running)
                {
                    switch ((EPlasmaPrepareStep)plasmaPrepareStep)
                    {
                        case EPlasmaPrepareStep.Start:
                            Log.Debug("Plasma Prepare Start");
                            plasmaPrepareStep++;
                            break;

                        case EPlasmaPrepareStep.Air_Valve_Open:
                            Log.Debug("Plasma Air Valve Open");
                            _plasma.AirOpenClose(true);
                            await Task.Delay(500);
                            plasmaPrepareStep++;
                            break;

                        case EPlasmaPrepareStep.Plasma_On:
                            Log.Debug("Plasma On");
                            _plasma.PlasmaOnOff(true);
                            await Task.Delay(500);
                            plasmaPrepareStep++;
                            break;

                        case EPlasmaPrepareStep.End:
                            Log.Debug("Plasma Prepare End");
                            IsPlasmaPrepare = true;
                            running = false;
                            break;
                    }
                }
            });
        }

        private void Sequence_UnloadRobotPlasma()
        {
            switch ((ERobotUnloadPlasmaStep)Step.RunStep)
            {
                case ERobotUnloadPlasmaStep.Start:
                    Log.Debug("Unload Robot Plasma Start");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.Wait_PlasmaPrepareDone:
                    if (IsPlasmaPrepare == false)
                    {
                        Wait(20);
                        break;
                    }

                    IsPlasmaPrepare = false;
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.Robot_Move_PlasmaPosition:
                    Log.Debug("Robot Move Plasma Position");
                    if (SendCommand(ERobotCommand.S2_RDY_PP, PlasmaSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotUnload.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S2_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotUnload_SendMotionCommand_Fail);
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.Robot_Move_PlasmaPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotUnload_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S2_RDY_PP} Done");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.End:
                    Log.Debug("Plasma End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Unload Robot Place");
                    Sequence = ESequence.UnloadRobotPlace;
                    break;
            }
        }

        private void Sequence_UnloadRobotPlace()
        {
            switch ((EUnloadRobotPlaceStep)Step.RunStep)
            {
                case EUnloadRobotPlaceStep.Start:
                    Log.Debug("Unload Robot Place Start");
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.CheckOutputStopValue:
                    if (_machineStatus.IsOutputStop == true)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.Wait_MachineRequestPlace:
                    if (FlagMachineRequestPlace == false && !_machineStatus.IsDryRunMode)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.Robot_Move_PlacePosition:
                    Log.Debug("Robot Move Place Position");
                    if (SendCommand(ERobotCommand.S3_RDY_PP, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotUnload.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S3_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseAlarm((int)EAlarm.RobotUnload_SendMotionCommand_Fail);
                    break;
                case EUnloadRobotPlaceStep.Robot_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotUnload_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S3_RDY_PP} Done");
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.VacuumOff:
                    Log.Debug("Vacuum Off");
                    VacuumOnOff(false);
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.Robot_Move_ReadyPlacePosition:
                    Log.Debug("Robot MoveBack Ready Place Position");
                    if (SendCommand(ERobotCommand.S3_PP_RDY, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotUnload.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S3_PP_RDY)));
                        Step.RunStep++;
                        break;
                    }
                    RaiseAlarm((int)EAlarm.RobotUnload_SendMotionCommand_Fail);
                    break;
                case EUnloadRobotPlaceStep.Robot_Move_ReadyPlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotUnload_MoveMotionCommand_Timeout);
                        break;
                    }
                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S3_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.End:
                    Log.Debug("Unload Robot Place End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    _workData.TaktTime.SetTaktTime();
                    Log.Info("Sequence Unload Robot Pick");
                    Sequence = ESequence.UnloadRobotPick;
                    break;
            }
        }
        #endregion
    }
}
