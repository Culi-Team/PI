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
                          RobotLoadRecipe robotLoadRecipe,
                          VinylCleanRecipe vinylCleanRecipe,
                          TransferFixtureRecipe transferFixtureRecipe,
                          DetachRecipe detachRecipe,
                          GlassTransferRecipe glassTransferRecipe,
                          [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
                          [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetCleanRightRecipe,
                          [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
                          [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe)
        {
            CommonRecipe = commonRecipe;
            CstLoadUnloadRecipe = cstLoadUnloadRecipe;
            RobotLoadRecipe = robotLoadRecipe;
            VinylCleanRecipe = vinylCleanRecipe;
            TransferFixtureRecipe = transferFixtureRecipe;
            DetachRecipe = detachRecipe;
            GlassTransferRecipe = glassTransferRecipe;
            WetCleanLeftRecipe = wetCleanLeftRecipe;
            WetCleanRightRecipe = wetCleanRightRecipe;
            AfCleanLeftRecipe = afCleanLeftRecipe;
            AfCleanRightRecipe = afCleanRightRecipe;

            CommonRecipe.Name = "Common";
            CstLoadUnloadRecipe.Name = "CST Load/Unload";
            RobotLoadRecipe.Name = "Robot Load";
            VinylCleanRecipe.Name = "Vinyl Clean";
            TransferFixtureRecipe.Name = "Transfer Fixture";
            DetachRecipe.Name = "Detach";
            GlassTransferRecipe.Name = "Glass Transfer";
            WetCleanLeftRecipe.Name = "Wet Clean Left";
            WetCleanRightRecipe.Name = "Wet Clean Right";
            AfCleanLeftRecipe.Name = "AF Clean Left";
            AfCleanRightRecipe.Name = "AF Clean Right";
        }

        public CommonRecipe CommonRecipe { get; }
        public CSTLoadUnloadRecipe CstLoadUnloadRecipe { get; }
        public RobotLoadRecipe RobotLoadRecipe { get; }
        public VinylCleanRecipe VinylCleanRecipe { get; }
        public TransferFixtureRecipe TransferFixtureRecipe { get; }
        public DetachRecipe DetachRecipe { get; }
        public GlassTransferRecipe GlassTransferRecipe { get; }
        public CleanRecipe WetCleanLeftRecipe { get; }
        public CleanRecipe WetCleanRightRecipe { get; }
        public CleanRecipe AfCleanLeftRecipe { get; }
        public CleanRecipe AfCleanRightRecipe { get; }
    }
}
