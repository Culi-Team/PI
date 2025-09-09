using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassTransferPickStep
    {
        Start,
        Wait_DetachRequestUnload,

        Cyl_Down,
        Cyl_Down_Wait,
        YAxis_MovePickPosition,
        YAxis_MovePickPosition_Wait,
        ZAxis_MovePickPosition,
        ZAxis_MovePickPosition_Wait,

        Vacuum_On,

        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,
        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPosition_Wait,

        Cyl_Up,
        Cyl_Up_Wait,

        Set_FlagPickDone,
        Wait_DetachGlassTransferPickDone_Received,
        End
    }
}
