using EQX.Core.InOut;
using EQX.Core.Robot;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using PIFilmAutoDetachCleanMC.Defines.Devices.Robot;
using PIFilmAutoDetachCleanMC.Defines.Process;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;
using PIFilmAutoDetachCleanMC.Recipe;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class RobotLoadProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly IRobot _robotLoad;
        private readonly CommonRecipe _commonRecipe;
        private readonly RobotLoadRecipe _robotLoadRecipe;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly Devices _devices;
        private readonly IDInputDevice _robotLoadInput;
        private readonly IDOutputDevice _robotLoadOutput;
        private readonly IDOutputDevice _removeFilmOutput;
        private readonly CassetteList _cassetteList;
        private readonly MachineStatus _machineStatus;
        private readonly CWorkData _workData;
        private int CurrentInWorkCSTFixtureIndex = -1;
        private int CurrentOutWorkCSTFixtureIndex = -1;
        private string[] paras = new string[8] { "0", "0", "0", "0", "0", "0", "0", "0" };

        private int HightSpeed => _robotLoadRecipe.RobotSpeedHigh;
        private int LowSpeed => _robotLoadRecipe.RobotSpeedLow;

        private ICylinder ClampCyl1 => _devices.Cylinders.RobotLoad_ClampCyl1;
        private ICylinder ClampCyl2 => _devices.Cylinders.RobotLoad_ClampCyl2;
        private ICylinder AlignCyl => _devices.Cylinders.RobotLoad_AlignCyl;

        private bool IsFixtureDetect => !ClampCyl1.IsBackward && !ClampCyl1.IsForward && !ClampCyl2.IsBackward && !ClampCyl2.IsForward;

        private bool SendCommand(ERobotCommand command, int lowSpeed, int highSpeed, string[] paras = null)
        {
            if (paras == null || paras.Length == 0)
            {
                _robotLoad.SendCommand(RobotHelpers.MotionCommands(command, lowSpeed, highSpeed));
            }
            else
            {
                _robotLoad.SendCommand(RobotHelpers.MotionCommands(command, lowSpeed, highSpeed, paras));
            }

            Wait(5000, () => _robotLoad.ReadResponse(RobotHelpers.MotionRspStart(command)));
            return !WaitTimeOutOccurred;
        }

        #endregion

        #region Inputs
        private IDInput PeriRDY => _devices.Inputs.LoadRobPeriRdy;
        private IDInput StopMess => _devices.Inputs.LoadRobStopmess;
        private IDInput ProACT => _devices.Inputs.LoadRobProAct;
        #endregion

        #region Outputs
        private IDOutput DrivesOn => _devices.Outputs.LoadRobDrivesOn;
        private IDOutput ConfMess => _devices.Outputs.LoadRobConfMess;
        private IDOutput ExtStart => _devices.Outputs.LoadRobExtStart;
        #endregion

        #region Flags
        private bool FlagRobotOriginDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.ROBOT_ORIGIN_DONE] = value;
            }
        }
        private bool FlagVinylCleanRequestLoad
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_LOAD];
            }
        }

        private bool FlagVinylCleanRequestUnload
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_UNLOAD];
            }
        }

        private bool FlagRemoveFilmRequestUnload
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.REMOVE_FILM_REQ_UNLOAD];
            }
        }

        private bool FlagInCSTReady
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.IN_CST_READY];
            }
        }

        private bool FlagInCSTPickDoneReceived
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.IN_CST_PICK_DONE_RECEIVED];
            }
        }

        private bool FlagVinylCleanLoadDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.VINYL_CLEAN_LOAD_DONE] = value;
            }
        }

        private bool FlagVinylCleanReceiveLoadDone
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_LOAD_DONE];
            }
        }

        private bool FlagVinylCleanUnloadDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.VINYL_CLEAN_UNLOAD_DONE] = value;
            }
        }

        private bool FlagVinylCleanReceiveUnloadDone
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE];
            }
        }

        private bool FlagFixtureAlignRequestLoad
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.FIXTURE_ALIGN_REQ_LOAD];
            }
        }

        private bool FlagFixtureAlignLoadDoneReceived
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.FIXTURE_ALIGN_LOAD_DONE_RECEIVED];
            }
        }
        private bool FlagFixtureAlignLoadDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.FIXTURE_ALIGN_LOAD_DONE] = value;
            }
        }

        private bool FlagRemoveFilmUnloadDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.REMOVE_FILM_UNLOAD_DONE] = value;
            }
        }

        private bool FlagOutCSTReady
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.OUT_CST_READY];
            }
        }

        private bool FlagOutCSTPlaceDoneReceived
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.OUT_CST_PLACE_DONE_RECEIVED];
            }
        }

        private bool FlagRobotPlaceOutCSTDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.ROBOT_PLACE_OUT_CST_DONE] = value;
            }
        }

        private bool FlagPickFromInCSTDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.ROBOT_PICK_IN_CST_DONE] = value;
            }
        }
        #endregion

        #region Constructor
        public RobotLoadProcess([FromKeyedServices("RobotLoad")] IRobot robotLoad,
            CommonRecipe commonRecipe,
            RobotLoadRecipe robotLoadRecipe,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            Devices devices,
            MachineStatus machineStatus,
            [FromKeyedServices("RobotLoadInput")] IDInputDevice robotLoadInput,
            [FromKeyedServices("RobotLoadOutput")] IDOutputDevice robotLoadOutput,
            [FromKeyedServices("RemoveFilmOutput")] IDOutputDevice removeFilmOutput,
            CassetteList cassetteList,
            CWorkData workData)
        {
            _robotLoad = robotLoad;
            _commonRecipe = commonRecipe;
            _robotLoadRecipe = robotLoadRecipe;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _devices = devices;
            _machineStatus = machineStatus;
            _robotLoadInput = robotLoadInput;
            _robotLoadOutput = robotLoadOutput;
            _removeFilmOutput = removeFilmOutput;
            _cassetteList = cassetteList;
            _workData = workData;
        }
        #endregion

        #region Override Methods
        public override bool ProcessToOrigin()
        {
            switch ((ERobotLoadToOriginStep)Step.OriginStep)
            {
                case ERobotLoadToOriginStep.Start:
                    Log.Debug("To Origin Start");
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.Clear_FlagOriginDone:
                    Log.Debug("Clear Flag Robot Origin Done");
                    FlagRobotOriginDone = false;
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.ReConnectIfRequired:
                    if (!_robotLoad.IsConnected)
                    {
                        Log.Debug("Robot is not connected, trying to reconnect.");
                        _robotLoad.Connect();
                    }

                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.CheckConnection:
                    if (!_robotLoad.IsConnected)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Connect_Fail);
                        break;
                    }

                    Log.Debug("Robot is connected.");
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.EnableOutput:
                    Log.Debug("Enable Outputs Move Enable, Driver Off");
                    _devices.Outputs.LoadRobMoveEnable.Value = true;
                    _devices.Outputs.LoadRobDrivesOff.Value = true;
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotMotionOnCheck:
                    if (!PeriRDY.Value)
                    {
                        Log.Debug("Driver ON");
                        DrivesOn.Value = true;
                        Step.OriginStep++;
                        break;
                    }

                    Step.OriginStep = (int)ERobotLoadToOriginStep.RobotStopMessCheck;
                    break;
                case ERobotLoadToOriginStep.RobotMotionOn_Delay:
                    Wait(3000, () => PeriRDY.Value);
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotMotionOn_Disable:
                    Log.Debug("Driver OFF");
                    DrivesOn.Value = false;
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotStopMessCheck:
                    if (StopMess.Value)
                    {
                        Log.Debug("Confirm message.");
                        ConfMess.Value = true;
                        Step.OriginStep++;
                        break;
                    }

                    Step.OriginStep = (int)ERobotLoadToOriginStep.RobotExtStart_Enable;
                    break;
                case ERobotLoadToOriginStep.RobotConfMess_Delay:
                    Wait(500);
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotConfMess_Disable:
                    Log.Debug("Disable Confirm message.");
                    ConfMess.Value = false;
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotExtStart_Enable:
                    Log.Debug("Enable External Start.");
                    ExtStart.Value = true;
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotExtStart_Delay:
                    Wait(500);
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotExtStart_Disable:
                    Log.Debug("Disable External Start.");
                    ExtStart.Value = false;
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.RobotProgramStart_Check:
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
                case ERobotLoadToOriginStep.SendPGMStart:
                    _robotLoad.SendCommand(RobotHelpers.PCPGMStart);
                    Wait(5000, () => _robotLoad.ReadResponse("RobotReady,0\r\n"));
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.SendPGMStart_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_No_Ready_Response);
                        break;
                    }

                    Log.Debug("Robot PGM Start Success.");
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.SetModel:
                    Log.Debug("Set Model: " + _robotLoadRecipe.Model);
                    _robotLoad.SendCommand(RobotHelpers.SetModel(_robotLoadRecipe.Model));

                    Wait(5000, () => _robotLoad.ReadResponse($"select,{_robotLoadRecipe.Model},0\r\n"));

                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.SetModel_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_SetModel_Fail);
                        break;
                    }

                    Log.Debug("Set Model: " + _robotLoadRecipe.Model + " success");
                    Step.OriginStep++;
                    break;
                case ERobotLoadToOriginStep.End:
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
            switch ((ERobotLoadOriginStep)Step.OriginStep)
            {
                case ERobotLoadOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Fixture_Detect_Check:
                    if (IsFixtureDetect)
                    {
                        RaiseWarning((int)EWarning.RobotLoadOriginFixtureDetect);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Cyl_Unclamp:
                    Log.Debug("Cylinders Unclamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Cyl_Unclamp_Wait:
                    if (WaitTimeOutOccurred)
                    {

                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                        break;
                    }

                    Log.Debug("Cylinders Unclamp Done");
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Cyl_UnAlign:
                    Log.Debug("Cylinders Unalign");
                    AlignCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Cyl_Unalign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                        break;
                    }

                    Log.Debug("Cylinders Unalign Done");
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.RobotHomePosition:
                    Log.Debug("Check Home Positon RobotLoad");
                    _robotLoad.SendCommand(RobotHelpers.HomePositionCheck);

                    Wait(5000, () => _robotLoad.ReadResponse("Robot in home,0\r\n"));

                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.RobotHomePosition_Check:
                    if (WaitTimeOutOccurred)
                    {
                        Log.Debug("Robot load is not in home position");
                        Step.OriginStep++;
                        break;
                    }

                    Log.Debug("Robot In Home .");
                    Step.OriginStep = (int)ERobotLoadOriginStep.End;
                    break;
                case ERobotLoadOriginStep.RobotSeqHome:
                    Log.Debug("Check sequence home robot load");
                    _robotLoad.SendCommand(RobotHelpers.SeqHomeCheck);

                    Wait(5000, () => _robotLoad.ReadResponse("Home safety,0\r\n"));

                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.RobotSeqHome_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Home_Manual_By_TeachingPendant);
                        break;
                    }

                    Log.Debug("Robot load can origin safely");
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Robot_Origin:
                    Log.Debug("Start Origin Robot Load");
                    Log.Debug($"Send Robot Motion Command {ERobotCommand.HOME}");
                    if (SendCommand(ERobotCommand.HOME, 10, 30))
                    {
                        Wait((int)(_commonRecipe.MotionOriginTimeout * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.HOME)));
                        Step.OriginStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadOriginStep.Robot_Origin_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug("Robot Origin Done");
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.End:
                    Log.Debug("Set Flag Robot Origin Done");
                    FlagRobotOriginDone = true;
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
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
                    Sequence_Ready();
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.InWorkCSTTilt:
                    break;
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.OutWorkCSTTilt:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    Sequence_RobotPickFixtureFromCST();
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    Sequence_RobotPickPlaceVinylClean(false);
                    break;
                case ESequence.VinylClean:
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    Sequence_RobotPickPlaceVinylClean(true);
                    break;
                case ESequence.RobotPlaceFixtureToAlign:
                    Sequence_RobotPlaceFixtureToAlign();
                    break;
                case ESequence.FixtureAlign:
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    Sequence_RobotPickFixtureFromRemoveZone();
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    Sequence_RobotPlaceFixtureToOutCST();
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
                case ESequence.GlassTransferPick:
                    break;
                case ESequence.GlassTransferPlace:
                    break;
                case ESequence.AlignGlassLeft:
                    break;
                case ESequence.TransferInShuttleLeftPick:
                    break;
                case ESequence.WETCleanLeftLoad:
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanLeftUnload:
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.AFCleanLeftLoad:
                    break;
                case ESequence.AFCleanLeft:
                    break;
                case ESequence.AFCleanLeftUnload:
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    break;
                case ESequence.UnloadAlignGlass:
                    break;
                case ESequence.UnloadRobotPick:
                    break;
                case ESequence.UnloadRobotPlasma:
                    break;
                case ESequence.UnloadRobotPlace:
                    break;
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((ERobotLoadProcessToRunStep)Step.ToRunStep)
            {
                case ERobotLoadProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case ERobotLoadProcessToRunStep.Send_CassettePitch:
                    Log.Debug("Send Cassette Pitch: " + _cstLoadUnloadRecipe.Pitch);
                    _robotLoad.SendCommand(RobotHelpers.SetCassettePitch(0, _cstLoadUnloadRecipe.Pitch));

                    Wait(5000, () => _robotLoad.ReadResponse($"pitch,0,{_cstLoadUnloadRecipe.Pitch},0\r\n"));

                    Step.ToRunStep++;
                    break;
                case ERobotLoadProcessToRunStep.Send_CassettePitch_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_SetCassettePitch_Fail);
                        break;
                    }

                    Log.Debug("Set pitch X: 0, Y: " + _cstLoadUnloadRecipe.Pitch + " success");
                    Step.ToRunStep++;
                    break;
                case ERobotLoadProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<ERobotLoadProcessOutput>)_robotLoadOutput).Clear();
                    Step.ToRunStep++;
                    break;
                case ERobotLoadProcessToRunStep.End:
                    Log.Debug("To Run End");
                    Step.ToRunStep++;
                    ProcessStatus = EProcessStatus.ToRunDone;
                    break;
                default:
                    Thread.Sleep(20);
                    break;
            }
            return true;
        }
        #endregion

        #region Private Methods
        private void Sequence_AutoRun()
        {
            switch ((ERobotLoadAutoRunStep)Step.RunStep)
            {
                case ERobotLoadAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    _workData.TaktTime.TaktTimeCounter = Environment.TickCount;
                    if (_machineStatus.IsDryRunMode)
                    {
                        Log.Info("Sequence Robot Pick Fixture from CST");
                        Sequence = ESequence.RobotPickFixtureFromCST;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadAutoRunStep.Check_Flag_VinylCleanRequestFixture:
                    if (FlagVinylCleanRequestLoad && FlagInCSTReady)
                    {
                        Log.Debug("Clear Flag Vinyl Clean Load Done");
                        FlagVinylCleanLoadDone = false;

                        Log.Info("Sequence Robot Pick Fixture From CST");
                        Sequence = ESequence.RobotPickFixtureFromCST;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadAutoRunStep.Check_Flag_RemoveFilm:
                    if (_devices.Inputs.RemoveZoneFixtureDetect.Value)
                    {
                        if (FlagRemoveFilmRequestUnload)
                        {
                            _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD] = false;
                            Log.Debug("Clear Flag Remove Film Unload Done");
                            FlagRemoveFilmUnloadDone = false;

                            Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                            Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                            break;
                        }
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadAutoRunStep.Check_Flag_VinylCleanRequestUnload:
                    if (FlagVinylCleanRequestUnload)
                    {
                        Log.Debug("Clear Flag Vinyl Clean Unload Done");
                        FlagVinylCleanUnloadDone = false;

                        Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                        Sequence = ESequence.RobotPickFixtureFromVinylClean;
                        break;
                    }
                    Step.RunStep = (int)ERobotLoadAutoRunStep.Check_Flag_VinylCleanRequestFixture;
                    break;
                case ERobotLoadAutoRunStep.End:
                    break;
            }
        }

        private void Sequence_Ready()
        {
            switch ((ERobotLoadReadyStep)Step.RunStep)
            {
                case ERobotLoadReadyStep.Start:
                    if (IsOriginOrInitSelected == false)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Debug("Ready Start");
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.ReConnectIfRequired:
                    if (!_robotLoad.IsConnected)
                    {
                        Log.Debug("Robot is not connected, trying to reconnect.");
                        _robotLoad.Connect();
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.CheckConnection:
                    if (!_robotLoad.IsConnected)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Connect_Fail);
                        break;
                    }

                    Log.Debug("Robot is connected.");
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.EnableOutput:
                    Log.Debug("Enable Outputs Move Enable, Driver Off");
                    _devices.Outputs.LoadRobMoveEnable.Value = true;
                    _devices.Outputs.LoadRobDrivesOff.Value = true;
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotMotionOnCheck:
                    if (!PeriRDY.Value)
                    {
                        Log.Debug("Driver ON");
                        DrivesOn.Value = true;
                        Step.RunStep++;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoadReadyStep.RobotStopMessCheck;
                    break;
                case ERobotLoadReadyStep.RobotMotionOn_Delay:
                    Wait(3000, () => PeriRDY.Value);
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotMotionOn_Disable:
                    Log.Debug("Driver OFF");
                    DrivesOn.Value = false;
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotStopMessCheck:
                    if (StopMess.Value)
                    {
                        Log.Debug("Confirm message.");
                        ConfMess.Value = true;
                        Step.RunStep++;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoadReadyStep.RobotExtStart_Enable;
                    break;
                case ERobotLoadReadyStep.RobotConfMess_Delay:
                    Wait(500);
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotConfMess_Disable:
                    Log.Debug("Disable Confirm message.");
                    ConfMess.Value = false;
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotExtStart_Enable:
                    Log.Debug("Enable External Start.");
                    ExtStart.Value = true;
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotExtStart_Delay:
                    Wait(500);
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotExtStart_Disable:
                    Log.Debug("Disable External Start.");
                    ExtStart.Value = false;
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotProgramStart_Check:
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
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.SendPGMStart:
                    _robotLoad.SendCommand(RobotHelpers.PCPGMStart);

                    Wait(5000, () => _robotLoad.ReadResponse("RobotReady,0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.SendPGMStart_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_No_Ready_Response);
                        break;
                    }

                    Log.Debug("Robot PGM Start Success.");
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.SetModel:
                    Log.Debug("Set Model: " + _robotLoadRecipe.Model);
                    _robotLoad.SendCommand(RobotHelpers.SetModel(_robotLoadRecipe.Model));

                    Wait(5000, () => _robotLoad.ReadResponse($"select,{_robotLoadRecipe.Model},0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.SetModel_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_SetModel_Fail);
                        break;
                    }

                    Log.Debug("Set Model: " + _robotLoadRecipe.Model + " success");
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotHomePosition:
                    Log.Debug("Check Home Positon RobotLoad");
                    _robotLoad.SendCommand(RobotHelpers.HomePositionCheck);

                    Wait(5000, () => _robotLoad.ReadResponse("Robot in home,0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotHomePosition_Check:
                    if (WaitTimeOutOccurred)
                    {
                        Step.RunStep++;
                        break;
                    }

                    Log.Debug("Robot In Home .");
                    Step.RunStep = (int)ERobotLoadReadyStep.End;
                    break;
                case ERobotLoadReadyStep.RobotSeqHome:
                    Log.Debug("Check sequence home robot load");
                    _robotLoad.SendCommand(RobotHelpers.SeqHomeCheck);

                    Wait(5000, () => _robotLoad.ReadResponse("Home safety,0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotSeqHome_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Home_Manual_By_TeachingPendant);
                        break;
                    }

                    Log.Debug("Robot Load Home Safely");
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.RobotHome:
                    Log.Debug("Start Home Robot Load");
                    Log.Debug($"Send Robot Motion Command {ERobotCommand.HOME}");
                    if (SendCommand(ERobotCommand.HOME, 10, 20))
                    {
                        Wait((int)(_commonRecipe.MotionOriginTimeout * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.HOME)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadReadyStep.RobotHome_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }
                    Log.Debug("Robot Home Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadReadyStep.End:
                    Log.Debug("Ready run end");
                    IsWarning = false;
                    Sequence = ESequence.Stop;
                    break;
                default:
                    Wait(20);
                    break;
            }
        }

        private void Sequence_RobotPickFixtureFromCST()
        {
            switch ((ERobotLoadPickFixtureFromCSTStep)Step.RunStep)
            {
                case ERobotLoadPickFixtureFromCSTStep.Start:
                    Log.Debug("Robot Pick Fixture From CST Start");
                    Step.RunStep++;
                    Log.Debug("Wait In Cassette Ready");
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    Log.Debug("Cylinder Unalign");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    AlignCyl.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => ClampCyl1.IsBackward && ClampCyl2.IsBackward && AlignCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_UnClamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        if(ClampCyl1.IsBackward == false || ClampCyl2.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                            break;
                        }

                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Wait_InCST_Ready:
                    if (FlagInCSTReady == false)
                    {
                        Wait(20);
                        break;
                    }

                    CurrentInWorkCSTFixtureIndex = _cassetteList.CassetteIn.GetFirstIndex(ETrayCellStatus.Working);
                    Log.Debug("Current Fixture Index: " + CurrentInWorkCSTFixtureIndex);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Index_Initiation:
                    //TODO: Index Initiation by sensor 
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_PickPositon:
                    paras = new string[]
                    {
                        CurrentInWorkCSTFixtureIndex.ToString(),
                        "1",
                        "0",
                        "0",
                        "0",
                        "0",
                        "0",
                        "0"
                    };

                    Log.Debug("Move In Cassette Pick Position");
                    Log.Debug($"Send Robot Motion Command {ERobotCommand.S1_RDY_PP}");
                    if (SendCommand(ERobotCommand.S1_RDY_PP, LowSpeed, HightSpeed, paras))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S1_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_PickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }
                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S1_RDY_PP} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Clamp:
                    Log.Debug("Cylinder Clamp");
                    ClampCyl1.Forward();
                    ClampCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsForward && ClampCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Align:
                    Log.Debug("Cylinder Align");
                    AlignCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Align_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Align Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_ReadyPositon:
                    Log.Debug("Move In Cassette Ready Position");
                    if (SendCommand(ERobotCommand.S1_PP_RDY, LowSpeed, HightSpeed, paras))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S1_PP_RDY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_ReadyPositon_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S1_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Update_Cassette_Status:
                    Log.Debug("Update Cassette Status");
                    _cassetteList.CassetteIn[(uint)CurrentInWorkCSTFixtureIndex] = ETrayCellStatus.Done;
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Set_Flag_RobotPickInCSTDone:
                    Log.Debug("Set Flag Robot Pick In CST Done");
                    FlagPickFromInCSTDone = true;
                    Log.Debug("Wait In CST Pick Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Wait_InCST_PickDone:
                    if (FlagInCSTPickDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Robot Pick In CST Done");
                    FlagPickFromInCSTDone = false;
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.End:
                    Log.Debug("Robot Pick Fixture From CST End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Sequence Place Fixture To Vinyl Clean");
                    Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                    break;
            }
        }

        private void Sequence_RobotPickPlaceVinylClean(bool bPick)
        {
            switch ((ERobotLoadPickPlaceFixtureVinylCleanStep)Step.RunStep)
            {
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Start:
                    Log.Debug("Robot" + (bPick ? " Pick" : " Place") + " Fixture Vinyl Clean Start");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Move_VinylClean_PickPlacePosition:
                    Log.Debug("Move Vinyl Clean Pick Place Position");
                    if (SendCommand(ERobotCommand.S2_RDY_PP, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S2_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Move_VinylClean_PickPlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }
                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S2_RDY_PP} Done");
                    if (bPick)
                    {
                        Step.RunStep++;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoadPickPlaceFixtureVinylCleanStep.CylUnClamp;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylAlign:
                    Log.Debug("Cylinder Align start");
                    AlignCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_Align_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Align Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylClamp:
                    Log.Debug("Cylinder Contact");
                    ClampCyl1.Forward();
                    ClampCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixtureDetect);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Clamp Done");
                    Step.RunStep = (int)ERobotLoadPickPlaceFixtureVinylCleanStep.Move_VinylClean_ReadyPosition;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylUnClamp:
                    Log.Debug("Cylinder UnContact");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylUnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylUnAlign:
                    Log.Debug("Cylinder UnAlign");
                    AlignCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylUnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Move_VinylClean_ReadyPosition:
                    Log.Debug("Move Vinyl Clean Ready Position");
                    if (SendCommand(ERobotCommand.S2_PP_RDY, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S2_PP_RDY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Move_VinylClean_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }
                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S2_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.SetFlag_VinylCleanLoadUnloadDone:
                    if (bPick)
                    {
                        Log.Debug("Set Flag Vinyl Clean Unload Done");
                        FlagVinylCleanUnloadDone = true;
                        Log.Debug("Wait Vinyl Clean Receive Unload Done");
                        Step.RunStep++;
                        break;
                    }
                    Log.Debug("Set Flag Vinyl Clean Load Done");
                    FlagVinylCleanLoadDone = true;
                    Log.Debug("Wait Vinyl Clean Receive Load Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Wait_VinylCleanReceiveLoadUnloadDone:
                    if (bPick)
                    {
                        if (FlagVinylCleanReceiveUnloadDone == false)
                        {
                            Wait(20);
                            break;
                        }
                        Log.Debug("Clear Flag Vinyl Clean Unload Done");
                        FlagVinylCleanUnloadDone = false;
                        Step.RunStep++;
                        break;
                    }
                    if (FlagVinylCleanReceiveLoadDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Vinyl Clean Load Done");
                    FlagVinylCleanLoadDone = false;
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.End:
                    Log.Debug("Robot Place Fixture To Vinyl Clean Done");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Wait_NextSequence:
                    if (bPick)
                    {
                        Log.Info("Sequence Place Fixture To Align");
                        Sequence = ESequence.RobotPlaceFixtureToAlign;
                        break;
                    }
                    else
                    {
                        if (_devices.Inputs.RemoveZoneFixtureDetect.Value)
                        {
                            if (FlagRemoveFilmRequestUnload)
                            {
                                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD] = false;
                                Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                                Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                                break;
                            }
                            break;
                        }
                        if (FlagVinylCleanRequestUnload)
                        {
                            Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                            Sequence = ESequence.RobotPickFixtureFromVinylClean;
                            break;
                        }

                        break;
                    }
            }
        }

        private void Sequence_RobotPlaceFixtureToAlign()
        {
            switch ((ERobotLoadPlaceFixtureToAlignStep)Step.RunStep)
            {
                case ERobotLoadPlaceFixtureToAlignStep.Start:
                    Log.Debug("Place Fixture To Align Start");
                    Step.RunStep++;
                    Log.Debug("Wait Fixture Align Request Load");
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Wait_FixtureAlignRequestFixture:
                    if (FlagFixtureAlignRequestLoad == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Move_FixtureAlignPlacePosition:
                    Log.Debug("Robot Move To Fixture Align Place Position");
                    if (SendCommand(ERobotCommand.S3_RDY_PP, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S3_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Move_FixtureAlignPlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S3_RDY_PP} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.UnContact:
                    Log.Debug("UnContact");
                    AlignCyl.Backward();
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsBackward && ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.UnContact_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (AlignCyl.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_Cylinder_Align_Fail);
                            break;
                        }
                        if (ClampCyl1.IsBackward == false || ClampCyl2.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                            break;
                        }
                        break;
                    }
                    Log.Debug("UnContact Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Move_FixtureAlignReadyPosition:
                    Log.Debug("Robot Move To Fixture Align Ready Position");
                    if (SendCommand(ERobotCommand.S3_PP_RDY, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S3_PP_RDY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Move_FixtureAlignReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S3_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Set_FlagFixtureAlignLoadDone:
                    Log.Debug("Set Flag Fixture Align Load Done");
                    FlagFixtureAlignLoadDone = true;
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Wait_FixtureAlignLoadDoneReceived:
                    if (FlagFixtureAlignLoadDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Fixture Align Load Done");
                    FlagFixtureAlignLoadDone = false;
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.End:
                    Log.Debug("Robot Place Fixture To Align Done");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.Wait_NextSequence:
                    if (FlagVinylCleanRequestLoad && FlagInCSTReady)
                    {
                        Log.Debug("Clear Flag Vinyl Clean Load Done");
                        FlagVinylCleanLoadDone = false;

                        Log.Info("Sequence Robot Pick Fixture From CST");
                        Sequence = ESequence.RobotPickFixtureFromCST;
                        break;
                    }
                    if (_devices.Inputs.RemoveZoneFixtureDetect.Value)
                    {
                        if (FlagRemoveFilmRequestUnload)
                        {
                            _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD] = false;
                            Log.Debug("Clear Flag Remove Film Unload Done");
                            FlagRemoveFilmUnloadDone = false;

                            Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                            Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                            break;
                        }
                        break;
                    }
                    if (FlagVinylCleanRequestUnload)
                    {
                        Log.Debug("Clear Flag Vinyl Clean Unload Done");
                        FlagVinylCleanUnloadDone = false;

                        Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                        Sequence = ESequence.RobotPickFixtureFromVinylClean;
                        break;
                    }
                    break;
            }
        }

        private void Sequence_RobotPickFixtureFromRemoveZone()
        {
            switch ((ERobotLoadPickFixtureFromRemoveZoneStep)Step.RunStep)
            {
                case ERobotLoadPickFixtureFromRemoveZoneStep.Start:
                    Log.Debug("Robot Pick Fixture From Remove Zone Start");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Move_RemoveZonePickPosition:
                    Log.Debug("Robot Move Remove Zone Pick Position");
                    if (SendCommand(ERobotCommand.S4_RDY_PP, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S4_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Move_RemoveZonePickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S4_RDY_PP} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Contact:
                    Log.Debug("Contact");
                    AlignCyl.Forward();
                    ClampCyl1.Forward();
                    ClampCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsForward && ClampCyl1.IsForward && ClampCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Contact_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (AlignCyl.IsForward == false)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                            break;
                        }
                        if (ClampCyl1.IsForward == false || ClampCyl2.IsForward == false)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_Cylinder_Clamp_Fail);
                            break;
                        }
                        break;
                    }
                    Log.Debug("Contact Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Move_RemoveZoneReadyPosition:
                    Log.Debug("Robot Move Remove Zone Ready Position");
                    if (SendCommand(ERobotCommand.S4_PP_RDY, LowSpeed, HightSpeed))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S4_PP_RDY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Move_RemoveZoneReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;

                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S4_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Set_FlagRemoveZoneUnloadDone:
                    Log.Debug("Set Flag Remove Zone Unload Done");
                    FlagRemoveFilmUnloadDone = true;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.RemoveZoneFixtureDetect, false);
#endif
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.End:
                    Log.Debug("Robot Pick Fixture From Remove Zone Done");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Place To Out Cassette");
                    Sequence = ESequence.RobotPlaceFixtureToOutWorkCST;
                    break;
            }
        }

        private void Sequence_RobotPlaceFixtureToOutCST()
        {
            switch ((ERobotLoadPlaceFixtureToOutCSTStep)Step.RunStep)
            {
                case ERobotLoadPlaceFixtureToOutCSTStep.Start:
                    Log.Debug("Robot Place Fixture To Out Cassette Start");
                    Step.RunStep++;
                    Log.Debug("Wait Out Cassette Ready");
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Wait_OutCSTReady:
                    if (FlagOutCSTReady == false)
                    {
                        Wait(20);
                        break;
                    }
                    CurrentOutWorkCSTFixtureIndex = _cassetteList.CassetteOut.GetFirstIndex(ETrayCellStatus.Working);
                    Log.Debug($"Current Fixture Index : {CurrentOutWorkCSTFixtureIndex}");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Move_OutCSTPlacePosition:
                    paras = new string[]
                    {
                        CurrentInWorkCSTFixtureIndex.ToString(),
                        "1",
                        "0",
                        "0",
                        "0",
                        "0",
                        "0",
                        "0"
                    };

                    Log.Debug("Robot Move Out Cassette Place Position");
                    if (SendCommand(ERobotCommand.S5_RDY_PP, LowSpeed, HightSpeed, paras))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S5_RDY_PP)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Move_OutCSTPlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S5_RDY_PP} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.UnContact:
                    Log.Debug("UnContact");
                    AlignCyl.Backward();
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsBackward && ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.UnContact_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (AlignCyl.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_Cylinder_Align_Fail);
                            break;
                        }
                        if (ClampCyl1.IsBackward == false || ClampCyl2.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                            break;
                        }
                        break;
                    }
                    Log.Debug("UnContact Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Move_OutCSTReadyPosition:
                    Log.Debug("Robot Move Out Cassette Ready Position");
                    if (SendCommand(ERobotCommand.S5_PP_RDY, LowSpeed, HightSpeed, paras))
                    {
                        Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.S5_PP_RDY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Move_OutCSTReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug($"Robot Move Motion Command {ERobotCommand.S5_PP_RDY} Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Update_Cassette_Status:
                    Log.Debug("Update Cassette Status");
                    _cassetteList.CassetteOut[(uint)CurrentOutWorkCSTFixtureIndex] = ETrayCellStatus.Done;
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Set_FlagPlaceOutCSTDone:
                    Log.Debug("Set Flag Place Out Cassette Done");
                    FlagRobotPlaceOutCSTDone = true;
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Wait_OutCSTPlaceDoneReceived:
                    if (FlagOutCSTPlaceDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Robot Place Out CST Done");
                    FlagRobotPlaceOutCSTDone = false;
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.End:
                    Log.Debug("Robot Place Fixture To Out CST End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.Wait_NextSequence:
                    if (FlagVinylCleanRequestUnload)
                    {
                        Log.Debug("Clear Flag Vinyl Clean Unload Done");
                        FlagVinylCleanUnloadDone = false;

                        Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                        Sequence = ESequence.RobotPickFixtureFromVinylClean;
                        break;
                    }
                    if (FlagVinylCleanRequestLoad && FlagInCSTReady)
                    {
                        Log.Debug("Clear Flag Vinyl Clean Load Done");
                        FlagVinylCleanLoadDone = false;

                        Log.Info("Sequence Robot Pick Fixture From In Cassette");
                        Sequence = ESequence.RobotPickFixtureFromCST;
                        break;
                    }
                    if (FlagRemoveFilmRequestUnload)
                    {
                        _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD] = false;
                        Log.Debug("Clear Flag Remove Film Unload Done");
                        FlagRemoveFilmUnloadDone = false;

                        Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                        Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                        break;
                    }
                    break;
            }
        }
        #endregion
    }
}
