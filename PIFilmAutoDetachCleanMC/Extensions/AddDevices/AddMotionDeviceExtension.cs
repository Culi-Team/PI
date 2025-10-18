using EQX.Core.Motion;
using EQX.Motion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using PIFilmAutoDetachCleanMC.Defines;
using EQX.Motion.ByVendor.Inovance;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using EQX.Motion.ByVendor.Ajinextek;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddMotionDeviceExtension
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

#if SIMULATION
                services.AddKeyedScoped<IMotionMaster, SimulationMotionMaster>("InovanceMaster#1");
                services.AddKeyedScoped<IMotionMaster, SimulationMotionMaster>("AjinMaster#1");

                services.AddKeyedScoped<IMotionFactory<IMotion>, SimulationMotionFactory>("InovanceMotionFactory");
                services.AddKeyedScoped<IMotionFactory<IMotion>, SimulationMotionFactory>("AjinMotionFactory");

                services.AddKeyedScoped<IMotionFactory<IMotion>, SimulationMotionFactory>("MotionEziPlusEFactory");
#else
                services.AddKeyedScoped<IMotionMaster>("InovanceMaster#1", (ser, obj) =>
                {
                    return new MotionMasterInovance() { NumberOfDevices = Enum.GetNames(typeof(EMotionInovance)).Length + 3 };
                });
                services.AddKeyedScoped<IMotionMaster, MotionMasterAjin>("AjinMaster#1", (ser, obj) =>
                {
                    return new MotionMasterAjin() { NumberOfDevices = Enum.GetNames(typeof(EMotionAjin)).Length + 1 };
                });

                services.AddKeyedScoped<IMotionFactory<IMotion>>("InovanceMotionFactory", (ser, obj) =>
                {
                    return new MotionInovanceFactory(ser.GetRequiredKeyedService<IMotionMaster>("InovanceMaster#1"));
                });

                services.AddKeyedScoped<IMotionFactory<IMotion>>("AjinMotionFactory", (ser, obj) =>
                {
                    return new MotionAjinFactory(ser.GetRequiredKeyedService<IMotionMaster>("AjinMaster#1"));
                });
                services.AddKeyedScoped<IMotionFactory<IMotion>>("MotionEziPlusEFactory", (ser, obj) => new MotionEziPlusEFactory());
#endif

                services.AddSingleton<Motions>();

                services.AddKeyedScoped<IMotion>("VinylCleanEncoder", (ser, obj) =>
                {
                    var configuration = ser.GetRequiredService<IConfiguration>();

                    string configContent = File.ReadAllText(configuration["Files:VinylCleanEncoderParaConfigFile"] ?? "");
                    MotionParameter vinylCleanEncoderPara = null;

                    try
                    {
                        // Try to deserialize as single object first
                        vinylCleanEncoderPara = JsonConvert.DeserializeObject<MotionParameter>(configContent);
                    }
                    catch
                    {
                        // If single object fails, try as array and take first element
                        var configArray = JsonConvert.DeserializeObject<MotionParameter[]>(configContent);
                        if (configArray != null && configArray.Length > 0)
                        {
                            vinylCleanEncoderPara = configArray[0];
                        }
                    }

                    IMotionFactory<IMotion> motionPlusEFactory = ser.GetRequiredKeyedService<IMotionFactory<IMotion>>("MotionEziPlusEFactory");

                    return motionPlusEFactory.Create(10, "VinylCleanEncoder", vinylCleanEncoderPara);
                });

                services.AddSingleton<Devices>();
            });

            return hostBuilder;
        }
    }
}
