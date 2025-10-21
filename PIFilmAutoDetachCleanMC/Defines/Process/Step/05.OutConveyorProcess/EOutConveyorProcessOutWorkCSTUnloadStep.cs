using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EOutConveyorProcessOutWorkCSTUnloadStep
    {
        Start,

        CstExist_Check,

        Stopper_Up,
        Stopper_Up_Wait,

        Wait_OutWorkCSTRequestUnload,
        Set_FlagOutConveyorReady,

        Conveyor_Run,
        Wait_OutWorkCSTUnloadDone,
        Conveyor_Stop,
        End
    }
}
