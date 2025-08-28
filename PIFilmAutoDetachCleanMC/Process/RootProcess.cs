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
        private readonly Devices _devices;
        private readonly MachineStatus _machineStatus;

        public RootProcess(Devices devices,
            MachineStatus machineStatus)
        {
            _devices = devices;
            _machineStatus = machineStatus;
        }

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

            return base.PreProcess();
        }

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
    }
}
