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

        public ObservableCollection<SD201SSpeedController> Rollers { get; set; }

        public ICommand ConveyorRunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    foreach (var roller in Rollers)
                    {
                        if(roller.IsConnected == false)
                        {
                            MessageBoxEx.ShowDialog("Conveyor is not connected.");
                        }
                        roller.Start();
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
    }
}
