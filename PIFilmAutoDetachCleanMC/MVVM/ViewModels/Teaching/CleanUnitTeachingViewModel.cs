using EQX.Core.Device.Regulator;
using EQX.Core.Device.SyringePump;
using EQX.Core.TorqueController;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching
{
    public class CleanUnitTeachingViewModel : UnitTeachingViewModel
    {
        public CleanUnitTeachingViewModel(string name,RecipeSelector recipeSelector) : base(name,recipeSelector)
        {
            Name = name;
        }

        public ITorqueController Winder { get; set; }
        public ITorqueController UnWinder { get; set; }
        public ISyringePump SyringePump { get; set; }
        public IRegulator Regulator { get; set; }
    }
}
