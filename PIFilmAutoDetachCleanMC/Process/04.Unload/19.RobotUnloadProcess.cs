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
    public class RobotUnloadProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

        private IDOutput vac1 => _devices.Outputs.UnloadRobotVac1OnOff;
        private IDOutput vac2 => _devices.Outputs.UnloadRobotVac2OnOff;
        private IDOutput vac3 => _devices.Outputs.UnloadRobotVac3OnOff;
        private IDOutput vac4 => _devices.Outputs.UnloadRobotVac4OnOff;

        private ICylinder cyl1 => _devices.Cylinders.UnloadRobotCyl1UpDown;
        private ICylinder cyl2 => _devices.Cylinders.UnloadRobotCyl2UpDown;
        private ICylinder cyl3 => _devices.Cylinders.UnloadRobotCyl3UpDown;
        private ICylinder cyl4 => _devices.Cylinders.UnloadRobotCyl4UpDown;

        private bool glassDetect1 => _devices.Inputs.UnloadRobotDetect1.Value;
        private bool glassDetect2 => _devices.Inputs.UnloadRobotDetect2.Value;
        private bool glassDetect3 => _devices.Inputs.UnloadRobotDetect3.Value;
        private bool glassDetect4 => _devices.Inputs.UnloadRobotDetect4.Value;

        public RobotUnloadProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
