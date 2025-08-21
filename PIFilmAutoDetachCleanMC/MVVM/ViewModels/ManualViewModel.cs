using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using System.Diagnostics;
using PIFilmAutoDetachCleanMC.Defines;
using System.Windows.Input;
using EQX.Core.Device.RollerController;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class ManualViewModel : ViewModelBase
    {
        private readonly IRollerController _rollerController;

        public ManualViewModel(Devices devices,IRollerController rollerController)
        {
            Inputs = devices.Inputs;
            Outputs = devices.Outputs;
            Motions = devices.Motions;
            _rollerController = rollerController;
        }

        private int _speed = 1000;

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; OnPropertyChanged(); }
        }

        public Inputs Inputs { get; }
        public Outputs Outputs { get; }
        public Motions Motions { get; }

        public ICommand ConnectMotionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Motions.All.ForEach(m => m.Connect());
                });
            }
        }

        public ICommand SetSpeedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _rollerController.SetSpeed(Speed);
                });
            }
        }

        public ICommand SetAccCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _rollerController.SetAcceleration(Speed);
                });
            }
        }

        public ICommand SetDecCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _rollerController.SetDeceleration(Speed);
                });
            }
        }

        public ICommand RunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _rollerController.Start();
                });
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _rollerController.Stop();
                });
            }
        }
    }
}
