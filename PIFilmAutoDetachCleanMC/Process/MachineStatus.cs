using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Process;
using EQX.Core.Sequence;
using EQX.InOut.InOut;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Services.DryRunServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class MachineStatus : ObservableObject
    {
        public MachineStatus()
        {
            _dryRunProfile = DryRunBypassProfile.CreateDefault();
        }

        private EMachineRunMode _machineRunMode;
        private EProcessMode currentProcessMode;
        private int _SemiAutoSequence;
        private int _OPCommand;
        private bool _originDone;
        private bool isInputStop;
        private bool isOutputStop;
        private readonly DryRunBypassProfile _dryRunProfile;
        public const int DryRunVacuumDurationMilliseconds = 1000;

        //public bool IsByPassMode => _machineRunMode == EMachineRunMode.ByPass;
        public bool IsDryRunMode => _machineRunMode == EMachineRunMode.DryRun;
        public IReadOnlyCollection<DryRunBypassGroup> DryRunBypassGroups => _dryRunProfile.EnabledGroups;
        public IReadOnlyCollection<EInput> ActiveDryRunBypasses => _dryRunProfile.ActiveInputs;
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
                //OnPropertyChanged(nameof(IsByPassMode));
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

        public bool OriginDone
        {
            get { return _originDone; }
            set { _originDone = value; }
        }

        public bool IsInputStop
        {
            get { return isInputStop; }
            set { isInputStop = value; OnPropertyChanged(); }
        }

        public bool IsOutputStop
        {
            get { return isOutputStop; }
            set { isOutputStop = value; OnPropertyChanged(); }
        }

        public ESemiSequence SemiAutoSequence
        {
            get => (ESemiSequence)_SemiAutoSequence;
            set
            {
                MultiThreadingHelpers.SafeSetValue(ref _SemiAutoSequence, value);
            }
        }

        public void ConfigureDryRunBypass(params DryRunBypassGroup[] groups)
        {
            _dryRunProfile.SetEnabledGroups(groups);
            OnPropertyChanged(nameof(DryRunBypassGroups));
        }

        public bool ShouldBypass(EInput input)
        {
            return IsDryRunMode && _dryRunProfile.ShouldBypass(input);
        }

        public bool ShouldBypass(int inputId)
        {
            if (!Enum.IsDefined(typeof(EInput), inputId))
            {
                return false;
            }

            return ShouldBypass((EInput)inputId);
        }

        public int DryRunVacuumDuration => DryRunVacuumDurationMilliseconds;
    }
}
