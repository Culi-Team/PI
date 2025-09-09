using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Device.SpeedController;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Diagnostics;
using System.Timers;
using System.Windows.Input;

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

    }
}
