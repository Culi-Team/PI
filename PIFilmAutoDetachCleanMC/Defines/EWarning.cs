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
        InitializeTimeout,

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
        Vinylclean_Full_Detect,
        Vinylclean_Vinyl_Not_Detect,

        //Robot Load
        RobotLoad = 7000,
        RobotLoad_Connect_Fail,
        RobotLoad_Programing_Not_Running,
        RobotLoadOriginFixtureDetect,
        RobotLoad_Home_Manual_By_TeachingPendant,
        RobotLoad_Cylinder_Clamp_Fail,
        RobotLoad_Cylinder_UnClamp_Fail,
        RobotLoad_Cylinder_Backward_Fail,
        RobotLoad_Cylinder_Forward_Fail,

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
        RemoveFilm_Full_Tape,

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
        TransferInShuttleLeft_RotateCylinder_180D_Fail,
        TransferInShuttleLeft_RotateCylinder_0D_Fail,

        //Transfer In Shuttle Right
        TransferInShuttleRight = 16000,
        TransferInShuttleRight_RotateCylinder_180D_Fail,
        TransferInShuttleRight_RotateCylinder_0D_Fail,

        //WET Clean Left
        WETCleanLeft = 17000,
        WETCleanLeft_PusherCylinder_Up_Fail,
        WETCleanLeft_PusherCylinder_Down_Fail,
        WETCleanLeft_FeedingRoller_NotDetect,
        WETCleanLeft_WiperClean_NotDetect,
        WETCleanLeft_PumpLeak_Detect,
        WETCleanLeft_AlcoholLeak_Detect,
        WETCleanLeft_ClampCylinder_Clamp_Fail,
        WETCleanLeft_ClampCylinder_Unclamp_Fail,
        WETCleanLeft_Vacuum_Detect_Fail,
        Shuttle_Left_XAxis_Collision_Detect,
        WETCleanLeft_BrushCylinder_Up_Fail,
        WETCleanLeft_BrushCylinder_Down_Fail,

        //WET Clean Right
        WETCleanRight = 18000,
        WETCleanRight_PusherCylinder_Up_Fail,
        WETCleanRight_PusherCylinder_Down_Fail,
        WETCleanRight_FeedingRoller_NotDetect,
        WETCleanRight_WiperClean_NotDetect,
        WETCleanRight_PumpLeak_Detect,
        WETCleanRight_AlcoholLeak_Detect,
        WETCleanRight_ClampCylinder_Clamp_Fail,
        WETCleanRight_ClampCylinder_Unclamp_Fail,
        WETCleanRight_Vacuum_Detect_Fail,
        Shuttle_Right_XAxis_Collision_Detect,
        WETCleanRight_BrushCylinder_Up_Fail,
        WETCleanRight_BrushCylinder_Down_Fail,

        //Transfer Rotation Left
        TransferRotationLeft = 19000,
        TransferRotationLeft_Cylinder_Backward_Fail,
        TransferRotationLeft_Cylinder_Forward_Fail,
        TransferRotationLeft_RotationCylinder_0D_Fail,
        TransferRotationLeft_RotationCylinder_180D_Fail,
        TransferRotationLeft_Cylinder_Up_Fail,
        TransferRotationLeft_Cylinder_Down_Fail,
        TransferRotationLeft_RotateVac_Check_Fail,
        TransferRotationLeft_GlassVacAfterRotate_Check_Fail,
        TransferRotationLeft_GlassVacuum1_Check_Fail,
        TransferRotationLeft_GlassVacuum2_Check_Fail,
        TransferRotationLeft_GlassVacuumRotate_Check_Fail,

        //Transfer Rotation Right
        TransferRotationRight = 20000,
        TransferRotationRight_Cylinder_Backward_Fail,
        TransferRotationRight_Cylinder_Forward_Fail,
        TransferRotationRight_RotationCylinder_0D_Fail,
        TransferRotationRight_RotationCylinder_180D_Fail,
        TransferRotationRight_Cylinder_Up_Fail,
        TransferRotationRight_Cylinder_Down_Fail,
        TransferRotationRight_RotateVac_Check_Fail,
        TransferRotationRight_GlassVacAfterRotate_Check_Fail,
        TransferRotationRight_GlassVacuum1_Check_Fail,
        TransferRotationRight_GlassVacuum2_Check_Fail,
        TransferRotationRight_GlassVacuumRotate_Check_Fail,

        //AF Clean Left
        AFCleanLeft = 21000,
        AFCleanLeft_PusherCylinder_Up_Fail,
        AFCleanLeft_PusherCylinder_Down_Fail,
        AFCleanLeft_FeedingRoller_NotDetect,
        AFCleanLeft_WiperClean_NotDetect,
        AFCleanLeft_PumpLeak_Detect,
        AFCleanLeft_AlcoholLeak_Detect,
        AFCleanLeft_Vacuum_Detect_Fail,
        AFCleanLeft_BrushCylinder_Up_Fail,
        AFCleanLeft_BrushCylinder_Down_Fail,

        //AF Clean Right
        AFCleanRight = 22000,
        AFCleanRight_PusherCylinder_Up_Fail,
        AFCleanRight_PusherCylinder_Down_Fail,
        AFCleanRight_FeedingRoller_NotDetect,
        AFCleanRight_WiperClean_NotDetect,
        AFCleanRight_PumpLeak_Detect,
        AFCleanRight_AlcoholLeak_Detect,
        AFCleanRight_Vacuum_Detect_Fail,
        AFCleanRight_BrushCylinder_Up_Fail,
        AFCleanRight_BrushCylinder_Down_Fail,

        //Unload Glass Left
        UnloadTransferLeft = 23000,

        //Unload Glass Right
        UnloadTransferRight = 24000,

        //Unload Align
        UnloadAlign = 25000,
        UnloadAlign_AlignCylinder_Up_Fail,
        UnloadAlign_AlignCylinder_Down_Fail,
        UnloadAlign_Vacuum_Fail,
        UnloadAlign_Glass_NotDetect,

        //Robot Unload
        RobotUnload = 26000,
        RobotUnload_Cylinder_Up_Fail,
        RobotUnload_Cylinder_Down_Fail,
        RobotUnload_Vacuum_On_Fail,
        RobotUnload_Pick_Fail,
    }
}
