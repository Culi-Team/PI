using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EQX.Core.Communication.Modbus;
using EQX.Device.Indicator;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddIndicatorDeviceExtension
    {
        public static IHostBuilder AddIndicatorDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<IModbusCommunication>("IndicatorModbusCommunication", (services, obj) =>
                {
                    return new ModbusRTUCommunication("COM9", 9600);
                });

                services.AddSingleton<NEOSHSDIndicator>((ser) =>
                {
                    return new NEOSHSDIndicator(1, "Indicator,", ser.GetRequiredKeyedService<IModbusCommunication>("IndicatorModbusCommunication"));
                });
            });

            return hostBuilder;
        }
    }
}
