using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmRobotPickFromRemoveZoneStep
    {
        Start,

        StepQueue_EmptyCheck,

        Cyl_UnClamp,
        Cyl_UnClamp_Wait,

        Set_Flag_RemoveFilmRequestUnload,
        Cyl_UpDown1_Down,
        Cyl_UpDown1_Down_Wait,

        FilmCyl_UnClamp,
        FilmCyl_UnClamp_Wait,

        Cyl_UpDown2_Down,
        Cyl_UpDown2_Down_Wait,
        Cyl_UpDown2_Up,
        Cyl_UpDown2_Up_Wait,

        Cyl_UpDown1_Up,
        Cyl_UpDown1_Up_Wait,

        Cyl_Pusher_Down,
        Cyl_Pusher_Down_Wait,
        Wait_RemoveFilmUnloadDone,
        End
    }
}
