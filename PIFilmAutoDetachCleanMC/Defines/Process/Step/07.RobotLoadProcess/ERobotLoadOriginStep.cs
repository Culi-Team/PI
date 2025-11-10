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
        RobotCurrentPosition_Check,
        RobotCurrentPosition_Wait,
        Check_RobotInPPPosition,
        Fixture_Detect_Check,
        Cyl_Unclamp,
        Cyl_Unclamp_Wait,
        Cyl_UnAlign,
        Cyl_Unalign_Wait,
        RobotHomePosition_Check,
        RobotSeqHome,
        RobotSeqHome_Check,
        Robot_Origin,
        Robot_Origin_Check,
        End
    }
}
