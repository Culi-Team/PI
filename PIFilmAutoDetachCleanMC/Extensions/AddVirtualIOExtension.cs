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
                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EInConveyorProcessInput>>("InConveyorInput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EWorkConveyorProcessInput>>("InWorkConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EWorkConveyorProcessOutput>>("InWorkConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EBufferConveyorProcessInput>>("BufferConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EBufferConveyorProcessOutput>>("BufferConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EWorkConveyorProcessInput>>("OutWorkConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EWorkConveyorProcessOutput>>("OutWorkConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EOutConveyorProcessInput>>("OutConveyorInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EOutConveyorProcessOutput>>("OutConveyorOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ERobotLoadProcessInput>>("RobotLoadInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ERobotLoadProcessOutput>>("RobotLoadOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EVinylCleanProcessInput>>("VinylCleanInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EVinylCleanProcessOutput>>("VinylCleanOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EFixtureAlignProcessInput>>("FixtureAlignInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EFixtureAlignProcessOutput>>("FixtureAlignOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ERemoveFilmProcessInput>>("RemoveFilmInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ERemoveFilmProcessOutput>>("RemoveFilmOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ETransferFixtureProcessInput>>("TransferFixtureInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ETransferFixtureProcessOutput>>("TransferFixtureOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EDetachProcessInput>>("DetachInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EDetachProcessOutput>>("DetachOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EGlassTransferProcessInput>>("GlassTransferInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EGlassTransferProcessOutput>>("GlassTransferOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ETransferInShuttleProcessInput>>("TransferInShuttleLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ETransferInShuttleProcessOutput>>("TransferInShuttleLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ETransferInShuttleProcessInput>>("TransferInShuttleRightInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ETransferInShuttleProcessOutput>>("TransferInShuttleRightOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ECleanProcessInput>>("WETCleanLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ECleanProcessOutput>>("WETCleanLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ECleanProcessInput>>("WETCleanRightInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ECleanProcessOutput>>("WETCleanRightOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ECleanProcessInput>>("AFCleanLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ECleanProcessOutput>>("AFCleanLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ECleanProcessInput>>("AFCleanRightInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ECleanProcessOutput>>("AFCleanRightOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ETransferRotationProcessInput>>("TransferRotationLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ETransferRotationProcessOutput>>("TransferRotationLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ETransferRotationProcessInput>>("TransferRotationRightInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ETransferRotationProcessOutput>>("TransferRotationRightOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EUnloadTransferProcessInput>>("UnloadTransferLeftInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EUnloadTransferProcessOutput>>("UnloadTransferLeftOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EUnloadTransferProcessInput>>("UnloadTransferRightInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EUnloadTransferProcessOutput>>("UnloadTransferRightOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<EUnloadAlignProcessInput>>("UnloadAlignInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<EUnloadAlignProcessOutput>>("UnloadAlignOutput");

                services.AddKeyedSingleton<IDInputDevice, MappableInputDevice<ERobotUnloadProcessInput>>("RobotUnloadInput");
                services.AddKeyedSingleton<IDOutputDevice, MappableOutputDevice<ERobotUnloadProcessOutput>>("RobotUnloadOutput");

                services.AddSingleton<VirtualIO>();

            });

            return hostBuilder;
        }
    }
}
