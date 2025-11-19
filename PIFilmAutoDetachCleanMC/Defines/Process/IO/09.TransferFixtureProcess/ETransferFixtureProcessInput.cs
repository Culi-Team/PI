using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferFixtureProcessInput
    {
        FIXTURE_ALIGN_DONE,
        READY_TO_TRANSFER,
        REMOVE_FILM_LOAD_READY,
        DETACH_ORIGIN_DONE,
        DETACH_READY_DONE,

        REMOVE_FILM_ORIGIN_DONE,
        REMOVE_FILM_READY_DONE,

        ALIGN_FIXTURE_UNCLAMP_DONE,
    }
}
