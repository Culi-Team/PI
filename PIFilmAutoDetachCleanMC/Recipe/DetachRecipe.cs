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
        [SingleRecipePosition(Motion = "DetachGlassZAxis")]
        public double DetachZAxisReadyPosition
        {
            get { return detachZAxisReadyPosition; }
            set { detachZAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach Ready Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "DetachGlassZAxis")]
        public double DetachZAxisDetachReadyPosition
        {
            get { return detachZAxisDetachReadyPosition; }
            set { detachZAxisDetachReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach 1 Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "DetachGlassZAxis")]
        public double DetachZAxisDetach1Position
        {
            get { return detachZAxisDetach1Position; }
            set { detachZAxisDetach1Position = value; }
        }

        [SingleRecipeDescription(Description = "Detach Z Axis Detach 2 Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "DetachGlassZAxis")]
        public double DetachZAxisDetach2Position
        {
            get { return detachZAxisDetach2Position; }
            set { detachZAxisDetach2Position = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Ready Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferZAxis")]
        public double ShuttleTransferZAxisReadyPosition
        {
            get { return shuttleTransferZAxisReadyPosition; }
            set { shuttleTransferZAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach Ready Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferZAxis")]
        public double ShuttleTransferZAxisDetachReadyPosition
        {
            get { return shuttleTransferZAxisDetachReadyPosition; }
            set { shuttleTransferZAxisDetachReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach 1 Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferZAxis")]
        public double ShuttleTransferZAxisDetach1Position
        {
            get { return shuttleTransferZAxisDetach1Position; }
            set { shuttleTransferZAxisDetach1Position = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Detach 2 Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferZAxis")]
        public double ShuttleTransferZAxisDetach2Position
        {
            get { return shuttleTransferZAxisDetach2Position; }
            set { shuttleTransferZAxisDetach2Position = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr Z Axis Unload Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferZAxis")]
        public double ShuttleTransferZAxisUnloadPosition
        {
            get { return shuttleTransferZAxisUnloadPosition; }
            set { shuttleTransferZAxisUnloadPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Detach Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferXAxis")]
        public double ShuttleTransferXAxisDetachPosition
        {
            get { return shuttleTransferXAxisDetachPosition; }
            set { shuttleTransferXAxisDetachPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Detach Check Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferXAxis")]
        public double ShuttleTransferXAxisDetachCheckPosition
        {
            get { return shuttleTransferXAxisDetachCheckPosition; }
            set { shuttleTransferXAxisDetachCheckPosition = value; }
        }

        [SingleRecipeDescription(Description = "Sht Tr X Axis Unload Position", Unit = Unit.mm)]
        [SingleRecipePosition(Motion = "ShuttleTransferXAxis")]
        public double ShuttleTransferXAxisUnloadPosition
        {
            get { return shuttleTransferXAxisUnloadPosition; }
            set { shuttleTransferXAxisUnloadPosition = value; }
        }

    }
}
