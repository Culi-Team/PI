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

        Robot_Move_ReadyPickPosition,
        Robot_Move_ReadyPickPosition_Wait,

        Wait_UnloadAlignRequestUnload,

        Robot_Move_PickPosition,
        Robot_Move_PickPosition_Wait,

        Cylinder_Down,
        Cylinder_Down_Wait,
        Vacuum_On,
        Vacuum_On_Wait,

        GlassDetect_Check,

        Robot_MoveBack_ReadyPickPosition,
        Robot_MoveBack_ReadyPickPosition_Wait,

        Set_FlagRobotPickDone,
        Wait_UnloadAlign_PickDoneReceived,

        Plasma_Prepare,
        
        End
    }
}
