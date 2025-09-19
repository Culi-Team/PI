using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class TransferInShuttleRecipe : RecipeBase
    {
        private double yAxisReadyPosition;
        private double zAxisReadyPosition;
        private double yAxisPickPosition1;
        private double yAxisPickPosition2;
        private double yAxisPickPosition3;
        private double zAxisPickPosition;
        private double yAxisPlacePosition;
        private double zAxisPlacePosition;

        [SingleRecipeDescription(Description = "Y Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisReadyPosition
        {
            get { return yAxisReadyPosition; }
            set { yAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Z Axis Ready Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Z Axis")]
        public double ZAxisReadyPosition
        {
            get { return zAxisReadyPosition; }
            set { zAxisReadyPosition = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Pick Position 1", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisPickPosition1
        {
            get { return yAxisPickPosition1; }
            set { yAxisPickPosition1 = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Pick Position 2", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisPickPosition2
        {
            get { return yAxisPickPosition2; }
            set { yAxisPickPosition2 = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Pick Position 3", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisPickPosition3
        {
            get { return yAxisPickPosition3; }
            set { yAxisPickPosition3 = value; }
        }

        [SingleRecipeDescription(Description = "Z Axis Pick Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Z Axis")]
        public double ZAxisPickPosition
        {
            get { return zAxisPickPosition; }
            set { zAxisPickPosition = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisPlacePosition
        {
            get { return yAxisPlacePosition; }
            set { yAxisPlacePosition = value; }
        }

        [SingleRecipeDescription(Description = "Z Axis Place Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Z Axis")]
        public double ZAxisPlacePosition
        {
            get { return zAxisPlacePosition; }
            set { zAxisPlacePosition = value; }
        }
    }
}
