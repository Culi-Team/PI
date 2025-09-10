using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachProcessOriginStep
    {
        Start,
        ZAxis_Origin,
        ZAxis_Origin_Wait,
        DetachCyl_Up,
        DetachCyl_Up_Wait,

        Cyl_Fix_Backward,
        Cyl_Fix_Backward_Wait,

        ShtTransferXAxis_Origin,
        ShtTransferXAxis_Origin_Wait,
        End
    }
}
