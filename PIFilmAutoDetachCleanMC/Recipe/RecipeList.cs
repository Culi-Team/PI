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
        public RecipeList(CommonRecipe commonRecipe,
                          CSTLoadUnloadRecipe cstLoadUnloadRecipe,
                          VinylCleanRecipe vinylCleanRecipe,
                          TransferFixtureRecipe transferFixtureRecipe,
                          DetachRecipe detachRecipe,
                          [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
                          [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetClenaRightRecipe,
                          [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
                          [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe)
        {
            CommonRecipe = commonRecipe;
            CstLoadUnloadRecipe = cstLoadUnloadRecipe;
            VinylCleanRecipe = vinylCleanRecipe;
            TransferFixtureRecipe = transferFixtureRecipe;
            DetachRecipe = detachRecipe;
            WetCleanLeftRecipe = wetCleanLeftRecipe;
            WetClenaRightRecipe = wetClenaRightRecipe;
            AfCleanLeftRecipe = afCleanLeftRecipe;
            AfCleanRightRecipe = afCleanRightRecipe;

            CommonRecipe.Name = "Common";
            CstLoadUnloadRecipe.Name = "CST Load/Unload";
            VinylCleanRecipe.Name = "Vinyl Clean";
            TransferFixtureRecipe.Name = "Transfer Fixture";
            DetachRecipe.Name = "Detach";
            WetCleanLeftRecipe.Name = "Wet Clean Left";
            WetClenaRightRecipe.Name = "Wet Clean Right";
            AfCleanLeftRecipe.Name = "AF Clean Left";
            AfCleanRightRecipe.Name = "AF Clean Right";
        }

        public CommonRecipe CommonRecipe { get; }
        public CSTLoadUnloadRecipe CstLoadUnloadRecipe { get; }
        public VinylCleanRecipe VinylCleanRecipe { get; }
        public TransferFixtureRecipe TransferFixtureRecipe { get; }
        public DetachRecipe DetachRecipe { get; }
        public CleanRecipe WetCleanLeftRecipe { get; }
        public CleanRecipe WetClenaRightRecipe { get; }
        public CleanRecipe AfCleanLeftRecipe { get; }
        public CleanRecipe AfCleanRightRecipe { get; }
    }
}
