using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmProcessTransferFixtureStep
    {
        Start,
        Cyl_Fixture_Unclamp,
        Cyl_Fixture_Unclamp_Wait,
        Wait_PreviousTransferDone,
        Set_Flag_RemoveFilmDone,
        Wait_TransferFixtureDone,
        Wait_TransferFixtureClear,
        Fixture_Detect_Check,
        End
    }
}
