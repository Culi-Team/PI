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
#if !SIMULATION
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
#else

                services.AddKeyedSingleton<IDInputDevice>("InConveyorInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EInConveyorProcessInput>("InConveyorInput"));

                services.AddKeyedSingleton<IDInputDevice>("InWorkConveyorInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EWorkConveyorProcessInput>("InWorkConveyorInput"));
                services.AddKeyedSingleton<IDOutputDevice>("InWorkConveyorOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EWorkConveyorProcessOutput>("InWorkConveyorOutput"));

                services.AddKeyedSingleton<IDInputDevice>("BufferConveyorInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EBufferConveyorProcessInput>("BufferConveyorInput"));
                services.AddKeyedSingleton<IDOutputDevice>("BufferConveyorOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EBufferConveyorProcessOutput>("BufferConveyorOutput"));

                services.AddKeyedSingleton<IDInputDevice>("OutWorkConveyorInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EWorkConveyorProcessInput>("OutWorkConveyorInput"));
                services.AddKeyedSingleton<IDOutputDevice>("OutWorkConveyorOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EWorkConveyorProcessOutput>("OutWorkConveyorOutput"));

                services.AddKeyedSingleton<IDInputDevice>("OutConveyorInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EOutConveyorProcessInput>("OutConveyorInput"));
                services.AddKeyedSingleton<IDOutputDevice>("OutConveyorOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EOutConveyorProcessOutput>("OutConveyorOutput"));

                services.AddKeyedSingleton<IDInputDevice>("RobotLoadInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ERobotLoadProcessInput>("RobotLoadInput"));
                services.AddKeyedSingleton<IDOutputDevice>("RobotLoadOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ERobotLoadProcessOutput>("RobotLoadOutput"));

                services.AddKeyedSingleton<IDInputDevice>("VinylCleanInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EVinylCleanProcessInput>("VinylCleanInput"));
                services.AddKeyedSingleton<IDOutputDevice>("VinylCleanOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EVinylCleanProcessOutput>("VinylCleanOutput"));

                services.AddKeyedSingleton<IDInputDevice>("FixtureAlignInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EFixtureAlignProcessInput>("FixtureAlignInput"));
                services.AddKeyedSingleton<IDOutputDevice>("FixtureAlignOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EFixtureAlignProcessOutput>("FixtureAlignOutput"));

                services.AddKeyedSingleton<IDInputDevice>("RemoveFilmInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ERemoveFilmProcessInput>("RemoveFilmInput"));
                services.AddKeyedSingleton<IDOutputDevice>("RemoveFilmOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ERemoveFilmProcessOutput>("RemoveFilmOutput"));

                services.AddKeyedSingleton<IDInputDevice>("TransferFixtureInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ETransferFixtureProcessInput>("TransferFixtureInput"));
                services.AddKeyedSingleton<IDOutputDevice>("TransferFixtureOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferFixtureProcessOutput>("TransferFixtureOutput"));

                services.AddKeyedSingleton<IDInputDevice>("DetachInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EDetachProcessInput>("DetachInput"));
                services.AddKeyedSingleton<IDOutputDevice>("DetachOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EDetachProcessOutput>("DetachOutput"));

                services.AddKeyedSingleton<IDInputDevice>("GlassTransferInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EGlassTransferProcessInput>("GlassTransferInput"));
                services.AddKeyedSingleton<IDOutputDevice>("GlassTransferOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EGlassTransferProcessOutput>("GlassTransferOutput"));

                services.AddKeyedSingleton<IDInputDevice>("GlassAlignLeftInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EGlassAlignProcessInput>("GlassAlignLeftInput"));
                services.AddKeyedSingleton<IDOutputDevice>("GlassAlignLeftOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EGlassAlignProcessOutput>("GlassAlignLeftOutput"));

                services.AddKeyedSingleton<IDInputDevice>("GlassAlignRightInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EGlassAlignProcessInput>("GlassAlignRightInput"));
                services.AddKeyedSingleton<IDOutputDevice>("GlassAlignRightOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EGlassAlignProcessOutput>("GlassAlignRightOutput"));

                services.AddKeyedSingleton<IDInputDevice>("TransferInShuttleLeftInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ETransferInShuttleProcessInput>("TransferInShuttleLeftInput"));
                services.AddKeyedSingleton<IDOutputDevice>("TransferInShuttleLeftOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferInShuttleProcessOutput>("TransferInShuttleLeftOutput"));

                services.AddKeyedSingleton<IDInputDevice>("TransferInShuttleRightInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ETransferInShuttleProcessInput>("TransferInShuttleRightInput"));
                services.AddKeyedSingleton<IDOutputDevice>("TransferInShuttleRightOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferInShuttleProcessOutput>("TransferInShuttleRightOutput"));

                services.AddKeyedSingleton<IDInputDevice>("WETCleanLeftInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("WETCleanLeftInput"));
                services.AddKeyedSingleton<IDOutputDevice>("WETCleanLeftOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("WETCleanLeftOutput"));

                services.AddKeyedSingleton<IDInputDevice>("WETCleanRightInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("WETCleanRightInput"));
                services.AddKeyedSingleton<IDOutputDevice>("WETCleanRightOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("WETCleanRightOutput"));

                services.AddKeyedSingleton<IDInputDevice>("AFCleanLeftInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("AFCleanLeftInput"));
                services.AddKeyedSingleton<IDOutputDevice>("AFCleanLeftOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("AFCleanLeftOutput"));

                services.AddKeyedSingleton<IDInputDevice>("AFCleanRightInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ECleanProcessInput>("AFCleanRightInput"));
                services.AddKeyedSingleton<IDOutputDevice>("AFCleanRightOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ECleanProcessOutput>("AFCleanRightOutput"));

                services.AddKeyedSingleton<IDInputDevice>("TransferRotationLeftInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ETransferRotationProcessInput>("TransferRotationLeftInput"));
                services.AddKeyedSingleton<IDOutputDevice>("TransferRotationLeftOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferRotationProcessOutput>("TransferRotationLeftOutput"));

                services.AddKeyedSingleton<IDInputDevice>("TransferRotationRightInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<ETransferRotationProcessInput>("TransferRotationRightInput"));
                services.AddKeyedSingleton<IDOutputDevice>("TransferRotationRightOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<ETransferRotationProcessOutput>("TransferRotationRightOutput"));

                services.AddKeyedSingleton<IDInputDevice>("UnloadTransferLeftInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EUnloadTransferProcessInput>("UnloadTransferLeftInput"));
                services.AddKeyedSingleton<IDOutputDevice>("UnloadTransferLeftOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EUnloadTransferProcessOutput>("UnloadTransferLeftOutput"));

                services.AddKeyedSingleton<IDInputDevice>("UnloadTransferRightInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EUnloadTransferProcessInput>("UnloadTransferRightInput"));
                services.AddKeyedSingleton<IDOutputDevice>("UnloadTransferRightOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EUnloadTransferProcessOutput>("UnloadTransferRightOutput"));

                services.AddKeyedSingleton<IDInputDevice>("UnloadAlignInput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddInputDevice<EUnloadAlignProcessInput>("UnloadAlignInput"));
                services.AddKeyedSingleton<IDOutputDevice>("UnloadAlignOutput", (services, _) =>
                    VirtualDeviceRegistry.GetOrAddOutputDevice<EUnloadAlignProcessOutput>("UnloadAlignOutput"));
#endif

                services.AddSingleton<VirtualIO>();

            });

            return hostBuilder;
        }
    }
}
