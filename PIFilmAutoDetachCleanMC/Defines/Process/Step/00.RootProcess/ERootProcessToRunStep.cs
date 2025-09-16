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
        AutoMode_Check,
        DoorSensorCheck,
        DoorLock,
        DoorLatchCheck,
        ChildsToRunDone_Wait,
        End
    }
}
