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
        private double zAxisTransferPositionUp;
        private double zAxisTransferPositionDown;
        private double zAxisPlacePosition;
        private bool isUseRotation;


        [SingleRecipeDescription(Description = "Z Axis Ready Position", Unit = Unit.mm)]
        public double ZAxisReadyPosition
		{
			get { return zAxisReadyPosition; }
			set { zAxisReadyPosition = value; }
		}

        [SingleRecipeDescription(Description = "Z Axis Pick Position", Unit = Unit.mm)]
        public double ZAxisPickPosition
		{
			get { return zAxisPickPosition; }
			set { zAxisPickPosition = value; }
		}

        [SingleRecipeDescription(Description = "Z Axis Transfer Position Up", Unit = Unit.mm)]
        public double ZAxisTransferPositionUp
		{
			get { return zAxisTransferPositionUp; }
			set { zAxisTransferPositionUp = value; }
		}

        [SingleRecipeDescription(Description = "Z Axis Transfer Position Down", Unit = Unit.mm)]
        public double ZAxisTransferPositionDown
		{
			get { return zAxisTransferPositionDown; }
			set { zAxisTransferPositionDown = value; }
		}

        [SingleRecipeDescription(Description = "Z Axis Place Position", Unit = Unit.mm)]
        public double ZAxisPlacePosition
		{
			get { return zAxisPlacePosition; }
			set { zAxisPlacePosition = value; }
		}

        [SingleRecipeDescription(Description = "Rotation", Unit = Unit.ETC)]
        public bool IsUseRotation
		{
			get { return isUseRotation; }
			set { isUseRotation = value; }
		}
	}
}
