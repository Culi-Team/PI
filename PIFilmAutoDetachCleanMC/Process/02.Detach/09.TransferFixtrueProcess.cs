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
    public class TransferFixtrueProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;
        private IMotion TransferFixtureYAxis => _devices.MotionsInovance.FixtureTransferYAxis;
        private Inputs Inputs => _devices.Inputs;
        private Outputs Outputs => _devices.Outputs;
        private ICylinder TransferFixtureUpDown => _devices.Cylinders.TransferFixtureUpDown;
        private ICylinder TransferFixtureClamp1 => _devices.Cylinders.TransferFixture1ClampUnclamp;
        private ICylinder TransferFixtureClamp2 => _devices.Cylinders.TransferFixture2ClampUnclamp;

        public TransferFixtrueProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
