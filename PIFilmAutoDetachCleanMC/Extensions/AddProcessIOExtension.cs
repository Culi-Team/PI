using EQX.Core.InOut;
using EQX.InOut.Virtual;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddProcessIOExtension 
    {
        public static IHostBuilder AddProcessIO(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EInWorkConveyorProcessInput>>("InWorkConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EInWorkConveyorProcessOutput>>("InWorkConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EOutWorkConveyorProcessInput>>("OutWorkConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EOutWorkConveyorProcessOutput>>("OutWorkConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ERobotLoadProcessInput>>("RobotLoadInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ERobotLoadProcessOutput>>("RobotLoadOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EVinylCleanProcessInput>>("VinylCleanInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EVinylCleanProcessOutput>>("VinylCleanOutput");


                services.AddSingleton<ProcessIO>();

            });

            return hostBuilder;
        }
    }
}
