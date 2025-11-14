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

        Cylinder_Up,
        Cylinder_Up_Wait,

        Robot_Move_PP_UP_Position,
        Robot_Move_PP_UP_Position_Wait,

        CheckOutputStopValue,

        Wait_MachineRequestPlace,

        Robot_Move_Place_Position,
        Robot_Move_Place_Position_Wait,

        VacuumOff,

        Robot_Move_ReadyPlacePosition,
        Robot_Move_ReadyPlacePosition_Wait,

        End
    }
}
