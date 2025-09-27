using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferInShuttlePickStep
    {
        Start,
        Cyl_Rotate_0D,
        Cyl_Rotate_0D_Wait,

        Wait_GlassAlignRequest_Pick,
        GlassDetect_Check,

        YAxis_Move_PickPosition1,
        YAxis_Move_PickPosition1_Wait,

        YAxis_Move_PickPosition2,
        YAxis_Move_PickPosition2_Wait,

        YAxis_Move_PickPosition3,
        YAxis_Move_PickPosition3_Wait,

        ZAxis_Move_PickPosition,
        ZAxis_Move_PickPosition_Wait,

        Vacuum_On,
        Vacuum_On_Wait,

        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,

        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPosition_Wait,

        Set_FlagTransferInShuttlePickDone,
        Wait_GlassAlignPickDoneReceived,
        End
    }
}
