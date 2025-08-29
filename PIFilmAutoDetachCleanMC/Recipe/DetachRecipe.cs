using EQX.Core.Recipe;
using EQX.Core.Units;
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
        private double detachZAxisDetach1Position;
        private double detachZAxisDetach2Position;
        private double shuttleTransferZAxisReadyPosition;
        private double shuttleTransferZAxisDetach1Position;
        private double shuttleTransferZAxisDetach2Postion;
        private double shuttleTransferXAxisDetachPosition;
        private double shuttleTransferXAxisDetachCheckPosition;
        private double shuttleTransferXAxisUnloadPositon;
        private double shuttleTransferZAxisUnloadPosition;

        [SingleRecipeDescription(Description = "Detach Z Axis Ready Position", Unit = Unit.mm)]
        public double DetachZAxisReadyPosition
        {
            get { return detachZAxisReadyPosition; }
            set { detachZAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach 1 Position", Unit = Unit.mm)]
        public double DetachZAxisDetach1Position
        {
            get { return detachZAxisDetach1Position; }
            set { detachZAxisDetach1Position = value; }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach 2 Position", Unit = Unit.mm)]
        public double DetachZAxisDetach2Positon
        {
            get { return detachZAxisDetach2Position; }
            set { detachZAxisDetach2Position = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Ready Position", Unit = Unit.mm)]
        public double ShuttleTransferZAxisReadyPositon
        {
            get { return shuttleTransferZAxisReadyPosition; }
            set { shuttleTransferZAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach 1 Position", Unit = Unit.mm)]
        public double ShuttleTransferZAxisDetach1Position
        {
            get { return shuttleTransferZAxisDetach1Position; }
            set { shuttleTransferZAxisDetach1Position = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach 2 Position", Unit = Unit.mm)]
        public double ShuttleTransferZAxisDetach2Positon
        {
            get { return shuttleTransferZAxisDetach2Postion; }
            set { shuttleTransferZAxisDetach2Postion = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Unload Position", Unit = Unit.mm)]
        public double ShuttleTransferZAxisUnloadPosition
        {
            get { return shuttleTransferZAxisUnloadPosition; }
            set { shuttleTransferZAxisUnloadPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Detach Position", Unit = Unit.mm)]
        public double ShuttleTransferXAxisDetachPosition
        {
            get { return shuttleTransferXAxisDetachPosition; }
            set { shuttleTransferXAxisDetachPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Detach Check Position", Unit = Unit.mm)]
        public double ShuttleTransferXAxisDetachCheckPositon
        {
            get { return shuttleTransferXAxisDetachCheckPosition; }
            set { shuttleTransferXAxisDetachCheckPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Unload Position", Unit = Unit.mm)]
        public double ShuttleTransferXAxisUnloadPositon
        {
            get { return shuttleTransferXAxisUnloadPositon; }
            set { shuttleTransferXAxisUnloadPositon = value; }
        }

    }
}
