using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferRotationAutoRunStep
    {
        Start,
        GlassVac_Check,

        OutShuttle_Vacuum,
        OutShuttle_Glass_Check,

        Wait_AFClean_Done,

        End
    }
}
