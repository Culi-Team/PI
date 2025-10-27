using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class TransferRotationRecipe : RecipeBase
    {
        private double zAxisReadyPosition;
        private double zAxisPickPosition;
        private double zAxisTransferReadyPosition;
        private double zAxisTransferBeforeRotatePosition;
        private double zAxisTransferAfterRotatePosition;
        private double zAxisPlacePosition;

        [SingleRecipeDescription(Description = "Z Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ZAxisReadyPosition
        {
            get { return zAxisReadyPosition; }
            set
            {
                OnRecipeChanged(zAxisReadyPosition, value);
                zAxisReadyPosition = value;
            }
        }

        [SingleRecipeDescription(Description = "Z Axis Pick Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ZAxisPickPosition
        {
            get { return zAxisPickPosition; }
            set
            {
                OnRecipeChanged(zAxisPickPosition, value);
                zAxisPickPosition = value;
            }
        }

        [SingleRecipeDescription(Description = "Z Axis Transfer Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ZAxisTransferReadyPosition
        {
            get { return zAxisTransferReadyPosition; }
            set
            {
                OnRecipeChanged(zAxisTransferReadyPosition, value);
                zAxisTransferReadyPosition = value;
            }
        }

        [SingleRecipeDescription(Description = "Z Axis Transfer Before Rotate Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ZAxisTransferBeforeRotatePosition
        {
            get { return zAxisTransferBeforeRotatePosition; }
            set
            {
                OnRecipeChanged(zAxisTransferBeforeRotatePosition, value);
                zAxisTransferBeforeRotatePosition = value;
            }
        }

        [SingleRecipeDescription(Description = "Z Axis Transfer After Rotate Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ZAxisTransferAfterRotatePosition
        {
            get { return zAxisTransferAfterRotatePosition; }
            set
            {
                OnRecipeChanged(zAxisTransferAfterRotatePosition, value);
                zAxisTransferAfterRotatePosition = value;
            }
        }

        [SingleRecipeDescription(Description = "Z Axis Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ZAxisPlacePosition
        {
            get { return zAxisPlacePosition; }
            set
            {
                OnRecipeChanged(zAxisPlacePosition, value);
                zAxisPlacePosition = value;
            }
        }
    }
}
