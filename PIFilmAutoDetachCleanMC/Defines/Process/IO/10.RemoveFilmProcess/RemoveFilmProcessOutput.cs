using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmProcessOutput
    {
        /// <summary>
        /// Robot unload request
        /// </summary>
        REMOVE_FILM_REQ_UNLOAD,

        REMOVE_FILM_LOAD_READY,

        REMOVE_FILM_ORIGIN_DONE,
        REMOVE_FILM_READY_DONE,

        REMOVE_FILM_UNCLAMP_FIXTURE_DONE,
    }
}
