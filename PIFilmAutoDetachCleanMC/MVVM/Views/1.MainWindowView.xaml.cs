using EQX.Core.Common;
using EQX.UI.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using PIFilmAutoDetachCleanMC.Factories;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        private readonly INavigationService _navigationService;
        private readonly ViewModelProvider _viewModelProvider;
        private readonly MachineStatus _machineStatus;

        public MainWindowView(INavigationService navigationService,
            ViewModelProvider viewModelProvider,
            MachineStatus machineStatus)
        {
            _navigationService = navigationService;
            _viewModelProvider = viewModelProvider;
            _machineStatus = machineStatus;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (typeof(MainWindowViewModel) != this.DataContext.GetType())
            {
                return;
            }

            var vm = this.DataContext as MainWindowViewModel;

            if (_machineStatus.ActiveScreen != vm.Screen) return;

            _navigationService.NavigateTo<InitDeinitViewModel>();

            _viewModelProvider.GetViewModel<InitDeinitViewModel>().Initialization();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Environment.ExitCode != 100)
            {
                e.Cancel = true;

                _viewModelProvider.GetViewModel<HeaderViewModel>().ApplicationCloseCommand.Execute(null);
            }
        }
    }
}
