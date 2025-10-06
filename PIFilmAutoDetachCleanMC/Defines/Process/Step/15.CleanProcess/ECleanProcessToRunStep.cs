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
        Set_Pressure,
        Dispense_Remain,
        Dispense_Remain_Wait,
        Fill,
        Fill_Wait,
        Clear_Flags,
        End
    }
}
