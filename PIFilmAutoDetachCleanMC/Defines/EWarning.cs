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
        InConveyor_CST_Stopper_Up_Fail,
        InConveyor_CST_Stopper_Down_Fail,

        //InWorkConveyor
        InWorkConveyor = 2000,
        InWorkConveyorCSTNotDetect,
        InWorkConveyor_TiltCylinder_Up_Fail,
        InWorkConveyor_TiltCylinder_Down_Fail,
        InWorkConveyor_SupportCV_Up_Fail,
        InWorkConveyor_SupportCV_Down_Fail,
        InWorkConveyor_FixCylinder_Backward_Fail,
        InWorkConveyor_FixCylinder_Forward_Fail,

        //BufferConveyor
        BufferConveyor = 3000,
        BufferConveyor_CST_Position_Error,
        BufferConveyor_Stopper1_Up_Fail,
        BufferConveyor_Stopper1_Down_Fail,
        BufferConveyor_Stopper2_Up_Fail,
        BufferConveyor_Stopper2_Down_Fail,

        //OutWorkConveyor
        OutWorkConveyor = 4000,
        OutWorkConveyorCSTNotDetect,
        OutWorkConveyor_TiltCylinder_Up_Fail,
        OutWorkConveyor_TiltCylinder_Down_Fail,
        OutWorkConveyor_SupportCV_Up_Fail,
        OutWorkConveyor_SupportCV_Down_Fail,
        OutWorkConveyor_FixCylinder_Backward_Fail,
        OutWorkConveyor_FixCylinder_Forward_Fail,

        //OutConveyor
        OutConveyor = 5000,
        OutConveyor_Stopper_Up_Fail,
        OutConveyor_Stopper_Down_Fail,

        //VinylClean
        VinylClean = 6000,
        VinylCleanFixtureNotDetect,
        VinylClean_FixtureCylinder_Clamp_Fail,
        VinylClean_FixtureCylinder_UnClamp_Fail,
        VinylClean_PusherCylinder_Up_Fail,
        VinylClean_PusherCylinder_Down_Fail,
        VinylClean_RollerBwFwCylinder_Backward_Fail,
        VinylClean_RollerBwFwCylinder_Forward_Fail,

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
        FixtureAlign_AlignCylinder_Backward_Fail,
        FixtureAlign_AlignCylinder_Forward_Fail,

        //Transfer Fixture
        TransferFixture = 9000,
        TransferFixtureOriginFixtureDetect = 9001,
        TransferFixture_UpDownCylinder_Up_Fail,
        TransferFixture_UpDownCylinder_Down_Fail,
        TransferFixture_ClampCylinder_Clamp_Fail,
        TransferFixture_ClampCylinder_UnClamp_Fail,

        //Detach
        Detach = 10000,
        DetachFail = 10001,
        Detach_FixCylinder_Backward_Fail,
        Detach_FixCylinder_Forward_Fail,
        Detach_DetachCylinder1_Up_Fail,
        Detach_DetachCylinder1_Down_Fail,
        Detach_DetachCylinder2_Up_Fail,
        Detach_DetachCylinder2_Down_Fail,

        //Remove Film
        RemoveFilm = 11000,
        RemoveFilm_FixCylinder_Backward_Fail,
        RemoveFilm_FixCylinder_Forward_Fail,
        RemoveFilm_TransferCylinder_Backward_Fail,
        RemoveFilm_TransferCylinder_Forward_Fail,
        RemoveFilm_UpDownCylinder1_Up_Fail,
        RemoveFilm_UpDownCylinder1_Down_Fail,
        RemoveFilm_UpDownCylinder2_Up_Fail,
        RemoveFilm_UpDownCylinder2_Down_Fail,
        RemoveFilm_PusherCylinder1_Up_Fail,
        RemoveFilm_PusherCylinder1_Down_Fail,
        RemoveFilm_PusherCylinder2_Up_Fail,
        RemoveFilm_PusherCylinder2_Down_Fail,
        RemoveFilm_ClampCylinder_Clamp_Fail,
        RemoveFilm_ClampCylinder_UnClamp_Fail,

        //Glass Transfer
        GlassTransfer = 12000,
        GlassTransfer_UpDownCylinder_Up_Fail,
        GlassTransfer_UpDownCylinder_Down_Fail,

        //Align Glass Left
        GlassAlignLeft = 13000,
        GlassAlignLeft_AlignCylinder_Up_Fail,
        GlassAlignLeft_AlignCylinder_Down_Fail,
        GlassAlignLeft_GlassNotDetect,

        //Align Glass Right
        GlassAlignRight = 14000,
        GlassAlignRight_AlignCylinder_Up_Fail,
        GlassAlignRight_AlignCylinder_Down_Fail,
        GlassAlignRight_GlassNotDetect,

        //Transfer In Shuttle Left
        TransferInShuttleLeft = 15000,

        //Transfer In Shuttle Right
        TransferInShuttleRight = 16000,

        //WET Clean Left
        WETCleanLeft = 17000,
        WETCleanLeft_PusherCylinder_Up_Fail,
        WETCleanLeft_PusherCylinder_Down_Fail,
        WETCleanLeft_FeedingRoller_NotDetect,
        WETCleanLeft_WiperClean_NotDetect,

        //WET Clean Right
        WETCleanRight = 18000,
        WETCleanRight_PusherCylinder_Up_Fail,
        WETCleanRight_PusherCylinder_Down_Fail,
        WETCleanRight_FeedingRoller_NotDetect,
        WETCleanRight_WiperClean_NotDetect,

        //AF Clean Left
        AFCleanLeft = 19000,
        AFCleanLeft_PusherCylinder_Up_Fail,
        AFCleanLeft_PusherCylinder_Down_Fail,
        AFCleanLeft_FeedingRoller_NotDetect,
        AFCleanLeft_WiperClean_NotDetect,

        //AF Clean Right
        AFCleanRight = 20000,
        AFCleanRight_PusherCylinder_Up_Fail,
        AFCleanRight_PusherCylinder_Down_Fail,
        AFCleanRight_FeedingRoller_NotDetect,
        AFCleanRight_WiperClean_NotDetect,

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
