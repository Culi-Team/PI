using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachProcessGlassTransferPickStep
    {
        Start,
        Vacuum_Off,
        Set_FlagDetachRequestUnload,
        Wait_GlassTransferPickDone,
        Set_FlagDetachDone,
        End
    }
}
