using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EUnloadAlignProcessOutput
    {
        UNLOAD_ALIGN_READY,
        UNLOAD_ALIGN_PLACE_DONE_RECEIVED,

        UNLOAD_ALIGN_REQ_ROBOT_UNLOAD,
        ROBOT_UNLOAD_DONE_RECEIVED,
    }
}
