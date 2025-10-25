using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EUnloadAlignUnloadTransferPlaceStep
    {
        Start,
        Vacuum_On,
        GlassVac_Check,
        Set_FlagUnloadAlignReady,
        Wait_UnloadTransferPlaceDone,
        End
    }
}
