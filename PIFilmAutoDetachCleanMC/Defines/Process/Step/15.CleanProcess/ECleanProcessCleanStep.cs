using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessCleanStep
    {
        Start,

        Wait_WETCleanUnloadDone,

        Cyl_Clamp,
        Cyl_Clamp_Wait,

        Vacuum_On,
        Vacuum_On_Wait,

        Axis_MoveCleanHorizontalPosition,
        Axis_MoveCleanHorizontalPosition_Wait,

        TAxis_Move_CleanHorizontalPosition,
        TAxis_Move_CleanHorizontalPosition_Wait,

        Wait_3M_PrepareDone,

        CylPusher_Down_CleanHorizontal,
        CylPusher_Down_CleanHorizontal_Wait,

        CleanHorizontal,
        CleanHorizontal_Wait,

        CylPusher_Up_CleanHorizontal,
        CylPusherUp_CleanHorizontal_Wait,

        Axis_Move_CleanVerticalPosition,
        Axis_Move_CleanVerticalPosition_Wait,

        CylPusher_Down_CleanVertical,
        CylPusher_Down_CleanVertical_Wait,

        CleanVertical,
        CleanVertical_Wait,

        CylPusher_Up_CleanVertical,
        CylPusher_Up_CleanVertical_Wait,

        XAxis_Move_ReadyPosition,
        XAxis_Move_ReadyPosition_Wait,
        End
    }
}
