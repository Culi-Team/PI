using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferRotationReadyStep
    {
        Start,
        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,

        Cyl_Up,
        Cyl_Up_Wait,

        Cyl_Transfer_Backward,
        Cyl_Transfer_Backward_Wait,
        End
    }
}
