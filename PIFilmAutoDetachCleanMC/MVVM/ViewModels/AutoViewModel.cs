using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Sequence;
using EQX.Device.Indicator;
using EQX.UI.Controls;
using log4net;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.ComponentModel;
using System.Windows;
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
            RecipeSelector recipeSelector,
            DieHardK180Plasma plasma,
            CWorkData workData,
            UserStore userStore,
            NEOSHSDIndicator nEOSHSDIndicator)
        {
            MachineStatus = machineStatus;
            _navigationService = navigationService;
            CassetteList = cassetteList;
            Devices = devices;
            RecipeSelector = recipeSelector;
            Plasma = plasma;
            WorkData = workData;
            UserStore = userStore;
            _nEOSHSDIndicator = nEOSHSDIndicator;
            MachineStatus.PropertyChanged += MachineStatusOnPropertyChanged;

            RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CassetteSizeChanged += CassetteSizeChanged_Handler;

            Log = LogManager.GetLogger("AutoVM");

            System.Timers.Timer temperatureUpdateTimer = new System.Timers.Timer(500);

            temperatureUpdateTimer.Elapsed += (s, e) =>
            {
                OnPropertyChanged(nameof(Temperature));
                OnPropertyChanged(nameof(Humidity));
            };

            temperatureUpdateTimer.AutoReset = true;
            temperatureUpdateTimer.Enabled = true;

            System.Timers.Timer statusUpdateTimer = new System.Timers.Timer(100);
            statusUpdateTimer.Elapsed +=StatusUpdateTimerHandler;
            statusUpdateTimer.AutoReset = true;
            statusUpdateTimer.Enabled = true;
        }
        #endregion

        private void StatusUpdateTimerHandler(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Devices.Inputs.EmoLoadL.RaiseValueUpdated();
            Devices.Inputs.EmoLoadR.RaiseValueUpdated();
            Devices.Inputs.OpLEmo.RaiseValueUpdated();
            Devices.Inputs.OpREmo.RaiseValueUpdated();
            Devices.Inputs.EmoUnloadL.RaiseValueUpdated();
            Devices.Inputs.EmoUnloadR.RaiseValueUpdated();

            Devices.Inputs.DoorLock1L.RaiseValueUpdated();
            Devices.Inputs.DoorLock1R.RaiseValueUpdated();
            Devices.Inputs.DoorLock2L.RaiseValueUpdated();
            Devices.Inputs.DoorLock2R.RaiseValueUpdated();
            Devices.Inputs.DoorLock3L.RaiseValueUpdated();
            Devices.Inputs.DoorLock3R.RaiseValueUpdated();
            Devices.Inputs.DoorLock4L.RaiseValueUpdated();
            Devices.Inputs.DoorLock4R.RaiseValueUpdated();
            Devices.Inputs.DoorLock5L.RaiseValueUpdated();
            Devices.Inputs.DoorLock5R.RaiseValueUpdated();
            Devices.Inputs.DoorLock6L.RaiseValueUpdated();
            Devices.Inputs.DoorLock6R.RaiseValueUpdated();
            Devices.Inputs.DoorLock7L.RaiseValueUpdated();
            Devices.Inputs.DoorLock7R.RaiseValueUpdated();

            Devices.Inputs.InCstDetect1.RaiseValueUpdated();
            Devices.Inputs.InCstDetect2.RaiseValueUpdated();

            Devices.Inputs.InCstWorkDetect1.RaiseValueUpdated();
            Devices.Inputs.InCstWorkDetect2.RaiseValueUpdated();
            Devices.Inputs.InCstWorkDetect3.RaiseValueUpdated();
            Devices.Inputs.InCstWorkDetect4.RaiseValueUpdated();

            Devices.Inputs.BufferCstDetect1.RaiseValueUpdated();
            Devices.Inputs.BufferCstDetect2.RaiseValueUpdated();

            Devices.Inputs.OutCstWorkDetect1.RaiseValueUpdated();
            Devices.Inputs.OutCstWorkDetect2.RaiseValueUpdated();
            Devices.Inputs.OutCstWorkDetect3.RaiseValueUpdated();

            Devices.Inputs.OutCstDetect1.RaiseValueUpdated();
            Devices.Inputs.OutCstDetect2.RaiseValueUpdated();

            Devices.Inputs.VinylCleanFixtureDetect.RaiseValueUpdated();
            Devices.Inputs.AlignFixtureDetect.RaiseValueUpdated();
            Devices.Inputs.DetachFixtureDetect.RaiseValueUpdated();
            Devices.Inputs.RemoveZoneFixtureDetect.RaiseValueUpdated();

            Devices.Inputs.DetachGlassShtVac1.RaiseValueUpdated();
            Devices.Inputs.DetachGlassShtVac2.RaiseValueUpdated();
            Devices.Inputs.DetachGlassShtVac3.RaiseValueUpdated();

            Devices.Inputs.AlignStageLGlassDetect1.RaiseValueUpdated();
            Devices.Inputs.AlignStageLGlassDetect2.RaiseValueUpdated();
            Devices.Inputs.AlignStageLGlassDetect3.RaiseValueUpdated();

            Devices.Inputs.AlignStageRGlassDetect1.RaiseValueUpdated();
            Devices.Inputs.AlignStageRGlassDetect2.RaiseValueUpdated();
            Devices.Inputs.AlignStageRGlassDetect3.RaiseValueUpdated();

            Devices.Inputs.InShuttleLVac.RaiseValueUpdated();
            Devices.Inputs.InShuttleRVac.RaiseValueUpdated();
            Devices.Inputs.OutShuttleLVac.RaiseValueUpdated();
            Devices.Inputs.OutShuttleRVac.RaiseValueUpdated();

            Devices.Inputs.TrRotateLeftVac1.RaiseValueUpdated();
            Devices.Inputs.TrRotateLeftVac2.RaiseValueUpdated();

            Devices.Inputs.TrRotateRightVac1.RaiseValueUpdated();
            Devices.Inputs.TrRotateRightVac2.RaiseValueUpdated();

            Devices.Inputs.UnloadGlassAlignVac1.RaiseValueUpdated();
            Devices.Inputs.UnloadGlassDetect1.RaiseValueUpdated();

            Devices.Inputs.UnloadGlassAlignVac2.RaiseValueUpdated();
            Devices.Inputs.UnloadGlassDetect2.RaiseValueUpdated();

            Devices.Inputs.UnloadGlassAlignVac3.RaiseValueUpdated();
            Devices.Inputs.UnloadGlassDetect3.RaiseValueUpdated();  

            Devices.Inputs.UnloadGlassAlignVac4.RaiseValueUpdated();
            Devices.Inputs.UnloadGlassDetect4.RaiseValueUpdated();

            Devices.Inputs.UnloadRobotVac1.RaiseValueUpdated();
            Devices.Inputs.UnloadRobotDetect1.RaiseValueUpdated();

            Devices.Inputs.UnloadRobotVac2.RaiseValueUpdated();
            Devices.Inputs.UnloadRobotDetect3.RaiseValueUpdated();

            Devices.Inputs.UnloadRobotVac3.RaiseValueUpdated();
            Devices.Inputs.UnloadRobotDetect3.RaiseValueUpdated();

            Devices.Inputs.UnloadRobotVac4.RaiseValueUpdated();
            Devices.Inputs.UnloadRobotDetect4.RaiseValueUpdated();
        }

        #region Properties
        public MachineStatus MachineStatus { get; }
        public CassetteList CassetteList { get; }
        public Devices Devices { get; }
        public RecipeSelector RecipeSelector { get; }
        public DieHardK180Plasma Plasma { get; }
        public CWorkData WorkData { get; }
        public UserStore UserStore { get; }

        public string MachineRunModeDisplay => MachineStatus.MachineRunModeDisplay;
        public double Temperature => _nEOSHSDIndicator.Temperature;
        public double Humidity => _nEOSHSDIndicator.Humidity;
        #endregion

        private void MachineStatusOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MachineStatus.MachineRunMode))
            {
                OnPropertyChanged(nameof(MachineRunModeDisplay));
            }
        }

        private void CassetteSizeChanged_Handler(object? sender, EventArgs e)
        {
            if (sender is CSTLoadUnloadRecipe cstLoadUnloadRecipe == false) return;
            CassetteList.CassetteIn.Rows = cstLoadUnloadRecipe.CasetteRows;
            CassetteList.CassetteIn.Columns = 1;
            CassetteList.CassetteIn.GenerateCells();

            CassetteList.CassetteOut.Rows = cstLoadUnloadRecipe.CasetteRows;
            CassetteList.CassetteOut.Columns = 1;
            CassetteList.CassetteOut.GenerateCells();

            CassetteList.SubscribeCellClickedEvent();
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
                    if (MachineStatus.IsInputStop == false)
                    {
                        MachineStatus.IsInputStop = true;
                        Log.Debug("ENABLE STOP INTPUT!");
                    }
                    else
                    {
                        MachineStatus.IsInputStop = false;
                        Log.Debug("DISABLE STOP INTPUT!");
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
                    if (MachineStatus.IsOutputStop == false)
                    {
                        MachineStatus.IsOutputStop = true;
                        Log.Debug("ENABLE STOP OUTPUT!");
                    }
                    else
                    {
                        MachineStatus.IsOutputStop = false;
                        Log.Debug("DISABLE STOP OUTPUT!");
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
                    _navigationService.NavigateTo<OriginViewModel>();
                });
            }
        }

        public ICommand StartCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var messageKey = "str_AreYouSureYouWantToStartMachine";

                    if (MachineStatus.IsDryRunMode)
                    {
                        messageKey = "str_AreYouSureYouWantToStartMachineDryRun";
                    }
                    //else if (MachineStatus.IsByPassMode)
                    //{
                    //    messageKey = "str_AreYouSureYouWantToStartMachineByPass";
                    //}

                    if (MessageBoxEx.ShowDialog((string)Application.Current.Resources[messageKey], (string)Application.Current.Resources["str_Confirm"]) == false)
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

        public ICommand ResetWorkDataCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (MessageBoxEx.ShowDialog("Are you sure you want to reset work data?", "Confirm Reset") == true)
                    {
                        WorkData.Reset();
                        Log.Debug("Work Data Reset");
                    }
                });
            }
        }

        public ICommand ResetCSTOutCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CassetteList.ResetCSTOut();
                });
            }
        }

        public ICommand ResetCSTInCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CassetteList.ResetCSTIn();
                });
            }
        }

        public ICommand DoorOpenCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    bool currentValue = Devices.Outputs.DoorOpen.Value;
                    Devices.Outputs.DoorOpen.Value = !currentValue;
                });
            }
        }

        public ICommand BuzzerOffCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Devices.Outputs.TowerBuzzer.Value = false;
                });
            }
        }
        #endregion

        #region Privates
        private readonly INavigationService _navigationService;
        private readonly NEOSHSDIndicator _nEOSHSDIndicator;
        #endregion
    }
}
