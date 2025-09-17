using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EGlassAlignGlassTransferPlaceStep
    {
        Start,
        Cyl_Align_Down,
        Cyl_Align_Down_Wait,
        Set_FlagRequestGlass,
        Wait_GlassTransferPlace_Done,
        End
    }
}
