using EQX.Core.Process;
using PIFilmAutoDetachCleanMC.Defines;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class Processes
    {
        private readonly List<IProcess<ESequence>> _processes;

        public IProcess<ESequence> RootProcess => _processes.First(p => p.Name == EProcess.Root.ToString());
        public IProcess<ESequence> InConveyorProcess => _processes.First(p => p.Name == EProcess.InConveyor.ToString());
        public IProcess<ESequence> InWorkConveyorProcess => _processes.First(p => p.Name == EProcess.InWorkConveyor.ToString());
        public IProcess<ESequence> BufferConveyorProcess => _processes.First(p => p.Name == EProcess.BufferConveyor.ToString());
        public IProcess<ESequence> OutWorkConveyorProcess => _processes.First(p => p.Name == EProcess.OutWorkConveyor.ToString());
        public IProcess<ESequence> OutConveyorProcess => _processes.First(p => p.Name == EProcess.OutConveyor.ToString());

        public IProcess<ESequence> RobotLoadProcess => _processes.First(p => p.Name == EProcess.RobotLoad.ToString());
        public IProcess<ESequence> VinylCleanProcess => _processes.First(p => p.Name == EProcess.VinylClean.ToString());
        public IProcess<ESequence> FixtureAlignProcess => _processes.First(p => p.Name == EProcess.FixtureAlign.ToString());
        public IProcess<ESequence> TransferFixtureProcess => _processes.First(p => p.Name == EProcess.TransferFixture.ToString());
        public IProcess<ESequence> DetachProcess => _processes.First(p => p.Name == EProcess.Detach.ToString());
        public IProcess<ESequence> RemoveFilmProcess => _processes.First(p => p.Name == EProcess.RemoveFilm.ToString());

        public IProcess<ESequence> GlassTransferProcess => _processes.First(p => p.Name == EProcess.GlassTransfer.ToString());
        public IProcess<ESequence> GlassAlignLeftProcess => _processes.First(p => p.Name == EProcess.GlassAlignLeft.ToString());
        public IProcess<ESequence> GlassAlignRightProcess => _processes.First(p => p.Name == EProcess.GlassAlignRight.ToString());
        public IProcess<ESequence> TransferInShuttleLeftProcess => _processes.First(p => p.Name == EProcess.TransferInShuttleLeft.ToString());
        public IProcess<ESequence> TransferInShuttleRightProcess => _processes.First(p => p.Name == EProcess.TransferInShuttleRight.ToString());
        public IProcess<ESequence> WETCleanLeftProcess => _processes.First(p => p.Name == EProcess.WETCleanLeft.ToString());
        public IProcess<ESequence> WETCleanRightProcess => _processes.First(p => p.Name == EProcess.WETCleanRight.ToString());
        public IProcess<ESequence> TransferRotationLeftProcess => _processes.First(p => p.Name == EProcess.TransferRotationLeft.ToString());
        public IProcess<ESequence> TransferRotationRightProcess => _processes.First(p => p.Name == EProcess.TransferRotationRight.ToString());
        public IProcess<ESequence> AFCleanLeftProcess => _processes.First(p => p.Name == EProcess.AFCleanLeft.ToString());
        public IProcess<ESequence> AFCleanRightProcess => _processes.First(p => p.Name == EProcess.AFCleanRight.ToString());

        public IProcess<ESequence> UnloadTransferLeftProcess => _processes.First(p => p.Name == EProcess.UnloadTransferLeft.ToString());
        public IProcess<ESequence> UnloadTransferRightProcess => _processes.First(p => p.Name == EProcess.UnloadTransferRight.ToString());
        public IProcess<ESequence> UnloadAlignProcess => _processes.First(p => p.Name == EProcess.UnloadAlign.ToString());
        public IProcess<ESequence> RobotUnloadProcess => _processes.First(p => p.Name == EProcess.RobotUnload.ToString());

        public Processes(List<IProcess<ESequence>> processes)
        {
            _processes = processes;
        }

        public void Initialize()
        {
            // Initialize the processes

            // Set the process hierarchy
            RootProcess.AddChild(InConveyorProcess);
            RootProcess.AddChild(InWorkConveyorProcess);
            RootProcess.AddChild(BufferConveyorProcess);
            RootProcess.AddChild(OutWorkConveyorProcess);
            RootProcess.AddChild(OutConveyorProcess);

            RootProcess.AddChild(RobotLoadProcess);
            RootProcess.AddChild(VinylCleanProcess);
            RootProcess.AddChild(FixtureAlignProcess);
            RootProcess.AddChild(TransferFixtureProcess);
            RootProcess.AddChild(DetachProcess);
            RootProcess.AddChild(RemoveFilmProcess);

            RootProcess.AddChild(GlassTransferProcess);
            RootProcess.AddChild(GlassAlignLeftProcess);
            RootProcess.AddChild(GlassAlignRightProcess);
            RootProcess.AddChild(TransferInShuttleLeftProcess);
            RootProcess.AddChild(TransferInShuttleRightProcess);
            RootProcess.AddChild(WETCleanLeftProcess);
            RootProcess.AddChild(WETCleanRightProcess);
            RootProcess.AddChild(TransferRotationLeftProcess);
            RootProcess.AddChild(TransferRotationRightProcess);
            RootProcess.AddChild(AFCleanLeftProcess);
            RootProcess.AddChild(AFCleanRightProcess);

            RootProcess.AddChild(UnloadTransferLeftProcess);
            RootProcess.AddChild(UnloadTransferRightProcess);
            RootProcess.AddChild(UnloadAlignProcess);
            RootProcess.AddChild(RobotUnloadProcess);


            ProcessesStart();
        }

        private void ProcessesStart()
        {
            RootProcess.Start();
            RootProcess.Childs?.All(p => p.Start());
        }
    }
}
