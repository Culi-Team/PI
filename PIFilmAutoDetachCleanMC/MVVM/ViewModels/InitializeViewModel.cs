using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Sequence;
using EQX.UI.Controls;
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
        private readonly Processes _processes;

        public InitializeViewModel(ProcessInitSelect processInitSelect,
            Devices devices,
            MachineStatus machineStatus,
            Processes processes)
        {
            ProcessInitSelect = processInitSelect;
            Devices = devices;
            MachineStatus = machineStatus;
            _processes = processes;
        }

        public ProcessInitSelect ProcessInitSelect { get; }
        public Devices Devices { get; }
        public MachineStatus MachineStatus { get; }

        public ICommand SelectAllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var props = ProcessInitSelect.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(bool) && prop.CanWrite)
                        {
                            prop.SetValue(ProcessInitSelect, true);
                        }
                    }
                });
            }
        }

        public ICommand UnSelectAllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var props = ProcessInitSelect.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(bool) && prop.CanWrite)
                        {
                            prop.SetValue(ProcessInitSelect, false);
                        }
                    }
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
                    if (Devices.MotionsAjin.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        return;
                    }

                    if (Devices.MotionsInovance.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        return;
                    }

                    if (MachineStatus.OriginDone == false)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_MachineNeedToBeOriginBeforeRun"]);
                        return;
                    }

                    _processes.RootProcess.Sequence = ESequence.Ready;

                    foreach (var process in _processes.RootProcess.Childs!)
                    {
                        process.ProcessStatus = EProcessStatus.None;
                        process.Sequence = ESequence.Ready;
                    }

                    _processes.RootProcess.ProcessMode = EProcessMode.ToRun;
                });
            }
        }
    }
}
