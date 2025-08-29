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
    public class DetachProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

        private IMotion DetachGlassZAxis => _devices.MotionsInovance.DetachGlassZAxis;
        private IMotion ShuttleTransferXAxis => _devices.MotionsInovance.ShuttleTransferXAxis;
        private IMotion ShuttleTransferZAxis => _devices.MotionsAjin.ShuttleTransferZAxis;

        private ICylinder DetachFixFixtureCyl1 => _devices.Cylinders.DetachFixFixtureCyl1FwBw;
        private ICylinder DetachFixFixtureCyl2 => _devices.Cylinders.DetachFixFixtureCyl2FwBw;

        private ICylinder DetachCyl1 => _devices.Cylinders.DetachCyl1UpDown;
        private ICylinder DetachCyl2 => _devices.Cylinders.DetachCyl2UpDown;

        private bool isFixtureDetect => _devices.Inputs.DetachFixtureDetect.Value;
        
        private bool isGlassShuttleVac1 => _devices.Inputs.DetachGlassShtVac1.Value;
        private bool isGlassShuttleVac2 => _devices.Inputs.DetachGlassShtVac2.Value;
        private bool isGlassShuttleVac3 => _devices.Inputs.DetachGlassShtVac3.Value;

        private IDOutput glassShuttleVac1 => _devices.Outputs.DetachGlassShtVac1OnOff;
        private IDOutput glassShuttleVac2 => _devices.Outputs.DetachGlassShtVac2OnOff;
        private IDOutput glassShuttleVac3 => _devices.Outputs.DetachGlassShtVac3OnOff;

        private void GlassShuttleVacOnOff(bool onOff)
        {
            glassShuttleVac1.Value = onOff;
            glassShuttleVac2.Value = onOff;
            glassShuttleVac3.Value = onOff;
        }

        public DetachProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
