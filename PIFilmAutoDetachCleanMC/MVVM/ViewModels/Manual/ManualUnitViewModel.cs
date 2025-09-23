using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual
{
    public class ManualUnitViewModel : ViewModelBase
    {
        public ObservableCollection<ICylinder> Cylinders { get; set; }
        public ObservableCollection<IMotion> Motions { get; set; }
        public ObservableCollection<IDInput> Inputs { get; set; }
        public ObservableCollection<IDOutput> Outputs { get; set; }
        public string Name { get; init; }
        public ImageSource Image { get; set; }

        public ManualUnitViewModel(string name)
        {
            Name = name;
        }
    }
}
