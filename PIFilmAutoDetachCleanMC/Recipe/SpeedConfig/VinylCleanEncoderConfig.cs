using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class VinylCleanEncoderConfig : RecipeBase
    {
        private double velocity;
        private double acceleration;
        private double deceleration;

        [SingleRecipeDescription(Description = "Velocity", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.1)]
        public double Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        [SingleRecipeDescription(Description = "Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.1)]
        public double Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        [SingleRecipeDescription(Description = "Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.1)]
        public double Deceleration
        {
            get { return deceleration; }
            set { deceleration = value; }
        }
    }
}
