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
        InCstStopperUpDown,

        // Out CST
        OutCstStopperUpDown,

        // In CST Work
        InCstFixCyl1FwBw,
        InCstFixCyl2FwBw,
        InCstTiltCylUpDown,

        // Buffer CV Work
        BufferCvStopper1UpDown,
        BufferCvStopper2UpDown,

        // In CV Support
        InCvSupportUpDown,

        // In CV Support Buffer
        InCvSupportBufferUpDown,

        // Out CV Support Buffer
        OutCvSupportBufferUpDown,

        // Out CV Support
        OutCvSupportUpDown,

        // Out CST Work
        OutCstFixCyl1FwBw,
        OutCstFixCyl2FwBw,
        OutCstTiltCylUpDown,

        // Robot 1 Load/Unload
        RobotFixtureAlignFwBw,
        RobotFixture1ClampUnclamp,
        RobotFixture2ClampUnclamp,

        // Fixture Align (non-robot, per cylinder by inputs)
        FixtureAlignCyl1FwBw,
        FixtureAlignCyl2FwBw,

        // Vinyl Clean
        VinylCleanRollerFwBw,
        VinylCleanFixture1ClampUnclamp,
        VinylCleanFixture2ClampUnclamp,
        VinylCleanPusherRollerUpDown,

        // Transfer Y Fixture
        TransferFixtureUpDown,
        TransferFixture1_1ClampUnclamp,
        TransferFixture1_2ClampUnclamp,
        TransferFixture2_1ClampUnclamp,
        TransferFixture2_2ClampUnclamp,

        // Detach Glass
        DetachCyl1UpDown,
        DetachCyl2UpDown,
        DetachFixFixtureCyl1_1FwBw,
        DetachFixFixtureCyl1_2FwBw,
        DetachFixFixtureCyl2_1FwBw,
        DetachFixFixtureCyl2_2FwBw,

        // Remove Zone
        RemoveZoneTrCylFwBw,
        RemoveZoneZCyl1UpDown,
        RemoveZoneZCyl2UpDown,
        RemoveZoneCyl1ClampUnclamp,
        RemoveZoneCyl2ClampUnclamp,
        RemoveZoneCyl3ClampUnclamp,
        RemoveZonePusherCyl1UpDown,
        RemoveZonePusherCyl2UpDown,
        RemoveZoneFixCyl1_1FwBw,
        RemoveZoneFixCyl1_2FwBw,
        RemoveZoneFixCyl2_1FwBw,
        RemoveZoneFixCyl2_2FwBw,

        // In Shuttle (rotate 0°/180°)
        TransferInShuttleLRotate,
        TransferInShuttleRRotate,

        // Align Stage L/R
        AlignStageLBrushCylUpDown,
        AlignStageL1AlignUnalign,
        AlignStageL2AlignUnalign,
        AlignStageL3AlignUnalign,
        AlignStageRBrushCylUpDown,
        AlignStageR1AlignUnalign,
        AlignStageR2AlignUnalign,
        AlignStageR3AlignUnalign,

        // Glass Transfer 
        GlassTransferCyl1UpDown,
        GlassTransferCyl2UpDown,
        GlassTransferCyl3UpDown,

        // Wet Clean
        WetCleanPusherRightUpDown,
        WetCleanPusherLeftUpDown,
        WetCleanBrushRightUpDown,
        WetCleanBrushLeftUpDown,
        WetCleanRight1ClampUnclamp,
        WetCleanRight2ClampUnclamp,
        WetCleanLeft1ClampUnclamp,
        WetCleanLeft2ClampUnclamp,


        // Transfer Rotate
        TrRotateRightRotate,
        TrRotateRightFwBw,
        TrRotateLeftRotate,
        TrRotateLeftFwBw,
        TrRotateRightUpDown,
        TrRotateLeftUpDown,

        // AF Clean
        AFCleanPusherRightUpDown,
        AFCleanPusherLeftUpDown,
        AFCleanBrushRightUpDown,
        AFCleanBrushLeftUpDown,

        // Robot 2 Unload
        UnloadRobotCyl1UpDown,
        UnloadRobotCyl2UpDown,
        UnloadRobotCyl3UpDown,
        UnloadRobotCyl4UpDown,

        // Unload Stage
        UnloadAlignCyl1UpDown,
        UnloadAlignCyl2UpDown,
        UnloadAlignCyl3UpDown,
        UnloadAlignCyl4UpDown,

        //In Shuttle
        InShuttleR1AlignUnalign,
        InShuttleL1AlignUnalign,
        InShuttleR2AlignUnalign,
        InShuttleL2AlignUnalign,

        //OutShuttle
        OutShuttleR1AlignUnalign,
        OutShuttleL1AlignUnalign,
        OutShuttleR2AlignUnalign,
        OutShuttleL2AlignUnalign
    }
}
