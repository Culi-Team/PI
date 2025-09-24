using EQX.Core.Recipe;
using EQX.Core.Units;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class RobotLoadRecipe : RecipeBase
    {
        private int robotSpeedHigh;
        private int robotSpeedLow;
        private int model;

        [SingleRecipeDescription(Description = "Model")]
        [SingleRecipeMinMax(Max = 100, Min = 0)]
        public int Model
        {
            get { return model; }
            set 
            {
                model = value;
                OnRecipeChanged(model, value);
            }
        }

        [SingleRecipeDescription(Description = "Robot Speed Low", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotSpeedLow
        {
            get { return robotSpeedLow; }
            set 
            {
                robotSpeedLow = value;
                OnRecipeChanged(robotSpeedLow, value);
            }
        }

        [SingleRecipeDescription(Description = "Robot Speed High", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotSpeedHigh
        {
            get { return robotSpeedHigh; }
            set 
            {
                robotSpeedHigh = value;
                OnRecipeChanged(robotSpeedHigh, value);
            }
        }

    }
}
