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
    public class RemoveFilmProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

        private ICylinder fixCyl1 => _devices.Cylinders.RemoveZoneFixCyl1FwBw;
        private ICylinder fixCyl2 => _devices.Cylinders.RemoveZoneFixCyl2FwBw;

        private ICylinder transferCyl => _devices.Cylinders.RemoveZoneTrCylFwBw;
        private ICylinder upDownCyl1 => _devices.Cylinders.RemoveZoneZCyl1UpDown;
        private ICylinder upDownCyl2 => _devices.Cylinders.RemoveZoneZCyl2UpDown;
        private ICylinder clampCyl => _devices.Cylinders.RemoveZoneCylClampUnclamp;

        private ICylinder pusherCyl1 => _devices.Cylinders.RemoveZonePusherCyl1UpDown;
        private ICylinder pusherCyl2 => _devices.Cylinders.RemoveZonePusherCyl2UpDown;
        public RemoveFilmProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
