using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EVinylCleanProcessRobotPickFixtureFromVinylClean
    {
        Start,
        Clear_Flag,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        SetFlag_VinylCleanRequestUnload,
        Wait_VinylCleanUnloadDone,
        End
    }
}
