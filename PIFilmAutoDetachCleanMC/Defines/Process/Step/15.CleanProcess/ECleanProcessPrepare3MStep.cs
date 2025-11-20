using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ECleanProcessPrepare3MStep
    {
        Start,
        Feeding_Forward,
        Feeding_Forward_Wait,

        Dispense,
        Dispense_Wait,

        Feeding_Backward,
        Feeding_Backward_Wait,

        RemainVolume_Check,
        Fill,
        FlowSensor_Check,
        Fill_Wait,

        End,
    }
}
