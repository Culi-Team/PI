using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachReadyStep
    {
        Start,
        ZAxis_MoveReady_CylDetach_MoveBackward,
        ZAxis_MoveReady_CylDetach_MoveBackward_Wait,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        End
    }
}
