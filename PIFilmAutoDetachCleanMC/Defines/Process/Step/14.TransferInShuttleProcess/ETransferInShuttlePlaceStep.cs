using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferInShuttlePlaceStep
    {
        Start,
        YAxis_Move_PlacePosition,
        YAxis_Move_PlacePosition_Wait,
        Wait_WETCleanRequestLoad,
        ZAxis_Move_PlacePosition,
        ZAxis_Move_PlacePosition_Wait,
        Vacuum_Off,
        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,
        Set_Flag_TransferInShuttlePlaceDone,
        Wait_WETCleanPlaceDoneReceived,
        End
    }
}
