using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EWorkConveyorOriginStep
    {
        Start,
        Cyl_UnTilt,
        Cyl_UnTilt_Wait,
        SupportCV_Down,
        SupportCV_Down_Wait,
        TAxis_Origin,
        TAxis_Origin_Wait,
        Cyl_UnAlign,
        Cyl_UnAlign_Wait,
        Roller_Stop,
        End
    }
}
