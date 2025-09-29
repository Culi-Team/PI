using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class UnloadTransferRecipe : RecipeBase
    {
		private double yAxisReadyPosition;
        private double zAxisReadyPositionn;
        private double yAxisPickPosition;
        private double zAxisPickPosition;
        private double yAxisPlacePosition1;
        private double yAxisPlacePosition2;
        private double yAxisPlacePosition3;
        private double yAxisPlacePosition4;
        private double zAxisPlacePosition;

        [SingleRecipeDescription(Description = "Y Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "YAxis")]
        public double YAxisReadyPosition
		{
			get { return yAxisReadyPosition; }
			set 
            {
                OnRecipeChanged(yAxisReadyPosition, value);
                yAxisReadyPosition = value; 
            }
		}

        [SingleRecipeDescription(Description = "Z Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "ZAxis")]
        public double ZAxisReadyPosition
		{
			get { return zAxisReadyPositionn; }
			set 
            {
                OnRecipeChanged(zAxisReadyPositionn, value);
                zAxisReadyPositionn = value; 
            }
		}

        [SingleRecipeDescription(Description = "Y Axis Pick Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "YAxis")]
        public double YAxisPickPosition
		{
			get { return yAxisPickPosition; }
			set 
            {
                OnRecipeChanged(yAxisPickPosition, value);
                yAxisPickPosition = value; 
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

        [SingleRecipeDescription(Description = "Y Axis Place Position 1", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "YAxis")]
        public double YAxisPlacePosition1
		{
			get { return yAxisPlacePosition1; }
			set 
            {
                OnRecipeChanged(yAxisPlacePosition1, value);
                yAxisPlacePosition1 = value; 
            }
		}

        [SingleRecipeDescription(Description = "Y Axis Place Position 2", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "YAxis")]
        public double YAxisPlacePosition2
        {
            get { return yAxisPlacePosition2; }
            set 
            {
                OnRecipeChanged(yAxisPlacePosition2, value);
                yAxisPlacePosition2 = value; 
            }
        }

        [SingleRecipeDescription(Description = "Y Axis Place Position 3", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "YAxis")]
        public double YAxisPlacePosition3
        {
            get { return yAxisPlacePosition3; }
            set 
            {
                OnRecipeChanged(yAxisPlacePosition3, value);
                yAxisPlacePosition3 = value;
            }
        }

        [SingleRecipeDescription(Description = "Y Axis Place Position 4", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "YAxis")]
        public double YAxisPlacePosition4
        {
            get { return yAxisPlacePosition4; }
            set 
            {
                OnRecipeChanged(yAxisPlacePosition4, value);
                yAxisPlacePosition4 = value; 
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
