namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EAlarm
    {
        None = -1,

        // System Level Alarms
        MainAirNotSupplied = 0,
        MainPowerDown = 1,
        MotionAlarmDetected = 2,
        DoorOpen = 3,
        LightCurtainLeftDetected = 4,
        LightCurtainRightDetected = 5,

        //InConveyor
        InConveyor = 1000,

        //InWorkConveyor
        InWorkConveyor = 2000,
        InWorkConveyor_TAxis_Origin_Fail,
        InWorkConveyor_TAxis_MoveWorkPosition_Fail,
        InWorkConveyor_TAxis_MoveLoadPosition_Fail,

        //BufferConveyor
        BufferConveyor = 3000,

        //OutWorkConveyor
        OutWorkConveyor = 4000,
        OutWorkConveyor_TAxis_Origin_Fail,
        OutWorkConveyor_TAxis_MoveWorkPosition_Fail,
        OutWorkConveyor_TAxis_MoveLoadPosition_Fail,

        //OutConveyor
        OutConveyor = 5000,

        //VinylClean
        VinylClean = 6000,

        //Robot Load
        RobotLoad = 7000,
        RobotLoad_No_Ready_Response,
        RobotLoad_SetModel_Fail,
        RobotLoad_MoveMotionCommand_Timeout,


        //Align Fixture
        AlignFixture = 8000,

        //Transfer Fixture
        TransferFixture = 9000,
        TransferFixtureOriginFixtureDetect = 9001,
        TransferFixture_YAxis_MoveLoadPosition_Fail,
        TransferFixture_YAxis_MoveUnloadPosition_Fail,

        //Detach
        Detach = 10000,
        Detach_ShuttleTransferZAxis_MoveReadyPosition_Fail,
        Detach_ShuttleTransferXAxis_MoveDetachCheckPosition_Fail,
        Detach_ShuttleTransferXAxis_MoveUnloadPosition_Fail,
        Detach_ShuttleTransferZAxis_MoveUnloadPosition_Fail,
        Detach_ShuttleTransferXAxis_MoveDetachPosition_Fail,
        Detach_ZAxis_MoveDetachReadyPosition_Fail,
        Detach_ZAxis_MoveDetachPosition1_Fail,
        Detach_ZAxis_MoveDetachPosition2_Fail,

        //Remove Film
        RemoveFilm = 11000,

        //Glass Transfer
        GlassTransfer = 12000,

        //Align Glass Left
        AlignGlassLeft = 13000,

        //Align Glass Right
        AlignGlassRight = 14000,

        //Transfer In Shuttle Left
        TransferInShuttleLeft = 15000,

        //Transfer In Shuttle Right
        TransferInShuttleRight = 16000,

        //WET Clean Left
        WETCleanLeft = 17000,
        WETCleanLeft_XAxis_Origin_Fail,
        WETCleanLeft_YAxis_Origin_Fail,
        WETCleanLeft_TAxis_Origin_Fail,
        WETCleanLeft_FeedingAxis_Origin_Fail,
        WETCleanLeft_XAxis_MoveLoadPosition_Fail,
        WETCleanLeft_YAxis_MoveLoadPosition_Fail,
        WETCleanLeft_TAxis_MoveLoadPosition_Fail,
        WETCleanLeft_XAxis_MoveCleanHorizontalPosition_Fail,
        WETCleanLeft_YAxis_MoveCleanHorizontalPosition_Fail,
        WETCleanLeft_TAxis_MoveCleanHorizontalPosition_Fail,
        WETCleanLeft_XAxis_MoveCleanVerticalPosition_Fail,
        WETCleanLeft_YAxis_MoveCleanVerticalPosition_Fail,
        WETCleanLeft_TAxis_MoveCleanVerticalPosition_Fail,
        WETCleanLeft_CleanHorizontal_Fail,
        WETCleanLeft_CleanVertical_Fail,
        WETCleanLeft_XAxis_MoveUnloadPosition_Fail,
        WETCleanLeft_YAxis_MoveUnloadPosition_Fail,
        WETCleanLeft_TAxis_MoveUnloadPosition_Fail,

        //WET Clean Right
        WETCleanRight = 18000,
        WETCleanRight_XAxis_Origin_Fail,
        WETCleanRight_YAxis_Origin_Fail,
        WETCleanRight_TAxis_Origin_Fail,
        WETCleanRight_FeedingAxis_Origin_Fail,
        WETCleanRight_XAxis_MoveLoadPosition_Fail,
        WETCleanRight_YAxis_MoveLoadPosition_Fail,
        WETCleanRight_TAxis_MoveLoadPosition_Fail,
        WETCleanRight_XAxis_MoveCleanHorizontalPosition_Fail,
        WETCleanRight_YAxis_MoveCleanHorizontalPosition_Fail,
        WETCleanRight_TAxis_MoveCleanHorizontalPosition_Fail,
        WETCleanRight_XAxis_MoveCleanVerticalPosition_Fail,
        WETCleanRight_YAxis_MoveCleanVerticalPosition_Fail,
        WETCleanRight_TAxis_MoveCleanVerticalPosition_Fail,
        WETCleanRight_CleanHorizontal_Fail,
        WETCleanRight_CleanVertical_Fail,
        WETCleanRight_XAxis_MoveUnloadPosition_Fail,
        WETCleanRight_YAxis_MoveUnloadPosition_Fail,
        WETCleanRight_TAxis_MoveUnloadPosition_Fail,

        //AF Clean Left
        AFCleanLeft = 19000,
        AFCleanLeft_XAxis_Origin_Fail,
        AFCleanLeft_YAxis_Origin_Fail,
        AFCleanLeft_TAxis_Origin_Fail,
        AFCleanLeft_FeedingAxis_Origin_Fail,
        AFCleanLeft_XAxis_MoveLoadPosition_Fail,
        AFCleanLeft_YAxis_MoveLoadPosition_Fail,
        AFCleanLeft_TAxis_MoveLoadPosition_Fail,
        AFCleanLeft_XAxis_MoveCleanHorizontalPosition_Fail,
        AFCleanLeft_YAxis_MoveCleanHorizontalPosition_Fail,
        AFCleanLeft_TAxis_MoveCleanHorizontalPosition_Fail,
        AFCleanLeft_XAxis_MoveCleanVerticalPosition_Fail,
        AFCleanLeft_YAxis_MoveCleanVerticalPosition_Fail,
        AFCleanLeft_TAxis_MoveCleanVerticalPosition_Fail,
        AFCleanLeft_CleanHorizontal_Fail,
        AFCleanLeft_CleanVertical_Fail,
        AFCleanLeft_XAxis_MoveUnloadPosition_Fail,
        AFCleanLeft_YAxis_MoveUnloadPosition_Fail,
        AFCleanLeft_TAxis_MoveUnloadPosition_Fail,

        //AF Clean Right
        AFCleanRight = 20000,
        AFCleanRight_XAxis_Origin_Fail,
        AFCleanRight_YAxis_Origin_Fail,
        AFCleanRight_TAxis_Origin_Fail,
        AFCleanRight_FeedingAxis_Origin_Fail,
        AFCleanRight_XAxis_MoveLoadPosition_Fail,
        AFCleanRight_YAxis_MoveLoadPosition_Fail,
        AFCleanRight_TAxis_MoveLoadPosition_Fail,
        AFCleanRight_XAxis_MoveCleanHorizontalPosition_Fail,
        AFCleanRight_YAxis_MoveCleanHorizontalPosition_Fail,
        AFCleanRight_TAxis_MoveCleanHorizontalPosition_Fail,
        AFCleanRight_XAxis_MoveCleanVerticalPosition_Fail,
        AFCleanRight_YAxis_MoveCleanVerticalPosition_Fail,
        AFCleanRight_TAxis_MoveCleanVerticalPosition_Fail,
        AFCleanRight_CleanHorizontal_Fail,
        AFCleanRight_CleanVertical_Fail,
        AFCleanRight_XAxis_MoveUnloadPosition_Fail,
        AFCleanRight_YAxis_MoveUnloadPosition_Fail,
        AFCleanRight_TAxis_MoveUnloadPosition_Fail,

        //Unload Glass Left
        UnloadGlassLeft = 21000,

        //Unload Glass Right
        UnloadGlassRight = 22000,

        //Unload Align
        UnloadAlign = 23000,

        //Robot Unload
        RobotUnload = 24000,
    }
}
