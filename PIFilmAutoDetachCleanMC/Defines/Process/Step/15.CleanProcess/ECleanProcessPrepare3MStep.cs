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

        Dispense_Port1,
        Dispense_Port1_Wait,

        Dispense_Port2,
        Dispense_Port2_Wait,

        Dispense_Port3,
        Dispense_Port3_Wait,

        Dispense_Port4,
        Dispense_Port4_Wait,

        Dispense_Port5,
        Dispense_Port5_Wait,

        Dispense_Port6,
        Dispense_Port6_Wait,

        Feeding_Backward,
        Feeding_Backward_Wait,

        RemainVolume_Check,
        Fill,
        FlowSensor_Check,
        Fill_Wait,

        End,
    }
}
