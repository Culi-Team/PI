using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using EQX.Core.Communication.Modbus;
using EQX.Device.Torque;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddTorqueControllerDeviceExtension
    {
        public static IHostBuilder AddTorqueControllerDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<IModbusCommunication>("TorqueControllerModbusCommunication", (services, obj) =>
                {
                    return new ModbusRTUCommunication("COM16", 115200);
                });

                services.AddSingleton<TorqueControllerList>((ser) =>
                {
                    IModbusCommunication modbusCommunication = ser.GetRequiredKeyedService<IModbusCommunication>("TorqueControllerModbusCommunication");

                    var torqueCtlList = Enum.GetNames(typeof(ETorqueController)).ToList();
                    var torqueCtlIndex = (int[])Enum.GetValues(typeof(ETorqueController));

                    var list = new List<DX3000TorqueController>();

                    for (int i = 0; i < torqueCtlList.Count; i++)
                    {
                        list.Add(new DX3000TorqueController(torqueCtlIndex[i], torqueCtlList[i], modbusCommunication));

                    }

                    return new TorqueControllerList(list);
                });
            });

            return hostBuilder;
        }
    }
}
