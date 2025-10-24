using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferInShuttleProcessInput
    {
        GLASS_TRANSFER_PLACE_DONE,

        WET_CLEAN_REQ_LOAD,

        /// <summary>
        /// Transfer In Shuttle current in safety Position, Glass Transfer is safe to move
        /// </summary>
        TRANSFER_IN_SAFETY_POS,
    }
}
