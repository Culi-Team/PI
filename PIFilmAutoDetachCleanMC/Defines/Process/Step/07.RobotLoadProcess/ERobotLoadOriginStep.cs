using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadOriginStep
    {
        Start,
        Fixture_Detect_Check,
        Cyl_Backward,
        Cyl_BackwardWait,
        RobotHomePosition,
        RobotHomePosition_Check,
        RobotSeqHome,
        RobotSeqHome_Check,
        Robot_Origin,
        Robot_Origin_Check,
        End
    }
}
