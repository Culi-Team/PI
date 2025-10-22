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
        CylDown_ZAxisUp,
        CylDown_ZAxisUp_Wait,
        YAxis_MoveReady,
        YAxis_MoveReady_Wait,
        Set_FlagRequestGlass,
        Wait_GlassTransferPlace_Done,
        End
    }
}
