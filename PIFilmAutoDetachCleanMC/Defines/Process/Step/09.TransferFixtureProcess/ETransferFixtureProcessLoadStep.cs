using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    // TODO: SIMPLY STEPS
    public enum ETransferFixtureProcessLoadStep
    {
        Start,
        Wait_Align_And_Detach_Done,
        Check_Y_Position,
        StepQueue_EmptyCheck,

        Cyl_Up,
        Cyl_Up_Wait,
        YAxis_Move_LoadPosition,
        YAxis_Move_LoadPosition_Wait,
        Cyl_Down,
        Cyl_Down_Wait,
        Cyl_Clamp,
        Cyl_Clamp_Wait,
        Wait_RemoveFilm_Done,
        YAxis_Move_UnloadPosition,
        YAxis_Move_UnloadPosition_Wait,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,

        SetFlagTransferDone,
        WaitProcesses_ReceiveTransferDone,

        End
    }
}
