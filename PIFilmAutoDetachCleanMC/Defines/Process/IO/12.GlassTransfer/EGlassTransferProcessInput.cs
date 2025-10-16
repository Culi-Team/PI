using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassTransferProcessInput
    {
        DETACH_REQ_UNLOAD_GLASS,
        GLASS_ALIGN_LEFT_REQ_GLASS,
        GLASS_ALIGN_RIGHT_REQ_GLASS,
        GLASS_TRANSFER_PICK_DONE_RECEIVED,
        GLASS_ALIGN_LEFT_PLACE_DONE_RECEIVED,
        GLASS_ALIGN_RIGHT_PLACE_DONE_RECEIVED,

        TRANSFER_IN_SHUTTLE_L_ORIGIN_DONE,
        TRANSFER_IN_SHUTTLE_R_ORIGIN_DONE,
    }
}
