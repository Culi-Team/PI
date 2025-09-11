using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessUnloadStep
    {
        Start,
        AxisMoveUnloadPosition,
        AxisMoveUnloadPosition_Wait,
        Vacuum_Off,
        Set_FlagRequestUnload,
        Wait_CleanUnloadDone,
        End
    }
}
