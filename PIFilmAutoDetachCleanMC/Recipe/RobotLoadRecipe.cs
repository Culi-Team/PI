using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class RobotLoadRecipe : RecipeBase
    {
		private int robotSpeed;
        private int robotPlaceSpeed;
        private int model;

        [SingleRecipeDescription(Description = "Model")]
        [SingleRecipeMinMax(Max = 100, Min = 0)]
        public int Model
        {
            get { return model; }
            set { model = value; }
        }

        [SingleRecipeDescription(Description = "Robot Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotSpeed
		{
			get { return robotSpeed; }
			set { robotSpeed = value; }
		}

        [SingleRecipeDescription(Description = "Robot Place Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotPlaceSpeed
		{
			get { return robotPlaceSpeed; }
			set { robotPlaceSpeed = value; }
		}

	}
}
