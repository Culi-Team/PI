using EQX.Core.Process;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddProcessExtension
    {
        public static IHostBuilder AddProcesses(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<IProcess<ESequence>, RootProcess<ESequence, ESemiSequence>>(EProcess.Root.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, InConveyorProcess>(EProcess.InConveyor.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, WorkConveyorProcess>(EProcess.InWorkConveyor.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, BufferConveyorProcess>(EProcess.BufferConveyor.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, WorkConveyorProcess>(EProcess.OutWorkConveyor.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, OutConveyorProcess>(EProcess.OutConveyor.ToString());

                services.AddKeyedScoped<IProcess<ESequence>, VinylCleanProcess>(EProcess.VinylClean.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, RobotLoadProcess>(EProcess.RobotLoad.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, FixtureAlignProcess>(EProcess.FixtureAlign.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, TransferFixtrueProcess>(EProcess.TransferFixture.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, RemoveFilmProcess>(EProcess.RemoveFilm.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, DetachProcess>(EProcess.Detach.ToString());

                services.AddKeyedScoped<IProcess<ESequence>, GlassTransferProcess>(EProcess.GlassTransfer.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, GlassAlignProcess>(EProcess.GlassAlignLeft.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, GlassAlignProcess>(EProcess.GlassAlignRight.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, TransferInShuttleProcess>(EProcess.TransferInShuttleLeft.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, TransferInShuttleProcess>(EProcess.TransferInShuttleRight.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, CleanProcess>(EProcess.WETCleanLeft.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, CleanProcess>(EProcess.WETCleanRight.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, TransferRotationProcess>(EProcess.TransferRotationLeft.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, TransferRotationProcess>(EProcess.TransferRotationRight.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, CleanProcess>(EProcess.AFCleanLeft.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, CleanProcess>(EProcess.AFCleanRight.ToString());

                services.AddKeyedScoped<IProcess<ESequence>, UnloadTransferProcess>(EProcess.UnloadTransferLeft.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, UnloadTransferProcess>(EProcess.UnloadTransferRight.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, UnloadAlignProcess>(EProcess.UnloadAlign.ToString());
                services.AddKeyedScoped<IProcess<ESequence>, RobotUnloadProcess>(EProcess.RobotUnload.ToString());


                services.AddSingleton((ser) =>
                {
                    List<IProcess<ESequence>> processList = new List<IProcess<ESequence>>();

                    foreach (EProcess process in Enum.GetValues(typeof(EProcess)))
                    {
                        var proc = ser.GetKeyedService<IProcess<ESequence>>(process.ToString());
                        if (proc != null)
                        {
                            ((ProcessBase<ESequence>)proc).Name = process.ToString();
                            processList.Add(proc);
                        }
                    }
                    return new Processes(processList);
                });

                services.AddSingleton<VirtualIO<EFlags>>();
            });
            return hostBuilder;
        }
    }
}
