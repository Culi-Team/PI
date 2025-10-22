using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassTransferOriginStep
    {
        Start,
        Wait_TransferInShuttleOriginDone,

        Cyl_Up,
        Cyl_Up_Wait,
        ZAxis_Origin,
        ZAxis_Origin_Wait,
        YAxis_Origin,
        YAxis_Origin_Wait,

        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPositionWait,
        End
    }
}
