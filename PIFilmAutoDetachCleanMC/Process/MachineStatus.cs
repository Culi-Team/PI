using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Common;
using EQX.Core.Process;
using EQX.Core.Sequence;
using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class MachineStatus : ObservableObject
    {
		private EMachineRunMode _machineRunMode;
        private EProcessMode currentProcessMode;
        private int _SemiAutoSequence;
        private int _OPCommand;

        public bool IsByPassMode => _machineRunMode == EMachineRunMode.ByPass;
        public bool IsDryRunMode => _machineRunMode == EMachineRunMode.DryRun;
        public EMachineRunMode MachineRunMode
        {
            get
            {
                return _machineRunMode;
            }
            set
            {
                if (_machineRunMode == value)
                {
                    return;
                }

                _machineRunMode = value;
                OnPropertyChanged(nameof(MachineRunMode));
                OnPropertyChanged(nameof(MachineRunModeDisplay));
                OnPropertyChanged(nameof(IsByPassMode));
                OnPropertyChanged(nameof(IsDryRunMode));
            }
        }
        public string MachineRunModeDisplay 
        { 
            get
            {
                return _machineRunMode.ToString();
            }
        }

        public EProcessMode CurrentProcessMode
        {
            get => currentProcessMode;
            set
            {
                currentProcessMode = value;
                OnPropertyChanged(nameof(IsRunningProcessMode));
                OnPropertyChanged(nameof(IsStandByProcessMode));
                OnPropertyChanged(nameof(IsReadyToRunProcessMode));
                OnPropertyChanged();
            }
        }

        public bool IsReadyToRunProcessMode
        {
            get
            {
                return
                    currentProcessMode == EProcessMode.Warning ||
                    currentProcessMode == EProcessMode.Stop;
            }
        }

        public bool IsStandByProcessMode
        {
            get
            {
                return
                    currentProcessMode == EProcessMode.None ||
                    currentProcessMode == EProcessMode.Alarm ||
                    currentProcessMode == EProcessMode.Warning ||
                    currentProcessMode == EProcessMode.Stop;
            }
        }

        public bool IsRunningProcessMode => !IsStandByProcessMode;

        public EOperationCommand OPCommand
        {
            get => (EOperationCommand)_OPCommand;
            set
            {
                MultiThreadingHelpers.SafeSetValue(ref _OPCommand, value);
            }
        }

        private bool _originDone;

        public bool OriginDone
        {
            get { return _originDone; }
            set { _originDone = value; }
        }

        private bool initializeDone;

        public bool InitializeDone
        {
            get { return initializeDone; }
            set { initializeDone = value; }
        }

        public ESemiSequence SemiAutoSequence
        {
            get => (ESemiSequence)_SemiAutoSequence;
            set
            {
                MultiThreadingHelpers.SafeSetValue(ref _SemiAutoSequence, value);
            }
        }
    }
}
