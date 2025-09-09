using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class RobotUnloadRecipe : RecipeBase
    {
		private int robotSpeed;
        private int robotPlasmaSpeed;

        [SingleRecipeDescription(Description = "Robot Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotSpeed
		{
			get { return robotSpeed; }
			set { robotSpeed = value; }
		}

        [SingleRecipeDescription(Description = "Robot Plasma Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotPlasmaSpeed
		{
			get { return robotPlasmaSpeed; }
			set { robotPlasmaSpeed = value; }
		}

	}
}
