using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EOutConveyorUnloadStep
    {
        Start,
        Stopper_Up,
        Stopper_Up_Wait,

        CSTDetect_Check,
        Conveyor_Run,
        Conveyor_Stop,

        Wait_CSTUnload,

        End
    }
}
