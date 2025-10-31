using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EVinylCleanProcessVinylCleanStep
    {
        Start,
        FixtureDetect_Check,
        Cyl_Clamp,
        Cyl_Clamp_Delay,
        Cyl_Clamp_Wait,
        Cyl_Pusher_Up,
        Cyl_Pusher_Up_Wait,
        Cyl_Forward,
        Cyl_Forward_Wait,
        Cyl_Backward,
        Cyl_Backward_Wait,
        Cyl_Pusher_Down,
        Cyl_Pusher_Down_Wait,
        End,
    }
}
