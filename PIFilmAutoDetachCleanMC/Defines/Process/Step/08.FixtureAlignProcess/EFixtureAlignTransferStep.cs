using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EFixtureAlignTransferStep
    {
        Start,
        Clear_Flag,
        Cyl_UnAlign,
        Cyl_UnAlign_Wait,
        Set_FlagAlignDone,
        Wait_TransferDone,
        End,
    }
}
