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
        private double yAxisLeftPlacePosition;
        private double zAxisLeftPlacePosition;
        private double yAxisRightPlacePosition;
        private double zAxisRightPlacePosition;

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Ready Position", Unit = Unit.mm)]
        public double YAxisReadyPosition
        {
            get { return yAxisReadyPosition; }
            set { yAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Ready Position", Unit = Unit.mm)]
        public double ZAxisReadyPosition
		{
			get { return zAxisReadyPosition; }
			set { zAxisReadyPosition = value; }
		}

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Left Place Position", Unit = Unit.mm)]
        public double YAxisLeftPlacePosition
        {
            get { return yAxisLeftPlacePosition; }
            set { yAxisLeftPlacePosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Left Place Position", Unit = Unit.mm)]
        public double ZAxisLeftPlacePosition
        {
            get { return zAxisLeftPlacePosition; }
            set { zAxisLeftPlacePosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Right Place Position", Unit = Unit.mm)]
        public double YAxisRightPlacePosition
        {
            get { return yAxisRightPlacePosition; }
            set { yAxisRightPlacePosition = value; }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Right Place Position", Unit = Unit.mm)]
        public double ZAxisRightPlacePosition
        {
            get { return zAxisRightPlacePosition; }
            set { zAxisRightPlacePosition = value; }
        }
    }
}
