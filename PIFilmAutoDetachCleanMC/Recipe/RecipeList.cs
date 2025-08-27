using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class RecipeList
    {
        public RecipeList(CSTLoadUnloadRecipe cstLoadUnloadRecipe,
                          [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
                          [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetClenaRightRecipe,
                          [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
                          [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe)
        {
            CstLoadUnloadRecipe = cstLoadUnloadRecipe;
            WetCleanLeftRecipe = wetCleanLeftRecipe;
            WetClenaRightRecipe = wetClenaRightRecipe;
            AfCleanLeftRecipe = afCleanLeftRecipe;
            AfCleanRightRecipe = afCleanRightRecipe;

            CstLoadUnloadRecipe.Name = "CST Load/Unload Recipe";
            WetCleanLeftRecipe.Name = "Wet Clean Left Recipe";
            WetClenaRightRecipe.Name = "Wet Clean Right Recipe";
            AfCleanLeftRecipe.Name = "AF Clean Left Recipe";
            AfCleanRightRecipe.Name = "AF Clean Right Recipe";
        }

        public CSTLoadUnloadRecipe CstLoadUnloadRecipe { get; }
        public CleanRecipe WetCleanLeftRecipe { get; }
        public CleanRecipe WetClenaRightRecipe { get; }
        public CleanRecipe AfCleanLeftRecipe { get; }
        public CleanRecipe AfCleanRightRecipe { get; }
    }
}
