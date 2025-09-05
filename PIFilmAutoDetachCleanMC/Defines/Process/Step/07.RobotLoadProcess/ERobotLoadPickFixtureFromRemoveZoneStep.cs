using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadPickFixtureFromRemoveZoneStep
    {
        Start,
        Move_RemoveZonePickPosition,
        Move_RemoveZonePickPosition_Wait,

        Contact,
        Contact_Wait,

        Move_RemoveZoneReadyPosition,
        Move_RemoveZoneReadyPosition_Wait,

        Set_FlagRemoveZoneUnloadDone,
        End
    }
}
