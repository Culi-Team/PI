using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EWorkConveyorTiltStep
    {
        Start,
        Cyl_SupportCV_Down,
        Cyl_SupportCV_Down_Wait,
        Cyl_Fix_Forward,
        Cyl_Fix_Forward_Wait,
        TAxis_Move_WorkPosition,
        TAxis_Move_WorkPosition_Wait,
        Cyl_Tilt_Up,
        Cyl_Tilt_Up_Wait,
        End
    }
}
