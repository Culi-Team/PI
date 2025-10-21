using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachProcessTransferFixtureUnloadStep
    {
        Start,
        Clear_Flag,
        DetachZAxis_Move_ReadyPosition,
        DetachedZAxis_Move_ReadyPosition_Wait,
        Cyl_Fix_Backward,
        Cyl_Fix_Backward_Wait,
        SetFlagDetachDone,
        Wait_FixtureTransferDone,
        End
    }
}
