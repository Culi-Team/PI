using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddCatsetteDeviceExtension
    {
        public static IHostBuilder AddCassette(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<CassetteList>();
            });

            return hostBuilder;
        }
    }
}
