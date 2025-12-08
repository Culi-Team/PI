using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.InOut;
using EQX.InOut;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines.Devices.Vacuum;

namespace PIFilmAutoDetachCleanMC.Extensions.AddDevices
{
    public static class AddVacuumDeviceExtension
    {
        public static IHostBuilder AddVacuumDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
#if SIMULATION
                services.AddSingleton<IVacuumFactory, SimulationVacuumFactory>();
#else
                services.AddSingleton<IVacuumFactory, VacuumFactory>();
#endif
                services.AddSingleton<Vacuums>();
            });

            return hostBuilder;
        }
    }
}
