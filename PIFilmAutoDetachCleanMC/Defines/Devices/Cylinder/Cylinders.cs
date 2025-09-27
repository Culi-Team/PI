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
        public ICylinder RobotFixtureClampUnclamp { get; }

        // Fixture Align (machine fixture, per input pair, shared outputs)
        public ICylinder AlignFixtureCyl1FwBw { get; }
        public ICylinder AlignFixtureCyl2FwBw { get; }

        // Vinyl Clean
        public ICylinder VinylCleanRollerFwBw { get; }
        public ICylinder VinylCleanFixtureClampUnclamp { get; }
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
        public ICylinder DetachGlassShtVac1OnOff { get;  }
        public ICylinder DetachGlassShtVac2OnOff { get;  }
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
        public ICylinder RemoveZoneFixCyl1FwBw { get; }
        public ICylinder RemoveZoneFixCyl2FwBw { get; }

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
                .Create(new List<IDInput> { _inputs.InCstStopperUp }, new List<IDInput> { _inputs.InCstStopperDown }, _outputs.InCstStopperUp, _outputs.InCstStopperDown)
                .SetIdentity((int)ECylinder.InCstStopperUpDown, ECylinder.InCstStopperUpDown.ToString());
            InCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            // Out CST
            OutCstStopperUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCstStopperUp }, new List<IDInput> { _inputs.OutCstStopperDown }, _outputs.OutCstStopperUp, _outputs.OutCstStopperDown)
                .SetIdentity((int)ECylinder.OutCstStopperUpDown, ECylinder.OutCstStopperUpDown.ToString());
            OutCstStopperUpDown.CylinderType = ECylinderType.UpDown;

            // In CST Work (per input pair)
            InCstFixCyl1FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.InCstFixCyl1Fw },
                    new List<IDInput> { _inputs.InCstFixCyl1Bw },
                    _outputs.InCstFixCyl1Fw, _outputs.InCstFixCyl1Bw)
                .SetIdentity((int)ECylinder.InCstFixCyl1FwBw, ECylinder.InCstFixCyl1FwBw.ToString());
            InCstFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            InCstFixCyl2FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.InCstFixCyl2Fw },
                    new List<IDInput> { _inputs.InCstFixCyl2Bw },
                    _outputs.InCstFixCyl2Fw, _outputs.InCstFixCyl2Bw)
                .SetIdentity((int)ECylinder.InCstFixCyl2FwBw, ECylinder.InCstFixCyl2FwBw.ToString());
            InCstFixCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            InCstTiltCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InCstTiltCylUp }, new List<IDInput> { _inputs.InCstTiltCylDown }, _outputs.InCstTiltCylUp, _outputs.InCstTiltCylDown)
                .SetIdentity((int)ECylinder.InCstTiltCylUpDown, ECylinder.InCstTiltCylUpDown.ToString());
            InCstTiltCylUpDown.CylinderType = ECylinderType.UpDown;

            // Buffer CV Work
            BufferCvStopper1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.BufferCvStopper1Up }, new List<IDInput> { _inputs.BufferCvStopper1Down }, _outputs.BufferCvStopper1Up, _outputs.BufferCvStopper1Down)
                .SetIdentity((int)ECylinder.BufferCvStopper1UpDown, ECylinder.BufferCvStopper1UpDown.ToString());
            BufferCvStopper1UpDown.CylinderType = ECylinderType.UpDown;

            BufferCvStopper2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.BufferCvStopper2Up }, new List<IDInput> { _inputs.BufferCvStopper2Down }, _outputs.BufferCvStopper2Up, _outputs.BufferCvStopper2Down)
                .SetIdentity((int)ECylinder.BufferCvStopper2UpDown, ECylinder.BufferCvStopper2UpDown.ToString());
            BufferCvStopper2UpDown.CylinderType = ECylinderType.UpDown;

            // In CV Support
            InCvSupportUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InCvSupportUp }, new List<IDInput> { _inputs.InCvSupportDown }, _outputs.InCvSupportUp, _outputs.InCvSupportDown)
                .SetIdentity((int)ECylinder.InCvSupportUpDown, ECylinder.InCvSupportUpDown.ToString());
            InCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            // In CV Support Buffer
            InCvSupportBufferUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InCvSupportBufferUp }, new List<IDInput> { _inputs.InCvSupportBufferDown }, _outputs.InCvSupportBufferUp, _outputs.InCvSupportBufferDown)
                .SetIdentity((int)ECylinder.InCvSupportBufferUpDown, ECylinder.InCvSupportBufferUpDown.ToString());
            InCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            // Out CV Support Buffer
            OutCvSupportBufferUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCvSupportBufferUp }, new List<IDInput> { _inputs.OutCvSupportBufferDown }, _outputs.OutCvSupportBufferUp, _outputs.OutCvSupportBufferDown)
                .SetIdentity((int)ECylinder.OutCvSupportBufferUpDown, ECylinder.OutCvSupportBufferUpDown.ToString());
            OutCvSupportBufferUpDown.CylinderType = ECylinderType.UpDown;

            // Out CV Support
            OutCvSupportUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCvSupportUp }, new List<IDInput> { _inputs.OutCvSupportDown }, _outputs.OutCvSupportUp, _outputs.OutCvSupportDown)
                .SetIdentity((int)ECylinder.OutCvSupportUpDown, ECylinder.OutCvSupportUpDown.ToString());
            OutCvSupportUpDown.CylinderType = ECylinderType.UpDown;

            // Out CST Work (per input pair)
            OutCstFixCyl1FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.OutCstFixCyl1Fw },
                    new List<IDInput> { _inputs.OutCstFixCyl1Bw },
                    _outputs.OutCstFixCyl1Fw, _outputs.OutCstFixCyl1Bw)
                .SetIdentity((int)ECylinder.OutCstFixCyl1FwBw, ECylinder.OutCstFixCyl1FwBw.ToString());
            OutCstFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            OutCstFixCyl2FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.OutCstFixCyl2Fw },
                    new List<IDInput> { _inputs.OutCstFixCyl2Bw },
                    _outputs.OutCstFixCyl2Fw, _outputs.OutCstFixCyl2Bw)
                .SetIdentity((int)ECylinder.OutCstFixCyl2FwBw, ECylinder.OutCstFixCyl2FwBw.ToString());
            OutCstFixCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            OutCstTiltCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutCstTiltCylUp }, new List<IDInput> { _inputs.OutCstTiltCylDown }, _outputs.OutCstTiltCylUp, _outputs.OutCstTiltCylDown)
                .SetIdentity((int)ECylinder.OutCstTiltCylUpDown, ECylinder.OutCstTiltCylUpDown.ToString());
            OutCstTiltCylUpDown.CylinderType = ECylinderType.UpDown;

            // Robot 1 Load/Unload
            RobotFixtureAlignFwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.RobotFixtureAlign1Bw, _inputs.RobotFixtureAlign2Bw },
                    new List<IDInput> { _inputs.RobotFixtureAlign1Fw, _inputs.RobotFixtureAlign2Fw },
                    _outputs.RobotFixtureAlignFw, _outputs.RobotFixtureAlignBw)
                .SetIdentity((int)ECylinder.RobotFixtureAlign1FwBw, ECylinder.RobotFixtureAlign1FwBw.ToString());
            RobotFixtureAlignFwBw.CylinderType = ECylinderType.ForwardBackward;

            RobotFixtureClampUnclamp = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.RobotFixture1Clamp, _inputs.RobotFixture2Clamp },
                    new List<IDInput> { _inputs.RobotFixture1Unclamp, _inputs.RobotFixture2Unclamp },
                    _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotFixture1ClampUnclamp, ECylinder.RobotFixture1ClampUnclamp.ToString());
            RobotFixtureClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            // Fixture Align (machine)
            AlignFixtureCyl1FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.FixtureAlign1CylFw },
                    new List<IDInput> { _inputs.FixtureAlign1CylBw },
                    _outputs.FixtureAlignCyl1Fw, _outputs.FixtureAlignCyl1Bw)
                .SetIdentity((int)ECylinder.FixtureAlignCyl1FwBw, ECylinder.FixtureAlignCyl1FwBw.ToString());
            AlignFixtureCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            AlignFixtureCyl2FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.FixtureAlign2CylFw },
                    new List<IDInput> { _inputs.FixtureAlign2CylBw },
                    _outputs.FixtureAlignCyl2Fw, _outputs.FixtureAlignCyl2Bw)
                .SetIdentity((int)ECylinder.FixtureAlignCyl2FwBw, ECylinder.FixtureAlignCyl2FwBw.ToString());
            AlignFixtureCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            // Vinyl Clean
            VinylCleanRollerFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.VinylCleanRollerFw }, new List<IDInput> { _inputs.VinylCleanRollerBw }, _outputs.VinylCleanRollerFw, _outputs.VinylCleanRollerBw)
                .SetIdentity((int)ECylinder.VinylCleanRollerFwBw, ECylinder.VinylCleanRollerFwBw.ToString());
            VinylCleanRollerFwBw.CylinderType = ECylinderType.ForwardBackward;

            VinylCleanFixtureClampUnclamp = _cylinderFactory
                .Create(new List<IDInput> { _inputs.VinylCleanFixture1Clamp, _inputs.VinylCleanFixture2Clamp }, new List<IDInput> { _inputs.VinylCleanFixture1Unclamp, _inputs.VinylCleanFixture2Unclamp }, _outputs.VinylCleanFixtureClamp, _outputs.VinylCleanFixtureUnclamp)
                .SetIdentity((int)ECylinder.VinylCleanFixtureClampUnclamp, ECylinder.VinylCleanFixtureClampUnclamp.ToString());
            VinylCleanFixtureClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            VinylCleanPusherRollerUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.VinylCleanPusherRollerUp }, new List<IDInput> { _inputs.VinylCleanPusherRollerDown }, _outputs.VinylCleanPusherRollerUp, null)
                .SetIdentity((int)ECylinder.VinylCleanPusherRollerUpDown, ECylinder.VinylCleanPusherRollerUpDown.ToString());
            VinylCleanPusherRollerUpDown.CylinderType = ECylinderType.UpDown;

            // Transfer Y Fixture
            TransferFixtureUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferFixtureUp }, new List<IDInput> { _inputs.TransferFixtureDown }, _outputs.TransferFixtureUp, _outputs.TransferFixtureDown)
                .SetIdentity((int)ECylinder.TransferFixtureUpDown, ECylinder.TransferFixtureUpDown.ToString());
            TransferFixtureUpDown.CylinderType = ECylinderType.UpDown;

            TransferFixture1ClampUnclamp = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.TransferFixture11Unclamp, _inputs.TransferFixture12Unclamp },
                    new List<IDInput> { _inputs.TransferFixture11Clamp, _inputs.TransferFixture12Clamp },
                    _outputs.TransferFixture1Unclamp, _outputs.TransferFixture1Clamp)
                .SetIdentity((int)ECylinder.TransferFixture1ClampUnclamp, ECylinder.TransferFixture1ClampUnclamp.ToString());
            TransferFixture1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture2ClampUnclamp = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.TransferFixture21Unclamp, _inputs.TransferFixture22Unclamp },
                    new List<IDInput> { _inputs.TransferFixture21Clamp, _inputs.TransferFixture22Clamp },
                    _outputs.TransferFixture2Unclamp, _outputs.TransferFixture2Clamp)
                .SetIdentity((int)ECylinder.TransferFixture2ClampUnclamp, ECylinder.TransferFixture2ClampUnclamp.ToString());
            TransferFixture2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            // Detach Glass
            DetachCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.DetachCyl1Down }, new List<IDInput> { _inputs.DetachCyl1Up }, _outputs.DetachCyl1Down, _outputs.DetachCyl1Up)
                .SetIdentity((int)ECylinder.DetachCyl1UpDown, ECylinder.DetachCyl1UpDown.ToString());
            DetachCyl1UpDown.CylinderType = ECylinderType.UpDown;

            DetachCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.DetachCyl2Down }, new List<IDInput> { _inputs.DetachCyl2Up }, _outputs.DetachCyl2Down, _outputs.DetachCyl2Up)
                .SetIdentity((int)ECylinder.DetachCyl2UpDown, ECylinder.DetachCyl2UpDown.ToString());
            DetachCyl2UpDown.CylinderType = ECylinderType.UpDown;

            DetachFixFixtureCyl1FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.DetachFixFixtureCyl11Fw, _inputs.DetachFixFixtureCyl12Fw },
                    new List<IDInput> { _inputs.DetachFixFixtureCyl11Bw, _inputs.DetachFixFixtureCyl12Bw },
                    _outputs.DetachFixFixtureCyl1Fw, _outputs.DetachFixFixtureCyl1Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl1FwBw, ECylinder.DetachFixFixtureCyl1FwBw.ToString());
            DetachFixFixtureCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            DetachFixFixtureCyl2FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.DetachFixFixtureCyl21Fw, _inputs.DetachFixFixtureCyl22Fw },
                    new List<IDInput> { _inputs.DetachFixFixtureCyl21Bw, _inputs.DetachFixFixtureCyl22Bw },
                    _outputs.DetachFixFixtureCyl2Fw, _outputs.DetachFixFixtureCyl2Bw)
                .SetIdentity((int)ECylinder.DetachFixFixtureCyl2FwBw, ECylinder.DetachFixFixtureCyl2FwBw.ToString());
            DetachFixFixtureCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            // Remove Zone
            RemoveZoneTrCylFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneTrCylFw }, new List<IDInput> { _inputs.RemoveZoneTrCylBw }, _outputs.RemoveZoneTrCylFw, _outputs.RemoveZoneTrCylBw)
                .SetIdentity((int)ECylinder.RemoveZoneTrCylFwBw, ECylinder.RemoveZoneTrCylFwBw.ToString());
            RemoveZoneTrCylFwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneZCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneZCyl1Down }, new List<IDInput> { _inputs.RemoveZoneZCyl1Up }, _outputs.RemoveZoneZCyl1Down, null)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl1UpDown, ECylinder.RemoveZoneZCyl1UpDown.ToString());
            RemoveZoneZCyl1UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneZCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZoneZCyl2Down }, new List<IDInput> { _inputs.RemoveZoneZCyl2Up }, _outputs.RemoveZoneZCyl2Down, null)
                .SetIdentity((int)ECylinder.RemoveZoneZCyl2UpDown, ECylinder.RemoveZoneZCyl2UpDown.ToString());
            RemoveZoneZCyl2UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneCyl1ClampUnclamp = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.RemoveZoneCyl1Clamp },
                    new List<IDInput> { _inputs.RemoveZoneCyl1Unclamp },
                    _outputs.RemoveZoneCyl1Clamp, _outputs.RemoveZoneCyl1Unclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl1ClampUnclamp, ECylinder.RemoveZoneCyl1ClampUnclamp.ToString());
            RemoveZoneCyl1ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZoneCyl2ClampUnclamp = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.RemoveZoneCyl2Clamp },
                    new List<IDInput> { _inputs.RemoveZoneCyl2Unclamp },
                    _outputs.RemoveZoneCyl2Clamp, _outputs.RemoveZoneCyl2Unclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl2ClampUnclamp, ECylinder.RemoveZoneCyl2ClampUnclamp.ToString());
            RemoveZoneCyl2ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZoneCyl3ClampUnclamp = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.RemoveZoneCyl3Clamp },
                    new List<IDInput> { _inputs.RemoveZoneCyl3Unclamp },
                    _outputs.RemoveZoneCyl3Clamp, _outputs.RemoveZoneCyl3Unclamp)
                .SetIdentity((int)ECylinder.RemoveZoneCyl3ClampUnclamp, ECylinder.RemoveZoneCyl3ClampUnclamp.ToString());
            RemoveZoneCyl3ClampUnclamp.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZonePusherCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZonePusherCyl1Up }, new List<IDInput> { _inputs.RemoveZonePusherCyl1Down }, _outputs.RemoveZonePusherCyl1Up, _outputs.RemoveZonePusherCyl1Down)
                .SetIdentity((int)ECylinder.RemoveZonePusherCyl1UpDown, ECylinder.RemoveZonePusherCyl1UpDown.ToString());
            RemoveZonePusherCyl1UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZonePusherCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.RemoveZonePusherCyl2Up }, new List<IDInput> { _inputs.RemoveZonePusherCyl2Down }, _outputs.RemoveZonePusherCyl2Up, _outputs.RemoveZonePusherCyl2Down)
                .SetIdentity((int)ECylinder.RemoveZonePusherCyl2UpDown, ECylinder.RemoveZonePusherCyl2UpDown.ToString());
            RemoveZonePusherCyl2UpDown.CylinderType = ECylinderType.UpDown;

            RemoveZoneFixCyl1FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.RemoveZoneFixCyl11Fw, _inputs.RemoveZoneFixCyl12Fw },
                    new List<IDInput> { _inputs.RemoveZoneFixCyl11Bw, _inputs.RemoveZoneFixCyl12Bw },
                    _outputs.RemoveZoneFixCyl1Fw, _outputs.RemoveZoneFixCyl1Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl1FwBw, ECylinder.RemoveZoneFixCyl1FwBw.ToString());
            RemoveZoneFixCyl1FwBw.CylinderType = ECylinderType.ForwardBackward;

            RemoveZoneFixCyl2FwBw = _cylinderFactory
                .Create(
                    new List<IDInput> { _inputs.RemoveZoneFixCyl21Fw, _inputs.RemoveZoneFixCyl22Fw },
                    new List<IDInput> { _inputs.RemoveZoneFixCyl21Bw, _inputs.RemoveZoneFixCyl22Bw },
                    _outputs.RemoveZoneFixCyl2Fw, _outputs.RemoveZoneFixCyl2Bw)
                .SetIdentity((int)ECylinder.RemoveZoneFixCyl2FwBw, ECylinder.RemoveZoneFixCyl2FwBw.ToString());
            RemoveZoneFixCyl2FwBw.CylinderType = ECylinderType.ForwardBackward;

            // In Shuttle rotate
            TransferInShuttleLRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferInShuttleL0Degree }, new List<IDInput> { _inputs.TransferInShuttleL180Degree }, _outputs.TransferInShuttleL0Degree, _outputs.TransferInShuttleL180Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleLRotate, ECylinder.TransferInShuttleLRotate.ToString());
            TransferInShuttleLRotate.CylinderType = ECylinderType.FlipUnflip;

            TransferInShuttleRRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TransferInShuttleR0Degree }, new List<IDInput> { _inputs.TransferInShuttleR180Degree }, _outputs.TransferInShuttleR0Degree, _outputs.TransferInShuttleR180Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleRRotate, ECylinder.TransferInShuttleRRotate.ToString());
            TransferInShuttleRRotate.CylinderType = ECylinderType.FlipUnflip;

            // Align Stage L/R
            AlignStageLBrushCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageLBrushCylUp }, new List<IDInput> { _inputs.AlignStageLBrushCylDown }, _outputs.AlignStageLBrushCylUp, null)
                .SetIdentity((int)ECylinder.AlignStageLBrushCylUpDown, ECylinder.AlignStageLBrushCylUpDown.ToString());
            AlignStageLBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageL1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageL1Align }, new List<IDInput> { _inputs.AlignStageL1Unalign }, _outputs.AlignStageL1Align, null)
                .SetIdentity((int)ECylinder.AlignStageL1AlignUnalign, ECylinder.AlignStageL1AlignUnalign.ToString());
            AlignStageL1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageL2Align }, new List<IDInput> { _inputs.AlignStageL2Unalign }, _outputs.AlignStageL2Align, null)
                .SetIdentity((int)ECylinder.AlignStageL2AlignUnalign, ECylinder.AlignStageL2AlignUnalign.ToString());
            AlignStageL2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageL3AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageL3Align }, new List<IDInput> { _inputs.AlignStageL3Unalign }, _outputs.AlignStageL3Align, null)
                .SetIdentity((int)ECylinder.AlignStageL3AlignUnalign, ECylinder.AlignStageL3AlignUnalign.ToString());
            AlignStageL3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageRBrushCylUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageRBrushCylUp }, new List<IDInput> { _inputs.AlignStageRBrushCylDown }, _outputs.AlignStageRBrushCylUp, null)
                .SetIdentity((int)ECylinder.AlignStageRBrushCylUpDown, ECylinder.AlignStageRBrushCylUpDown.ToString());
            AlignStageRBrushCylUpDown.CylinderType = ECylinderType.UpDown;

            AlignStageR1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageR1Align }, new List<IDInput> { _inputs.AlignStageR1Unalign }, _outputs.AlignStageR1Align, null)
                .SetIdentity((int)ECylinder.AlignStageR1AlignUnalign, ECylinder.AlignStageR1AlignUnalign.ToString());
            AlignStageR1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageR2Align }, new List<IDInput> { _inputs.AlignStageR2Unalign }, _outputs.AlignStageR2Align, null)
                .SetIdentity((int)ECylinder.AlignStageR2AlignUnalign, ECylinder.AlignStageR2AlignUnalign.ToString());
            AlignStageR2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            AlignStageR3AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AlignStageR3Align }, new List<IDInput> { _inputs.AlignStageR3Unalign }, _outputs.AlignStageR3Align, null)
                .SetIdentity((int)ECylinder.AlignStageR3AlignUnalign, ECylinder.AlignStageR3AlignUnalign.ToString());
            AlignStageR3AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            // Glass Transfer (Transfer)
            GlassTransferCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.GlassTransferCyl1Down }, new List<IDInput> { _inputs.GlassTransferCyl1Up }, _outputs.GlassTransferCyl1Down, _outputs.GlassTransferCyl1Up)
                .SetIdentity((int)ECylinder.GlassTransferCyl1UpDown, ECylinder.GlassTransferCyl1UpDown.ToString());
            GlassTransferCyl1UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.GlassTransferCyl2Down }, new List<IDInput> { _inputs.GlassTransferCyl2Up }, _outputs.GlassTransferCyl2Down, _outputs.GlassTransferCyl2Up)
                .SetIdentity((int)ECylinder.GlassTransferCyl2UpDown, ECylinder.GlassTransferCyl2UpDown.ToString());
            GlassTransferCyl2UpDown.CylinderType = ECylinderType.UpDown;

            GlassTransferCyl3UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.GlassTransferCyl3Down }, new List<IDInput> { _inputs.GlassTransferCyl3Up }, _outputs.GlassTransferCyl3Down, _outputs.GlassTransferCyl3Up)
                .SetIdentity((int)ECylinder.GlassTransferCyl3UpDown, ECylinder.GlassTransferCyl3UpDown.ToString());
            GlassTransferCyl3UpDown.CylinderType = ECylinderType.UpDown;

            // Wet Clean
            WetCleanPusherRightUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.WetCleanPusherRightDown }, new List<IDInput> { _inputs.WetCleanPusherRightUp }, _outputs.WetCleanPusherRightDown, _outputs.WetCleanPusherRightUp)
                .SetIdentity((int)ECylinder.WetCleanPusherRightUpDown, ECylinder.WetCleanPusherRightUpDown.ToString());
            WetCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanPusherLeftUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.WetCleanPusherLeftDown }, new List<IDInput> { _inputs.WetCleanPusherLeftUp }, _outputs.WetCleanPusherLeftDown, _outputs.WetCleanPusherLeftUp)
                .SetIdentity((int)ECylinder.WetCleanPusherLeftUpDown, ECylinder.WetCleanPusherLeftUpDown.ToString());
            WetCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanBrushRightUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.WetCleanBrushRightDown }, new List<IDInput> { _inputs.WetCleanBrushRightUp }, _outputs.WetCleanBrushRightDown, null)
                .SetIdentity((int)ECylinder.WetCleanBrushRightUpDown, ECylinder.WetCleanBrushRightUpDown.ToString());
            WetCleanBrushRightUpDown.CylinderType = ECylinderType.UpDown;

            WetCleanBrushLeftUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.WetCleanBrushLeftDown }, new List<IDInput> { _inputs.WetCleanBrushLeftUp }, _outputs.WetCleanBrushLeftDown, null)
                .SetIdentity((int)ECylinder.WetCleanBrushLeftUpDown, ECylinder.WetCleanBrushLeftUpDown.ToString());
            WetCleanBrushLeftUpDown.CylinderType = ECylinderType.UpDown;

            // Transfer Rotate
            TrRotateRightRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateRight180Degree }, new List<IDInput> { _inputs.TrRotateRight0Degree }, _outputs.TrRotateRight180Degree, _outputs.TrRotateRight0Degree)
                .SetIdentity((int)ECylinder.TrRotateRightRotate, ECylinder.TrRotateRightRotate.ToString());
            TrRotateRightRotate.CylinderType = ECylinderType.FlipUnflip;

            TrRotateRightFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateRightFw }, new List<IDInput> { _inputs.TrRotateRightBw }, _outputs.TrRotateRightFw, _outputs.TrRotateRightBw)
                .SetIdentity((int)ECylinder.TrRotateRightFwBw, ECylinder.TrRotateRightFwBw.ToString());
            TrRotateRightFwBw.CylinderType = ECylinderType.ForwardBackward;

            TrRotateLeftRotate = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateLeft180Degree }, new List<IDInput> { _inputs.TrRotateLeft0Degree }, _outputs.TrRotateLeft180Degree, _outputs.TrRotateLeft0Degree)
                .SetIdentity((int)ECylinder.TrRotateLeftRotate, ECylinder.TrRotateLeftRotate.ToString());
            TrRotateLeftRotate.CylinderType = ECylinderType.FlipUnflip;

            TrRotateLeftFwBw = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateLeftFw }, new List<IDInput> { _inputs.TrRotateLeftBw }, _outputs.TrRotateLeftFw, _outputs.TrRotateLeftBw)
                .SetIdentity((int)ECylinder.TrRotateLeftFwBw, ECylinder.TrRotateLeftFwBw.ToString());
            TrRotateLeftFwBw.CylinderType = ECylinderType.ForwardBackward;

            TrRotateRightUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateRightDown }, new List<IDInput> { _inputs.TrRotateRightUp }, _outputs.TrRotateRightDown, _outputs.TrRotateRightUp)
                .SetIdentity((int)ECylinder.TrRotateRightUpDown, ECylinder.TrRotateRightUpDown.ToString());
            TrRotateRightUpDown.CylinderType = ECylinderType.UpDown;

            TrRotateLeftUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.TrRotateLeftDown }, new List<IDInput> { _inputs.TrRotateLeftUp }, _outputs.TrRotateLeftDown, _outputs.TrRotateLeftUp)
                .SetIdentity((int)ECylinder.TrRotateLeftUpDown, ECylinder.TrRotateLeftUpDown.ToString());
            TrRotateLeftUpDown.CylinderType = ECylinderType.UpDown;


            // AF Clean
            AFCleanPusherRightUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AfCleanPusherRightDown }, new List<IDInput> { _inputs.AfCleanPusherRightUp }, _outputs.AfCleanPusherRightDown, _outputs.AfCleanPusherRightUp)
                .SetIdentity((int)ECylinder.AFCleanPusherRightUpDown, ECylinder.AFCleanPusherRightUpDown.ToString());
            AFCleanPusherRightUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanPusherLeftUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AfCleanPusherLeftDown }, new List<IDInput> { _inputs.AfCleanPusherLeftUp }, _outputs.AfCleanPusherLeftDown, _outputs.AfCleanPusherLeftUp)
                .SetIdentity((int)ECylinder.AFCleanPusherLeftUpDown, ECylinder.AFCleanPusherLeftUpDown.ToString());
            AFCleanPusherLeftUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanBrushRightUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AfCleanBrushRightDown }, new List<IDInput> { _inputs.AfCleanBrushRightUp }, _outputs.AfCleanBrushRightDown, null)
                .SetIdentity((int)ECylinder.AFCleanBrushRightUpDown, ECylinder.AFCleanBrushRightUpDown.ToString());
            AFCleanBrushRightUpDown.CylinderType = ECylinderType.UpDown;

            AFCleanBrushLeftUpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.AfCleanBrushLeftDown }, new List<IDInput> { _inputs.AfCleanBrushLeftUp }, _outputs.AfCleanBrushLeftDown, null)
                .SetIdentity((int)ECylinder.AFCleanBrushLeftUpDown, ECylinder.AFCleanBrushLeftUpDown.ToString());
            AFCleanBrushLeftUpDown.CylinderType = ECylinderType.UpDown;

            // Robot 2 Unload
            UnloadRobotCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl1Down }, new List<IDInput> { _inputs.UnloadRobotCyl1Up }, _outputs.UnloadRobotCyl1Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl1UpDown, ECylinder.UnloadRobotCyl1UpDown.ToString());
            UnloadRobotCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl2Down }, new List<IDInput> { _inputs.UnloadRobotCyl2Up }, _outputs.UnloadRobotCyl2Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl2UpDown, ECylinder.UnloadRobotCyl2UpDown.ToString());
            UnloadRobotCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl3UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl3Down }, new List<IDInput> { _inputs.UnloadRobotCyl3Up }, _outputs.UnloadRobotCyl3Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl3UpDown, ECylinder.UnloadRobotCyl3UpDown.ToString());
            UnloadRobotCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadRobotCyl4UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadRobotCyl4Down }, new List<IDInput> { _inputs.UnloadRobotCyl4Up }, _outputs.UnloadRobotCyl4Down, null)
                .SetIdentity((int)ECylinder.UnloadRobotCyl4UpDown, ECylinder.UnloadRobotCyl4UpDown.ToString());
            UnloadRobotCyl4UpDown.CylinderType = ECylinderType.UpDown;

            // Unload Stage
            UnloadAlignCyl1UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl1Up }, new List<IDInput> { _inputs.UnloadAlignCyl1Down }, _outputs.UnloadAlignCyl1Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl1UpDown, ECylinder.UnloadAlignCyl1UpDown.ToString());
            UnloadAlignCyl1UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl2UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl2Up }, new List<IDInput> { _inputs.UnloadAlignCyl2Down }, _outputs.UnloadAlignCyl2Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl2UpDown, ECylinder.UnloadAlignCyl2UpDown.ToString());
            UnloadAlignCyl2UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl3UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl3Up }, new List<IDInput> { _inputs.UnloadAlignCyl3Down }, _outputs.UnloadAlignCyl3Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl3UpDown, ECylinder.UnloadAlignCyl3UpDown.ToString());
            UnloadAlignCyl3UpDown.CylinderType = ECylinderType.UpDown;

            UnloadAlignCyl4UpDown = _cylinderFactory
                .Create(new List<IDInput> { _inputs.UnloadAlignCyl4Up }, new List<IDInput> { _inputs.UnloadAlignCyl4Down }, _outputs.UnloadAlignCyl4Up, null)
                .SetIdentity((int)ECylinder.UnloadAlignCyl4UpDown, ECylinder.UnloadAlignCyl4UpDown.ToString());
            UnloadAlignCyl4UpDown.CylinderType = ECylinderType.UpDown;

            //In Shuttle
            InShuttleL1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InShuttleLAlign1FW }, new List<IDInput> { _inputs.InShuttleLAlign1BW }, _outputs.InShuttleLAlignFw, _outputs.InShuttleLAlignBw)
                .SetIdentity((int)ECylinder.InShuttleL1AlignUnalign, ECylinder.InShuttleL1AlignUnalign.ToString());
            InShuttleL1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            InShuttleL2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InShuttleLAlign2FW }, new List<IDInput> { _inputs.InShuttleLAlign2BW }, _outputs.InShuttleLAlignFw, _outputs.InShuttleLAlignBw)
                .SetIdentity((int)ECylinder.InShuttleL2AlignUnalign, ECylinder.InShuttleL2AlignUnalign.ToString());
            InShuttleL2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            InShuttleR1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InShuttleRAlign1FW }, new List<IDInput> { _inputs.InShuttleRAlign1BW }, _outputs.InShuttleRAlignFw, _outputs.InShuttleRAlignBw)
                .SetIdentity((int)ECylinder.InShuttleR1AlignUnalign, ECylinder.InShuttleR1AlignUnalign.ToString());
            InShuttleR1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            InShuttleR2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.InShuttleRAlign2FW }, new List<IDInput> { _inputs.InShuttleRAlign2BW }, _outputs.InShuttleRAlignFw, _outputs.InShuttleRAlignBw)
                .SetIdentity((int)ECylinder.InShuttleR2AlignUnalign, ECylinder.InShuttleR2AlignUnalign.ToString());
            InShuttleR2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            //Out Shuttle
            OutShuttleL1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutShuttleLAlign1FW }, new List<IDInput> { _inputs.OutShuttleLAlign1BW }, _outputs.OutShuttleLAlignFw, _outputs.OutShuttleLAlignBw)
                .SetIdentity((int)ECylinder.OutShuttleL1AlignUnalign, ECylinder.OutShuttleL1AlignUnalign.ToString());
            OutShuttleL1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            OutShuttleL2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutShuttleLAlign2FW }, new List<IDInput> { _inputs.OutShuttleLAlign2BW }, _outputs.OutShuttleLAlignFw, _outputs.OutShuttleLAlignBw)
                .SetIdentity((int)ECylinder.OutShuttleL2AlignUnalign, ECylinder.OutShuttleL2AlignUnalign.ToString());
            OutShuttleL2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            OutShuttleR1AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutShuttleRAlign1FW }, new List<IDInput> { _inputs.OutShuttleRAlign1BW }, _outputs.OutShuttleRAlignFw, _outputs.OutShuttleRAlignBw)
                .SetIdentity((int)ECylinder.OutShuttleR1AlignUnalign, ECylinder.OutShuttleR1AlignUnalign.ToString());
            OutShuttleR1AlignUnalign.CylinderType = ECylinderType.AlignUnalign;

            OutShuttleR2AlignUnalign = _cylinderFactory
                .Create(new List<IDInput> { _inputs.OutShuttleRAlign2FW }, new List<IDInput> { _inputs.OutShuttleRAlign2BW }, _outputs.OutShuttleRAlignFw, _outputs.OutShuttleRAlignBw)
                .SetIdentity((int)ECylinder.InShuttleR2AlignUnalign, ECylinder.InShuttleR2AlignUnalign.ToString());
            OutShuttleR2AlignUnalign.CylinderType = ECylinderType.AlignUnalign;
        }

        private readonly ICylinderFactory _cylinderFactory;
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
    }
}
