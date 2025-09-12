using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EUnloadAlignRobotPickStep
    {
        Start,
        Vacuum_Off,

        Set_FlagRequestRobotUnload,
        Wait_RobotUnloadDone,
        End
    }
}
