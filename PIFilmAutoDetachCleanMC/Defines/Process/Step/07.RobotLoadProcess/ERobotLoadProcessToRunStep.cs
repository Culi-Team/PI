using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadProcessToRunStep
    {
        Start,
        Send_CassettePitch,
        Send_CassettePitch_Check,
        Clear_Flags,
        End
    }
}
