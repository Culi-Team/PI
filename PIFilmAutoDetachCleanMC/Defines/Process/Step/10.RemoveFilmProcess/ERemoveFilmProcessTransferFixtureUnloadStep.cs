using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmProcessTransferFixtureUnloadStep
    {
        Start,
        Set_Flag_RemoveFilmDone,
        Wait_TransferFixtureDone,
        End
    }
}
