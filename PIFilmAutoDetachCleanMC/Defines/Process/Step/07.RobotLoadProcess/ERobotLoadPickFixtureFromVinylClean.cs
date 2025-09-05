using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadPickFixtureFromVinylClean
    {
        Start,
        Move_VinylCleanPickPosition,
        Move_VinylCleanPickPosition_Wait,

        Contact,
        Contact_Wait,

        Move_VinylClean_ReadyPosition,
        Move_VinylClean_ReadyPosition_Wait,
    }
}
