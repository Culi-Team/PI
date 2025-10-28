using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Process;
using EQX.Core.Sequence;
using EQX.InOut.InOut;
using PIFilmAutoDetachCleanMC.Defines;
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
        #region Events
        public event Action? ActiveScreenChanged;
        #endregion

        public MachineStatus(Inputs inputs)
        {
            _inputs = inputs;
            _fixtureExistStatus = new List<bool>() { false, false, false };
        }

        public const int DryRunVacuumDurationMilliseconds = 1000;
        private readonly Inputs _inputs;

        public EScreen ActiveScreen
        {
            get { return activeScreen; }
            set
            {
                if (activeScreen == value)
                {
                    return;
                }

                activeScreen = value;
                ActiveScreenChanged?.Invoke();
            }
        }

        //public bool IsByPassMode => _machineRunMode == EMachineRunMode.ByPass;
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

        public bool MachineReadyDone { get; set; }

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

        public bool MachineTestMode { get; set; }

        public void RecordFixtureExistStatus()
        {
            _fixtureExistStatus[0] = _inputs.AlignFixtureDetect.Value || IsDryRunMode;
            _fixtureExistStatus[1] = _inputs.DetachFixtureDetect.Value || IsDryRunMode;
            _fixtureExistStatus[2] = _inputs.RemoveZoneFixtureDetect.Value;
        }

        public IReadOnlyList<bool> FixtureExistStatus => _fixtureExistStatus;
        private readonly List<bool> _fixtureExistStatus;

        #region Privates
        private EMachineRunMode _machineRunMode;
        private EProcessMode currentProcessMode;
        private int _SemiAutoSequence;
        private int _OPCommand;
        private bool _originDone;
        private bool isInputStop;
        private bool isOutputStop;
        private EScreen activeScreen;
        #endregion
    }
}
