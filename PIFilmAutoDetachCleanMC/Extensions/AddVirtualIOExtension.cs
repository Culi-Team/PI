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
    public static class AddVirtualIOExtension 
    {
        public static IHostBuilder AddProcessIO(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EInConveyorProcessInput>>("InConveyorInput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EWorkConveyorProcessInput>>("InWorkConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EWorkConveyorProcessOutput>>("InWorkConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EBufferConveyorProcessInput>>("BufferConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EBufferConveyorProcessOutput>>("BufferConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EWorkConveyorProcessInput>>("OutWorkConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EWorkConveyorProcessOutput>>("OutWorkConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EOutConveyorProcessInput>>("OutConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EOutConveyorProcessOutput>>("OutConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ERobotLoadProcessInput>>("RobotLoadInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ERobotLoadProcessOutput>>("RobotLoadOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EVinylCleanProcessInput>>("VinylCleanInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EVinylCleanProcessOutput>>("VinylCleanOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EFixtureAlignProcessInput>>("FixtureAlignInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EFixtureAlignProcessOutput>>("FixtureAlignOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ERemoveFilmProcessInput>>("RemoveFilmInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ERemoveFilmProcessOutput>>("RemoveFilmOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ETransferFixtureProcessInput>>("TransferFixtureInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ETransferFixtureProcessOutput>>("TransferFixtureOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EDetachProcessInput>>("DetachInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EDetachProcessOutput>>("DetachOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EGlassTransferProcessInput>>("GlassTransferInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EGlassTransferProcessOutput>>("GlassTransferOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EGlassAlignProcessInput>>("GlassAlignLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EGlassAlignProcessOutput>>("GlassAlignLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EGlassAlignProcessInput>>("GlassAlignRightInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EGlassAlignProcessOutput>>("GlassAlignRightOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ETransferInShuttleProcessInput>>("TransferInShuttleLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ETransferInShuttleProcessOutput>>("TransferInShuttleLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ETransferInShuttleProcessInput>>("TransferInShuttleRightInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ETransferInShuttleProcessOutput>>("TransferInShuttleRightOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ECleanProcessInput>>("WETCleanLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ECleanProcessOutput>>("WETCleanLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ECleanProcessInput>>("WETCleanRightInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ECleanProcessOutput>>("WETCleanRightOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ECleanProcessInput>>("AFCleanLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ECleanProcessOutput>>("AFCleanLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ECleanProcessInput>>("AFCleanRightInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ECleanProcessOutput>>("AFCleanRightOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ETransferRotationProcessInput>>("TransferRotationLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ETransferRotationProcessOutput>>("TransferRotationLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<ETransferRotationProcessInput>>("TransferRotationRightInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<ETransferRotationProcessOutput>>("TransferRotationRightOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EUnloadTransferProcessInput>>("UnloadTransferLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EUnloadTransferProcessOutput>>("UnloadTransferLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EUnloadTransferProcessInput>>("UnloadTransferRightInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EUnloadTransferProcessOutput>>("UnloadTransferRightOutput");

                services.AddKeyedSingleton<IDInputDevice, VirtualInputDevice<EUnloadAlignProcessInput>>("UnloadAlignInput");
                services.AddKeyedSingleton<IDOutputDevice, VirtualOutputDevice<EUnloadAlignProcessOutput>>("UnloadAlignOutput");

                services.AddSingleton<VirtualIO>();

            });

            return hostBuilder;
        }
    }
}
