using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotUnloadOriginStep
    {
        Start,
        Cylinder_Up,
        Cylinder_Up_Wait,
        RobotHomePosition,
        RobotHomePosition_Check,
        RobotSeqHome,
        RobotSeqHome_Check,
        Robot_Origin,
        Robot_Origin_Check,
        End
    }
}
