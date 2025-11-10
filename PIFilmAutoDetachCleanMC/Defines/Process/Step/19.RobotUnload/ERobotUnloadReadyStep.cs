namespace PIFilmAutoDetachCleanMC.Defines.Process
{
    public enum ERobotUnloadReadyStep
    {
        Start,

        Cylinder_Up,
        Cylinder_Up_Wait,

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

        RobotHomePosition_Check,
        RobotSeqHome,
        RobotSeqHome_Check,
        RobotHome,
        RobotHome_Check,

        End
    }
}
