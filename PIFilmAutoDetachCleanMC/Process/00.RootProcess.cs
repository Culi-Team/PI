using EQX.Core.Sequence;
using EQX.Process;
using EQX.UI.Controls;
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
        #endregion

        #region Constructor
        public RootProcess(Devices devices,
            MachineStatus machineStatus)
        {
            _devices = devices;
            _machineStatus = machineStatus;
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
            switch((ERootProcessToOriginStep)Step.OriginStep)
            {
                case ERootProcessToOriginStep.Start:
                    Log.Info("To Origin started");
                    Step.OriginStep++;
                    break;
                case ERootProcessToOriginStep.DoorSensorCheck:
                    Log.Info("DoorSensorCheck");
                    if (DoorSensor == false)
                    {
                        //WARNING
                        RaiseWarning(0);
                        break;
                    }
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
        #endregion

        private void CheckRealTimeAlarmStatus()
        {
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

                    if (_machineStatus.OriginDone == false)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_MachineNeedToBeOriginBeforeRun"]);
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

    }
}
