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
    public class DetachProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

        private IMotion DetachGlassZAxis => _devices.MotionsInovance.DetachGlassZAxis;
        private IMotion ShuttleTransferXAxis => _devices.MotionsInovance.ShuttleTransferXAxis;
        private IMotion ShuttleTransferZAxis => _devices.MotionsAjin.ShuttleTransferZAxis;

        public DetachProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
