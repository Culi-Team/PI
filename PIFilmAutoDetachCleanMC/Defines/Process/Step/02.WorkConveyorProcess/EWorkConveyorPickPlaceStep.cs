using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EWorkConveyorPickPlaceStep
    {
        Start,
        Cassette_Detect_Check,
        Cassette_WorkCondition_Check,
        Set_FlagReady,
        Wait_Robot_PickPlace_Done,
        End
    }
}
