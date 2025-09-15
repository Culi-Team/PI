using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachProcessToRunStep
    {
        Start,
        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,
        Clear_Flags,
        End
    }
}
