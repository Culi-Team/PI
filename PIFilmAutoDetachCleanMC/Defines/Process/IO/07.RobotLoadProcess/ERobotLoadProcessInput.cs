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
        IN_CST_PICK_DONE,
        VINYL_CLEAN_REQ_LOAD,
        VINYL_CLEAN_RECEIVE_LOAD_DONE,

        OUT_CST_READY,
        OUT_CST_PLACE_DONE,
        VINYL_CLEAN_REQ_UNLOAD,
        VINYL_CLEAN_RECEIVE_UNLOAD_DONE,

        FIXTURE_ALIGN_REQ_LOAD,
        REMOVE_FILM_REQ_UNLOAD
    }
}
