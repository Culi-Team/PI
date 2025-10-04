using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Sequence;
using EQX.UI.Controls;
using log4net;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class OriginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public OriginViewModel(Processes processes, MachineStatus machineStatus,
            INavigationService navigationService)
        {
            Processes = processes;
            MachineStatus = machineStatus;
            _navigationService = navigationService;
            Log = LogManager.GetLogger("OriginVM");
        }

        public Processes Processes { get; }
        public MachineStatus MachineStatus { get; }

        public ICommand SelectAllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Processes.RootProcess.Childs!.ToList().ForEach(p => p.IsOriginOrInitSelected = true);
                });
            }
        }

        public ICommand UnSelectAllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Processes.RootProcess.Childs!.ToList().ForEach(p => p.IsOriginOrInitSelected = false);
                });
            }
        }

        public ICommand OriginCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_AreYouSureYouWantToSetMachineOrigin"], (string)Application.Current.Resources["str_Confirm"]) == false)
                    {
                        return;
                    }
                    Log.Debug("Origin Button Click");
                    MachineStatus.OPCommand = EOperationCommand.Origin;
                });
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Log.Debug("Stop Button Click");
                    MachineStatus.OPCommand = EOperationCommand.Stop;
                });
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<AutoViewModel>();
                });
            }
        }
    }
}
