using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessToRunStep
    {
        Start,
        FeedingRollerDetect_Check,
        Wiper_Check,
        Clear_Flags,
        End
    }
}
