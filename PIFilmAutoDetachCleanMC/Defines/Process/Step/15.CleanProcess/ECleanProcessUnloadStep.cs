using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessUnloadStep
    {
        Start,
        Wait_AFCleanLoadDone,
        AxisMoveUnloadPosition,
        AxisMoveUnloadPosition_Wait,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Vacuum_Off,
        
        Set_FlagRequestUnload,
        Wait_CleanUnloadDone,
        YAxis_MoveReadyPosition,
        YAxis_MoveReadyPosition_Wait,
        XTAxis_MoveReadyPosition,
        XTAxis_MoveReadyPosition_Wait,

        Brush_Up,
        Brush_Up_Wait,

        End
    }
}
