using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EInWorkConveyorProcessInWorkCSTLoadStep
    {
        Start,
        Wait_InWorkCSTRequestLoad,

        Stopper_Down,
        Stopper_Down_Wait,

        Muting_LightCurtain,
        Conveyor_Run,
        Wait_InWorkCSTLoadDone,
        Enable_LightCurtain,
        Conveyor_Stop,

        Stopper_Up,
        Stopper_Up_Wait,
        End
    }
}
