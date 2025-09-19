using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EInConveyorToRunStep
    {
        Start,
        Conveyor_Stop,
        Set_ConveyorSpeed,
        Set_ConveyorAccel,
        Set_ConveyorDeccel,

        End
    }
}
