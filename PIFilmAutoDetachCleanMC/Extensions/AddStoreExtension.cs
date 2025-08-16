using EQX.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddStoreExtension
    {
        public static IHostBuilder AddStores(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<UserStore>();

                services.AddSingleton<Processes>();
            });

            return hostBuilder;
        }
    }
}
