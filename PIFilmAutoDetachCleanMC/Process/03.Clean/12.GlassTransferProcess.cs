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
    public class GlassTransferProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

        private IMotion GlassTransferYAxis => _devices.MotionsInovance.GlassTransferYAxis;
        private IMotion GlassTransferZAxis => _devices.MotionsInovance.GlassTransferZAxis;

        public GlassTransferProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
