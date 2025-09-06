using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.Core.Sequence;
using EQX.UI.Controls;
using EQX.UI.Interlock;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

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
        
        // Interlock properties
        private readonly InterlockService _interlockService;
        private bool _isSafetyDoorClosed = true;
        private bool _isAxisMoving = false;
        private bool _isCylinderOk = true;
        
        public bool IsSafetyDoorClosed
        {
            get => _isSafetyDoorClosed;
            set
            {
                _isSafetyDoorClosed = value;
                OnPropertyChanged();
                UpdateInterlockContext();
            }
        }
        
        public bool IsAxisMoving
        {
            get => _isAxisMoving;
            set
            {
                _isAxisMoving = value;
                OnPropertyChanged();
                UpdateInterlockContext();
            }
        }
        
        public bool IsCylinderOk
        {
            get => _isCylinderOk;
            set
            {
                _isCylinderOk = value;
                OnPropertyChanged();
                UpdateInterlockContext();
            }
        }
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

        // CSTLoadUnload Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> InWorkConveyorPositionTeachings => GetInWorkConveyorPositionTeachings();
        public ObservableCollection<PositionTeaching> OutWorkConveyorPositionTeachings => GetOutWorkConveyorPositionTeachings();

        // Detach Tab Motion Properties
        public ObservableCollection<IMotion> TransferFixtureMotions => GetTransferFixtureMotions();
        public ObservableCollection<IMotion> DetachMotions => GetDetachMotions();

        // Detach Tab PositionTeaching Properties
        public ObservableCollection<PositionTeaching> TransferFixturePositionTeachings => GetTransferFixturePositionTeachings();
        public ObservableCollection<PositionTeaching> DetachPositionTeachings => GetDetachPositionTeachings();


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
                // Detach Z Axis positions
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
                // Shuttle Transfer Z Axis positions
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
                // Shuttle Transfer X Axis positions
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

        #endregion

        #region GetMotions
        // CSTLoadUnload Tab Motions
        private ObservableCollection<IMotion> GetInWorkConveyorMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.InCassetteTAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetOutWorkConveyorMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.OutCassetteTAxis);
            return new ObservableCollection<IMotion>(motions);
        }


        // Detach Tab Motions
        private ObservableCollection<IMotion> GetTransferFixtureMotions()
        {
            var motions = new List<IMotion>();
            if (Devices?.MotionsInovance?.FixtureTransferYAxis != null)
                motions.Add(Devices.MotionsInovance.FixtureTransferYAxis);
            // TransferFixtureProcess chỉ sử dụng FixtureTransferYAxis, không sử dụng ShuttleTransferZAxis
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetDetachMotions()
        {
            var motions = new List<IMotion>();
            if (Devices?.MotionsInovance?.DetachGlassZAxis != null)
                motions.Add(Devices.MotionsInovance.DetachGlassZAxis);
            if (Devices?.MotionsAjin?.ShuttleTransferZAxis != null)
                motions.Add(Devices.MotionsAjin.ShuttleTransferZAxis);
            if (Devices?.MotionsInovance?.ShuttleTransferXAxis != null)
                motions.Add(Devices.MotionsInovance.ShuttleTransferXAxis);
            return new ObservableCollection<IMotion>(motions);
        }


        #endregion

        #region GetCylinders
        // CSTLoadUnload Tab Cylinders
        private ObservableCollection<ICylinder> GetInWorkConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetBufferConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetOutWorkConveyorCylinders() => new ObservableCollection<ICylinder>();

        // Detach Tab Cylinders
        private ObservableCollection<ICylinder> GetVinylCleanCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetRobotLoadCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetFixtureAlignCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferFixtureCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetRemoveFilmCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetDetachCylinders() => new ObservableCollection<ICylinder>();

        // Clean Tab Cylinders
        private ObservableCollection<ICylinder> GetGlassTransferCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetGlassAlignLeftCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetGlassAlignRightCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferInShuttleLeftCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferInShuttleRightCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetWETCleanLeftCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetWETCleanRightCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetAFCleanLeftCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetAFCleanRightCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferRotationLeftCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferRotationRightCylinders() => new ObservableCollection<ICylinder>();

        // Unload Tab Cylinders
        private ObservableCollection<ICylinder> GetUnloadTransferLeftCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetUnloadTransferRightCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetUnloadAlignCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetRobotUnloadCylinders() => new ObservableCollection<ICylinder>();
        #endregion

        #region GetInputs
        // CSTLoadUnload Tab Inputs
        private ObservableCollection<IDInput> GetInWorkConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetBufferConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetOutWorkConveyorInputs() => new ObservableCollection<IDInput>();

        // Detach Tab Inputs
        private ObservableCollection<IDInput> GetVinylCleanInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetRobotLoadInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetFixtureAlignInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferFixtureInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetRemoveFilmInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetDetachInputs() => new ObservableCollection<IDInput>();

        // Clean Tab Inputs
        private ObservableCollection<IDInput> GetGlassTransferInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetGlassAlignLeftInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetGlassAlignRightInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferInShuttleLeftInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferInShuttleRightInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetWETCleanLeftInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetWETCleanRightInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetAFCleanLeftInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetAFCleanRightInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferRotationLeftInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferRotationRightInputs() => new ObservableCollection<IDInput>();

        // Unload Tab Inputs
        private ObservableCollection<IDInput> GetUnloadTransferLeftInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetUnloadTransferRightInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetUnloadAlignInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetRobotUnloadInputs() => new ObservableCollection<IDInput>();
        #endregion

        #region GetOutputs
        // CSTLoadUnload Tab Outputs
        private ObservableCollection<IDOutput> GetInWorkConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetBufferConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetOutWorkConveyorOutputs() => new ObservableCollection<IDOutput>();

        // Detach Tab Outputs
        private ObservableCollection<IDOutput> GetVinylCleanOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetRobotLoadOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetFixtureAlignOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferFixtureOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetRemoveFilmOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetDetachOutputs() => new ObservableCollection<IDOutput>();

        // Clean Tab Outputs
        private ObservableCollection<IDOutput> GetGlassTransferOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetGlassAlignLeftOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetGlassAlignRightOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferInShuttleLeftOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferInShuttleRightOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetWETCleanLeftOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetWETCleanRightOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetAFCleanLeftOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetAFCleanRightOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferRotationLeftOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferRotationRightOutputs() => new ObservableCollection<IDOutput>();

        // Unload Tab Outputs
        private ObservableCollection<IDOutput> GetUnloadTransferLeftOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetUnloadTransferRightOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetUnloadAlignOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetRobotUnloadOutputs() => new ObservableCollection<IDOutput>();
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
            
            // Initialize InterlockService
            _interlockService = InterlockService.Default;
            InitializeInterlockRules();
        }
        
        private void InitializeInterlockRules()
        {
            // Rule 1: Safety door must be closed
            _interlockService.RegisterRule(new LambdaInterlockRule(
                "SafetyDoorClosed",
                ctx => ctx.IsSafetyDoorClosed
            ));
            
            // Rule 2: No axis should be moving
            _interlockService.RegisterRule(new LambdaInterlockRule(
                "NoAxisMoving",
                ctx => !ctx.IsAxisMoving
            ));
            
            // Rule 3: Cylinder status must be OK
            _interlockService.RegisterRule(new LambdaInterlockRule(
                "CylinderOk",
                ctx => ctx.IsCylinderOk
            ));
            
            // Subscribe to interlock changes
            _interlockService.InterlockChanged += OnInterlockChanged;
        }
        
        private void UpdateInterlockContext()
        {
            _interlockService.UpdateContext(ctx =>
            {
                ctx.IsSafetyDoorClosed = IsSafetyDoorClosed;
                ctx.IsAxisMoving = IsAxisMoving;
                ctx.IsCylinderOk = IsCylinderOk;
            });
        }
        
        private void OnInterlockChanged(string key, bool isSatisfied)
        {

        }
        
        /// <summary>
        /// Updates interlock status based on actual device states
        /// Call this method periodically to keep interlock status current
        /// </summary>
        public void UpdateInterlockStatus()
        {
            // Update safety door status from inputs
            IsSafetyDoorClosed = Devices?.Inputs?.DoorLock1L?.Value == true &&
                                Devices?.Inputs?.DoorLock1R?.Value == true &&
                                Devices?.Inputs?.DoorLock2L?.Value == true &&
                                Devices?.Inputs?.DoorLock2R?.Value == true &&
                                Devices?.Inputs?.DoorLock3L?.Value == true &&
                                Devices?.Inputs?.DoorLock3R?.Value == true &&
                                Devices?.Inputs?.DoorLock4L?.Value == true &&
                                Devices?.Inputs?.DoorLock4R?.Value == true &&
                                Devices?.Inputs?.DoorLock5L?.Value == true &&
                                Devices?.Inputs?.DoorLock5R?.Value == true &&
                                Devices?.Inputs?.DoorLock6L?.Value == true &&
                                Devices?.Inputs?.DoorLock6R?.Value == true &&
                                Devices?.Inputs?.DoorLock7L?.Value == true &&
                                Devices?.Inputs?.DoorLock7R?.Value == true;
            
            // Update axis moving status
            bool anyAxisMoving = false;
            if (Devices?.MotionsInovance?.All != null)
            {
                anyAxisMoving |= Devices.MotionsInovance.All.Any(m => m.Status.IsMotioning);
            }
            if (Devices?.MotionsAjin?.All != null)
            {
                anyAxisMoving |= Devices.MotionsAjin.All.Any(m => m.Status.IsMotioning);
            }
            IsAxisMoving = anyAxisMoving;
            
            // Update cylinder status (assuming all cylinders should be in proper state)
            bool allCylindersOk = true;
            if (Devices?.Cylinders != null)
            {
                // Check some critical cylinders for safety
                // You can add more cylinder checks as needed
                var criticalCylinders = new[]
                {
                    Devices.Cylinders.InCstStopperUpDown,
                    Devices.Cylinders.OutCstStopperUpDown,
                    Devices.Cylinders.RobotFixtureClampUnclamp,
                    Devices.Cylinders.TransferFixtureUpDown
                };
                allCylindersOk = criticalCylinders.All(c => c != null);
            }
            IsCylinderOk = allCylindersOk;
        }
        public class PositionTeaching : ObservableObject
        {
            public PositionTeaching(RecipeSelector recipeSelector)
            {
                PositionChecker();
                _recipeSelector = recipeSelector;
            }
            private bool isActive;

            public string Name { get; set; }
            public string PropertyName { get; set; } // Tên biến recipe property
            public bool IsActive
            {
                get => isActive;
                set
                {
                    isActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
            public double Position { get; set; }
            public IMotion Motion { get; set; }
            public ICommand PositionTeachingCommand
            {
                get => new RelayCommand<object>((p) =>
                {
                    // Check interlock conditions before moving
                    if (!CheckInterlockConditions())
                    {
                        return;
                    }
                    
                    string MoveTo = (string)Application.Current.Resources["str_MoveTo"];
                    if (MessageBoxEx.ShowDialog($"{MoveTo} {Name} ?") == true)
                    {
                        Motion.MoveAbs(Position);
                    }
                });
            }
            
            private bool CheckInterlockConditions()
            {
                // Get the parent TeachViewModel to access interlock service
                var teachViewModel = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
                if (teachViewModel?.CurrentFrameVM is TeachViewModel parentTeachVM)
                {
                    // Check if all interlock conditions are satisfied
                    var context = new InterlockContext
                    {
                        IsSafetyDoorClosed = parentTeachVM.IsSafetyDoorClosed,
                        IsAxisMoving = parentTeachVM.IsAxisMoving,
                        IsCylinderOk = parentTeachVM.IsCylinderOk
                    };
                    
                    // Check safety door
                    if (!context.IsSafetyDoorClosed)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_SafetyDoorOpen"], 
                                              (string)Application.Current.Resources["str_Warning"]);
                        return false;
                    }
                    
                    // Check if any axis is moving
                    if (context.IsAxisMoving)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_AxisMoving"], 
                                              (string)Application.Current.Resources["str_Warning"]);
                        return false;
                    }
                    
                    // Check cylinder status
                    if (!context.IsCylinderOk)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_CylinderNotOk"], 
                                              (string)Application.Current.Resources["str_Warning"]);
                        return false;
                    }
                }
                
                return true;
            }
            private System.Timers.Timer _timer;
            private readonly RecipeSelector _recipeSelector;

            public void PositionChecker()
            {
                _timer = new System.Timers.Timer(1000);
                _timer.Elapsed += CheckPosition;
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }

            private void CheckPosition(object sender, ElapsedEventArgs e)
            {
                IsActive = Motion?.IsOnPosition(Position) ?? false;
            }
            public RecipeSelector RecipeSelector { get; set; }
            public double PositionRecipe { get; set; }
            public ICommand SaveRecipeCommand => new RelayCommand(SaveRecipe);

            private void SaveRecipe()
            {
                if (MessageBoxEx.ShowDialog($"{(string)Application.Current.Resources["str_Save"]}?") == true)
                {
                    Position = Math.Round(Motion.Status.ActualPosition, 3);
                    SavePosition(Motion.Id, Name);
                }
            }
            public void SavePosition(int id, string name)
            {
                // Sử dụng PropertyName để mapping chính xác - đơn giản và rõ ràng nhất
                if (string.IsNullOrEmpty(PropertyName))
                {
                    System.Diagnostics.Debug.WriteLine($"Error: PropertyName is null or empty for position '{name}'");
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
                    
                    default:
                        System.Diagnostics.Debug.WriteLine($"Warning: PropertyName '{PropertyName}' not mapped in SavePosition method");
                        break;
                }
                _recipeSelector.Save();
            }

        }

    }
}
