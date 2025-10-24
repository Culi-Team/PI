namespace PIFilmAutoDetachCleanMC.Defines.Process
{
    public enum ERobotLoad_ReadyStep
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

        RobotHomePosition_Check,

        RobotSeqHome,
        RobotSeqHome_Check,
        RobotReady,
        RobotReady_Check,

        Set_RobotReadyFlag,

        Send_CassettePitch,
        Send_CassettePitch_Check,

        End

    }
}
