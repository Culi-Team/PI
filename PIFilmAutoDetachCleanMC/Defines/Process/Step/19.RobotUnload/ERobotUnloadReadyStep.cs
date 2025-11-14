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

        RobotCurrentPosition_Check,
        RobotCurrentPosition_Wait,
        Check_RobotInRDYPosition,

        RobotSeqHome,
        RobotSeqHome_Check,
        RobotReady,
        RobotReady_Check,

        End
    }
}
