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
                if (cylinderMoveTimeout == value) return;

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
                if (motionOriginTimeout == value) return;

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
                if (motionMoveTimeout == value) return;

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
                if (vacDelay == value) return;

                OnRecipeChanged(vacDelay, value);
                vacDelay = value;
            }
        }

        [SingleRecipeDescription(
            Description = "Disable Left Port",
            Detail = "Check to disable Left Port (Glass Transfer -> Glass Unload)")]
        public bool DisableLeftPort
        {
            get { return disableLeftPort; }
            set
            {
                if (disableLeftPort == value) return;

                OnRecipeChanged(disableLeftPort, value);
                disableLeftPort = value;
            }
        }

        [SingleRecipeDescription(
            Description = "Disable Right Port",
            Detail = "Check to disable Right Port (Glass Transfer -> Glass Unload)")]
        public bool DisableRightPort
        {
            get { return disableRightPort; }
            set
            {
                if (disableRightPort == value) return;

                OnRecipeChanged(DisableRightPort, value);
                disableRightPort = value;
            }
        }

        [SingleRecipeDescription(
            Description = "Skip Vinyl Clean",
            Detail = "Check to skip Vinyl Clean")]
        public bool SkipVinylClean
        {
            get { return skipVinylClean; }
            set
            {
                if (skipVinylClean == value) return;

                OnRecipeChanged(skipVinylClean, value);
                skipVinylClean = value;
            }
        }

        #region Privates
        private bool disableLeftPort;
        private bool disableRightPort;
        private bool skipVinylClean;
        #endregion
    }
}
