using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EQX.Core.Robot;
using EQX.Motion.Robot;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddRobotDeviceExtension
    {
        public static IHostBuilder AddRobotDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
#if SIMULATION
                //services.AddKeyedSingleton<IRobot, RobotKukaTcp>("RobotLoad", (services, obj) =>
                //{
                //    return new RobotKukaTcp(1, "Kuka Robot Load", "192.168.1.100");
                //});

                services.AddKeyedSingleton<IRobot, RobotSimulation>("RobotLoad", (services, obj) =>
                {
                    return new RobotSimulation(1, "Kuka Robot Load");
                });
                services.AddKeyedSingleton<IRobot, RobotSimulation>("RobotUnload", (services, obj) =>
                {
                    return new RobotSimulation(2, "Kuka Robot Unload");
                });
#else
                services.AddKeyedSingleton<IRobot, RobotKukaTcp>("RobotLoad", (services, obj) =>
                {
                    return new RobotKukaTcp(1, "Kuka Robot Load", "192.168.0.101");
                });

                services.AddKeyedSingleton<IRobot, RobotKukaTcp>("RobotUnload", (services, obj) =>
                {
                    return new RobotKukaTcp(2, "Kuka Robot Unload", "192.168.0.102");
                });
#endif
            });

            return hostBuilder;
        }
    }
}
