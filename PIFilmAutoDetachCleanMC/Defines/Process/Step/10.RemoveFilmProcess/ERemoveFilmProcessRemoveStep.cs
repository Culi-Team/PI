using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERemoveFilmProcessRemoveStep
    {
        Start,
        Align_Fixture,
        Align_Fixture_Wait,
        Cyl_Transfer_Forward,
        Cyl_Transfer_Forward_Wait,
        Pusher_Cyl_1_Up,
        Pusher_Cyl_1_Up_Wait,
        Cyl_UpDown1_Down,
        Cyl_UpDown1_Down_Wait,
        Cyl_Clamp,
        Cyl_Clamp_Wait,
        Pusher_Cyl_2_Up,
        Pusher_Cyl_2_Up_Wait,
        Cyl_UpDown1_Up,
        Cyl_UpDown1_Up_Wait,
        Cyl_Transfer_Backward,
        Cyl_Transfer_Backward_Wait,
        Set_Flag_RemoveFilmRequestUnload,
        End
    }
}
