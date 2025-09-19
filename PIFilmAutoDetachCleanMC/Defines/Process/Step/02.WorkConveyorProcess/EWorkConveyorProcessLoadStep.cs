using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EWorkConveyorProcessLoadStep
    {
        Start,

        Cyl_Tilt_Down,
        Cyl_Tilt_Down_Wait,

        TAxis_Move_LoadPosition,
        TAxis_Move_MoveLoadPosition_Wait,

        Cyl_Fix_Backward,
        Cyl_Fix_Backward_Wait,

        Support_CV_Up,
        Support_CV_Up_Wait,

        Conveyor_Run,

        Set_FlagRequestCassetteIn,

        Wait_CassetteLoadDone,

        Reset_Cassette_Status,

        Conveyor_Stop,

        End
    }
}
