using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class MaintenanceViewModel : ViewModelBase
    {
        public ICommand SwitchActiveScreen
        {
            get => new RelayCommand<string>((screenName) =>
            {
                var confirm = MessageBoxEx.ShowDialog($"Machine controlled by another screen.\r\nCheck safety before taking control.");

                if (confirm != true) return;

                if (_machineStatus.ActiveScreen == Defines.EScreen.RightScreen)
                {
                    _machineStatus.ActiveScreen = Defines.EScreen.LeftScreen;
                }
                else
                {
                    _machineStatus.ActiveScreen = Defines.EScreen.RightScreen;
                }
            });
        }

        public MaintenanceViewModel(MachineStatus machineStatus)
        {
            _machineStatus = machineStatus;
        }

        private readonly MachineStatus _machineStatus;
    }
}