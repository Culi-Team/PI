using EQX.Core.Common;
using EQX.Core.Sequence;
using EQX.Process;
using EQX.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using System.Windows;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class RootProcess<TESequence, TESemiSequence> : ProcessBase<ESequence>
        where TESequence : Enum
        where TESemiSequence : Enum
    {
        #region Privates
        private readonly Devices _devices;
        private readonly MachineStatus _machineStatus;
        private int raisedAlarmCode = -1;
        private int raisedWarningCode = -1;
        private readonly IAlertService _alarmService;

        private readonly IAlertService _warningService;
        private readonly object _lockAlarm = new object();

        private bool DoorSensor
        {
            get
            {
                return _devices.Inputs.DoorLock1L.Value &&
                       _devices.Inputs.DoorLock1R.Value &&
                       _devices.Inputs.DoorLock2L.Value &&
                       _devices.Inputs.DoorLock2R.Value &&
                       _devices.Inputs.DoorLock3L.Value &&
                       _devices.Inputs.DoorLock3R.Value &&
                       _devices.Inputs.DoorLock4L.Value &&
                       _devices.Inputs.DoorLock4R.Value &&
                       _devices.Inputs.DoorLock5L.Value &&
                       _devices.Inputs.DoorLock5R.Value &&
                       _devices.Inputs.DoorLock6L.Value &&
                       _devices.Inputs.DoorLock6R.Value &&
                       _devices.Inputs.DoorLock7L.Value &&
                       _devices.Inputs.DoorLock7R.Value;
            }
        }

        private bool DoorLatch
        {
            get
            {
                return _devices.Inputs.DoorLatch1L.Value &&
                       _devices.Inputs.DoorLatch1R.Value &&
                       _devices.Inputs.DoorLatch2L.Value &&
                       _devices.Inputs.DoorLatch2R.Value &&
                       _devices.Inputs.DoorLatch3L.Value &&
                       _devices.Inputs.DoorLatch3R.Value &&
                       _devices.Inputs.DoorLatch4L.Value &&
                       _devices.Inputs.DoorLatch4R.Value &&
                       _devices.Inputs.DoorLatch5L.Value &&
                       _devices.Inputs.DoorLatch5R.Value &&
                       _devices.Inputs.DoorLatch6L.Value &&
                       _devices.Inputs.DoorLatch6R.Value &&
                       _devices.Inputs.DoorLatch7L.Value &&
                       _devices.Inputs.DoorLatch7R.Value;
            }
        }

        private bool IsAutoMode => _devices.Inputs.AutoModeSwitchL.Value && _devices.Inputs.AutoModeSwitchR.Value;
        private bool IsManualMode => _devices.Inputs.ManualModeSwitchL.Value || _devices.Inputs.ManualModeSwitchL.Value;

        private bool IsLightCurtainLeftDetect => _devices.Inputs.OutCstLightCurtainAlarmDetect.Value;
        private bool IsLightCurtainRightDetect => _devices.Inputs.InCstLightCurtainAlarmDetect.Value;
        private bool IsMainAirSupplied => _devices.Inputs.MainAir1.Value && _devices.Inputs.MainAir2.Value && _devices.Inputs.MainAir3.Value;
        private bool IsEmergencyStopActive =>
            _devices.Inputs.EmoLoadL.Value ||
            _devices.Inputs.EmoLoadR.Value ||
            _devices.Inputs.OpLEmo.Value ||
            _devices.Inputs.OpREmo.Value ||
            _devices.Inputs.EmoUnloadL.Value ||
            _devices.Inputs.EmoUnloadR.Value;
        private bool IsPowerMCOn => _devices.Inputs.PowerMCOn1.Value && _devices.Inputs.PowerMCOn2.Value;
        #endregion

        #region Constructor
        public RootProcess(Devices devices,
            MachineStatus machineStatus,
            [FromKeyedServices("AlarmService")] IAlertService alarmService,
            [FromKeyedServices("WarningService")] IAlertService warningService)
        {
            _devices = devices;
            _machineStatus = machineStatus;
            _alarmService = alarmService;
            _warningService = warningService;

            this.ProcessModeUpdated += ProcessModeUpdatedHandler;

            this.AlarmRaised += (alarmId, alarmSource) =>
            {
                RootProcess_AlarmRaised(alarmId, alarmSource);
            };
            this.WarningRaised += (warningId, warningSource) =>
            {
                RootProcess_WarningRaised(warningId, warningSource);
            };
        }

        #endregion

        #region Override Methods
        public override bool PreProcess()
        {
            // 1. CHECK ALARM STATUS (Utils, Motion, Safety...)
            if (ProcessMode != EProcessMode.ToAlarm && ProcessMode != EProcessMode.Alarm)
            {
                CheckRealTimeAlarmStatus();
            }

            if (ProcessMode == EProcessMode.ToOrigin || ProcessMode == EProcessMode.Origin || ProcessMode == EProcessMode.ToRun || ProcessMode == EProcessMode.Run)
            {
                if (!IsLightCurtainLeftDetect)
                {
                    RaiseAlarm((int)EAlarm.LightCurtainLeftDetected);
                }
                if (!IsLightCurtainRightDetect)
                {
                    RaiseAlarm(alarmId: (int)EAlarm.LightCurtainRightDetected);
                }
                if (IsAutoMode == false || IsManualMode == true)
                {
                    RaiseWarning((int)EWarning.ManualModeSwitch);
                }
            }

            //2.CHECK USER OPERATION COMMAND(Origin / Ready / Start / Stop / Semiauto...)
            EOperationCommand command = EOperationCommand.None;
            if (_machineStatus.IsRunningProcessMode)
            {
                // Machine is doing something : Run, Origin, To-Action
                // Can Stop
                if (_machineStatus.OPCommand == EOperationCommand.Stop
                    || _devices.Inputs.OpLButtonStop.Value == true
                    || _devices.Inputs.OpRButtonStop.Value == true
                    /*|| _machineStatus.InitDeinitStatus == EInitDeinitStatus.DeinitStarted*/)
                {
                    command = EOperationCommand.Stop;
                }
                // Block run OPCommand actived while machine is Runiing
                _machineStatus.OPCommand = EOperationCommand.None;
            }
            else
            {
                // Machine is in standby mode (doing nothing)
                // Can Origin / Ready / Start
                if (_machineStatus.OPCommand == EOperationCommand.Origin)
                {
                    command = EOperationCommand.Origin;
                }

                if (ProcessMode != EProcessMode.ToAlarm &&
                    ProcessMode != EProcessMode.Alarm && ProcessMode != EProcessMode.None)
                {
                    if (_machineStatus.OPCommand == EOperationCommand.Ready
                        || _devices.Inputs.OpLButtonReset.Value == true
                        || _devices.Inputs.OpRButtonReset.Value == true)
                    {
                        command = EOperationCommand.Ready;
                    }
                    else if (_machineStatus.OPCommand == EOperationCommand.Start
                        || _devices.Inputs.OpLButtonStart.Value == true
                        || _devices.Inputs.OpRButtonStart.Value == true)
                    {
                        command = EOperationCommand.Start;
                    }
                    else if (_machineStatus.OPCommand == EOperationCommand.SemiAuto)
                    {
                        command = EOperationCommand.SemiAuto;
                    }
                }
                else if (_machineStatus.OPCommand == EOperationCommand.Ready
                    || _machineStatus.OPCommand == EOperationCommand.Start
                    || _devices.Inputs.OpLButtonReset.Value == true
                    || _devices.Inputs.OpRButtonReset.Value == true)
                {
                    MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_ResetAlarmBeforeRun"], (string)Application.Current.Resources["str_Confirm"]);
                    _machineStatus.OPCommand = EOperationCommand.None;
                }
            }

            // 3. HANDLE USER OPERATION COMMAND
            HandleOPCommand(command);
            return base.PreProcess();
        }

        public override bool ProcessToAlarm()
        {
            if (Childs!.Count(child => child.ProcessStatus != EProcessStatus.ToAlarmDone) == 0)
            {
                foreach (var motion in _devices.MotionsInovance.All!) { motion.Stop(); }
                foreach (var motion in _devices.MotionsAjin.All!) { motion.Stop(); }

                _machineStatus.OriginDone = false;

                _devices.Outputs.Lamp_Alarm();

                ProcessMode = EProcessMode.Alarm;
                Log.Info("ToAlarm Done, Alarm");
                AlertNotifyView.ShowDialog(_alarmService.GetById(raisedAlarmCode));
                raisedAlarmCode = -1;
            }
            else
            {
                Thread.Sleep(10);
            }

            return true;
        }

        public override bool ProcessToWarning()
        {
            if (Childs!.Count(child => child.ProcessStatus != EProcessStatus.ToWarningDone) == 0)
            {
                foreach (var motion in _devices.MotionsInovance.All!) { motion.Stop(); }
                foreach (var motion in _devices.MotionsAjin.All!) { motion.Stop(); }

                // Set Robot process Warning , Need Initialize before Run
                Childs!.First(p => p.Name == EProcess.RobotLoad.ToString()).IsWarning = true;
                Childs!.First(p => p.Name == EProcess.RobotUnload.ToString()).IsWarning = true;

                _devices.Outputs.Lamp_Alarm();
                ProcessMode = EProcessMode.Warning;
                Log.Info("ToWarning Done, Warning");
                AlertNotifyView.ShowDialog(_warningService.GetById(raisedWarningCode), true);
                raisedWarningCode = -1;
            }
            else
            {
                Thread.Sleep(10);
            }

            return true;
        }

        public override bool ProcessOrigin()
        {
            if (Childs!.Count(child => child.ProcessStatus != EProcessStatus.OriginDone) == 0)
            {
                _machineStatus.OriginDone = true;

                ProcessMode = EProcessMode.ToStop;
                Log.Info("Origin done, Stop");
                MessageBoxEx.Show(Application.Current.Resources["str_OriginSuccess"].ToString(), false);
            }
            else
            {
                Thread.Sleep(10);
            }

            return true;
        }

        public override bool ProcessToOrigin()
        {
            switch ((ERootProcessToOriginStep)Step.OriginStep)
            {
                case ERootProcessToOriginStep.Start:
                    Log.Info("To Origin started");
                    _devices.Outputs.Lamp_Run();
                    Step.OriginStep++;
                    break;
                case ERootProcessToOriginStep.AutoMode_Check:
                    if (IsAutoMode == false || IsManualMode == true)
                    {
                        RaiseWarning((int)EWarning.ManualModeSwitch);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case ERootProcessToOriginStep.DoorSensorCheck:
                    //if (_machineStatus.IsByPassMode)
                    //{
                    //    Log.Warn("ByPass mode active - skipping door sensor check during origin.");
                    //    Step.OriginStep++;
                    //    break;
                    //}
                    if (DoorSensor == false)
                    {
                        //WARNING
                        RaiseWarning((int)EWarning.DoorOpen);
                        break;
                    }
                    Log.Debug("Doors closed.");
                    Step.OriginStep++;
                    break;
                case ERootProcessToOriginStep.DoorLock:
                    //if (_machineStatus.IsByPassMode)
                    //{
                    //    Log.Warn("ByPass mode active - skipping door lock during origin.");
                    //    Step.OriginStep++;
                    //    break;
                    //}
                    _devices.Outputs.DoorOpen.Value = false;
                    Step.OriginStep++;
                    break;
                case ERootProcessToOriginStep.DoorLatchCheck:
                    //if (_machineStatus.IsByPassMode)
                    //{
                    //    Log.Warn("ByPass mode active - skipping door latch check during origin.");
                    //    Step.OriginStep++;
                    //    break;
                    //}
                    if (DoorLatch == false)
                    {
                        RaiseWarning((int)EWarning.DoorNotSafetyLock);
                        break;
                    }
                    Log.Debug("Doors locked safely.");
                    Step.OriginStep++;
                    break;
                case ERootProcessToOriginStep.ChildsToOriginDoneWait:
                    if (Childs!.Count(child => child.ProcessStatus != EProcessStatus.ToOriginDone) != 0)
                    {
                        Wait(10);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case ERootProcessToOriginStep.End:
                    Log.Info("To Origin done");
                    ProcessMode = EProcessMode.Origin;
                    Step.OriginStep = 0;
                    break;
            }
            return true;
        }

        public override bool ProcessToStop()
        {
            if (Childs!.Count(child => child.ProcessStatus != EProcessStatus.ToStopDone) == 0)
            {
                ProcessMode = EProcessMode.Stop;

                _devices.Outputs.Lamp_Stop();
                Log.Info("ToStop Done, Stop");
            }
            else
            {
                Thread.Sleep(10);
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((ERootProcessToRunStep)Step.ToRunStep)
            {
                case ERootProcessToRunStep.Start:
                    Log.Debug("ToRun Start");
                    _devices.Outputs.Lamp_Run();
                    Step.ToRunStep++;
                    break;
                case ERootProcessToRunStep.AutoMode_Check:
                    if (IsAutoMode == false || IsManualMode == true)
                    {
                        RaiseWarning((int)EWarning.ManualModeSwitch);
                        break;
                    }
                    Step.ToRunStep++;
                    break;
                case ERootProcessToRunStep.DoorSensorCheck:
                    //if (_machineStatus.IsByPassMode)
                    //{
                    //    Log.Warn("ByPass mode active - skipping door sensor check during run.");
                    //    Step.ToRunStep++;
                    //    break;
                    //}
                    if (DoorSensor == false)
                    {
                        RaiseWarning((int)EWarning.DoorOpen);
                        break;
                    }
                    Log.Debug("Doors closed.");
                    Step.ToRunStep++;
                    break;
                case ERootProcessToRunStep.DoorLock:
                    //if (_machineStatus.IsByPassMode)
                    //{
                    //    Log.Warn("ByPass mode active - skipping door lock during run.");
                    //    Step.ToRunStep++;
                    //    break;
                    //}
                    _devices.Outputs.DoorOpen.Value = false;
                    Step.ToRunStep++;
                    break;
                case ERootProcessToRunStep.DoorLatchCheck:
                    //if (_machineStatus.IsByPassMode)
                    //{
                    //    Log.Warn("ByPass mode active - skipping door latch check during run.");
                    //    Step.ToRunStep++;
                    //    break;
                    //}
                    if (DoorLatch == false)
                    {
                        RaiseWarning((int)EWarning.DoorNotSafetyLock);
                        break;
                    }
                    Log.Debug("Doors locked safely.");
                    Step.ToRunStep++;
                    break;
                case ERootProcessToRunStep.ChildsToRunDone_Wait:
                    if (Childs!.Count(child => child.ProcessStatus != EProcessStatus.ToRunDone) != 0)
                    {
                        Wait(10);
                        break;
                    }
                    Step.ToRunStep++;
                    break;
                case ERootProcessToRunStep.End:
                    ProcessMode = EProcessMode.Run;
                    Log.Info("ToRun Done, Running");
                    break;
                default:
                    Thread.Sleep(10);
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
                    break;
                case ESequence.Ready:
                    if (Childs!.Count(child => child.Sequence != ESequence.Stop) == 0)
                    {
                        ProcessMode = EProcessMode.ToStop;
                        Log.Info("Initialize done");
                    }
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
                    break;
                case ESequence.UnloadRobotPlasma:
                    break;
                case ESequence.UnloadRobotPlace:
                    break;
            }

            return true;
        }

        #endregion

        #region Private Methods
        private void ProcessModeUpdatedHandler(object? sender, EventArgs e)
        {
            _machineStatus.CurrentProcessMode = this.ProcessMode;
        }

        private void CheckRealTimeAlarmStatus()
        {
            if (!IsPowerMCOn)
            {
                RaiseAlarm((int)EAlarm.PowerMCOff);
                Childs!.ToList().ForEach(p => p.IsAlarm = true);
                return;
            }

            if (IsEmergencyStopActive)
            {
                Childs!.ToList().ForEach(p => p.IsAlarm = true);
                RaiseAlarm((int)EAlarm.EmergencyStopActivated);
                return;
            }

            if (IsMainAirSupplied == false)
            {
                Childs!.ToList().ForEach(p => p.IsAlarm = true);
                RaiseAlarm((int)EAlarm.MainAirNotSupplied);
                return;
            }
        }

        //private EOperationCommand GetUserCommand()
        //{
        //    if (_systemState.CommandState.HMICommand == EOperationCommand.Stop ||
        //        _devices.Inputs.StopSW.Value == true)
        //    {
        //        return EOperationCommand.Stop;
        //    }
        //    if (_systemState.CommandState.HMICommand == EOperationCommand.Stop ||
        //        _devices.Inputs.StopSW.Value == true)
        //    {
        //        return EOperationCommand.Stop;
        //    }

        //    return EOperationCommand.None;
        //}

        private void HandleOPCommand(EOperationCommand command)
        {
            switch (command)
            {
                case EOperationCommand.None:
                    Thread.Sleep(10);
                    break;
                case EOperationCommand.Origin:
                    foreach (var motion in _devices.MotionsAjin.All!) { motion.MotionOn(); }
                    foreach (var motion in _devices.MotionsInovance.All!) { motion.MotionOn(); }

                    Thread.Sleep(1000);

                    if (_devices.MotionsAjin.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog("Motions turn on failed");
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (_devices.MotionsInovance.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog("Motions turn on failed");
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    //_machineStatus.ZAxisHomeDone = false;

                    Step.OriginStep = 0;
                    Childs?.ToList().ForEach(p => p.Step.OriginStep = 0);
                    ProcessMode = EProcessMode.ToOrigin;
                    _machineStatus.OPCommand = EOperationCommand.None;
                    break;
                case EOperationCommand.Ready:
                    if (_devices.MotionsAjin.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (_devices.MotionsInovance.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (_machineStatus.OriginDone == false)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_MachineNeedToBeOriginBeforeRun"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    Sequence = ESequence.Ready;
                    foreach (var process in Childs!)
                    {
                        process.ProcessStatus = EProcessStatus.None;
                        process.Sequence = Sequence;
                    }

                    ProcessMode = EProcessMode.ToRun;
                    _machineStatus.OPCommand = EOperationCommand.None;
                    break;
                case EOperationCommand.Start:
                    if (_devices.MotionsAjin.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (_devices.MotionsInovance.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (Childs!.Any(p => p.IsAlarm))
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_MachineNeedToBeOriginBeforeRun"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (Childs!.Any(p => p.IsWarning))
                    {
                        MessageBoxEx.ShowDialog("Machine Need To Be Initialize Before Run");
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    Sequence = ESequence.AutoRun;

                    foreach (var process in Childs!)
                    {
                        process.ProcessStatus = EProcessStatus.None;
                        process.Sequence = Sequence;
                    }

                    ProcessMode = EProcessMode.ToRun;

                    _machineStatus.OPCommand = EOperationCommand.None;
                    _machineStatus.SemiAutoSequence = ESemiSequence.None;
                    break;
                case EOperationCommand.Stop:
                    foreach (var motion in _devices.MotionsAjin.All!) { motion.Stop(); }
                    foreach (var motion in _devices.MotionsInovance.All!) { motion.Stop(); }

                    ProcessMode = EProcessMode.ToStop;
                    _machineStatus.OPCommand = EOperationCommand.None;
                    break;
                case EOperationCommand.SemiAuto:
                    if (_devices.MotionsAjin.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (_devices.MotionsInovance.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    if (_machineStatus.OriginDone == false)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_MachineNeedToBeOriginBeforeRun"]);
                        _machineStatus.OPCommand = EOperationCommand.None;
                        return;
                    }

                    //switch (_machineStatus.SemiAutoSequence)
                    //{
                    //    case ESemiAutoSequence.LeftIn_Load: Sequence = EUTGSequence.LeftIn_Load; break;
                    //    case ESemiAutoSequence.LeftIn_PickPlace: Sequence = EUTGSequence.LeftIn_PickPlace; break;
                    //    case ESemiAutoSequence.LeftIn_Transfer: Sequence = EUTGSequence.LeftIn_Transfer; break;
                    //    case ESemiAutoSequence.LeftIn_Change: Sequence = EUTGSequence.LeftIn_Change; break;
                    //    case ESemiAutoSequence.RightIn_Load: Sequence = EUTGSequence.RightIn_Load; break;
                    //    case ESemiAutoSequence.RightIn_PickPlace: Sequence = EUTGSequence.RightIn_PickPlace; break;
                    //    case ESemiAutoSequence.RightIn_Change: Sequence = EUTGSequence.RightIn_Change; break;
                    //    case ESemiAutoSequence.TraySup_Load: Sequence = EUTGSequence.TraySup_Load; break;
                    //    case ESemiAutoSequence.TraySup_Change: Sequence = EUTGSequence.TraySup_Change; break;
                    //    case ESemiAutoSequence.NGTray_Load: Sequence = EUTGSequence.NGTray_Load; break;
                    //    case ESemiAutoSequence.NGTray_PlaceNG: Sequence = EUTGSequence.NGTray_PlaceNG; break;
                    //    case ESemiAutoSequence.NGTray_Transfer: Sequence = EUTGSequence.NGTray_Transfer; break;
                    //    case ESemiAutoSequence.NGTray_Change: Sequence = EUTGSequence.NGTray_Change; break;
                    //    case ESemiAutoSequence.Robot_Inspect: Sequence = EUTGSequence.Robot_Inspect; break;
                    //    case ESemiAutoSequence.None:
                    //    default:
                    //        Thread.Sleep(20);
                    //        break;
                    //}

                    foreach (var process in Childs!)
                    {
                        process.ProcessStatus = EProcessStatus.None;
                        process.Sequence = Sequence;
                    }

                    ProcessMode = EProcessMode.ToRun;

                    _machineStatus.OPCommand = EOperationCommand.None;
                    _machineStatus.SemiAutoSequence = ESemiSequence.None;
                    break;
                default:
                    break;
            }
        }

        private void RootProcess_AlarmRaised(int alarmId, string alarmSource)
        {
            lock (_lockAlarm)
            {
                if (this.IsInAlarmMode()) return;

                Log.Error($"{alarmSource} raising alarm [#{(int)(EAlarm)alarmId}] {(EAlarm)alarmId}");
                raisedAlarmCode = alarmId;
                ProcessMode = EProcessMode.ToAlarm;
            }
        }

        private void RootProcess_WarningRaised(int warningId, string warningSource)
        {
            lock (_lockAlarm)
            {
                if (this.IsInWarningMode() || this.IsInAlarmMode()) return;

                Log.Warn($"{warningSource} raising warning [#{(int)(EWarning)warningId}] {(EWarning)warningId}");

                raisedWarningCode = warningId;
                ProcessMode = EProcessMode.ToWarning;
            }
        }
        #endregion
    }
}
