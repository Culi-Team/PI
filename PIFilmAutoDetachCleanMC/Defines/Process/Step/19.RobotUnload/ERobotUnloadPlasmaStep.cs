using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotUnloadPlasmaStep
    {
        Start,
        Wait_PlasmaPrepareDone,
        Robot_Move_PlasmaPosition,
        Robot_Move_PlasmaPosition_Wait,

        End
    }
}
