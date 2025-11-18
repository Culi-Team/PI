using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachProcessGlassTransferPickStep
    {
        Start,
        Vacuum_Off,
        Set_FlagDetachRequestUnload,
        Wait_GlassTransferPickDone,

        ShuttleZAxis_Move_ReadyPosition,
        ShuttleZAxis_Move_ReadyPosition_Wait,
        ShuttleXAxis_Move_DetachPosition,
        ShuttleXAxis_Move_DetachPosition_Wait,
        End
    }
}
