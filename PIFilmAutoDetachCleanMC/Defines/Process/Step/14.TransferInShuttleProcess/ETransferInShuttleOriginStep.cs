using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferInShuttleOriginStep
    {
        Start,
        ZAxis_Origin,
        ZAxis_Origin_Wait,
        YAxis_Origin,
        YAxis_Origin_Wait,
        End
    }
}
