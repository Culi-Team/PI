using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Sequence;
using EQX.UI.Controls;
using log4net;
using OpenCvSharp.Dnn;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Process;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class InitializeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public InitializeViewModel(Devices devices,
            INavigationService navigationService,
            MachineStatus machineStatus,
            Processes processes)
        {
            Devices = devices;
            _navigationService = navigationService;
            MachineStatus = machineStatus;
            Processes = processes;
            Log = LogManager.GetLogger("InitializeVM");
        }

        public Devices Devices { get; }
        public MachineStatus MachineStatus { get; }
        public Processes Processes { get; }

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

        public ICommand InitializeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if(MessageBoxEx.ShowDialog("Are you sure you want Initialize Machine") == false)
                    {
                        return;
                    }
                    if (Devices.Motions.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        return;
                    }

                    if (MachineStatus.OriginDone == false)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_MachineNeedToBeOriginBeforeRun"]);
                        return;
                    }

                    Processes.RootProcess.Sequence = ESequence.Ready;

                    foreach (var process in Processes.RootProcess.Childs!)
                    {
                        process.ProcessStatus = EProcessStatus.None;
                        process.Sequence = ESequence.Ready;
                    }

                    Processes.RootProcess.ProcessMode = EProcessMode.ToRun;
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
