using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadPlaceFixtureToOutCSTStep
    {
        Start,
        Wait_OutCSTReady,

        Move_OutCSTPlacePosition,
        Move_OutCSTPlacePosition_Wait,

        UnContact,
        UnContact_Wait,

        Move_OutCSTReadyPosition,
        Move_OutCSTReadyPosition_Wait,

        Set_FlagPlaceOutCSTDone,
        Wait_OutCSTPlaceDoneReceived,
        End,
        Wait_NextSequence
    }
}
