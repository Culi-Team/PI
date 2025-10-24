using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Helpers;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching
{
    public class CSTUnloadTeachingViewModel : UnitTeachingViewModel
    {
        public CSTUnloadTeachingViewModel(string name, RecipeSelector recipeSelector) : base(name, recipeSelector)
        {
        }

        public ICommand GetCurrentDistanceCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (Recipe is CSTLoadUnloadRecipe cstLoadUnloadRecipe == false) return;
                    Devices devices = App.AppHost!.Services.GetRequiredService<Devices>();

                    cstLoadUnloadRecipe.FirstFixtureDistance = AnalogConverter.Convert(devices.AnalogInputs.Laser.Volt, 0.0, 8191.0, 0, 800.0);
                });
            }
        }
    }
}
