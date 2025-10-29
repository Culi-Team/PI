using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferRotationWETCleanUnloadStep
    {
        Start,
        Cyl_Up,
        Cyl_Up_Wait,
        Cyl_Backward,
        Cyl_Backward_Wait,
        Set_FlagTransferRotationReadyPick,
        Wait_WETCleanRequestUnload,
        ZAxis_Move_PickPosition,
        ZAxis_Move_PickPositionWait,
        Vacuum_On,
        Vacuum_On_Wait,
        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPositionWait,

        ZAxis_Move_TransferReadyPosition,
        ZAxis_Move_TransferReadyPosition_Wait,

        Set_FlagWETCleanUnloadDone,
        Wait_WETCleanUnloadDoneReceived,
        End
    }
}
