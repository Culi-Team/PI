using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class VinylCleanRecipe : RecipeBase
    {
		private int unWinderTorque;
        private double vinylLengthPerCleaning;

        [SingleRecipeDescription(Description = "UnWinder Torque", Unit = Unit.Percentage)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public int UnWinderTorque
		{
			get { return unWinderTorque; }
			set 
			{
                OnRecipeChanged(unWinderTorque, value);
                unWinderTorque = value; 
			}
		}

        [SingleRecipeDescription(Description = "Vinyl Length Per Cleaning", Unit = Unit.mm)]
        public double VinylLengthPerCleaning
		{
			get { return vinylLengthPerCleaning; }
			set 
			{
                OnRecipeChanged(vinylLengthPerCleaning, value);
                vinylLengthPerCleaning = value; 
			}
		}
	}
}
