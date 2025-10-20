using CommunityToolkit.Mvvm.Input;
using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Device.SpeedController;
using EQX.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual
{
    public class ConveyorManualUnitViewModel : ManualUnitViewModel
    {
        public ConveyorManualUnitViewModel(string name) : base(name)
        {
        }

        public ObservableCollection<BD201SRollerController> Rollers { get; set; }

        public ICommand ConveyorRunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    foreach (var roller in Rollers)
                    {
                        roller.SetSpeed(ConveyorSpeed);
                        roller.SetAcceleration(ConveyorAcc);
                        roller.SetDeceleration(ConveyorDec);
                        if (roller.IsConnected == false)
                        {
                            MessageBoxEx.ShowDialog("Conveyor is not connected.");
                        }
                        roller.Run();
                    }
                });
            }
        }

        public ICommand ConveyorStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    foreach (var roller in Rollers)
                    {
                        if (roller.IsConnected == false)
                        {
                            MessageBoxEx.ShowDialog("Conveyor is not connected.");
                        }
                        roller.Stop();
                    }
                });
            }
        }

        private int conveyorSpeed = 200;

        public int ConveyorSpeed
        {
            get { return conveyorSpeed; }
            set { conveyorSpeed = value; OnPropertyChanged(); }
        }

        private int conveyorAcc = 500;

        public int ConveyorAcc
        {
            get { return conveyorAcc; }
            set { conveyorAcc = value; OnPropertyChanged(); }
        }

        private int conveyorDec = 500;

        public int ConveyorDec
        {
            get { return conveyorDec; }
            set { conveyorDec = value; OnPropertyChanged(); }
        }

    }
}
