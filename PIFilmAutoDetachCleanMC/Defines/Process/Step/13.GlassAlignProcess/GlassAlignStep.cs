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
        Vacuum_On_1st_Wait,
        Cyl_Align_Up,
        Cyl_Align_Up_Wait,
        Vacuum_Off,
        Wait_GlassDetect,
        Vacuum_On_2nd,
        Vacuum_On_2nd_Wait,
        Cyl_Align_Down, 
        Cyl_Align_Down_Wait,
        End
    }
}
