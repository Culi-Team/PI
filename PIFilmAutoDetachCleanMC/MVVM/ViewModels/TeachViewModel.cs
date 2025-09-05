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


        #endregion

        #region GetPositionTeaching
        // CSTLoadUnload Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetInConveyorPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.CstLoadUnloadRecipe == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_InConveyorPosition"]?.ToString() ?? "In Conveyor Position", 
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisLoadPosition,
                    Motion = Devices?.MotionsInovance?.InCassetteTAxis
                },
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_InConveyorWorkPosition"]?.ToString() ?? "In Conveyor Work Position", 
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisWorkPosition,
                    Motion = Devices?.MotionsInovance?.InCassetteTAxis
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetInWorkConveyorPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.CstLoadUnloadRecipe == null || Devices?.MotionsInovance?.InCassetteTAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_InCstTAxisLoadPosition"]?.ToString() ?? "In Cassette T Axis Load Position", 
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisLoadPosition, 
                    Motion = Devices.MotionsInovance.InCassetteTAxis 
                },
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_InCstTAxisWorkPosition"]?.ToString() ?? "In Cassette T Axis Work Position", 
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
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisLoadPosition, 
                    Motion = Devices.MotionsInovance.OutCassetteTAxis 
                },
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_OutCstTAxisWorkPosition"]?.ToString() ?? "Out Cassette T Axis Work Position", 
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisWorkPosition, 
                    Motion = Devices.MotionsInovance.OutCassetteTAxis 
                }
            };
        }

        private ObservableCollection<PositionTeaching> GetOutConveyorPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.CstLoadUnloadRecipe == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_OutConveyorPosition"]?.ToString() ?? "Out Conveyor Position", 
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisLoadPosition,
                    Motion = Devices?.MotionsInovance?.OutCassetteTAxis
                },
                new PositionTeaching(RecipeSelector) 
                { 
                    Name = Application.Current.Resources["str_OutConveyorWorkPosition"]?.ToString() ?? "Out Conveyor Work Position", 
                    Position = RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisWorkPosition,
                    Motion = Devices?.MotionsInovance?.OutCassetteTAxis
                }
            };
        }
        // 
        private ObservableCollection<PositionTeaching> GetTransferFixturePositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.TransferFixtureRecipe == null || Devices?.MotionsInovance?.FixtureTransferYAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferFixtureYAxisLoadPosition"]?.ToString() ?? "Transfer Fixture Y Axis Load Position",
                    Position = RecipeSelector.CurrentRecipe.TransferFixtureRecipe.TransferFixtureYAxisLoadPosition,
                    Motion = Devices.MotionsInovance.FixtureTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_TransferFixtureYAxisUnloadPosition"]?.ToString() ?? "Transfer Fixture Y Axis Unload Position",
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
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_DetachZAxisDetachReadyPosition"]?.ToString() ?? "Detach Z Axis Detach Ready Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetachReadyPosition,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_DetachZAxisDetach1Position"]?.ToString() ?? "Detach Z Axis Detach 1 Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach1Position,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_DetachZAxisDetach2Position"]?.ToString() ?? "Detach Z Axis Detach 2 Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach2Position,
                    Motion = Devices.MotionsInovance.DetachGlassZAxis
                },
                // Shuttle Transfer Z Axis positions
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisReadyPosition"]?.ToString() ?? "Shuttle Transfer Z Axis Ready Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisReadyPosition,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisDetachReadyPosition"]?.ToString() ?? "Shuttle Transfer Z Axis Detach Ready Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetachReadyPosition,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisDetach1Position"]?.ToString() ?? "Shuttle Transfer Z Axis Detach 1 Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetach1Position,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisDetach2Position"]?.ToString() ?? "Shuttle Transfer Z Axis Detach 2 Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetach2Position,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferZAxisUnloadPosition"]?.ToString() ?? "Shuttle Transfer Z Axis Unload Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisUnloadPosition,
                    Motion = Devices.MotionsAjin.ShuttleTransferZAxis
                },
                // Shuttle Transfer X Axis positions
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferXAxisDetachPosition"]?.ToString() ?? "Shuttle Transfer X Axis Detach Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachPosition,
                    Motion = Devices.MotionsInovance.ShuttleTransferXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferXAxisDetachCheckPosition"]?.ToString() ?? "Shuttle Transfer X Axis Detach Check Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachCheckPosition,
                    Motion = Devices.MotionsInovance.ShuttleTransferXAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_ShuttleTransferXAxisUnloadPosition"]?.ToString() ?? "Shuttle Transfer X Axis Unload Position",
                    Position = RecipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisUnloadPosition,
                    Motion = Devices.MotionsInovance.ShuttleTransferXAxis
                }
            };
        }

        #endregion

        #region GetMotions
        // CSTLoadUnload Tab Motions
        private ObservableCollection<IMotion> GetInConveyorMotions()
        {
            var motions = new List<IMotion>();
            if (Devices?.MotionsInovance?.InCassetteTAxis != null)
            {
                motions.Add(Devices.MotionsInovance.InCassetteTAxis);
            }
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
            if (Devices?.MotionsInovance?.OutCassetteTAxis != null)
            {
                motions.Add(Devices.MotionsInovance.OutCassetteTAxis);
            }
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
                switch (id)
                {
                    case 1: // InCassetteTAxis
                        if (name.Contains("Load") || name.Contains("Position"))
                        {
                            _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisLoadPosition = Position;
                        }
                        else if (name.Contains("Work"))
                        {
                            _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.InCstTAxisWorkPosition = Position;
                        }
                        break;
                    case 2: // OutCassetteTAxis
                        if (name.Contains("Load") || name.Contains("Position"))
                        {
                            _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisLoadPosition = Position;
                        }
                        else if (name.Contains("Work"))
                        {
                            _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.OutCstTAxisWorkPosition = Position;
                        }
                        break;
                    case 3: // FixtureTransferYAxis
                        if (name.Contains("Load"))
                        {
                            _recipeSelector.CurrentRecipe.TransferFixtureRecipe.TransferFixtureYAxisLoadPosition = Position;
                        }
                        else if (name.Contains("Unload"))
                        {
                            _recipeSelector.CurrentRecipe.TransferFixtureRecipe.TransferFixtureYAxisUnloadPosition = Position;
                        }
                        break;
                    case 4: // DetachGlassZAxis
                        if (name.Contains("Ready"))
                        {
                            _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisReadyPosition = Position;
                        }
                        else if (name.Contains("Detach Ready"))
                        {
                            _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetachReadyPosition = Position;
                        }
                        else if (name.Contains("Detach1"))
                        {
                            _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach1Position = Position;
                        }
                        else if (name.Contains("Detach2"))
                        {
                            _recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetach2Position = Position;
                        }
                        break;
                    case 5: // ShuttleTransferXAxis
                        if (name.Contains("Detach Position"))
                        {
                            _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachPosition = Position;
                        }
                        else if (name.Contains("Detach Check"))
                        {
                            _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisDetachCheckPosition = Position;
                        }
                        else if (name.Contains("Unload"))
                        {
                            _recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferXAxisUnloadPosition = Position;
                        }
                        break;
                    default:
                        break;
                }
                _recipeSelector.Save();
            }

        }

    }
}
