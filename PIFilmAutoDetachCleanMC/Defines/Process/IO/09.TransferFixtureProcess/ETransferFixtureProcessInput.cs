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
        DETACH_DONE,
        REMOVE_FILM_DONE,
        DETACH_ORIGIN_DONE,
        DETACH_READY_DONE,

        REMOVE_FILM_ORIGIN_DONE,
        REMOVE_FILM_READY_DONE,

        ALIGN_FIXTURE_UNCLAMP_DONE,
        DETACH_FIXTURE_UNCLAMP_DONE,
    }
}
