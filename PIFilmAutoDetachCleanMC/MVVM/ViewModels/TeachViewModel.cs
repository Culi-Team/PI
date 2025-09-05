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
        public ObservableCollection<IMotion> InConveyorMotions => GetInConveyorMotions();
        public ObservableCollection<IMotion> InWorkConveyorMotions => GetInWorkConveyorMotions();
        public ObservableCollection<IMotion> OutWorkConveyorMotions => GetOutWorkConveyorMotions();
        public ObservableCollection<IMotion> OutConveyorMotions => GetOutConveyorMotions();

        // Detach Tab Motion Properties
        public ObservableCollection<IMotion> TransferFixtureMotions => GetTransferFixtureMotions();
        public ObservableCollection<IMotion> DetachMotions => GetDetachMotions();

        // Clean Tab Motion Properties
        public ObservableCollection<IMotion> GlassTransferMotions => GetGlassTransferMotions();
        public ObservableCollection<IMotion> GlassAlignLeftMotions => GetGlassAlignLeftMotions();
        public ObservableCollection<IMotion> GlassAlignRightMotions => GetGlassAlignRightMotions();
        public ObservableCollection<IMotion> TransferInShuttleLeftMotions => GetTransferInShuttleLeftMotions();
        public ObservableCollection<IMotion> TransferInShuttleRightMotions => GetTransferInShuttleRightMotions();
        public ObservableCollection<IMotion> WETCleanLeftMotions => GetWETCleanLeftMotions();
        public ObservableCollection<IMotion> WETCleanRightMotions => GetWETCleanRightMotions();
        public ObservableCollection<IMotion> AFCleanLeftMotions => GetAFCleanLeftMotions();
        public ObservableCollection<IMotion> AFCleanRightMotions => GetAFCleanRightMotions();
        public ObservableCollection<IMotion> TransferRotationLeftMotions => GetTransferRotationLeftMotions();
        public ObservableCollection<IMotion> TransferRotationRightMotions => GetTransferRotationRightMotions();

        // Unload Tab Motion Properties
        public ObservableCollection<IMotion> UnloadTransferLeftMotions => GetUnloadTransferLeftMotions();
        public ObservableCollection<IMotion> UnloadTransferRightMotions => GetUnloadTransferRightMotions();
        public ObservableCollection<IMotion> UnloadAlignMotions => GetUnloadAlignMotions();

        #endregion

        #region GetCylinder

        #endregion

        #region GetMotions
        // CSTLoadUnload Tab Motions
        private ObservableCollection<IMotion> GetInConveyorMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.InCassetteTAxis);
            return new ObservableCollection<IMotion>(motions);
        }

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

        private ObservableCollection<IMotion> GetOutConveyorMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.OutCassetteTAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        // Detach Tab Motions
        private ObservableCollection<IMotion> GetTransferFixtureMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.FixtureTransferYAxis);
                motions.Add(Devices.MotionsAjin.ShuttleTransferZAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetDetachMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.DetachGlassZAxis);
                motions.Add(Devices.MotionsInovance.ShuttleTransferXAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        // Clean Tab Motions
        private ObservableCollection<IMotion> GetGlassTransferMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassTransferYAxis);
                motions.Add(Devices.MotionsInovance.GlassTransferZAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetGlassAlignLeftMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassTransferYAxis);
                motions.Add(Devices.MotionsInovance.GlassTransferZAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetGlassAlignRightMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassTransferYAxis);
                motions.Add(Devices.MotionsInovance.GlassTransferZAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetTransferInShuttleLeftMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.TransferInShuttleLYAxis);
                motions.Add(Devices.MotionsInovance.TransferInShuttleLZAxis);
                motions.Add(Devices.MotionsAjin.InShuttleLXAxis);
                motions.Add(Devices.MotionsAjin.InShuttleLYAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetTransferInShuttleRightMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.TransferInShuttleRYAxis);
                motions.Add(Devices.MotionsInovance.TransferInShuttleRZAxis);
                motions.Add(Devices.MotionsAjin.InShuttleRXAxis);
                motions.Add(Devices.MotionsAjin.InShuttleRYAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetWETCleanLeftMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.WETCleanLFeedingAxis);
                motions.Add(Devices.MotionsInovance.InShuttleLTAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetWETCleanRightMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.WETCleanRFeedingAxis);
                motions.Add(Devices.MotionsInovance.InShuttleRTAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetAFCleanLeftMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.AFCleanLFeedingAxis);
                motions.Add(Devices.MotionsInovance.InShuttleLTAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetAFCleanRightMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.AFCleanRFeedingAxis);
                motions.Add(Devices.MotionsInovance.InShuttleRTAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetTransferRotationLeftMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.TransferRotationLZAxis);
                motions.Add(Devices.MotionsAjin.OutShuttleLXAxis);
                motions.Add(Devices.MotionsAjin.OutShuttleLYAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetTransferRotationRightMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.TransferRotationRZAxis);
                motions.Add(Devices.MotionsAjin.OutShuttleRXAxis);
                motions.Add(Devices.MotionsAjin.OutShuttleRYAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        // Unload Tab Motions
        private ObservableCollection<IMotion> GetUnloadTransferLeftMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassUnloadLYAxis);
                motions.Add(Devices.MotionsInovance.GlassUnloadLZAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetUnloadTransferRightMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassUnloadRYAxis);
                motions.Add(Devices.MotionsInovance.GlassUnloadRZAxis);
            return new ObservableCollection<IMotion>(motions);
        }

        private ObservableCollection<IMotion> GetUnloadAlignMotions()
        {
            var motions = new List<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassUnloadLYAxis);
                motions.Add(Devices.MotionsInovance.GlassUnloadLZAxis);
                motions.Add(Devices.MotionsInovance.GlassUnloadRYAxis);
                motions.Add(Devices.MotionsInovance.GlassUnloadRZAxis);
            return new ObservableCollection<IMotion>(motions);
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
        private ObservableCollection<PositionTeaching> GetOutWorkConveyorPositionTeachings() => new ObservableCollection<PositionTeaching>();
        private ObservableCollection<PositionTeaching> GetOutConveyorPositionTeachings() => new ObservableCollection<PositionTeaching>();

        // Detach Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetTransferFixturePositionTeachings() => new ObservableCollection<PositionTeaching>();
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
        #endregion

        #region GetProcess
        private ObservableCollection<IProcess<ESequence>> GetProcessList()
        {
            ObservableCollection<IProcess<ESequence>> processes = new ObservableCollection<IProcess<ESequence>>
            {
                // CSTLoadUnload Tab (4 units)
                Processes.InConveyorProcess,
                Processes.InWorkConveyorProcess,
                Processes.OutWorkConveyorProcess,
                Processes.OutConveyorProcess,
                
                // Detach Tab (2 units)
                Processes.TransferFixtureProcess,
                Processes.DetachProcess,
                
                // Clean Tab (10 units)
                Processes.GlassTransferProcess,
                Processes.GlassAlignLeftProcess,
                Processes.GlassAlignRightProcess,
                Processes.TransferInShuttleLeftProcess,
                Processes.TransferInShuttleRightProcess,
                Processes.WETCleanLeftProcess,
                Processes.WETCleanRightProcess,
                Processes.AFCleanLeftProcess,
                Processes.AFCleanRightProcess,
                Processes.TransferRotationLeftProcess,
                Processes.TransferRotationRightProcess,
                
                // Unload Tab (3 units)
                Processes.UnloadTransferLeftProcess,
                Processes.UnloadTransferRightProcess,
                Processes.UnloadAlignProcess,
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
