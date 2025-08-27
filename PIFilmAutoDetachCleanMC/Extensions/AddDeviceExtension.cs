using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.InOut;
using EQX.Motion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using PIFilmAutoDetachCleanMC.Defines;
using EQX.Motion.ByVendor.Inovance;
using System.IO.Ports;
using EQX.Core.Communication.Modbus;
using EQX.Core.Device.SpeedController;
using EQX.Device.SpeedController;
using EQX.InOut.ByVendor.Inovance;
using PIFilmAutoDetachCleanMC.Defines.Devices;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddDeviceExtension
    {
        public static IHostBuilder AddMotionDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<List<IMotionParameter>>("MotionInovanceParameters", (ser, obj) =>
                {
                    var configuration = ser.GetRequiredService<IConfiguration>();

                    var motionParameters = JsonConvert.DeserializeObject<List<MotionInovanceParameter>>(
                        File.ReadAllText(configuration["Files:MotionInovanceParaConfigFile"] ?? "")
                    );
                    if (motionParameters == null)
                    {
                        throw new FormatException("MotionParaConfigFile format error");
                    }
                    List<IMotionParameter> result = new List<IMotionParameter>();
                    foreach (var parameter in motionParameters)
                    {
                        result.Add(parameter);
                    }
                    return result;
                });

                services.AddKeyedScoped<List<IMotionParameter>>("MotionAjinParameters", (ser, obj) =>
                {
                    var configuration = ser.GetRequiredService<IConfiguration>();

                    var motionParameters = JsonConvert.DeserializeObject<List<MotionAjinParameter>>(
                        File.ReadAllText(configuration["Files:MotionAjinParaConfigFile"] ?? "")
                    );
                    if (motionParameters == null)
                    {
                        throw new FormatException("MotionParaConfigFile format error");
                    }
                    List<IMotionParameter> result = new List<IMotionParameter>();
                    foreach (var parameter in motionParameters)
                    {
                        result.Add(parameter);
                    }
                    return result;
                });

                services.AddKeyedScoped<IMotionController, MotionControllerInovance>("InovanceController#1");
                services.AddKeyedScoped<IMotionController, MotionControllerInovance>("InovanceController#2");
                services.AddKeyedScoped<IMotionController, MotionControllerInovance>("InovanceController#3");
                services.AddKeyedScoped<IMotionController, MotionControllerInovance>("InovanceController#4");

#if SIMULATION
                services.AddSingleton<IMotionFactory<IMotion>, SimulationMotionFactory>();
#else
                services.AddKeyedScoped<IMotionFactory<IMotion>>("InovanceMotionFactory", (ser, obj) =>
                    new MotionInovanceFactoryWithDefaultCardHandler
                    {
                        MotionController = ser.GetRequiredKeyedService<IMotionController>("InovanceController#1")
                    }
                );

                services.AddKeyedScoped<IMotionFactory<IMotion>>("AjinMotionFactory", (ser, obj) => new MotionAjinFactory());
#endif

                services.AddSingleton<MotionsInovance>(ser =>
                {
                    return new MotionsInovance(
                        ser.GetRequiredKeyedService<IMotionFactory<IMotion>>("InovanceMotionFactory"),
                        ser.GetRequiredKeyedService<List<IMotionParameter>>("MotionInovanceParameters"),
                        ser.GetRequiredKeyedService<IMotionController>("InovanceController#1")
                    );
                });

                services.AddSingleton<MotionsAjin>(ser =>
                {
                    return new MotionsAjin(
                        ser.GetRequiredKeyedService<IMotionFactory<IMotion>>("AjinMotionFactory"),
                        ser.GetRequiredKeyedService<List<IMotionParameter>>("MotionAjinParameters")
                        );
                });

                services.AddSingleton<Devices>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddIODevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
#if SIMULATION
                services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new SimulationInputDevice_Client<EInput1>() { Id = 1, Name = "InDevice1", MaxPin = 96 }; });
                services.AddKeyedScoped<IDInputDevice>("InputDevice#2", (services, obj) => { return new SimulationInputDevice_Client<EInput2>() { Id = 2, Name = "InDevice2", MaxPin = 96 }; });
                services.AddKeyedScoped<IDInputDevice>("InputDevice#3", (services, obj) => { return new SimulationInputDevice_Client<EInput3>() { Id = 3, Name = "InDevice3", MaxPin = 96 }; });
                services.AddKeyedScoped<IDInputDevice>("InputDevice#4", (services, obj) => { return new SimulationInputDevice_Client<EInput4>() { Id = 4, Name = "InDevice4", MaxPin = 96 }; });
#else
                services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) =>
                {
                    return new InovanceInputDevice<EInput1>()
                    {
                        Id = 1,
                        Name = "InDevice1",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#1")
                    };
                });

                services.AddKeyedScoped<IDInputDevice>("InputDevice#2", (services, obj) =>
                {
                    return new InovanceInputDevice<EInput2>()
                    {
                        Id = 2,
                        Name = "InDevice2",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#2")
                    };
                });

                services.AddKeyedScoped<IDInputDevice>("InputDevice#3", (services, obj) =>
                {
                    return new InovanceInputDevice<EInput3>()
                    {
                        Id = 3,
                        Name = "InDevice3",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#3")
                    };
                });

                services.AddKeyedScoped<IDInputDevice>("InputDevice#4", (services, obj) =>
                {
                    return new InovanceInputDevice<EInput4>()
                    {
                        Id = 4,
                        Name = "InDevice4",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#4")
                    };
                });
                //services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new AjinInputDevice<EInput1> { Id = 1, Name = "InDevice1", MaxPin = 32 }; });
#endif

#if SIMULATION
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) => { return new SimulationOutputDevice<EOutput1>() { Id = 1, Name = "OutDevice1", MaxPin = 96 }; });
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#2", (services, obj) => { return new SimulationOutputDevice<EOutput2>() { Id = 2, Name = "OutDevice2", MaxPin = 96 }; });
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#3", (services, obj) => { return new SimulationOutputDevice<EOutput3>() { Id = 3, Name = "OutDevice3", MaxPin = 96 }; });
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#4", (services, obj) => { return new SimulationOutputDevice<EOutput4>() { Id = 4, Name = "OutDevice4", MaxPin = 96 }; });
#else
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) =>
                {
                    return new InovanceOutputDevice<EOutput1>()
                    {
                        Id = 1,
                        Name = "OutDevice1",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#1")
                    };
                });

                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#2", (services, obj) =>
                {
                    return new InovanceOutputDevice<EOutput2>()
                    {
                        Id = 2,
                        Name = "OutDevice2",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#2")
                    };
                });

                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#3", (services, obj) =>
                {
                    return new InovanceOutputDevice<EOutput3>()
                    {
                        Id = 3,
                        Name = "OutDevice3",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#3")
                    };
                });

                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#4", (services, obj) =>
                {
                    return new InovanceOutputDevice<EOutput4>()
                    {
                        Id = 4,
                        Name = "OutDevice4",
                        MaxPin = 96,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#4")
                    };
                });
                //services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) => { return new AjinOutputDevice<EOutput1> { Id = 1, Name = "OutDevice1", MaxPin = 32 }; });
#endif

                services.AddSingleton<Inputs>();
                services.AddSingleton<Outputs>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddTorqueControllerDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<IModbusCommunication>("TorqueControllerModbusCommunication", (services, obj) =>
                {
                    return new ModbusRTUCommunication("COM2", 9600);
                });

                services.AddSingleton<TorqueControllerList>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddSpeedControllerDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<IModbusCommunication>("RollerModbusCommunication", (services, obj) =>
                {
                    return new ModbusRTUCommunication("COM3", 9600);
                });

                services.AddSingleton<SpeedControllerList>();
            });

            return hostBuilder;
        }

    }
}
