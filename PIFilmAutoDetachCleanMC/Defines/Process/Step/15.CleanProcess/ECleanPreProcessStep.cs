using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanPreProcessStep
    {
        Start,
        Shuttle_XAxis_Collision_Check,
        Wiper_Detect_Check,
        PumpLeak_Detect_Check,
        AlcoholLeak_Detect_Check,
        End
    }
}
