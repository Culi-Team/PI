using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.Core.Sequence;
using EQX.UI.Controls;
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
        private ObservableCollection<MotionWrapper> _motions;
        public ObservableCollection<MotionWrapper> Motions
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
        public ObservableCollection<MotionWrapper> InConveyorMotions => GetInConveyorMotions();
        public ObservableCollection<MotionWrapper> InWorkConveyorMotions => GetInWorkConveyorMotions();
        public ObservableCollection<MotionWrapper> BufferConveyorMotions => GetBufferConveyorMotions();
        public ObservableCollection<MotionWrapper> OutWorkConveyorMotions => GetOutWorkConveyorMotions();
        public ObservableCollection<MotionWrapper> OutConveyorMotions => GetOutConveyorMotions();

        // Detach Tab Motion Properties
        public ObservableCollection<MotionWrapper> VinylCleanMotions => GetVinylCleanMotions();
        public ObservableCollection<MotionWrapper> RobotLoadMotions => GetRobotLoadMotions();
        public ObservableCollection<MotionWrapper> FixtureAlignMotions => GetFixtureAlignMotions();
        public ObservableCollection<MotionWrapper> TransferFixtureMotions => GetTransferFixtureMotions();
        public ObservableCollection<MotionWrapper> RemoveFilmMotions => GetRemoveFilmMotions();
        public ObservableCollection<MotionWrapper> DetachMotions => GetDetachMotions();

        // Clean Tab Motion Properties
        public ObservableCollection<MotionWrapper> GlassTransferMotions => GetGlassTransferMotions();
        public ObservableCollection<MotionWrapper> GlassAlignLeftMotions => GetGlassAlignLeftMotions();
        public ObservableCollection<MotionWrapper> GlassAlignRightMotions => GetGlassAlignRightMotions();
        public ObservableCollection<MotionWrapper> TransferInShuttleLeftMotions => GetTransferInShuttleLeftMotions();
        public ObservableCollection<MotionWrapper> TransferInShuttleRightMotions => GetTransferInShuttleRightMotions();
        public ObservableCollection<MotionWrapper> WETCleanLeftMotions => GetWETCleanLeftMotions();
        public ObservableCollection<MotionWrapper> WETCleanRightMotions => GetWETCleanRightMotions();
        public ObservableCollection<MotionWrapper> AFCleanLeftMotions => GetAFCleanLeftMotions();
        public ObservableCollection<MotionWrapper> AFCleanRightMotions => GetAFCleanRightMotions();
        public ObservableCollection<MotionWrapper> TransferRotationLeftMotions => GetTransferRotationLeftMotions();
        public ObservableCollection<MotionWrapper> TransferRotationRightMotions => GetTransferRotationRightMotions();

        // Unload Tab Motion Properties
        public ObservableCollection<MotionWrapper> UnloadTransferLeftMotions => GetUnloadTransferLeftMotions();
        public ObservableCollection<MotionWrapper> UnloadTransferRightMotions => GetUnloadTransferRightMotions();
        public ObservableCollection<MotionWrapper> UnloadAlignMotions => GetUnloadAlignMotions();
        public ObservableCollection<MotionWrapper> RobotUnloadMotions => GetRobotUnloadMotions();

        #endregion

        #region GetCylinder

        #endregion

        #region GetMotions
        // CSTLoadUnload Tab Motions
        private ObservableCollection<MotionWrapper> GetInConveyorMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.InCassetteTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.InCassetteTAxis, "In Cassette T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetInWorkConveyorMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.InCassetteTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.InCassetteTAxis, "In Work Cassette T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetBufferConveyorMotions()
        {
            var motions = new List<MotionWrapper>();
            // Buffer conveyor typically doesn't have specific motions
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetOutWorkConveyorMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.OutCassetteTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.OutCassetteTAxis, "Out Work Cassette T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetOutConveyorMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.OutCassetteTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.OutCassetteTAxis, "Out Cassette T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        // Detach Tab Motions
        private ObservableCollection<MotionWrapper> GetVinylCleanMotions()
        {
            var motions = new List<MotionWrapper>();
            // Vinyl clean typically doesn't have specific motions
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetRobotLoadMotions()
        {
            var motions = new List<MotionWrapper>();
            // Robot load typically doesn't have specific motions
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetFixtureAlignMotions()
        {
            var motions = new List<MotionWrapper>();
            // Fixture align typically doesn't have specific motions
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetTransferFixtureMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.FixtureTransferYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.FixtureTransferYAxis, "Fixture Transfer Y Axis"));
            if (Devices?.MotionsAjin?.ShuttleTransferZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.ShuttleTransferZAxis, "Shuttle Transfer Z Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetRemoveFilmMotions()
        {
            var motions = new List<MotionWrapper>();
            // Remove film typically doesn't have specific motions
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetDetachMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.DetachGlassZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.DetachGlassZAxis, "Detach Glass Z Axis"));
            if (Devices?.MotionsInovance?.ShuttleTransferXAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.ShuttleTransferXAxis, "Shuttle Transfer X Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        // Clean Tab Motions
        private ObservableCollection<MotionWrapper> GetGlassTransferMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.GlassTransferYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassTransferYAxis, "Glass Transfer Y Axis"));
            if (Devices?.MotionsInovance?.GlassTransferZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassTransferZAxis, "Glass Transfer Z Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetGlassAlignLeftMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.GlassTransferYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassTransferYAxis, "Glass Align Left Y Axis"));
            if (Devices?.MotionsInovance?.GlassTransferZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassTransferZAxis, "Glass Align Left Z Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetGlassAlignRightMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.GlassTransferYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassTransferYAxis, "Glass Align Right Y Axis"));
            if (Devices?.MotionsInovance?.GlassTransferZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassTransferZAxis, "Glass Align Right Z Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetTransferInShuttleLeftMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.TransferInShuttleLYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.TransferInShuttleLYAxis, "Transfer In Shuttle Left Y Axis"));
            if (Devices?.MotionsInovance?.TransferInShuttleLZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.TransferInShuttleLZAxis, "Transfer In Shuttle Left Z Axis"));
            if (Devices?.MotionsAjin?.InShuttleLXAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.InShuttleLXAxis, "In Shuttle Left X Axis"));
            if (Devices?.MotionsAjin?.InShuttleLYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.InShuttleLYAxis, "In Shuttle Left Y Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetTransferInShuttleRightMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.TransferInShuttleRYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.TransferInShuttleRYAxis, "Transfer In Shuttle Right Y Axis"));
            if (Devices?.MotionsInovance?.TransferInShuttleRZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.TransferInShuttleRZAxis, "Transfer In Shuttle Right Z Axis"));
            if (Devices?.MotionsAjin?.InShuttleRXAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.InShuttleRXAxis, "In Shuttle Right X Axis"));
            if (Devices?.MotionsAjin?.InShuttleRYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.InShuttleRYAxis, "In Shuttle Right Y Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetWETCleanLeftMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.WETCleanLFeedingAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.WETCleanLFeedingAxis, "WET Clean Left Feeding Axis"));
            if (Devices?.MotionsInovance?.InShuttleLTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.InShuttleLTAxis, "In Shuttle Left T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetWETCleanRightMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.WETCleanRFeedingAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.WETCleanRFeedingAxis, "WET Clean Right Feeding Axis"));
            if (Devices?.MotionsInovance?.InShuttleRTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.InShuttleRTAxis, "In Shuttle Right T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetAFCleanLeftMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.AFCleanLFeedingAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.AFCleanLFeedingAxis, "AF Clean Left Feeding Axis"));
            if (Devices?.MotionsInovance?.InShuttleLTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.InShuttleLTAxis, "In Shuttle Left T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetAFCleanRightMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.AFCleanRFeedingAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.AFCleanRFeedingAxis, "AF Clean Right Feeding Axis"));
            if (Devices?.MotionsInovance?.InShuttleRTAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.InShuttleRTAxis, "In Shuttle Right T Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetTransferRotationLeftMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.TransferRotationLZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.TransferRotationLZAxis, "Transfer Rotation Left Z Axis"));
            if (Devices?.MotionsAjin?.OutShuttleLXAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.OutShuttleLXAxis, "Out Shuttle Left X Axis"));
            if (Devices?.MotionsAjin?.OutShuttleLYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.OutShuttleLYAxis, "Out Shuttle Left Y Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetTransferRotationRightMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.TransferRotationRZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.TransferRotationRZAxis, "Transfer Rotation Right Z Axis"));
            if (Devices?.MotionsAjin?.OutShuttleRXAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.OutShuttleRXAxis, "Out Shuttle Right X Axis"));
            if (Devices?.MotionsAjin?.OutShuttleRYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsAjin.OutShuttleRYAxis, "Out Shuttle Right Y Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        // Unload Tab Motions
        private ObservableCollection<MotionWrapper> GetUnloadTransferLeftMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.GlassUnloadLYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadLYAxis, "Glass Unload Left Y Axis"));
            if (Devices?.MotionsInovance?.GlassUnloadLZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadLZAxis, "Glass Unload Left Z Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetUnloadTransferRightMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.GlassUnloadRYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadRYAxis, "Glass Unload Right Y Axis"));
            if (Devices?.MotionsInovance?.GlassUnloadRZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadRZAxis, "Glass Unload Right Z Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetUnloadAlignMotions()
        {
            var motions = new List<MotionWrapper>();
            if (Devices?.MotionsInovance?.GlassUnloadLYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadLYAxis, "Glass Unload Align Left Y Axis"));
            if (Devices?.MotionsInovance?.GlassUnloadLZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadLZAxis, "Glass Unload Align Left Z Axis"));
            if (Devices?.MotionsInovance?.GlassUnloadRYAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadRYAxis, "Glass Unload Align Right Y Axis"));
            if (Devices?.MotionsInovance?.GlassUnloadRZAxis != null)
                motions.Add(new MotionWrapper(Devices.MotionsInovance.GlassUnloadRZAxis, "Glass Unload Align Right Z Axis"));
            return new ObservableCollection<MotionWrapper>(motions);
        }

        private ObservableCollection<MotionWrapper> GetRobotUnloadMotions()
        {
            var motions = new List<MotionWrapper>();
            // Robot unload typically doesn't have specific motions
            return new ObservableCollection<MotionWrapper>(motions);
        }
        #endregion

        #region GetCylinders
        // CSTLoadUnload Tab Cylinders
        private ObservableCollection<ICylinder> GetInConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetInWorkConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetBufferConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetOutWorkConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetOutConveyorCylinders() => new ObservableCollection<ICylinder>();

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
        private ObservableCollection<IDInput> GetInConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetInWorkConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetBufferConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetOutWorkConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetOutConveyorInputs() => new ObservableCollection<IDInput>();

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
        private ObservableCollection<IDOutput> GetInConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetInWorkConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetBufferConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetOutWorkConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetOutConveyorOutputs() => new ObservableCollection<IDOutput>();

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

        #region GetPositionTeachings
        // CSTLoadUnload Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetInConveyorPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetInWorkConveyorPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetBufferConveyorPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetOutWorkConveyorPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetOutConveyorPositionTeachings() => new ObservableCollection<PositionTeaching>();

        // Detach Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetVinylCleanPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetRobotLoadPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetFixtureAlignPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetTransferFixturePositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetRemoveFilmPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetDetachPositionTeachings() => new ObservableCollection<PositionTeaching>();

        // Clean Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetGlassTransferPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetGlassAlignLeftPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetGlassAlignRightPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetTransferInShuttleLeftPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetTransferInShuttleRightPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetWETCleanLeftPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetWETCleanRightPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetAFCleanLeftPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetAFCleanRightPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetTransferRotationLeftPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetTransferRotationRightPositionTeachings() => new ObservableCollection<PositionTeaching>();

        // Unload Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetUnloadTransferLeftPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetUnloadTransferRightPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetUnloadAlignPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetRobotUnloadPositionTeachings() => new ObservableCollection<PositionTeaching>();
        #endregion

        #region GetProcess
        private ObservableCollection<IProcess<ESequence>> GetProcessList()
        {
            ObservableCollection<IProcess<ESequence>> processes = new ObservableCollection<IProcess<ESequence>>
            {
                Processes.TransferFixtureProcess,
                Processes.DetachProcess,
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
            if (SelectedProcess == Processes.InConveyorProcess)
            {
                Motions = GetInConveyorMotions();
                Cylinders = GetInConveyorCylinders();
                Inputs = GetInConveyorInputs();
                Outputs = GetInConveyorOutputs();
                PositionTeachings = GetInConveyorPositionTeachings();
            }
            else if (SelectedProcess == Processes.InWorkConveyorProcess)
            {
                Motions = GetInWorkConveyorMotions();
                Cylinders = GetInWorkConveyorCylinders();
                Inputs = GetInWorkConveyorInputs();
                Outputs = GetInWorkConveyorOutputs();
                PositionTeachings = GetInWorkConveyorPositionTeachings();
            }
            else if (SelectedProcess == Processes.BufferConveyorProcess)
            {
                Motions = GetBufferConveyorMotions();
                Cylinders = GetBufferConveyorCylinders();
                Inputs = GetBufferConveyorInputs();
                Outputs = GetBufferConveyorOutputs();
                PositionTeachings = GetBufferConveyorPositionTeachings();
            }
            else if (SelectedProcess == Processes.OutWorkConveyorProcess)
            {
                Motions = GetOutWorkConveyorMotions();
                Cylinders = GetOutWorkConveyorCylinders();
                Inputs = GetOutWorkConveyorInputs();
                Outputs = GetOutWorkConveyorOutputs();
                PositionTeachings = GetOutWorkConveyorPositionTeachings();
            }
            else if (SelectedProcess == Processes.OutConveyorProcess)
            {
                Motions = GetOutConveyorMotions();
                Cylinders = GetOutConveyorCylinders();
                Inputs = GetOutConveyorInputs();
                Outputs = GetOutConveyorOutputs();
                PositionTeachings = GetOutConveyorPositionTeachings();
            }
            // Detach Tab
            else if (SelectedProcess == Processes.VinylCleanProcess)
            {
                Motions = GetVinylCleanMotions();
                Cylinders = GetVinylCleanCylinders();
                Inputs = GetVinylCleanInputs();
                Outputs = GetVinylCleanOutputs();
                PositionTeachings = GetVinylCleanPositionTeachings();
            }
            else if (SelectedProcess == Processes.RobotLoadProcess)
            {
                Motions = GetRobotLoadMotions();
                Cylinders = GetRobotLoadCylinders();
                Inputs = GetRobotLoadInputs();
                Outputs = GetRobotLoadOutputs();
                PositionTeachings = GetRobotLoadPositionTeachings();
            }
            else if (SelectedProcess == Processes.FixtureAlignProcess)
            {
                Motions = GetFixtureAlignMotions();
                Cylinders = GetFixtureAlignCylinders();
                Inputs = GetFixtureAlignInputs();
                Outputs = GetFixtureAlignOutputs();
                PositionTeachings = GetFixtureAlignPositionTeachings();
            }
            else if (SelectedProcess == Processes.TransferFixtureProcess)
            {
                Motions = GetTransferFixtureMotions();
                Cylinders = GetTransferFixtureCylinders();
                Inputs = GetTransferFixtureInputs();
                Outputs = GetTransferFixtureOutputs();
                PositionTeachings = GetTransferFixturePositionTeachings();
            }
            else if (SelectedProcess == Processes.RemoveFilmProcess)
            {
                Motions = GetRemoveFilmMotions();
                Cylinders = GetRemoveFilmCylinders();
                Inputs = GetRemoveFilmInputs();
                Outputs = GetRemoveFilmOutputs();
                PositionTeachings = GetRemoveFilmPositionTeachings();
            }
            else if (SelectedProcess == Processes.DetachProcess)
            {
                Motions = GetDetachMotions();
                Cylinders = GetDetachCylinders();
                Inputs = GetDetachInputs();
                Outputs = GetDetachOutputs();
                PositionTeachings = GetDetachPositionTeachings();
            }
            // Clean Tab
            else if (SelectedProcess == Processes.GlassTransferProcess)
            {
                Motions = GetGlassTransferMotions();
                Cylinders = GetGlassTransferCylinders();
                Inputs = GetGlassTransferInputs();
                Outputs = GetGlassTransferOutputs();
                PositionTeachings = GetGlassTransferPositionTeachings();
            }
            else if (SelectedProcess == Processes.GlassAlignLeftProcess)
            {
                Motions = GetGlassAlignLeftMotions();
                Cylinders = GetGlassAlignLeftCylinders();
                Inputs = GetGlassAlignLeftInputs();
                Outputs = GetGlassAlignLeftOutputs();
                PositionTeachings = GetGlassAlignLeftPositionTeachings();
            }
            else if (SelectedProcess == Processes.GlassAlignRightProcess)
            {
                Motions = GetGlassAlignRightMotions();
                Cylinders = GetGlassAlignRightCylinders();
                Inputs = GetGlassAlignRightInputs();
                Outputs = GetGlassAlignRightOutputs();
                PositionTeachings = GetGlassAlignRightPositionTeachings();
            }
            else if (SelectedProcess == Processes.TransferInShuttleLeftProcess)
            {
                Motions = GetTransferInShuttleLeftMotions();
                Cylinders = GetTransferInShuttleLeftCylinders();
                Inputs = GetTransferInShuttleLeftInputs();
                Outputs = GetTransferInShuttleLeftOutputs();
                PositionTeachings = GetTransferInShuttleLeftPositionTeachings();
            }
            else if (SelectedProcess == Processes.TransferInShuttleRightProcess)
            {
                Motions = GetTransferInShuttleRightMotions();
                Cylinders = GetTransferInShuttleRightCylinders();
                Inputs = GetTransferInShuttleRightInputs();
                Outputs = GetTransferInShuttleRightOutputs();
                PositionTeachings = GetTransferInShuttleRightPositionTeachings();
            }
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
            // Unload Tab
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
            else if (SelectedProcess == Processes.UnloadAlignProcess)
            {
                Motions = GetUnloadAlignMotions();
                Cylinders = GetUnloadAlignCylinders();
                Inputs = GetUnloadAlignInputs();
                Outputs = GetUnloadAlignOutputs();
                PositionTeachings = GetUnloadAlignPositionTeachings();
            }
            else if (SelectedProcess == Processes.RobotUnloadProcess)
            {
                Motions = GetRobotUnloadMotions();
                Cylinders = GetRobotUnloadCylinders();
                Inputs = GetRobotUnloadInputs();
                Outputs = GetRobotUnloadOutputs();
                PositionTeachings = GetRobotUnloadPositionTeachings();
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
                    string MoveTo = (string)Application.Current.Resources["str_MoveTo"];
                    if (MessageBoxEx.ShowDialog($"{MoveTo} {Name} ?") == true)
                    {
                        Motion.MoveAbs(Position);
                    }
                });
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
                IsActive = Motion.IsOnPosition(Position);
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
                switch (id)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    default:
                        break;
                }
                _recipeSelector.Save();
            }

        }

    }
}
