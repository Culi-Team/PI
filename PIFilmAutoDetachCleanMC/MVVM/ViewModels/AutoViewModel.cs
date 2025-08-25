using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Process;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class AutoViewModel : ViewModelBase
    {
        public AutoViewModel(MachineStatus machineStatus)
        {
            MachineStatus = machineStatus;
        }

        public MachineStatus MachineStatus { get; }
    }
}
