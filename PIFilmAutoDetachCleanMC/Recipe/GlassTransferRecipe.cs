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
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisReadyPosition
        {
            get { return yAxisReadyPosition; }
            set 
            {
                OnRecipeChanged(yAxisReadyPosition, value);
                yAxisReadyPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Z Axis")]
        public double ZAxisReadyPosition
		{
			get { return zAxisReadyPosition; }
			set 
            {
                OnRecipeChanged(zAxisReadyPosition, value);
                zAxisReadyPosition = value; 
            }
		}

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Pick Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisPickPosition
        {
            get { return yAxisPickPosition; }
            set 
            {
                OnRecipeChanged(yAxisPickPosition, value);
                yAxisPickPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Pick Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Z Axis")]
        public double ZAxisPickPosition
        {
            get { return zAxisPickPosition; }
            set 
            {
                OnRecipeChanged(zAxisPickPosition, value);
                zAxisPickPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Left Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisLeftPlacePosition
        {
            get { return yAxisLeftPlacePosition; }
            set 
            {
                OnRecipeChanged(yAxisLeftPlacePosition, value);
                yAxisLeftPlacePosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Left Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Z Axis")]
        public double ZAxisLeftPlacePosition
        {
            get { return zAxisLeftPlacePosition; }
            set 
            {
                OnRecipeChanged(zAxisLeftPlacePosition, value);
                zAxisLeftPlacePosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Y Axis Right Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisRightPlacePosition
        {
            get { return yAxisRightPlacePosition; }
            set 
            {
                OnRecipeChanged(yAxisRightPlacePosition, value);
                yAxisRightPlacePosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Glass Transfer Z Axis Right Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Z Axis")]
        public double ZAxisRightPlacePosition
        {
            get { return zAxisRightPlacePosition; }
            set 
            {
                OnRecipeChanged(zAxisRightPlacePosition, value);
                zAxisRightPlacePosition = value; 
            }
        }
    }
}
