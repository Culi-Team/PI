using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachProcessTransferFixtureLoadStep
    {
        Start,
        DetachZAxis_Move_ReadyPosition,
        DetachZAxis_Move_ReadyPosition_Wait,

        Detach_Cylinder_Up,
        Detach_Cylinder_Up_Wait,
        Set_FlagDetachDoneForSemiAutoSequence,

        Wait_TransferFixtureClampDone,

        Cyl_Clamp_Backward,
        Cyl_Clamp_Backward_Wait,

        Wait_FixtureTransferDone,
        Clear_FlagDetachDone,
        Wait_TransferFixtureClear,

        Fixture_Detect_Check,

        End
    }
}
