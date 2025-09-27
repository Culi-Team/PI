using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EUnloadAlignStep
    {
        Start,
        Cyl_Align_Up,
        Cyl_Align_Up_Wait,
        Vacuum_Off_Align,
        Vacuum_On,
        Vacuum_On_Wait,
        GlassDetect_Check,
        Cyl_Align_Down,
        Cyl_Align_Down_Wait,

        End
    }
}
