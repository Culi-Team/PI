using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadProcessInput
    {
        IN_CST_READY,
        VINYL_CLEAN_REQ_LOAD,

        OUT_CST_READY,
        VINYL_CLEAN_REQ_UNLOAD,

        FIXTURE_ALIGN_REQ_LOAD,
        REMOVE_FILM_REQ_UNLOAD
    }
}
