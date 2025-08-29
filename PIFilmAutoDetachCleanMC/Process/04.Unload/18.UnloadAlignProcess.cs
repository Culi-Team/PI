using EQX.Core.InOut;
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
    public class UnloadAlignProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

        private ICylinder alignCyl1 => _devices.Cylinders.UnloadAlignCyl1UpDown;
        private ICylinder alignCyl2 => _devices.Cylinders.UnloadAlignCyl2UpDown;
        private ICylinder alignCyl3 => _devices.Cylinders.UnloadAlignCyl3UpDown;
        private ICylinder alignCyl4 => _devices.Cylinders.UnloadAlignCyl4UpDown;

        private IDOutput alignVac1 => _devices.Outputs.UnloadGlassAlignVac1OnOff;
        private IDOutput alignVac2 => _devices.Outputs.UnloadGlassAlignVac2OnOff;
        private IDOutput alignVac3 => _devices.Outputs.UnloadGlassAlignVac3OnOff;
        private IDOutput alignVac4 => _devices.Outputs.UnloadGlassAlignVac4OnOff;

        private bool isGlassVac1 => _devices.Inputs.UnloadGlassAlignVac1.Value;
        private bool isGlassVac2 => _devices.Inputs.UnloadGlassAlignVac2.Value;
        private bool isGlassVac3 => _devices.Inputs.UnloadGlassAlignVac3.Value;
        private bool isGlassVac4 => _devices.Inputs.UnloadGlassAlignVac4.Value;

        private bool isGlassDetect1 => _devices.Inputs.UnloadGlassDetect1.Value;
        private bool isGlassDetect2 => _devices.Inputs.UnloadGlassDetect2.Value;
        private bool isGlassDetect3 => _devices.Inputs.UnloadGlassDetect3.Value;
        private bool isGlassDetect4 => _devices.Inputs.UnloadGlassDetect4.Value;
        public UnloadAlignProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
