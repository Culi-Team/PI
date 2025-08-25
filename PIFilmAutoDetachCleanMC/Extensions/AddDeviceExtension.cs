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
                services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new SimulationInputDevice_Client<EInput1>() { Id = 1, Name = "InDevice1", MaxPin = 32 }; });
#else
                services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new InovanceInputDevice<EInput1>() 
                {
                    Id = 1, Name = "InDevice1", MaxPin = 1000 ,
                    MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#1")
                }; });
                //services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new AjinInputDevice<EInput1> { Id = 1, Name = "InDevice1", MaxPin = 32 }; });
#endif

#if SIMULATION
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) => { return new SimulationOutputDevice<EOutput1>() { Id = 1, Name = "OutDevice1", MaxPin = 32 }; });
#else
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) => { return new InovanceOutputDevice<EOutput1>() 
                {
                    Id = 1, Name = "OutDevice1", MaxPin = 1000 ,
                    MotionController = (MotionControllerInovance)services.GetRequiredKeyedService<IMotionController>("InovanceController#1")
                }; });
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
