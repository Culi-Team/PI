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

        private ICylinder FixCyl1 => _devices.Cylinders.RemoveZoneFixCyl1FwBw;
        private ICylinder FixCyl2 => _devices.Cylinders.RemoveZoneFixCyl2FwBw;

        private ICylinder TransferCyl => _devices.Cylinders.RemoveZoneTrCylFwBw;
        private ICylinder UpDownCyl1 => _devices.Cylinders.RemoveZoneZCyl1UpDown;
        private ICylinder UpDownCyl2 => _devices.Cylinders.RemoveZoneZCyl2UpDown;
        private ICylinder ClampCyl => _devices.Cylinders.RemoveZoneCylClampUnclamp;

        private ICylinder PusherCyl1 => _devices.Cylinders.RemoveZonePusherCyl1UpDown;
        private ICylinder PusherCyl2 => _devices.Cylinders.RemoveZonePusherCyl2UpDown;
        public RemoveFilmProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
