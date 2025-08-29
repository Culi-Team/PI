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
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using EQX.Core.Device.Regulator;
using EQX.Device.Regulator;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;

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
                    return new InovanceInputDevice<EInput>()
                    {
                        Id = 1,
                        Name = "InputDevice",
                        MaxPin = 1000,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#1")
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
                    return new InovanceOutputDevice<EOutput>()
                    {
                        Id = 1,
                        Name = "OutputDevice",
                        MaxPin = 1000,
                        MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#1")
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

        public static IHostBuilder AddCylinderDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ICylinderFactory, CylinderFactory>();

                services.AddSingleton<Cylinders>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddRegulatorDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("WETCleanLeft", (ser, obj) =>
                {
                    return new ITVRegulatorRC(1, "WETCleanLeft", 0.9, "COM4", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("WETCleanRight", (ser, obj) =>
                {
                    return new ITVRegulatorRC(2, "WETCleanLeft", 0.9, "COM5", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("AFCleanLeft", (ser, obj) =>
                {
                    return new ITVRegulatorRC(3, "WETCleanLeft", 0.9, "COM6", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("AFCleanRight", (ser, obj) =>
                {
                    return new ITVRegulatorRC(4, "WETCleanLeft", 0.9, "COM7", 9600);
                });

                services.AddSingleton<Regulators>();
            });

            return hostBuilder;
        }
    }
}
