using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmReadyStep
    {
        Start,
        Cyl_Up,
        Cyl_Up_Wait,

        Cyl_Puhser_Down,
        Cyl_Pusher_Down_Wait,

        Cyl_Backward,
        Cyl_Backward_Wait,

        Cyl_UnClamp,
        Cyl_UnClamp_Wait,

        End
    }
}
