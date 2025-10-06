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
        Wait_AFCleanLoadDone,
        AxisMoveUnloadPosition,
        AxisMoveUnloadPosition_Wait,
        Vacuum_Off,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Set_FlagRequestUnload,
        Wait_CleanUnloadDone,
        End
    }
}
