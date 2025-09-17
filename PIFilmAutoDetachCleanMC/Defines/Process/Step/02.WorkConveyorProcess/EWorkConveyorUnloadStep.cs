using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EWorkConveyorUnloadStep
    {
        Start,
        Cyl_Tilt_Down,
        Cyl_Tilt_Down_Wait,

        Support_CV_Down,
        Support_CV_Down_Wait,

        TAxis_MoveLoadPosition,
        TAxis_MoveLoadPosition_Wait,

        Support_CV_Up,
        Support_CV_Up_Wait,

        Cyl_Fix_Backward,
        Cyl_Fix_Backward_Wait,

        Set_FlagRequestCSTOut,
        Wait_NextConveyorReady,

        Muting_LightCurtain,

        Conveyor_Run,

        Wait_CSTOut_Done,

        End
    }
}
