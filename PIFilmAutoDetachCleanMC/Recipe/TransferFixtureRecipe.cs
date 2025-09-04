using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class TransferFixtureRecipe : RecipeBase
    {
		private double transferFixtureYAxisLoadPosition;
        private double transferFixtureYAxisUnloadPosition;

        [SingleRecipeDescription(Description = "Transfer Fixture Y Axis Load Position", Unit = Unit.mm)]
        public double TransferFixtureYAxisLoadPosition
		{
			get { return transferFixtureYAxisLoadPosition; }
			set { transferFixtureYAxisLoadPosition = value; }
		}

        [SingleRecipeDescription(Description = "Transfer Fixture Y Axis Unload Position", Unit = Unit.mm)]
        public double TransferFixtureYAxisUnloadPosition
		{
			get { return transferFixtureYAxisUnloadPosition; }
			set { transferFixtureYAxisUnloadPosition = value; }
		}

	}
}
