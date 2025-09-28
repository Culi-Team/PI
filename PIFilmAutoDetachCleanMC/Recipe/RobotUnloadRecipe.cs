using EQX.Core.Recipe;
using EQX.Core.Units;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class RobotUnloadRecipe : RecipeBase
    {
        private int robotHighSpeed;
        private int robotLowSpeed;
        private int robotPlasmaSpeed;

        [SingleRecipeDescription(Description = "Robot Plasma Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotPlasmaSpeed
        {
            get { return robotPlasmaSpeed; }
            set
            {
                OnRecipeChanged(robotPlasmaSpeed, value);
                robotPlasmaSpeed = value;
            }
        }

        [SingleRecipeDescription(Description = "Robot Speed Low", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotSpeedLow
        {
            get { return robotLowSpeed; }
            set
            {
                robotLowSpeed = value;
                OnRecipeChanged(robotLowSpeed, value);
            }
        }

        [SingleRecipeDescription(Description = "Robot Speed High", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int RobotSpeedHigh
        {
            get { return robotHighSpeed; }
            set
            {
                robotHighSpeed = value;
                OnRecipeChanged(robotHighSpeed, value);
            }
        }



    }
}
