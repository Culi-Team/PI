using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassTransferReadyStep
    {
        Start,

        TransInShuttle_SafePos_Wait,

        ZAxis_MoveReady_CylUp,
        ZAxis_MoveReady_CylUp_Wait,
        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPosition_Wait,
        End
    }
}
