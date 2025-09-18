using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Extensions;

namespace PIFilmAutoDetachCleanMC.Test
{
    public class TestAppCommon
    {
        public static IHost? AppHost { get; internal set; }

        public static IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
                .AddConfigs()
                .AddViews()
                .AddViewModels()
                .AddStores()
                .AddMachineDescriptions()
                .AddIODevices()
                .AddProcessIO()
                .AddMotionDevices()
                .AddCylinderDevices()
                .AddRegulatorDevices()
                .AddRobotDevices()
                .AddSpeedControllerDevices()
                .AddTorqueControllerDevices()
                .AddRecipes()
                .AddProcesses()
                .AddCassette()
                .Build();
        }
    }
}
