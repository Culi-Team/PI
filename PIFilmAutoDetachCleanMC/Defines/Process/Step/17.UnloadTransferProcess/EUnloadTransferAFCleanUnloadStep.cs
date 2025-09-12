using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EUnloadTransferAFCleanUnloadStep
    {
        Start,
        YAxis_Move_PickPositon,
        YAxis_Move_PickPosition_Wait,

        Wait_AFCleanRequestUnload,

        ZAxis_Move_PickPosition,
        ZAxis_Move_PickPositon_Wait,

        Vacuum_On,

        ZAxis_Move_ReadyPositon,
        ZAxis_Move_ReadyPositon_Wait,

        Set_FlagAFCleanUnloadDone,
        Wait_AFCleanUnloadDoneReceived,

        End
    }
}
