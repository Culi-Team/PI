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

        XYTAxisMoveLoadPosition,
        XYTAxisMoveLoadPosition_Wait,

        TAxisMoveLoadPosition,
        TAxisMoveLoadPosition_Wait,
        XYAxisMoveLoadPosition,
        XYAxisMoveLoadPosition_Wait,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Set_FlagCleanRequestLoad,
        Wait_CleanLoadDone,

        Cyl_Clamp,
        Cyl_Clamp_Wait,
        Vacuum_On,
        Vacuum_Check,

        Axis_MoveReadyPosition,
        Axis_MoveReadyPosition_Wait,
        End
    }
}
