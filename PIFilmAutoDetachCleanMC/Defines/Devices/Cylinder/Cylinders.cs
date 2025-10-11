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

        // In CST Work (per input pair)
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

        // Out CST Work (per input pair)
        public ICylinder OutCstFixCyl1FwBw { get; }
        public ICylinder OutCstFixCyl2FwBw { get; }
        public ICylinder OutCstTiltCylUpDown { get; }

        // Robot 1 Load/Unload (per cylinder by inputs, shared outputs where applicable)
        public ICylinder RobotFixtureAlignFwBw { get; }
        public ICylinder RobotFixture1ClampUnclamp { get; }
        public ICylinder RobotFixture2ClampUnclamp { get; }

        // Fixture Align (machine fixture, per input pair, shared outputs)
        public ICylinder AlignFixtureCyl1FwBw { get; }
        public ICylinder AlignFixtureCyl2FwBw { get; }

        // Vinyl Clean
        public ICylinder VinylCleanRollerFwBw { get; }
        public ICylinder VinylCleanFixture1ClampUnclamp { get; }
        public ICylinder VinylCleanFixture2ClampUnclamp { get; }
        public ICylinder VinylCleanPusherRollerUpDown { get; }

        // Transfer Y Fixture
        public ICylinder TransferFixtureUpDown { get; }
        public ICylinder TransferFixture1_1ClampUnclamp { get; }
        public ICylinder TransferFixture1_2ClampUnclamp { get; }
        public ICylinder TransferFixture2_1ClampUnclamp { get; }
        public ICylinder TransferFixture2_2ClampUnclamp { get; }

        // Detach Glass
        public ICylinder DetachCyl1UpDown { get; }
        public ICylinder DetachCyl2UpDown { get; }
        public ICylinder DetachFixFixtureCyl1_1FwBw { get; }
        public ICylinder DetachFixFixtureCyl1_2FwBw { get; }
        public ICylinder DetachFixFixtureCyl2_1FwBw { get; }
        public ICylinder DetachFixFixtureCyl2_2FwBw { get; }
        public ICylinder DetachGlassShtVac1OnOff { get; }
        public ICylinder DetachGlassShtVac2OnOff { get; }
        public ICylinder DetachGlassShtVac3OnOff { get; }

        // Remove Zone
        public ICylinder RemoveZoneTrCylFwBw { get; }
        public ICylinder RemoveZoneZCyl1UpDown { get; }
        public ICylinder RemoveZoneZCyl2UpDown { get; }
        public ICylinder RemoveZoneCyl1ClampUnclamp { get; }
        public ICylinder RemoveZoneCyl2ClampUnclamp { get; }
        public ICylinder RemoveZoneCyl3ClampUnclamp { get; }
        public ICylinder RemoveZonePusherCyl1UpDown { get; }
        public ICylinder RemoveZonePusherCyl2UpDown { get; }
        public ICylinder RemoveZoneFixCyl1_1FwBw { get; }
        public ICylinder RemoveZoneFixCyl1_2FwBw { get; }
        public ICylinder RemoveZoneFixCyl2_1FwBw { get; }
        public ICylinder RemoveZoneFixCyl2_2FwBw { get; }

        // In Shuttle (rotate 0°/180°)
        public ICylinder TransferInShuttleLRotate { get; }
        public ICylinder TransferInShuttleRRotate { get; }
        public ICylinder TransferInShuttleLVacOnOff { get; }
        public ICylinder TransferInShuttleRVacOnOff { get; }

        // Align Stage L/R
        public ICylinder AlignStageLBrushCylUpDown { get; }
        public ICylinder AlignStageL1AlignUnalign { get; }
        public ICylinder AlignStageL2AlignUnalign { get; }
        public ICylinder AlignStageL3AlignUnalign { get; }
        public ICylinder AlignStageRBrushCylUpDown { get; }
        public ICylinder AlignStageR1AlignUnalign { get; }
        public ICylinder AlignStageR2AlignUnalign { get; }
        public ICylinder AlignStageR3AlignUnalign { get; }

        // Glass Transfer (enum uses 'Transfer')
        public ICylinder GlassTransferCyl1UpDown { get; }
        public ICylinder GlassTransferCyl2UpDown { get; }
        public ICylinder GlassTransferCyl3UpDown { get; }
        public ICylinder GlassTransferVac1OnOff { get; }
        public ICylinder GlassTransferVac2OnOff { get; }
        public ICylinder GlassTransferVac3OnOff { get; }

        // Wet Clean
        public ICylinder WetCleanPusherRightUpDown { get; }
        public ICylinder WetCleanPusherLeftUpDown { get; }
        public ICylinder WetCleanBrushRightUpDown { get; }
        public ICylinder WetCleanBrushLeftUpDown { get; }

        public ICylinder WetCleanRight1ClampUnclamp { get; }
        public ICylinder WetCleanRight2ClampUnclamp { get; }
        public ICylinder WetCleanLeft1ClampUnclamp { get; }
        public ICylinder WetCleanLeft2ClampUnclamp { get; }

        // Transfer Rotate
        public ICylinder TrRotateRightRotate { get; }
        public ICylinder TrRotateRightFwBw { get; }
        public ICylinder TrRotateLeftRotate { get; }
        public ICylinder TrRotateLeftFwBw { get; }
        public ICylinder TrRotateRightUpDown { get; }
        public ICylinder TrRotateLeftUpDown { get; }
        public ICylinder TrRotateRightVacOnOff { get; }
        public ICylinder TrRotateLeftVacOnOff { get; }
        public ICylinder TrRotateRightVac1OnOff { get; }
        public ICylinder TrRotateRightVac2OnOff { get; }
        public ICylinder TrRotateLeftVac1OnOff { get; }
        public ICylinder TrRotateLeftVac2OnOff { get; }

        // AF Clean
        public ICylinder AFCleanPusherRightUpDown { get; }
        public ICylinder AFCleanPusherLeftUpDown { get; }
        public ICylinder AFCleanBrushRightUpDown { get; }
        public ICylinder AFCleanBrushLeftUpDown { get; }

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

        // Unload Transfer
        public ICylinder UnloadTransferLVacOnOff { get; }
        public ICylinder UnloadTransferRVacOnOff { get; }
        public ICylinder UnloadGlassAlignVac1OnOff { get; }
        public ICylinder UnloadGlassAlignVac2OnOff { get; }
        public ICylinder UnloadGlassAlignVac3OnOff { get; }
        public ICylinder UnloadGlassAlignVac4OnOff { get; }

        //In Shuttle
        public ICylinder InShuttleR1AlignUnalign { get; }
        public ICylinder InShuttleR2AlignUnalign { get; }
        public ICylinder InShuttleL1AlignUnalign { get; }
        public ICylinder InShuttleL2AlignUnalign { get; }

        //Out Shuttle
        public ICylinder OutShuttleR1AlignUnalign { get; }
        public ICylinder OutShuttleR2AlignUnalign { get; }
        public ICylinder OutShuttleL1AlignUnalign { get; }
        public ICylinder OutShuttleL2AlignUnalign { get; }

        public Cylinders(ICylinderFactory cylinderFactory, Inputs inputs, Outputs outputs)
        {
            _cylinderFactory = cylinderFactory;
            _inputs = inputs;
            _outputs = outputs;

            // In CST
            InCstStopperUpDown = _cylinderFactory
                .Create(_inputs.InCstStopperUp, _inputs.InCstStopperDown, _outputs.InCstStopperUp, _outputs.InCstStopperDown)
                .SetIdentity((int)ECylinder.InCstStopperUpDown, ECylinder.InCstStopperUpDown.ToString());
            InCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            // Out CST
            OutCstStopperUpDown = _cylinderFactory
                .Create(_inputs.OutCstStopperUp, _inputs.OutCstStopperDown, _outputs.OutCstStopperUp, _outputs.OutCstStopperDown)
                .SetIdentity((int)ECylinder.OutCstStopperUpDown, ECylinder.OutCstStopperUpDown.ToString());
            OutCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            // In CST Work (per input pair)
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

            // Buffer CV Work
            BufferCvStopper1UpDown = _cylinderFactory
                .Create(_inputs.BufferCvStopper1Up, _inputs.BufferCvStopper1Down, _outputs.BufferCvStopper1Up, _outputs.BufferCvStopper1Down)
                .SetIdentity((int)ECylinder.BufferCvStopper1UpDown, ECylinder.BufferCvStopper1UpDown.ToString());
            BufferCvStopper1UpDown.CylinderType = ECylinderType.UpDown;

            BufferCvStopper2UpDown = _cylinderFactory
                .Create(_inputs.BufferCvStopper2Up, _inputs.BufferCvStopper2Down, _outputs.BufferCvStopper2Up, _outputs.BufferCvStopper2Down)
                .SetIdentity((int)ECylinder.BufferCvStopper2UpDown, ECylinder.BufferCvStopper2UpDown.ToString());
            BufferCvStopper2UpDown.CylinderType = ECylinderType.UpDown;

            // In CV Support
            InCvSupportUpDown = _cylinderFactory
                .Create(_inputs.InCvSupportUp, _inputs.InCvSupportDown, _outputs.InCvSupportUp, _outputs.InCvSupportDown)
                .SetIdentity((int)ECylinder.InCvSupportUpDown, ECylinder.InCvSupportUpDown.ToString());
            InCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            // In CV Support Buffer
            InCvSupportBufferUpDown = _cylinderFactory
                .Create(_inputs.InCvSupportBufferUp, _inputs.InCvSupportBufferDown, _outputs.InCvSupportBufferUp, _outputs.InCvSupportBufferDown)
                .SetIdentity((int)ECylinder.InCvSupportBufferUpDown, ECylinder.InCvSupportBufferUpDown.ToString());
            InCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            // Out CV Support Buffer
            OutCvSupportBufferUpDown = _cylinderFactory
                .Create(_inputs.OutCvSupportBufferUp, _inputs.OutCvSupportBufferDown, _outputs.OutCvSupportBufferUp, _outputs.OutCvSupportBufferDown)
                .SetIdentity((int)ECylinder.OutCvSupportBufferUpDown, ECylinder.OutCvSupportBufferUpDown.ToString());
            OutCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            // Out CV Support
            OutCvSupportUpDown = _cylinderFactory
                .Create(_inputs.OutCvSupportUp, _inputs.OutCvSupportDown, _outputs.OutCvSupportUp, _outputs.OutCvSupportDown)
                .SetIdentity((int)ECylinder.OutCvSupportUpDown, ECylinder.OutCvSupportUpDown.ToString());
            OutCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            // Out CST Work (per input pair)
            OutCstFixCyl1FwBw = _cylinderFactory
                .Create(_inputs.OutCstFixCyl1Fw, _inputs.OutCstFixCyl1Bw, _outputs.OutCstFixCyl1Fw, _outputs.OutCstFixCyl1Bw)
                .SetIdentity((int)ECylinder.OutCstFixCyl1FwBw, ECylinder.OutCstFixCyl1FwBw.ToString());
            OutCstFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            OutCstFixCyl2FwBw = _cylinderFactory
                .Create(_inputs.OutCstFixCyl2Fw, _inputs.OutCstFixCyl2Bw, _outputs.OutCstFixCyl2Fw, _outputs.OutCstFixCyl2Bw)
                .SetIdentity((int)ECylinder.OutCstFixCyl2FwBw, ECylinder.OutCstFixCyl2FwBw.ToString());
            OutCstFixCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            OutCstTiltCylUpDown = _cylinderFactory
                .Create(_inputs.OutCstTiltCylUp, _inputs.OutCstTiltCylDown, _outputs.OutCstTiltCylUp, _outputs.OutCstTiltCylDown)
                .SetIdentity((int)ECylinder.OutCstTiltCylUpDown, ECylinder.OutCstTiltCylUpDown.ToString());
            OutCstTiltCylUpDown.CylinderType = ECylinderType.UpDown;

            // Robot 1 Load/Unload
            RobotFixtureAlignFwBw = _cylinderFactory
                .Create(_inputs.RobotFixtureAlignFw, _inputs.RobotFixtureAlignBw, _outputs.RobotFixtureAlignFw, _outputs.RobotFixtureAlignBw)
                .SetIdentity((int)ECylinder.RobotFixtureAlignFwBw, ECylinder.RobotFixtureAlignFwBw.ToString());
            RobotFixtureAlignFwBw.CylinderType = ECylinderType.ForwardBackward;

            RobotFixture1ClampUnclamp = _cylinderFactory
                .Create(_inputs.RobotFixture1Clamp, _inputs.RobotFixture1Unclamp, _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotFixture1ClampUnclamp, ECylinder.RobotFixture1ClampUnclamp.ToString());
            RobotFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RobotFixture2ClampUnclamp = _cylinderFactory
                .Create(_inputs.RobotFixture2Clamp, _inputs.RobotFixture2Unclamp, _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotFixture2ClampUnclamp, ECylinder.RobotFixture2ClampUnclamp.ToString());
            RobotFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            // Fixture Align (machine)
            AlignFixtureCyl1FwBw = _cylinderFactory
                .Create(_inputs.FixtureAlign1CylFw, _inputs.FixtureAlign1CylBw, _outputs.FixtureAlignCyl1Fw, _outputs.FixtureAlignCyl1Bw)
                .SetIdentity((int)ECylinder.FixtureAlignCyl1FwBw, ECylinder.FixtureAlignCyl1FwBw.ToString());
            AlignFixtureCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            AlignFixtureCyl2FwBw = _cylinderFactory
                .Create(_inputs.FixtureAlign2CylFw, _inputs.FixtureAlign2CylBw, _outputs.FixtureAlignCyl2Fw, _outputs.FixtureAlignCyl2Bw)
                .SetIdentity((int)ECylinder.FixtureAlignCyl2FwBw, ECylinder.FixtureAlignCyl2FwBw.ToString());
            AlignFixtureCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            // Vinyl Clean
            VinylCleanRollerFwBw = _cylinderFactory
                .Create(_inputs.VinylCleanRollerFw, _inputs.VinylCleanRollerBw, _outputs.VinylCleanRollerFw, _outputs.VinylCleanRollerBw)
                .SetIdentity((int)ECylinder.VinylCleanRollerFwBw, ECylinder.VinylCleanRollerFwBw.ToString());
            VinylCleanRollerFwBw.CylinderType = ECylinderType.ForwardBackward;

            VinylCleanFixture1ClampUnclamp = _cylinderFactory
                .Create(_inputs.VinylCleanFixture1Clamp, _inputs.VinylCleanFixture1Unclamp, _outputs.VinylCleanFixtureClamp, _outputs.VinylCleanFixtureUnclamp)
                .SetIdentity((int)ECylinder.VinylCleanFixture1ClampUnclamp, ECylinder.VinylCleanFixture1ClampUnclamp.ToString());
            VinylCleanFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanFixture2ClampUnclamp = _cylinderFactory
                .Create(_inputs.VinylCleanFixture2Clamp, _inputs.VinylCleanFixture2Unclamp, _outputs.VinylCleanFixtureClamp, _outputs.VinylCleanFixtureUnclamp)
                .SetIdentity((int)ECylinder.VinylCleanFixture2ClampUnclamp, ECylinder.VinylCleanFixture2ClampUnclamp.ToString());
            VinylCleanFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanPusherRollerUpDown = _cylinderFactory
                .Create(_inputs.VinylCleanPusherRollerUp, _inputs.VinylCleanPusherRollerDown, _outputs.VinylCleanPusherRollerUp, null)
                .SetIdentity((int)ECylinder.VinylCleanPusherRollerUpDown, ECylinder.VinylCleanPusherRollerUpDown.ToString());
            VinylCleanPusherRollerUpDown.CylinderType = ECylinderType.UpDown;

            // Transfer Y Fixture
            TransferFixtureUpDown = _cylinderFactory
                .Create(_inputs.TransferFixtureUp, _inputs.TransferFixtureDown, _outputs.TransferFixtureUp, _outputs.TransferFixtureDown)
                .SetIdentity((int)ECylinder.TransferFixtureUpDown, ECylinder.TransferFixtureUpDown.ToString());
            TransferFixtureUpDown.CylinderType = ECylinderType.UpDown;

            TransferFixture1_1ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture11Unclamp, _inputs.TransferFixture11Clamp, _outputs.TransferFixture1Unclamp, _outputs.TransferFixture1Clamp)
                .SetIdentity((int)ECylinder.TransferFixture1_1ClampUnclamp, ECylinder.TransferFixture1_1ClampUnclamp.ToString());
            TransferFixture1_1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture1_2ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture12Unclamp, _inputs.TransferFixture12Clamp, _outputs.TransferFixture1Unclamp, _outputs.TransferFixture1Clamp)
                .SetIdentity((int)ECylinder.TransferFixture1_2ClampUnclamp, ECylinder.TransferFixture1_2ClampUnclamp.ToString());
            TransferFixture1_2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture2_1ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture21Unclamp, _inputs.TransferFixture21Clamp, _outputs.TransferFixture2Unclamp, _outputs.TransferFixture2Clamp)
                .SetIdentity((int)ECylinder.TransferFixture2_1ClampUnclamp, ECylinder.TransferFixture2_1ClampUnclamp.ToString());
            TransferFixture2_1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture2_2ClampUnclamp = _cylinderFactory
                .Create(_inputs.TransferFixture22Unclamp, _inputs.TransferFixture22Clamp, _outputs.TransferFixture2Unclamp, _outputs.TransferFixture2Clamp)
                .SetIdentity((int)ECylinder.TransferFixture2_2ClampUnclamp, ECylinder.TransferFixture2_2ClampUnclamp.ToString());
            TransferFixture2_2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            // Detach Glass
            DetachCyl1UpDown = _cylinderFactory
                .Create(_inputs.DetachCyl1Down, _inputs.DetachCyl1Up, _outputs.DetachCyl1Down, _outputs.DetachCyl1Up)
                .SetIdentity((int)ECylinder.DetachCyl1UpDown, ECylinder.DetachCyl1UpDown.ToString());
            DetachCyl1UpDown.CylinderType = ECylinderType.UpDown;

            DetachCyl2UpDown = _cylinderFactory
                .Create(_inputs.DetachCyl2Down, _inputs.DetachCyl2Up, _outputs.DetachCyl2Down, _outputs.DetachCyl2Up)
                .SetIdentity((int)ECylinder.DetachCyl2UpDown, ECylinder.DetachCyl2UpDown.ToString());
            DetachCyl2UpDown.CylinderType = ECylinderType.UpDown;

            DetachFixFixtureCyl1_1FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl11Fw, _inputs.DetachFixFixtureCyl11Bw, _outputs.DetachFixFixtureCyl1Fw, _outputs.DetachFixFixtureCyl1Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl1_1FwBw, ECylinder.DetachFixFixtureCyl1_1FwBw.ToString());
            DetachFixFixtureCyl1_1FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl1_2FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl12Fw, _inputs.DetachFixFixtureCyl12Bw, _outputs.DetachFixFixtureCyl1Fw, _outputs.DetachFixFixtureCyl1Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl1_2FwBw, ECylinder.DetachFixFixtureCyl1_2FwBw.ToString());
            DetachFixFixtureCyl1_2FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl2_1FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl21Fw, _inputs.DetachFixFixtureCyl21Bw, _outputs.DetachFixFixtureCyl2Fw, _outputs.DetachFixFixtureCyl2Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl2_1FwBw, ECylinder.DetachFixFixtureCyl2_1FwBw.ToString());
            DetachFixFixtureCyl2_1FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl2_2FwBw = _cylinderFactory
                .Create(_inputs.DetachFixFixtureCyl22Fw, _inputs.DetachFixFixtureCyl22Bw, _outputs.DetachFixFixtureCyl2Fw, _outputs.DetachFixFixtureCyl2Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl2_2FwBw, ECylinder.DetachFixFixtureCyl2_2FwBw.ToString());
            DetachFixFixtureCyl2_2FwBw.CylinderType = ECylinderType.ForwardBackward;

            // Remove Zone
            RemoveZoneTrCylFwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneTrCylFw, _inputs.RemoveZoneTrCylBw, _outputs.RemoveZoneTrCylFw, _outputs.RemoveZoneTrCylBw)
                .SetIdentity((int)ECylinder.RemoveZoneTrCylFwBw, ECylinder.RemoveZoneTrCylFwBw.ToString());
            RemoveZoneTrCylFwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneZCyl1UpDown = _cylinderFactory
                .Create(_inputs.RemoveZoneZCyl1Down, _inputs.RemoveZoneZCyl1Up, _outputs.RemoveZoneZCyl1Down, null)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl1UpDown, ECylinder.RemoveZoneZCyl1UpDown.ToString());
            RemoveZoneZCyl1UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneZCyl2UpDown = _cylinderFactory
                .Create(_inputs.RemoveZoneZCyl2Down, _inputs.RemoveZoneZCyl2Up, _outputs.RemoveZoneZCyl2Down, null)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl2UpDown, ECylinder.RemoveZoneZCyl2UpDown.ToString());
            RemoveZoneZCyl2UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneCyl1ClampUnclamp = _cylinderFactory
                .Create(_inputs.RemoveZoneCyl1Clamp, _inputs.RemoveZoneCyl1Unclamp, _outputs.RemoveZoneCyl1Clamp, _outputs.RemoveZoneCyl1Unclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl1ClampUnclamp, ECylinder.RemoveZoneCyl1ClampUnclamp.ToString());
            RemoveZoneCyl1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZoneCyl2ClampUnclamp = _cylinderFactory
                .Create(_inputs.RemoveZoneCyl2Clamp, _inputs.RemoveZoneCyl2Unclamp, _outputs.RemoveZoneCyl2Clamp, _outputs.RemoveZoneCyl2Unclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl2ClampUnclamp, ECylinder.RemoveZoneCyl2ClampUnclamp.ToString());
            RemoveZoneCyl2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZoneCyl3ClampUnclamp = _cylinderFactory
                .Create(_inputs.RemoveZoneCyl3Clamp, _inputs.RemoveZoneCyl3Unclamp, _outputs.RemoveZoneCyl3Clamp, _outputs.RemoveZoneCyl3Unclamp)
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

            RemoveZoneFixCyl1_1FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl11Fw, _inputs.RemoveZoneFixCyl11Bw, _outputs.RemoveZoneFixCyl1Fw, _outputs.RemoveZoneFixCyl1Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl1_1FwBw, ECylinder.RemoveZoneFixCyl1_1FwBw.ToString());
            RemoveZoneFixCyl1_1FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl1_2FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl12Fw, _inputs.RemoveZoneFixCyl12Bw, _outputs.RemoveZoneFixCyl1Fw, _outputs.RemoveZoneFixCyl1Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl1_2FwBw, ECylinder.RemoveZoneFixCyl1_2FwBw.ToString());
            RemoveZoneFixCyl1_2FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl2_1FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl21Fw, _inputs.RemoveZoneFixCyl21Bw, _outputs.RemoveZoneFixCyl2Fw, _outputs.RemoveZoneFixCyl2Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl2_1FwBw, ECylinder.RemoveZoneFixCyl2_1FwBw.ToString());
            RemoveZoneFixCyl2_1FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl2_2FwBw = _cylinderFactory
                .Create(_inputs.RemoveZoneFixCyl22Fw, _inputs.RemoveZoneFixCyl22Bw, _outputs.RemoveZoneFixCyl2Fw, _outputs.RemoveZoneFixCyl2Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl2_2FwBw, ECylinder.RemoveZoneFixCyl2_2FwBw.ToString());
            RemoveZoneFixCyl2_2FwBw.CylinderType = ECylinderType.ForwardBackward;

            // In Shuttle rotate
            TransferInShuttleLRotate = _cylinderFactory
                .Create(_inputs.TransferInShuttleL180Degree, _inputs.TransferInShuttleL0Degree, _outputs.TransferInShuttleL180Degree, _outputs.TransferInShuttleL0Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleLRotate, ECylinder.TransferInShuttleLRotate.ToString());
            TransferInShuttleLRotate.CylinderType = ECylinderType.FlipUnflip;

            TransferInShuttleRRotate = _cylinderFactory
                .Create(_inputs.TransferInShuttleR180Degree, _inputs.TransferInShuttleR0Degree, _outputs.TransferInShuttleR180Degree, _outputs.TransferInShuttleR0Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleRRotate, ECylinder.TransferInShuttleRRotate.ToString());
            TransferInShuttleRRotate.CylinderType = ECylinderType.FlipUnflip;

            // Align Stage L/R
            AlignStageLBrushCylUpDown = _cylinderFactory
                .Create(_inputs.AlignStageLBrushCylUp, _inputs.AlignStageLBrushCylDown, _outputs.AlignStageLBrushCylUp, null)
                .SetIdentity((int)ECylinder.AlignStageLBrushCylUpDown, ECylinder.AlignStageLBrushCylUpDown.ToString());
            AlignStageLBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageL1AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageL1Align, _inputs.AlignStageL1Unalign, _outputs.AlignStageL1Align, null)
                .SetIdentity((int)ECylinder.AlignStageL1AlignUnalign, ECylinder.AlignStageL1AlignUnalign.ToString());
            AlignStageL1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL2AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageL2Align, _inputs.AlignStageL2Unalign, _outputs.AlignStageL2Align, null)
                .SetIdentity((int)ECylinder.AlignStageL2AlignUnalign, ECylinder.AlignStageL2AlignUnalign.ToString());
            AlignStageL2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL3AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageL3Align, _inputs.AlignStageL3Unalign, _outputs.AlignStageL3Align, null)
                .SetIdentity((int)ECylinder.AlignStageL3AlignUnalign, ECylinder.AlignStageL3AlignUnalign.ToString());
            AlignStageL3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageRBrushCylUpDown = _cylinderFactory
                .Create(_inputs.AlignStageRBrushCylUp, _inputs.AlignStageRBrushCylDown, _outputs.AlignStageRBrushCylUp, null)
                .SetIdentity((int)ECylinder.AlignStageRBrushCylUpDown, ECylinder.AlignStageRBrushCylUpDown.ToString());
            AlignStageRBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageR1AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageR1Align, _inputs.AlignStageR1Unalign, _outputs.AlignStageR1Align, null)
                .SetIdentity((int)ECylinder.AlignStageR1AlignUnalign, ECylinder.AlignStageR1AlignUnalign.ToString());
            AlignStageR1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR2AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageR2Align, _inputs.AlignStageR2Unalign, _outputs.AlignStageR2Align, null)
                .SetIdentity((int)ECylinder.AlignStageR2AlignUnalign, ECylinder.AlignStageR2AlignUnalign.ToString());
            AlignStageR2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR3AlignUnalign = _cylinderFactory
                .Create(_inputs.AlignStageR3Align, _inputs.AlignStageR3Unalign, _outputs.AlignStageR3Align, null)
                .SetIdentity((int)ECylinder.AlignStageR3AlignUnalign, ECylinder.AlignStageR3AlignUnalign.ToString());
            AlignStageR3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            // Glass Transfer (Transfer)
            GlassTransferCyl1UpDown = _cylinderFactory
                .Create(_inputs.GlassTransferCyl1Down, _inputs.GlassTransferCyl1Up, _outputs.GlassTransferCyl1Down, _outputs.GlassTransferCyl1Up)
                .SetIdentity((int)ECylinder.GlassTransferCyl1UpDown, ECylinder.GlassTransferCyl1UpDown.ToString());
            GlassTransferCyl1UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl2UpDown = _cylinderFactory
                .Create(_inputs.GlassTransferCyl2Down, _inputs.GlassTransferCyl2Up, _outputs.GlassTransferCyl2Down, _outputs.GlassTransferCyl2Up)
                .SetIdentity((int)ECylinder.GlassTransferCyl2UpDown, ECylinder.GlassTransferCyl2UpDown.ToString());
            GlassTransferCyl2UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl3UpDown = _cylinderFactory
                .Create(_inputs.GlassTransferCyl3Down, _inputs.GlassTransferCyl3Up, _outputs.GlassTransferCyl3Down, _outputs.GlassTransferCyl3Up)
                .SetIdentity((int)ECylinder.GlassTransferCyl3UpDown, ECylinder.GlassTransferCyl3UpDown.ToString());
            GlassTransferCyl3UpDown.CylinderType = ECylinderType.UpDown;

            // Wet Clean
            WetCleanPusherRightUpDown = _cylinderFactory
                .Create(_inputs.WetCleanPusherRightDown, _inputs.WetCleanPusherRightUp, _outputs.WetCleanPusherRightDown, _outputs.WetCleanPusherRightUp)
                .SetIdentity((int)ECylinder.WetCleanPusherRightUpDown, ECylinder.WetCleanPusherRightUpDown.ToString());
            WetCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanPusherLeftUpDown = _cylinderFactory
                .Create(_inputs.WetCleanPusherLeftDown, _inputs.WetCleanPusherLeftUp, _outputs.WetCleanPusherLeftDown, _outputs.WetCleanPusherLeftUp)
                .SetIdentity((int)ECylinder.WetCleanPusherLeftUpDown, ECylinder.WetCleanPusherLeftUpDown.ToString());
            WetCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanBrushRightUpDown = _cylinderFactory
                .Create(_inputs.WetCleanBrushRightDown, _inputs.WetCleanBrushRightUp, _outputs.WetCleanBrushRightDown, null)
                .SetIdentity((int)ECylinder.WetCleanBrushRightUpDown, ECylinder.WetCleanBrushRightUpDown.ToString());
            WetCleanBrushRightUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanBrushLeftUpDown = _cylinderFactory
                .Create(_inputs.WetCleanBrushLeftDown, _inputs.WetCleanBrushLeftUp, _outputs.WetCleanBrushLeftDown, null)
                .SetIdentity((int)ECylinder.WetCleanBrushLeftUpDown, ECylinder.WetCleanBrushLeftUpDown.ToString());
            WetCleanBrushLeftUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanRight1ClampUnclamp = _cylinderFactory
                .Create(_inputs.InShuttleRClamp1FW, _inputs.InShuttleRClamp1BW, _outputs.InShuttleRClampFw, _outputs.InShuttleRClampBw)
                .SetIdentity((int)ECylinder.WetCleanRight1ClampUnclamp, ECylinder.WetCleanRight1ClampUnclamp.ToString());
            WetCleanRight1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            WetCleanRight2ClampUnclamp = _cylinderFactory
                .Create(_inputs.InShuttleRClamp2FW, _inputs.InShuttleRClamp2BW, _outputs.InShuttleRClampFw, _outputs.InShuttleRClampBw)
                .SetIdentity((int)ECylinder.WetCleanRight2ClampUnclamp, ECylinder.WetCleanRight2ClampUnclamp.ToString());
            WetCleanRight2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            WetCleanLeft1ClampUnclamp = _cylinderFactory
                .Create(_inputs.InShuttleLClamp1FW, _inputs.InShuttleLClamp1BW, _outputs.InShuttleLClampFw, _outputs.InShuttleLClampBw)
                .SetIdentity((int)ECylinder.WetCleanLeft1ClampUnclamp, ECylinder.WetCleanLeft1ClampUnclamp.ToString());
            WetCleanLeft1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            WetCleanLeft2ClampUnclamp = _cylinderFactory
                .Create(_inputs.InShuttleLClamp2FW, _inputs.InShuttleLClamp2BW, _outputs.InShuttleLClampFw, _outputs.InShuttleLClampBw)
                .SetIdentity((int)ECylinder.WetCleanLeft2ClampUnclamp, ECylinder.WetCleanLeft2ClampUnclamp.ToString());
            WetCleanLeft2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            // Transfer Rotate
            TrRotateRightRotate = _cylinderFactory
                .Create(_inputs.TrRotateRight180Degree, _inputs.TrRotateRight0Degree, _outputs.TrRotateRight180Degree, _outputs.TrRotateRight0Degree)
                .SetIdentity((int)ECylinder.TrRotateRightRotate, ECylinder.TrRotateRightRotate.ToString());
            TrRotateRightRotate.CylinderType = ECylinderType.FlipUnflip;

            TrRotateRightFwBw = _cylinderFactory
                .Create(_inputs.TrRotateRightFw, _inputs.TrRotateRightBw, _outputs.TrRotateRightFw, _outputs.TrRotateRightBw)
                .SetIdentity((int)ECylinder.TrRotateRightFwBw, ECylinder.TrRotateRightFwBw.ToString());
            TrRotateRightFwBw.CylinderType = ECylinderType.ForwardBackward;

            TrRotateLeftRotate = _cylinderFactory
                .Create(_inputs.TrRotateLeft180Degree, _inputs.TrRotateLeft0Degree, _outputs.TrRotateLeft180Degree, _outputs.TrRotateLeft0Degree)
                .SetIdentity((int)ECylinder.TrRotateLeftRotate, ECylinder.TrRotateLeftRotate.ToString());
            TrRotateLeftRotate.CylinderType = ECylinderType.FlipUnflip;

            TrRotateLeftFwBw = _cylinderFactory
                .Create(_inputs.TrRotateLeftFw, _inputs.TrRotateLeftBw, _outputs.TrRotateLeftFw, _outputs.TrRotateLeftBw)
                .SetIdentity((int)ECylinder.TrRotateLeftFwBw, ECylinder.TrRotateLeftFwBw.ToString());
            TrRotateLeftFwBw.CylinderType = ECylinderType.ForwardBackward;

            TrRotateRightUpDown = _cylinderFactory
                .Create(_inputs.TrRotateRightDown, _inputs.TrRotateRightUp, _outputs.TrRotateRightDown, _outputs.TrRotateRightUp)
                .SetIdentity((int)ECylinder.TrRotateRightUpDown, ECylinder.TrRotateRightUpDown.ToString());
            TrRotateRightUpDown.CylinderType = ECylinderType.UpDown;

            TrRotateLeftUpDown = _cylinderFactory
                .Create(_inputs.TrRotateLeftDown, _inputs.TrRotateLeftUp, _outputs.TrRotateLeftDown, _outputs.TrRotateLeftUp)
                .SetIdentity((int)ECylinder.TrRotateLeftUpDown, ECylinder.TrRotateLeftUpDown.ToString());
            TrRotateLeftUpDown.CylinderType = ECylinderType.UpDown;


            // AF Clean
            AFCleanPusherRightUpDown = _cylinderFactory
                .Create(_inputs.AfCleanPusherRightDown, _inputs.AfCleanPusherRightUp, _outputs.AfCleanPusherRightDown, _outputs.AfCleanPusherRightUp)
                .SetIdentity((int)ECylinder.AFCleanPusherRightUpDown, ECylinder.AFCleanPusherRightUpDown.ToString());
            AFCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanPusherLeftUpDown = _cylinderFactory
                .Create(_inputs.AfCleanPusherLeftDown, _inputs.AfCleanPusherLeftUp, _outputs.AfCleanPusherLeftDown, _outputs.AfCleanPusherLeftUp)
                .SetIdentity((int)ECylinder.AFCleanPusherLeftUpDown, ECylinder.AFCleanPusherLeftUpDown.ToString());
            AFCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanBrushRightUpDown = _cylinderFactory
                .Create(_inputs.AfCleanBrushRightDown, _inputs.AfCleanBrushRightUp, _outputs.AfCleanBrushRightDown, null)
                .SetIdentity((int)ECylinder.AFCleanBrushRightUpDown, ECylinder.AFCleanBrushRightUpDown.ToString());
            AFCleanBrushRightUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanBrushLeftUpDown = _cylinderFactory
                .Create(_inputs.AfCleanBrushLeftDown, _inputs.AfCleanBrushLeftUp, _outputs.AfCleanBrushLeftDown, null)
                .SetIdentity((int)ECylinder.AFCleanBrushLeftUpDown, ECylinder.AFCleanBrushLeftUpDown.ToString());
            AFCleanBrushLeftUpDown.CylinderType = ECylinderType.UpDown;

            // Robot 2 Unload
            UnloadRobotCyl1UpDown = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl1Down, _inputs.UnloadRobotCyl1Up, _outputs.UnloadRobotCyl1Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl1UpDown, ECylinder.UnloadRobotCyl1UpDown.ToString());
            UnloadRobotCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl2UpDown = _cylinderFactory
                .Create( _inputs.UnloadRobotCyl2Down ,  _inputs.UnloadRobotCyl2Up , _outputs.UnloadRobotCyl2Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl2UpDown, ECylinder.UnloadRobotCyl2UpDown.ToString());
            UnloadRobotCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl3UpDown = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl3Down, _inputs.UnloadRobotCyl3Up, _outputs.UnloadRobotCyl3Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl3UpDown, ECylinder.UnloadRobotCyl3UpDown.ToString());
            UnloadRobotCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl4UpDown = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl4Down, _inputs.UnloadRobotCyl4Up, _outputs.UnloadRobotCyl4Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl4UpDown, ECylinder.UnloadRobotCyl4UpDown.ToString());
            UnloadRobotCyl4UpDown.CylinderType = ECylinderType.UpDown;

            // Unload Stage
            UnloadAlignCyl1UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl1Up, _inputs.UnloadAlignCyl1Down, _outputs.UnloadAlignCyl1Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl1UpDown, ECylinder.UnloadAlignCyl1UpDown.ToString());
            UnloadAlignCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl2UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl2Up, _inputs.UnloadAlignCyl2Down, _outputs.UnloadAlignCyl2Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl2UpDown, ECylinder.UnloadAlignCyl2UpDown.ToString());
            UnloadAlignCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl3UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl3Up, _inputs.UnloadAlignCyl3Down, _outputs.UnloadAlignCyl3Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl3UpDown, ECylinder.UnloadAlignCyl3UpDown.ToString());
            UnloadAlignCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl4UpDown = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl4Up, _inputs.UnloadAlignCyl4Down, _outputs.UnloadAlignCyl4Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl4UpDown, ECylinder.UnloadAlignCyl4UpDown.ToString());
            UnloadAlignCyl4UpDown.CylinderType = ECylinderType.UpDown;
        
        }

        private readonly ICylinderFactory _cylinderFactory;
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
    }
}
