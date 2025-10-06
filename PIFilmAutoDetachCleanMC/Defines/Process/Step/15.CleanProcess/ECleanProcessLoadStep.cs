using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessLoadStep
    {
        Start,
        Wait_WETCleanUnloadDone,
        AxisMoveLoadPosition,
        AxisMoveLoadPosition_Wait,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Set_FlagCleanRequestLoad,
        Wait_CleanLoadDone,
        Set_FlagCleanLoadDoneReceived,
        Vacuum_On,
        Vacuum_Check,
        Cyl_Clamp,
        Cyl_Clamp_Wait,
        End
    }
}
