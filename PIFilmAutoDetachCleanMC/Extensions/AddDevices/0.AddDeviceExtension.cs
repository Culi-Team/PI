using Microsoft.Extensions.Hosting;
using EQX.Core.Device.SpeedController;
using EQX.InOut.InOut;
using EQX.Core.TorqueController;
using EQX.InOut.InOut.Analog;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddDeviceExtension
    {
        public static IHostBuilder AddDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.AddMotionDevices();
            hostBuilder.AddIODevices();
            hostBuilder.AddTorqueControllerDevices();
            hostBuilder.AddSpeedControllerDevices();
            hostBuilder.AddCylinderDevices();
            hostBuilder.AddRegulatorDevices();
            hostBuilder.AddSyringePumpDevices();
            hostBuilder.AddPlasmaDevices();
            hostBuilder.AddRobotDevices();
            hostBuilder.AddCassette();
            hostBuilder.AddIndicatorDevices();

            return hostBuilder;
        }

    }
}
