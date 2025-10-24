using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessCleanShuttleStep
    {
        Start,
        Cyl_Up,
        Cyl_Up_Wait,

        Clamp_Cyl_Unclamp,
        Clamp_Cyl_Unclamp_Wait,

        YAxis_MoveReadyPosition,
        YAxis_MoveReadyPosition_Wait,

        XTAxis_MoveCleanShuttlePosition,
        XTAxis_MoveCleanShuttlePosition_Wait,

        YAxis_MoveCleanShuttlePosition,
        YAxis_MoveCleanShuttlePosition_Wait,

        Brush_Cyl_Down,
        Brush_Cyl_Down_Wait,

        CleanShuttle,
        CleanShuttle_Wait,

        End
    }
}
