using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadToOriginStep
    {
        Start, 
        Clear_FlagOriginDone,
        ReConnectIfRequired,
        CheckConnection,

        EnableOutput,
        RobotMotionOnCheck,
        RobotMotionOn_Delay,
        RobotMotionOn_Disable,

        RobotStopMessCheck,
        RobotConfMess_Delay,
        RobotConfMess_Disable,

        RobotExtStart_Enable,
        RobotExtStart_Delay,
        RobotExtStart_Disable,

        RobotProgramStart_Check,

        SendPGMStart,
        SendPGMStart_Check,
        SetModel,
        SetModel_Check,

        End,
    }
}
