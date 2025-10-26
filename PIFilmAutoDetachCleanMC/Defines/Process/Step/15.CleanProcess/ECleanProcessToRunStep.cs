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
        Cyl_Up,
        Cyl_Up_Wait,
        FeedingRollerDetect_Check,
        Wiper_Check,
        Set_Torque,
        Winder_UnWinder_Run,
        Set_Pressure,
        Clear_Flags,
        End
    }
}
