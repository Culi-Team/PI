using EQX.Core.InOut;
using EQX.InOut;
using PIFilmAutoDetachCleanMC.Defines.Cylinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder
{
    public class Cylinders
    {
        // In CST
        public ICylinder InCstStopperUpDown { get; }

        // Out CST
        public ICylinder OutCstStopperUpDown { get; }

        // In CST Work
        public ICylinder InCstFixCyl1FwBw { get; }
        public ICylinder InCstFixCyl2FwBw { get; }
        public ICylinder InCstTiltCylUpDown { get; }

        // Buffer CV Work
        public ICylinder BufferCvStopper1UpDown { get; }
        public ICylinder BufferCvStopper2UpDown { get; }

        // In CV Support
        public ICylinder InCvSupportUpDown { get; }

        // In CV Support Buffer
        public ICylinder InCvSupportBufferUpDown { get; }

        // Out CV Support Buffer
        public ICylinder OutCvSupportBufferUpDown { get; }

        // Out CV Support
        public ICylinder OutCvSupportUpDown { get; }

        // Out CST Work
        public ICylinder OutCstFixCyl1FwBw { get; }
        public ICylinder OutCstFixCyl2FwBw { get; }
        public ICylinder OutCstTiltCylUpDown { get; }

        // Robot 1 Load/Unload
        public ICylinder RobotFixtureAlign1FwBw { get; }
        public ICylinder RobotFixtureAlign2FwBw { get; }
        public ICylinder RobotFixture1ClampUnclamp { get; }
        public ICylinder RobotFixture2ClampUnclamp { get; }

        // Vinyl Clean
        public ICylinder VinylCleanFixture1ClampUnclamp { get; }
        public ICylinder VinylCleanFixture2ClampUnclamp { get; }
        public ICylinder VinylCleanPusherRollerUpDown { get; }

        // Transfer Y Fixture
        public ICylinder TransferFixtureUpDown { get; }
        public ICylinder TransferFixture1ClampUnclamp { get; }
        public ICylinder TransferFixture2ClampUnclamp { get; }
        public ICylinder TransferFixture3ClampUnclamp { get; }
        public ICylinder TransferFixture4ClampUnclamp { get; }

        // Detach Glass
        public ICylinder DetachCyl1UpDown { get; }
        public ICylinder DetachCyl2UpDown { get; }
        public ICylinder DetachFixFixtureCyl1FwBw { get; }
        public ICylinder DetachFixFixtureCyl2FwBw { get; }
        public ICylinder DetachFixFixtureCyl3FwBw { get; }
        public ICylinder DetachFixFixtureCyl4FwBw { get; }

        // Remove Zone
        public ICylinder RemoveZoneTrCylFwBw { get; }
        public ICylinder RemoveZoneZCyl1UpDown { get; }
        public ICylinder RemoveZoneZCyl2UpDown { get; }
        public ICylinder RemoveZoneCyl1ClampUnclamp { get; }
        public ICylinder RemoveZoneCyl2ClampUnclamp { get; }
        public ICylinder RemoveZoneCyl3ClampUnclamp { get; }
        public ICylinder RemoveZonePusherCyl1UpDown { get; }
        public ICylinder RemoveZonePusherCyl2UpDown { get; }
        public ICylinder RemoveZoneFixCyl1FwBw { get; }
        public ICylinder RemoveZoneFixCyl2FwBw { get; }
        public ICylinder RemoveZoneFixCyl3FwBw { get; }
        public ICylinder RemoveZoneFixCyl4FwBw { get; }

        // In Shuttle
        public ICylinder TransferInShuttleLRotate { get; }
        public ICylinder TransferInShuttleRRotate { get; }

        // Align Fixture
        public ICylinder FixtureAlign1CylFwBw { get; }
        public ICylinder FixtureAlign2CylFwBw { get; }

        // Align Stage L/R
        public ICylinder AlignStageLBrushCylUpDown { get; }
        public ICylinder AlignStageL1AlignUnalign { get; }
        public ICylinder AlignStageL2AlignUnalign { get; }
        public ICylinder AlignStageL3AlignUnalign { get; }
        public ICylinder AlignStageRBrushCylUpDown { get; }
        public ICylinder AlignStageR1AlignUnalign { get; }
        public ICylinder AlignStageR2AlignUnalign { get; }
        public ICylinder AlignStageR3AlignUnalign { get; }

        // Glass Transfer
        public ICylinder GlassTransferCyl1UpDown { get; }
        public ICylinder GlassTransferCyl2UpDown { get; }
        public ICylinder GlassTransferCyl3UpDown { get; }

        // Wet Clean
        public ICylinder WetCleanPusherRightUpDown { get; }
        public ICylinder WetCleanPusherLeftUpDown { get; }

        // Transfer Rotate
        public ICylinder TrRotateRightClampUnclamp { get; }
        public ICylinder TrRotateRightRotate { get; }
        public ICylinder TrRotateRightFwBw { get; }
        public ICylinder TrRotateLeftClampUnclamp { get; }
        public ICylinder TrRotateLeftRotate { get; }
        public ICylinder TrRotateLeftFwBw { get; }

        // AF Clean
        public ICylinder AFCleanPusherRightUpDown { get; }
        public ICylinder AFCleanPusherLeftUpDown { get; }

        // Robot 2 Unload
        public ICylinder UnloadRobotCyl1UpDown { get; }
        public ICylinder UnloadRobotCyl2UpDown { get; }
        public ICylinder UnloadRobotCyl3UpDown { get; }
        public ICylinder UnloadRobotCyl4UpDown { get; }

        // Unload Stage
        public ICylinder UnloadAlignCyl1UpDown { get; }
        public ICylinder UnloadAlignCyl2UpDown { get; }
        public ICylinder UnloadAlignCyl3UpDown { get; }
        public ICylinder UnloadAlignCyl4UpDown { get; }

        public Cylinders(ICylinderFactory cylinderFactory, Inputs inputs, Outputs outputs)
        {
            _cylinderFactory = cylinderFactory;
            _inputs = inputs;
            _outputs = outputs;

            InCstStopperUpDown = _cylinderFactory
                .Create(_inputs.InCstStopperUp, _inputs.InCstStopperDown, _outputs.InCstStopperUp, _outputs.InCstStopperDown)
                .SetIdentity((int)ECylinder.InCstStopperUpDown, ECylinder.InCstStopperUpDown.ToString());
            InCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            OutCstStopperUpDown = _cylinderFactory
                .Create(_inputs.OutCstStopperUp, _inputs.OutCstStopperDown, _outputs.OutCstStopperUp, _outputs.OutCstStopperDown)
                .SetIdentity((int)ECylinder.OutCstStopperUpDown, ECylinder.OutCstStopperUpDown.ToString());
            OutCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            InCstFixCyl1FwBw = _cylinderFactory
                .Create(_inputs.InCstFixCyl1Fw, _inputs.InCstFixCyl1Bw, _outputs.InCstFixCyl1Fw, _outputs.InCstFixCyl1Bw)
                .SetIdentity((int)ECylinder.InCstFixCyl1FwBw, ECylinder.InCstFixCyl1FwBw.ToString());
            InCstFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            InCstFixCyl2FwBw = _cylinderFactory
                .Create(_inputs.InCstFixCyl2Fw, _inputs.InCstFixCyl2Bw, _outputs.InCstFixCyl2Fw, _outputs.InCstFixCyl2Bw)
                .SetIdentity((int)ECylinder.InCstFixCyl2FwBw, ECylinder.InCstFixCyl2FwBw.ToString());
            InCstFixCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            InCstTiltCylUpDown = _cylinderFactory
                .Create(_inputs.InCstTiltCylUp, _inputs.InCstTiltCylDown, _outputs.InCstTiltCylUp, _outputs.InCstTiltCylDown)
                .SetIdentity((int)ECylinder.InCstTiltCylUpDown, ECylinder.InCstTiltCylUpDown.ToString());
            InCstTiltCylUpDown.CylinderType = ECylinderType.UpDown;

            BufferCvStopper1UpDown = _cylinderFactory
                .Create(_inputs.BufferCvStopper1Up, _inputs.BufferCvStopper1Down, _outputs.BufferCvStopper1Up, _outputs.BufferCvStopper1Down)
                .SetIdentity((int)ECylinder.BufferCvStopper1UpDown, ECylinder.BufferCvStopper1UpDown.ToString());
            BufferCvStopper1UpDown.CylinderType = ECylinderType.UpDown;

            BufferCvStopper2UpDown = _cylinderFactory
                .Create(_inputs.BufferCvStopper2Up, _inputs.BufferCvStopper2Down, _outputs.BufferCvStopper2Up, _outputs.BufferCvStopper2Down)
                .SetIdentity((int)ECylinder.BufferCvStopper2UpDown, ECylinder.BufferCvStopper2UpDown.ToString());
            BufferCvStopper2UpDown.CylinderType = ECylinderType.UpDown;

            InCvSupportUpDown = _cylinderFactory
                .Create(_inputs.InCvSupportUp, _inputs.InCvSupportDown, _outputs.InCvSupportUp, _outputs.InCvSupportDown)
                .SetIdentity((int)ECylinder.InCvSupportUpDown, ECylinder.InCvSupportUpDown.ToString());
            InCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            InCvSupportBufferUpDown = _cylinderFactory
                .Create(_inputs.InCvSupportBufferUp, _inputs.InCvSupportBufferDown, _outputs.InCvSupportBufferUp, _outputs.InCvSupportBufferDown)
                .SetIdentity((int)ECylinder.InCvSupportBufferUpDown, ECylinder.InCvSupportBufferUpDown.ToString());
            InCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            OutCvSupportBufferUpDown = _cylinderFactory
                .Create(_inputs.OutCvSupportBufferUp, _inputs.OutCvSupportBufferDown, _outputs.OutCvSupportBufferUp, _outputs.OutCvSupportBufferDown)
                .SetIdentity((int)ECylinder.OutCvSupportBufferUpDown, ECylinder.OutCvSupportBufferUpDown.ToString());
            OutCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            OutCvSupportUpDown = _cylinderFactory
                .Create(_inputs.OutCvSupportUp, _inputs.OutCvSupportDown, _outputs.OutCvSupportUp, _outputs.OutCvSupportDown)
                .SetIdentity((int)ECylinder.OutCvSupportUpDown, ECylinder.OutCvSupportUpDown.ToString());
            OutCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            OutCstFixCyl1FwBw = _cylinderFactory
                .Create(_inputs.OutCstFixCyl1Fw, _inputs.OutCstFixCyl1Bw, _outputs.OutCstFixCyl1Fw, _outputs.OutCstFixCyl1Bw)
                .SetIdentity((int)ECylinder.OutCstFixCyl1FwBw, ECylinder.OutCstFixCyl1FwBw.ToString());
            OutCstFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            OutCstFixCyl2FwBw = _cylinderFactory
                .Create(_inputs.OutCstFixCyl2Fw, _inputs.OutCstFixCyl2Bw, _outputs.OutCstFixCyl2Fw, _outputs.OutCstFixCyl2Bw)
                .SetIdentity((int)ECylinder.OutCstFixCyl2FwBw, ECylinder.OutCstFixCyl2FwBw.ToString());

            OutCstTiltCylUpDown = _cylinderFactory
                .Create(_inputs.OutCstTiltCylUp, _inputs.OutCstTiltCylDown, _outputs.OutCstTiltCylUp, _outputs.OutCstTiltCylDown)
                .SetIdentity((int)ECylinder.OutCstTiltCylUpDown, ECylinder.OutCstTiltCylUpDown.ToString());
            OutCstTiltCylUpDown.CylinderType = ECylinderType.UpDown;

            RobotFixtureAlign1FwBw = _cylinderFactory
                .Create(_inputs.RobotFixtureAlign1Fw, _inputs.RobotFixtureAlign1Bw, _outputs.RobotFixtureAlignFw, _outputs.RobotFixtureAlignBw)
                .SetIdentity((int)ECylinder.RobotFixtureAlign1FwBw, ECylinder.RobotFixtureAlign1FwBw.ToString());
            RobotFixtureAlign1FwBw.CylinderType = ECylinderType.ForwardBackward;

            RobotFixtureAlign2FwBw = _cylinderFactory
                .Create(_inputs.RobotFixtureAlign2Fw, _inputs.RobotFixtureAlign2Bw, _outputs.RobotFixtureAlignFw, _outputs.RobotFixtureAlignBw)
                .SetIdentity((int)ECylinder.RobotFixtureAlign2FwBw, ECylinder.RobotFixtureAlign2FwBw.ToString());
            RobotFixtureAlign2FwBw.CylinderType = ECylinderType.ForwardBackward;

            RobotFixture1ClampUnclamp = _cylinderFactory
                .Create(_inputs.RobotFixture1Clamp, _inputs.RobotFixture1Unclamp, _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotFixture1ClampUnclamp, ECylinder.RobotFixture1ClampUnclamp.ToString());
            RobotFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RobotFixture2ClampUnclamp = _cylinderFactory
                .Create(_inputs.RobotFixture2Clamp, _inputs.RobotFixture2Unclamp, _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotFixture2ClampUnclamp, ECylinder.RobotFixture2ClampUnclamp.ToString());
            RobotFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanFixture1ClampUnclamp = _cylinderFactory
                .Create(_inputs.VinylCleanFixture1Clamp, _inputs.VinylCleanFixture1Unclamp, _outputs.VinylCleanFixture1Clamp, _outputs.VinylCleanFixture1Unclamp)
                .SetIdentity((int)ECylinder.VinylCleanFixture1ClampUnclamp, ECylinder.VinylCleanFixture1ClampUnclamp.ToString());
            VinylCleanFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanFixture2ClampUnclamp = _cylinderFactory
                .Create(_inputs.VinylCleanFixture2Clamp, _inputs.VinylCleanFixture2Unclamp, _outputs.VinylCleanFixture2Clamp, _outputs.VinylCleanFixture2Unclamp)
                .SetIdentity((int)ECylinder.VinylCleanFixture2ClampUnclamp, ECylinder.VinylCleanFixture2ClampUnclamp.ToString());
            VinylCleanFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanPusherRollerUpDown = _cylinderFactory
                .Create(_inputs.VinylCleanPusherRollerUp, _inputs.VinylCleanPusherRollerDown, _outputs.VinylCleanPusherRollerUp, _outputs.VinylCleanPusherRollerDown)
                .SetIdentity((int)ECylinder.VinylCleanPusherRollerUpDown, ECylinder.VinylCleanPusherRollerUpDown.ToString());
            VinylCleanPusherRollerUpDown.CylinderType = ECylinderType.UpDown;

            TransferFixtureUpDown = _cylinderFactory
                .Create(_inputs.TransferFixtureUp, _inputs.TransferFixtureDown, _outputs.TransferFixtureUp, _outputs.TransferFixtureDown)
                .SetIdentity((int)ECylinder.TransferFixtureUpDown, ECylinder.TransferFixtureUpDown.ToString());
            TransferFixtureUpDown.CylinderType = ECylinderType.UpDown;

            TransferFixture1ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture1Clamp, _inputs.TransferFixture1Unclamp, _outputs.TransferFixture1Clamp, _outputs.TransferFixture1Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture1ClampUnclamp, ECylinder.TransferFixture1ClampUnclamp.ToString());
            TransferFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture2ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture2Clamp, _inputs.TransferFixture2Unclamp, _outputs.TransferFixture2Clamp, _outputs.TransferFixture2Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture2ClampUnclamp, ECylinder.TransferFixture2ClampUnclamp.ToString());
            TransferFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture3ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture3Clamp, _inputs.TransferFixture3Unclamp, _outputs.TransferFixture3Clamp, _outputs.TransferFixture3Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture3ClampUnclamp, ECylinder.TransferFixture3ClampUnclamp.ToString());
            TransferFixture3ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture4ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture4Clamp, _inputs.TransferFixture4Unclamp, _outputs.TransferFixture4Clamp, _outputs.TransferFixture4Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture4ClampUnclamp, ECylinder.TransferFixture4ClampUnclamp.ToString());
            TransferFixture4ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            DetachCyl1UpDown = _cylinderFactory
                .Create(_inputs.DetachCyl1Up, _inputs.DetachCyl1Down, _outputs.DetachCyl1Up, _outputs.DetachCyl1Down)
                .SetIdentity((int)ECylinder.DetachCyl1UpDown, ECylinder.DetachCyl1UpDown.ToString());
            DetachCyl1UpDown.CylinderType = ECylinderType.UpDown;

            DetachCyl2UpDown = _cylinderFactory
                .Create(_inputs.DetachCyl2Up, _inputs.DetachCyl2Down, _outputs.DetachCyl2Up, _outputs.DetachCyl2Down)
                .SetIdentity((int)ECylinder.DetachCyl2UpDown, ECylinder.DetachCyl2UpDown.ToString());
            DetachCyl2UpDown.CylinderType = ECylinderType.UpDown;

            DetachFixFixtureCyl1FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl1Fw, _inputs.DetachFixFixtureCyl1Bw, _outputs.DetachFixFixtureCyl1Fw, _outputs.DetachFixFixtureCyl1Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl1FwBw, ECylinder.DetachFixFixtureCyl1FwBw.ToString());
            DetachFixFixtureCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl2FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl2Fw, _inputs.DetachFixFixtureCyl2Bw, _outputs.DetachFixFixtureCyl2Fw, _outputs.DetachFixFixtureCyl2Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl2FwBw, ECylinder.DetachFixFixtureCyl2FwBw.ToString());
            DetachFixFixtureCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl3FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl3Fw, _inputs.DetachFixFixtureCyl3Bw, _outputs.DetachFixFixtureCyl3Fw, _outputs.DetachFixFixtureCyl3Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl3FwBw, ECylinder.DetachFixFixtureCyl3FwBw.ToString());
            DetachFixFixtureCyl3FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl4FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl4Fw, _inputs.DetachFixFixtureCyl4Bw, _outputs.DetachFixFixtureCyl4Fw, _outputs.DetachFixFixtureCyl4Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl4FwBw, ECylinder.DetachFixFixtureCyl4FwBw.ToString());
            DetachFixFixtureCyl4FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneTrCylFwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneTrCylFw, _inputs.RemoveZoneTrCylBw, _outputs.RemoveZoneTrCylFw, _outputs.RemoveZoneTrCylBw)
                .SetIdentity((int)ECylinder.RemoveZoneTrCylFwBw, ECylinder.RemoveZoneTrCylFwBw.ToString());
            RemoveZoneTrCylFwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneZCyl1UpDown = _cylinderFactory
                .Create(_inputs.RemoveZoneZCyl1Up, _inputs.RemoveZoneZCyl1Down, _outputs.RemoveZoneZCyl1Up, _outputs.RemoveZoneZCyl1Down)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl1UpDown, ECylinder.RemoveZoneZCyl1UpDown.ToString());
            RemoveZoneZCyl1UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneZCyl2UpDown = _cylinderFactory
                .Create(_inputs.RemoveZoneZCyl2Up, _inputs.RemoveZoneZCyl2Down, _outputs.RemoveZoneZCyl2Up, _outputs.RemoveZoneZCyl2Down)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl2UpDown, ECylinder.RemoveZoneZCyl2UpDown.ToString());
            RemoveZoneZCyl2UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneCyl1ClampUnclamp = _cylinderFactory
                .Create(_inputs.RemoveZoneCyl1Clamp, _inputs.RemoveZoneCyl1Unclamp, _outputs.RemoveZoneCylClamp, _outputs.RemoveZoneCylUnclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl1ClampUnclamp, ECylinder.RemoveZoneCyl1ClampUnclamp.ToString());
            RemoveZoneCyl1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZoneCyl2ClampUnclamp = _cylinderFactory
                .Create(_inputs.RemoveZoneCyl2Clamp, _inputs.RemoveZoneCyl2Unclamp, _outputs.RemoveZoneCylClamp, _outputs.RemoveZoneCylUnclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl2ClampUnclamp, ECylinder.RemoveZoneCyl2ClampUnclamp.ToString());
            RemoveZoneCyl2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZoneCyl3ClampUnclamp = _cylinderFactory
                .Create(_inputs.RemoveZoneCyl3Clamp, _inputs.RemoveZoneCyl3Unclamp, _outputs.RemoveZoneCylClamp, _outputs.RemoveZoneCylUnclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl3ClampUnclamp, ECylinder.RemoveZoneCyl3ClampUnclamp.ToString());
            RemoveZoneCyl3ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZonePusherCyl1UpDown = _cylinderFactory
                .Create(_inputs.RemoveZonePusherCyl1Up, _inputs.RemoveZonePusherCyl1Down, _outputs.RemoveZonePusherCyl1Up, _outputs.RemoveZonePusherCyl1Down)
                .SetIdentity((int)ECylinder.RemoveZonePusherCyl1UpDown, ECylinder.RemoveZonePusherCyl1UpDown.ToString());
            RemoveZonePusherCyl1UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZonePusherCyl2UpDown = _cylinderFactory
                .Create(_inputs.RemoveZonePusherCyl2Up, _inputs.RemoveZonePusherCyl2Down, _outputs.RemoveZonePusherCyl2Up, _outputs.RemoveZonePusherCyl2Down)
                .SetIdentity((int)ECylinder.RemoveZonePusherCyl2UpDown, ECylinder.RemoveZonePusherCyl2UpDown.ToString());
            RemoveZonePusherCyl2UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneFixCyl1FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl1Fw, _inputs.RemoveZoneFixCyl1Bw, _outputs.RemoveZoneFixCyl1Fw, _outputs.RemoveZoneFixCyl1Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl1FwBw, ECylinder.RemoveZoneFixCyl1FwBw.ToString());
            RemoveZoneFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl2FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl2Fw, _inputs.RemoveZoneFixCyl2Bw, _outputs.RemoveZoneFixCyl2Fw, _outputs.RemoveZoneFixCyl2Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl2FwBw, ECylinder.RemoveZoneFixCyl2FwBw.ToString());
            RemoveZoneFixCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl3FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl3Fw, _inputs.RemoveZoneFixCyl3Bw, _outputs.RemoveZoneFixCyl3Fw, _outputs.RemoveZoneFixCyl3Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl3FwBw, ECylinder.RemoveZoneFixCyl3FwBw.ToString());
            RemoveZoneFixCyl3FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl4FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl4Fw, _inputs.RemoveZoneFixCyl4Bw, _outputs.RemoveZoneFixCyl4Fw, _outputs.RemoveZoneFixCyl4Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl4FwBw, ECylinder.RemoveZoneFixCyl4FwBw.ToString());
            RemoveZoneFixCyl4FwBw.CylinderType = ECylinderType.ForwardBackward;

            TransferInShuttleLRotate = _cylinderFactory
                .Create(_inputs.TransferInShuttleL0Degree, _inputs.TransferInShuttleL180Degree, _outputs.TransferInShuttleL0Degree, _outputs.TransferInShuttleL180Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleLRotate, ECylinder.TransferInShuttleLRotate.ToString());
            TransferInShuttleLRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            TransferInShuttleRRotate = _cylinderFactory
                .Create(_inputs.TransferInShuttleR0Degree, _inputs.TransferInShuttleR180Degree, _outputs.TransferInShuttleR0Degree, _outputs.TransferInShuttleR180Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleRRotate, ECylinder.TransferInShuttleRRotate.ToString());
            TransferInShuttleRRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            FixtureAlign1CylFwBw = _cylinderFactory
                .Create(_inputs.FixtureAlign1CylFw, _inputs.FixtureAlign1CylBw, _outputs.FixtureAlign1CylFw, _outputs.FixtureAlign1CylBw)
                .SetIdentity((int)ECylinder.FixtureAlign1CylFwBw, ECylinder.FixtureAlign1CylFwBw.ToString());
            FixtureAlign1CylFwBw.CylinderType = ECylinderType.ForwardBackward;

            FixtureAlign2CylFwBw = _cylinderFactory
                .Create(_inputs.FixtureAlign2CylFw, _inputs.FixtureAlign2CylBw, _outputs.FixtureAlign2CylFw, _outputs.FixtureAlign2CylBw)
                .SetIdentity((int)ECylinder.FixtureAlign2CylFwBw, ECylinder.FixtureAlign2CylFwBw.ToString());
            FixtureAlign2CylFwBw.CylinderType = ECylinderType.ForwardBackward;

            AlignStageLBrushCylUpDown = _cylinderFactory
                .Create(_inputs.AlignStageLBrushCylUp, _inputs.AlignStageLBrushCylDown, _outputs.AlignStageLBrushCylUp, _outputs.AlignStageLBrushCylDown)
                .SetIdentity((int)ECylinder.AlignStageLBrushCylUpDown, ECylinder.AlignStageLBrushCylUpDown.ToString());
            AlignStageLBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageL1AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageL1Align, _inputs.AlignStageL1Unalign, _outputs.AlignStageL1Align, _outputs.AlignStageL1Unalign)
                .SetIdentity((int)ECylinder.AlignStageL1AlignUnalign, ECylinder.AlignStageL1AlignUnalign.ToString());
            AlignStageL1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL2AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageL2Align, _inputs.AlignStageL2Unalign, _outputs.AlignStageL2Align, _outputs.AlignStageL2Unalign)
                .SetIdentity((int)ECylinder.AlignStageL2AlignUnalign, ECylinder.AlignStageL2AlignUnalign.ToString());
            AlignStageL2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL3AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageL3Align, _inputs.AlignStageL3Unalign, _outputs.AlignStageL3Align, _outputs.AlignStageL3Unalign)
                .SetIdentity((int)ECylinder.AlignStageL3AlignUnalign, ECylinder.AlignStageL3AlignUnalign.ToString());
            AlignStageL3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageRBrushCylUpDown = _cylinderFactory
                .Create(_inputs.AlignStageRBrushCylUp, _inputs.AlignStageRBrushCylDown, _outputs.AlignStageRBrushCylUp, _outputs.AlignStageRBrushCylDown)
                .SetIdentity((int)ECylinder.AlignStageRBrushCylUpDown, ECylinder.AlignStageRBrushCylUpDown.ToString());
            AlignStageRBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageR1AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageR1Align, _inputs.AlignStageR1Unalign, _outputs.AlignStageR1Align, _outputs.AlignStageR1Unalign)
                .SetIdentity((int)ECylinder.AlignStageR1AlignUnalign, ECylinder.AlignStageR1AlignUnalign.ToString());
            AlignStageR1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR2AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageR2Align, _inputs.AlignStageR2Unalign, _outputs.AlignStageR2Align, _outputs.AlignStageR2Unalign)
                .SetIdentity((int)ECylinder.AlignStageR2AlignUnalign, ECylinder.AlignStageR2AlignUnalign.ToString());
            AlignStageR2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR3AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageR3Align, _inputs.AlignStageR3Unalign, _outputs.AlignStageR3Align, _outputs.AlignStageR3Unalign)
                .SetIdentity((int)ECylinder.AlignStageR3AlignUnalign, ECylinder.AlignStageR3AlignUnalign.ToString());
            AlignStageR3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            GlassTransferCyl1UpDown = _cylinderFactory
                .Create(_inputs.GlassTransferCyl1Up, _inputs.GlassTransferCyl1Down, _outputs.GlassTransferCyl1Up, _outputs.GlassTransferCyl1Down)
                .SetIdentity((int)ECylinder.GlassTransferCyl1UpDown, ECylinder.GlassTransferCyl1UpDown.ToString());
            GlassTransferCyl1UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl2UpDown = _cylinderFactory
                .Create(_inputs.GlassTransferCyl2Up, _inputs.GlassTransferCyl2Down, _outputs.GlassTransferCyl2Up, _outputs.GlassTransferCyl2Down)
                .SetIdentity((int)ECylinder.GlassTransferCyl2UpDown, ECylinder.GlassTransferCyl2UpDown.ToString());
            GlassTransferCyl2UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl3UpDown = _cylinderFactory
                .Create(_inputs.GlassTransferCyl3Up, _inputs.GlassTransferCyl3Down, _outputs.GlassTransferCyl3Up, _outputs.GlassTransferCyl3Down)
                .SetIdentity((int)ECylinder.GlassTransferCyl3UpDown, ECylinder.GlassTransferCyl3UpDown.ToString());
            GlassTransferCyl3UpDown.CylinderType = ECylinderType.UpDown;

            WetCleanPusherRightUpDown = _cylinderFactory
                .Create(_inputs.WetCleanPusherRightUp, _inputs.WetCleanPusherRightDown, _outputs.WetCleanPusherRightUp, _outputs.WetCleanPusherRightDown)
                .SetIdentity((int)ECylinder.WetCleanPusherRightUpDown, ECylinder.WetCleanPusherRightUpDown.ToString());
            WetCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanPusherLeftUpDown = _cylinderFactory
                .Create(_inputs.WetCleanPusherLeftUp, _inputs.WetCleanPusherLeftDown, _outputs.WetCleanPusherLeftUp, _outputs.WetCleanPusherLeftDown)
                .SetIdentity((int)ECylinder.WetCleanPusherLeftUpDown, ECylinder.WetCleanPusherLeftUpDown.ToString());
            WetCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            TrRotateRightClampUnclamp = _cylinderFactory
                .Create(_inputs.TrRotateRightClamp, _inputs.TrRotateRightUnclamp, _outputs.TrRotateRightClamp, _outputs.TrRotateRightUnclamp)
                .SetIdentity((int)ECylinder.TrRotateRightClampUnclamp, ECylinder.TrRotateRightClampUnclamp.ToString());
            TrRotateRightClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TrRotateRightRotate = _cylinderFactory
                .Create(_inputs.TrRotateLeft0Degree, _inputs.TrRotateRight180Degree, _outputs.TrRotateRight0Degree, _outputs.TrRotateRight180Degree)
                .SetIdentity((int)ECylinder.TrRotateRightRotate, ECylinder.TrRotateRightRotate.ToString());
            TrRotateRightRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            TrRotateRightFwBw = _cylinderFactory
                .Create(_inputs.TrRotateRightFw, _inputs.TrRotateRightBw, _outputs.TrRotateRightFw, _outputs.TrRotateRightBw)
                .SetIdentity((int)ECylinder.TrRotateRightFwBw, ECylinder.TrRotateRightFwBw.ToString());
            TrRotateRightFwBw.CylinderType = ECylinderType.ForwardBackward;

            TrRotateLeftClampUnclamp = _cylinderFactory
                .Create(_inputs.TrRotateLeftClamp, _inputs.TrRotateLeftUnclamp, _outputs.TrRotateLeftClamp, _outputs.TrRotateLeftUnclamp)
                .SetIdentity((int)ECylinder.TrRotateLeftClampUnclamp, ECylinder.TrRotateLeftClampUnclamp.ToString());
            TrRotateLeftClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TrRotateLeftRotate = _cylinderFactory
                .Create(_inputs.TrRotateLeft0Degree, _inputs.TrRotateLeft180Degree, _outputs.TrRotateLeft0Degree, _outputs.TrRotateLeft180Degree)
                .SetIdentity((int)ECylinder.TrRotateLeftRotate, ECylinder.TrRotateLeftRotate.ToString());
            TrRotateLeftRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            TrRotateLeftFwBw = _cylinderFactory
                .Create(_inputs.TrRotateLeftFw, _inputs.TrRotateLeftBw, _outputs.TrRotateLeftFw, _outputs.TrRotateLeftBw)
                .SetIdentity((int)ECylinder.TrRotateLeftFwBw, ECylinder.TrRotateLeftFwBw.ToString());
            TrRotateLeftFwBw.CylinderType = ECylinderType.ForwardBackward;

            AFCleanPusherRightUpDown = _cylinderFactory
                .Create(_inputs.AfCleanPusherRightUp, _inputs.AfCleanPusherRightDown, _outputs.AfCleanPusherRightUp, _outputs.AfCleanPusherRightDown)
                .SetIdentity((int)ECylinder.AFCleanPusherRightUpDown, ECylinder.AFCleanPusherRightUpDown.ToString());
            AFCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanPusherLeftUpDown = _cylinderFactory
                .Create(_inputs.AfCleanPusherLeftUp, _inputs.AfCleanPusherLeftDown, _outputs.AfCleanPusherLeftUp, _outputs.AfCleanPusherLeftDown)
                .SetIdentity((int)ECylinder.AFCleanPusherLeftUpDown, ECylinder.AFCleanPusherLeftUpDown.ToString());
            AFCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl1UpDown = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl1Up, _inputs.UnloadRobotCyl1Down, _outputs.UnloadRobotCyl1Up, _outputs.UnloadRobotCyl1Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl1UpDown, ECylinder.UnloadRobotCyl1UpDown.ToString());
            UnloadRobotCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl2UpDown = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl2Up, _inputs.UnloadRobotCyl2Down, _outputs.UnloadRobotCyl2Up, _outputs.UnloadRobotCyl2Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl2UpDown, ECylinder.UnloadRobotCyl2UpDown.ToString());
            UnloadRobotCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl3UpDown = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl3Up, _inputs.UnloadRobotCyl3Down, _outputs.UnloadRobotCyl3Up, _outputs.UnloadRobotCyl3Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl3UpDown, ECylinder.UnloadRobotCyl3UpDown.ToString());
            UnloadRobotCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl4UpDown = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl4Up, _inputs.UnloadRobotCyl4Down, _outputs.UnloadRobotCyl4Up, _outputs.UnloadRobotCyl4Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl4UpDown, ECylinder.UnloadRobotCyl4UpDown.ToString());
            UnloadRobotCyl4UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl1UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl1Up, _inputs.UnloadAlignCyl1Down, _outputs.UnloadAlignCyl1Up, _outputs.UnloadAlignCyl1Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl1UpDown, ECylinder.UnloadAlignCyl1UpDown.ToString());
            UnloadAlignCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl2UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl2Up, _inputs.UnloadAlignCyl2Down, _outputs.UnloadAlignCyl2Up, _outputs.UnloadAlignCyl2Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl2UpDown, ECylinder.UnloadAlignCyl2UpDown.ToString());
            UnloadAlignCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl3UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl3Up, _inputs.UnloadAlignCyl3Down, _outputs.UnloadAlignCyl3Up, _outputs.UnloadAlignCyl3Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl3UpDown, ECylinder.UnloadAlignCyl3UpDown.ToString());
            UnloadAlignCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl4UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl4Up, _inputs.UnloadAlignCyl4Down, _outputs.UnloadAlignCyl4Up, _outputs.UnloadAlignCyl4Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl4UpDown, ECylinder.UnloadAlignCyl4UpDown.ToString());
            UnloadAlignCyl4UpDown.CylinderType = ECylinderType.UpDown;
        }

        private readonly ICylinderFactory _cylinderFactory;
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
    }
}
