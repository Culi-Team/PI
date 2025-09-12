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
        GlassVac_Check,
        Set_FlagUnloadAlignReady,
        Wait_UnloadTransfePlaceDone,
        End
    }
}
