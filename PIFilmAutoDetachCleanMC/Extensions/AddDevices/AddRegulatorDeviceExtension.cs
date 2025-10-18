using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EQX.Core.Device.Regulator;
using EQX.Device.Regulator;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddRegulatorDeviceExtension
    {
        public static IHostBuilder AddRegulatorDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
#if SIMULATION
                services.AddKeyedScoped<IRegulator, SimulationRegulator>("WETCleanLeft", (ser, obj) =>
                {
                    return new SimulationRegulator(1, "WETCleanLeft");
                });
                services.AddKeyedScoped<IRegulator, SimulationRegulator>("WETCleanRight", (ser, obj) =>
                {
                    return new SimulationRegulator(2, "WETCleanRight");
                });
                services.AddKeyedScoped<IRegulator, SimulationRegulator>("AFCleanLeft", (ser, obj) =>
                {
                    return new SimulationRegulator(3, "AFCleanLeft");
                });
                services.AddKeyedScoped<IRegulator, SimulationRegulator>("AFCleanRight", (ser, obj) =>
                {
                    return new SimulationRegulator(4, "AFCleanRight");
                });
#else
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("WETCleanLeft", (ser, obj) =>
                {
                    return new ITVRegulatorRC(1, "WETCleanLeft", 0.9, "COM12", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("WETCleanRight", (ser, obj) =>
                {
                    return new ITVRegulatorRC(2, "WETCleanRight", 0.9, "COM11", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("AFCleanLeft", (ser, obj) =>
                {
                    return new ITVRegulatorRC(3, "AFCleanLeft", 0.9, "COM14", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("AFCleanRight", (ser, obj) =>
                {
                    return new ITVRegulatorRC(4, "AFCleanRight", 0.9, "COM13", 9600);
                });
#endif
                services.AddSingleton<Regulators>();
            });

            return hostBuilder;
        }
    }
}
