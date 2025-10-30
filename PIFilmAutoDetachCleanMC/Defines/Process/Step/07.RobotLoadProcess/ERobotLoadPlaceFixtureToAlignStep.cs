using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadPlaceFixtureToAlignStep
    {
        Start,
        Wait_FixtureAlignRequestFixture,

        Move_FixtureAlignPlacePosition,
        Move_FixtureAlignPlacePosition_Wait,

        UnClamp,
        UnClamp_Wait,
        UnAlign,
        UnAlign_Wait,

        Move_FixtureAlignReadyPosition,
        Move_FixtureAlignReadyPosition_Wait,

        Set_FlagFixtureAlignLoadDone,
        Wait_FixtureAlignLoadDoneReceived,

        End,
        Wait_NextSequence
    }
}
