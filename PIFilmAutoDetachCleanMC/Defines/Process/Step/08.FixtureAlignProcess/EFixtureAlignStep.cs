using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EFixtureAlignStep
    {
        Start,
        Cyl_Align,
        Cyl_Align_Wait,
        Cyl_Align2,
        Cyl_Align_Wait2,
        TiltCheck,
        ReverseCheck,
        SetFlagAlignDone,
        End,
    }
}
