using CommunityToolkit.Mvvm.ComponentModel;
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

		public EMachineRunMode MachineRunMode
		{
			get 
			{
				return _machineRunMode; 
			}
			set 
			{
				_machineRunMode = value;
				OnPropertyChanged(); 
			}
		}

	}
}
