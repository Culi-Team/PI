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
        Cyl_Align,
        Cyl_Align_Wait,
        Vacuum_Off_Align,
        Vacuum_On,
        GlassDetect_Check,
        Cyl_UnAlign,
        Cyl_UnAlign_Wait,

        End
    }
}
