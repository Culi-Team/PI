using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERootProcessToRunStep
    {
        Start,
        DoorSensorCheck,
        DoorLock,
        DoorLatchCheck,
        ChildsToRunDone_Wait,
        End
    }
}
