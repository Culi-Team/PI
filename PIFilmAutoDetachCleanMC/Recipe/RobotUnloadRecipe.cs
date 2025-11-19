using EQX.Core.Recipe;
using EQX.Core.Units;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class RobotUnloadRecipe : RecipeBase
    {
        private int robotHighSpeed;
        private int robotLowSpeed;
        private int robotPlasmaSpeed;
        private int robotPlasmaMinTemperature;
        private int model;

        private double downstreamMinAliveTime;

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

        [SingleRecipeDescription(Description = "Robot Plasma Speed", Unit = Unit.Percentage)]
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

        [SingleRecipeDescription(Description = "Plasma Min Temparature", Unit = Unit.Celsius)]
        [SingleRecipeMinMax(Max = 50, Min = 25)]
        public int RobotPlasmaMinTemparature
        {
            get { return robotPlasmaMinTemperature; }
            set
            {
                if (robotPlasmaMinTemperature == value) return;

                OnRecipeChanged(robotPlasmaMinTemperature, value);
                robotPlasmaMinTemperature = value;
            }
        }

        [SingleRecipeDescription(Description = "Robot Speed Low", Unit = Unit.Percentage)]
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

        [SingleRecipeDescription(Description = "Robot Speed High", Unit = Unit.Percentage)]
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

        [SingleRecipeDescription(Description = "Downstream (Clean Machine) Min Alive Time", Unit = Unit.Second)]
        [SingleRecipeMinMax(Max = 100, Min = 0)]
        public double DownstreamMinAliveTime
        {
            get { return downstreamMinAliveTime; }
            set
            {
                downstreamMinAliveTime = value;
                OnRecipeChanged(downstreamMinAliveTime, value);
            }
        }
    }
}
