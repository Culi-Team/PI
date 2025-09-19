using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class GlassTransferRecipe : RecipeBase
    {
        private double yAxisReadyPosition;
        private double zAxisReadyPosition;
        private double yAxisPickPosition;
        private double zAxisPickPosition;
        private double yAxisLeftPlacePosition;
        private double zAxisLeftPlacePosition;
        private double yAxisRightPlacePosition;
        private double zAxisRightPlacePosition;

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferYAxis")]
        public double YAxisReadyPosition
        {
            get { return yAxisReadyPosition; }
            set { yAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferZAxis")]
        public double ZAxisReadyPosition
		{
			get { return zAxisReadyPosition; }
			set { zAxisReadyPosition = value; }
		}

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Pick Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferYAxis")]
        public double YAxisPickPosition
        {
            get { return yAxisPickPosition; }
            set { yAxisPickPosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Pick Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferZAxis")]
        public double ZAxisPickPosition
        {
            get { return zAxisPickPosition; }
            set { zAxisPickPosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Left Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferYAxis")]
        public double YAxisLeftPlacePosition
        {
            get { return yAxisLeftPlacePosition; }
            set { yAxisLeftPlacePosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Left Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferZAxis")]
        public double ZAxisLeftPlacePosition
        {
            get { return zAxisLeftPlacePosition; }
            set { zAxisLeftPlacePosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Right Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferYAxis")]
        public double YAxisRightPlacePosition
        {
            get { return yAxisRightPlacePosition; }
            set { yAxisRightPlacePosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Right Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "GlassTransferZAxis")]
        public double ZAxisRightPlacePosition
        {
            get { return zAxisRightPlacePosition; }
            set { zAxisRightPlacePosition = value; }
        }
    }
}
