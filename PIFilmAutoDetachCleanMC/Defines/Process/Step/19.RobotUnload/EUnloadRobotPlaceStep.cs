using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EUnloadRobotPlaceStep
    {
        Start,
        Wait_MachineRequestPlace,

        Robot_Move_PlacePosition,
        Robot_Move_PlacePosition_Wait,

        VacuumOff,
        End
    }
}
