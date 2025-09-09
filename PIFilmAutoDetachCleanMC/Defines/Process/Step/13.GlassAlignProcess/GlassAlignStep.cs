using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassAlignStep
    {
        Start,
        Vacuum_On_1st,
        Cyl_Align,
        Cyl_Align_Wait,
        Vacuum_Off,
        Wait_GlassDetect,
        Vacuum_On_2nd,
        Cyl_UnAlign, 
        Cyl_UnAlign_Wait,
        End
    }
}
