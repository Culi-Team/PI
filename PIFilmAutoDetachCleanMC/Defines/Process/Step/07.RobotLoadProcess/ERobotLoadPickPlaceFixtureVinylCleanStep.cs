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

        CylAlign,
        CylAlign_Wait,
        CylClamp,
        CylClamp_Delay,
        CylClamp_Wait,
        SetFlag_RobotMoveVinylCleanDone_Unload,
        Wait_VinylCleanUnClampDone,

        SetFlag_RobotMoveVinylCleanDone_Load,
        Wait_VinylCleanClampDone,
        CylUnClamp,
        CylUnClamp_Wait,
        CylUnAlign,
        CylUnAlign_Wait,

        SetFlag_VinylCleanLoadDone,

        Move_VinylClean_ReadyPosition,
        Move_VinylClean_ReadyPosition_Wait,

        SetFlag_VinylCleanUnloadDone,
        Wait_VinylCleanReceiveLoadUnloadDone,

        End,
        Wait_NextSequence,
    }
}
