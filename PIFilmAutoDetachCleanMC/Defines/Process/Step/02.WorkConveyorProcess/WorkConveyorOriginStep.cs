using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EWorkConveyorOriginStep
    {
        Start,
        Wait_RobotLoadOriginDone,
        Cyl_Tilt_Down,
        Cyl_Tilt_Down_Wait,
        SupportCV_Down,
        SupportCV_Down_Wait,
        TAxis_Origin,
        TAxis_Origin_Wait,
        Cyl_Fix_Backward,
        Cyl_Fix_Backward_Wait,
        Conveyor_Stop,
        End
    }
}
