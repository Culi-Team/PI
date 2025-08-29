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
    public class TransferRotationProcess : ProcessBase<ESequence>
    {
        private EPort port => Name == EProcess.TransferRotationLeft.ToString() ? EPort.Left : EPort.Right;

        private readonly Devices _devices;

        private IMotion zAxis => port == EPort.Left ? _devices.MotionsInovance.TransferRotationLZAxis :
                                                      _devices.MotionsInovance.TransferRotationRZAxis;

        private ICylinder clampCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftClampUnclamp :
                                                      _devices.Cylinders.TrRotateRightClampUnclamp;

        private ICylinder rotateCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftRotate :
                                                      _devices.Cylinders.TrRotateRightRotate;

        private ICylinder transferCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftFwBw :
                                                      _devices.Cylinders.TrRotateRightFwBw;

        public IDOutput glassVac => port == EPort.Left ? _devices.Outputs.TrRotateLeftVacOnOff :
                                                      _devices.Outputs.TrRotateRightVacOnOff;

        private bool isVacDetect => port == EPort.Left ? _devices.Inputs.TrRotateLeftVac.Value :
                                                         _devices.Inputs.TrRotateRightVac.Value;
        public TransferRotationProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
