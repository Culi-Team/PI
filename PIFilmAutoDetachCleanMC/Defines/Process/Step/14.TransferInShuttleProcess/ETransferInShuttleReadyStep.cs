using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferInShuttleReadyStep
    {
        Start,

        SafetyPosition_Check,

        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,

        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPosition_Wait,

        Flag_InSafePosition_Set,

        End
    }
}
