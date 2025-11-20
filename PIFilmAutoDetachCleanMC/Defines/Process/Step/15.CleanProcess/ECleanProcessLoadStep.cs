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

        Cyl_Brush_Down,
        Cyl_Brush_Down_Wait,

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

        Cyl_Brush_Up,
        Cyl_Brush_Up_Wait,

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
