using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERootProcessToOriginStep
    {
        Start,
        Muting_Off,
        DoorSensorCheck,
        DoorLock,
        DoorLatchCheck,
        ChildsToOriginDoneWait,
        End
    }
}
