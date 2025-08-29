using EQX.Core.Motion;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class TransferInShuttleProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;
        private EPort port => Name == EProcess.TransferInShuttleLeft.ToString() ? EPort.Left : EPort.Right;

        private IMotion TransferInShuttleYAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLYAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRYAxis;

        private IMotion TransferInShuttleZAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLZAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRZAxis;

        public TransferInShuttleProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
