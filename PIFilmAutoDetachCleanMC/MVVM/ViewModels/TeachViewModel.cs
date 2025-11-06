using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Motion;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class TeachViewModel : ViewModelBase
    {
        private System.Timers.Timer _inoutUpdateTimer;

        #region Properties

        public Devices Devices { get; }
        public RecipeList RecipeList;
        public RecipeSelector RecipeSelector;
        public Processes Processes;
        private readonly ViewModelNavigationStore _navigationStore;

        public MachineStatus MachineStatus { get; }

        public ObservableCollection<UnitTeachingViewModel> TeachingUnits { get; }

        private UnitTeachingViewModel selectedTeachingUnit;

        public UnitTeachingViewModel SelectedTeachingUnit
        {
            get 
            {
                return selectedTeachingUnit; 
            }
            set 
            {
                selectedTeachingUnit = value;
                OnPropertyChanged(nameof(SelectedTeachingUnit));
            }
        }

        #endregion

        public void SelectedUnitTeachingOnChanged()
        {
            OnPropertyChanged(nameof(SelectedTeachingUnit));
        }

        public ICommand GetTeachingUnitViewModel
        {
            get
            {
                return new RelayCommand<object>((teachingUnit) =>
                {
                    if(teachingUnit is UnitTeachingViewModel unitTeachingViewModel)
                    {
                        if (SelectedTeachingUnit.Name == unitTeachingViewModel.Name) return;

                        SelectedTeachingUnit.IsSelected = false;
                        SelectedTeachingUnit = unitTeachingViewModel;
                        SelectedTeachingUnit.IsSelected = true;
                        SelectedUnitTeachingOnChanged();
                    }
                });
            }
        }
        public TeachViewModel(Devices devices,
            RecipeList recipeList,
            RecipeSelector recipeSelector,
            Processes processes,
            MachineStatus machineStatus,
            ViewModelNavigationStore navigationStore)
        {
            Devices = devices;
            RecipeList = recipeList;
            Processes = processes;
            RecipeSelector = recipeSelector;
            MachineStatus = machineStatus;
            _navigationStore = navigationStore;

            #region INIT UNIT TEACHING
            // Initialize Unit Teachings
            UnitTeachingViewModel CSTLoadUnitTeaching = new UnitTeachingViewModel("CST Load", recipeSelector);
            CSTLoadUnitTeaching.Cylinders = Devices.GetInWorkConveyorCylinders();
            CSTLoadUnitTeaching.Motions = Devices.GetCSTLoadMotions();
            CSTLoadUnitTeaching.Inputs = Devices.GetInWorkConveyorInputs();
            CSTLoadUnitTeaching.Outputs = Devices.GetInWorkConveyorOutputs();
            CSTLoadUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe;
            CSTLoadUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InputCassetteStageImage");

            CSTUnloadTeachingViewModel CSTUnloadUnitTeaching = new CSTUnloadTeachingViewModel("CST Unload", recipeSelector);
            CSTUnloadUnitTeaching.Cylinders = Devices.GetOutWorkConveyorCylinders();
            CSTUnloadUnitTeaching.Motions = Devices.GetCSTUnloadMotions();
            CSTUnloadUnitTeaching.Inputs = Devices.GetOutWorkConveyorInputs();
            CSTUnloadUnitTeaching.Outputs = Devices.GetOutWorkConveyorOutputs();
            CSTUnloadUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe;
            CSTUnloadUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InputCassetteStageImage");

            VinylCleanTeachingViewModel VinylCleanUnitTeaching = new VinylCleanTeachingViewModel("Vinyl Clean",recipeSelector);
            VinylCleanUnitTeaching.Cylinders = Devices.GetVinylCleanCylinders();
            VinylCleanUnitTeaching.Inputs = devices.GetVinylCleanInputs();
            VinylCleanUnitTeaching.Outputs= devices.GetVinylCleanOutputs();
            VinylCleanUnitTeaching.Motions = new ObservableCollection<IMotion> {Devices.VinylCleanEncoder };
            VinylCleanUnitTeaching.UnWinder = Devices.TorqueControllers.VinylCleanUnWinder;
            VinylCleanUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.VinylCleanRecipe;
            VinylCleanUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("VinylCleanImage");

            UnitTeachingViewModel TransferFixtureUnitTeaching = new UnitTeachingViewModel("Transfer Fixture", recipeSelector);
            TransferFixtureUnitTeaching.Cylinders = Devices.GetTransferFixtureCylinders();
            TransferFixtureUnitTeaching.Motions = Devices.GetTransferFixtureMotions();
            TransferFixtureUnitTeaching.Inputs = Devices.GetTransferFixtureInputs();
            TransferFixtureUnitTeaching.Outputs = Devices.GetTransferFixtureOutputs();
            TransferFixtureUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferFixtureRecipe;
            TransferFixtureUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferFixtureImage");

            UnitTeachingViewModel DetachUnitTeaching = new UnitTeachingViewModel("Detach",recipeSelector);
            DetachUnitTeaching.Cylinders = Devices.GetDetachCylinders();
            DetachUnitTeaching.Motions = Devices.GetDetachMotions();
            DetachUnitTeaching.Inputs = Devices.GetDetachInputs();
            DetachUnitTeaching.Outputs = Devices.GetDetachOutputs();
            DetachUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.DetachRecipe;
            DetachUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("DetachImage");

            UnitTeachingViewModel GlassTransferUnitTeaching = new UnitTeachingViewModel("Glass Transfer",recipeSelector);
            GlassTransferUnitTeaching.Cylinders = Devices.GetGlassTransferCylinders();
            GlassTransferUnitTeaching.Motions = Devices.GetGlassTransferMotions();
            GlassTransferUnitTeaching.Inputs = Devices.GetGlassTransferInputs();
            GlassTransferUnitTeaching.Outputs = Devices.GetGlassTransferOutputs();
            GlassTransferUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.GlassTransferRecipe;
            GlassTransferUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("GlassTransferImage");

            UnitTeachingViewModel TransferInShuttleLeftUnitTeaching = new UnitTeachingViewModel("Transfer In Shuttle Left", recipeSelector);
            TransferInShuttleLeftUnitTeaching.Cylinders = Devices.GetTransferInShuttleLeftCylinders();
            TransferInShuttleLeftUnitTeaching.Motions = Devices.GetTransferInShuttleLeftMotions();
            TransferInShuttleLeftUnitTeaching.Inputs = Devices.GetTransferInShuttleLeftInputs();
            TransferInShuttleLeftUnitTeaching.Outputs = Devices.GetTransferInShuttleLeftOutputs();
            TransferInShuttleLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe;
            TransferInShuttleLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage");

            UnitTeachingViewModel TransferInShuttleRightUnitTeaching = new UnitTeachingViewModel("Transfer In Shuttle Right", recipeSelector);
            TransferInShuttleRightUnitTeaching.Cylinders = Devices.GetTransferInShuttleRightCylinders();
            TransferInShuttleRightUnitTeaching.Motions = Devices.GetTransferInShuttleRightMotions();
            TransferInShuttleRightUnitTeaching.Inputs = Devices.GetTransferInShuttleRightInputs();
            TransferInShuttleRightUnitTeaching.Outputs = Devices.GetTransferInShuttleRightOutputs();
            TransferInShuttleRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe;
            TransferInShuttleRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage");

            CleanUnitTeachingViewModel WETCleanLeftUnitTeaching = new CleanUnitTeachingViewModel("WET Clean Left", recipeSelector);
            WETCleanLeftUnitTeaching.Cylinders = Devices.GetWETCleanLeftCylinders();
            WETCleanLeftUnitTeaching.Motions = Devices.GetWETCleanLeftMotions();
            WETCleanLeftUnitTeaching.Inputs = Devices.GetWETCleanLeftInputs();
            WETCleanLeftUnitTeaching.Outputs = Devices.GetWETCleanLeftOutputs();
            WETCleanLeftUnitTeaching.SyringePump = Devices.SyringePumps.WetCleanLeftSyringePump;
            WETCleanLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe;
            WETCleanLeftUnitTeaching.Winder = Devices.TorqueControllers.WETCleanLeftWinder;
            WETCleanLeftUnitTeaching.UnWinder = Devices.TorqueControllers.WETCleanLeftUnWinder;
            WETCleanLeftUnitTeaching.Regulator = Devices.Regulators.WetCleanLRegulator;
            WETCleanLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            CleanUnitTeachingViewModel WETCleanRightUnitTeaching = new CleanUnitTeachingViewModel("WET Clean Right", recipeSelector);
            WETCleanRightUnitTeaching.Cylinders = Devices.GetWETCleanRightCylinders();
            WETCleanRightUnitTeaching.Motions = Devices.GetWETCleanRightMotions();
            WETCleanRightUnitTeaching.Inputs = Devices.GetWETCleanRightInputs();
            WETCleanRightUnitTeaching.Outputs = Devices.GetWETCleanRightOutputs();
            WETCleanRightUnitTeaching.SyringePump = Devices.SyringePumps.WetCleanRightSyringePump;
            WETCleanRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.WetCleanRightRecipe;
            WETCleanRightUnitTeaching.Winder = Devices.TorqueControllers.WETCleanRightWinder;
            WETCleanRightUnitTeaching.UnWinder = Devices.TorqueControllers.WETCleanRightUnWinder;
            WETCleanRightUnitTeaching.Regulator = Devices.Regulators.WetCleanRRegulator;
            WETCleanRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            UnitTeachingViewModel TransferRotationLeftUnitTeaching = new UnitTeachingViewModel("Transfer Rotation Left", recipeSelector);
            TransferRotationLeftUnitTeaching.Cylinders = Devices.GetTransferRotationLeftCylinders();
            TransferRotationLeftUnitTeaching.Motions = Devices.GetTransferRotationLeftMotions();
            TransferRotationLeftUnitTeaching.Inputs = Devices.GetTransferRotationLeftInputs();
            TransferRotationLeftUnitTeaching.Outputs = Devices.GetTransferRotationLeftOutputs();
            TransferRotationLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe;
            TransferRotationLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage");

            UnitTeachingViewModel TransferRotationRightUnitTeaching = new UnitTeachingViewModel("Transfer Rotation Right", recipeSelector);
            TransferRotationRightUnitTeaching.Cylinders = Devices.GetTransferRotationRightCylinders();
            TransferRotationRightUnitTeaching.Motions = Devices.GetTransferRotationRightMotions();
            TransferRotationRightUnitTeaching.Inputs = Devices.GetTransferRotationRightInputs();
            TransferRotationRightUnitTeaching.Outputs = Devices.GetTransferRotationRightOutputs();
            TransferRotationRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferRotationRightRecipe;
            TransferRotationRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage");

            CleanUnitTeachingViewModel AFCleanLeftUnitTeaching = new CleanUnitTeachingViewModel("AF Clean Left", recipeSelector);
            AFCleanLeftUnitTeaching.Cylinders = Devices.GetAFCleanLeftCylinders();
            AFCleanLeftUnitTeaching.Motions = Devices.GetAFCleanLeftMotions();
            AFCleanLeftUnitTeaching.Inputs = Devices.GetAFCleanLeftInputs();
            AFCleanLeftUnitTeaching.Outputs = Devices.GetAFCleanLeftOutputs();
            AFCleanLeftUnitTeaching.SyringePump = Devices.SyringePumps.AfCleanLeftSyringePump;
            AFCleanLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe;
            AFCleanLeftUnitTeaching.Winder = Devices.TorqueControllers.AFCleanLeftWinder;
            AFCleanLeftUnitTeaching.UnWinder = Devices.TorqueControllers.AFCleanLeftUnWinder;
            AFCleanLeftUnitTeaching.Regulator = Devices.Regulators.AfCleanLRegulator;
            AFCleanLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            CleanUnitTeachingViewModel AFCleanRightUnitTeaching = new CleanUnitTeachingViewModel("AF Clean Right", recipeSelector);
            AFCleanRightUnitTeaching.Cylinders = Devices.GetAFCleanRightCylinders();
            AFCleanRightUnitTeaching.Motions = Devices.GetAFCleanRightMotions();
            AFCleanRightUnitTeaching.Inputs = Devices.GetAFCleanRightInputs();
            AFCleanRightUnitTeaching.Outputs = Devices.GetAFCleanRightOutputs();
            AFCleanRightUnitTeaching.SyringePump = Devices.SyringePumps.AfCleanRightSyringePump;
            AFCleanRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.AfCleanRightRecipe;
            AFCleanRightUnitTeaching.Winder = Devices.TorqueControllers.AFCleanRightWinder;
            AFCleanRightUnitTeaching.UnWinder = Devices.TorqueControllers.AFCleanRightUnWinder;
            AFCleanRightUnitTeaching.Regulator = Devices.Regulators.AfCleanRRegulator;
            AFCleanRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            UnitTeachingViewModel UnloadTransferLeftUnitTeaching = new UnitTeachingViewModel("Unload Transfer Left", recipeSelector);
            UnloadTransferLeftUnitTeaching.Cylinders = Devices.GetUnloadTransferLeftCylinders();
            UnloadTransferLeftUnitTeaching.Motions = Devices.GetUnloadTransferLeftMotions();
            UnloadTransferLeftUnitTeaching.Inputs = Devices.GetUnloadTransferLeftInputs();
            UnloadTransferLeftUnitTeaching.Outputs = Devices.GetUnloadTransferLeftOutputs();
            UnloadTransferLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe;
            UnloadTransferLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage");

            UnitTeachingViewModel UnloadTransferRightUnitTeaching = new UnitTeachingViewModel("Unload Transfer Right", recipeSelector);
            UnloadTransferRightUnitTeaching.Cylinders = Devices.GetUnloadTransferRightCylinders();
            UnloadTransferRightUnitTeaching.Motions = Devices.GetUnloadTransferRightMotions();
            UnloadTransferRightUnitTeaching.Inputs = Devices.GetUnloadTransferRightInputs();
            UnloadTransferRightUnitTeaching.Outputs = Devices.GetUnloadTransferRightOutputs();
            UnloadTransferRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe;
            UnloadTransferRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage");


            TeachingUnits = new ObservableCollection<UnitTeachingViewModel>()
            {
                CSTLoadUnitTeaching,
                CSTUnloadUnitTeaching,
                VinylCleanUnitTeaching,
                TransferFixtureUnitTeaching,
                DetachUnitTeaching,
                GlassTransferUnitTeaching,

                TransferInShuttleLeftUnitTeaching,
                TransferInShuttleRightUnitTeaching,

                WETCleanLeftUnitTeaching,
                WETCleanRightUnitTeaching,

                TransferRotationLeftUnitTeaching,
                TransferRotationRightUnitTeaching,
                
                AFCleanLeftUnitTeaching,
                AFCleanRightUnitTeaching,

                UnloadTransferLeftUnitTeaching,
                UnloadTransferRightUnitTeaching
            };
            #endregion

            SelectedTeachingUnit = TeachingUnits.First();

            _inoutUpdateTimer = new System.Timers.Timer(100);
            _inoutUpdateTimer.Elapsed += _inoutUpdateTimer_Elapsed;
            _inoutUpdateTimer.Start();
        }

        private void _inoutUpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (_navigationStore.CurrentViewModel.GetType() != typeof(TeachViewModel)) return;

            if (SelectedTeachingUnit == null) return;
            if (SelectedTeachingUnit.Inputs == null) return;
            if (SelectedTeachingUnit.Inputs.Count <= 0) return;

            foreach (var input in SelectedTeachingUnit.Inputs)
            {
                input.RaiseValueUpdated();
            }
        }
    }

}
