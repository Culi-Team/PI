using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachAutoRunStep
    {
        Start,

        Handle_GlassOnShuttle,
        Handle_GlassNotOnShuttle,

        End
    }
}
