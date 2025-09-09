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

        YAxis_MovePickPosition,
        YAxis_MovePickPosition_Wait,
        ZAxis_MovePickPosition,
        ZAxis_MovePickPosition_Wait,

        Vacuum_On,

        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,
        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPosition_Wait,

        Set_FlagPickDone,
        End
    }
}
