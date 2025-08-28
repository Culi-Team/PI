using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Common;
using EQX.Core.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
	public enum EMachineRunMode
	{
		Auto,
		ByPass,
		Dryrun
	}
    public class MachineStatus : ObservableObject
    {
		private EMachineRunMode _machineRunMode;
        private EProcessMode currentProcessMode;
        private int _OPCommand;

        public EMachineRunMode MachineRunMode
		{
			get 
			{
				return _machineRunMode; 
			}
			set 
			{
				_machineRunMode = value;
				OnPropertyChanged(nameof(MachineRunModeString)); 
			}
		}

		public string MachineRunModeString
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
    }
}
