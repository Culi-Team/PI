using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessAutoRunStep
    {
        Start,
        Vacuum_On,
        Dispense_Remain,
        Dispense_Remain_Wait,
        Fill,
        Fill_Wait,

        VacDetect_Check,
        End
    }
}
