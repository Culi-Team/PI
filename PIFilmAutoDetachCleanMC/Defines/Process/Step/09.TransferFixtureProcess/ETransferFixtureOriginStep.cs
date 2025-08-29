using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Process.Step._09.TransferFixtureProcess
{
    public enum ETransferFixtureOriginStep
    {
        Start,
        FixtureDetectCheck,
        Unclamp,
        Unclamp_Wait,
        Wait_Detach_Origin,
        CylUp,
        CylUp_Wait,
        YAxis_Origin,
        YAxis_Origin_Wait,
        End
    }
}
