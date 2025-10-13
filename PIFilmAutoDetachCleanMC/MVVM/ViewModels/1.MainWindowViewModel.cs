using EQX.Core.Common;
using System.Windows;
using System.Windows.Threading;
using PIFilmAutoDetachCleanMC.Factories;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties
        public ViewModelBase HeaderVM { get; }
        public ViewModelBase FooterVM { get; }

        public ViewModelBase CurrentFrameVM => _navigationStore.CurrentViewModel;
        #endregion

        public MainWindowViewModel(ViewModelNavigationStore navigationStore,
                                   ViewModelProvider viewModelProvider)
        {
            _navigationStore = navigationStore;
            _viewModelProvider = viewModelProvider;
            HeaderVM = _viewModelProvider.GetViewModel<HeaderViewModel>();
            FooterVM = _viewModelProvider.GetViewModel<FooterViewModel>();

            _navigationStore.CurrentViewModelChanged += FrameNavigationStore_CurrentViewModelChanged;
        }

        #region Privates methods
        private void FrameNavigationStore_CurrentViewModelChanged()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (CurrentFrameVM is AutoViewModel vm)
                {
                    vm.EnableTimer();
                }
                else
                {
                    _viewModelProvider.GetViewModel<AutoViewModel>().DisableTimer();
                }

                OnPropertyChanged(nameof(CurrentFrameVM));
            }), DispatcherPriority.DataBind);
        }
        #endregion

        #region Private fields
        private readonly ViewModelNavigationStore _navigationStore;
        private readonly ViewModelProvider _viewModelProvider;
        #endregion
    }
}
