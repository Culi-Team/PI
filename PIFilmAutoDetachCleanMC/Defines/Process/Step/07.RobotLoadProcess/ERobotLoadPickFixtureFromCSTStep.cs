using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadPickFixtureFromCSTStep
    {
        Start,
        Wait_InCST_Ready,
        Move_InCST_PickPositon,
        Move_InCST_PickPosition_Wait,

        Cyl_Clamp,
        Cyl_Clamp_Wait,
        Cyl_Align,
        Cyl_Align_Wait,

        Move_InCST_ReadyPositon,
        Move_InCST_ReadyPositon_Wait,

        Set_Flag_RobotPickInCSTDone,

        End
    }
}
