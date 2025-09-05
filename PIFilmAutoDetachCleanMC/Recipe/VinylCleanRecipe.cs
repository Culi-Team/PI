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
		private int winderTorque;
        private double vinylLengthPerCleaning;

        [SingleRecipeDescription(Description = "Winder Torque", Unit = Unit.Percentage)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public int WinderTorque
		{
			get { return winderTorque; }
			set { winderTorque = value; }
		}

        [SingleRecipeDescription(Description = "Vinyl Length Per Cleaning", Unit = Unit.mm)]
        public double VinylLengthPerCleaning
		{
			get { return vinylLengthPerCleaning; }
			set { vinylLengthPerCleaning = value; }
		}
	}
}
