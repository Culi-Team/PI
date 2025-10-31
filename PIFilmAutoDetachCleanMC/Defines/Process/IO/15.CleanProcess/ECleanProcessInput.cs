using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessInput
    {
        LOAD_DONE,
        UNLOAD_DONE,
        TRANSFER_ROTATION_READY_PICK_PLACE,

        TRANSFER_IN_SHUTTLE_IN_SAFE_POS,

        AF_CLEAN_CLEANING,

        AF_CLEAN_DISABLE,
    }
}
