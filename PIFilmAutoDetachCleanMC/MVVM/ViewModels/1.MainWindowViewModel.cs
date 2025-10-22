using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Factories;
using PIFilmAutoDetachCleanMC.Process;
using System.Windows;
using System.Windows.Threading;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties
        public EScreen Screen { get; set; }

        public bool IsThisVMActive => _machineStatus.ActiveScreen == Screen;

        public ViewModelBase HeaderVM { get; }
        public ViewModelBase FooterVM { get; }

        public ViewModelBase CurrentFrameVM
        {
            get
            {
                if (IsThisVMActive)
                {
                    return _navigationStore.CurrentViewModel;
                }
                else
                {
                    return _maintenanceVM;
                }
            }
        }
        #endregion

        public MainWindowViewModel(ViewModelNavigationStore navigationStore,
                                   ViewModelProvider viewModelProvider,
                                   MachineStatus machineStatus)
        {
            _navigationStore = navigationStore;
            _viewModelProvider = viewModelProvider;
            _machineStatus = machineStatus;

            HeaderVM = _viewModelProvider.GetViewModel<HeaderViewModel>();
            FooterVM = _viewModelProvider.GetViewModel<FooterViewModel>();
            _maintenanceVM = _viewModelProvider.GetViewModel<MaintenanceViewModel>();

            _navigationStore.CurrentViewModelChanged += FrameNavigationStore_CurrentViewModelChanged;
            _machineStatus.ActiveScreenChanged += MachineStatus_ActiveScreenChanged;
        }

        #region Privates methods
        private void MachineStatus_ActiveScreenChanged()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                OnPropertyChanged(nameof(CurrentFrameVM));
                OnPropertyChanged(nameof(IsThisVMActive));
            }), DispatcherPriority.DataBind);
        }

        private void FrameNavigationStore_CurrentViewModelChanged()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                OnPropertyChanged(nameof(CurrentFrameVM));
            }), DispatcherPriority.DataBind);
        }
        #endregion

        #region Private fields
        private readonly ViewModelBase _maintenanceVM;

        private readonly ViewModelNavigationStore _navigationStore;
        private readonly ViewModelProvider _viewModelProvider;
        private readonly MachineStatus _machineStatus;
        #endregion
    }
}
