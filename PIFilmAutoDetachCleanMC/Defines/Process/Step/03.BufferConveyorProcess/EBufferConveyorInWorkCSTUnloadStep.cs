using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EBufferConveyorInWorkCSTUnloadStep
    {
        Start,
        Stopper1_Down,
        Stopper1_Down_Wait,

        Stopper2_Up,
        Stopper2_Up_Wait,

        CSTDetect_Check,

        Wait_InWorkCSTRequestCSTOut,

        Conveyor_Run,

        Conveyor_Stop,

        BothStopper_Up,
        BothStopper_Up_Wait,

        End,
    }
}
