using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilm_ReadyStep
    {
        Start,

        Wait_Robot_Ready,

        CylUpDown_CylPusher_MoveBackward,
        CylUpDown_CylPusher_MoveBackward_Wait,

        CylTransfer_Backward,
        CylTransfer_Backward_Wait,

        Cyl_UnClamp,
        Cyl_UnClamp_Wait,

        End
    }
}
