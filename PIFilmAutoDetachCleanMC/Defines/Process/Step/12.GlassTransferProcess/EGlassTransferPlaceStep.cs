using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassTransferPlaceStep
    {
        Start,
        Wait_Glass_AlignRequestGlass,

        YAxis_Move_PlacePosition,
        YAxis_Move_PlacePosition_Wait,

        Cyl_Down,
        Cyl_Down_Wait,

        ZAxis_Move_PlacePosition,
        ZAxis_Move_PlacePosition_Wait,
        Vacuum_Off,
        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,

        Cyl_Up,
        Cyl_Up_Wait,

        YAxis_Move_ReadyPosition,
        YAxis_Move_ReadyPosition_Wait,

        Set_Flag_PlaceDone,
        End
    }
}
