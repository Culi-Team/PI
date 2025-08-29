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
                          DetachRecipe detachRecipe,
                          [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
                          [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetClenaRightRecipe,
                          [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
                          [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe)
        {
            CstLoadUnloadRecipe = cstLoadUnloadRecipe;
            DetachRecipe = detachRecipe;
            WetCleanLeftRecipe = wetCleanLeftRecipe;
            WetClenaRightRecipe = wetClenaRightRecipe;
            AfCleanLeftRecipe = afCleanLeftRecipe;
            AfCleanRightRecipe = afCleanRightRecipe;

            CstLoadUnloadRecipe.Name = "CST Load/Unload";
            DetachRecipe.Name = "Detach";
            WetCleanLeftRecipe.Name = "Wet Clean Left";
            WetClenaRightRecipe.Name = "Wet Clean Right";
            AfCleanLeftRecipe.Name = "AF Clean Left";
            AfCleanRightRecipe.Name = "AF Clean Right";
        }

        public CSTLoadUnloadRecipe CstLoadUnloadRecipe { get; }
        public DetachRecipe DetachRecipe { get; }
        public CleanRecipe WetCleanLeftRecipe { get; }
        public CleanRecipe WetClenaRightRecipe { get; }
        public CleanRecipe AfCleanLeftRecipe { get; }
        public CleanRecipe AfCleanRightRecipe { get; }
    }
}
