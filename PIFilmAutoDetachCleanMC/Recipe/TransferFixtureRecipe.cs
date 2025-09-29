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
		[SinglePositionTeaching(Motion = "YAxis")]
        public double TransferFixtureYAxisLoadPosition
		{
			get { return transferFixtureYAxisLoadPosition; }
			set 
			{
                OnRecipeChanged(transferFixtureYAxisLoadPosition, value);
                transferFixtureYAxisLoadPosition = value; 
			}
		}

        [SingleRecipeDescription(Description = "Transfer Fixture Y Axis Unload Position", Unit = Unit.mm)]
		[SinglePositionTeaching(Motion = "YAxis")]
        public double TransferFixtureYAxisUnloadPosition
		{
			get { return transferFixtureYAxisUnloadPosition; }
			set 
			{
                OnRecipeChanged(transferFixtureYAxisUnloadPosition, value);
                transferFixtureYAxisUnloadPosition = value; 
			}
		}

	}
}
