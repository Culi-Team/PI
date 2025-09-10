using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessLoadStep
    {
        Start,
        AxisMoveLoadPosition,
        AxisMoveLoadPosition_Wait,
        Set_FlagCleanRequestLoad,
        Wait_CleanLoadDone,
        Set_FlagCleanLoadDoneReceived,
        End
    }
}
