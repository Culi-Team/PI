using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferFixtureProcessUnloadStep
    {
        Start,
        Cyl_Up,
        Cyl_Up_Wait,
        Wait_RemoveFilm_Done,
        YAxis_Move_UnloadPosition,
        YAxis_Move_UnloadPosition_Wait,
        Cyl_Down,
        Cyl_Down_Wait,
        Cyl_UnClamp, 
        Cyl_UnClamp_Wait,
        SetFlagTransferDone,
        WaitProcesses_ReceiveTransferDone,
        End
    }
}
