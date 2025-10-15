using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using EQX.Core.Communication.Modbus;
using EQX.Device.SpeedController;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddSpeedControllerDeviceExtension
    {
        public static IHostBuilder AddSpeedControllerDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<IModbusCommunication>("RollerModbusCommunication", (services, obj) =>
                {
                    return new ModbusRTUCommunication("COM15", 38400);
                });

                services.AddSingleton<SpeedControllerList>((ser) =>
                {
                    IModbusCommunication modbusCommunication = ser.GetRequiredKeyedService<IModbusCommunication>("RollerModbusCommunication");

                    var speedCtlList = Enum.GetNames(typeof(ESpeedController)).ToList();
                    var speedCtlIndex = (int[])Enum.GetValues(typeof(ESpeedController));

                    var speedcontrollerList = new List<SD201SSpeedController>();

                    for (int i = 0; i < speedCtlList.Count; i++)
                    {

                        speedcontrollerList.Add(new SD201SSpeedController(speedCtlIndex[i], speedCtlList[i], modbusCommunication));
                    }
                    return new SpeedControllerList(speedcontrollerList);
                });
            });

            return hostBuilder;
        }
    }
}
