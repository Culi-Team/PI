using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanOriginStep
    {
        Start,
        PushCyl_Up,
        PushCyl_Up_Wait,
        AxisOrigin,
        AxisOrigin_Wait,
        SyringePump_Origin,
        SyringePump_Origin_Wait,
        End
    }
}
