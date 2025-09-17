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
                          MontionSpeedConfig montionSpeedConfig,
                          CSTLoadUnloadRecipe cstLoadUnloadRecipe,
                          RobotLoadRecipe robotLoadRecipe,
                          VinylCleanRecipe vinylCleanRecipe,
                          TransferFixtureRecipe transferFixtureRecipe,
                          DetachRecipe detachRecipe,
                          GlassTransferRecipe glassTransferRecipe,
                          [FromKeyedServices("TransferInShuttleLeftRecipe")] TransferInShuttleRecipe transferInShuttleLeftRecipe,
                          [FromKeyedServices("TransferInShuttleRightRecipe")] TransferInShuttleRecipe transferInShuttleRightRecipe,
                          [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
                          [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetCleanRightRecipe,
                          [FromKeyedServices("TransferRotationLeftRecipe")] TransferRotationRecipe transferRotationLeftRecipe,
                          [FromKeyedServices("TransferRotationRightRecipe")] TransferRotationRecipe transferRotationRightRecipe,
                          [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
                          [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe,
                          [FromKeyedServices("UnloadTransferLeftRecipe")] UnloadTransferRecipe unloadTransferLeftRecipe,
                          [FromKeyedServices("UnloadTransferRightRecipe")] UnloadTransferRecipe unloadTransferRightRecipe,
                          RobotUnloadRecipe robotUnloadRecipe)
        {
            CommonRecipe = commonRecipe;
            MontionSpeedConfig = montionSpeedConfig;
            CstLoadUnloadRecipe = cstLoadUnloadRecipe;
            RobotLoadRecipe = robotLoadRecipe;
            VinylCleanRecipe = vinylCleanRecipe;
            TransferFixtureRecipe = transferFixtureRecipe;
            DetachRecipe = detachRecipe;
            GlassTransferRecipe = glassTransferRecipe;
            TransferInShuttleLeftRecipe = transferInShuttleLeftRecipe;
            TransferInShuttleRightRecipe = transferInShuttleRightRecipe;
            WetCleanLeftRecipe = wetCleanLeftRecipe;
            WetCleanRightRecipe = wetCleanRightRecipe;
            TransferRotationLeftRecipe = transferRotationLeftRecipe;
            TransferRotationRightRecipe = transferRotationRightRecipe;
            AfCleanLeftRecipe = afCleanLeftRecipe;
            AfCleanRightRecipe = afCleanRightRecipe;
            UnloadTransferLeftRecipe = unloadTransferLeftRecipe;
            UnloadTransferRightRecipe = unloadTransferRightRecipe;
            RobotUnloadRecipe = robotUnloadRecipe;
            CommonRecipe.Name = "Common";
            CstLoadUnloadRecipe.Name = "CST Load/Unload";
            RobotLoadRecipe.Name = "Robot Load";
            VinylCleanRecipe.Name = "Vinyl Clean";
            TransferFixtureRecipe.Name = "Transfer Fixture";
            DetachRecipe.Name = "Detach";
            GlassTransferRecipe.Name = "Glass Transfer";
            TransferInShuttleLeftRecipe.Name = "Transfer In Shuttle Left";
            TransferInShuttleRightRecipe.Name = "Transfer In Shuttle Right";
            WetCleanLeftRecipe.Name = "Wet Clean Left";
            WetCleanRightRecipe.Name = "Wet Clean Right";
            TransferRotationLeftRecipe.Name = "Transfer Rotation Left";
            TransferRotationRightRecipe.Name = "Transfer Rotation Right";
            AfCleanLeftRecipe.Name = "AF Clean Left";
            AfCleanRightRecipe.Name = "AF Clean Right";
            UnloadTransferLeftRecipe.Name = "Unload Transfer Left";
            UnloadTransferRightRecipe.Name = "Unload Transfer Right";
            RobotUnloadRecipe.Name = "Robot Unload";
        }

        public CommonRecipe CommonRecipe { get; }
        public MontionSpeedConfig MontionSpeedConfig { get; }
        public CSTLoadUnloadRecipe CstLoadUnloadRecipe { get; }
        public RobotLoadRecipe RobotLoadRecipe { get; }
        public VinylCleanRecipe VinylCleanRecipe { get; }
        public TransferFixtureRecipe TransferFixtureRecipe { get; }
        public DetachRecipe DetachRecipe { get; }
        public GlassTransferRecipe GlassTransferRecipe { get; }
        public TransferInShuttleRecipe TransferInShuttleLeftRecipe { get; }
        public TransferInShuttleRecipe TransferInShuttleRightRecipe { get; }
        public CleanRecipe WetCleanLeftRecipe { get; }
        public CleanRecipe WetCleanRightRecipe { get; }
        public TransferRotationRecipe TransferRotationLeftRecipe { get; }
        public TransferRotationRecipe TransferRotationRightRecipe { get; }
        public CleanRecipe AfCleanLeftRecipe { get; }
        public CleanRecipe AfCleanRightRecipe { get; }
        public UnloadTransferRecipe UnloadTransferLeftRecipe { get; }
        public UnloadTransferRecipe UnloadTransferRightRecipe { get; }
        public RobotUnloadRecipe RobotUnloadRecipe { get; }
    }
}
