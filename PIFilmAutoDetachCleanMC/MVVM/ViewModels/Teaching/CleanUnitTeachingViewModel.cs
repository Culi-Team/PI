using CommunityToolkit.Mvvm.Input;
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
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching
{
    public class CleanUnitTeachingViewModel : UnitTeachingViewModel
    {
        public CleanUnitTeachingViewModel(string name, RecipeSelector recipeSelector) : base(name, recipeSelector)
        {
            Name = name;
        }

        public ITorqueController Winder { get; set; }
        public ITorqueController UnWinder { get; set; }
        public ISyringePump SyringePump { get; set; }
        public IRegulator Regulator { get; set; }

        public ICommand SetPressureCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Regulator.SetPressure(((CleanRecipe)Recipe).CylinderPushPressure);
                });
            }
        }

        public ICommand WinderSetTorqueCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Winder.SetTorque(((CleanRecipe)Recipe).WinderTorque);
                });
            }
        }

        public ICommand UnWinderSetTorqueCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UnWinder.SetTorque(((CleanRecipe)Recipe).UnwinderTorque);
                });
            }
        }

        public ICommand WinderRunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Winder.Run();
                });
            }
        }

        public ICommand WinderStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Winder.Stop();
                });
            }
        }

        public ICommand UnWinderRunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UnWinder.Run();
                });
            }
        }

        public ICommand UnWinderStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UnWinder.Stop();
                });
            }
        }
    }
}
