using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using System.Diagnostics;
using PIFilmAutoDetachCleanMC.Defines;
using System.Windows.Input;
using EQX.Core.Device.SpeedController;
using PIFilmAutoDetachCleanMC.Defines.Devices;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class ManualViewModel : ViewModelBase
    {

        public ManualViewModel(Devices devices)
        {
            Inputs = devices.Inputs;
            Outputs = devices.Outputs;
            MotionsInovance = devices.MotionsInovance;
            MotionsAjin = devices.MotionsAjin;
        }

        private int _speed = 1000;

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; OnPropertyChanged(); }
        }

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
