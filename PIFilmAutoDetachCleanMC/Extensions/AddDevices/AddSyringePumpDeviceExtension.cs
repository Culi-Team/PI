using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using EQX.Core.Communication;
using EQX.Core.Device.SyringePump;
using EQX.Device.SyringePump;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddSyringePumpDeviceExtension
    {
        public static IHostBuilder AddSyringePumpDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedSingleton<SerialCommunicator>("SyringePumpSerialCommunicator", (ser, obj) =>
                {
                    return new SerialCommunicator(1, "SyringePumpSerialCommunicator", "COM17", 38400);
                });
                services.AddKeyedSingleton<ISyringePump>("WETCleanRightSyringePump", (ser, obj) =>
                {
                    var serialCommunicator = ser.GetRequiredKeyedService<SerialCommunicator>("SyringePumpSerialCommunicator");
#if SIMULATION
                    return new SimulationSyringePump("WETCleanRightSyringePump", 1);
#else
                    return new PSD4SyringePump("WETCleanRightSyringePump", 1, serialCommunicator, 1.0);
#endif
                });
                services.AddKeyedSingleton<ISyringePump>("WETCleanLeftSyringePump", (ser, obj) =>
                {
                    var serialCommunicator = ser.GetRequiredKeyedService<SerialCommunicator>("SyringePumpSerialCommunicator");
#if SIMULATION
                    return new SimulationSyringePump("WETCleanLeftSyringePump", 2);
#else
                    return new PSD4SyringePump("WETCleanLeftSyringePump", 2, serialCommunicator, 1.0);
#endif
                });
                services.AddKeyedSingleton<ISyringePump>("AFCleanRightSyringePump", (ser, obj) =>
                {
                    var serialCommunicator = ser.GetRequiredKeyedService<SerialCommunicator>("SyringePumpSerialCommunicator");
#if SIMULATION
                    return new SimulationSyringePump("AFCleanRightSyringePump", 3);
#else
                    return new PSD4SyringePump("AFCleanRightSyringePump", 3, serialCommunicator, 1.0);
#endif
                });
                services.AddKeyedSingleton<ISyringePump>("AFCleanLeftSyringePump", (ser, obj) =>
                {
                    var serialCommunicator = ser.GetRequiredKeyedService<SerialCommunicator>("SyringePumpSerialCommunicator");
#if SIMULATION
                    return new SimulationSyringePump("AFCleanLeftSyringePump", 4);
#else
                    return new PSD4SyringePump("AFCleanLeftSyringePump", 4, serialCommunicator, 1.0);
#endif
                });

                services.AddSingleton<SyringePumps>();
            });

            return hostBuilder;
        }
    }
}
