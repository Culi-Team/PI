using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EUnloadTransferPlaceStep
    {
        Start,
        SetFlagUnloadTransferReady,
        Wait_UnloadAlignReady,

        UnloadAlign_GlassVacCheck,

        YAxis_Move_PlacePosition1,
        YAxis_Move_PlacePosition1_Wait,
        YAxis_Move_PlacePosition2,
        YAxis_Move_PlacePosition2_Wait,
        YAxis_Move_PlacePosition3,
        YAxis_Move_PlacePosition3_Wait,
        YAxis_Move_PlacePosition4,
        YAxis_Move_PlacePosition4_Wait,

        ZAxis_Move_PlacePosition,
        ZAxis_Move_PlacePosition_Wait,

        Vacuum_Off,

        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,

        UnloadAlign_GlassVac_Check,

        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPosition_Wait,

        Set_FlagUnloadTransferPlaceDone,
        Wait_UnloadAlignPlaceDoneReceived,

        End
    }
}
