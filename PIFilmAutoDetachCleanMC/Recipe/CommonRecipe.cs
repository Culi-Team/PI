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
		private int cylinderMoveTimeout;
        private int motionOriginTimeout;
        private int motionMoveTimeout;
        private int vacDelay;

        [SingleRecipeDescription(Description = "Cylinder Move Timeout", Unit = Unit.MilliSecond)]
        public int CylinderMoveTimeout
		{
			get { return cylinderMoveTimeout; }
			set 
			{
				OnRecipeChanged(cylinderMoveTimeout, value);
                cylinderMoveTimeout = value; 
			}
		}

        [SingleRecipeDescription(Description = "Motion Origin Timeout", Unit = Unit.MilliSecond)]
        public int MotionOriginTimeout
		{
			get { return motionOriginTimeout; }
			set 
			{
                OnRecipeChanged(motionOriginTimeout, value);
                motionOriginTimeout = value; 
			}
		}

        [SingleRecipeDescription(Description = "Motion Move Timeout", Unit = Unit.MilliSecond)]
        public int MotionMoveTimeOut
		{
			get { return motionMoveTimeout; }
			set 
			{
                OnRecipeChanged(motionMoveTimeout, value);
                motionMoveTimeout = value; 
			}
		}

        [SingleRecipeDescription(Description = "Vacuum Delay", Unit = Unit.MilliSecond)]
        public int VacDelay
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
