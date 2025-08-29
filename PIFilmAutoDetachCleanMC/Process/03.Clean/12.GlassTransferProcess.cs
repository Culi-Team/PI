using EQX.Core.InOut;
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

        private IDOutput glassVac1 => _devices.Outputs.GlassTransferVac1OnOff;
        private IDOutput glassVac2 => _devices.Outputs.GlassTransferVac2OnOff;
        private IDOutput glassVac3 => _devices.Outputs.GlassTransferVac3OnOff;

        private bool isVac1Detect => _devices.Inputs.GlassTransferVac1.Value;
        private bool isVac2Detect => _devices.Inputs.GlassTransferVac2.Value;
        private bool isVac3Detect => _devices.Inputs.GlassTransferVac3.Value;

        public GlassTransferProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
