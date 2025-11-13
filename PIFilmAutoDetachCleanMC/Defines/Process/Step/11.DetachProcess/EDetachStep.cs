using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EDetachStep
    {
        Start,

        InitQueue,
        StepQueue_EmptyCheck,

        Cyl_Clamp_Forward,
        Cyl_Clamp_Forward_Wait,

        ShuttleZAxis_Move_ReadyPosition,
        ShuttleZAxis_Move_ReadyPosition_Wait,

        XAxis_Move_DetachPosition,
        XAxis_Move_DetachPosition_Wait,

        ZAxis_Move_ReadyDetach1Position,
        ZAxis_Move_ReadyDetach1Position_Wait,

        Cyl_Detach1_Down,
        Cyl_Detach1_Down_Wait,
        ZAxis_Move_Detach1Position,
        ZAxis_Move_Detach1Position_Wait,

        Vacuum_On,

        ZAxis_Move_ReadyDetach2Position,
        ZAxis_Move_ReadyDetach2Position_Wait,
        Cyl_Detach2_Down,
        Cyl_Detach2_Down_Wait,
        ZAxis_Move_Detach2Position,
        ZAxis_Move_Detach2Position_Wait,

        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,

        Cyl_Detach_Up,
        Cyl_Detach_Up_Wait,

        XAxis_Move_DetachCheck_Position,
        XAxis_Move_DetachCheck_Position_Wait,
        Vacuum_Check,

        Cyl_Clamp_Backward,
        Cyl_Clamp_Backward_Wait,

        Set_FlagDetachDone,

        End
    }
}
