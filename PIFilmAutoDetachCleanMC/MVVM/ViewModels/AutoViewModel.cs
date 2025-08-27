using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Process;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class AutoViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public AutoViewModel(MachineStatus machineStatus,
            INavigationService navigationService)
        {
            MachineStatus = machineStatus;
            _navigationService = navigationService;
        }

        public MachineStatus MachineStatus { get; }

        public ICommand IOMonitoringNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<IOMonitoringViewModel>();
                });
            }
        }
    }
}
