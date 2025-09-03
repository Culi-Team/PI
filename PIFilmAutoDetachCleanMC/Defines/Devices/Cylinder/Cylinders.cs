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
        public ICylinder InCstFixCylFwBw { get; }
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
        public ICylinder OutCstFixCylFwBw { get; }
        public ICylinder OutCstTiltCylUpDown { get; }

        // Robot 1 Load/Unload
        public ICylinder RobotFixtureAlignFwBw { get; }
        public ICylinder RobotFixtureClampUnclamp { get; }


        //Align Fixture
        public ICylinder AlignFixtureBwFw { get; }

        // Vinyl Clean
        public ICylinder VinylCleanFixture1ClampUnclamp { get; }
        public ICylinder VinylCleanFixture2ClampUnclamp { get; }
        public ICylinder VinylCleanPusherRollerUpDown { get; }

        // Transfer Y Fixture
        public ICylinder TransferFixtureUpDown { get; }
        public ICylinder TransferFixture1ClampUnclamp { get; }
        public ICylinder TransferFixture2ClampUnclamp { get; }

        // Detach Glass
        public ICylinder DetachCyl1UpDown { get; }
        public ICylinder DetachCyl2UpDown { get; }
        public ICylinder DetachFixFixtureCyl1FwBw { get; }
        public ICylinder DetachFixFixtureCyl2FwBw { get; }

        // Remove Zone
        public ICylinder RemoveZoneTrCylFwBw { get; }
        public ICylinder RemoveZoneZCyl1UpDown { get; }
        public ICylinder RemoveZoneZCyl2UpDown { get; }
        public ICylinder RemoveZoneCylClampUnclamp { get; }
        public ICylinder RemoveZonePusherCyl1UpDown { get; }
        public ICylinder RemoveZonePusherCyl2UpDown { get; }
        public ICylinder RemoveZoneFixCyl1FwBw { get; }
        public ICylinder RemoveZoneFixCyl2FwBw { get; }

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
                .Create(new List<IDInput> { _inputs.InCstStopperUp }, new List<IDInput> { _inputs.InCstStopperDown }, _outputs.InCstStopperUp, _outputs.InCstStopperDown)
                .SetIdentity((int)ECylinder.InCstStopperUpDown, ECylinder.InCstStopperUpDown.ToString());
            InCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            OutCstStopperUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCstStopperUp }, new List<IDInput> { _inputs.OutCstStopperDown }, _outputs.OutCstStopperUp, _outputs.OutCstStopperDown)
                .SetIdentity((int)ECylinder.OutCstStopperUpDown, ECylinder.OutCstStopperUpDown.ToString());
            OutCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            InCstFixCylFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InCstFixCyl1Fw, _inputs.InCstFixCyl2Fw }, new List<IDInput> { _inputs.InCstFixCyl1Bw, _inputs.InCstFixCyl2Bw }, _outputs.InCstFixCyl1Fw, _outputs.InCstFixCyl1Bw)
                .SetIdentity((int)ECylinder.InCstFixCyl1FwBw, ECylinder.InCstFixCyl1FwBw.ToString());
            InCstFixCylFwBw.CylinderType = ECylinderType.ForwardBackward;

            InCstTiltCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InCstTiltCylUp }, new List<IDInput> { _inputs.InCstTiltCylDown }, _outputs.InCstTiltCylUp, _outputs.InCstTiltCylDown)
                .SetIdentity((int)ECylinder.InCstTiltCylUpDown, ECylinder.InCstTiltCylUpDown.ToString());
            InCstTiltCylUpDown.CylinderType = ECylinderType.UpDown;

            BufferCvStopper1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.BufferCvStopper1Up }, new List<IDInput> { _inputs.BufferCvStopper1Down }, _outputs.BufferCvStopper1Up, _outputs.BufferCvStopper1Down)
                .SetIdentity((int)ECylinder.BufferCvStopper1UpDown, ECylinder.BufferCvStopper1UpDown.ToString());
            BufferCvStopper1UpDown.CylinderType = ECylinderType.UpDown;

            BufferCvStopper2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.BufferCvStopper2Up }, new List<IDInput> { _inputs.BufferCvStopper2Down }, _outputs.BufferCvStopper2Up, _outputs.BufferCvStopper2Down)
                .SetIdentity((int)ECylinder.BufferCvStopper2UpDown, ECylinder.BufferCvStopper2UpDown.ToString());
            BufferCvStopper2UpDown.CylinderType = ECylinderType.UpDown;

            InCvSupportUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InCvSupportUp }, new List<IDInput> { _inputs.InCvSupportDown }, _outputs.InCvSupportUp, _outputs.InCvSupportDown)
                .SetIdentity((int)ECylinder.InCvSupportUpDown, ECylinder.InCvSupportUpDown.ToString());
            InCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            InCvSupportBufferUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InCvSupportBufferUp }, new List<IDInput> { _inputs.InCvSupportBufferDown }, _outputs.InCvSupportBufferUp, _outputs.InCvSupportBufferDown)
                .SetIdentity((int)ECylinder.InCvSupportBufferUpDown, ECylinder.InCvSupportBufferUpDown.ToString());
            InCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            OutCvSupportBufferUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCvSupportBufferUp }, new List<IDInput> { _inputs.OutCvSupportBufferDown }, _outputs.OutCvSupportBufferUp, _outputs.OutCvSupportBufferDown)
                .SetIdentity((int)ECylinder.OutCvSupportBufferUpDown, ECylinder.OutCvSupportBufferUpDown.ToString());
            OutCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            OutCvSupportUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCvSupportUp }, new List<IDInput> { _inputs.OutCvSupportDown }, _outputs.OutCvSupportUp, _outputs.OutCvSupportDown)
                .SetIdentity((int)ECylinder.OutCvSupportUpDown, ECylinder.OutCvSupportUpDown.ToString());
            OutCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            OutCstFixCylFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCstFixCyl1Fw, _inputs.OutCstFixCyl2Fw }, new List<IDInput> { _inputs.OutCstFixCyl1Bw, _inputs.OutCstFixCyl2Bw }, _outputs.OutCstFixCyl1Fw, _outputs.OutCstFixCyl1Bw)
                .SetIdentity((int)ECylinder.OutCstFixCyl1FwBw, ECylinder.OutCstFixCyl1FwBw.ToString());
            OutCstFixCylFwBw.CylinderType = ECylinderType.ForwardBackward;

            OutCstTiltCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCstTiltCylUp }, new List<IDInput> { _inputs.OutCstTiltCylDown }, _outputs.OutCstTiltCylUp, _outputs.OutCstTiltCylDown)
                .SetIdentity((int)ECylinder.OutCstTiltCylUpDown, ECylinder.OutCstTiltCylUpDown.ToString());
            OutCstTiltCylUpDown.CylinderType = ECylinderType.UpDown;

            RobotFixtureAlignFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RobotFixtureAlign1Fw, _inputs.RobotFixtureAlign2Fw }, new List<IDInput> { _inputs.RobotFixtureAlign1Bw, _inputs.RobotFixtureAlign2Bw }, _outputs.RobotFixtureAlignFw, _outputs.RobotFixtureAlignBw)
                .SetIdentity((int)ECylinder.RobotFixtureAlign1FwBw, ECylinder.RobotFixtureAlign1FwBw.ToString());
            RobotFixtureAlignFwBw.CylinderType = ECylinderType.ForwardBackward;

            RobotFixtureClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RobotFixture1Clamp, _inputs.RobotFixture2Clamp }, new List<IDInput> { _inputs.RobotFixture1Unclamp, _inputs.RobotFixture2Unclamp }, _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotFixture1ClampUnclamp, ECylinder.RobotFixture1ClampUnclamp.ToString());
            RobotFixtureClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            AlignFixtureBwFw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignFixture1FW, _inputs.AlignFixture2FW }, new List<IDInput> { _inputs.AlignFixture1BW, _inputs.AlignFixture2BW }, _outputs.AlignFixtureFW, _outputs.AlignFixtureBW)
                .SetIdentity((int)ECylinder.FixtureAlignCylFwBw, ECylinder.FixtureAlignCylFwBw.ToString());

            VinylCleanFixture1ClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.VinylCleanFixture1Clamp }, new List<IDInput> { _inputs.VinylCleanFixture1Unclamp }, _outputs.VinylCleanFixture1Clamp, _outputs.VinylCleanFixture1Unclamp)
                .SetIdentity((int)ECylinder.VinylCleanFixture1ClampUnclamp, ECylinder.VinylCleanFixture1ClampUnclamp.ToString());
            VinylCleanFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanFixture2ClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.VinylCleanFixture2Clamp }, new List<IDInput> { _inputs.VinylCleanFixture2Unclamp }, _outputs.VinylCleanFixture2Clamp, _outputs.VinylCleanFixture2Unclamp)
                .SetIdentity((int)ECylinder.VinylCleanFixture2ClampUnclamp, ECylinder.VinylCleanFixture2ClampUnclamp.ToString());
            VinylCleanFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanPusherRollerUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.VinylCleanPusherRollerUp }, new List<IDInput> { _inputs.VinylCleanPusherRollerDown }, _outputs.VinylCleanPusherRollerUp, _outputs.VinylCleanPusherRollerDown)
                .SetIdentity((int)ECylinder.VinylCleanPusherRollerUpDown, ECylinder.VinylCleanPusherRollerUpDown.ToString());
            VinylCleanPusherRollerUpDown.CylinderType = ECylinderType.UpDown;

            TransferFixtureUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferFixtureUp }, new List<IDInput> { _inputs.TransferFixtureDown }, _outputs.TransferFixtureUp, _outputs.TransferFixtureDown)
                .SetIdentity((int)ECylinder.TransferFixtureUpDown, ECylinder.TransferFixtureUpDown.ToString());
            TransferFixtureUpDown.CylinderType = ECylinderType.UpDown;

            TransferFixture1ClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferFixture1_1Clamp, _inputs.TransferFixture1_2Clamp }, new List<IDInput> { _inputs.TransferFixture1_1Unclamp, _inputs.TransferFixture1_2Unclamp }, _outputs.TransferFixture1Clamp, _outputs.TransferFixture1Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture1ClampUnclamp, ECylinder.TransferFixture1ClampUnclamp.ToString());
            TransferFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture2ClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferFixture2_1Clamp, _inputs.TransferFixture2_2Clamp }, new List<IDInput> { _inputs.TransferFixture2_1Unclamp, _inputs.TransferFixture2_2Unclamp }, _outputs.TransferFixture2Clamp, _outputs.TransferFixture2Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture2ClampUnclamp, ECylinder.TransferFixture2ClampUnclamp.ToString());
            TransferFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            DetachCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.DetachCyl1Up }, new List<IDInput> { _inputs.DetachCyl1Down }, _outputs.DetachCyl1Up, _outputs.DetachCyl1Down)
                .SetIdentity((int)ECylinder.DetachCyl1UpDown, ECylinder.DetachCyl1UpDown.ToString());
            DetachCyl1UpDown.CylinderType = ECylinderType.UpDown;

            DetachCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.DetachCyl2Up }, new List<IDInput> { _inputs.DetachCyl2Down }, _outputs.DetachCyl2Up, _outputs.DetachCyl2Down)
                .SetIdentity((int)ECylinder.DetachCyl2UpDown, ECylinder.DetachCyl2UpDown.ToString());
            DetachCyl2UpDown.CylinderType = ECylinderType.UpDown;

            DetachFixFixtureCyl1FwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.DetachFixFixtureCyl1_1Fw, _inputs.DetachFixFixtureCyl1_2Fw }, new List<IDInput> { _inputs.DetachFixFixtureCyl1_1Bw, _inputs.DetachFixFixtureCyl1_2Bw }, _outputs.DetachFixFixtureCyl1Fw, _outputs.DetachFixFixtureCyl1Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl1FwBw, ECylinder.DetachFixFixtureCyl1FwBw.ToString());
            DetachFixFixtureCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl2FwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.DetachFixFixtureCyl2_1Fw, _inputs.DetachFixFixtureCyl2_2Fw }, new List<IDInput> { _inputs.DetachFixFixtureCyl2_1Bw, _inputs.DetachFixFixtureCyl2_2Bw }, _outputs.DetachFixFixtureCyl2Fw, _outputs.DetachFixFixtureCyl2Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl2FwBw, ECylinder.DetachFixFixtureCyl2FwBw.ToString());
            DetachFixFixtureCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneTrCylFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneTrCylFw }, new List<IDInput> { _inputs.RemoveZoneTrCylBw }, _outputs.RemoveZoneTrCylFw, _outputs.RemoveZoneTrCylBw)
                .SetIdentity((int)ECylinder.RemoveZoneTrCylFwBw, ECylinder.RemoveZoneTrCylFwBw.ToString());
            RemoveZoneTrCylFwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneZCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneZCyl1Up }, new List<IDInput> { _inputs.RemoveZoneZCyl1Down }, _outputs.RemoveZoneZCyl1Up, _outputs.RemoveZoneZCyl1Down)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl1UpDown, ECylinder.RemoveZoneZCyl1UpDown.ToString());
            RemoveZoneZCyl1UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneZCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneZCyl2Up }, new List<IDInput> { _inputs.RemoveZoneZCyl2Down }, _outputs.RemoveZoneZCyl2Up, _outputs.RemoveZoneZCyl2Down)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl2UpDown, ECylinder.RemoveZoneZCyl2UpDown.ToString());
            RemoveZoneZCyl2UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneCylClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneCyl1Clamp, _inputs.RemoveZoneCyl2Clamp, _inputs.RemoveZoneCyl3Clamp }, new List<IDInput> { _inputs.RemoveZoneCyl1Unclamp, _inputs.RemoveZoneCyl2Unclamp, _inputs.RemoveZoneCyl3Unclamp }, _outputs.RemoveZoneCylClamp, _outputs.RemoveZoneCylUnclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl1ClampUnclamp, ECylinder.RemoveZoneCyl1ClampUnclamp.ToString());
            RemoveZoneCylClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZonePusherCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZonePusherCyl1Up }, new List<IDInput> { _inputs.RemoveZonePusherCyl1Down }, _outputs.RemoveZonePusherCyl1Up, _outputs.RemoveZonePusherCyl1Down)
                .SetIdentity((int)ECylinder.RemoveZonePusherCyl1UpDown, ECylinder.RemoveZonePusherCyl1UpDown.ToString());
            RemoveZonePusherCyl1UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZonePusherCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZonePusherCyl2Up }, new List<IDInput> { _inputs.RemoveZonePusherCyl2Down }, _outputs.RemoveZonePusherCyl2Up, _outputs.RemoveZonePusherCyl2Down)
                .SetIdentity((int)ECylinder.RemoveZonePusherCyl2UpDown, ECylinder.RemoveZonePusherCyl2UpDown.ToString());
            RemoveZonePusherCyl2UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneFixCyl1FwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneFixCyl1_1Fw, _inputs.RemoveZoneFixCyl1_2Fw }, new List<IDInput> { _inputs.RemoveZoneFixCyl1_1Bw, _inputs.RemoveZoneFixCyl1_2Bw }, _outputs.RemoveZoneFixCyl1Fw, _outputs.RemoveZoneFixCyl1Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl1FwBw, ECylinder.RemoveZoneFixCyl1FwBw.ToString());
            RemoveZoneFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl2FwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneFixCyl2_1Fw, _inputs.RemoveZoneFixCyl2_2Fw }, new List<IDInput> { _inputs.RemoveZoneFixCyl2_1Bw, _inputs.RemoveZoneFixCyl2_2Bw }, _outputs.RemoveZoneFixCyl2Fw, _outputs.RemoveZoneFixCyl2Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl2FwBw, ECylinder.RemoveZoneFixCyl2FwBw.ToString());
            RemoveZoneFixCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            TransferInShuttleLRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferInShuttleL0Degree }, new List<IDInput> { _inputs.TransferInShuttleL180Degree }, _outputs.TransferInShuttleL0Degree, _outputs.TransferInShuttleL180Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleLRotate, ECylinder.TransferInShuttleLRotate.ToString());
            TransferInShuttleLRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            TransferInShuttleRRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferInShuttleR0Degree }, new List<IDInput> { _inputs.TransferInShuttleR180Degree }, _outputs.TransferInShuttleR0Degree, _outputs.TransferInShuttleR180Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleRRotate, ECylinder.TransferInShuttleRRotate.ToString());
            TransferInShuttleRRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            FixtureAlign1CylFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.FixtureAlign1CylFw }, new List<IDInput> { _inputs.FixtureAlign1CylBw }, _outputs.FixtureAlign1CylFw, _outputs.FixtureAlign1CylBw)
                .SetIdentity((int)ECylinder.FixtureAlign1CylFwBw, ECylinder.FixtureAlign1CylFwBw.ToString());
            FixtureAlign1CylFwBw.CylinderType = ECylinderType.ForwardBackward;

            FixtureAlign2CylFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.FixtureAlign2CylFw }, new List<IDInput> { _inputs.FixtureAlign2CylBw }, _outputs.FixtureAlign2CylFw, _outputs.FixtureAlign2CylBw)
                .SetIdentity((int)ECylinder.FixtureAlign2CylFwBw, ECylinder.FixtureAlign2CylFwBw.ToString());
            FixtureAlign2CylFwBw.CylinderType = ECylinderType.ForwardBackward;

            AlignStageLBrushCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageLBrushCylUp }, new List<IDInput> { _inputs.AlignStageLBrushCylDown }, _outputs.AlignStageLBrushCylUp, _outputs.AlignStageLBrushCylDown)
                .SetIdentity((int)ECylinder.AlignStageLBrushCylUpDown, ECylinder.AlignStageLBrushCylUpDown.ToString());
            AlignStageLBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageL1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageL1Align }, new List<IDInput> { _inputs.AlignStageL1Unalign }, _outputs.AlignStageL1Align, _outputs.AlignStageL1Unalign)
                .SetIdentity((int)ECylinder.AlignStageL1AlignUnalign, ECylinder.AlignStageL1AlignUnalign.ToString());
            AlignStageL1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageL2Align }, new List<IDInput> { _inputs.AlignStageL2Unalign }, _outputs.AlignStageL2Align, _outputs.AlignStageL2Unalign)
                .SetIdentity((int)ECylinder.AlignStageL2AlignUnalign, ECylinder.AlignStageL2AlignUnalign.ToString());
            AlignStageL2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL3AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageL3Align }, new List<IDInput> { _inputs.AlignStageL3Unalign }, _outputs.AlignStageL3Align, _outputs.AlignStageL3Unalign)
                .SetIdentity((int)ECylinder.AlignStageL3AlignUnalign, ECylinder.AlignStageL3AlignUnalign.ToString());
            AlignStageL3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageRBrushCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageRBrushCylUp }, new List<IDInput> { _inputs.AlignStageRBrushCylDown }, _outputs.AlignStageRBrushCylUp, _outputs.AlignStageRBrushCylDown)
                .SetIdentity((int)ECylinder.AlignStageRBrushCylUpDown, ECylinder.AlignStageRBrushCylUpDown.ToString());
            AlignStageRBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageR1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageR1Align }, new List<IDInput> { _inputs.AlignStageR1Unalign }, _outputs.AlignStageR1Align, _outputs.AlignStageR1Unalign)
                .SetIdentity((int)ECylinder.AlignStageR1AlignUnalign, ECylinder.AlignStageR1AlignUnalign.ToString());
            AlignStageR1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageR2Align }, new List<IDInput> { _inputs.AlignStageR2Unalign }, _outputs.AlignStageR2Align, _outputs.AlignStageR2Unalign)
                .SetIdentity((int)ECylinder.AlignStageR2AlignUnalign, ECylinder.AlignStageR2AlignUnalign.ToString());
            AlignStageR2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR3AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageR3Align }, new List<IDInput> { _inputs.AlignStageR3Unalign }, _outputs.AlignStageR3Align, _outputs.AlignStageR3Unalign)
                .SetIdentity((int)ECylinder.AlignStageR3AlignUnalign, ECylinder.AlignStageR3AlignUnalign.ToString());
            AlignStageR3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            GlassTransferCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.GlassTransferCyl1Up }, new List<IDInput> { _inputs.GlassTransferCyl1Down }, _outputs.GlassTransferCyl1Up, _outputs.GlassTransferCyl1Down)
                .SetIdentity((int)ECylinder.GlassTransferCyl1UpDown, ECylinder.GlassTransferCyl1UpDown.ToString());
            GlassTransferCyl1UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.GlassTransferCyl2Up }, new List<IDInput> { _inputs.GlassTransferCyl2Down }, _outputs.GlassTransferCyl2Up, _outputs.GlassTransferCyl2Down)
                .SetIdentity((int)ECylinder.GlassTransferCyl2UpDown, ECylinder.GlassTransferCyl2UpDown.ToString());
            GlassTransferCyl2UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl3UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.GlassTransferCyl3Up }, new List<IDInput> { _inputs.GlassTransferCyl3Down }, _outputs.GlassTransferCyl3Up, _outputs.GlassTransferCyl3Down)
                .SetIdentity((int)ECylinder.GlassTransferCyl3UpDown, ECylinder.GlassTransferCyl3UpDown.ToString());
            GlassTransferCyl3UpDown.CylinderType = ECylinderType.UpDown;

            WetCleanPusherRightUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.WetCleanPusherRightUp }, new List<IDInput> { _inputs.WetCleanPusherRightDown }, _outputs.WetCleanPusherRightUp, _outputs.WetCleanPusherRightDown)
                .SetIdentity((int)ECylinder.WetCleanPusherRightUpDown, ECylinder.WetCleanPusherRightUpDown.ToString());
            WetCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanPusherLeftUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.WetCleanPusherLeftUp }, new List<IDInput> { _inputs.WetCleanPusherLeftDown }, _outputs.WetCleanPusherLeftUp, _outputs.WetCleanPusherLeftDown)
                .SetIdentity((int)ECylinder.WetCleanPusherLeftUpDown, ECylinder.WetCleanPusherLeftUpDown.ToString());
            WetCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            TrRotateRightClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateRightClamp }, new List<IDInput> { _inputs.TrRotateRightUnclamp }, _outputs.TrRotateRightClamp, _outputs.TrRotateRightUnclamp)
                .SetIdentity((int)ECylinder.TrRotateRightClampUnclamp, ECylinder.TrRotateRightClampUnclamp.ToString());
            TrRotateRightClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TrRotateRightRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateLeft0Degree }, new List<IDInput> { _inputs.TrRotateRight180Degree }, _outputs.TrRotateRight0Degree, _outputs.TrRotateRight180Degree)
                .SetIdentity((int)ECylinder.TrRotateRightRotate, ECylinder.TrRotateRightRotate.ToString());
            TrRotateRightRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            TrRotateRightFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateRightFw }, new List<IDInput> { _inputs.TrRotateRightBw }, _outputs.TrRotateRightFw, _outputs.TrRotateRightBw)
                .SetIdentity((int)ECylinder.TrRotateRightFwBw, ECylinder.TrRotateRightFwBw.ToString());
            TrRotateRightFwBw.CylinderType = ECylinderType.ForwardBackward;

            TrRotateLeftClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateLeftClamp }, new List<IDInput> { _inputs.TrRotateLeftUnclamp }, _outputs.TrRotateLeftClamp, _outputs.TrRotateLeftUnclamp)
                .SetIdentity((int)ECylinder.TrRotateLeftClampUnclamp, ECylinder.TrRotateLeftClampUnclamp.ToString());
            TrRotateLeftClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TrRotateLeftRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateLeft0Degree }, new List<IDInput> { _inputs.TrRotateLeft180Degree }, _outputs.TrRotateLeft0Degree, _outputs.TrRotateLeft180Degree)
                .SetIdentity((int)ECylinder.TrRotateLeftRotate, ECylinder.TrRotateLeftRotate.ToString());
            TrRotateLeftRotate.CylinderType = ECylinderType.FlipUnflip; //Note

            TrRotateLeftFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateLeftFw }, new List<IDInput> { _inputs.TrRotateLeftBw }, _outputs.TrRotateLeftFw, _outputs.TrRotateLeftBw)
                .SetIdentity((int)ECylinder.TrRotateLeftFwBw, ECylinder.TrRotateLeftFwBw.ToString());
            TrRotateLeftFwBw.CylinderType = ECylinderType.ForwardBackward;

            AFCleanPusherRightUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AfCleanPusherRightUp }, new List<IDInput> { _inputs.AfCleanPusherRightDown }, _outputs.AfCleanPusherRightUp, _outputs.AfCleanPusherRightDown)
                .SetIdentity((int)ECylinder.AFCleanPusherRightUpDown, ECylinder.AFCleanPusherRightUpDown.ToString());
            AFCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanPusherLeftUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AfCleanPusherLeftUp }, new List<IDInput> { _inputs.AfCleanPusherLeftDown }, _outputs.AfCleanPusherLeftUp, _outputs.AfCleanPusherLeftDown)
                .SetIdentity((int)ECylinder.AFCleanPusherLeftUpDown, ECylinder.AFCleanPusherLeftUpDown.ToString());
            AFCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl1Up }, new List<IDInput> { _inputs.UnloadRobotCyl1Down }, _outputs.UnloadRobotCyl1Up, _outputs.UnloadRobotCyl1Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl1UpDown, ECylinder.UnloadRobotCyl1UpDown.ToString());
            UnloadRobotCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl2Up }, new List<IDInput> { _inputs.UnloadRobotCyl2Down }, _outputs.UnloadRobotCyl2Up, _outputs.UnloadRobotCyl2Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl2UpDown, ECylinder.UnloadRobotCyl2UpDown.ToString());
            UnloadRobotCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl3UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl3Up }, new List<IDInput> { _inputs.UnloadRobotCyl3Down }, _outputs.UnloadRobotCyl3Up, _outputs.UnloadRobotCyl3Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl3UpDown, ECylinder.UnloadRobotCyl3UpDown.ToString());
            UnloadRobotCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl4UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl4Up }, new List<IDInput> { _inputs.UnloadRobotCyl4Down }, _outputs.UnloadRobotCyl4Up, _outputs.UnloadRobotCyl4Down)
                .SetIdentity((int)ECylinder.UnloadRobotCyl4UpDown, ECylinder.UnloadRobotCyl4UpDown.ToString());
            UnloadRobotCyl4UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl1Up }, new List<IDInput> { _inputs.UnloadAlignCyl1Down }, _outputs.UnloadAlignCyl1Up, _outputs.UnloadAlignCyl1Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl1UpDown, ECylinder.UnloadAlignCyl1UpDown.ToString());
            UnloadAlignCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl2Up }, new List<IDInput> { _inputs.UnloadAlignCyl2Down }, _outputs.UnloadAlignCyl2Up, _outputs.UnloadAlignCyl2Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl2UpDown, ECylinder.UnloadAlignCyl2UpDown.ToString());
            UnloadAlignCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl3UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl3Up }, new List<IDInput> { _inputs.UnloadAlignCyl3Down }, _outputs.UnloadAlignCyl3Up, _outputs.UnloadAlignCyl3Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl3UpDown, ECylinder.UnloadAlignCyl3UpDown.ToString());
            UnloadAlignCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl4UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl4Up }, new List<IDInput> { _inputs.UnloadAlignCyl4Down }, _outputs.UnloadAlignCyl4Up, _outputs.UnloadAlignCyl4Down)
                .SetIdentity((int)ECylinder.UnloadAlignCyl4UpDown, ECylinder.UnloadAlignCyl4UpDown.ToString());
            UnloadAlignCyl4UpDown.CylinderType = ECylinderType.UpDown;
        }

        private readonly ICylinderFactory _cylinderFactory;
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
    }
}
