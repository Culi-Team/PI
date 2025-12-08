using EQX.Core.Device.SpeedController;
using EQX.Core.TorqueController;
using EQX.InOut.InOut;
using EQX.InOut.InOut.Analog;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Extensions.AddDevices;

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
            hostBuilder.AddVacuumDevices();
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
