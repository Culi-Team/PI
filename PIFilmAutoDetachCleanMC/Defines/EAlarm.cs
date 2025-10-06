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
        EmergencyStopActivated = 6,
        PowerMCOff = 7,

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
        VinylClean_Motor_UnWinder_Run_Fail,

        //Robot Load
        RobotLoad = 7000,
        RobotLoad_No_Ready_Response,
        RobotLoad_SetModel_Fail,
        RobotLoad_MoveMotionCommand_Timeout,
        RobotLoad_SendMotionCommand_Fail,


        //Align Fixture
        AlignFixture = 8000,

        //Transfer Fixture
        TransferFixture = 9000,
        TransferFixtureOriginFixtureDetect = 9001,
        TransferFixture_YAxis_MoveLoadPosition_Fail,
        TransferFixture_YAxis_MoveUnloadPosition_Fail,
        TransferFixture_YAxis_Origin_Fail,

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
        Detach_ZAxis_Origin_Fail,
        Detach_ShuttleTransferZAxis_Origin_Fail,
        Detach_ShuttleTransferXAxis_Origin_Fail,
        Detach_ZAxis_MoveReadyPosition_Fail,

        //Remove Film
        RemoveFilm = 11000,

        //Glass Transfer
        GlassTransfer = 12000,
        GlassTransfer_YAxis_Origin_Fail,
        GlassTransfer_ZAxis_Origin_Fail,
        GlassTransfer_YAxis_MoveReadyPosition_Fail,
        GlassTransfer_ZAxis_MoveReadyPosition_Fail,
        GlassTransfer_YAxis_MovePickPosition_Fail,
        GlassTransfer_ZAxis_MovePickPosition_Fail,
        GlassTransfer_YAxis_MovePlacePosition_Fail,
        GlassTransfer_ZAxis_MovePlacePosition_Fail,

        //Align Glass Left
        AlignGlassLeft = 13000,

        //Align Glass Right
        AlignGlassRight = 14000,

        //Transfer In Shuttle Left
        TransferInShuttleLeft = 15000,
        TransferInShuttleLeft_YAxis_Origin_Fail,
        TransferInShuttleLeft_ZAxis_Origin_Fail,
        TransferInShuttleLeft_YAxis_MoveReadyPosition_Fail,
        TransferInShuttleLeft_ZAxis_MoveReadyPosition_Fail,
        TransferInShuttleLeft_YAxis_MovePlacePosition_Fail,
        TransferInShuttleLeft_ZAxis_MovePlacePosition_Fail,
        TransferInShuttleLeft_YAxis_MovePickPosition1_Fail,
        TransferInShuttleLeft_YAxis_MovePickPosition2_Fail,
        TransferInShuttleLeft_YAxis_MovePickPosition3_Fail,
        TransferInShuttleLeft_ZAxis_MovePickPosition_Fail,


        //Transfer In Shuttle Right
        TransferInShuttleRight = 16000,
        TransferInShuttleRight_YAxis_Origin_Fail,
        TransferInShuttleRight_ZAxis_Origin_Fail,
        TransferInShuttleRight_YAxis_MoveReadyPosition_Fail,
        TransferInShuttleRight_ZAxis_MoveReadyPosition_Fail,
        TransferInShuttleRight_YAxis_MovePlacePosition_Fail,
        TransferInShuttleRight_ZAxis_MovePlacePosition_Fail,
        TransferInShuttleRight_YAxis_MovePickPosition1_Fail,
        TransferInShuttleRight_YAxis_MovePickPosition2_Fail,
        TransferInShuttleRight_YAxis_MovePickPosition3_Fail,
        TransferInShuttleRight_ZAxis_MovePickPosition_Fail,

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
        WETCleanLeft_RFeedingAxis_MoveForward_Fail,
        WETCleanLeft_RFeedingAxis_MoveBackward_Fail,
        WETCleanLeft_SyringePump_Origin_Fail,

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
        WETCleanRight_RFeedingAxis_MoveForward_Fail,
        WETCleanRight_RFeedingAxis_MoveBackward_Fail,
        WETCleanRight_SyringePump_Origin_Fail,

        //Transfer Rotation Left
        TransferRotationLeft = 19000,
        TransferRotationLeft_ZAxis_Origin_Fail,
        TransferRotationLeft_ZAxis_MoveReadyPosition_Fail,
        TransferRotationLeft_ZAxis_MovePickPosition_Fail,
        TransferRotationLeft_ZAxis_Move_TransferBeforeRotatePosition_Fail,
        TransferRotationLeft_ZAxis_Move_TransferAfterRotatePosition_Fail,
        TransferRotationLeft_ZAxis_MovePlacePosition_Fail,

        //Transfer Rotation Right
        TransferRotationRight = 20000,
        TransferRotationRight_ZAxis_Origin_Fail,
        TransferRotationRight_ZAxis_MoveReadyPosition_Fail,
        TransferRotationRight_ZAxis_MovePickPosition_Fail,
        TransferRotationRight_ZAxis_Move_TransferBeforeRotatePosition_Fail,
        TransferRotationRight_ZAxis_Move_TransferAfterRotatePosition_Fail,
        TransferRotationRight_ZAxis_MovePlacePosition_Fail,

        //AF Clean Left
        AFCleanLeft = 21000,
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
        AFCleanLeft_RFeedingAxis_MoveForward_Fail,
        AFCleanLeft_RFeedingAxis_MoveBackward_Fail,
        AFCleanLeft_SyringePump_Origin_Fail,

        //AF Clean Right
        AFCleanRight = 22000,
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
        AFCleanRight_RFeedingAxis_MoveForward_Fail,
        AFCleanRight_RFeedingAxis_MoveBackward_Fail,
        AFCleanRight_SyringePump_Origin_Fail,

        //Unload Glass Left
        UnloadTransferLeft = 23000,
        UnloadTransferLeft_YAxis_Origin_Fail,
        UnloadTransferLeft_ZAxis_Origin_Fail,
        UnloadTransferLeft_YAxis_MoveReadyPosition_Fail,
        UnloadTransferLeft_ZAxis_MoveReadyPosition_Fail,
        UnloadTransferLeft_YAxis_MovePickPosition_Fail,
        UnloadTransferLeft_ZAxis_MovePickPosition_Fail,
        UnloadTransferLeft_YAxis_MovePlacePosition1_Fail,
        UnloadTransferLeft_YAxis_MovePlacePosition2_Fail,
        UnloadTransferLeft_YAxis_MovePlacePosition3_Fail,
        UnloadTransferLeft_YAxis_MovePlacePosition4_Fail,
        UnloadTransferLeft_ZAxis_MovePlacePosition_Fail,
        UnloadTransferLeft_VacuumOn_Fail,

        //Unload Glass Right
        UnloadTransferRight = 24000,
        UnloadTransferRight_YAxis_Origin_Fail,
        UnloadTransferRight_ZAxis_Origin_Fail,
        UnloadTransferRight_YAxis_MoveReadyPosition_Fail,
        UnloadTransferRight_ZAxis_MoveReadyPosition_Fail,
        UnloadTransferRight_YAxis_MovePickPosition_Fail,
        UnloadTransferRight_ZAxis_MovePickPosition_Fail,
        UnloadTransferRight_YAxis_MovePlacePosition1_Fail,
        UnloadTransferRight_YAxis_MovePlacePosition2_Fail,
        UnloadTransferRight_YAxis_MovePlacePosition3_Fail,
        UnloadTransferRight_YAxis_MovePlacePosition4_Fail,
        UnloadTransferRight_ZAxis_MovePlacePosition_Fail,
        UnloadTransferRight_VacuumOn_Fail,

        //Unload Align
        UnloadAlign = 25000,

        //Robot Unload
        RobotUnload = 26000,
        RobotUnload_No_Ready_Response,
        RobotUnload_SetModel_Fail,
        RobotUnload_MoveMotionCommand_Timeout,
        RobotUnload_SendMotionCommand_Fail,
    }
}
