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
                var currentScreen = _machineStatus.ActiveScreen;
                var newScreen = currentScreen == EScreen.RightScreen ? EScreen.LeftScreen : EScreen.RightScreen;
                _machineStatus.ActiveScreen = newScreen;
            });
        }

        public MaintenanceViewModel(MachineStatus machineStatus)
        {
            _machineStatus = machineStatus;
        }

        private readonly MachineStatus _machineStatus;
    }
}