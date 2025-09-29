using EQX.Core.Recipe;
using EQX.Core.Units;
using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class DetachRecipe : RecipeBase
    {
        private double detachZAxisReadyPosition;
        private double detachZAxisDetachReadyPosition;
        private double detachZAxisDetach1Position;
        private double detachZAxisDetach2Position;
        private double shuttleTransferZAxisReadyPosition;
        private double shuttleTransferZAxisDetachReadyPosition;
        private double shuttleTransferZAxisDetach1Position;
        private double shuttleTransferZAxisDetach2Position;
        private double shuttleTransferXAxisDetachPosition;
        private double shuttleTransferXAxisDetachCheckPosition;
        private double shuttleTransferXAxisUnloadPosition;
        private double shuttleTransferZAxisUnloadPosition;

        [SingleRecipeDescription(Description = "Detach Z Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double DetachZAxisReadyPosition
        {
            get { return detachZAxisReadyPosition; }
            set 
            {
                OnRecipeChanged(detachZAxisReadyPosition, value);
                detachZAxisReadyPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double DetachZAxisDetachReadyPosition
        {
            get { return detachZAxisDetachReadyPosition; }
            set 
            {
                OnRecipeChanged(detachZAxisDetachReadyPosition, value);
                detachZAxisDetachReadyPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach 1 Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double DetachZAxisDetach1Position
        {
            get { return detachZAxisDetach1Position; }
            set 
            {
                OnRecipeChanged(detachZAxisDetach1Position, value);
                detachZAxisDetach1Position = value; 
            }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach 2 Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double DetachZAxisDetach2Position
        {
            get { return detachZAxisDetach2Position; }
            set 
            {
                OnRecipeChanged(detachZAxisDetach2Position, value);
                detachZAxisDetach2Position = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ShuttleTransferZAxisReadyPosition
        {
            get { return shuttleTransferZAxisReadyPosition; }
            set 
            {
                OnRecipeChanged(shuttleTransferZAxisReadyPosition, value);
                shuttleTransferZAxisReadyPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ShuttleTransferZAxisDetachReadyPosition
        {
            get { return shuttleTransferZAxisDetachReadyPosition; }
            set 
            {
                OnRecipeChanged(shuttleTransferZAxisDetachReadyPosition, value);
                shuttleTransferZAxisDetachReadyPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach 1 Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ShuttleTransferZAxisDetach1Position
        {
            get { return shuttleTransferZAxisDetach1Position; }
            set 
            {
                OnRecipeChanged(shuttleTransferZAxisDetach1Position, value);
                shuttleTransferZAxisDetach1Position = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach 2 Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ShuttleTransferZAxisDetach2Position
        {
            get { return shuttleTransferZAxisDetach2Position; }
            set 
            {
                OnRecipeChanged(shuttleTransferZAxisDetach2Position, value);
                shuttleTransferZAxisDetach2Position = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Unload Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ShuttleTransferZAxisUnloadPosition
        {
            get { return shuttleTransferZAxisUnloadPosition; }
            set 
            {
                OnRecipeChanged(shuttleTransferZAxisUnloadPosition, value);
                shuttleTransferZAxisUnloadPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Detach Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "XAxis")]
        public double ShuttleTransferXAxisDetachPosition
        {
            get { return shuttleTransferXAxisDetachPosition; }
            set 
            {
                OnRecipeChanged(shuttleTransferXAxisDetachPosition, value);
                shuttleTransferXAxisDetachPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Detach Check Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "XAxis")]
        public double ShuttleTransferXAxisDetachCheckPosition
        {
            get { return shuttleTransferXAxisDetachCheckPosition; }
            set 
            {
                OnRecipeChanged(shuttleTransferXAxisDetachCheckPosition, value);
                shuttleTransferXAxisDetachCheckPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Unload Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "XAxis")]
        public double ShuttleTransferXAxisUnloadPosition
        {
            get { return shuttleTransferXAxisUnloadPosition; }
            set 
            {
                OnRecipeChanged(shuttleTransferXAxisUnloadPosition, value);
                shuttleTransferXAxisUnloadPosition = value; 
            }
        }

    }
}
