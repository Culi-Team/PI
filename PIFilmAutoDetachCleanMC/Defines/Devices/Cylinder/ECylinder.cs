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
        RobotFixtureAlign1FwBw,
        RobotFixtureAlign2FwBw,
        RobotFixture1ClampUnclamp,
        RobotFixture2ClampUnclamp,

        // Fixture Align (non-robot, per cylinder by inputs)
        FixtureAlignCylFwBw,

        // Vinyl Clean
        VinylCleanRollerFwBw,
        VinylCleanFixture1ClampUnclamp,
        VinylCleanFixture2ClampUnclamp,
        VinylCleanPusherRollerUpDown,

        // Transfer Y Fixture
        TransferFixtureUpDown,
        TransferFixture1ClampUnclamp,
        TransferFixture2ClampUnclamp,

        // Detach Glass
        DetachCyl1UpDown,
        DetachCyl2UpDown,
        DetachFixFixtureCyl1FwBw,
        DetachFixFixtureCyl2FwBw,

        // Remove Zone
        RemoveZoneTrCylFwBw,
        RemoveZoneZCyl1UpDown,
        RemoveZoneZCyl2UpDown,
        RemoveZoneCyl1ClampUnclamp,
        RemoveZoneCyl2ClampUnclamp,
        RemoveZoneCyl3ClampUnclamp,
        RemoveZonePusherCyl1UpDown,
        RemoveZonePusherCyl2UpDown,
        RemoveZoneFixCyl1FwBw,
        RemoveZoneFixCyl2FwBw,

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
    }
}
