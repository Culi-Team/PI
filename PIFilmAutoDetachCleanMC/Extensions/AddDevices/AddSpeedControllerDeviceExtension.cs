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

                services.AddSingleton<RollerList>((ser) =>
                {
                    IModbusCommunication modbusCommunication = ser.GetRequiredKeyedService<IModbusCommunication>("RollerModbusCommunication");

                    var speedCtlList = Enum.GetNames(typeof(ERoller)).ToList();
                    var speedCtlIndex = (int[])Enum.GetValues(typeof(ERoller));

                    var speedcontrollerList = new List<BD201SRollerController>();

                    for (int i = 0; i < speedCtlList.Count; i++)
                    {

                        speedcontrollerList.Add(new BD201SRollerController(speedCtlIndex[i], speedCtlList[i], modbusCommunication));
                    }
                    return new RollerList(speedcontrollerList);
                });
            });

            return hostBuilder;
        }
    }
}
