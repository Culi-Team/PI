using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Sequence;
using EQX.Motion;
using EQX.UI.Controls;
using log4net;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using PIFilmAutoDetachCleanMC.Process;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class AutoViewModel : ViewModelBase
    {
        #region Constructor
        public AutoViewModel(MachineStatus machineStatus,
            INavigationService navigationService,
            CassetteList cassetteList,
            Devices devices,
            DieHardK180Plasma plasma)
        {
            MachineStatus = machineStatus;
            _navigationService = navigationService;
            CassetteList = cassetteList;
            Devices = devices;
            Plasma = plasma;
            MachineStatus.PropertyChanged += MachineStatusOnPropertyChanged;

            Log = LogManager.GetLogger("AutoVM");
        }
        #endregion

        #region Properties
        public MachineStatus MachineStatus { get; }
        public CassetteList CassetteList { get; }
        public Devices Devices { get; }
        public DieHardK180Plasma Plasma { get; }

        public bool IsInputStop
        {
            get => _isInputStop;
            set
            {
                if (_isInputStop != value)
                {
                    _isInputStop = value;
                    OnPropertyChanged(nameof(IsInputStop));
                }
            }
        }

        public bool IsOutputStop
        {
            get => _isOutputStop;
            set
            {
                if (_isOutputStop != value)
                {
                    _isOutputStop = value;
                    OnPropertyChanged(nameof(IsOutputStop));
                }
            }
        }
        public string MachineRunModeDisplay => MachineStatus.MachineRunModeDisplay;
        #endregion

        private void MachineStatusOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MachineStatus.MachineRunMode))
            {
                OnPropertyChanged(nameof(MachineRunModeDisplay));
            }
        }

        #region Commands
        public ICommand SelectRunModeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var selected = RunModeDialog.ShowRunModeDialog(Enum.GetValues<EMachineRunMode>());
                    if (selected.HasValue)
                    {
                        MachineStatus.MachineRunMode = selected.Value;
                    }
                });
            }
        }

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

        public ICommand InitializeNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<InitializeViewModel>();
                });
            }
        }

        public ICommand InputStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (IsInputStop == false)
                    {
                        IsInputStop = true;
                        LogManager.GetLogger("ENABLE STOP INTPUT!");
                    }
                    else
                    {
                        IsInputStop = false;
                        LogManager.GetLogger("DISABLE STOP INTPUT!");
                    }
                });
            }
        }

        public ICommand OutputStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (IsOutputStop == false)
                    {
                        IsOutputStop = true;
                        LogManager.GetLogger("ENABLE STOP OUTPUT!");
                    }
                    else
                    {
                        IsOutputStop = false;
                        LogManager.GetLogger("DISABLE STOP OUTPUT!");
                    }
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

        public ICommand StartCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_AreYouSureYouWantToStartMachine"], (string)Application.Current.Resources["str_Confirm"]) == false)
                    {
                        return;
                    }
                    Log.Debug("Start Button Click");
                    MachineStatus.OPCommand = EOperationCommand.Start;
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
        #endregion

        #region Privates
        private readonly INavigationService _navigationService;
        private bool _isInputStop;
        private bool _isOutputStop;
        #endregion

    }
}
