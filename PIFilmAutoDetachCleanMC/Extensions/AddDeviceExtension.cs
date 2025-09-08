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
using EQX.InOut.InOut;
using EQX.Core.TorqueController;
using EQX.Motion.Torque;
using EQX.Core.Robot;
using EQX.Motion.Robot;

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
                services.AddKeyedScoped<IMotionFactory<IMotion>, SimulationMotionFactory>("InovanceMotionFactory");
                services.AddKeyedScoped<IMotionFactory<IMotion>, SimulationMotionFactory>("AjinMotionFactory");
                services.AddKeyedScoped<IMotionFactory<IMotion>, SimulationMotionFactory>("MotionEziPlusEFactory");
#else
                services.AddKeyedScoped<IMotionFactory<IMotion>>("InovanceMotionFactory", (ser, obj) =>
                    new MotionInovanceFactoryWithDefaultCardHandler
                    {
                        MotionController = ser.GetRequiredKeyedService<IMotionController>("InovanceController#1")
                    }
                );

                services.AddKeyedScoped<IMotionFactory<IMotion>>("AjinMotionFactory", (ser, obj) => new MotionAjinFactory());
                services.AddKeyedScoped<IMotionFactory<IMotion>>("MotionEziPlusEFactory", (ser, obj) => new MotionEziPlusEFactory());
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

                services.AddKeyedScoped<IMotion>("VinylCleanEncoder", (ser, obj) =>
                {
                    var configuration = ser.GetRequiredService<IConfiguration>();

                    var vinylCleanEncoderPara = JsonConvert.DeserializeObject<MotionParameter>(
                        File.ReadAllText(configuration["Files:VinylCleanEncoderParaConfigFile"] ?? "")
                    );
                    IMotionFactory<IMotion> motionPlusEFactory = ser.GetRequiredKeyedService<IMotionFactory<IMotion>>("MotionEziPlusEFactory");

                    return motionPlusEFactory.Create(10, "VinylCleanEncoder", vinylCleanEncoderPara);
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
                services.AddKeyedScoped<IDInputDevice>("InputDevice#1", (services, obj) => { return new SimulationInputDeviceClientModbus<EInput>() { Id = 1, Name = "InDevice1", MaxPin = 500 }; });
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
                services.AddKeyedScoped<IDOutputDevice>("OutputDevice#1", (services, obj) => { return new SimulationOutputDevice<EOutput>() { Id = 1, Name = "OutDevice1", MaxPin = 500 }; });
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

                services.AddSingleton<TorqueControllerList>((ser) =>
                {
                    IModbusCommunication modbusCommunication = ser.GetRequiredKeyedService<IModbusCommunication>("TorqueControllerModbusCommunication");

                    var torqueCtlList = Enum.GetNames(typeof(ETorqueController)).ToList();
                    var torqueCtlIndex = (int[])Enum.GetValues(typeof(ETorqueController));

                    var list = new List<ITorqueController>();

                    for (int i = 0; i < torqueCtlList.Count; i++)
                    {
                        list.Add(new DX3000TorqueController(torqueCtlIndex[i], torqueCtlList[i]) { ModbusCommunication = modbusCommunication });
                    }

                    return new TorqueControllerList(list);
                });
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

                services.AddSingleton<SpeedControllerList>((ser) =>
                {
                    IModbusCommunication modbusCommunication = ser.GetRequiredKeyedService<IModbusCommunication>("RollerModbusCommunication");

                    var speedCtlList = Enum.GetNames(typeof(ESpeedController)).ToList();
                    var speedCtlIndex = (int[])Enum.GetValues(typeof(ESpeedController));

                    var speedcontrollerList = new List<ISpeedController>();

                    for (int i = 0; i < speedCtlList.Count; i++)
                    {
#if SIMULATION
                        speedcontrollerList.Add(new SimulationSpeedController(speedCtlIndex[i], speedCtlList[i]));
#else
                        speedcontrollerList.Add(new SD201SSpeedController(speedCtlIndex[i], speedCtlList[i]) { ModbusCommunication = modbusCommunication });
#endif
                    }
                    return new SpeedControllerList(speedcontrollerList);
                });
            });

            return hostBuilder;
        }

        public static IHostBuilder AddCylinderDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
#if SIMULATION
                services.AddSingleton<ICylinderFactory, SimulationCylinderFactory>();
#else
                services.AddSingleton<ICylinderFactory, CylinderFactory>();

#endif
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
                    return new ITVRegulatorRC(2, "WETCleanRight", 0.9, "COM5", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("AFCleanLeft", (ser, obj) =>
                {
                    return new ITVRegulatorRC(3, "AFCleanLeft", 0.9, "COM6", 9600);
                });
                services.AddKeyedScoped<IRegulator, ITVRegulatorRC>("AFCleanRight", (ser, obj) =>
                {
                    return new ITVRegulatorRC(4, "AFCleanRight", 0.9, "COM7", 9600);
                });
                services.AddSingleton<Regulators>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddRobotDevices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
#if SIMULATION
                services.AddKeyedSingleton<IRobot, RobotSimulation>("RobotLoad",(services,obj) =>
                {
                    return new RobotSimulation(1, "Kuka Robot Load");
                });
                services.AddKeyedSingleton<IRobot, RobotSimulation>("RobotUnload", (services, obj) =>
                {
                    return new RobotSimulation(2, "Kuka Robot Unload");
                });
#else
                services.AddKeyedSingleton<IRobot, RobotKukaTcp>("RobotLoad",(services,obj) =>
                {
                    return new RobotKukaTcp(1, "Kuka Robot","192.168.1.100");
                });
                services.AddKeyedSingleton<IRobot, RobotKukaTcp>("RobotUnload",(services,obj) =>
                {
                    return new RobotKukaTcp(2, "Kuka Robot","192.168.1.200");
                });
#endif
            });

            return hostBuilder;
        }

    }
}
