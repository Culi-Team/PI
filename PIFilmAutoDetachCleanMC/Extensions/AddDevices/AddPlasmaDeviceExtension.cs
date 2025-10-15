using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines.Devices;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class  AddPlasmaDeviceExtension
    {
        public static IHostBuilder AddPlasmaDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DieHardK180Plasma>();
            });
            return hostBuilder;
        }
    }
}
