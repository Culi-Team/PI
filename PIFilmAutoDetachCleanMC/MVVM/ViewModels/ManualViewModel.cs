using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Device.SpeedController;
using EQX.Core.Display;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.Core.Sequence;
using EQX.UI.Controls;
using EQX.UI.Display;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Converters;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class ManualViewModel : ViewModelBase
    {        
        private readonly System.Timers.Timer pressureUpdateTimer;

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

        // CSTLoadUnload Tab Motion Properties
        public ObservableCollection<IMotion> InConveyorMotions => GetInConveyorMotions();
        public ObservableCollection<IMotion> InWorkConveyorMotions => GetInWorkConveyorMotions();
        public ObservableCollection<IMotion> OutWorkConveyorMotions => GetOutWorkConveyorMotions();

        // CSTLoadUnload Tab Speed Controller Properties
        public ObservableCollection<ISpeedController> InConveyorSpeedControllers => GetInConveyorSpeedControllers();
        public ObservableCollection<ISpeedController> BufferConveyorSpeedControllers => GetBufferConveyorSpeedControllers();
        public ObservableCollection<ISpeedController> OutConveyorSpeedControllers => GetOutConveyorSpeedControllers();

        // CSTLoadUnload Tab Cylinder Properties
        public ObservableCollection<ICylinder> InConveyorCylinders => GetInConveyorCylinders();
        public ObservableCollection<ICylinder> InWorkConveyorCylinders => GetInWorkConveyorCylinders();
        public ObservableCollection<ICylinder> OutWorkConveyorCylinders => GetOutWorkConveyorCylinders();

        // CSTLoadUnload Tab Input Properties
        public ObservableCollection<IDInput> InConveyorInputs => GetInConveyorInputs();
        public ObservableCollection<IDInput> InWorkConveyorInputs => GetInWorkConveyorInputs();
        public ObservableCollection<IDInput> OutWorkConveyorInputs => GetOutWorkConveyorInputs();

        // CSTLoadUnload Tab Output Properties
        public ObservableCollection<IDOutput> InConveyorOutputs => GetInConveyorOutputs();
        public ObservableCollection<IDOutput> InWorkConveyorOutputs => GetInWorkConveyorOutputs();
        public ObservableCollection<IDOutput> OutWorkConveyorOutputs => GetOutWorkConveyorOutputs();

        // Buffer Conveyor Properties
        public ObservableCollection<IMotion> BufferConveyorMotions => GetBufferConveyorMotions();
        public ObservableCollection<ICylinder> BufferConveyorCylinders => GetBufferConveyorCylinders();
        public ObservableCollection<IDInput> BufferConveyorInputs => GetBufferConveyorInputs();
        public ObservableCollection<IDOutput> BufferConveyorOutputs => GetBufferConveyorOutputs();

        // Out Conveyor Properties
        public ObservableCollection<IMotion> OutConveyorMotions => GetOutConveyorMotions();
        public ObservableCollection<ICylinder> OutConveyorCylinders => GetOutConveyorCylinders();
        public ObservableCollection<IDInput> OutConveyorInputs => GetOutConveyorInputs();
        public ObservableCollection<IDOutput> OutConveyorOutputs => GetOutConveyorOutputs();

        // Vinyl Clean Properties
        public ObservableCollection<IMotion> VinylCleanMotions => GetVinylCleanMotions();
        public ObservableCollection<ICylinder> VinylCleanCylinders => GetVinylCleanCylinders();
        public ObservableCollection<IDInput> VinylCleanInputs => GetVinylCleanInputs();
        public ObservableCollection<IDOutput> VinylCleanOutputs => GetVinylCleanOutputs();

        // Robot Load Properties
        public ObservableCollection<IMotion> RobotLoadMotions => GetRobotLoadMotions();
        public ObservableCollection<ICylinder> RobotLoadCylinders => GetRobotLoadCylinders();
        public ObservableCollection<IDInput> RobotLoadInputs => GetRobotLoadInputs();
        public ObservableCollection<IDOutput> RobotLoadOutputs => GetRobotLoadOutputs();

        // Fixture Align Properties
        public ObservableCollection<IMotion> FixtureAlignMotions => GetFixtureAlignMotions();
        public ObservableCollection<ICylinder> FixtureAlignCylinders => GetFixtureAlignCylinders();
        public ObservableCollection<IDInput> FixtureAlignInputs => GetFixtureAlignInputs();
        public ObservableCollection<IDOutput> FixtureAlignOutputs => GetFixtureAlignOutputs();

        // Detach Tab Motion Properties
        public ObservableCollection<IMotion> TransferFixtureMotions => GetTransferFixtureMotions();
        public ObservableCollection<ICylinder> TransferFixtureCylinders => GetTransferFixtureCylinders();
        public ObservableCollection<IDInput> TransferFixtureInputs => GetTransferFixtureInputs();
        public ObservableCollection<IDOutput> TransferFixtureOutputs => GetTransferFixtureOutputs();
        
        public ObservableCollection<IMotion> DetachMotions => GetDetachMotions();
        public ObservableCollection<ICylinder> DetachCylinders => GetDetachCylinders();
        public ObservableCollection<IDInput> DetachInputs => GetDetachInputs();
        public ObservableCollection<IDOutput> DetachOutputs => GetDetachOutputs();

        // Remove Film Properties
        public ObservableCollection<IMotion> RemoveFilmMotions => GetRemoveFilmMotions();
        public ObservableCollection<ICylinder> RemoveFilmCylinders => GetRemoveFilmCylinders();
        public ObservableCollection<IDInput> RemoveFilmInputs => GetRemoveFilmInputs();
        public ObservableCollection<IDOutput> RemoveFilmOutputs => GetRemoveFilmOutputs();

        // Glass Transfer Tab Motion Properties
        public ObservableCollection<IMotion> GlassTransferMotions => GetGlassTransferMotions();
        public ObservableCollection<ICylinder> GlassTransferCylinders => GetGlassTransferCylinders();
        public ObservableCollection<IDInput> GlassTransferInputs => GetGlassTransferInputs();
        public ObservableCollection<IDOutput> GlassTransferOutputs => GetGlassTransferOutputs();

        // Glass Align Properties
        public ObservableCollection<IMotion> GlassAlignMotions => GetGlassAlignMotions();
        public ObservableCollection<ICylinder> GlassAlignCylinders => GetGlassAlignCylinders();
        public ObservableCollection<IDInput> GlassAlignInputs => GetGlassAlignInputs();
        public ObservableCollection<IDOutput> GlassAlignOutputs => GetGlassAlignOutputs();

        // Transfer In Shuttle Properties
        public ObservableCollection<IMotion> TransferInShuttleMotions => GetTransferInShuttleMotions();
        public ObservableCollection<ICylinder> TransferInShuttleCylinders => GetTransferInShuttleCylinders();
        public ObservableCollection<IDInput> TransferInShuttleInputs => GetTransferInShuttleInputs();
        public ObservableCollection<IDOutput> TransferInShuttleOutputs => GetTransferInShuttleOutputs();

        // Transfer Rotation Properties
        public ObservableCollection<IMotion> TransferRotationMotions => GetTransferRotationMotions();
        public ObservableCollection<ICylinder> TransferRotationCylinders => GetTransferRotationCylinders();
        public ObservableCollection<IDInput> TransferRotationInputs => GetTransferRotationInputs();
        public ObservableCollection<IDOutput> TransferRotationOutputs => GetTransferRotationOutputs();

        // Unload Transfer Properties
        public ObservableCollection<IMotion> UnloadTransferMotions => GetUnloadTransferMotions();
        public ObservableCollection<ICylinder> UnloadTransferCylinders => GetUnloadTransferCylinders();
        public ObservableCollection<IDInput> UnloadTransferInputs => GetUnloadTransferInputs();
        public ObservableCollection<IDOutput> UnloadTransferOutputs => GetUnloadTransferOutputs();

        // Unload Align Properties
        public ObservableCollection<IMotion> UnloadAlignMotions => GetUnloadAlignMotions();
        public ObservableCollection<ICylinder> UnloadAlignCylinders => GetUnloadAlignCylinders();
        public ObservableCollection<IDInput> UnloadAlignInputs => GetUnloadAlignInputs();
        public ObservableCollection<IDOutput> UnloadAlignOutputs => GetUnloadAlignOutputs();

        // Robot Unload Properties
        public ObservableCollection<IMotion> RobotUnloadMotions => GetRobotUnloadMotions();
        public ObservableCollection<ICylinder> RobotUnloadCylinders => GetRobotUnloadCylinders();
        public ObservableCollection<IDInput> RobotUnloadInputs => GetRobotUnloadInputs();
        public ObservableCollection<IDOutput> RobotUnloadOutputs => GetRobotUnloadOutputs();

        // ManualViewModel specific properties
        public CleanRecipe WetCleanLeftRecipe { get; }
        public CleanRecipe WetCleanRightRecipe { get; }
        public CleanRecipe AfCleanLeftRecipe { get; }
        public CleanRecipe AfCleanRightRecipe { get; }
        public Regulators Regulators { get; set; }
        public Inputs DeviceInputs { get; }
        public Outputs DeviceOutputs { get; }
        public MotionsInovance MotionsInovance { get; }
        public MotionsAjin MotionsAjin { get; }

        // Properties for binding
        public bool MotionsInovanceIsConnected => MotionsInovance.MotionControllerInovance.IsConnected;
        public bool MotionAjinIsConnected => MotionsAjin.All.All(m => m.IsConnected);
        
        // Robot properties - dựa trên input status
        public bool RobotLoadIsConnected => DeviceInputs.LoadRobPeriRdy.Value && DeviceInputs.LoadRobIoActconf.Value;
        public bool RobotUnloadIsConnected => DeviceInputs.LoadRobPeriRdy.Value && DeviceInputs.LoadRobIoActconf.Value; 

        private int _speed = 1000;
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; OnPropertyChanged(); }
        }

        private double _wetCleanLeftPressure;
        public double WetCleanLeftPressure
        {
            get => _wetCleanLeftPressure;
            set { _wetCleanLeftPressure = value; OnPropertyChanged(); }
        }

        private double _wetCleanRightPressure;
        public double WetCleanRightPressure
        {
            get => _wetCleanRightPressure;
            set { _wetCleanRightPressure = value; OnPropertyChanged(); }
        }

        private double _afCleanLeftPressure;
        public double AfCleanLeftPressure
        {
            get => _afCleanLeftPressure;
            set { _afCleanLeftPressure = value; OnPropertyChanged(); }
        }

        private double _afCleanRightPressure;
        public double AfCleanRightPressure
        {
            get => _afCleanRightPressure;
            set { _afCleanRightPressure = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands
        public ICommand SetWetCleanLeftPressureCommand => new RelayCommand(() =>
        {
            Regulators.WetCleanLRegulator.SetPressure(WetCleanLeftRecipe.CylinderPushPressure);
            WetCleanLeftPressure = Regulators.WetCleanLRegulator.GetPressure();
        });

        public ICommand SetWetCleanRightPressureCommand => new RelayCommand(() =>
        {
            Regulators.WetCleanRRegulator.SetPressure(WetCleanRightRecipe.CylinderPushPressure);
            WetCleanRightPressure = Regulators.WetCleanRRegulator.GetPressure();
        });

        public ICommand SetAfCleanLeftPressureCommand => new RelayCommand(() =>
        {
            Regulators.AfCleanLRegulator.SetPressure(AfCleanLeftRecipe.CylinderPushPressure);
            AfCleanLeftPressure = Regulators.AfCleanLRegulator.GetPressure();
        });

        public ICommand SetAfCleanRightPressureCommand => new RelayCommand(() =>
        {
            Regulators.AfCleanRRegulator.SetPressure(AfCleanRightRecipe.CylinderPushPressure);
            AfCleanRightPressure = Regulators.AfCleanRRegulator.GetPressure();
        });

        public ICommand ConnectMotionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MotionsAjin.All.ForEach(m => m.Connect());
                    MotionsInovance.MotionControllerInovance.Connect();
                    OnPropertyChanged(nameof(MotionsInovanceIsConnected));
                    OnPropertyChanged(nameof(MotionAjinIsConnected));
                });
            }
        }

        public ICommand MotionAjinConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MotionsAjin.All.ForEach(m => m.Connect());
                    OnPropertyChanged(nameof(MotionAjinIsConnected));
                });
            }
        }

        public ICommand RobotLoadConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Kích hoạt robot load - set các output cần thiết
                    DeviceOutputs.LoadRobMoveEnable.Value = true;
                    DeviceOutputs.LoadRobDrivesOn.Value = true;
                    OnPropertyChanged(nameof(RobotLoadIsConnected));
                });
            }
        }

        public ICommand RobotUnloadConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // Kích hoạt robot unload - set các output cần thiết
                    DeviceOutputs.LoadRobMoveEnable.Value = true;
                    DeviceOutputs.LoadRobDrivesOn.Value = true;
                    OnPropertyChanged(nameof(RobotUnloadIsConnected));
                });
            }
        }
        #endregion

        #region GetMotions
        // CSTLoadUnload Tab Motions
        private ObservableCollection<IMotion> GetInConveyorMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // In Conveyor không có motion riêng, chỉ có speed controller
            return motions;
        }

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

        private ObservableCollection<IMotion> GetBufferConveyorMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Buffer Conveyor không có motion, chỉ có speed controller
            return motions;
        }

        private ObservableCollection<IMotion> GetOutConveyorMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Out Conveyor không có motion, chỉ có speed controller
            return motions;
        }

        private ObservableCollection<IMotion> GetVinylCleanMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Vinyl Clean không có motion trong manual view, chỉ sử dụng cylinders
            return motions;
        }

        private ObservableCollection<IMotion> GetRobotLoadMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Robot Load không có motion riêng, sử dụng robot
            return motions;
        }

        private ObservableCollection<IMotion> GetFixtureAlignMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Fixture Align không có motion riêng
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

        private ObservableCollection<IMotion> GetRemoveFilmMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Remove Film không có motion riêng
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

        private ObservableCollection<IMotion> GetGlassAlignMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Glass Align không có motion riêng
            return motions;
        }

        private ObservableCollection<IMotion> GetTransferInShuttleMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Add Transfer In Shuttle Left motions
            motions.Add(Devices.MotionsInovance.TransferInShuttleLYAxis);
            motions.Add(Devices.MotionsInovance.TransferInShuttleLZAxis);
            
            // Add Transfer In Shuttle Right motions
            motions.Add(Devices.MotionsInovance.TransferInShuttleRYAxis);
            motions.Add(Devices.MotionsInovance.TransferInShuttleRZAxis);
            
            return motions;
        }

        private ObservableCollection<IMotion> GetTransferRotationMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Add Transfer Rotation motions
            motions.Add(Devices.MotionsInovance.TransferRotationLZAxis);
            motions.Add(Devices.MotionsInovance.TransferRotationRZAxis);
            
            return motions;
        }

        private ObservableCollection<IMotion> GetUnloadTransferMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Add Unload Transfer Left motions
            motions.Add(Devices.MotionsInovance.GlassUnloadLYAxis);
            motions.Add(Devices.MotionsInovance.GlassUnloadLZAxis);
            
            // Add Unload Transfer Right motions
            motions.Add(Devices.MotionsInovance.GlassUnloadRYAxis);
            motions.Add(Devices.MotionsInovance.GlassUnloadRZAxis);
            
            return motions;
        }

        private ObservableCollection<IMotion> GetUnloadAlignMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Unload Align không có motion riêng
            return motions;
        }

        private ObservableCollection<IMotion> GetRobotUnloadMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Robot Unload sử dụng robot, không có motion riêng
            return motions;
        }

        private ObservableCollection<IMotion> GetWetCleanMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // Wet Clean không có motion riêng, chỉ sử dụng pressure regulators
            return motions;
        }

        private ObservableCollection<IMotion> GetAfCleanMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // AF Clean không có motion riêng, chỉ sử dụng pressure regulators
            return motions;
        }

        #endregion

        #region GetSpeedControllers
        // CSTLoadUnload Tab Speed Controllers
        private ObservableCollection<ISpeedController> GetInConveyorSpeedControllers()
        {
            ObservableCollection<ISpeedController> speedControllers = new ObservableCollection<ISpeedController>();
            // Add In Conveyor rollers
            speedControllers.Add(Devices.SpeedControllerList.InConveyorRoller1);
            speedControllers.Add(Devices.SpeedControllerList.InConveyorRoller2);
            speedControllers.Add(Devices.SpeedControllerList.InConveyorRoller3);
            return speedControllers;
        }

        private ObservableCollection<ISpeedController> GetBufferConveyorSpeedControllers()
        {
            ObservableCollection<ISpeedController> speedControllers = new ObservableCollection<ISpeedController>();
            // Add Buffer Conveyor rollers
            speedControllers.Add(Devices.SpeedControllerList.BufferConveyorRoller1);
            speedControllers.Add(Devices.SpeedControllerList.BufferConveyorRoller2);
            return speedControllers;
        }

        private ObservableCollection<ISpeedController> GetOutConveyorSpeedControllers()
        {
            ObservableCollection<ISpeedController> speedControllers = new ObservableCollection<ISpeedController>();
            // Add Out Conveyor rollers
            speedControllers.Add(Devices.SpeedControllerList.OutConveyorRoller1);
            speedControllers.Add(Devices.SpeedControllerList.OutConveyorRoller2);
            speedControllers.Add(Devices.SpeedControllerList.OutConveyorRoller3);
            return speedControllers;
        }
        #endregion

        #region GetCylinders
        // CSTLoadUnload Tab Cylinders
        private ObservableCollection<ICylinder> GetInConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add In Conveyor stopper cylinder
            cylinders.Add(Devices.Cylinders.InCstStopperUpDown);
            return cylinders;
        }

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

        private ObservableCollection<ICylinder> GetBufferConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Devices.Cylinders.BufferCvStopper1UpDown);
            cylinders.Add(Devices.Cylinders.BufferCvStopper2UpDown);
            cylinders.Add(Devices.Cylinders.InCvSupportBufferUpDown);
            cylinders.Add(Devices.Cylinders.OutCvSupportBufferUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetOutConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Devices.Cylinders.OutCstStopperUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetVinylCleanCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Devices.Cylinders.VinylCleanFixture1ClampUnclamp);
            cylinders.Add(Devices.Cylinders.VinylCleanRollerFwBw);
            cylinders.Add(Devices.Cylinders.VinylCleanPusherRollerUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetRobotLoadCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Devices.Cylinders.RobotFixtureClampUnclamp);
            cylinders.Add(Devices.Cylinders.RobotFixtureAlignFwBw);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetFixtureAlignCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Devices.Cylinders.AlignFixtureCylFwBw);
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

        private ObservableCollection<ICylinder> GetRemoveFilmCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Remove Film fix cylinders
            cylinders.Add(Devices.Cylinders.RemoveZoneFixCyl1FwBw);
            cylinders.Add(Devices.Cylinders.RemoveZoneFixCyl2FwBw);
            
            // Add Remove Film transfer cylinder
            cylinders.Add(Devices.Cylinders.RemoveZoneTrCylFwBw);
            
            // Add Remove Film up/down cylinders
            cylinders.Add(Devices.Cylinders.RemoveZoneZCyl1UpDown);
            cylinders.Add(Devices.Cylinders.RemoveZoneZCyl2UpDown);
            
            // Add Remove Film clamp cylinder
            cylinders.Add(Devices.Cylinders.RemoveZoneCylClampUnclamp);
            
            // Add Remove Film pusher cylinders
            cylinders.Add(Devices.Cylinders.RemoveZonePusherCyl1UpDown);
            cylinders.Add(Devices.Cylinders.RemoveZonePusherCyl2UpDown);
            
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
            
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetGlassAlignCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Glass Align Left cylinders
            cylinders.Add(Devices.Cylinders.AlignStageL1AlignUnalign);
            cylinders.Add(Devices.Cylinders.AlignStageL2AlignUnalign);
            cylinders.Add(Devices.Cylinders.AlignStageL3AlignUnalign);
            
            // Add Glass Align Right cylinders
            cylinders.Add(Devices.Cylinders.AlignStageR1AlignUnalign);
            cylinders.Add(Devices.Cylinders.AlignStageR2AlignUnalign);
            cylinders.Add(Devices.Cylinders.AlignStageR3AlignUnalign);
            
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetTransferInShuttleCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Transfer In Shuttle không có cylinder riêng
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetTransferRotationCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Transfer Rotation Left cylinders
            cylinders.Add(Devices.Cylinders.TrRotateLeftRotate);
            cylinders.Add(Devices.Cylinders.TrRotateLeftFwBw);
            
            // Add Transfer Rotation Right cylinders
            cylinders.Add(Devices.Cylinders.TrRotateRightRotate);
            cylinders.Add(Devices.Cylinders.TrRotateRightFwBw);
            
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetUnloadTransferCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Unload Transfer không có cylinder riêng
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetUnloadAlignCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Unload Align cylinders
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl4UpDown);
            
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetRobotUnloadCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Robot Unload cylinders
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl4UpDown);
            
            return cylinders;
        }

        // Clean Tab Cylinders
        private ObservableCollection<ICylinder> GetWetCleanCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Wet Clean không có cylinder riêng, sử dụng pressure regulators
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetAfCleanCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // AF Clean không có cylinder riêng, sử dụng pressure regulators
            return cylinders;
        }

        // Unload Tab Cylinders

        #endregion

        #region GetInputs
        // CSTLoadUnload Tab Inputs
        private ObservableCollection<IDInput> GetInConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add In Conveyor detection inputs
            inputs.Add(Devices.Inputs.InCstDetect1);
            inputs.Add(Devices.Inputs.InCstDetect2);
            // Add In Conveyor button inputs
            inputs.Add(Devices.Inputs.InButton1);
            inputs.Add(Devices.Inputs.InButton2);
            // Add In Conveyor light curtain safety input
            inputs.Add(Devices.Inputs.InCstLightCurtainAlarmDetect);
            return inputs;
        }

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

        private ObservableCollection<IDInput> GetBufferConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Buffer Cassette detection inputs
            inputs.Add(Devices.Inputs.BufferCstDetect1);
            inputs.Add(Devices.Inputs.BufferCstDetect2);
            return inputs;
        }

        private ObservableCollection<IDInput> GetOutConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
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

        private ObservableCollection<IDInput> GetVinylCleanInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Vinyl Clean detection inputs
            inputs.Add(Devices.Inputs.VinylCleanFixtureDetect);
            inputs.Add(Devices.Inputs.VinylCleanFullDetect);
            inputs.Add(Devices.Inputs.VinylCleanRunoffDetect);
            return inputs;
        }

        private ObservableCollection<IDInput> GetRobotLoadInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Robot Load sử dụng virtual I/O, không có physical inputs riêng
            return inputs;
        }

        private ObservableCollection<IDInput> GetFixtureAlignInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Fixture Align detection inputs
            inputs.Add(Devices.Inputs.AlignFixtureDetect);
            inputs.Add(Devices.Inputs.AlignFixtureTiltDetect);
            inputs.Add(Devices.Inputs.AlignFixtureReverseDetect);
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

        private ObservableCollection<IDInput> GetRemoveFilmInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Remove Film sử dụng virtual I/O, không có physical inputs riêng
            return inputs;
        }

        // Clean Tab Inputs
        private ObservableCollection<IDInput> GetWetCleanInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Wet Clean không có physical inputs riêng, sử dụng pressure monitoring
            return inputs;
        }

        private ObservableCollection<IDInput> GetAfCleanInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // AF Clean không có physical inputs riêng, sử dụng pressure monitoring
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
            return inputs;
        }

        private ObservableCollection<IDInput> GetGlassAlignInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            
            // Add Glass Align Left vacuum detection inputs
            inputs.Add(Devices.Inputs.AlignStageLVac1);
            inputs.Add(Devices.Inputs.AlignStageLVac2);
            inputs.Add(Devices.Inputs.AlignStageLVac3);
            
            // Add Glass Align Left glass detection inputs
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect1);
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect2);
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect3);
            
            // Add Glass Align Right vacuum detection inputs
            inputs.Add(Devices.Inputs.AlignStageRVac1);
            inputs.Add(Devices.Inputs.AlignStageRVac2);
            inputs.Add(Devices.Inputs.AlignStageRVac3);
            
            // Add Glass Align Right glass detection inputs
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect1);
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect2);
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect3);
            
            return inputs;
        }

        private ObservableCollection<IDInput> GetTransferInShuttleInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            
            // Add Transfer In Shuttle vacuum detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleLVac);
            inputs.Add(Devices.Inputs.TransferInShuttleRVac);
            
            return inputs;
        }

        private ObservableCollection<IDInput> GetTransferRotationInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            
            // Add Transfer Rotation vacuum detection inputs
            inputs.Add(Devices.Inputs.TrRotateLeft1Vac);
            inputs.Add(Devices.Inputs.TrRotateRight1Vac);
            
            return inputs;
        }

        private ObservableCollection<IDInput> GetUnloadTransferInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            
            // Add Unload Transfer vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadTransferLVac);
            inputs.Add(Devices.Inputs.UnloadTransferRVac);
            
            return inputs;
        }

        private ObservableCollection<IDInput> GetUnloadAlignInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            
            // Add Unload Align vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac1);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac2);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac3);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac4);
            
            // Add Unload Align glass detection inputs
            inputs.Add(Devices.Inputs.UnloadGlassDetect1);
            inputs.Add(Devices.Inputs.UnloadGlassDetect2);
            inputs.Add(Devices.Inputs.UnloadGlassDetect3);
            inputs.Add(Devices.Inputs.UnloadGlassDetect4);
            
            return inputs;
        }

        private ObservableCollection<IDInput> GetRobotUnloadInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            
            // Add Robot Unload glass detection inputs
            inputs.Add(Devices.Inputs.UnloadRobotDetect1);
            inputs.Add(Devices.Inputs.UnloadRobotDetect2);
            inputs.Add(Devices.Inputs.UnloadRobotDetect3);
            inputs.Add(Devices.Inputs.UnloadRobotDetect4);
            
            return inputs;
        }

        // Unload Tab Inputs

        #endregion

        #region GetOutputs
        // CSTLoadUnload Tab Outputs
        private ObservableCollection<IDOutput> GetInConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add In Conveyor button lamp outputs
            outputs.Add(Devices.Outputs.InButtonLamp1);
            outputs.Add(Devices.Outputs.InButtonLamp2);
            // Add In Conveyor light curtain muting output
            outputs.Add(Devices.Outputs.InCstLightCurtainMuting1);
            outputs.Add(Devices.Outputs.InCstLightCurtainMuting2);
            return outputs;
        }

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

        private ObservableCollection<IDOutput> GetBufferConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Buffer Conveyor không có output riêng
            return outputs;
        }

        private ObservableCollection<IDOutput> GetOutConveyorOutputs()
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

        private ObservableCollection<IDOutput> GetVinylCleanOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vinyl Clean motor output
            outputs.Add(Devices.Outputs.VinylCleanMotorOnOff);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetRobotLoadOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Robot Load sử dụng virtual I/O, không có physical outputs riêng
            return outputs;
        }

        private ObservableCollection<IDOutput> GetFixtureAlignOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Fixture Align sử dụng virtual I/O, không có physical outputs riêng
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

        private ObservableCollection<IDOutput> GetRemoveFilmOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Remove Film sử dụng virtual I/O, không có physical outputs riêng
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

        private ObservableCollection<IDOutput> GetGlassAlignOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            
            // Add Glass Align Left vacuum outputs
            outputs.Add(Devices.Outputs.AlignStageLVac1OnOff);
            outputs.Add(Devices.Outputs.AlignStageLVac2OnOff);
            outputs.Add(Devices.Outputs.AlignStageLVac3OnOff);
            
            // Add Glass Align Right vacuum outputs
            outputs.Add(Devices.Outputs.AlignStageRVac1OnOff);
            outputs.Add(Devices.Outputs.AlignStageRVac2OnOff);
            outputs.Add(Devices.Outputs.AlignStageRVac3OnOff);
            
            return outputs;
        }

        private ObservableCollection<IDOutput> GetTransferInShuttleOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            
            // Add Transfer In Shuttle vacuum control outputs
            outputs.Add(Devices.Outputs.TransferInShuttleLVacOnOff);
            outputs.Add(Devices.Outputs.TransferInShuttleRVacOnOff);
            
            return outputs;
        }

        private ObservableCollection<IDOutput> GetTransferRotationOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            
            // Add Transfer Rotation vacuum control outputs
            outputs.Add(Devices.Outputs.TrRotateLeft1VacOnOff);
            outputs.Add(Devices.Outputs.TrRotateRight1VacOnOff);
            
            return outputs;
        }

        private ObservableCollection<IDOutput> GetUnloadTransferOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            
            // Add Unload Transfer vacuum control outputs
            outputs.Add(Devices.Outputs.UnloadTransferLVacOnOff);
            outputs.Add(Devices.Outputs.UnloadTransferRVacOnOff);
            
            return outputs;
        }

        private ObservableCollection<IDOutput> GetUnloadAlignOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            
            // Add Unload Align vacuum control outputs
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac1OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac2OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac3OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac4OnOff);
            
            return outputs;
        }

        private ObservableCollection<IDOutput> GetRobotUnloadOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            
            // Add Robot Unload vacuum control outputs
            outputs.Add(Devices.Outputs.UnloadRobotVac1OnOff);
            outputs.Add(Devices.Outputs.UnloadRobotVac2OnOff);
            outputs.Add(Devices.Outputs.UnloadRobotVac3OnOff);
            outputs.Add(Devices.Outputs.UnloadRobotVac4OnOff);
            
            return outputs;
        }

        // Clean Tab Outputs
        private ObservableCollection<IDOutput> GetWetCleanOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Wet Clean không có physical outputs riêng, sử dụng pressure regulators
            return outputs;
        }

        private ObservableCollection<IDOutput> GetAfCleanOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // AF Clean không có physical outputs riêng, sử dụng pressure regulators
            return outputs;
        }

        // Unload Tab Outputs
        #endregion

        #region Wet Clean Properties
        public ObservableCollection<IMotion> WetCleanMotions => GetWetCleanMotions();
        public ObservableCollection<ICylinder> WetCleanCylinders => GetWetCleanCylinders();
        public ObservableCollection<IDInput> WetCleanInputs => GetWetCleanInputs();
        public ObservableCollection<IDOutput> WetCleanOutputs => GetWetCleanOutputs();
        #endregion

        #region AF Clean Properties
        public ObservableCollection<IMotion> AfCleanMotions => GetAfCleanMotions();
        public ObservableCollection<ICylinder> AfCleanCylinders => GetAfCleanCylinders();
        public ObservableCollection<IDInput> AfCleanInputs => GetAfCleanInputs();
        public ObservableCollection<IDOutput> AfCleanOutputs => GetAfCleanOutputs();
        #endregion

        #region GetProcess
        private ObservableCollection<IProcess<ESequence>> GetProcessList()
        {
            ObservableCollection<IProcess<ESequence>> processes = new ObservableCollection<IProcess<ESequence>>
            {
                // CSTLoadUnload Tab (5 units)
                Processes.InConveyorProcess,
                Processes.InWorkConveyorProcess,
                Processes.BufferConveyorProcess,
                Processes.OutWorkConveyorProcess,
                Processes.OutConveyorProcess,
                
                // Detach Tab (6 units)
                Processes.RobotLoadProcess,
                Processes.VinylCleanProcess,
                Processes.FixtureAlignProcess,
                Processes.TransferFixtureProcess,
                Processes.DetachProcess,
                Processes.RemoveFilmProcess,
                
                // Clean Tab (9 units)
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
                
                // Unload Tab (4 units)
                Processes.UnloadTransferLeftProcess,
                Processes.UnloadTransferRightProcess,
                Processes.UnloadAlignProcess,
                Processes.RobotUnloadProcess,
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
            }
            else if (SelectedProcess == Processes.InWorkConveyorProcess)
            {
                Motions = GetInWorkConveyorMotions();
                Cylinders = GetInWorkConveyorCylinders();
                Inputs = GetInWorkConveyorInputs();
                Outputs = GetInWorkConveyorOutputs();
            }
            else if (SelectedProcess == Processes.BufferConveyorProcess)
            {
                Motions = GetBufferConveyorMotions();
                Cylinders = GetBufferConveyorCylinders();
                Inputs = GetBufferConveyorInputs();
                Outputs = GetBufferConveyorOutputs();
            }
            else if (SelectedProcess == Processes.OutWorkConveyorProcess)
            {
                Motions = GetOutWorkConveyorMotions();
                Cylinders = GetOutWorkConveyorCylinders();
                Inputs = GetOutWorkConveyorInputs();
                Outputs = GetOutWorkConveyorOutputs();
            }
            else if (SelectedProcess == Processes.OutConveyorProcess)
            {
                Motions = GetOutConveyorMotions();
                Cylinders = GetOutConveyorCylinders();
                Inputs = GetOutConveyorInputs();
                Outputs = GetOutConveyorOutputs();
            }

            // Detach Tab
            else if (SelectedProcess == Processes.RobotLoadProcess)
            {
                Motions = GetRobotLoadMotions();
                Cylinders = GetRobotLoadCylinders();
                Inputs = GetRobotLoadInputs();
                Outputs = GetRobotLoadOutputs();
            }
            else if (SelectedProcess == Processes.VinylCleanProcess)
            {
                Motions = GetVinylCleanMotions();
                Cylinders = GetVinylCleanCylinders();
                Inputs = GetVinylCleanInputs();
                Outputs = GetVinylCleanOutputs();
            }
            else if (SelectedProcess == Processes.FixtureAlignProcess)
            {
                Motions = GetFixtureAlignMotions();
                Cylinders = GetFixtureAlignCylinders();
                Inputs = GetFixtureAlignInputs();
                Outputs = GetFixtureAlignOutputs();
            }
            else if (SelectedProcess == Processes.TransferFixtureProcess)
            {
                Motions = GetTransferFixtureMotions();
                Cylinders = GetTransferFixtureCylinders();
                Inputs = GetTransferFixtureInputs();
                Outputs = GetTransferFixtureOutputs();
            }
            else if (SelectedProcess == Processes.DetachProcess)
            {
                Motions = GetDetachMotions();
                Cylinders = GetDetachCylinders();
                Inputs = GetDetachInputs();
                Outputs = GetDetachOutputs();
            }
            else if (SelectedProcess == Processes.RemoveFilmProcess)
            {
                Motions = GetRemoveFilmMotions();
                Cylinders = GetRemoveFilmCylinders();
                Inputs = GetRemoveFilmInputs();
                Outputs = GetRemoveFilmOutputs();
            }

            // Clean Tab
            else if (SelectedProcess == Processes.GlassTransferProcess)
            {
                Motions = GetGlassTransferMotions();
                Cylinders = GetGlassTransferCylinders();
                Inputs = GetGlassTransferInputs();
                Outputs = GetGlassTransferOutputs();
            }
            else if (SelectedProcess == Processes.GlassAlignLeftProcess || SelectedProcess == Processes.GlassAlignRightProcess)
            {
                Motions = GetGlassAlignMotions();
                Cylinders = GetGlassAlignCylinders();
                Inputs = GetGlassAlignInputs();
                Outputs = GetGlassAlignOutputs();
            }
            else if (SelectedProcess == Processes.TransferInShuttleLeftProcess || SelectedProcess == Processes.TransferInShuttleRightProcess)
            {
                Motions = GetTransferInShuttleMotions();
                Cylinders = GetTransferInShuttleCylinders();
                Inputs = GetTransferInShuttleInputs();
                Outputs = GetTransferInShuttleOutputs();
            }
            else if (SelectedProcess == Processes.TransferRotationLeftProcess || SelectedProcess == Processes.TransferRotationRightProcess)
            {
                Motions = GetTransferRotationMotions();
                Cylinders = GetTransferRotationCylinders();
                Inputs = GetTransferRotationInputs();
                Outputs = GetTransferRotationOutputs();
            }
            else if (SelectedProcess == Processes.WETCleanLeftProcess || SelectedProcess == Processes.WETCleanRightProcess)
            {
                Motions = GetWetCleanMotions();
                Cylinders = GetWetCleanCylinders();
                Inputs = GetWetCleanInputs();
                Outputs = GetWetCleanOutputs();
            }
            else if (SelectedProcess == Processes.AFCleanLeftProcess || SelectedProcess == Processes.AFCleanRightProcess)
            {
                Motions = GetAfCleanMotions();
                Cylinders = GetAfCleanCylinders();
                Inputs = GetAfCleanInputs();
                Outputs = GetAfCleanOutputs();
            }
            else if (SelectedProcess == Processes.UnloadTransferLeftProcess || SelectedProcess == Processes.UnloadTransferRightProcess)
            {
                Motions = GetUnloadTransferMotions();
                Cylinders = GetUnloadTransferCylinders();
                Inputs = GetUnloadTransferInputs();
                Outputs = GetUnloadTransferOutputs();
            }
            else if (SelectedProcess == Processes.UnloadAlignProcess)
            {
                Motions = GetUnloadAlignMotions();
                Cylinders = GetUnloadAlignCylinders();
                Inputs = GetUnloadAlignInputs();
                Outputs = GetUnloadAlignOutputs();
            }
            else if (SelectedProcess == Processes.RobotUnloadProcess)
            {
                Motions = GetRobotUnloadMotions();
                Cylinders = GetRobotUnloadCylinders();
                Inputs = GetRobotUnloadInputs();
                Outputs = GetRobotUnloadOutputs();
            }
        }
        #endregion

        #region Pressure Timer
        private void PressureUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                WetCleanLeftPressure = Regulators.WetCleanLRegulator.GetPressure();
                WetCleanRightPressure = Regulators.WetCleanRRegulator.GetPressure();
                AfCleanLeftPressure = Regulators.AfCleanLRegulator.GetPressure();
                AfCleanRightPressure = Regulators.AfCleanRRegulator.GetPressure();
            });
        }
        #endregion

        public ManualViewModel(Devices devices, MachineStatus machineStatus, RecipeList recipeList, RecipeSelector recipeSelector, Processes processes, DataViewModel dataViewModel,
            [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
            [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetCleanRightRecipe,
            [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
            [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe)
        {
            Devices = devices;
            MachineStatus = machineStatus;
            RecipeList = recipeList;
            RecipeSelector = recipeSelector;
            Processes = processes;
            DataViewModel = dataViewModel;
            
            DeviceInputs = devices.Inputs;
            DeviceOutputs = devices.Outputs;
            Regulators = devices.Regulators;
            MotionsInovance = devices.MotionsInovance;
            MotionsAjin = devices.MotionsAjin;
            WetCleanLeftRecipe = wetCleanLeftRecipe;
            WetCleanRightRecipe = wetCleanRightRecipe;
            AfCleanLeftRecipe = afCleanLeftRecipe;
            AfCleanRightRecipe = afCleanRightRecipe;

            WetCleanLeftPressure = Regulators.WetCleanLRegulator.GetPressure();
            WetCleanRightPressure = Regulators.WetCleanRRegulator.GetPressure();
            AfCleanLeftPressure = Regulators.AfCleanLRegulator.GetPressure();
            AfCleanRightPressure = Regulators.AfCleanRRegulator.GetPressure();

            SetPrimaryInteractiveCommand = new RelayCommand(
                () => ActiveScreen = InteractiveScreen.Primary,
                () => !IsViewOnly);
            SetSecondaryInteractiveCommand = new RelayCommand(
                () => ActiveScreen = InteractiveScreen.Secondary,
                () => !IsViewOnly);
            _displayManager.EnableExtend();
            _activeScreen = InteractiveScreen.Primary;
            MoveMainWindowTo(_activeScreen);
            _isViewOnly = false;

            pressureUpdateTimer = new System.Timers.Timer(500);
            pressureUpdateTimer.Elapsed += PressureUpdateTimer_Elapsed;
            pressureUpdateTimer.Start();
            
            SelectedProcess = ProcessListTeaching.FirstOrDefault();
        }

        public IRelayCommand SetPrimaryInteractiveCommand { get; }
        public IRelayCommand SetSecondaryInteractiveCommand { get; }

        public InteractiveScreen ActiveScreen
        {
            get => _activeScreen;
            set
            {
                if (_activeScreen != value)
                {
                    _activeScreen = value;
                    OnPropertyChanged(nameof(ActiveScreen));
                    MoveMainWindowTo(_activeScreen);
                    if (_isViewOnly)
                    {
                        _viewOnlyOverlay.ShowOn(_activeScreen == InteractiveScreen.Primary ? 1 : 0);
                    }
                }
            }
        }

        public bool IsViewOnly
        {
            get => _isViewOnly;
            set
            {
                if (_isViewOnly != value)
                {
                    _isViewOnly = value;
                    OnPropertyChanged(nameof(IsViewOnly));
                    if (_isViewOnly)
                    {
                        _viewOnlyOverlay.ShowOn(_activeScreen == InteractiveScreen.Primary ? 1 : 0);
                    }
                    else
                    {
                        _viewOnlyOverlay.Hide();
                    }
                    SetPrimaryInteractiveCommand.NotifyCanExecuteChanged();
                    SetSecondaryInteractiveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private readonly DisplayManager _displayManager = new();
        private readonly ViewOnlyOverlay _viewOnlyOverlay = new();
        private InteractiveScreen _activeScreen;
        private bool _isViewOnly;

        private void MoveMainWindowTo(InteractiveScreen screen)
        {
            var monitors = GetMonitors();
            if (screen == InteractiveScreen.Primary && monitors.Count > 0)
            {
                PositionWindow(monitors[0]);
            }
            else if (screen == InteractiveScreen.Secondary && monitors.Count > 1)
            {
                PositionWindow(monitors[1]);
            }
        }

        private static void PositionWindow(MonitorInfo info)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Application.Current.MainWindow;
                if (window != null)
                {
                    var wasMaximized = window.WindowState == WindowState.Maximized;
                    if (wasMaximized)
                    {
                        window.WindowState = WindowState.Normal;
                    }
                    window.Left = info.Left;
                    window.Top = info.Top;
                    window.Width = info.Width;
                    window.Height = info.Height;
                    if (wasMaximized)
                    {
                        window.WindowState = WindowState.Maximized;
                    }
                }
            });
        }

        private static IList<MonitorInfo> GetMonitors()
        {
            var result = new List<MonitorInfo>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                (IntPtr hMonitor, IntPtr hdc, ref RECT rect, IntPtr data) =>
                {
                    result.Add(new MonitorInfo(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top));
                    return true;
                }, IntPtr.Zero);
            return result;
        }

        private record MonitorInfo(int Left, int Top, int Width, int Height);

        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdc, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
