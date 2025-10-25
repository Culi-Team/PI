using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmProcessOriginStep
    {
        Start,
        Wait_Robot_Origin,

        Clamp_Cyl_Backward,
        Clamp_Cyl_Backward_Wait,
        Pusher_Cyl_Down,
        Pusher_Cyl_Down_Wait,
        Cyl_Up,
        Cyl_Up_Wait,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Cyl_Transfer_Backward,
        Cyl_Transfer_Backward_Wait,
        End
    }
}
