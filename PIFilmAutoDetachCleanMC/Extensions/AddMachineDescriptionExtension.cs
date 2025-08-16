using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddMachineDescriptionExtension
    {
        public static IHostBuilder AddMachineDescriptions(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<Information>();
            });

            return hostBuilder;
        }
    }
}
