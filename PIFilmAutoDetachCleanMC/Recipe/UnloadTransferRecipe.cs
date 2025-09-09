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
        public double YAxisReadyPosition
		{
			get { return yAxisReadyPosition; }
			set { yAxisReadyPosition = value; }
		}

        [SingleRecipeDescription(Description = "Z Axis Ready Position", Unit = Unit.mm)]
        public double ZAxisReadyPosition
		{
			get { return zAxisReadyPositionn; }
			set { zAxisReadyPositionn = value; }
		}

        [SingleRecipeDescription(Description = "Y Axis Pick Position", Unit = Unit.mm)]
        public double YAxisPickPosition
		{
			get { return yAxisPickPosition; }
			set { yAxisPickPosition = value; }
		}

        [SingleRecipeDescription(Description = "Z Axis Pick Position", Unit = Unit.mm)]
        public double ZAxisPickPosition
		{
			get { return zAxisPickPosition; }
			set { zAxisPickPosition = value; }
		}

        [SingleRecipeDescription(Description = "Y Axis Place Position 1", Unit = Unit.mm)]
        public double YAxisPlacePosition1
		{
			get { return yAxisPlacePosition1; }
			set { yAxisPlacePosition1 = value; }
		}

        [SingleRecipeDescription(Description = "Y Axis Place Position 2", Unit = Unit.mm)]
        public double YAxisPlacePosition2
        {
            get { return yAxisPlacePosition2; }
            set { yAxisPlacePosition2 = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Place Position 3", Unit = Unit.mm)]
        public double YAxisPlacePosition3
        {
            get { return yAxisPlacePosition3; }
            set { yAxisPlacePosition3 = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Place Position 4", Unit = Unit.mm)]
        public double YAxisPlacePosition4
        {
            get { return yAxisPlacePosition4; }
            set { yAxisPlacePosition4 = value; }
        }

        [SingleRecipeDescription(Description = "Z Axis Place Position", Unit = Unit.mm)]
        public double ZAxisPlacePosition
		{
			get { return zAxisPlacePosition; }
			set { zAxisPlacePosition = value; }
		}

	}
}
