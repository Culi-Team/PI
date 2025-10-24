using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessReadyStep
    {
        Start,

        TransInShuttle_SafePos_Wait,

        Cyl_Up,
        Cyl_Up_Wait,
        XAxis_MoveReadyPosition,
        XAxis_MoveReadyPosition_Wait,
        End
    }
}
