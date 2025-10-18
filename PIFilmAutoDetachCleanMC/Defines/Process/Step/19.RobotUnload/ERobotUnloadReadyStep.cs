namespace PIFilmAutoDetachCleanMC.Defines.Process
{
    public enum ERobotUnloadReadyStep
    {
        Start,

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

        RobotHomePosition,
        RobotHomePosition_Check,
        RobotSeqHome,
        RobotSeqHome_Check,
        RobotHome,
        RobotHome_Check,

        End
    }
}
