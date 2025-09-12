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

        //BufferConveyor
        BufferConveyor = 3000,

        //OutWorkConveyor
        OutWorkConveyor = 4000,

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

        //Detach
        Detach = 10000,

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

        //WET Clean Right
        WETCleanRight = 18000,

        //AF Clean Left
        AFCleanLeft = 19000,

        //AF Clean Right
        AFCleanRight = 20000,

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
