using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferFixtureProcessLoadStep
    {
        Start,
        Wait_Align_And_Detach_Done,
        Check_Y_Position,
        Cyl_Up,
        Cyl_Up_Wait,
        YAxis_Move_LoadPosition,
        YAxis_Move_LoadPosition_Wait,
        Cyl_Down,
        Cyl_Down_Wait,
        Cyl_Clamp,
        Cyl_Clamp_Wait,
        End
    }
}
