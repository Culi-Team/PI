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

        // Glass Transfer Tab Motion Properties
        public ObservableCollection<IMotion> GlassTransferMotions => GetGlassTransferMotions();
        public ObservableCollection<PositionTeaching> GlassTransferPositionTeachings => GetGlassTransferPositionTeachings();

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

        // Glass Transfer Tab PositionTeachings
        private ObservableCollection<PositionTeaching> GetGlassTransferPositionTeachings()
        {
            if (RecipeSelector?.CurrentRecipe?.GlassTransferRecipe == null || 
                Devices?.MotionsInovance?.GlassTransferYAxis == null || 
                Devices?.MotionsInovance?.GlassTransferZAxis == null)
                return new ObservableCollection<PositionTeaching>();

            return new ObservableCollection<PositionTeaching>()
            {
                // Y Axis Ready and Place Positions
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferYAxisReadyPosition"]?.ToString() ?? "Glass Transfer Y Axis Ready Position",
                    PropertyName = "YAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferYAxisLeftPlacePosition"]?.ToString() ?? "Glass Transfer Y Axis Left Place Position",
                    PropertyName = "YAxisLeftPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisLeftPlacePosition,
                    Motion = Devices.MotionsInovance.GlassTransferYAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferYAxisRightPlacePosition"]?.ToString() ?? "Glass Transfer Y Axis Right Place Position",
                    PropertyName = "YAxisRightPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisRightPlacePosition,
                    Motion = Devices.MotionsInovance.GlassTransferYAxis
                },
                // Z Axis Ready and Place Positions
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferZAxisReadyPosition"]?.ToString() ?? "Glass Transfer Z Axis Ready Position",
                    PropertyName = "ZAxisReadyPosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisReadyPosition,
                    Motion = Devices.MotionsInovance.GlassTransferZAxis
                },
                new PositionTeaching(RecipeSelector)
                {
                    Name = Application.Current.Resources["str_GlassTransferZAxisLeftPlacePosition"]?.ToString() ?? "Glass Transfer Z Axis Left Place Position",
                    PropertyName = "ZAxisLeftPlacePosition",
                    Position = RecipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisLeftPlacePosition,
                    Motion = Devices.MotionsInovance.GlassTransferZAxis
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
            if (Devices?.MotionsInovance?.GlassTransferYAxis != null)
                motions.Add(Devices.MotionsInovance.GlassTransferYAxis);
            if (Devices?.MotionsInovance?.GlassTransferZAxis != null)
                motions.Add(Devices.MotionsInovance.GlassTransferZAxis);
            return motions;
        }

        #endregion

        #region GetCylinders
        // CSTLoadUnload Tab Cylinders
        private ObservableCollection<ICylinder> GetInWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Devices.Cylinders.InCstStopperUpDown);
            return cylinders;
        }
        
        private ObservableCollection<ICylinder> GetOutWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Devices.Cylinders.OutCstStopperUpDown);
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
            
            return cylinders;
        }

        // Glass Transfer Tab Cylinders
        private ObservableCollection<ICylinder> GetGlassTransferCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Glass Transfer không sử dụng cylinders, chỉ sử dụng vacuum
            return cylinders;
        }

        // Clean Tab Cylinders

        // Unload Tab Cylinders

        #endregion

        #region GetInputs
        // CSTLoadUnload Tab Inputs
        private ObservableCollection<IDInput> GetInWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add In Cassette detection inputs
            inputs.Add(Devices.Inputs.InCstDetect1);
            inputs.Add(Devices.Inputs.InCstDetect2);
            // Add In Cassette button inputs
            inputs.Add(Devices.Inputs.InButton1);
            inputs.Add(Devices.Inputs.InButton2);
            // Add In Cassette light curtain safety input
            inputs.Add(Devices.Inputs.InCstLightCurtainAlarmDetect);
            
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
            
            return inputs;
        }

        // Detach Tab Inputs
        private ObservableCollection<IDInput> GetTransferFixtureInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // TransferFixtureProcess doesn't have specific inputs defined
            // Return empty collection for now
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
            
            return inputs;
        }

        // Clean Tab Inputs

        // Glass Transfer Tab Inputs
        private ObservableCollection<IDInput> GetGlassTransferInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass Transfer vacuum detection inputs
            inputs.Add(Devices.Inputs.GlassTransferVac1);
            inputs.Add(Devices.Inputs.GlassTransferVac2);
            inputs.Add(Devices.Inputs.GlassTransferVac3);
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
            return outputs;
        }

        // Detach Tab Outputs
        private ObservableCollection<IDOutput> GetTransferFixtureOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // TransferFixtureProcess doesn't have specific outputs defined
            return outputs;
        }
        
        private ObservableCollection<IDOutput> GetDetachOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Detach glass shuttle vacuum outputs
            outputs.Add(Devices.Outputs.DetachGlassShtVac1OnOff);
            outputs.Add(Devices.Outputs.DetachGlassShtVac2OnOff);
            outputs.Add(Devices.Outputs.DetachGlassShtVac3OnOff);
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
            return outputs;
        }

        // Clean Tab Outputs

        // Unload Tab Outputs
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
                    case "ZAxisReadyPosition":
                        _recipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisReadyPosition = Position;
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
}
