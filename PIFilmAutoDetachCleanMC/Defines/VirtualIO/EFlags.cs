using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EFlags
    {
        DetachProcessOriginDone,

        InCSTReady,
        OutCSTReady,

        RobotPickInCSTDone,
        RobotPlaceOutCSTDone,

        VinylCleanRequestFixture,
        VinylCleanLoadDone,
        VinylCleanRequestUnload,
        VinylCleanUnloadDone,

        FixtureAlignRequestFixture,
        FixtureAlignLoadDone,
        FixtureAlignDone,

        FixtureTransferAlignDone,

        FixtureTransferDetachDone,

        FixtureTransferRemoveFilmDone,

        DetachDone,
        DetachRequestUnloadGlass,

        RemoveFilmDone,
        RemoveFilmRequestUnload,

        RemoveFilmUnloadDone,

        GlassTransferPickDone,
    }
}
