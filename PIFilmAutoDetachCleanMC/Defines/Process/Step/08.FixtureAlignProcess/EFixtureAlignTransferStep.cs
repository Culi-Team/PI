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
        Set_FlagAlignDoneForSemiAutoSequence,
        Wait_TransferFixtureClampDone,
        Cyl_UnAlign,
        Cyl_UnAlign_Wait,
        Set_FlagUnClampDone,
        Clear_FlagUnClampDone,
        Wait_TransferDone,
        End,
    }
}
