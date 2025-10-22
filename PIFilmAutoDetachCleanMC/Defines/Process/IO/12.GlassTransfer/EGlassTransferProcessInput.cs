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

        TRANSFER_IN_SHUTTLE_LEFT_GLASS_REQUEST,
        TRANSFER_IN_SHUTTLE_RIGHT_GLASS_REQUEST,

        TRANSFER_IN_SHUTTLE_L_ORIGIN_DONE,
        TRANSFER_IN_SHUTTLE_R_ORIGIN_DONE,
    }
}
