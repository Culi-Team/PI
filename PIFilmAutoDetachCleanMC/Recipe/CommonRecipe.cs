using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class CommonRecipe : RecipeBase
    {
		private double cylinderMoveTimeout;
        private double motionOriginTimeout;
        private double motionMoveTimeout;
        private double vacDelay;

        [SingleRecipeDescription(Description = "Cylinder Move Timeout", Unit = Unit.Second)]
        public double CylinderMoveTimeout
		{
			get { return cylinderMoveTimeout; }
			set 
			{
				OnRecipeChanged(cylinderMoveTimeout, value);
                cylinderMoveTimeout = value; 
			}
		}

        [SingleRecipeDescription(Description = "Motion Origin Timeout", Unit = Unit.Second)]
        public double MotionOriginTimeout
		{
			get { return motionOriginTimeout; }
			set 
			{
                OnRecipeChanged(motionOriginTimeout, value);
                motionOriginTimeout = value; 
			}
		}

        [SingleRecipeDescription(Description = "Motion Move Timeout", Unit = Unit.Second)]
        public double MotionMoveTimeOut
		{
			get { return motionMoveTimeout; }
			set 
			{
                OnRecipeChanged(motionMoveTimeout, value);
                motionMoveTimeout = value; 
			}
		}

        [SingleRecipeDescription(Description = "Vacuum Delay", Unit = Unit.Second)]
        public double VacDelay
		{
			get { return vacDelay; }
			set 
			{
                OnRecipeChanged(vacDelay, value);
                vacDelay = value; 
			}
		}


	}
}
