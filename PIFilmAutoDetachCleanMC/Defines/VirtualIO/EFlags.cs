using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.VirtualIO
{
    public enum EFlags
    {
        DetachProcessOriginDone,

        FixtureAlignRequestFixture,
        FixtureAlignLoadDone,
        FixtureAlignDone,

        FixtureTransferAlignDone,

        FixtureTransferDetachDone,

        FixtureTransferRemoveFilmDone,

        DetachDone,

        RemoveFilmDone,
        RemoveFilmRequestUnload,

        RemoveFilmUnloadDone,

        GlassTransferPickDone,
    }
}
