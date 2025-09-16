using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddRecipeExtension
    {
        public static IHostBuilder AddRecipes(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<CommonRecipe>();
                services.AddSingleton<CSTLoadUnloadRecipe>();
                services.AddSingleton<RobotLoadRecipe>();
                services.AddSingleton<VinylCleanRecipe>();
                services.AddSingleton<TransferFixtureRecipe>();
                services.AddSingleton<DetachRecipe>();
                services.AddSingleton<GlassTransferRecipe>();
                services.AddKeyedSingleton<TransferInShuttleRecipe>("TransferInShuttleLeftRecipe");
                services.AddKeyedSingleton<TransferInShuttleRecipe>("TransferInShuttleRightRecipe");

                services.AddKeyedSingleton<CleanRecipe>("WETCleanLeftRecipe");
                services.AddKeyedSingleton<CleanRecipe>("WETCleanRightRecipe");

                services.AddKeyedSingleton<TransferRotationRecipe>("TransferRotationLeftRecipe");
                services.AddKeyedSingleton<TransferRotationRecipe>("TransferRotationRightRecipe");

                services.AddKeyedSingleton<CleanRecipe>("AFCleanLeftRecipe");
                services.AddKeyedSingleton<CleanRecipe>("AFCleanRightRecipe");

                services.AddKeyedSingleton<UnloadTransferRecipe>("UnloadTransferLeftRecipe");
                services.AddKeyedSingleton<UnloadTransferRecipe>("UnloadTransferRightRecipe");

                services.AddSingleton<RobotUnloadRecipe>();

                services.AddSingleton<RecipeList>();
                services.AddSingleton<RecipeSelector>();

                // Config services - Multiple instances for different recipes
                services.AddKeyedSingleton<MotionAjinConfig>("MotionAjinConfig");
                services.AddKeyedSingleton<MotionInovanceConfig>("MotionInovanceConfig");
                services.AddKeyedSingleton<VinylCleanEncoderConfig>("VinylCleanEncoderConfig");

                services.AddSingleton<MotionConfigList>();
                services.AddSingleton<MotionConfigSelector>();
            });
            return hostBuilder;
        }
    }
}
