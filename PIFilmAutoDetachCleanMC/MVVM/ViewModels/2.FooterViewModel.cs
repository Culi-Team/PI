using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Factories;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class FooterViewModel : ViewModelBase
    {
        public ViewModelBase NavigateVM { get; set; }

        /// <summary>
        /// Is InitDeinitView currently be displayed?
        /// </summary>
        public bool IsInitializing => _viewModelNavigationStore?.CurrentViewModel?.GetType() != typeof(InitDeinitViewModel);

        public FooterViewModel(ViewModelNavigationStore viewModelNavigationStore,
            ViewModelProvider viewModelProvider)
        {
            _viewModelNavigationStore = viewModelNavigationStore;
            _viewModelProvider = viewModelProvider;

            _viewModelNavigationStore.CurrentViewModelChanged += _viewModelNavigationStore_CurrentViewModelChanged;

            NavigateVM = _viewModelProvider.GetViewModel<NavigateMenuViewModel>();
        }

        private void _viewModelNavigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(IsInitializing));
        }

        private readonly ViewModelNavigationStore _viewModelNavigationStore;
        private readonly ViewModelProvider _viewModelProvider;
    }
}
