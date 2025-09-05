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
                services.AddKeyedScoped<CleanRecipe>("WETCleanLeftRecipe");
                services.AddKeyedScoped<CleanRecipe>("WETCleanRightRecipe");
                services.AddKeyedScoped<CleanRecipe>("AFCleanLeftRecipe");
                services.AddKeyedScoped<CleanRecipe>("AFCleanRightRecipe");

                services.AddSingleton<RecipeList>();
                services.AddSingleton<RecipeSelector>();
            });
            return hostBuilder;
        }
    }
}
