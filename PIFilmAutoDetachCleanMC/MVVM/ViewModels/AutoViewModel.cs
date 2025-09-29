using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Sequence;
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
            AppSettings appSettings,
            CWorkData workData)
        {
            MachineStatus = machineStatus;
            _navigationService = navigationService;
            CassetteList = cassetteList;
            Devices = devices;
            RecipeSelector = recipeSelector;
            Plasma = plasma;
            _appSettings = appSettings;
            WorkData = workData;
            MachineStatus.PropertyChanged += MachineStatusOnPropertyChanged;

            RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CassetteSizeChanged += CassetteSizeChanged_Handler;

            Log = LogManager.GetLogger("AutoVM");
        }
        #endregion

        #region Properties
        public MachineStatus MachineStatus { get; }
        public CassetteList CassetteList { get; }
        public Devices Devices { get; }
        public RecipeSelector RecipeSelector { get; }
        public DieHardK180Plasma Plasma { get; }
        public CWorkData WorkData { get; }

        public string MachineRunModeDisplay => MachineStatus.MachineRunModeDisplay;
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
        #endregion

        #region Privates
        private readonly INavigationService _navigationService;
        private bool _isInputStop;
        private bool _isOutputStop;
        private readonly AppSettings _appSettings;
        #endregion

    }
}
