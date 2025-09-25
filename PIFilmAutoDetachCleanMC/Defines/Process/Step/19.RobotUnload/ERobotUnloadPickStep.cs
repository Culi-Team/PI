using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotUnloadPickStep
    {
        Start,
        Wait_UnloadAlignRequestUnload,
        Robot_Move_PickPosition,
        Robot_Move_PickPosition_Wait,
        Cylinder_Down,
        Cylinder_Down_Wait,
        Vacuum_On,
        Vacuum_On_Wait,

        GlassDetect_Check,

        Set_FlagRobotPickDone,
        Wait_UnloadAlign_PickDoneReceived,

        Plasma_Prepare,

        Robot_Move_ReadyPlasmaPosition,
        Robot_Move_ReadyPlasmaPosition_Wait,
        End
    }
}
