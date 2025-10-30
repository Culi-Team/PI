using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EVinylCleanProcessRobotPlaceFixtureToVinylClean
    {
        Start,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Cyl_Pusher_Down,
        Cyl_Pusher_Down_Wait,
        Cyl_Backward,
        Cyl_Backward_Wait,
        Encoder_Clear_Position,
        Motor_UnWinder_Run,
        Motor_UnWinder_Run_Wait,
        Set_Flag_RequestFixture,
        Wait_RobotMoveVinylCleanDone,
        Cyl_Clamp,
        Cyl_Clamp_Wait,
        SetFlag_VinylCleanClampDone,
        Wait_FixtureLoadDone,
        Fixture_Detect_Check,
        End
    }
}
