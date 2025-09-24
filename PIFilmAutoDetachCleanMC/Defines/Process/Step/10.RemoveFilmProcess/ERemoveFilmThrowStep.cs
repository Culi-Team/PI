using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmThrowStep
    {
        Start,
        Cyl_UpDown1_Down,
        Cyl_UpDown1_Down_Wait,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Cyl_UpDown2_Down_1st,
        Cyl_UpDown2_Down_1st_Wait,
        Cyl_UpDown2_Up_1st,
        Cyl_UpDown2_Up_1st_Wait,
        Cyl_UpDown2_Down_2nd,
        Cyl_UpDown2_Down_2nd_Wait,
        Cyl_UpDown2_Up_2nd,
        Cyl_UpDown2_Up_2nd_Wait,
        Cyl_UpDown1_Up,
        Cyl_UpDown1_Up_Wait,
        Wait_RemoveFilmUnloadDone,
        End
    }
}
