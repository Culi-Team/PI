using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.InOut;
using EQX.InOut.ByVendor.Ajinextek;
using EQX.InOut.ByVendor.Inovance;
using EQX.InOut.InOut.Analog;
using EQX.Motion.ByVendor.Inovance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddIODevicesExtension
    {
        public static IHostBuilder AddIODevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
#if SIMULATION
                services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new SimulationInputDevice_ClientMMF<EInput>() { Id = 1, Name = "InDevice1", MaxPin = 500 }; });
#else
                services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) =>
                {
                    return new InovanceInputDevice<EInput>()
                    {
                        Id = 1,
                        Name = "InputDevice",
                        MaxPin = 1000,
                        MotionMaster = (MotionMasterInovance)services.GetRequiredKeyedService<IMotionMaster>("InovanceMaster#1")
                    };
                });
                //services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new AjinInputDevice<EInput1> { Id = 1, Name = "InDevice1", MaxPin = 32 }; });
#endif

#if SIMULATION
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) => { return new SimulationOutputDevice<EOutput>() { Id = 1, Name = "OutDevice1", MaxPin = 500 }; });
#else
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) =>
                {
                    return new InovanceOutputDevice<EOutput>()
                    {
                        Id = 1,
                        Name = "OutputDevice",
                        MaxPin = 1000,
                        MotionMaster = (MotionMasterInovance)services.GetRequiredKeyedService<IMotionMaster>("InovanceMaster#1")
                    };
                });
                //services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) => { return new AjinOutputDevice<EOutput1> { Id = 1, Name = "OutDevice1", MaxPin = 32 }; });
#endif
#if SIMULATION
                services.AddKeyedSingleton<IAInputDevice>("AnalogInputDevice#1", (services, obj) => { return new SimulationAnalogInputDevice<EAnalogInput>(); });
#else
                services.AddKeyedSingleton<IAInputDevice>("AnalogInputDevice#1", (services, obj) =>
                {
                    return new AjinAnalogInputDevice<EAnalogInput>()
                    {
                        Id = 9,
                        Name = "AnalogInputDevice",
                    };
                });
#endif

                services.AddSingleton<Inputs>();
                services.AddSingleton<Outputs>();

                services.AddSingleton<AnalogInputs>();
            });

            return hostBuilder;
        }
    }
}
