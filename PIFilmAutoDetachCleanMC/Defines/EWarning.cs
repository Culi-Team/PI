using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EWarning
    {
        //Root
        Root = 0,
        ManualModeSwitch,
        DoorOpen,
        DoorNotSafetyLock,
        LightCurtainLeftDetected,
        LightCurtainRightDetected,

        //InConveyor
        InConveyor = 1000,
        InConveyor_CST_Position_Error,

        //InWorkConveyor
        InWorkConveyor = 2000,
        InWorkConveyorCSTNotDetect,

        //BufferConveyor
        BufferConveyor = 3000,
        BufferConveyor_CST_Position_Error,

        //OutWorkConveyor
        OutWorkConveyor = 4000,
        OutWorkConveyorCSTNotDetect,

        //OutConveyor
        OutConveyor = 5000,

        //VinylClean
        VinylClean = 6000,
        VinylCleanFixtureNotDetect,

        //Robot Load
        RobotLoad = 7000,
        RobotLoad_Connect_Fail,
        RobotLoad_Programing_Not_Running,
        RobotLoadOriginFixtureDetect,
        RobotLoad_Home_Manual_By_TeachingPendant,

        //Fixture Align
        FixtureAlign = 8000,
        FixtureAlignTiltDetect = 8001,
        FixtureAlignReverseDetect = 8002,
        FixtureAlignLoadFail = 8003,

        //Transfer Fixture
        TransferFixture = 9000,
        TransferFixtureOriginFixtureDetect = 9001,

        //Detach
        Detach = 10000,
        DetachFail = 10001,

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
