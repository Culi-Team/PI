using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.InOut;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Cylinder;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class TeachViewModel : ViewModelBase
    {        
        #region Properties
        public Devices Devices { get; }
        public MachineStatus MachineStatus { get; }
        public RecipeList RecipeList;
        public RecipeSelector RecipeSelector;
        public Processes Processes;
        public DataViewModel DataViewModel { get; }
        public ObservableCollection<IProcess<ESequence>> ProcessListTeaching => GetProcessList();
        private IProcess<ESequence> _selectedProcess;
        public IProcess<ESequence> SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
                SelectedPropertyProcess();
            }
        }
        private ObservableCollection<ICylinder> _cylinders;
        public ObservableCollection<ICylinder> Cylinders
        {
            get { return _cylinders; }
            set { _cylinders = value; OnPropertyChanged(); }
        }
        private ObservableCollection<IMotion> _motions;
        public ObservableCollection<IMotion> Motions
        {
            get { return _motions; }
            set { _motions = value; OnPropertyChanged(); }
        }
        private ObservableCollection<IDOutput> _outputs;
        public ObservableCollection<IDOutput> Outputs
        {
            get { return _outputs; }
            set { _outputs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<IDInput> _inputs;
        public ObservableCollection<IDInput> Inputs
        {
            get { return _inputs; }
            set { _inputs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<PositionTeaching> _positionTeachings;
        public ObservableCollection<PositionTeaching> PositionTeachings
        {
            get { return _positionTeachings; }
            set { _positionTeachings = value; OnPropertyChanged(); }
        }

        // CSTLoadUnload Tab Motion Properties
        public ObservableCollection<IMotion> InWorkConveyorMotions => GetInWorkConveyorMotions();
        public ObservableCollection<IMotion> OutWorkConveyorMotions => GetOutWorkConveyorMotions();

        // CSTLoadUnload Tab Cylinder Properties
        public ObservableCollection<ICylinder> InWorkConveyorCylinders => GetInWorkConveyorCylinders();
        public ObservableCollection<ICylinder> OutWorkConveyorCylinders => GetOutWorkConveyorCylinders();

        // CSTLoadUnload Tab Input Properties
        public ObservableCollection<IDInput> InWorkConveyorInputs => GetInWorkConveyorInputs();
        public ObservableCollection<IDInput> OutWorkConveyorInputs => GetOutWorkConveyorInputs();

        // CSTLoadUnload Tab Output Properties
        public ObservableCollection<IDOutput> InWorkConveyorOutputs => GetInWorkConveyorOutputs();
        public ObservableCollection<IDOutput> OutWorkConveyorOutputs => GetOutWorkConveyorOutputs();

        // CSTLoadUnload Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> InWorkConveyorPositionTeachings => GetInWorkConveyorPositionTeachings();
        public ObservableCollection<PositionTeaching> OutWorkConveyorPositionTeachings => GetOutWorkConveyorPositionTeachings();

        // Detach Tab Motion Properties
        public ObservableCollection<IMotion> TransferFixtureMotions => GetTransferFixtureMotions();
        public ObservableCollection<IMotion> DetachMotions => GetDetachMotions();

        // Detach Tab Cylinder Properties
        public ObservableCollection<ICylinder> TransferFixtureCylinders => GetTransferFixtureCylinders();
        public ObservableCollection<ICylinder> DetachCylinders => GetDetachCylinders();

        // Detach Tab Input Properties
        public ObservableCollection<IDInput> TransferFixtureInputs => GetTransferFixtureInputs();
        public ObservableCollection<IDInput> DetachInputs => GetDetachInputs();

        // Detach Tab Output Properties
        public ObservableCollection<IDOutput> TransferFixtureOutputs => GetTransferFixtureOutputs();
        public ObservableCollection<IDOutput> DetachOutputs => GetDetachOutputs();

        // Detach Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> TransferFixturePositionTeachings => GetTransferFixturePositionTeachings();
        public ObservableCollection<PositionTeaching> DetachPositionTeachings => GetDetachPositionTeachings();

        // Glass Transfer Tab Motion Properties
        public ObservableCollection<IMotion> GlassTransferMotions => GetGlassTransferMotions();

        // Glass Transfer Tab Cylinder Properties
        public ObservableCollection<ICylinder> GlassTransferCylinders => GetGlassTransferCylinders();

        // Glass Transfer Tab Input Properties
        public ObservableCollection<IDInput> GlassTransferInputs => GetGlassTransferInputs();

        // Glass Transfer Tab Output Properties
        public ObservableCollection<IDOutput> GlassTransferOutputs => GetGlassTransferOutputs();

        // Glass Transfer Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> GlassTransferPositionTeachings => GetGlassTransferPositionTeachings();

        // Transfer Shutter Tab Motion Properties
        public ObservableCollection<IMotion> TransferShutterLeftMotions => GetTransferShutterLeftMotions();
        public ObservableCollection<IMotion> TransferShutterRightMotions => GetTransferShutterRightMotions();

        // Transfer Shutter Tab Cylinder Properties
        public ObservableCollection<ICylinder> TransferShutterLeftCylinders => GetTransferShutterLeftCylinders();
        public ObservableCollection<ICylinder> TransferShutterRightCylinders => GetTransferShutterRightCylinders();

        // Transfer Shutter Tab Input Properties
        public ObservableCollection<IDInput> TransferShutterLeftInputs => GetTransferShutterLeftInputs();
        public ObservableCollection<IDInput> TransferShutterRightInputs => GetTransferShutterRightInputs();

        // Transfer Shutter Tab Output Properties
        public ObservableCollection<IDOutput> TransferShutterLeftOutputs => GetTransferShutterLeftOutputs();
        public ObservableCollection<IDOutput> TransferShutterRightOutputs => GetTransferShutterRightOutputs();

        // Transfer Shutter Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> TransferShutterLeftPositionTeachings => GetTransferShutterLeftPositionTeachings();
        public ObservableCollection<PositionTeaching> TransferShutterRightPositionTeachings => GetTransferShutterRightPositionTeachings();

        // Transfer Rotation Tab Motion Properties
        public ObservableCollection<IMotion> TransferRotationLeftMotions => GetTransferRotationLeftMotions();
        public ObservableCollection<IMotion> TransferRotationRightMotions => GetTransferRotationRightMotions();

        // Transfer Rotation Tab Cylinder Properties
        public ObservableCollection<ICylinder> TransferRotationLeftCylinders => GetTransferRotationLeftCylinders();
        public ObservableCollection<ICylinder> TransferRotationRightCylinders => GetTransferRotationRightCylinders();

        // Transfer Rotation Tab Input Properties
        public ObservableCollection<IDInput> TransferRotationLeftInputs => GetTransferRotationLeftInputs();
        public ObservableCollection<IDInput> TransferRotationRightInputs => GetTransferRotationRightInputs();

        // Transfer Rotation Tab Output Properties
        public ObservableCollection<IDOutput> TransferRotationLeftOutputs => GetTransferRotationLeftOutputs();
        public ObservableCollection<IDOutput> TransferRotationRightOutputs => GetTransferRotationRightOutputs();

        // Transfer Rotation Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> TransferRotationLeftPositionTeachings => GetTransferRotationLeftPositionTeachings();
        public ObservableCollection<PositionTeaching> TransferRotationRightPositionTeachings => GetTransferRotationRightPositionTeachings();

        // Unload Transfer Tab Motion Properties
        public ObservableCollection<IMotion> UnloadTransferLeftMotions => GetUnloadTransferLeftMotions();
        public ObservableCollection<IMotion> UnloadTransferRightMotions => GetUnloadTransferRightMotions();

        // Unload Transfer Tab Cylinder Properties
        public ObservableCollection<ICylinder> UnloadTransferLeftCylinders => GetUnloadTransferLeftCylinders();
        public ObservableCollection<ICylinder> UnloadTransferRightCylinders => GetUnloadTransferRightCylinders();

        // Unload Transfer Tab Input Properties
        public ObservableCollection<IDInput> UnloadTransferLeftInputs => GetUnloadTransferLeftInputs();
        public ObservableCollection<IDInput> UnloadTransferRightInputs => GetUnloadTransferRightInputs();

        // Unload Transfer Tab Output Properties
        public ObservableCollection<IDOutput> UnloadTransferLeftOutputs => GetUnloadTransferLeftOutputs();
        public ObservableCollection<IDOutput> UnloadTransferRightOutputs => GetUnloadTransferRightOutputs();

        // Unload Transfer Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> UnloadTransferLeftPositionTeachings => GetUnloadTransferLeftPositionTeachings();
        public ObservableCollection<PositionTeaching> UnloadTransferRightPositionTeachings => GetUnloadTransferRightPositionTeachings();

        // Clean Tab Motion Properties
        public ObservableCollection<IMotion> WETCleanLeftMotions => GetWETCleanLeftMotions();
        public ObservableCollection<IMotion> WETCleanRightMotions => GetWETCleanRightMotions();
        public ObservableCollection<IMotion> AFCleanLeftMotions => GetAFCleanLeftMotions();
        public ObservableCollection<IMotion> AFCleanRightMotions => GetAFCleanRightMotions();

        // Clean Tab Cylinder Properties
        public ObservableCollection<ICylinder> WETCleanLeftCylinders => GetWETCleanLeftCylinders();
        public ObservableCollection<ICylinder> WETCleanRightCylinders => GetWETCleanRightCylinders();
        public ObservableCollection<ICylinder> AFCleanLeftCylinders => GetAFCleanLeftCylinders();
        public ObservableCollection<ICylinder> AFCleanRightCylinders => GetAFCleanRightCylinders();

        // Clean Tab Input Properties
        public ObservableCollection<IDInput> WETCleanLeftInputs => GetWETCleanLeftInputs();
        public ObservableCollection<IDInput> WETCleanRightInputs => GetWETCleanRightInputs();
        public ObservableCollection<IDInput> AFCleanLeftInputs => GetAFCleanLeftInputs();
        public ObservableCollection<IDInput> AFCleanRightInputs => GetAFCleanRightInputs();

        // Clean Tab Output Properties
        public ObservableCollection<IDOutput> WETCleanLeftOutputs => GetWETCleanLeftOutputs();
        public ObservableCollection<IDOutput> WETCleanRightOutputs => GetWETCleanRightOutputs();
        public ObservableCollection<IDOutput> AFCleanLeftOutputs => GetAFCleanLeftOutputs();
        public ObservableCollection<IDOutput> AFCleanRightOutputs => GetAFCleanRightOutputs();

        // Clean Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> WETCleanLeftPositionTeachings => GetWETCleanLeftPositionTeachings();
        public ObservableCollection<PositionTeaching> WETCleanRightPositionTeachings => GetWETCleanRightPositionTeachings();
        public ObservableCollection<PositionTeaching> AFCleanLeftPositionTeachings => GetAFCleanLeftPositionTeachings();
        public ObservableCollection<PositionTeaching> AFCleanRightPositionTeachings => GetAFCleanRightPositionTeachings();

        #endregion

        #region GetPositionTeaching
        // CSTLoadUnload Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetInWorkConveyorPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.CstLoadUnloadRecipe == null || Devices?.MotionsInovance?.InCassetteTAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_InCstTAxisLoadPosition"]?.ToString() ?? "In Cassette T Axis Load Position", 
                    PropertyName = "InCstTAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisLoadPosition, 
                    Motion = Devices.MotionsInovance.InCassetteTAxis 
                },
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_InCstTAxisWorkPosition"]?.ToString() ?? "In Cassette T Axis Work Position", 
                    PropertyName = "InCstTAxisWorkPosition",
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisWorkPosition, 
                    Motion = Devices.MotionsInovance.InCassetteTAxis 
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetOutWorkConveyorPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.CstLoadUnloadRecipe == null || Devices?.MotionsInovance?.OutCassetteTAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_OutCstTAxisLoadPosition"]?.ToString() ?? "Out Cassette T Axis Load Position", 
                    PropertyName = "OutCstTAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisLoadPosition, 
                    Motion = Devices.MotionsInovance.OutCassetteTAxis 
                },
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_OutCstTAxisWorkPosition"]?.ToString() ?? "Out Cassette T Axis Work Position", 
                    PropertyName = "OutCstTAxisWorkPosition",
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisWorkPosition, 
                    Motion = Devices.MotionsInovance.OutCassetteTAxis 
                }
            };
        }
        // Transfer Fixture LoadUnload PositionTeachings 
        private ObservableCollection<PositionTeaching> GetTransferFixturePositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.TransferFixtureRecipe == null || Devices?.MotionsInovance?.FixtureTransferYAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferFixtureYAxisLoadPosition"]?.ToString() ?? "Transfer Fixture Y Axis Load Position",
                    PropertyName = "TransferFixtureYAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferFixtureRecipe.TransferFixtureYAxisLoadPosition,
                    Motion = Devices.MotionsInovance.FixtureTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferFixtureYAxisUnloadPosition"]?.ToString() ?? "Transfer Fixture Y Axis Unload Position",
                    PropertyName = "TransferFixtureYAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferFixtureRecipe.TransferFixtureYAxisUnloadPosition,
                    Motion = Devices.MotionsInovance.FixtureTransferYAxis
                }
            };
        }

        // Detach Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetDetachPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.DetachRecipe == null ||
                Devices?.MotionsInovance?.DetachGlassZAxis == null ||
                Devices?.MotionsAjin?.ShuttleTransferZAxis == null ||
                Devices?.MotionsInovance?.ShuttleTransferXAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                // Detach Z Axis positions - Ready → Detach Ready → Detach 1 → Detach 2
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_DetachZAxisReadyPosition"]?.ToString() ?? "Detach Z Axis Ready Position",
                    PropertyName = "DetachZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_DetachZAxisDetachReadyPosition"]?.ToString() ?? "Detach Z Axis Detach Ready Position",
                    PropertyName = "DetachZAxisDetachReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetachReadyPosition,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_DetachZAxisDetach1Position"]?.ToString() ?? "Detach Z Axis Detach 1 Position",
                    PropertyName = "DetachZAxisDetach1Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach1Position,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_DetachZAxisDetach2Position"]?.ToString() ?? "Detach Z Axis Detach 2 Position",
                    PropertyName = "DetachZAxisDetach2Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach2Position,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                // Shuttle Transfer Z Axis positions - Ready → Detach Ready → Detach 1 → Detach 2 → Unload
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisReadyPosition"]?.ToString() ?? "Shuttle Transfer Z Axis Ready Position",
                    PropertyName = "ShuttleTransferZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisReadyPosition,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisDetachReadyPosition"]?.ToString() ?? "Shuttle Transfer Z Axis Detach Ready Position",
                    PropertyName = "ShuttleTransferZAxisDetachReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetachReadyPosition,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisDetach1Position"]?.ToString() ?? "Shuttle Transfer Z Axis Detach 1 Position",
                    PropertyName = "ShuttleTransferZAxisDetach1Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetach1Position,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisDetach2Position"]?.ToString() ?? "Shuttle Transfer Z Axis Detach 2 Position",
                    PropertyName = "ShuttleTransferZAxisDetach2Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetach2Position,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisUnloadPosition"]?.ToString() ?? "Shuttle Transfer Z Axis Unload Position",
                    PropertyName = "ShuttleTransferZAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                // Shuttle Transfer X Axis positions - Detach → Detach Check → Unload
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferXAxisDetachPosition"]?.ToString() ?? "Shuttle Transfer X Axis Detach Position",
                    PropertyName = "ShuttleTransferXAxisDetachPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachPosition,
                    Motion = Devices.MotionsInovance.ShuttleTransferXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferXAxisDetachCheckPosition"]?.ToString() ?? "Shuttle Transfer X Axis Detach Check Position",
                    PropertyName = "ShuttleTransferXAxisDetachCheckPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachCheckPosition,
                    Motion = Devices.MotionsInovance.ShuttleTransferXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferXAxisUnloadPosition"]?.ToString() ?? "Shuttle Transfer X Axis Unload Position",
                    PropertyName = "ShuttleTransferXAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisUnloadPosition,
                    Motion = Devices.MotionsInovance.ShuttleTransferXAxis
                }
            };
        }

        // Glass Transfer Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetGlassTransferPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.GlassTransferRecipe == null || 
                Devices?.MotionsInovance?.GlassTransferYAxis == null || 
                Devices?.MotionsInovance?.GlassTransferZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                // Ready Positions: Y, Z
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferYAxisReadyPosition"]?.ToString() ?? "Glass Transfer Y Axis Ready Position",
                    PropertyName = "YAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferZAxisReadyPosition"]?.ToString() ?? "Glass Transfer Z Axis Ready Position",
                    PropertyName = "ZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassTransferZAxis
                },
                // Pick Positions: Y, Z
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferYAxisPickPosition"]?.ToString() ?? "Glass Transfer Y Axis Pick Position",
                    PropertyName = "YAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisPickPosition,
                    Motion = Devices.MotionsInovance.GlassTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferZAxisPickPosition"]?.ToString() ?? "Glass Transfer Z Axis Pick Position",
                    PropertyName = "ZAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisPickPosition,
                    Motion = Devices.MotionsInovance.GlassTransferZAxis
                },
                // Left Place Positions: Y, Z
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferYAxisLeftPlacePosition"]?.ToString() ?? "Glass Transfer Y Axis Left Place Position",
                    PropertyName = "YAxisLeftPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisLeftPlacePosition,
                    Motion = Devices.MotionsInovance.GlassTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferZAxisLeftPlacePosition"]?.ToString() ?? "Glass Transfer Z Axis Left Place Position",
                    PropertyName = "ZAxisLeftPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisLeftPlacePosition,
                    Motion = Devices.MotionsInovance.GlassTransferZAxis
                },
                // Right Place Positions: Y, Z
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferYAxisRightPlacePosition"]?.ToString() ?? "Glass Transfer Y Axis Right Place Position",
                    PropertyName = "YAxisRightPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisRightPlacePosition,
                    Motion = Devices.MotionsInovance.GlassTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferZAxisRightPlacePosition"]?.ToString() ?? "Glass Transfer Z Axis Right Place Position",
                    PropertyName = "ZAxisRightPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisRightPlacePosition,
                    Motion = Devices.MotionsInovance.GlassTransferZAxis
                }
            };
        }

        // Transfer Shutter Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetTransferShutterLeftPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.TransferInShuttleLeftRecipe == null || 
                Devices?.MotionsInovance?.TransferInShuttleLYAxis == null || 
                Devices?.MotionsInovance?.TransferInShuttleLZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisReadyPosition"]?.ToString() ?? "Transfer In Shuttle Y Axis Ready Position",
                    PropertyName = "TransferInShuttleLeftYAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisReadyPosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleZAxisReadyPosition"]?.ToString() ?? "Transfer In Shuttle Z Axis Ready Position",
                    PropertyName = "TransferInShuttleLeftZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPickPosition1"]?.ToString() ?? "Transfer In Shuttle Y Axis Pick Position 1",
                    PropertyName = "TransferInShuttleLeftYAxisPickPosition1",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPickPosition1,
                    Motion = Devices.MotionsInovance.TransferInShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPickPosition2"]?.ToString() ?? "Transfer In Shuttle Y Axis Pick Position 2",
                    PropertyName = "TransferInShuttleLeftYAxisPickPosition2",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPickPosition2,
                    Motion = Devices.MotionsInovance.TransferInShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPickPosition3"]?.ToString() ?? "Transfer In Shuttle Y Axis Pick Position 3",
                    PropertyName = "TransferInShuttleLeftYAxisPickPosition3",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPickPosition3,
                    Motion = Devices.MotionsInovance.TransferInShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleZAxisPickPosition"]?.ToString() ?? "Transfer In Shuttle Z Axis Pick Position",
                    PropertyName = "TransferInShuttleLeftZAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisPickPosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPlacePosition"]?.ToString() ?? "Transfer In Shuttle Y Axis Place Position",
                    PropertyName = "TransferInShuttleLeftYAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPlacePosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleZAxisPlacePosition"]?.ToString() ?? "Transfer In Shuttle Z Axis Place Position",
                    PropertyName = "TransferInShuttleLeftZAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisPlacePosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleLZAxis
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetTransferShutterRightPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.TransferInShuttleRightRecipe == null || 
                Devices?.MotionsInovance?.TransferInShuttleRYAxis == null || 
                Devices?.MotionsInovance?.TransferInShuttleRZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisReadyPosition"]?.ToString() ?? "Transfer In Shuttle Y Axis Ready Position",
                    PropertyName = "TransferInShuttleRightYAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisReadyPosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleZAxisReadyPosition"]?.ToString() ?? "Transfer In Shuttle Z Axis Ready Position",
                    PropertyName = "TransferInShuttleRightZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPickPosition1"]?.ToString() ?? "Transfer In Shuttle Y Axis Pick Position 1",
                    PropertyName = "TransferInShuttleRightYAxisPickPosition1",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPickPosition1,
                    Motion = Devices.MotionsInovance.TransferInShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPickPosition2"]?.ToString() ?? "Transfer In Shuttle Y Axis Pick Position 2",
                    PropertyName = "TransferInShuttleRightYAxisPickPosition2",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPickPosition2,
                    Motion = Devices.MotionsInovance.TransferInShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPickPosition3"]?.ToString() ?? "Transfer In Shuttle Y Axis Pick Position 3",
                    PropertyName = "TransferInShuttleRightYAxisPickPosition3",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPickPosition3,
                    Motion = Devices.MotionsInovance.TransferInShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleZAxisPickPosition"]?.ToString() ?? "Transfer In Shuttle Z Axis Pick Position",
                    PropertyName = "TransferInShuttleRightZAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisPickPosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleYAxisPlacePosition"]?.ToString() ?? "Transfer In Shuttle Y Axis Place Position",
                    PropertyName = "TransferInShuttleRightYAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPlacePosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferInShuttleZAxisPlacePosition"]?.ToString() ?? "Transfer In Shuttle Z Axis Place Position",
                    PropertyName = "TransferInShuttleRightZAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisPlacePosition,
                    Motion = Devices.MotionsInovance.TransferInShuttleRZAxis
                }
            };
        }

        // Transfer Rotation Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetTransferRotationLeftPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.TransferRotationLeftRecipe == null || 
                Devices?.MotionsInovance?.TransferRotationLZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisReadyPosition"]?.ToString() ?? "Transfer Rotation Z Axis Ready Position",
                    PropertyName = "TransferRotationLeftZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.TransferRotationLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisPickPosition"]?.ToString() ?? "Transfer Rotation Z Axis Pick Position",
                    PropertyName = "TransferRotationLeftZAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisPickPosition,
                    Motion = Devices.MotionsInovance.TransferRotationLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisTransferBeforeRotatePosition"]?.ToString() ?? "Transfer Rotation Z Axis Transfer Before Rotate Position",
                    PropertyName = "TransferRotationLeftZAxisTransferBeforeRotatePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisTransferBeforeRotatePosition,
                    Motion = Devices.MotionsInovance.TransferRotationLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisTransferAfterRotatePosition"]?.ToString() ?? "Transfer Rotation Z Axis Transfer After Rotate Position",
                    PropertyName = "TransferRotationLeftZAxisTransferAfterRotatePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisTransferAfterRotatePosition,
                    Motion = Devices.MotionsInovance.TransferRotationLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisPlacePosition"]?.ToString() ?? "Transfer Rotation Z Axis Place Position",
                    PropertyName = "TransferRotationLeftZAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisPlacePosition,
                    Motion = Devices.MotionsInovance.TransferRotationLZAxis
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetTransferRotationRightPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.TransferRotationRightRecipe == null || 
                Devices?.MotionsInovance?.TransferRotationRZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisReadyPosition"]?.ToString() ?? "Transfer Rotation Z Axis Ready Position",
                    PropertyName = "TransferRotationRightZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.TransferRotationRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisPickPosition"]?.ToString() ?? "Transfer Rotation Z Axis Pick Position",
                    PropertyName = "TransferRotationRightZAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisPickPosition,
                    Motion = Devices.MotionsInovance.TransferRotationRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisTransferBeforeRotatePosition"]?.ToString() ?? "Transfer Rotation Z Axis Transfer Before Rotate Position",
                    PropertyName = "TransferRotationRightZAxisTransferPositionUp",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisTransferBeforeRotatePosition,
                    Motion = Devices.MotionsInovance.TransferRotationRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisTransferAfterRotatePosition"]?.ToString() ?? "Transfer Rotation Z Axis Transfer After Rotate Position",
                    PropertyName = "TransferRotationRightZAxisTransferPositionDown",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisTransferAfterRotatePosition,
                    Motion = Devices.MotionsInovance.TransferRotationRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferRotationZAxisPlacePosition"]?.ToString() ?? "Transfer Rotation Z Axis Place Position",
                    PropertyName = "TransferRotationRightZAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisPlacePosition,
                    Motion = Devices.MotionsInovance.TransferRotationRZAxis
                }
            };
        }

        // Unload Transfer Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetUnloadTransferLeftPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.UnloadTransferLeftRecipe == null || 
                Devices?.MotionsInovance?.GlassUnloadLYAxis == null || 
                Devices?.MotionsInovance?.GlassUnloadLZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisReadyPosition"]?.ToString() ?? "Unload Transfer Y Axis Ready Position",
                    PropertyName = "UnloadTransferLeftYAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferZAxisReadyPosition"]?.ToString() ?? "Unload Transfer Z Axis Ready Position",
                    PropertyName = "UnloadTransferLeftZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPickPosition"]?.ToString() ?? "Unload Transfer Y Axis Pick Position",
                    PropertyName = "UnloadTransferLeftYAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPickPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferZAxisPickPosition"]?.ToString() ?? "Unload Transfer Z Axis Pick Position",
                    PropertyName = "UnloadTransferLeftZAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisPickPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadLZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition1"]?.ToString() ?? "Unload Transfer Y Axis Place Position 1",
                    PropertyName = "UnloadTransferLeftYAxisPlacePosition1",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition1,
                    Motion = Devices.MotionsInovance.GlassUnloadLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition2"]?.ToString() ?? "Unload Transfer Y Axis Place Position 2",
                    PropertyName = "UnloadTransferLeftYAxisPlacePosition2",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition2,
                    Motion = Devices.MotionsInovance.GlassUnloadLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition3"]?.ToString() ?? "Unload Transfer Y Axis Place Position 3",
                    PropertyName = "UnloadTransferLeftYAxisPlacePosition3",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition3,
                    Motion = Devices.MotionsInovance.GlassUnloadLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition4"]?.ToString() ?? "Unload Transfer Y Axis Place Position 4",
                    PropertyName = "UnloadTransferLeftYAxisPlacePosition4",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition4,
                    Motion = Devices.MotionsInovance.GlassUnloadLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferZAxisPlacePosition"]?.ToString() ?? "Unload Transfer Z Axis Place Position",
                    PropertyName = "UnloadTransferLeftZAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisPlacePosition,
                    Motion = Devices.MotionsInovance.GlassUnloadLZAxis
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetUnloadTransferRightPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.UnloadTransferRightRecipe == null || 
                Devices?.MotionsInovance?.GlassUnloadRYAxis == null || 
                Devices?.MotionsInovance?.GlassUnloadRZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisReadyPosition"]?.ToString() ?? "Unload Transfer Y Axis Ready Position",
                    PropertyName = "UnloadTransferRightYAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferZAxisReadyPosition"]?.ToString() ?? "Unload Transfer Z Axis Ready Position",
                    PropertyName = "UnloadTransferRightZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPickPosition"]?.ToString() ?? "Unload Transfer Y Axis Pick Position",
                    PropertyName = "UnloadTransferRightYAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPickPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferZAxisPickPosition"]?.ToString() ?? "Unload Transfer Z Axis Pick Position",
                    PropertyName = "UnloadTransferRightZAxisPickPosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisPickPosition,
                    Motion = Devices.MotionsInovance.GlassUnloadRZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition1"]?.ToString() ?? "Unload Transfer Y Axis Place Position 1",
                    PropertyName = "UnloadTransferRightYAxisPlacePosition1",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition1,
                    Motion = Devices.MotionsInovance.GlassUnloadRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition2"]?.ToString() ?? "Unload Transfer Y Axis Place Position 2",
                    PropertyName = "UnloadTransferRightYAxisPlacePosition2",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition2,
                    Motion = Devices.MotionsInovance.GlassUnloadRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition3"]?.ToString() ?? "Unload Transfer Y Axis Place Position 3",
                    PropertyName = "UnloadTransferRightYAxisPlacePosition3",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition3,
                    Motion = Devices.MotionsInovance.GlassUnloadRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferYAxisPlacePosition4"]?.ToString() ?? "Unload Transfer Y Axis Place Position 4",
                    PropertyName = "UnloadTransferRightYAxisPlacePosition4",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition4,
                    Motion = Devices.MotionsInovance.GlassUnloadRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_UnloadTransferZAxisPlacePosition"]?.ToString() ?? "Unload Transfer Z Axis Place Position",
                    PropertyName = "UnloadTransferRightZAxisPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisPlacePosition,
                    Motion = Devices.MotionsInovance.GlassUnloadRZAxis
                }
            };
        }

        // Clean Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetWETCleanLeftPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.WetCleanLeftRecipe == null || 
                Devices?.MotionsAjin?.InShuttleLXAxis == null ||
                Devices?.MotionsAjin?.InShuttleLYAxis == null ||
                Devices?.MotionsInovance?.InShuttleLTAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisLoadPosition"]?.ToString() ?? "WET Clean X Axis Load Position",
                    PropertyName = "WETCleanLeftXAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisLoadPosition,
                    Motion = Devices.MotionsAjin.InShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisLoadPosition"]?.ToString() ?? "WET Clean Y Axis Load Position",
                    PropertyName = "WETCleanLeftYAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisLoadPosition,
                    Motion = Devices.MotionsAjin.InShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisLoadPosition"]?.ToString() ?? "WET Clean T Axis Load Position",
                    PropertyName = "WETCleanLeftTAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisLoadPosition,
                    Motion = Devices.MotionsInovance.InShuttleLTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisCleanHorizontalPosition"]?.ToString() ?? "WET Clean X Axis Clean Horizontal Position",
                    PropertyName = "WETCleanLeftXAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.InShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisCleanHorizontalPosition"]?.ToString() ?? "WET Clean Y Axis Clean Horizontal Position",
                    PropertyName = "WETCleanLeftYAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.InShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisCleanHorizontalPosition"]?.ToString() ?? "WET Clean T Axis Clean Horizontal Position",
                    PropertyName = "WETCleanLeftTAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsInovance.InShuttleLTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisCleanVerticalPosition"]?.ToString() ?? "WET Clean X Axis Clean Vertical Position",
                    PropertyName = "WETCleanLeftXAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.InShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisCleanVerticalPosition"]?.ToString() ?? "WET Clean Y Axis Clean Vertical Position",
                    PropertyName = "WETCleanLeftYAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.InShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisCleanVerticalPosition"]?.ToString() ?? "WET Clean T Axis Clean Vertical Position",
                    PropertyName = "WETCleanLeftTAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisCleanVerticalPosition,
                    Motion = Devices.MotionsInovance.InShuttleLTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisUnloadPosition"]?.ToString() ?? "WET Clean X Axis Unload Position",
                    PropertyName = "WETCleanLeftXAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.InShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisUnloadPosition"]?.ToString() ?? "WET Clean Y Axis Unload Position",
                    PropertyName = "WETCleanLeftYAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.InShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisUnloadPosition"]?.ToString() ?? "WET Clean T Axis Unload Position",
                    PropertyName = "WETCleanLeftTAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisUnloadPosition,
                    Motion = Devices.MotionsInovance.InShuttleLTAxis
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetWETCleanRightPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.WetCleanRightRecipe == null || 
                Devices?.MotionsAjin?.InShuttleRXAxis == null ||
                Devices?.MotionsAjin?.InShuttleRYAxis == null ||
                Devices?.MotionsInovance?.InShuttleRTAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisLoadPosition"]?.ToString() ?? "WET Clean X Axis Load Position",
                    PropertyName = "WETCleanRightXAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisLoadPosition,
                    Motion = Devices.MotionsAjin.InShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisLoadPosition"]?.ToString() ?? "WET Clean Y Axis Load Position",
                    PropertyName = "WETCleanRightYAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisLoadPosition,
                    Motion = Devices.MotionsAjin.InShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisLoadPosition"]?.ToString() ?? "WET Clean T Axis Load Position",
                    PropertyName = "WETCleanRightTAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisLoadPosition,
                    Motion = Devices.MotionsInovance.InShuttleRTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisCleanHorizontalPosition"]?.ToString() ?? "WET Clean X Axis Clean Horizontal Position",
                    PropertyName = "WETCleanRightXAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.InShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisCleanHorizontalPosition"]?.ToString() ?? "WET Clean Y Axis Clean Horizontal Position",
                    PropertyName = "WETCleanRightYAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.InShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisCleanHorizontalPosition"]?.ToString() ?? "WET Clean T Axis Clean Horizontal Position",
                    PropertyName = "WETCleanRightTAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsInovance.InShuttleRTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisCleanVerticalPosition"]?.ToString() ?? "WET Clean X Axis Clean Vertical Position",
                    PropertyName = "WETCleanRightXAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.InShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisCleanVerticalPosition"]?.ToString() ?? "WET Clean Y Axis Clean Vertical Position",
                    PropertyName = "WETCleanRightYAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.InShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisCleanVerticalPosition"]?.ToString() ?? "WET Clean T Axis Clean Vertical Position",
                    PropertyName = "WETCleanRightTAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisCleanVerticalPosition,
                    Motion = Devices.MotionsInovance.InShuttleRTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanXAxisUnloadPosition"]?.ToString() ?? "WET Clean X Axis Unload Position",
                    PropertyName = "WETCleanRightXAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.InShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanYAxisUnloadPosition"]?.ToString() ?? "WET Clean Y Axis Unload Position",
                    PropertyName = "WETCleanRightYAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.InShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_WETCleanTAxisUnloadPosition"]?.ToString() ?? "WET Clean T Axis Unload Position",
                    PropertyName = "WETCleanRightTAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisUnloadPosition,
                    Motion = Devices.MotionsInovance.InShuttleRTAxis
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetAFCleanLeftPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.AfCleanLeftRecipe == null || 
                Devices?.MotionsAjin?.OutShuttleLXAxis == null ||
                Devices?.MotionsAjin?.OutShuttleLYAxis == null ||
                Devices?.MotionsInovance?.OutShuttleLTAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisLoadPosition"]?.ToString() ?? "AF Clean X Axis Load Position",
                    PropertyName = "AFCleanLeftXAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisLoadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisLoadPosition"]?.ToString() ?? "AF Clean Y Axis Load Position",
                    PropertyName = "AFCleanLeftYAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisLoadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisLoadPosition"]?.ToString() ?? "AF Clean T Axis Load Position",
                    PropertyName = "AFCleanLeftTAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisLoadPosition,
                    Motion = Devices.MotionsInovance.OutShuttleLTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisCleanHorizontalPosition"]?.ToString() ?? "AF Clean X Axis Clean Horizontal Position",
                    PropertyName = "AFCleanLeftXAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisCleanHorizontalPosition"]?.ToString() ?? "AF Clean Y Axis Clean Horizontal Position",
                    PropertyName = "AFCleanLeftYAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisCleanHorizontalPosition"]?.ToString() ?? "AF Clean T Axis Clean Horizontal Position",
                    PropertyName = "AFCleanLeftTAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsInovance.OutShuttleLTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisCleanVerticalPosition"]?.ToString() ?? "AF Clean X Axis Clean Vertical Position",
                    PropertyName = "AFCleanLeftXAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisCleanVerticalPosition"]?.ToString() ?? "AF Clean Y Axis Clean Vertical Position",
                    PropertyName = "AFCleanLeftYAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisCleanVerticalPosition"]?.ToString() ?? "AF Clean T Axis Clean Vertical Position",
                    PropertyName = "AFCleanLeftTAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisCleanVerticalPosition,
                    Motion = Devices.MotionsInovance.OutShuttleLTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisUnloadPosition"]?.ToString() ?? "AF Clean X Axis Unload Position",
                    PropertyName = "AFCleanLeftXAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisUnloadPosition"]?.ToString() ?? "AF Clean Y Axis Unload Position",
                    PropertyName = "AFCleanLeftYAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleLYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisUnloadPosition"]?.ToString() ?? "AF Clean T Axis Unload Position",
                    PropertyName = "AFCleanLeftTAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisUnloadPosition,
                    Motion = Devices.MotionsInovance.OutShuttleLTAxis
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetAFCleanRightPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.AfCleanRightRecipe == null || 
                Devices?.MotionsAjin?.OutShuttleRXAxis == null ||
                Devices?.MotionsAjin?.OutShuttleRYAxis == null ||
                Devices?.MotionsInovance?.OutShuttleRTAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisLoadPosition"]?.ToString() ?? "AF Clean X Axis Load Position",
                    PropertyName = "AFCleanRightXAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisLoadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisLoadPosition"]?.ToString() ?? "AF Clean Y Axis Load Position",
                    PropertyName = "AFCleanRightYAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisLoadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisLoadPosition"]?.ToString() ?? "AF Clean T Axis Load Position",
                    PropertyName = "AFCleanRightTAxisLoadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisLoadPosition,
                    Motion = Devices.MotionsInovance.OutShuttleRTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisCleanHorizontalPosition"]?.ToString() ?? "AF Clean X Axis Clean Horizontal Position",
                    PropertyName = "AFCleanRightXAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisCleanHorizontalPosition"]?.ToString() ?? "AF Clean Y Axis Clean Horizontal Position",
                    PropertyName = "AFCleanRightYAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisCleanHorizontalPosition"]?.ToString() ?? "AF Clean T Axis Clean Horizontal Position",
                    PropertyName = "AFCleanRightTAxisCleanHorizontalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisCleanHorizontalPosition,
                    Motion = Devices.MotionsInovance.OutShuttleRTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisCleanVerticalPosition"]?.ToString() ?? "AF Clean X Axis Clean Vertical Position",
                    PropertyName = "AFCleanRightXAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisCleanVerticalPosition"]?.ToString() ?? "AF Clean Y Axis Clean Vertical Position",
                    PropertyName = "AFCleanRightYAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisCleanVerticalPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisCleanVerticalPosition"]?.ToString() ?? "AF Clean T Axis Clean Vertical Position",
                    PropertyName = "AFCleanRightTAxisCleanVerticalPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisCleanVerticalPosition,
                    Motion = Devices.MotionsInovance.OutShuttleRTAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanXAxisUnloadPosition"]?.ToString() ?? "AF Clean X Axis Unload Position",
                    PropertyName = "AFCleanRightXAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanYAxisUnloadPosition"]?.ToString() ?? "AF Clean Y Axis Unload Position",
                    PropertyName = "AFCleanRightYAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.OutShuttleRYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_AFCleanTAxisUnloadPosition"]?.ToString() ?? "AF Clean T Axis Unload Position",
                    PropertyName = "AFCleanRightTAxisUnloadPosition",
                    Position = RecipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisUnloadPosition,
                    Motion = Devices.MotionsInovance.OutShuttleRTAxis
                }
            };
        }

        #endregion

        #region GetMotions
        // CSTLoadUnload Tab Motions
        private ObservableCollection<IMotion> GetInWorkConveyorMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.InCassetteTAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetOutWorkConveyorMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.OutCassetteTAxis);
            return motions;
        }


        // Detach Tab Motions
        private ObservableCollection<IMotion> GetTransferFixtureMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
                motions.Add(Devices.MotionsInovance.FixtureTransferYAxis);
            // TransferFixtureProcess chỉ sử dụng FixtureTransferYAxis, không sử dụng ShuttleTransferZAxis
            return motions;
        }

        private ObservableCollection<IMotion> GetDetachMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
                motions.Add(Devices.MotionsInovance.DetachGlassZAxis);
                motions.Add(Devices.MotionsAjin.ShuttleTransferZAxis);
                motions.Add(Devices.MotionsInovance.ShuttleTransferXAxis);
            return motions;
        }

        // Glass Transfer Tab Motions
        private ObservableCollection<IMotion> GetGlassTransferMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassTransferYAxis);
                motions.Add(Devices.MotionsInovance.GlassTransferZAxis);
            return motions;
        }

        // Transfer Shutter Tab Motions
        private ObservableCollection<IMotion> GetTransferShutterLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferInShuttleLYAxis);
            motions.Add(Devices.MotionsInovance.TransferInShuttleLZAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetTransferShutterRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferInShuttleRYAxis);
            motions.Add(Devices.MotionsInovance.TransferInShuttleRZAxis);
            return motions;
        }

        // Transfer Rotation Tab Motions
        private ObservableCollection<IMotion> GetTransferRotationLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferRotationLZAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetTransferRotationRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferRotationRZAxis);
            return motions;
        }

        // Unload Transfer Tab Motions
        private ObservableCollection<IMotion> GetUnloadTransferLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.GlassUnloadLYAxis);
            motions.Add(Devices.MotionsInovance.GlassUnloadLZAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetUnloadTransferRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.GlassUnloadRYAxis);
            motions.Add(Devices.MotionsInovance.GlassUnloadRZAxis);
            return motions;
        }

        // Clean Tab Motions
        private ObservableCollection<IMotion> GetWETCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // WET Clean Left sử dụng InShuttle axes
            motions.Add(Devices.MotionsAjin.InShuttleLXAxis);
            motions.Add(Devices.MotionsAjin.InShuttleLYAxis);
            motions.Add(Devices.MotionsInovance.InShuttleLTAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetWETCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // WET Clean Right sử dụng InShuttle axes
            motions.Add(Devices.MotionsAjin.InShuttleRXAxis);
            motions.Add(Devices.MotionsAjin.InShuttleRYAxis);
            motions.Add(Devices.MotionsInovance.InShuttleRTAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetAFCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // AF Clean Left sử dụng OutShuttle axes
            motions.Add(Devices.MotionsAjin.OutShuttleLXAxis);
            motions.Add(Devices.MotionsAjin.OutShuttleLYAxis);
            motions.Add(Devices.MotionsInovance.OutShuttleLTAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetAFCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // AF Clean Right sử dụng OutShuttle axes
            motions.Add(Devices.MotionsAjin.OutShuttleRXAxis);
            motions.Add(Devices.MotionsAjin.OutShuttleRYAxis);
            motions.Add(Devices.MotionsInovance.OutShuttleRTAxis);
            return motions;
        }

        #endregion

        #region GetCylinders
        // CSTLoadUnload Tab Cylinders
        private ObservableCollection<ICylinder> GetInWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // In CST Stopper
            cylinders.Add(Devices.Cylinders.InCstStopperUpDown);
            // In CST Work cylinders
            cylinders.Add(Devices.Cylinders.InCstFixCyl1FwBw);
            cylinders.Add(Devices.Cylinders.InCstFixCyl2FwBw);
            cylinders.Add(Devices.Cylinders.InCstTiltCylUpDown);
            // In CV Support cylinders
            cylinders.Add(Devices.Cylinders.InCvSupportUpDown);
            cylinders.Add(Devices.Cylinders.InCvSupportBufferUpDown);
            return cylinders;
        }
        
        private ObservableCollection<ICylinder> GetOutWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Out CST Stopper
            cylinders.Add(Devices.Cylinders.OutCstStopperUpDown);
            // Out CST Work cylinders
            cylinders.Add(Devices.Cylinders.OutCstFixCyl1FwBw);
            cylinders.Add(Devices.Cylinders.OutCstFixCyl2FwBw);
            cylinders.Add(Devices.Cylinders.OutCstTiltCylUpDown);
            // Out CV Support cylinders
            cylinders.Add(Devices.Cylinders.OutCvSupportUpDown);
            cylinders.Add(Devices.Cylinders.OutCvSupportBufferUpDown);
            return cylinders;
        }

        // Detach Tab Cylinders
        private ObservableCollection<ICylinder> GetTransferFixtureCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Transfer Fixture cylinders
            cylinders.Add(Devices.Cylinders.TransferFixtureUpDown);
            cylinders.Add(Devices.Cylinders.TransferFixture1ClampUnclamp);
            cylinders.Add(Devices.Cylinders.TransferFixture2ClampUnclamp);

            return cylinders;
        }
        
        private ObservableCollection<ICylinder> GetDetachCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Detach fix fixture cylinders
            cylinders.Add(Devices.Cylinders.DetachFixFixtureCyl1FwBw);
            cylinders.Add(Devices.Cylinders.DetachFixFixtureCyl2FwBw);
            
            // Add Detach cylinders
            cylinders.Add(Devices.Cylinders.DetachCyl1UpDown);
            cylinders.Add(Devices.Cylinders.DetachCyl2UpDown);
            
            // Add Detach vacuum cylinders
            cylinders.Add(Devices.Cylinders.DetachGlassShtVac1OnOff);
            cylinders.Add(Devices.Cylinders.DetachGlassShtVac2OnOff);
            cylinders.Add(Devices.Cylinders.DetachGlassShtVac3OnOff);
            
            return cylinders;
        }

        // Glass Transfer Tab Cylinders
        private ObservableCollection<ICylinder> GetGlassTransferCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Glass Transfer cylinders
            cylinders.Add(Devices.Cylinders.GlassTransferCyl1UpDown);
            cylinders.Add(Devices.Cylinders.GlassTransferCyl2UpDown);
            cylinders.Add(Devices.Cylinders.GlassTransferCyl3UpDown);
            
            // Add Glass Transfer vacuum cylinders
            cylinders.Add(Devices.Cylinders.GlassTransferVac1OnOff);
            cylinders.Add(Devices.Cylinders.GlassTransferVac2OnOff);
            cylinders.Add(Devices.Cylinders.GlassTransferVac3OnOff);
            
            return cylinders;
        }

        // Transfer Shutter Tab Cylinders (No cylinders used in TransferInShuttle process)
        private ObservableCollection<ICylinder> GetTransferShutterLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer In Shuttle Left cylinders
            cylinders.Add(Devices.Cylinders.TransferInShuttleLRotate);
            cylinders.Add(Devices.Cylinders.TransferInShuttleLVacOnOff);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetTransferShutterRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer In Shuttle Right cylinders
            cylinders.Add(Devices.Cylinders.TransferInShuttleRRotate);
            cylinders.Add(Devices.Cylinders.TransferInShuttleRVacOnOff);
            return cylinders;
        }

        // Transfer Rotation Tab Cylinders
        private ObservableCollection<ICylinder> GetTransferRotationLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer Rotation Left cylinders
            cylinders.Add(Devices.Cylinders.TrRotateLeftRotate);
            cylinders.Add(Devices.Cylinders.TrRotateLeftFwBw);
            cylinders.Add(Devices.Cylinders.TrRotateLeftUpDown);
            cylinders.Add(Devices.Cylinders.TrRotateLeftVacOnOff);
            cylinders.Add(Devices.Cylinders.TrRotateLeftVac1OnOff);
            cylinders.Add(Devices.Cylinders.TrRotateLeftVac2OnOff);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetTransferRotationRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer Rotation Right cylinders
            cylinders.Add(Devices.Cylinders.TrRotateRightRotate);
            cylinders.Add(Devices.Cylinders.TrRotateRightFwBw);
            cylinders.Add(Devices.Cylinders.TrRotateRightUpDown);
            cylinders.Add(Devices.Cylinders.TrRotateRightVacOnOff);
            cylinders.Add(Devices.Cylinders.TrRotateRightVac1OnOff);
            cylinders.Add(Devices.Cylinders.TrRotateRightVac2OnOff);
            return cylinders;
        }

        // Unload Transfer Tab Cylinders
        private ObservableCollection<ICylinder> GetUnloadTransferLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Unload Robot Left cylinders
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl4UpDown);
            // Add Unload Align Left cylinders
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl4UpDown);
            // Add Unload Transfer Left vacuum cylinders
            cylinders.Add(Devices.Cylinders.UnloadTransferLVacOnOff);
            cylinders.Add(Devices.Cylinders.UnloadGlassAlignVac1OnOff);
            cylinders.Add(Devices.Cylinders.UnloadGlassAlignVac2OnOff);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetUnloadTransferRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Unload Robot Right cylinders
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl4UpDown);
            // Add Unload Align Right cylinders
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl4UpDown);
            // Add Unload Transfer Right vacuum cylinders
            cylinders.Add(Devices.Cylinders.UnloadTransferRVacOnOff);
            cylinders.Add(Devices.Cylinders.UnloadGlassAlignVac3OnOff);
            cylinders.Add(Devices.Cylinders.UnloadGlassAlignVac4OnOff);
            return cylinders;
        }

        // Clean Tab Cylinders
        private ObservableCollection<ICylinder> GetWETCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add WET Clean Left cylinders
            cylinders.Add(Devices.Cylinders.WetCleanPusherLeftUpDown);
            cylinders.Add(Devices.Cylinders.WetCleanBrushLeftUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetWETCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add WET Clean Right cylinders
            cylinders.Add(Devices.Cylinders.WetCleanPusherRightUpDown);
            cylinders.Add(Devices.Cylinders.WetCleanBrushRightUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetAFCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add AF Clean Left cylinders
            cylinders.Add(Devices.Cylinders.AFCleanPusherLeftUpDown);
            cylinders.Add(Devices.Cylinders.AFCleanBrushLeftUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetAFCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add AF Clean Right cylinders
            cylinders.Add(Devices.Cylinders.AFCleanPusherRightUpDown);
            cylinders.Add(Devices.Cylinders.AFCleanBrushRightUpDown);
            return cylinders;
        }

        #endregion

        #region GetInputs
        // CSTLoadUnload Tab Inputs
        private ObservableCollection<IDInput> GetInWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add In Cassette detection inputs
            inputs.Add(Devices.Inputs.InCstDetect1);
            inputs.Add(Devices.Inputs.InCstDetect2);
            // Add In Cassette work detection inputs
            inputs.Add(Devices.Inputs.InCstWorkDetect1);
            inputs.Add(Devices.Inputs.InCstWorkDetect2);
            inputs.Add(Devices.Inputs.InCstWorkDetect3);
            inputs.Add(Devices.Inputs.InCstWorkDetect4);
            // Add In Cassette button inputs
            inputs.Add(Devices.Inputs.InButton1);
            inputs.Add(Devices.Inputs.InButton2);
            // Add In Cassette light curtain safety input
            inputs.Add(Devices.Inputs.InCstLightCurtainAlarmDetect);
            // Add In CV Support detection inputs
            inputs.Add(Devices.Inputs.InCvSupportUp);
            inputs.Add(Devices.Inputs.InCvSupportDown);
            inputs.Add(Devices.Inputs.InCvSupportBufferUp);
            inputs.Add(Devices.Inputs.InCvSupportBufferDown);
            // Add In CST Fix cylinder detection inputs
            inputs.Add(Devices.Inputs.InCstFixCyl1Fw);
            inputs.Add(Devices.Inputs.InCstFixCyl1Bw);
            inputs.Add(Devices.Inputs.InCstFixCyl2Fw);
            inputs.Add(Devices.Inputs.InCstFixCyl2Bw);
            // Add In CST Tilt cylinder detection inputs
            inputs.Add(Devices.Inputs.InCstTiltCylUp);
            inputs.Add(Devices.Inputs.InCstTiltCylDown);
            // Add In CST Stopper detection inputs
            inputs.Add(Devices.Inputs.InCstStopperUp);
            inputs.Add(Devices.Inputs.InCstStopperDown);
            
            return inputs;
        }
        
        private ObservableCollection<IDInput> GetOutWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Out Cassette work detection inputs
            inputs.Add(Devices.Inputs.OutCstWorkDetect1);
            inputs.Add(Devices.Inputs.OutCstWorkDetect2);
            inputs.Add(Devices.Inputs.OutCstWorkDetect3);
            // Add Out Cassette detection inputs
            inputs.Add(Devices.Inputs.OutCstDetect1);
            inputs.Add(Devices.Inputs.OutCstDetect2);
            // Add Out Cassette button inputs
            inputs.Add(Devices.Inputs.OutButton1);
            inputs.Add(Devices.Inputs.OutButton2);
            // Add Out Cassette light curtain safety input
            inputs.Add(Devices.Inputs.OutCstLightCurtainAlarmDetect);
            // Add Out CV Support detection inputs
            inputs.Add(Devices.Inputs.OutCvSupportUp);
            inputs.Add(Devices.Inputs.OutCvSupportDown);
            inputs.Add(Devices.Inputs.OutCvSupportBufferUp);
            inputs.Add(Devices.Inputs.OutCvSupportBufferDown);
            // Add Out CST Fix cylinder detection inputs
            inputs.Add(Devices.Inputs.OutCstFixCyl1Fw);
            inputs.Add(Devices.Inputs.OutCstFixCyl1Bw);
            inputs.Add(Devices.Inputs.OutCstFixCyl2Fw);
            inputs.Add(Devices.Inputs.OutCstFixCyl2Bw);
            // Add Out CST Tilt cylinder detection inputs
            inputs.Add(Devices.Inputs.OutCstTiltCylUp);
            inputs.Add(Devices.Inputs.OutCstTiltCylDown);
            // Add Out CST Stopper detection inputs
            inputs.Add(Devices.Inputs.OutCstStopperUp);
            inputs.Add(Devices.Inputs.OutCstStopperDown);
            
            return inputs;
        }

        // Detach Tab Inputs
        private ObservableCollection<IDInput> GetTransferFixtureInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Transfer Fixture detection inputs
            inputs.Add(Devices.Inputs.TransferFixtureUp);
            inputs.Add(Devices.Inputs.TransferFixtureDown);
            // Add Transfer Fixture clamp detection inputs
            inputs.Add(Devices.Inputs.TransferFixture11Clamp);
            inputs.Add(Devices.Inputs.TransferFixture11Unclamp);
            inputs.Add(Devices.Inputs.TransferFixture12Clamp);
            inputs.Add(Devices.Inputs.TransferFixture12Unclamp);
            inputs.Add(Devices.Inputs.TransferFixture21Clamp);
            inputs.Add(Devices.Inputs.TransferFixture21Unclamp);
            inputs.Add(Devices.Inputs.TransferFixture22Clamp);
            inputs.Add(Devices.Inputs.TransferFixture22Unclamp);
            
            return inputs;
        }
        
        private ObservableCollection<IDInput> GetDetachInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Detach fixture detection input
            inputs.Add(Devices.Inputs.DetachFixtureDetect);
            // Add Detach glass shuttle vacuum inputs
            inputs.Add(Devices.Inputs.DetachGlassShtVac1);
            inputs.Add(Devices.Inputs.DetachGlassShtVac2);
            inputs.Add(Devices.Inputs.DetachGlassShtVac3);
            // Add Detach cylinder detection inputs
            inputs.Add(Devices.Inputs.DetachCyl1Up);
            inputs.Add(Devices.Inputs.DetachCyl1Down);
            inputs.Add(Devices.Inputs.DetachCyl2Up);
            inputs.Add(Devices.Inputs.DetachCyl2Down);
            // Add Detach fix fixture cylinder detection inputs
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl11Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl11Bw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl12Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl12Bw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl21Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl21Bw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl22Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl22Bw);
            
            return inputs;
        }

        // Transfer Shutter Tab Inputs
        private ObservableCollection<IDInput> GetTransferShutterLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass detection inputs
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect1);
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect2);
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect3);
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleLVac);
            // Add Transfer In Shuttle Left rotate detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleL0Degree);
            inputs.Add(Devices.Inputs.TransferInShuttleL180Degree);
            return inputs;
        }

        private ObservableCollection<IDInput> GetTransferShutterRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass detection inputs
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect1);
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect2);
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect3);
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleRVac);
            // Add Transfer In Shuttle Right rotate detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleR0Degree);
            inputs.Add(Devices.Inputs.TransferInShuttleR180Degree);
            return inputs;
        }

        // Transfer Rotation Tab Inputs
        private ObservableCollection<IDInput> GetTransferRotationLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TrRotateLeftVac1);
            inputs.Add(Devices.Inputs.TrRotateLeftVac2);
            inputs.Add(Devices.Inputs.TrRotateLeftRotVac);
            // Add Transfer Rotation Left cylinder detection inputs
            inputs.Add(Devices.Inputs.TrRotateLeft0Degree);
            inputs.Add(Devices.Inputs.TrRotateLeft180Degree);
            inputs.Add(Devices.Inputs.TrRotateLeftFw);
            inputs.Add(Devices.Inputs.TrRotateLeftBw);
            inputs.Add(Devices.Inputs.TrRotateLeftUp);
            inputs.Add(Devices.Inputs.TrRotateLeftDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetTransferRotationRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TrRotateRightVac1);
            inputs.Add(Devices.Inputs.TrRotateRightVac2);
            inputs.Add(Devices.Inputs.TrRotateRightRotVac);
            // Add Transfer Rotation Right cylinder detection inputs
            inputs.Add(Devices.Inputs.TrRotateRight0Degree);
            inputs.Add(Devices.Inputs.TrRotateRight180Degree);
            inputs.Add(Devices.Inputs.TrRotateRightFw);
            inputs.Add(Devices.Inputs.TrRotateRightBw);
            inputs.Add(Devices.Inputs.TrRotateRightUp);
            inputs.Add(Devices.Inputs.TrRotateRightDown);
            return inputs;
        }

        // Unload Transfer Tab Inputs
        private ObservableCollection<IDInput> GetUnloadTransferLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Unload Transfer Left vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadTransferLVac);
            // Add Unload Align vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac1);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac2);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac3);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac4);
            // Add Unload Robot cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadAlignCyl1Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl1Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl2Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl2Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl3Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl3Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl4Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl4Down);
            return inputs;
        }

        private ObservableCollection<IDInput> GetUnloadTransferRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Unload Transfer Right vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadTransferRVac);
            // Add Unload Align vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac1);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac2);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac3);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac4);
            // Add Unload Robot cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadAlignCyl1Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl1Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl2Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl2Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl3Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl3Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl4Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl4Down);
            return inputs;
        }

        // Clean Tab Inputs
        private ObservableCollection<IDInput> GetWETCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add WET Clean Left detection inputs
            inputs.Add(Devices.Inputs.WetCleanLeftFeedingRollerDetect);
            // Add WET Clean Left pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanPusherLeftUp);
            inputs.Add(Devices.Inputs.WetCleanPusherLeftDown);
            // Add WET Clean Left brush cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanBrushLeftUp);
            inputs.Add(Devices.Inputs.WetCleanBrushLeftDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetWETCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add WET Clean Right detection inputs
            inputs.Add(Devices.Inputs.WetCleanRightFeedingRollerDetect);
            // Add WET Clean Right pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanPusherRightUp);
            inputs.Add(Devices.Inputs.WetCleanPusherRightDown);
            // Add WET Clean Right brush cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanBrushRightUp);
            inputs.Add(Devices.Inputs.WetCleanBrushRightDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetAFCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add AF Clean Left detection inputs
            inputs.Add(Devices.Inputs.AfCleanLeftFeedingRollerDetect);
            // Add AF Clean Left pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanPusherLeftUp);
            inputs.Add(Devices.Inputs.AfCleanPusherLeftDown);
            // Add AF Clean Left brush cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanBrushLeftUp);
            inputs.Add(Devices.Inputs.AfCleanBrushLeftDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetAFCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add AF Clean Right detection inputs
            inputs.Add(Devices.Inputs.AfCleanRightFeedingRollerDetect);
            // Add AF Clean Right pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanPusherRightUp);
            inputs.Add(Devices.Inputs.AfCleanPusherRightDown);
            // Add AF Clean Right brush cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanBrushRightUp);
            inputs.Add(Devices.Inputs.AfCleanBrushRightDown);
            return inputs;
        }

        // Glass Transfer Tab Inputs
        private ObservableCollection<IDInput> GetGlassTransferInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass Transfer vacuum detection inputs
            inputs.Add(Devices.Inputs.GlassTransferVac1);
            inputs.Add(Devices.Inputs.GlassTransferVac2);
            inputs.Add(Devices.Inputs.GlassTransferVac3);
            // Add Glass Transfer cylinder detection inputs
            inputs.Add(Devices.Inputs.GlassTransferCyl1Up);
            inputs.Add(Devices.Inputs.GlassTransferCyl1Down);
            inputs.Add(Devices.Inputs.GlassTransferCyl2Up);
            inputs.Add(Devices.Inputs.GlassTransferCyl2Down);
            inputs.Add(Devices.Inputs.GlassTransferCyl3Up);
            inputs.Add(Devices.Inputs.GlassTransferCyl3Down);
            return inputs;
        }

        // Unload Tab Inputs

        #endregion

        #region GetOutputs
        // CSTLoadUnload Tab Outputs
        private ObservableCollection<IDOutput> GetInWorkConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add In Cassette button lamp outputs
            outputs.Add(Devices.Outputs.InButtonLamp1);
            outputs.Add(Devices.Outputs.InButtonLamp2);
            // Add In Cassette light curtain muting output
            outputs.Add(Devices.Outputs.InCstLightCurtainMuting1);
            outputs.Add(Devices.Outputs.InCstLightCurtainMuting2);
            // Add In CST Stopper outputs
            outputs.Add(Devices.Outputs.InCstStopperUp);
            outputs.Add(Devices.Outputs.InCstStopperDown);
            // Add In CST Fix cylinder outputs
            outputs.Add(Devices.Outputs.InCstFixCyl1Fw);
            outputs.Add(Devices.Outputs.InCstFixCyl1Bw);
            outputs.Add(Devices.Outputs.InCstFixCyl2Fw);
            outputs.Add(Devices.Outputs.InCstFixCyl2Bw);
            // Add In CST Tilt cylinder outputs
            outputs.Add(Devices.Outputs.InCstTiltCylUp);
            outputs.Add(Devices.Outputs.InCstTiltCylDown);
            // Add In CV Support outputs
            outputs.Add(Devices.Outputs.InCvSupportUp);
            outputs.Add(Devices.Outputs.InCvSupportDown);
            outputs.Add(Devices.Outputs.InCvSupportBufferUp);
            outputs.Add(Devices.Outputs.InCvSupportBufferDown);

            return outputs;
        }
        
        private ObservableCollection<IDOutput> GetOutWorkConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Out Cassette button lamp outputs
            outputs.Add(Devices.Outputs.OutButtonLamp1);
            outputs.Add(Devices.Outputs.OutButtonLamp2);
            // Add Out Cassette light curtain muting output
            outputs.Add(Devices.Outputs.OutCstLightCurtainMuting1);
            outputs.Add(Devices.Outputs.OutCstLightCurtainMuting2);
            // Add Out CST Stopper outputs
            outputs.Add(Devices.Outputs.OutCstStopperUp);
            outputs.Add(Devices.Outputs.OutCstStopperDown);
            // Add Out CST Fix cylinder outputs
            outputs.Add(Devices.Outputs.OutCstFixCyl1Fw);
            outputs.Add(Devices.Outputs.OutCstFixCyl1Bw);
            outputs.Add(Devices.Outputs.OutCstFixCyl2Fw);
            outputs.Add(Devices.Outputs.OutCstFixCyl2Bw);
            // Add Out CST Tilt cylinder outputs
            outputs.Add(Devices.Outputs.OutCstTiltCylUp);
            outputs.Add(Devices.Outputs.OutCstTiltCylDown);
            // Add Out CV Support outputs
            outputs.Add(Devices.Outputs.OutCvSupportUp);
            outputs.Add(Devices.Outputs.OutCvSupportDown);
            outputs.Add(Devices.Outputs.OutCvSupportBufferUp);
            outputs.Add(Devices.Outputs.OutCvSupportBufferDown);
            return outputs;
        }

        // Detach Tab Outputs
        private ObservableCollection<IDOutput> GetTransferFixtureOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Transfer Fixture outputs
            outputs.Add(Devices.Outputs.TransferFixtureUp);
            outputs.Add(Devices.Outputs.TransferFixtureDown);
            // Add Transfer Fixture clamp outputs
            outputs.Add(Devices.Outputs.TransferFixture1Clamp);
            outputs.Add(Devices.Outputs.TransferFixture1Unclamp);
            outputs.Add(Devices.Outputs.TransferFixture2Clamp);
            outputs.Add(Devices.Outputs.TransferFixture2Unclamp);
            
            return outputs;
        }
        
        private ObservableCollection<IDOutput> GetDetachOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Detach glass shuttle vacuum outputs
            outputs.Add(Devices.Outputs.DetachGlassShtVac1OnOff);
            outputs.Add(Devices.Outputs.DetachGlassShtVac2OnOff);
            outputs.Add(Devices.Outputs.DetachGlassShtVac3OnOff);
            // Add Detach cylinder outputs
            outputs.Add(Devices.Outputs.DetachCyl1Up);
            outputs.Add(Devices.Outputs.DetachCyl1Down);
            outputs.Add(Devices.Outputs.DetachCyl2Up);
            outputs.Add(Devices.Outputs.DetachCyl2Down);
            // Add Detach fix fixture cylinder outputs
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl1Fw);
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl1Bw);
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl2Fw);
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl2Bw);
            return outputs;
        }

        // Glass Transfer Tab Outputs
        private ObservableCollection<IDOutput> GetGlassTransferOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Glass Transfer vacuum outputs
            outputs.Add(Devices.Outputs.GlassTransferVac1OnOff);
            outputs.Add(Devices.Outputs.GlassTransferVac2OnOff);
            outputs.Add(Devices.Outputs.GlassTransferVac3OnOff);
            // Add Glass Transfer cylinder outputs
            outputs.Add(Devices.Outputs.GlassTransferCyl1Up);
            outputs.Add(Devices.Outputs.GlassTransferCyl1Down);
            outputs.Add(Devices.Outputs.GlassTransferCyl2Up);
            outputs.Add(Devices.Outputs.GlassTransferCyl2Down);
            outputs.Add(Devices.Outputs.GlassTransferCyl3Up);
            outputs.Add(Devices.Outputs.GlassTransferCyl3Down);
            return outputs;
        }

        // Transfer Shutter Tab Outputs
        private ObservableCollection<IDOutput> GetTransferShutterLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TransferInShuttleLVacOnOff);
            // Add Transfer In Shuttle Left rotate outputs
            outputs.Add(Devices.Outputs.TransferInShuttleL0Degree);
            outputs.Add(Devices.Outputs.TransferInShuttleL180Degree);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetTransferShutterRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TransferInShuttleRVacOnOff);
            // Add Transfer In Shuttle Right rotate outputs
            outputs.Add(Devices.Outputs.TransferInShuttleR0Degree);
            outputs.Add(Devices.Outputs.TransferInShuttleR180Degree);
            return outputs;
        }

        // Transfer Rotation Tab Outputs
        private ObservableCollection<IDOutput> GetTransferRotationLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TrRotateLeftVac1OnOff);
            outputs.Add(Devices.Outputs.TrRotateLeftVac2OnOff);
            outputs.Add(Devices.Outputs.TrRotateLeftRotVacOnOff);
            // Add Transfer Rotation Left cylinder control outputs
            outputs.Add(Devices.Outputs.TrRotateLeft0Degree);
            outputs.Add(Devices.Outputs.TrRotateLeft180Degree);
            outputs.Add(Devices.Outputs.TrRotateLeftFw);
            outputs.Add(Devices.Outputs.TrRotateLeftBw);
            outputs.Add(Devices.Outputs.TrRotateLeftUp);
            outputs.Add(Devices.Outputs.TrRotateLeftDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetTransferRotationRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TrRotateRightVac1OnOff);
            outputs.Add(Devices.Outputs.TrRotateRightVac2OnOff);
            outputs.Add(Devices.Outputs.TrRotateRightRotVacOnOff);
            // Add Transfer Rotation Right cylinder control outputs
            outputs.Add(Devices.Outputs.TrRotateRight0Degree);
            outputs.Add(Devices.Outputs.TrRotateRight180Degree);
            outputs.Add(Devices.Outputs.TrRotateRightFw);
            outputs.Add(Devices.Outputs.TrRotateRightBw);
            outputs.Add(Devices.Outputs.TrRotateRightUp);
            outputs.Add(Devices.Outputs.TrRotateRightDown);
            return outputs;
        }

        // Unload Transfer Tab Outputs
        private ObservableCollection<IDOutput> GetUnloadTransferLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Unload Transfer Left vacuum outputs
            outputs.Add(Devices.Outputs.UnloadTransferLVacOnOff);
            // Add Unload Align vacuum outputs
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac1OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac2OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac3OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac4OnOff);
            // Add Unload Robot cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadRobotCyl1Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl2Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl3Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadAlignCyl1Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl2Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl3Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl4Up);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetUnloadTransferRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Unload Transfer Right vacuum outputs
            outputs.Add(Devices.Outputs.UnloadTransferRVacOnOff);
            // Add Unload Align vacuum outputs
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac1OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac2OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac3OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac4OnOff);
            // Add Unload Robot cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadRobotCyl1Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl2Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl3Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadAlignCyl1Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl2Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl3Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl4Up);
            return outputs;
        }

        // Clean Tab Outputs
        private ObservableCollection<IDOutput> GetWETCleanLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add WET Clean Left cylinder control outputs
            outputs.Add(Devices.Outputs.WetCleanPusherLeftUp);
            outputs.Add(Devices.Outputs.WetCleanPusherLeftDown);
            outputs.Add(Devices.Outputs.WetCleanBrushLeftDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetWETCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add WET Clean Right cylinder control outputs
            outputs.Add(Devices.Outputs.WetCleanPusherRightUp);
            outputs.Add(Devices.Outputs.WetCleanPusherRightDown);
            outputs.Add(Devices.Outputs.WetCleanBrushRightDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetAFCleanLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add AF Clean Left cylinder control outputs
            outputs.Add(Devices.Outputs.AfCleanPusherLeftUp);
            outputs.Add(Devices.Outputs.AfCleanPusherLeftDown);
            outputs.Add(Devices.Outputs.AfCleanBrushLeftDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetAFCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add AF Clean Right cylinder control outputs
            outputs.Add(Devices.Outputs.AfCleanPusherRightUp);
            outputs.Add(Devices.Outputs.AfCleanPusherRightDown);
            outputs.Add(Devices.Outputs.AfCleanBrushRightDown);
            return outputs;
        }

        #endregion

        #region GetProcess
        private ObservableCollection<IProcess<ESequence>> GetProcessList()
        {
            ObservableCollection<IProcess<ESequence>> processes = new ObservableCollection<IProcess<ESequence>>
            {
                // CSTLoadUnload Tab (2 units)
                Processes.InWorkConveyorProcess,
                Processes.OutWorkConveyorProcess,
                
                // Detach Tab (2 units)
                Processes.TransferFixtureProcess,
                Processes.DetachProcess,
                
                // Glass Transfer Tab
                Processes.GlassTransferProcess,
                
                // Transfer Shutter Tab (2 units)
                Processes.TransferInShuttleLeftProcess,
                Processes.TransferInShuttleRightProcess,
                
                // Transfer Rotation Tab (2 units)
                Processes.TransferRotationLeftProcess,
                Processes.TransferRotationRightProcess,
                
                // Unload Transfer Tab (2 units)
                Processes.UnloadTransferLeftProcess,
                Processes.UnloadTransferRightProcess,
                
                // Clean Tab (4 units)
                Processes.WETCleanLeftProcess,
                Processes.WETCleanRightProcess,
                Processes.AFCleanLeftProcess,
                Processes.AFCleanRightProcess,
            };
            return processes;
        }
        #endregion

        #region GetDetailProcess
        public void Dispose()
        {
            Cylinders = null;
            Inputs = null;
            Outputs = null;
            Motions = null;
            PositionTeachings = null;
        }
        private void SelectedPropertyProcess()
        {
            Dispose();

            // CSTLoadUnload Tab
            if (SelectedProcess == Processes.InWorkConveyorProcess)
            {
                Motions = GetInWorkConveyorMotions();
                Cylinders = GetInWorkConveyorCylinders();
                Inputs = GetInWorkConveyorInputs();
                Outputs = GetInWorkConveyorOutputs();
                PositionTeachings = GetInWorkConveyorPositionTeachings();
            }
            else if (SelectedProcess == Processes.OutWorkConveyorProcess)
            {
                Motions = GetOutWorkConveyorMotions();
                Cylinders = GetOutWorkConveyorCylinders();
                Inputs = GetOutWorkConveyorInputs();
                Outputs = GetOutWorkConveyorOutputs();
                PositionTeachings = GetOutWorkConveyorPositionTeachings();
            }

            // Detach Tab
            else if (SelectedProcess == Processes.TransferFixtureProcess)
            {
                Motions = GetTransferFixtureMotions();
                Cylinders = GetTransferFixtureCylinders();
                Inputs = GetTransferFixtureInputs();
                Outputs = GetTransferFixtureOutputs();
                PositionTeachings = GetTransferFixturePositionTeachings();
            }
            else if (SelectedProcess == Processes.DetachProcess)
            {
                Motions = GetDetachMotions();
                Cylinders = GetDetachCylinders();
                Inputs = GetDetachInputs();
                Outputs = GetDetachOutputs();
                PositionTeachings = GetDetachPositionTeachings();
            }

            // Glass Transfer Tab
            else if (SelectedProcess == Processes.GlassTransferProcess)
            {
                Motions = GetGlassTransferMotions();
                Cylinders = GetGlassTransferCylinders();
                Inputs = GetGlassTransferInputs();
                Outputs = GetGlassTransferOutputs();
                PositionTeachings = GetGlassTransferPositionTeachings();
            }

            // Transfer Shutter Tab
            else if (SelectedProcess == Processes.TransferInShuttleLeftProcess)
            {
                Motions = GetTransferShutterLeftMotions();
                Cylinders = GetTransferShutterLeftCylinders();
                Inputs = GetTransferShutterLeftInputs();
                Outputs = GetTransferShutterLeftOutputs();
                PositionTeachings = GetTransferShutterLeftPositionTeachings();
            }
            else if (SelectedProcess == Processes.TransferInShuttleRightProcess)
            {
                Motions = GetTransferShutterRightMotions();
                Cylinders = GetTransferShutterRightCylinders();
                Inputs = GetTransferShutterRightInputs();
                Outputs = GetTransferShutterRightOutputs();
                PositionTeachings = GetTransferShutterRightPositionTeachings();
            }

            // Transfer Rotation Tab
            else if (SelectedProcess == Processes.TransferRotationLeftProcess)
            {
                Motions = GetTransferRotationLeftMotions();
                Cylinders = GetTransferRotationLeftCylinders();
                Inputs = GetTransferRotationLeftInputs();
                Outputs = GetTransferRotationLeftOutputs();
                PositionTeachings = GetTransferRotationLeftPositionTeachings();
            }
            else if (SelectedProcess == Processes.TransferRotationRightProcess)
            {
                Motions = GetTransferRotationRightMotions();
                Cylinders = GetTransferRotationRightCylinders();
                Inputs = GetTransferRotationRightInputs();
                Outputs = GetTransferRotationRightOutputs();
                PositionTeachings = GetTransferRotationRightPositionTeachings();
            }

            // Unload Transfer Tab
            else if (SelectedProcess == Processes.UnloadTransferLeftProcess)
            {
                Motions = GetUnloadTransferLeftMotions();
                Cylinders = GetUnloadTransferLeftCylinders();
                Inputs = GetUnloadTransferLeftInputs();
                Outputs = GetUnloadTransferLeftOutputs();
                PositionTeachings = GetUnloadTransferLeftPositionTeachings();
            }
            else if (SelectedProcess == Processes.UnloadTransferRightProcess)
            {
                Motions = GetUnloadTransferRightMotions();
                Cylinders = GetUnloadTransferRightCylinders();
                Inputs = GetUnloadTransferRightInputs();
                Outputs = GetUnloadTransferRightOutputs();
                PositionTeachings = GetUnloadTransferRightPositionTeachings();
            }

            // Clean Tab
            else if (SelectedProcess == Processes.WETCleanLeftProcess)
            {
                Motions = GetWETCleanLeftMotions();
                Cylinders = GetWETCleanLeftCylinders();
                Inputs = GetWETCleanLeftInputs();
                Outputs = GetWETCleanLeftOutputs();
                PositionTeachings = GetWETCleanLeftPositionTeachings();
            }
            else if (SelectedProcess == Processes.WETCleanRightProcess)
            {
                Motions = GetWETCleanRightMotions();
                Cylinders = GetWETCleanRightCylinders();
                Inputs = GetWETCleanRightInputs();
                Outputs = GetWETCleanRightOutputs();
                PositionTeachings = GetWETCleanRightPositionTeachings();
            }
            else if (SelectedProcess == Processes.AFCleanLeftProcess)
            {
                Motions = GetAFCleanLeftMotions();
                Cylinders = GetAFCleanLeftCylinders();
                Inputs = GetAFCleanLeftInputs();
                Outputs = GetAFCleanLeftOutputs();
                PositionTeachings = GetAFCleanLeftPositionTeachings();
            }
            else if (SelectedProcess == Processes.AFCleanRightProcess)
            {
                Motions = GetAFCleanRightMotions();
                Cylinders = GetAFCleanRightCylinders();
                Inputs = GetAFCleanRightInputs();
                Outputs = GetAFCleanRightOutputs();
                PositionTeachings = GetAFCleanRightPositionTeachings();
            }
        }
        #endregion

        public TeachViewModel(Devices devices, MachineStatus machineStatus, RecipeList recipeList, RecipeSelector recipeSelector, Processes processes, DataViewModel dataViewModel)
        {
            Devices = devices;
            MachineStatus = machineStatus;
            RecipeList = recipeList;
            Processes = processes;
            RecipeSelector = recipeSelector;
            DataViewModel = dataViewModel;
            SelectedProcess = ProcessListTeaching.FirstOrDefault();
            
            // Initialize Commands
            CylinderForwardCommand = new RelayCommand<ICylinder>(CylinderForward);
            CylinderBackwardCommand = new RelayCommand<ICylinder>(CylinderBackward);
        }

        #region Commands
        public ICommand CylinderForwardCommand { get; }
        public ICommand CylinderBackwardCommand { get; }

        public void CylinderForward(ICylinder cylinder)
        {
            if (cylinder == null) return;

            // Check interlock before moving
            if (!CylinderInterLock(cylinder, true, out string CylinderInterlockMsg))
            {
                MessageBoxEx.ShowDialog($"InterLock Fail! Cannot Move Cylinder\n\n{CylinderInterlockMsg}");
                return;
            }
            if (cylinder.CylinderType == ECylinderType.ForwardBackwardReverse ||
                    cylinder.CylinderType == ECylinderType.UpDownReverse ||
                    cylinder.CylinderType == ECylinderType.RightLeftReverse ||
                    cylinder.CylinderType == ECylinderType.LockUnlockReverse ||
                    cylinder.CylinderType == ECylinderType.GripUngripReverse ||
                    cylinder.CylinderType == ECylinderType.AlignUnalignReverse ||
                    cylinder.CylinderType == ECylinderType.FlipUnflipReverse ||
                    cylinder.CylinderType == ECylinderType.ClampUnclampReverse
                    )
            {
                cylinder.Backward();
                return;
            }
            cylinder.Forward();

            // For OnOff cylinders in simulation mode, auto-set input feedback
#if SIMULATION
            if (cylinder.CylinderType == ECylinderType.OnOff)
            {
                // Use reflection to access protected InForward property
                var inForwardProperty = cylinder.GetType().GetProperty("InForward", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var inForward = inForwardProperty?.GetValue(cylinder) as List<IDInput>;
                
                if (inForward?.Count > 0)
                {
                    // Set vacuum detect input to true when vacuum is turned on
                    SimulationInputSetter.SetSimModbusInput(inForward[0], true);
                }
            }
#endif
        }

        public void CylinderBackward(ICylinder cylinder)
        {
            if (cylinder == null) return;

            // Check interlock before moving
            if (!CylinderInterLock(cylinder, false, out string CylinderInterlockMsg))
            {
                MessageBoxEx.ShowDialog($"InterLock Fail! Cannot Move Cylinder\n\n{CylinderInterlockMsg}");
                return;
            }
            if (cylinder.CylinderType == ECylinderType.ForwardBackwardReverse ||
                    cylinder.CylinderType == ECylinderType.UpDownReverse ||
                    cylinder.CylinderType == ECylinderType.RightLeftReverse ||
                    cylinder.CylinderType == ECylinderType.LockUnlockReverse ||
                    cylinder.CylinderType == ECylinderType.GripUngripReverse ||
                    cylinder.CylinderType == ECylinderType.AlignUnalignReverse ||
                    cylinder.CylinderType == ECylinderType.FlipUnflipReverse ||
                    cylinder.CylinderType == ECylinderType.ClampUnclampReverse
                    )
            {
                cylinder.Forward();
                return;
            }
            cylinder.Backward();

            // For OnOff cylinders in simulation mode, auto-set input feedback
#if SIMULATION
            if (cylinder.CylinderType == ECylinderType.OnOff)
            {
                // Use reflection to access protected InForward property
                var inForwardProperty = cylinder.GetType().GetProperty("InForward", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var inForward = inForwardProperty?.GetValue(cylinder) as List<IDInput>;
                
                if (inForward?.Count > 0)
                {
                    // Set vacuum detect input to false when vacuum is turned off
                    SimulationInputSetter.SetSimModbusInput(inForward[0], false);
                }
            }
#endif
        }

        // InterLock for Cylinder
        public bool CylinderInterLock(ICylinder cylinder, bool isForward, out string CylinderInterlockMsg)
        {
            CylinderInterlockMsg = string.Empty;

            // Interlock for TrRotateLeftRotate
            if (cylinder.Name.Contains("TrRotateLeftRotate") || cylinder.Name.Contains("TrRotateLeftFwBw"))
            {
                CylinderInterlockMsg = "Need Transfer Rotation ZAxis at Ready Position before Moving";
                return Devices?.MotionsInovance?.TransferRotationLZAxis?.IsOnPosition(RecipeSelector?.CurrentRecipe?.TransferRotationLeftRecipe?.ZAxisReadyPosition ?? 0) == true;
            }            

            return true;
        }
        #endregion
    }
    public class PositionTeaching : ObservableObject
    {
        public PositionTeaching(RecipeSelector recipeSelector)
        {
            _recipeSelector = recipeSelector;
            _position = 0.0;
        }
        public string Name { get; set; }
        public string PropertyName { get; set; }
        private double _position;
        public double Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        private IMotion _motion;
        public IMotion Motion
        {
            get => _motion;
            set
            {
                _motion = value;
                OnPropertyChanged(nameof(Motion));
            }
        }
        public ICommand PositionTeachingMoveCommand
        {
            get => new RelayCommand<object>((p) =>
            {
                if (p is TeachViewModel teachViewModel)
                    if (!InterLock(teachViewModel, teachViewModel.SelectedProcess, out string interlockMsg))
                    {
                        MessageBoxEx.ShowDialog($"InterLock Fail! Cannot Move Axis\n\n{interlockMsg}");
                        return;
                    }
                string MoveTo = (string)Application.Current.Resources["str_MoveTo"];
                Motion.MoveAbs(Position);
            });
        }

        // InterLock
        public bool InterLock(TeachViewModel teachViewModel, IProcess<ESequence> process, out string interlockMsg)
        {
            interlockMsg = string.Empty;
            // Interlock TransferFixTure
            if (PropertyName == "TransferFixtureYAxisLoadPosition" || PropertyName == "OutCstTAxisWorkPosition")
            {
                interlockMsg = "Need TransferFixtureUp, RemoveZonePusherCyl1Up, DetachGlassZAxis at Ready Position before Moving";
                return teachViewModel.Devices?.Cylinders?.TransferFixtureUpDown?.IsBackward == true &&
                       teachViewModel.Devices?.Cylinders?.RemoveZonePusherCyl1UpDown?.IsBackward == true &&
                       teachViewModel.Devices?.MotionsInovance?.DetachGlassZAxis?.IsOnPosition(teachViewModel.RecipeSelector?.CurrentRecipe?.DetachRecipe?.DetachZAxisReadyPosition ?? 0) == true;
            }
            // Interlock Detach
            if (PropertyName == "ShuttleTransferXAxisDetachPosition" 
                || PropertyName == "ShuttleTransferXAxisDetachCheckPosition" 
                || PropertyName == "ShuttleTransferXAxisUnloadPosition")
            {
                interlockMsg = "Need ShuttleTransfer Z Axis Ready Position before Moving";
                return teachViewModel.Devices?.MotionsAjin?.ShuttleTransferZAxis?.IsOnPosition(teachViewModel.RecipeSelector?.CurrentRecipe?.DetachRecipe?.ShuttleTransferZAxisReadyPosition ?? 0) == true;
            }
            // Interlock GlassTranfer
            if (PropertyName == "YAxisReadyPosition"
                || PropertyName == "YAxisPickPosition"
                || PropertyName == "YAxisLeftPlacePosition"
                || PropertyName == "YAxisRightPlacePosition")
            {
                interlockMsg = "Need GlassTransfer Z Axis Ready Position before Moving";
                return teachViewModel.Devices?.MotionsInovance?.GlassTransferZAxis?.IsOnPosition(teachViewModel.RecipeSelector?.CurrentRecipe?.GlassTransferRecipe?.ZAxisReadyPosition ?? 0) == true;
            }
            // Interlock TransferShutter Left / Right
            if (PropertyName == "TransferInShuttleLeftYAxisReadyPosition"
                || PropertyName == "TransferInShuttleLeftYAxisPickPosition1"
                || PropertyName == "TransferInShuttleLeftYAxisPickPosition2"
                || PropertyName == "TransferInShuttleLeftYAxisPickPosition3"
                || PropertyName == "TransferInShuttleLeftYAxisPlacePosition"
                || PropertyName == "TransferInShuttleRightYAxisReadyPosition"
                || PropertyName == "TransferInShuttleRightYAxisPickPosition1"
                || PropertyName == "TransferInShuttleRightYAxisPickPosition2"
                || PropertyName == "TransferInShuttleRightYAxisPickPosition3"
                || PropertyName == "TransferInShuttleRightZAxisPlacePosition")
            {
                interlockMsg = "Need GlassTransfer Z Axis Ready Position before Moving";
                return teachViewModel.Devices?.MotionsAjin?.ShuttleTransferZAxis?.IsOnPosition(teachViewModel.RecipeSelector?.CurrentRecipe?.GlassTransferRecipe?.ZAxisReadyPosition ?? 0) == true;
            }
            // Interlock WetClean Left - Check WetCleanPusherLeftUpDown status
            if (PropertyName == "WETCleanLeftXAxisLoadPosition"
                || PropertyName == "WETCleanLeftYAxisLoadPosition"
                || PropertyName == "WETCleanLeftTAxisLoadPosition"
                || PropertyName == "WETCleanLeftXAxisCleanHorizontalPosition"
                || PropertyName == "WETCleanLeftYAxisCleanHorizontalPosition"
                || PropertyName == "WETCleanLeftTAxisCleanHorizontalPosition"
                || PropertyName == "WETCleanLeftXAxisCleanVerticalPosition"
                || PropertyName == "WETCleanLeftYAxisCleanVerticalPosition"
                || PropertyName == "WETCleanLeftTAxisCleanVerticalPosition"
                || PropertyName == "WETCleanLeftXAxisUnloadPosition"
                || PropertyName == "WETCleanLeftYAxisUnloadPosition"
                || PropertyName == "WETCleanLeftTAxisUnloadPosition")
            {
                interlockMsg = "Need WetCleanPusherLeftUpDown at UP position before Moving";
                return teachViewModel.Devices?.Cylinders?.WetCleanPusherLeftUpDown?.IsBackward == true;
            }
            //cylinder.Name   "WetCleanPusherLeftUpDown"  string

            return true; 
        }

        private readonly RecipeSelector _recipeSelector;
        public RecipeSelector RecipeSelector { get; set; }
        public double PositionRecipe { get; set; }
        public ICommand SaveRecipeCommand => new RelayCommand(SaveRecipe);

        private void SaveRecipe()
        {
            if (MessageBoxEx.ShowDialog($"{(string)Application.Current.Resources["str_Save"]}?") == true)
            {
                // Sử dụng giá trị Position hiện tại (có thể từ DataEditor hoặc Get Motion)
                // Không ghi đè Position với Motion.Status.ActualPosition
                SavePosition(Motion.Id, Name);
            }
        }
        public void SavePosition(int id, string name)
        {
            // Sử dụng PropertyName để mapping chính xác - đơn giản và rõ ràng nhất
            if (string.IsNullOrEmpty(PropertyName))
            {
                return;
            }

            // Mapping trực tiếp từ PropertyName đến recipe property
            switch (PropertyName)
            {
                // CSTLoadUnloadRecipe properties
                case "InCstTAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisLoadPosition = Position;
                    break;
                case "InCstTAxisWorkPosition":
                    _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisWorkPosition = Position;
                    break;
                case "OutCstTAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisLoadPosition = Position;
                    break;
                case "OutCstTAxisWorkPosition":
                    _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisWorkPosition = Position;
                    break;

                // TransferFixtureRecipe properties
                case "TransferFixtureYAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.TransferFixtureRecipe.TransferFixtureYAxisLoadPosition = Position;
                    break;
                case "TransferFixtureYAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.TransferFixtureRecipe.TransferFixtureYAxisUnloadPosition = Position;
                    break;

                // DetachRecipe properties
                case "DetachZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisReadyPosition = Position;
                    break;
                case "DetachZAxisDetachReadyPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetachReadyPosition = Position;
                    break;
                case "DetachZAxisDetach1Position":
                    _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach1Position = Position;
                    break;
                case "DetachZAxisDetach2Position":
                    _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach2Position = Position;
                    break;
                case "ShuttleTransferZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisReadyPosition = Position;
                    break;
                case "ShuttleTransferZAxisDetachReadyPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetachReadyPosition = Position;
                    break;
                case "ShuttleTransferZAxisDetach1Position":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetach1Position = Position;
                    break;
                case "ShuttleTransferZAxisDetach2Position":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetach2Position = Position;
                    break;
                case "ShuttleTransferZAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisUnloadPosition = Position;
                    break;
                case "ShuttleTransferXAxisDetachPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachPosition = Position;
                    break;
                case "ShuttleTransferXAxisDetachCheckPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachCheckPosition = Position;
                    break;
                case "ShuttleTransferXAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisUnloadPosition = Position;
                    break;

                // GlassTransferRecipe properties
                case "YAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisReadyPosition = Position;
                    break;
                case "YAxisPickPosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisPickPosition = Position;
                    break;
                case "ZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisReadyPosition = Position;
                    break;
                case "ZAxisPickPosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisPickPosition = Position;
                    break;
                case "YAxisLeftPlacePosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisLeftPlacePosition = Position;
                    break;
                case "ZAxisLeftPlacePosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisLeftPlacePosition = Position;
                    break;
                case "YAxisRightPlacePosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisRightPlacePosition = Position;
                    break;
                case "ZAxisRightPlacePosition":
                    _recipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisRightPlacePosition = Position;
                    break;

                // TransferInShuttleRecipe properties (Left & Right)
                case "TransferInShuttleLeftYAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisReadyPosition = Position;
                    break;
                case "TransferInShuttleLeftZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition = Position;
                    break;
                case "TransferInShuttleLeftYAxisPickPosition1":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPickPosition1 = Position;
                    break;
                case "TransferInShuttleLeftYAxisPickPosition2":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPickPosition2 = Position;
                    break;
                case "TransferInShuttleLeftYAxisPickPosition3":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPickPosition3 = Position;
                    break;
                case "TransferInShuttleLeftZAxisPickPosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisPickPosition = Position;
                    break;
                case "TransferInShuttleLeftYAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.YAxisPlacePosition = Position;
                    break;
                case "TransferInShuttleLeftZAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisPlacePosition = Position;
                    break;
                case "TransferInShuttleRightYAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisReadyPosition = Position;
                    break;
                case "TransferInShuttleRightZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisReadyPosition = Position;
                    break;
                case "TransferInShuttleRightYAxisPickPosition1":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPickPosition1 = Position;
                    break;
                case "TransferInShuttleRightYAxisPickPosition2":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPickPosition2 = Position;
                    break;
                case "TransferInShuttleRightYAxisPickPosition3":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPickPosition3 = Position;
                    break;
                case "TransferInShuttleRightZAxisPickPosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisPickPosition = Position;
                    break;
                case "TransferInShuttleRightYAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.YAxisPlacePosition = Position;
                    break;
                case "TransferInShuttleRightZAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisPlacePosition = Position;
                    break;

                // TransferRotationRecipe properties (Left & Right)
                case "TransferRotationLeftZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisReadyPosition = Position;
                    break;
                case "TransferRotationLeftZAxisPickPosition":
                    _recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisPickPosition = Position;
                    break;
                case "TransferRotationLeftZAxisTransferBeforeRotatePosition":
                    _recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisTransferBeforeRotatePosition = Position;
                    break;
                case "TransferRotationLeftZAxisTransferAfterRotatePosition":
                    _recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisTransferAfterRotatePosition = Position;
                    break;
                case "TransferRotationLeftZAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisPlacePosition = Position;
                    break;
                case "TransferRotationRightZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisReadyPosition = Position;
                    break;
                case "TransferRotationRightZAxisPickPosition":
                    _recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisPickPosition = Position;
                    break;
                case "TransferRotationRightZAxisTransferBeforeRotatePosition":
                    _recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisTransferBeforeRotatePosition = Position;
                    break;
                case "TransferRotationRightZAxisTransferAfterRotatePosition":
                    _recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisTransferAfterRotatePosition = Position;
                    break;
                case "TransferRotationRightZAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisPlacePosition = Position;
                    break;

                // WETCleanLeftRecipe properties

                case "WETCleanLeftXAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisLoadPosition = Position;
                    break;
                case "WETCleanLeftYAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisLoadPosition = Position;
                    break;
                case "WETCleanLeftTAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisLoadPosition = Position;
                    break;
                case "WETCleanLeftXAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisCleanHorizontalPosition = Position;
                    break;
                case "WETCleanLeftYAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisCleanHorizontalPosition = Position;
                    break;
                case "WETCleanLeftTAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisCleanHorizontalPosition = Position;
                    break;
                case "WETCleanLeftXAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisCleanVerticalPosition = Position;
                    break;
                case "WETCleanLeftYAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisCleanVerticalPosition = Position;
                    break;
                case "WETCleanLeftTAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisCleanVerticalPosition = Position;
                    break;
                case "WETCleanLeftXAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisUnloadPosition = Position;
                    break;
                case "WETCleanLeftYAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisUnloadPosition = Position;
                    break;
                case "WETCleanLeftTAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanLeftRecipe.TAxisUnloadPosition = Position;
                    break;

                // WETCleanRightRecipe properties

                case "WETCleanRightXAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisLoadPosition = Position;
                    break;
                case "WETCleanRightYAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisLoadPosition = Position;
                    break;
                case "WETCleanRightTAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisLoadPosition = Position;
                    break;
                case "WETCleanRightXAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisCleanHorizontalPosition = Position;
                    break;
                case "WETCleanRightYAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisCleanHorizontalPosition = Position;
                    break;
                case "WETCleanRightTAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisCleanHorizontalPosition = Position;
                    break;
                case "WETCleanRightXAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisCleanVerticalPosition = Position;
                    break;
                case "WETCleanRightYAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisCleanVerticalPosition = Position;
                    break;
                case "WETCleanRightTAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisCleanVerticalPosition = Position;
                    break;
                case "WETCleanRightXAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisUnloadPosition = Position;
                    break;
                case "WETCleanRightYAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisUnloadPosition = Position;
                    break;
                case "WETCleanRightTAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.WetCleanRightRecipe.TAxisUnloadPosition = Position;
                    break;

                // AFCleanLeftRecipe properties

                case "AFCleanLeftXAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisLoadPosition = Position;
                    break;
                case "AFCleanLeftYAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisLoadPosition = Position;
                    break;
                case "AFCleanLeftTAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisLoadPosition = Position;
                    break;
                case "AFCleanLeftXAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisCleanHorizontalPosition = Position;
                    break;
                case "AFCleanLeftYAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisCleanHorizontalPosition = Position;
                    break;
                case "AFCleanLeftTAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisCleanHorizontalPosition = Position;
                    break;
                case "AFCleanLeftXAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisCleanVerticalPosition = Position;
                    break;
                case "AFCleanLeftYAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisCleanVerticalPosition = Position;
                    break;
                case "AFCleanLeftTAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisCleanVerticalPosition = Position;
                    break;
                case "AFCleanLeftXAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisUnloadPosition = Position;
                    break;
                case "AFCleanLeftYAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.YAxisUnloadPosition = Position;
                    break;
                case "AFCleanLeftTAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanLeftRecipe.TAxisUnloadPosition = Position;
                    break;

                // AFCleanRightRecipe properties

                case "AFCleanRightXAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisLoadPosition = Position;
                    break;
                case "AFCleanRightYAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisLoadPosition = Position;
                    break;
                case "AFCleanRightTAxisLoadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisLoadPosition = Position;
                    break;
                case "AFCleanRightXAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisCleanHorizontalPosition = Position;
                    break;
                case "AFCleanRightYAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisCleanHorizontalPosition = Position;
                    break;
                case "AFCleanRightTAxisCleanHorizontalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisCleanHorizontalPosition = Position;
                    break;
                case "AFCleanRightXAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisCleanVerticalPosition = Position;
                    break;
                case "AFCleanRightYAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisCleanVerticalPosition = Position;
                    break;
                case "AFCleanRightTAxisCleanVerticalPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisCleanVerticalPosition = Position;
                    break;
                case "AFCleanRightXAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisUnloadPosition = Position;
                    break;
                case "AFCleanRightYAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.YAxisUnloadPosition = Position;
                    break;
                case "AFCleanRightTAxisUnloadPosition":
                    _recipeSelector.CurrentRecipe.AfCleanRightRecipe.TAxisUnloadPosition = Position;
                    break;

                // UnloadTransferRecipe properties (Left & Right)
                case "UnloadTransferLeftYAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisReadyPosition = Position;
                    break;
                case "UnloadTransferLeftZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisReadyPosition = Position;
                    break;
                case "UnloadTransferLeftYAxisPickPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPickPosition = Position;
                    break;
                case "UnloadTransferLeftZAxisPickPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisPickPosition = Position;
                    break;
                case "UnloadTransferLeftYAxisPlacePosition1":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition1 = Position;
                    break;
                case "UnloadTransferLeftYAxisPlacePosition2":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition2 = Position;
                    break;
                case "UnloadTransferLeftYAxisPlacePosition3":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition3 = Position;
                    break;
                case "UnloadTransferLeftYAxisPlacePosition4":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.YAxisPlacePosition4 = Position;
                    break;
                case "UnloadTransferLeftZAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisPlacePosition = Position;
                    break;
                case "UnloadTransferRightYAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisReadyPosition = Position;
                    break;
                case "UnloadTransferRightZAxisReadyPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisReadyPosition = Position;
                    break;
                case "UnloadTransferRightYAxisPickPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPickPosition = Position;
                    break;
                case "UnloadTransferRightZAxisPickPosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisPickPosition = Position;
                    break;
                case "UnloadTransferRightYAxisPlacePosition1":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition1 = Position;
                    break;
                case "UnloadTransferRightYAxisPlacePosition2":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition2 = Position;
                    break;
                case "UnloadTransferRightYAxisPlacePosition3":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition3 = Position;
                    break;
                case "UnloadTransferRightYAxisPlacePosition4":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.YAxisPlacePosition4 = Position;
                    break;
                case "UnloadTransferRightZAxisPlacePosition":
                    _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisPlacePosition = Position;
                    break;

                default:
                    break;
            }
            _recipeSelector.Save();
        }

        public void UpdatePositionFromMotion()
        {
            if (Motion != null)
            {
                Position = Math.Round(Motion.Status.ActualPosition, 3);
                OnPropertyChanged(nameof(Position));
            }
        }

    }

}
