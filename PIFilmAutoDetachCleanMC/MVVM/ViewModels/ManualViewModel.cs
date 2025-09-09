using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Device.SpeedController;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Input;
using EQX.Core.Motion;
using EQX.Core.InOut;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class ManualViewModel : ViewModelBase
    {
        private readonly System.Timers.Timer pressureUpdateTimer;

        public ManualViewModel(Devices devices,
            [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
            [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetCleanRightRecipe,
            [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
            [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe)
        {
            Inputs = devices.Inputs;
            Outputs = devices.Outputs;
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

            pressureUpdateTimer = new System.Timers.Timer(500);
            pressureUpdateTimer.Elapsed += PressureUpdateTimer_Elapsed;
            pressureUpdateTimer.Start();
        }

        private void PressureUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            WetCleanLeftPressure = Regulators.WetCleanLRegulator.GetPressure();
            WetCleanRightPressure = Regulators.WetCleanRRegulator.GetPressure();
            AfCleanLeftPressure = Regulators.AfCleanLRegulator.GetPressure();
            AfCleanRightPressure = Regulators.AfCleanRRegulator.GetPressure();
        }
        private int _speed = 1000;

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; OnPropertyChanged(); }
        }

        public CleanRecipe WetCleanLeftRecipe { get; }
        public CleanRecipe WetCleanRightRecipe { get; }
        public CleanRecipe AfCleanLeftRecipe { get; }
        public CleanRecipe AfCleanRightRecipe { get; }
        public Regulators Regulators { get; set; }
        public Inputs Inputs { get; }
        public Outputs Outputs { get; }
        public MotionsInovance MotionsInovance { get; }
        public MotionsAjin MotionsAjin { get; }

        // Properties for binding
        public bool MotionsInovanceIsConnected => MotionsInovance.MotionControllerInovance.IsConnected;
        public bool MotionAjinIsConnected => MotionsAjin.All.All(m => m.IsConnected);
        
        // Robot properties - dựa trên input status
        public bool RobotLoadIsConnected => Inputs.LoadRobPeriRdy.Value && Inputs.LoadRobIoActconf.Value;
        public bool RobotUnloadIsConnected => Inputs.LoadRobPeriRdy.Value && Inputs.LoadRobIoActconf.Value; // Giả định cùng robot

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

        public ICommand SetWetCleanLeftPressureCommand => new RelayCommand(() =>
        {
            Regulators.WetCleanLRegulator.SetPressure(WetCleanLeftRecipe.CylinderPushPressure);
        });

        public ICommand SetWetCleanRightPressureCommand => new RelayCommand(() =>
        {
            Regulators.WetCleanRRegulator.SetPressure(WetCleanRightRecipe.CylinderPushPressure);
        });

        public ICommand SetAfCleanLeftPressureCommand => new RelayCommand(() =>
        {
            Regulators.AfCleanLRegulator.SetPressure(AfCleanLeftRecipe.CylinderPushPressure);
        });

        public ICommand SetAfCleanRightPressureCommand => new RelayCommand(() =>
        {
            Regulators.AfCleanRRegulator.SetPressure(AfCleanRightRecipe.CylinderPushPressure);
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
                    Outputs.LoadRobMoveEnable.Value = true;
                    Outputs.LoadRobDrivesOn.Value = true;
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
                    Outputs.LoadRobMoveEnable.Value = true;
                    Outputs.LoadRobDrivesOn.Value = true;
                    OnPropertyChanged(nameof(RobotUnloadIsConnected));
                });
            }
        }

        #region Unit Tab Properties

        // CSTLoadUnload Tab Properties
        public ObservableCollection<IMotion> InConveyorMotions => GetInConveyorMotions();
        public ObservableCollection<IMotion> InWorkConveyorMotions => GetInWorkConveyorMotions();
        public ObservableCollection<IMotion> BufferConveyorMotions => GetBufferConveyorMotions();
        public ObservableCollection<IMotion> OutWorkConveyorMotions => GetOutWorkConveyorMotions();
        public ObservableCollection<IMotion> OutConveyorMotions => GetOutConveyorMotions();

        public ObservableCollection<ICylinder> InConveyorCylinders => GetInConveyorCylinders();
        public ObservableCollection<ICylinder> InWorkConveyorCylinders => GetInWorkConveyorCylinders();
        public ObservableCollection<ICylinder> BufferConveyorCylinders => GetBufferConveyorCylinders();
        public ObservableCollection<ICylinder> OutWorkConveyorCylinders => GetOutWorkConveyorCylinders();
        public ObservableCollection<ICylinder> OutConveyorCylinders => GetOutConveyorCylinders();

        public ObservableCollection<IDInput> InConveyorInputs => GetInConveyorInputs();
        public ObservableCollection<IDInput> InWorkConveyorInputs => GetInWorkConveyorInputs();
        public ObservableCollection<IDInput> BufferConveyorInputs => GetBufferConveyorInputs();
        public ObservableCollection<IDInput> OutWorkConveyorInputs => GetOutWorkConveyorInputs();
        public ObservableCollection<IDInput> OutConveyorInputs => GetOutConveyorInputs();

        public ObservableCollection<IDOutput> InConveyorOutputs => GetInConveyorOutputs();
        public ObservableCollection<IDOutput> InWorkConveyorOutputs => GetInWorkConveyorOutputs();
        public ObservableCollection<IDOutput> BufferConveyorOutputs => GetBufferConveyorOutputs();
        public ObservableCollection<IDOutput> OutWorkConveyorOutputs => GetOutWorkConveyorOutputs();
        public ObservableCollection<IDOutput> OutConveyorOutputs => GetOutConveyorOutputs();

        // Detach Tab Properties
        public ObservableCollection<IMotion> VinylCleanMotions => GetVinylCleanMotions();
        public ObservableCollection<IMotion> RobotLoadMotions => GetRobotLoadMotions();
        public ObservableCollection<IMotion> FixtureAlignMotions => GetFixtureAlignMotions();
        public ObservableCollection<IMotion> TransferFixtureMotions => GetTransferFixtureMotions();
        public ObservableCollection<IMotion> RemoveFilmMotions => GetRemoveFilmMotions();
        public ObservableCollection<IMotion> DetachMotions => GetDetachMotions();

        public ObservableCollection<ICylinder> VinylCleanCylinders => GetVinylCleanCylinders();
        public ObservableCollection<ICylinder> RobotLoadCylinders => GetRobotLoadCylinders();
        public ObservableCollection<ICylinder> FixtureAlignCylinders => GetFixtureAlignCylinders();
        public ObservableCollection<ICylinder> TransferFixtureCylinders => GetTransferFixtureCylinders();
        public ObservableCollection<ICylinder> RemoveFilmCylinders => GetRemoveFilmCylinders();
        public ObservableCollection<ICylinder> DetachCylinders => GetDetachCylinders();

        public ObservableCollection<IDInput> VinylCleanInputs => GetVinylCleanInputs();
        public ObservableCollection<IDInput> RobotLoadInputs => GetRobotLoadInputs();
        public ObservableCollection<IDInput> FixtureAlignInputs => GetFixtureAlignInputs();
        public ObservableCollection<IDInput> TransferFixtureInputs => GetTransferFixtureInputs();
        public ObservableCollection<IDInput> RemoveFilmInputs => GetRemoveFilmInputs();
        public ObservableCollection<IDInput> DetachInputs => GetDetachInputs();

        public ObservableCollection<IDOutput> VinylCleanOutputs => GetVinylCleanOutputs();
        public ObservableCollection<IDOutput> RobotLoadOutputs => GetRobotLoadOutputs();
        public ObservableCollection<IDOutput> FixtureAlignOutputs => GetFixtureAlignOutputs();
        public ObservableCollection<IDOutput> TransferFixtureOutputs => GetTransferFixtureOutputs();
        public ObservableCollection<IDOutput> RemoveFilmOutputs => GetRemoveFilmOutputs();
        public ObservableCollection<IDOutput> DetachOutputs => GetDetachOutputs();

        // Clean Tab Properties
        public ObservableCollection<IMotion> GlassTransferMotions => GetGlassTransferMotions();
        public ObservableCollection<IMotion> GlassAlignMotions => GetGlassAlignMotions();
        public ObservableCollection<IMotion> TransferInShuttleMotions => GetTransferInShuttleMotions();
        public ObservableCollection<IMotion> TransferRotationMotions => GetTransferRotationMotions();

        public ObservableCollection<ICylinder> GlassTransferCylinders => GetGlassTransferCylinders();
        public ObservableCollection<ICylinder> GlassAlignCylinders => GetGlassAlignCylinders();
        public ObservableCollection<ICylinder> TransferInShuttleCylinders => GetTransferInShuttleCylinders();
        public ObservableCollection<ICylinder> TransferRotationCylinders => GetTransferRotationCylinders();

        public ObservableCollection<IDInput> GlassTransferInputs => GetGlassTransferInputs();
        public ObservableCollection<IDInput> GlassAlignInputs => GetGlassAlignInputs();
        public ObservableCollection<IDInput> TransferInShuttleInputs => GetTransferInShuttleInputs();
        public ObservableCollection<IDInput> TransferRotationInputs => GetTransferRotationInputs();

        public ObservableCollection<IDOutput> GlassTransferOutputs => GetGlassTransferOutputs();
        public ObservableCollection<IDOutput> GlassAlignOutputs => GetGlassAlignOutputs();
        public ObservableCollection<IDOutput> TransferInShuttleOutputs => GetTransferInShuttleOutputs();
        public ObservableCollection<IDOutput> TransferRotationOutputs => GetTransferRotationOutputs();

        // Unload Tab Properties
        public ObservableCollection<IMotion> UnloadTransferMotions => GetUnloadTransferMotions();
        public ObservableCollection<IMotion> UnloadAlignMotions => GetUnloadAlignMotions();
        public ObservableCollection<IMotion> RobotUnloadMotions => GetRobotUnloadMotions();

        public ObservableCollection<ICylinder> UnloadTransferCylinders => GetUnloadTransferCylinders();
        public ObservableCollection<ICylinder> UnloadAlignCylinders => GetUnloadAlignCylinders();
        public ObservableCollection<ICylinder> RobotUnloadCylinders => GetRobotUnloadCylinders();

        public ObservableCollection<IDInput> UnloadTransferInputs => GetUnloadTransferInputs();
        public ObservableCollection<IDInput> UnloadAlignInputs => GetUnloadAlignInputs();
        public ObservableCollection<IDInput> RobotUnloadInputs => GetRobotUnloadInputs();

        public ObservableCollection<IDOutput> UnloadTransferOutputs => GetUnloadTransferOutputs();
        public ObservableCollection<IDOutput> UnloadAlignOutputs => GetUnloadAlignOutputs();
        public ObservableCollection<IDOutput> RobotUnloadOutputs => GetRobotUnloadOutputs();

        #endregion

        #region Get Methods

        // CSTLoadUnload Tab Get Methods
        private ObservableCollection<IMotion> GetInConveyorMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetInWorkConveyorMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetBufferConveyorMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetOutWorkConveyorMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetOutConveyorMotions() => new ObservableCollection<IMotion>();

        private ObservableCollection<ICylinder> GetInConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetInWorkConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetBufferConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetOutWorkConveyorCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetOutConveyorCylinders() => new ObservableCollection<ICylinder>();

        private ObservableCollection<IDInput> GetInConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetInWorkConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetBufferConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetOutWorkConveyorInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetOutConveyorInputs() => new ObservableCollection<IDInput>();

        private ObservableCollection<IDOutput> GetInConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetInWorkConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetBufferConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetOutWorkConveyorOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetOutConveyorOutputs() => new ObservableCollection<IDOutput>();

        // Detach Tab Get Methods
        private ObservableCollection<IMotion> GetVinylCleanMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetRobotLoadMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetFixtureAlignMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetTransferFixtureMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetRemoveFilmMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetDetachMotions() => new ObservableCollection<IMotion>();

        private ObservableCollection<ICylinder> GetVinylCleanCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetRobotLoadCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetFixtureAlignCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferFixtureCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetRemoveFilmCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetDetachCylinders() => new ObservableCollection<ICylinder>();

        private ObservableCollection<IDInput> GetVinylCleanInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetRobotLoadInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetFixtureAlignInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferFixtureInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetRemoveFilmInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetDetachInputs() => new ObservableCollection<IDInput>();

        private ObservableCollection<IDOutput> GetVinylCleanOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetRobotLoadOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetFixtureAlignOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferFixtureOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetRemoveFilmOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetDetachOutputs() => new ObservableCollection<IDOutput>();

        // Clean Tab Get Methods
        private ObservableCollection<IMotion> GetGlassTransferMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetGlassAlignMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetTransferInShuttleMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetTransferRotationMotions() => new ObservableCollection<IMotion>();

        private ObservableCollection<ICylinder> GetGlassTransferCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetGlassAlignCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferInShuttleCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetTransferRotationCylinders() => new ObservableCollection<ICylinder>();

        private ObservableCollection<IDInput> GetGlassTransferInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetGlassAlignInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferInShuttleInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetTransferRotationInputs() => new ObservableCollection<IDInput>();

        private ObservableCollection<IDOutput> GetGlassTransferOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetGlassAlignOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferInShuttleOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetTransferRotationOutputs() => new ObservableCollection<IDOutput>();

        // Unload Tab Get Methods
        private ObservableCollection<IMotion> GetUnloadTransferMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetUnloadAlignMotions() => new ObservableCollection<IMotion>();
        private ObservableCollection<IMotion> GetRobotUnloadMotions() => new ObservableCollection<IMotion>();

        private ObservableCollection<ICylinder> GetUnloadTransferCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetUnloadAlignCylinders() => new ObservableCollection<ICylinder>();
        private ObservableCollection<ICylinder> GetRobotUnloadCylinders() => new ObservableCollection<ICylinder>();

        private ObservableCollection<IDInput> GetUnloadTransferInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetUnloadAlignInputs() => new ObservableCollection<IDInput>();
        private ObservableCollection<IDInput> GetRobotUnloadInputs() => new ObservableCollection<IDInput>();

        private ObservableCollection<IDOutput> GetUnloadTransferOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetUnloadAlignOutputs() => new ObservableCollection<IDOutput>();
        private ObservableCollection<IDOutput> GetRobotUnloadOutputs() => new ObservableCollection<IDOutput>();

        #endregion

    }
}
