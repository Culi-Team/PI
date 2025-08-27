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

        public ICommand ConnectMotionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MotionsInovance.MotionControllerInovance.Connect();
                });
            }
        }
    }
}
