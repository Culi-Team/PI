using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Cylinder
{
    public enum ECylinder
    {
        // In CST
        InCV_StopperUpDown,

        // Out CST
        OutCV_StopperUpDown,

        // In CST Work
        InWorkCV_AlignCyl1,
        InWorkCV_AlignCyl2,
        InWorkCV_TiltCyl,

        // Buffer CV Work
        BufferCV_StopperCyl1,
        BufferCV_StopperCyl2,

        // In CV Support
        InWorkCV_SupportCyl1,

        // In CV Support Buffer
        InWorkCV_SupportCyl2,

        // Out CV Support Buffer
        OutWorkCV_SupportCyl1,

        // Out CV Support
        OutWorkCV_SupportCyl2,

        // Out CST Work
        OutWorkCV_AlignCyl1,
        OutWorkCV_AlignCyl2,
        OutWorkCV_TiltCyl,

        // Robot 1 Load/Unload
        RobotLoad_AlignCyl,
        RobotLoad_ClampCyl1,
        RobotLoad_ClampCyl2,

        // Fixture Align (non-robot, per cylinder by inputs)
        FixtureAlign_AlignCyl1,
        FixtureAlign_AlignCyl2,

        // Vinyl Clean
        VinylClean_BwFwCyl,
        VinylClean_ClampCyl1,
        VinylClean_ClampCyl2,
        VinylClean_UpDownCyl,

        // Transfer Y Fixture
        TransferFixture_UpDownCyl,
        TransferFixture_ClampCyl1,
        TransferFixture_ClampCyl2,
        TransferFixture_ClampCyl3,
        TransferFixture_ClampCyl4,

        // Detach Glass
        Detach_UpDownCyl1,
        Detach_UpDownCyl2,
        Detach_ClampCyl1,
        Detach_ClampCyl2,
        Detach_ClampCyl3,
        Detach_ClampCyl4,

        // Remove Zone
        RemoveZone_TransferCyl,
        RemoveZone_UpDownCyl1,
        RemoveZone_UpDownCyl2,
        RemoveZone_FilmClampCyl1,
        RemoveZone_FilmClampCyl2,
        RemoveZone_FilmClampCyl3,
        RemoveZone_PusherCyl1,
        RemoveZone_PusherCyl2,
        RemoveZone_ClampCyl1,
        RemoveZone_ClampCyl2,
        RemoveZone_ClampCyl3,
        RemoveZone_ClampCyl4,

        // In Shuttle (rotate 0°/180°)
        TransferInShuttleL_RotateCyl,
        TransferInShuttleR_RotateCyl,

        // Align Stage L/R
        AlignStageL_BrushCyl,
        AlignStageL_AlignCyl1,
        AlignStageL_AlignCyl2,
        AlignStageL_AlignCyl3,
        AlignStageR_BrushCyl,
        AlignStageR_AlignCyl1,
        AlignStageR_AlignCyl2,
        AlignStageR_AlignCyl3,

        // Glass Transfer 
        GlassTransfer_UpDownCyl1,
        GlassTransfer_UpDownCyl2,
        GlassTransfer_UpDownCyl3,

        // Wet Clean
        WetCleanR_PusherCyl,
        WetCleanL_PusherCyl,
        WetCleanR_BrushCyl,
        WetCleanL_BrushCyl,
        WetCleanR_ClampCyl1,
        WetCleanR_ClampCyl2,
        WetCleanL_ClampCyl1,
        WetCleanL_ClampCyl2,

        // Transfer Rotate
        TransferRotationR_RotationCyl,
        TransferRotationR_BwFwCyl,
        TransferRotationL_RotationCyl,
        TransferRotationL_BwFwCyl,
        TransferRotationR_UpDownCyl,
        TransferRotationLeft_UpDownCyl,

        // AF Clean
        AFCleanR_PusherCyl,
        AFCleanL_PusherCyl,
        AFCleanR_BrushCyl,
        AFCleanL_BrushCyl,

        // Robot 2 Unload
        UnloadRobot_UpDownCyl1,
        UnloadRobot_UpDownCyl2,
        UnloadRobot_UpDownCyl3,
        UnloadRobot_UpDownCyl4,

        // Unload Stage
        UnloadAlign_UpDownCyl1,
        UnloadAlign_UpDownCyl2,
        UnloadAlign_UpDownCyl3,
        UnloadAlign_UpDownCyl4,

        //In Shuttle
        InShuttleR_ClampCyl1,
        InShuttleR_ClampCyl2,
        InShuttleL_ClampCyl1,
        InShuttleL_ClampCyl2,

        //OutShuttle
        OutShuttleR_ClampCyl1,
        OutShuttleR_ClampCyl2,
        OutShuttleL_ClampCyl1,
        OutShuttleL_ClampCyl2
    }
}
