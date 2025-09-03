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
			set { cylinderMoveTimeout = value; }
		}

        [SingleRecipeDescription(Description = "Motion Origin Timeout", Unit = Unit.MilliSecond)]
        public int MotionOriginTimeout
		{
			get { return motionOriginTimeout; }
			set { motionOriginTimeout = value; }
		}

        [SingleRecipeDescription(Description = "Motion Move Timeout", Unit = Unit.MilliSecond)]
        public int MotionMoveTimeOut
		{
			get { return motionMoveTimeout; }
			set { motionMoveTimeout = value; }
		}

        [SingleRecipeDescription(Description = "Vacuum Delay", Unit = Unit.MilliSecond)]
        public int VacDelay
		{
			get { return vacDelay; }
			set { vacDelay = value; }
		}


	}
}
