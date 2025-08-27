using EQX.Core.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class Processes
    {
        public IProcess<ESequence> Root { get; }
        public IProcess<ESequence> Head { get; }
        public IProcess<ESequence> StageLeft { get; }
        public IProcess<ESequence> StageRight { get; }

        public Processes(Devices devices)
        {
            Root = new RootProcess<ESequence, ESemiSequence>(devices) { Name = "ProcRoot" };
            Head = new HeadProcess() { Name = "ProcHead" };
            StageLeft = new StageProcess() { Name = "ProcStageL" };
            StageRight = new StageProcess() { Name = "ProcStageR" };
        }

        public void Initialize()
        {
            // Initialize the processes

            // Set the process hierarchy
            Root.AddChild(Head);
            Root.AddChild(StageLeft);
            Root.AddChild(StageRight);

            ProcessesStart();
        }

        private void ProcessesStart()
        {
            Root.Start();
            Root.Childs?.All(p => p.Start());
        }
    }
}
