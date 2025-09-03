using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EFixtureAlignRobotPlaceFixtureToAlignStep
    {
        Start,
        SetFlagRequestFixture,
        WaitFixtureAlignLoadDone,
        FixtureDetectCheck,
        End
    }
}
