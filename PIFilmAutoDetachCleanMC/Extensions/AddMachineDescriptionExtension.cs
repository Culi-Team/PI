using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Process;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddMachineDescriptionExtension
    {
        public static IHostBuilder AddMachineDescriptions(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<Information>();
                services.AddSingleton<MachineStatus>();
            });

            return hostBuilder;
        }
    }
}
