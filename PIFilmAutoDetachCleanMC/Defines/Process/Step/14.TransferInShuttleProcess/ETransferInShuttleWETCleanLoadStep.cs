using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferInShuttleWETCleanLoadStep
    {
        Start,

        Cyl_Rotate_0D,
        Cyl_Rotate_0D_Wait,

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

        Brush_Cyl_Up,
        Brush_Cyl_Up_Wait,

        ZAxis_Move_TransferReadyPosition,
        ZAxis_Move_TransferReadyPosition_Wait,

        YAxis_Move_PlacePosition,
        YAxis_Move_PlacePosition_Wait,

        Cyl_Rotate_180D,
        Cyl_Rotate_180D_Wait,

        Wait_WETCleanRequestLoad,

        ZAxis_Move_PlacePosition,
        ZAxis_Move_PlacePosition_Wait,

        Vacuum_Off,

        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,

        Set_FlagWETCleanLoadDone,
        Wait_WETCleanPlaceDoneReceived,
        End
    }
}
