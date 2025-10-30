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
        SetFlag_VinylCleanRequestUnload,
        Wait_RobotMoveVinylCleanDone,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        SetFlag_VinylCleanUnClampDone,
        Wait_VinylCleanUnloadDone,
        End
    }
}
