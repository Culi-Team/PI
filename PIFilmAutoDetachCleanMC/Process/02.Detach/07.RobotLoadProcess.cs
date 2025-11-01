using EQX.Core.InOut;
using EQX.Core.Robot;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using PIFilmAutoDetachCleanMC.Defines.Devices.Robot;
using PIFilmAutoDetachCleanMC.Defines.Process;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Text.RegularExpressions;

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
        private readonly CassetteList _cassetteList;
        private readonly MachineStatus _machineStatus;
        private readonly CWorkData _workData;
        private int CurrentInWorkCSTFixtureIndex = -1;
        private int CurrentOutWorkCSTFixtureIndex = -1;
        private string[] paras = new string[8] { "0", "0", "0", "0", "0", "0", "0", "0" };
        private string strLastPosition;
        private int LastPosition;

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

        private bool RobotInPPPosition(int currentPos)
        {
            switch ((ERobotCommand)currentPos)
            {
                case ERobotCommand.S1_PP:
                case ERobotCommand.S2_PP:
                case ERobotCommand.S3_PP:
                case ERobotCommand.S4_PP:
                case ERobotCommand.S5_PP:
                    return true;
                case ERobotCommand.S1_RDY:
                case ERobotCommand.S2_RDY:
                case ERobotCommand.S3_RDY:
                case ERobotCommand.S4_RDY:
                case ERobotCommand.S5_RDY:
                default:
                    return false;
            }
        }

        private bool RobotInRDYPosition(int currentPos)
        {
            switch ((ERobotCommand)currentPos)
            {
                case ERobotCommand.S1_RDY:
                case ERobotCommand.S2_RDY:
                case ERobotCommand.S3_RDY:
                case ERobotCommand.S4_RDY:
                case ERobotCommand.S5_RDY:
                    return true;
                case ERobotCommand.S1_PP:
                case ERobotCommand.S2_PP:
                case ERobotCommand.S3_PP:
                case ERobotCommand.S4_PP:
                case ERobotCommand.S5_PP:
                default:
                    return false;
            }
        }
        #endregion

        #region Inputs
        private IDInput PeriRDY => _devices.Inputs.LoadRobPeriRdy;
        private IDInput StopMess => _devices.Inputs.LoadRobStopmess;
        private IDInput ProACT => _devices.Inputs.LoadRobProAct;
        private IDInput InHome => _devices.Inputs.LoadRobInHome;
        private IDInput InReady => _devices.Inputs.LoadRobInReady;
        private IDInput IOActCONF => _devices.Inputs.LoadRobIoActconf;
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

        private bool FlagVinylCleanLoadDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.VINYL_CLEAN_LOAD_DONE] = value;
            }
        }

        private bool FlagVinylCleanUnloadDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.VINYL_CLEAN_UNLOAD_DONE] = value;
            }
        }

        private bool FlagFixtureAlignRequestLoad
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.FIXTURE_ALIGN_REQ_LOAD];
            }
        }

        private bool FlagIn_VinylCleanClampUnClampDone
        {
            get
            {
                return _robotLoadInput[(int)ERobotLoadProcessInput.VINYL_CLEAN_CLAMP_UNCLAMP_DONE];
            }
        }

        private bool FlagOut_RobotMoveVinylCleanDone
        {
            set
            {
                _robotLoadOutput[(int)ERobotLoadProcessOutput.ROBOT_MOVE_VINYL_CLEAN_DONE] = value;
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
            _cassetteList = cassetteList;
            _workData = workData;
        }
        #endregion

        #region Override Methods
        public override bool ProcessToAlarm()
        {
            switch ((ERobotLoadToAlarmStep)Step.ToRunStep)
            {
                case ERobotLoadToAlarmStep.Start:
                    Log.Debug("To Alarm Start");
                    Step.ToRunStep++;
                    break;
                case ERobotLoadToAlarmStep.Stop:
                    Log.Debug("Stop Robot Load");
                    _robotLoad.SendCommand(RobotHelpers.RobotStop);

                    Wait(5000, () => _robotLoad.ReadResponse("Stop complete,0\r\n"));

                    Step.ToRunStep++;
                    break;
                case ERobotLoadToAlarmStep.Stop_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Stop_Fail);
                        break;
                    }

                    Log.Debug("Robot Load Stop Complete");
                    Step.ToRunStep++;
                    break;
                case ERobotLoadToAlarmStep.End:
                    if (ProcessStatus == EProcessStatus.ToAlarmDone)
                    {
                        Thread.Sleep(10);
                        break;
                    }

                    Log.Debug("To Alarm End");
                    ProcessStatus = EProcessStatus.ToAlarmDone;
                    Step.ToRunStep++;
                    break;
                default:
                    break;
            }
            return true;
        }

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
                case ERobotLoadToOriginStep.IOActCONF_Check:
                    if (IOActCONF.Value == false)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Automatic_External_Not_active);
                        break;
                    }

                    Log.Debug($"Automatic External active check done");
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
                        RaiseWarning((int)EWarning.RobotLoad_OriginFixtureDetect);
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
                case ERobotLoadOriginStep.RobotHomePosition_Check:
                    if (InHome.Value)
                    {
                        Log.Debug("Robot in home position");
                        Step.OriginStep = (int)ERobotLoadOriginStep.End;
                        break;
                    }

                    Log.Debug("Robot load is not in home position");
                    Step.OriginStep++;
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
                    if (SendCommand(ERobotCommand.HOME, 10, 10))
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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    Sequence_RobotPickFixtureFromCST();
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    Sequence_RobotPickPlaceVinylClean(false);
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    Sequence_RobotPickPlaceVinylClean(true);
                    break;
                case ESequence.RobotPlaceFixtureToAlign:
                    Sequence_RobotPlaceFixtureToAlign();
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    Sequence_RobotPickFixtureFromRemoveZone();
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    Sequence_RobotPlaceFixtureToOutCST();
                    break;
                default:
                    Sequence = ESequence.Stop;
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
                case ERobotLoadProcessToRunStep.IOActCONF_Check:
                    if (IOActCONF.Value == false)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Automatic_External_Not_active);
                        break;
                    }

                    Log.Debug($"Automatic External active check done");
                    Step.ToRunStep++;
                    break;
                case ERobotLoadProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<ERobotLoadProcessOutput>)_robotLoadOutput).ClearOutputs();
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
            switch ((ERobotLoad_AutoRunStep)Step.RunStep)
            {
                case ERobotLoad_AutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    _workData.TaktTime.TaktTimeCounter = Environment.TickCount;
                    Step.RunStep++;
                    break;
                case ERobotLoad_AutoRunStep.Check_FixtureDetect:
                    if (IsFixtureDetect == false)
                    {
                        Step.RunStep = (int)ERobotLoad_AutoRunStep.Check_Flag_VinylCleanRequestFixture;
                        break;
                    }

                    // FIXTURE DETECT
                    if (LastPosition == (int)ERobotCommand.S1_PP || LastPosition == (int)ERobotCommand.S1_RDY)
                    {
                        if (_devices.Inputs.VinylCleanFixtureDetect.Value == true)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_FixtureStatusAbnormal);
                            break;
                        }

                        if (FlagVinylCleanRequestLoad == false)
                        {
                            Wait(20);
                            break;
                        }

                        Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                        break;
                    }
                    if (LastPosition == (int)ERobotCommand.S2_PP || LastPosition == (int)ERobotCommand.S2_RDY)
                    {
                        if (_devices.Inputs.VinylCleanFixtureDetect.Value == false)
                        {
                            if (FlagVinylCleanRequestLoad == false)
                            {
                                Wait(20);
                                break;
                            }

                            Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                            break;
                        }

                        if (_devices.Inputs.AlignFixtureDetect.Value == false)
                        {
                            if (FlagFixtureAlignRequestLoad == false)
                            {
                                Wait(20);
                                break;
                            }

                            Sequence = ESequence.RobotPlaceFixtureToAlign;
                            break;
                        }

                        RaiseWarning((int)EWarning.RobotLoad_FixtureStatusAbnormal);
                        break;
                    }
                    if (LastPosition == (int)ERobotCommand.S3_PP || LastPosition == (int)ERobotCommand.S3_RDY)
                    {
                        if (_devices.Inputs.RemoveZoneFixtureDetect.Value == true ||
                            _devices.Inputs.AlignFixtureDetect.Value == true)
                        {
                            RaiseWarning((int)EWarning.RobotLoad_FixtureStatusAbnormal);
                            break;
                        }

                        if (FlagFixtureAlignRequestLoad == false)
                        {
                            Wait(20);
                            break;
                        }

                        Sequence = ESequence.RobotPlaceFixtureToAlign;
                        break;
                    }
                    if (LastPosition == (int)ERobotCommand.S4_PP || LastPosition == (int)ERobotCommand.S4_RDY ||
                        LastPosition == (int)ERobotCommand.S5_PP || LastPosition == (int)ERobotCommand.S5_RDY)
                    {
                        if (FlagOutCSTReady == false)
                        {
                            Wait(20);
                            break;
                        }

                        Sequence = ESequence.RobotPlaceFixtureToOutWorkCST;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_FixtureStatusAbnormal);
                    break;
                case ERobotLoad_AutoRunStep.Check_Flag_VinylCleanRequestFixture:
                    if (FlagVinylCleanRequestLoad && FlagInCSTReady)
                    {
                        Log.Info("Sequence Robot Pick Fixture From CST");
                        Sequence = ESequence.RobotPickFixtureFromCST;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoad_AutoRunStep.Check_Flag_RemoveFilm:
                    if (_devices.Inputs.RemoveZoneFixtureDetect.Value && !_machineStatus.IsDryRunMode)
                    {
                        if (FlagRemoveFilmRequestUnload)
                        {
                            Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                            Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                            break;
                        }
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoad_AutoRunStep.Check_Flag_VinylCleanRequestUnload:
                    if (FlagVinylCleanRequestUnload)
                    {
                        Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                        Sequence = ESequence.RobotPickFixtureFromVinylClean;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoad_AutoRunStep.Check_Flag_VinylCleanRequestFixture;
                    break;
                case ERobotLoad_AutoRunStep.End:
                    break;
            }
        }

        private void Sequence_Ready()
        {
            switch ((ERobotLoad_ReadyStep)Step.RunStep)
            {
                case ERobotLoad_ReadyStep.Start:
                    Log.Debug("Ready Start");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.ReConnectIfRequired:
                    if (!_robotLoad.IsConnected)
                    {
                        Log.Debug("Robot is not connected, trying to reconnect.");
                        _robotLoad.Connect();
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.CheckConnection:
                    if (!_robotLoad.IsConnected)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Connect_Fail);
                        break;
                    }

                    Log.Debug("Robot is connected.");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.EnableOutput:
                    Log.Debug("Enable Outputs Move Enable, Driver Off");
                    _devices.Outputs.LoadRobMoveEnable.Value = true;
                    _devices.Outputs.LoadRobDrivesOff.Value = true;
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotMotionOnCheck:
                    if (!PeriRDY.Value)
                    {
                        Log.Debug("Driver ON");
                        DrivesOn.Value = true;
                        Step.RunStep++;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoad_ReadyStep.RobotStopMessCheck;
                    break;
                case ERobotLoad_ReadyStep.RobotMotionOn_Delay:
                    Wait(3000, () => PeriRDY.Value);
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotMotionOn_Disable:
                    Log.Debug("Driver OFF");
                    DrivesOn.Value = false;
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotStopMessCheck:
                    if (StopMess.Value)
                    {
                        Log.Debug("Confirm message.");
                        ConfMess.Value = true;
                        Step.RunStep++;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoad_ReadyStep.RobotExtStart_Enable;
                    break;
                case ERobotLoad_ReadyStep.RobotConfMess_Delay:
                    Wait(500);
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotConfMess_Disable:
                    Log.Debug("Disable Confirm message.");
                    ConfMess.Value = false;
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotExtStart_Enable:
                    Log.Debug("Enable External Start.");
                    ExtStart.Value = true;
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotExtStart_Delay:
                    Wait(500);
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotExtStart_Disable:
                    Log.Debug("Disable External Start.");
                    ExtStart.Value = false;
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotProgramStart_Check:
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
                case ERobotLoad_ReadyStep.SendPGMStart:
                    _robotLoad.SendCommand(RobotHelpers.PCPGMStart);

                    Wait(5000, () => _robotLoad.ReadResponse("RobotReady,0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.SendPGMStart_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_No_Ready_Response);
                        break;
                    }

                    Log.Debug("Robot PGM Start Success.");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.SetModel:
                    Log.Debug("Set Model: " + _robotLoadRecipe.Model);
                    _robotLoad.SendCommand(RobotHelpers.SetModel(_robotLoadRecipe.Model));

                    Wait(5000, () => _robotLoad.ReadResponse($"select,{_robotLoadRecipe.Model},0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.SetModel_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_SetModel_Fail);
                        break;
                    }

                    Log.Debug("Set Model: " + _robotLoadRecipe.Model + " success");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotCurrentPosition_Check:
                    Log.Debug("Check robot last position");
                    _robotLoad.SendCommand(RobotHelpers.CheckLastPosition);

                    Wait(5000, () =>
                    {
                        strLastPosition = _robotLoad.ReadResponse();
                        return strLastPosition != string.Empty;
                    });

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotCurrentPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_GetLastPosition_Fail);
                        break;
                    }
                    Match match = Regex.Match(strLastPosition, @",(\d+),");

                    if (match.Success)
                    {
                        string data = match.Groups[1].Value;

                        if (int.TryParse(data, out LastPosition))
                        {
                            Log.Debug($"{_robotLoad.Name} is current in #{LastPosition} position");
                        }
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.Check_RobotInPPPosition:
                    if (RobotInPPPosition(LastPosition))
                    {
                        Log.Debug("Robot in PP position");
                        Step.RunStep = (int)ERobotLoad_ReadyStep.UnClamp;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.Check_RobotInRDYPosition:
                    if (RobotInRDYPosition(LastPosition) || InHome.Value || InReady.Value)
                    {
                        Log.Debug("Robot in RDY position");

                        // No Fixture Detect, Unclamp to prevent colision later
                        if (IsFixtureDetect == false)
                        {
                            Step.RunStep = (int)ERobotLoad_ReadyStep.UnClamp;
                            break;
                        }

                        // Fixture Detected in RDY position, just move on
                        Step.RunStep = (int)ERobotLoad_ReadyStep.Send_CassettePitch;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.Handle_RobotInUnknownPosition:
                    // Robot is not in PP / READY / HOME position
                    Log.Debug("Robot in UNKNOWN position, Please origin and try again");
                    RaiseWarning((int)EWarning.RobotLoad_Home_Manual_By_TeachingPendant);
                    break;
                case ERobotLoad_ReadyStep.UnClamp:
                    if (ClampCyl1.IsBackward && ClampCyl2.IsBackward)
                    {
                        Step.RunStep = (int)ERobotLoad_ReadyStep.UnAlign;
                        break;
                    }

                    Log.Debug("Unclamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                        break;
                    }

                    Log.Debug("UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.UnAlign:
                    if (AlignCyl.IsBackward)
                    {
                        Step.RunStep = (int)ERobotLoad_ReadyStep.RobotHomePosition_Check;
                        break;
                    }

                    Log.Debug("UnAlign");
                    AlignCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.UnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                        break;
                    }

                    Log.Debug("UnAlign Done");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotHomePosition_Check:
                    // InReady.Value and RobotInRDYPosition(LastPosition) is the same; just make sure
                    if (InHome.Value || InReady.Value || RobotInRDYPosition(LastPosition))
                    {
                        Log.Debug($"Robot load in {(InHome.Value ? "HOME" : "READY")} position");
                        Step.RunStep = (int)ERobotLoad_ReadyStep.Send_CassettePitch;
                        break;
                    }

                    Log.Debug("Robot load is not in ready position");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotSeqHome:
                    Log.Debug("Check sequence home robot load");
                    _robotLoad.SendCommand(RobotHelpers.SeqHomeCheck);

                    Wait(5000, () => _robotLoad.ReadResponse("Home safety,0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotSeqHome_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Home_Manual_By_TeachingPendant);
                        break;
                    }

                    Log.Debug("Robot Load Home Safely");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.RobotReady:
                    Log.Debug("Start Move Ready Position Robot Load");
                    Log.Debug($"Send Robot Motion Command {ERobotCommand.READY}");
                    if (SendCommand(ERobotCommand.READY, 10, 10))
                    {
                        Wait((int)(_commonRecipe.MotionOriginTimeout * 1000), () => _robotLoad.ReadResponse(RobotHelpers.MotionRspComplete(ERobotCommand.READY)));
                        Step.RunStep++;
                        break;
                    }

                    RaiseWarning((int)EWarning.RobotLoad_SendMotionCommand_Fail);
                    break;
                case ERobotLoad_ReadyStep.RobotReady_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.RobotLoad_MoveMotionCommand_Timeout);
                        break;
                    }

                    Log.Debug("Robot Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.Send_CassettePitch:
                    Log.Debug("Send Cassette Pitch: " + _cstLoadUnloadRecipe.Pitch);
                    _robotLoad.SendCommand(RobotHelpers.SetCassettePitch(0, _cstLoadUnloadRecipe.Pitch));

                    Wait(5000, () => _robotLoad.ReadResponse($"pitch,0,{_cstLoadUnloadRecipe.Pitch},0\r\n"));

                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.Send_CassettePitch_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_SetCassettePitch_Fail);
                        break;
                    }

                    Log.Debug("Set pitch X: 0, Y: " + _cstLoadUnloadRecipe.Pitch + " success");
                    Step.RunStep++;
                    break;
                case ERobotLoad_ReadyStep.End:
                    Log.Debug("Ready run end");
                    Sequence = ESequence.Stop;
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
                    if (WaitTimeOutOccurred)
                    {
                        if (ClampCyl1.IsBackward == false || ClampCyl2.IsBackward == false)
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
                        "1",
                        CurrentInWorkCSTFixtureIndex.ToString(),
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
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Align:
                    Log.Debug("Cylinder Align");
                    AlignCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Align_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_Align_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Align Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Clamp:
                    Log.Debug("Cylinder Clamp");
                    ClampCyl1.Forward();
                    ClampCyl2.Forward();
                    Wait(300);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Clamp_Delay:
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => (IsFixtureDetect) || _machineStatus.IsDryRunMode);
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
                    if (FlagInCSTReady == true)
                    {
                        Wait(20);
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
                        break;
                    }

                    Log.Info("Sequence Place Fixture To Vinyl Clean");
                    Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                    break;
            }
        }

        private void Sequence_RobotPickPlaceVinylClean(bool isUnload)
        {
            switch ((ERobotLoadPickPlaceFixtureVinylCleanStep)Step.RunStep)
            {
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Start:
                    Log.Debug("Robot" + (isUnload ? " Pick" : " Place") + " Fixture Vinyl Clean Start");
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

                    if (isUnload)
                    {
                        Step.RunStep++;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoadPickPlaceFixtureVinylCleanStep.SetFlag_RobotMoveVinylCleanDone_Load;
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
                    Wait(300);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylClamp_Delay:
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => (IsFixtureDetect) || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.CylClamp_Wait:
                    if (WaitTimeOutOccurred && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Clamp Done");
                    Log.Debug("Wait Vinyl Clean UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.SetFlag_RobotMoveVinylCleanDone_Unload:
                    Log.Debug("Set Flag Robot Move Vilyn Clean Done");
                    FlagOut_RobotMoveVinylCleanDone = true;
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Wait_VinylCleanUnClampDone:
                    if (FlagIn_VinylCleanClampUnClampDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear Flag RobotMoveVinylCleanDone");
                    FlagOut_RobotMoveVinylCleanDone = false;
                    Log.Debug("Vinyl Clean UnClamp Done");
                    Step.RunStep = (int)ERobotLoadPickPlaceFixtureVinylCleanStep.Move_VinylClean_ReadyPosition;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.SetFlag_RobotMoveVinylCleanDone_Load:
                    Log.Debug("Set Flag Robot Move Vinyl Clean Done");
                    FlagOut_RobotMoveVinylCleanDone = true;
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Wait_VinylCleanClampDone:
                    if (FlagIn_VinylCleanClampUnClampDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear Flag RobotMoveVinylCleanDone");
                    FlagOut_RobotMoveVinylCleanDone = false;
                    Log.Debug("Vinyl Clean Clamp Done");
                    Step.RunStep++;
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
                case ERobotLoadPickPlaceFixtureVinylCleanStep.SetFlag_VinylCleanLoadDone:
                    Log.Debug("Set Flag Vinyl Clean Load Done");
                    FlagVinylCleanLoadDone = true;
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
                case ERobotLoadPickPlaceFixtureVinylCleanStep.SetFlag_VinylCleanUnloadDone:
                    if (isUnload)
                    {
                        Log.Debug("Set Flag Vinyl Clean Unload Done");
                        FlagVinylCleanUnloadDone = true;
                        Log.Debug("Wait Vinyl Clean Receive Unload Done");
                        Step.RunStep++;
                        break;
                    }

                    Log.Debug("Wait Vinyl Clean Receive Load Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Wait_VinylCleanReceiveLoadUnloadDone:
                    if (isUnload)
                    {
                        if (FlagVinylCleanRequestUnload == true)
                        {
                            Wait(20);
                            break;
                        }

                        Log.Debug("Clear Flag Vinyl Clean Unload Done");
                        FlagVinylCleanUnloadDone = false;
                        Step.RunStep++;
                        break;
                    }

                    if (FlagVinylCleanRequestLoad == true)
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
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadPickPlaceFixtureVinylCleanStep.Wait_NextSequence:
                    if (isUnload)
                    {
                        Log.Info("Sequence Place Fixture To Align");
                        Sequence = ESequence.RobotPlaceFixtureToAlign;
                        break;
                    }
                    else
                    {
                        if (_devices.Inputs.RemoveZoneFixtureDetect.Value || _machineStatus.IsDryRunMode)
                        {
                            if (FlagRemoveFilmRequestUnload)
                            {
                                Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                                Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                                break;
                            }

                            else if (FlagVinylCleanRequestUnload && _machineStatus.IsDryRunMode)
                            {
                                Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                                Sequence = ESequence.RobotPickFixtureFromVinylClean;
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
                case ERobotLoadPlaceFixtureToAlignStep.UnClamp:
                    Log.Debug("Unclamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                        break;
                    }

                    Log.Debug("UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.UnAlign:
                    Log.Debug("UnAlign");
                    AlignCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToAlignStep.UnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                        break;
                    }

                    Log.Debug("UnAlign Done");
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
                    if (FlagFixtureAlignRequestLoad == true)
                    {
                        Wait(20);
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
                case ERobotLoadPickFixtureFromRemoveZoneStep.Align:
                    Log.Debug("Contact");
                    AlignCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Align_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_Align_Fail);
                        break;
                    }

                    Log.Debug("Align Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Clamp:
                    Log.Debug("Clamp");
                    ClampCyl1.Forward();
                    ClampCyl2.Forward();
                    Wait(300);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Clamp_Delay:
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => (IsFixtureDetect) || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.Clamp_Wait:
                    if (WaitTimeOutOccurred && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_Clamp_Fail);
                        break;
                    }

                    Log.Debug("Load Robot Clamp Done");
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
                case ERobotLoadPickFixtureFromRemoveZoneStep.Flag_RemoveZoneUnloadRequestClear_Wait:
                    if (FlagRemoveFilmRequestUnload == true)
                    {
                        Wait(20);
                        break;
                    }

                    FlagRemoveFilmUnloadDone = false;
                    Log.Debug("FlagRemoveFilmUnloadDone Clear");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromRemoveZoneStep.End:
                    Log.Debug("Robot Pick Fixture From Remove Zone Done");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
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
                        "1",
                        CurrentOutWorkCSTFixtureIndex.ToString(),
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
                case ERobotLoadPlaceFixtureToOutCSTStep.UnClamp:
                    Log.Debug("Load Robot UnClamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.UnAlign:
                    Log.Debug("Robot Load UnAlign");
                    AlignCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToOutCSTStep.UnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotLoad_Cylinder_UnAlign_Fail);
                        break;
                    }

                    Log.Debug("UnAlign Done");
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
                    if (FlagOutCSTReady == true)
                    {
                        Wait(20);
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
