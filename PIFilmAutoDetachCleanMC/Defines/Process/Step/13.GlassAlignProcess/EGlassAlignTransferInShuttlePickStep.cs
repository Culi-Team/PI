using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassAlignTransferInShuttlePickStep
    {
        Start,
        Vacuum_Off,
        Glass_Detect_Check,
        Set_FlagGlassAlignRequestPick,
        Wait_TransferInShuttlePickDone,
        End

    }
}
