using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual
{
    public class ConveyorManualUnitViewModel : ManualUnitViewModel
    {
        public ConveyorManualUnitViewModel(string name) : base(name)
        {
        }

        public ObservableCollection<ISpeedController> Rollers { get; set; }
    }
}
