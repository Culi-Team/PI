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
    public class UnloadTransferProcess : ProcessBase<ESequence>
    {
        private EPort port => Name == EProcess.UnloadTransferLeft.ToString() ? EPort.Left : EPort.Right;
        private readonly Devices _devices;

        private IMotion yAxis => port == EPort.Left ? _devices.MotionsInovance.GlassUnloadLYAxis :
                                                  _devices.MotionsInovance.GlassUnloadRYAxis;

        private IMotion zAxis => port == EPort.Left ? _devices.MotionsInovance.GlassUnloadLZAxis :
                                                  _devices.MotionsInovance.GlassUnloadRZAxis;

        private IDOutput glassVac => port == EPort.Left ? _devices.Outputs.UnloadTransferLVacOnOff:
                                                  _devices.Outputs.UnloadTransferRVacOnOff;

        private bool isVacDetect => port == EPort.Left ? _devices.Inputs.UnloadTransferLVac.Value :
                                                         _devices.Inputs.UnloadTransferRVac.Value;
        public UnloadTransferProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
