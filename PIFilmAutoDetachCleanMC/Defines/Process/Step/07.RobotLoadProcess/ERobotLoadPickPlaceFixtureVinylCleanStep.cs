using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadPickPlaceFixtureVinylCleanStep
    {
        Start,
        Move_VinylClean_PickPlacePosition,
        Move_VinylClean_PickPlacePosition_Wait,

        CylConntact,
        CylConntact_Wait,

        Cyl_UnContact,
        Cyl_UnContact_Wait,

        Move_VinylClean_ReadyPosition,
        Move_VinylClean_ReadyPosition_Wait,

        SetFlag_VinylCleanLoadUnloadDone,
        Wait_VinylCleanReceiveLoadUnloadDone,

        End,
        Wait_NextSequence,
    }
}
