using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachUnloadStep
    {
        Start,
        ShuttleZAxis_Move_ReadyPosition,
        ShuttleZAxis_Move_ReadyPosition_Wait,
        XAxis_Move_DetachCheck_Position,
        XAxis_Move_DetachCheck_Position_Wait,
        Vacuum_Check,
        XAxis_Move_UnloadPosition,
        XAxis_Move_UnloadPosition_Wait,
        ShuttleZAxis_Move_UnloadPosition,
        ShuttleZAxis_Move_UnloadPosition_Wait,
        Vacuum_Off,
        SetFlagDetachRequestUnload,
        End
    }
}
