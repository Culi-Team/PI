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
        public ICylinder InCV_StopperCyl { get; }

        // Out CST
        public ICylinder OutCV_StopperCyl { get; }

        // In CST Work (per input pair)
        public ICylinder InWorkCV_AlignCyl1 { get; }
        public ICylinder InWorkCV_AlignCyl2 { get; }
        public ICylinder InWorkCV_TiltCyl { get; }

        // Buffer CV Work
        public ICylinder BufferCV_StopperCyl1 { get; }
        public ICylinder BufferCV_StopperCyl2 { get; }

        // In CV Support
        public ICylinder InWorkCV_SupportCyl1 { get; }

        // In CV Support Buffer
        public ICylinder InWorkCV_SupportCyl2 { get; }

        // Out CV Support Buffer
        public ICylinder OutWorkCV_SupportCyl1 { get; }

        // Out CV Support
        public ICylinder OutWorkCV_SupportCyl2 { get; }

        // Out CST Work (per input pair)
        public ICylinder OutWorkCV_AlignCyl1 { get; }
        public ICylinder OutWorkCV_AlignCyl2 { get; }
        public ICylinder OutWorkCV_TiltCyl { get; }

        // Robot 1 Load/Unload (per cylinder by inputs, shared outputs where applicable)
        public ICylinder RobotLoad_AlignCyl { get; }
        public ICylinder RobotLoad_ClampCyl1 { get; }
        public ICylinder RobotLoad_ClampCyl2 { get; }

        // Fixture Align (machine fixture, per input pair, shared outputs)
        public ICylinder FixtureAlign_AlignCyl1 { get; }
        public ICylinder FixtureAlign_AlignCyl2 { get; }

        // Vinyl Clean
        public ICylinder VinylClean_BwFwCyl { get; }
        public ICylinder VinylClean_ClampCyl1 { get; }
        public ICylinder VinylClean_ClampCyl2 { get; }
        public ICylinder VinylClean_UpDownCyl { get; }

        // Transfer Y Fixture
        public ICylinder TransferFixture_UpDownCyl { get; }
        public ICylinder TransferFixture_ClampCyl1 { get; }
        public ICylinder TransferFixture_ClampCyl2 { get; }
        public ICylinder TransferFixture_ClampCyl3 { get; }
        public ICylinder TransferFixture_ClampCyl4 { get; }

        // Detach Glass
        public ICylinder Detach_UpDownCyl1 { get; }
        public ICylinder Detach_UpDownCyl2 { get; }
        public ICylinder Detach_ClampCyl1 { get; }
        public ICylinder Detach_ClampCyl2 { get; }
        public ICylinder Detach_ClampCyl3 { get; }
        public ICylinder Detach_ClampCyl4 { get; }

        // Remove Zone
        public ICylinder RemoveZone_TransferCyl { get; }
        public ICylinder RemoveZone_UpDownCyl1 { get; }
        public ICylinder RemoveZone_UpDownCyl2 { get; }
        public ICylinder RemoveZone_FilmClampCyl1 { get; }
        public ICylinder RemoveZone_FilmClampCyl2 { get; }
        public ICylinder RemoveZone_FilmClampCyl3 { get; }
        public ICylinder RemoveZone_PusherCyl1 { get; }
        public ICylinder RemoveZone_PusherCyl2 { get; }
        public ICylinder RemoveZone_ClampCyl1 { get; }
        public ICylinder RemoveZone_ClampCyl2 { get; }
        public ICylinder RemoveZone_ClampCyl3 { get; }
        public ICylinder RemoveZone_ClampCyl4 { get; }

        // In Shuttle (rotate 0°/180°)
        public ICylinder TransferInShuttleL_RotateCyl { get; }
        public ICylinder TransferInShuttleR_RotateCyl { get; }

        // Align Stage L/R
        public ICylinder AlignStageL_BrushCyl { get; }
        public ICylinder AlignStageL_AlignCyl1 { get; }
        public ICylinder AlignStageL_AlignCyl2 { get; }
        public ICylinder AlignStageL_AlignCyl3 { get; }
        public ICylinder AlignStageR_BrushCyl { get; }
        public ICylinder AlignStageR_AlignCyl1 { get; }
        public ICylinder AlignStageR_AlignCyl2 { get; }
        public ICylinder AlignStageR_AlignCyl3 { get; }

        // Glass Transfer (enum uses 'Transfer')
        public ICylinder GlassTransfer_UpDownCyl1 { get; }
        public ICylinder GlassTransfer_UpDownCyl2 { get; }
        public ICylinder GlassTransfer_UpDownCyl3 { get; }

        // Wet Clean
        public ICylinder WetCleanR_PusherCyl { get; }
        public ICylinder WetCleanL_PusherCyl { get; }
        public ICylinder WetCleanR_BrushCyl { get; }
        public ICylinder WetCleanL_BrushCyl { get; }

        public ICylinder WetCleanR_ClampCyl1 { get; }
        public ICylinder WetCleanR_ClampCyl2 { get; }
        public ICylinder WetCleanL_ClampCyl1 { get; }
        public ICylinder WetCleanL_ClampCyl2 { get; }

        // Transfer Rotate
        public ICylinder TransferRotationR_RotationCyl { get; }
        public ICylinder TransferRotationR_BwFwCyl { get; }
        public ICylinder TransferRotationL_RotationCyl { get; }
        public ICylinder TransferRotationL_BwFwCyl { get; }
        public ICylinder TransferRotationR_UpDownCyl { get; }
        public ICylinder TransferRotationL_UpDownCyl { get; }

        // AF Clean
        public ICylinder AFCleanR_PusherCyl { get; }
        public ICylinder AFCleanL_PusherCyl { get; }
        public ICylinder AFCleanR_BrushCyl { get; }
        public ICylinder AFCleanL_BrushCyl { get; }

        // Robot 2 Unload
        public ICylinder UnloadRobot_UpDownCyl1 { get; }
        public ICylinder UnloadRobot_UpDownCyl2 { get; }
        public ICylinder UnloadRobot_UpDownCyl3 { get; }
        public ICylinder UnloadRobot_UpDownCyl4 { get; }

        // Unload Stage
        public ICylinder UnloadAlign_UpDownCyl1 { get; }
        public ICylinder UnloadAlign_UpDownCyl2 { get; }
        public ICylinder UnloadAlign_UpDownCyl3 { get; }
        public ICylinder UnloadAlign_UpDownCyl4 { get; }

        //In Shuttle
        public ICylinder InShuttleR_ClampCyl1 { get; }
        public ICylinder InShuttleR_ClampCyl2 { get; }
        public ICylinder InShuttleL_ClampCyl1 { get; }
        public ICylinder InShuttleL_ClampCyl2 { get; }

        //Out Shuttle
        public ICylinder OutShuttleR_ClampCyl1 { get; }
        public ICylinder OutShuttleR_ClampCyl2 { get; }
        public ICylinder OutShuttleL_ClampCyl1 { get; }
        public ICylinder OutShuttleL_ClampCyl2 { get; }

        public Cylinders(ICylinderFactory cylinderFactory, Inputs inputs, Outputs outputs)
        {
            _cylinderFactory = cylinderFactory;
            _inputs = inputs;
            _outputs = outputs;

            // In CST
            InCV_StopperCyl = _cylinderFactory
                .Create(_inputs.InCV_StopperUp, _inputs.InCV_StopperDown, _outputs.InCst_StopperUp, _outputs.InCst_StopperDown)
                .SetIdentity((int)ECylinder.InCV_StopperUpDown, ECylinder.InCV_StopperUpDown.ToString());
            InCV_StopperCyl.CylinderType = ECylinderType.UpDown;

            // Out CST
            OutCV_StopperCyl = _cylinderFactory
                .Create(_inputs.OutCV_StopperUp, _inputs.OutCV_StopperDown, _outputs.OutCst_StopperUp, _outputs.OutCst_StopperDown)
                .SetIdentity((int)ECylinder.OutCV_StopperUpDown, ECylinder.OutCV_StopperUpDown.ToString());
            OutCV_StopperCyl.CylinderType = ECylinderType.UpDown;

            // In CST Work (per input pair)
            InWorkCV_AlignCyl1 = _cylinderFactory
                .Create(_inputs.InWorkCV_AlignCyl1Fw, _inputs.InWorkCV_AlignCyl1Bw, _outputs.InWorkCst_AlignCyl1Fw, _outputs.InWorkCst_AlignCyl1Bw)
                .SetIdentity((int)ECylinder.InWorkCV_AlignCyl1, ECylinder.InWorkCV_AlignCyl1.ToString());
            InWorkCV_AlignCyl1.CylinderType = ECylinderType.ForwardBackward;

            InWorkCV_AlignCyl2 = _cylinderFactory
                .Create(_inputs.InWorkCV_AlignCyl2Fw, _inputs.InWorkCV_AlignCyl2Bw, _outputs.InWorkCst_AlignCyl2Fw, _outputs.InWorkCst_AlignCyl2Bw)
                .SetIdentity((int)ECylinder.InWorkCV_AlignCyl2, ECylinder.InWorkCV_AlignCyl2.ToString());
            InWorkCV_AlignCyl2.CylinderType = ECylinderType.ForwardBackward;

            InWorkCV_TiltCyl = _cylinderFactory
                .Create(_inputs.InWorkCV_TiltCylUp, _inputs.InWorkCV_TiltCylDown, _outputs.InWorkCst_TiltCylUp, _outputs.InWorkCst_TiltCylDown)
                .SetIdentity((int)ECylinder.InWorkCV_TiltCyl, ECylinder.InWorkCV_TiltCyl.ToString());
            InWorkCV_TiltCyl.CylinderType = ECylinderType.UpDown;

            // Buffer CV Work
            BufferCV_StopperCyl1 = _cylinderFactory
                .Create(_inputs.BufferCV_StopperCyl1Up, _inputs.BufferCV_StopperCyl1Down, _outputs.BufferCvStopper1Up, _outputs.BufferCvStopper1Down)
                .SetIdentity((int)ECylinder.BufferCV_StopperCyl1, ECylinder.BufferCV_StopperCyl1.ToString());
            BufferCV_StopperCyl1.CylinderType = ECylinderType.UpDown;

            BufferCV_StopperCyl2 = _cylinderFactory
                .Create(_inputs.BufferCV_StopperCyl2Up, _inputs.BufferCV_StopperCyl2Down, _outputs.BufferCvStopper2Up, _outputs.BufferCvStopper2Down)
                .SetIdentity((int)ECylinder.BufferCV_StopperCyl2, ECylinder.BufferCV_StopperCyl2.ToString());
            BufferCV_StopperCyl2.CylinderType = ECylinderType.UpDown;

            // In CV Support
            InWorkCV_SupportCyl1 = _cylinderFactory
                .Create(_inputs.InCvSupportUp, _inputs.InCvSupportDown, _outputs.InCvSupportUp, _outputs.InCvSupportDown)
                .SetIdentity((int)ECylinder.InWorkCV_SupportCyl1, ECylinder.InWorkCV_SupportCyl1.ToString());
            InWorkCV_SupportCyl1.CylinderType = ECylinderType.UpDown;

            // In CV Support Buffer
            InWorkCV_SupportCyl2 = _cylinderFactory
                .Create(_inputs.InCvSupportBufferUp, _inputs.InCvSupportBufferDown, _outputs.InCvSupportBufferUp, _outputs.InCvSupportBufferDown)
                .SetIdentity((int)ECylinder.InWorkCV_SupportCyl2, ECylinder.InWorkCV_SupportCyl2.ToString());
            InWorkCV_SupportCyl2.CylinderType = ECylinderType.UpDown;

            // Out CV Support Buffer
            OutWorkCV_SupportCyl1 = _cylinderFactory
                .Create(_inputs.OutCvSupportBufferUp, _inputs.OutCvSupportBufferDown, _outputs.OutCvSupportBufferUp, _outputs.OutCvSupportBufferDown)
                .SetIdentity((int)ECylinder.OutWorkCV_SupportCyl1, ECylinder.OutWorkCV_SupportCyl1.ToString());
            OutWorkCV_SupportCyl1.CylinderType = ECylinderType.UpDown;

            // Out CV Support
            OutWorkCV_SupportCyl2 = _cylinderFactory
                .Create(_inputs.OutCvSupportUp, _inputs.OutCvSupportDown, _outputs.OutCvSupportUp, _outputs.OutCvSupportDown)
                .SetIdentity((int)ECylinder.OutWorkCV_SupportCyl2, ECylinder.OutWorkCV_SupportCyl2.ToString());
            OutWorkCV_SupportCyl2.CylinderType = ECylinderType.UpDown;

            // Out CST Work (per input pair)
            OutWorkCV_AlignCyl1 = _cylinderFactory
                .Create(_inputs.OutWorkCV_AlignCyl1Fw, _inputs.OutWorkCV_AlignCyl1Bw, _outputs.OutWorkCst_AlignCyl1Fw, _outputs.OutWorkCst_AlignCyl1Bw)
                .SetIdentity((int)ECylinder.OutWorkCV_AlignCyl1, ECylinder.OutWorkCV_AlignCyl1.ToString());
            OutWorkCV_AlignCyl1.CylinderType = ECylinderType.ForwardBackward;

            OutWorkCV_AlignCyl2 = _cylinderFactory
                .Create(_inputs.OutWorkCV_AlignCyl2Fw, _inputs.OutWorkCV_AlignCyl2Bw, _outputs.OutWorkCst_AlignCyl2Fw, _outputs.OutWorkCst_AlignCyl2Bw)
                .SetIdentity((int)ECylinder.OutWorkCV_AlignCyl2, ECylinder.OutWorkCV_AlignCyl2.ToString());
            OutWorkCV_AlignCyl2.CylinderType = ECylinderType.ForwardBackward;

            OutWorkCV_TiltCyl = _cylinderFactory
                .Create(_inputs.OutWorkCV_TiltCylUp, _inputs.OutWorkCV_TiltCylDown, _outputs.OutWorkCst_TiltCylUp, _outputs.OutWorkCst_TiltCylDown)
                .SetIdentity((int)ECylinder.OutWorkCV_TiltCyl, ECylinder.OutWorkCV_TiltCyl.ToString());
            OutWorkCV_TiltCyl.CylinderType = ECylinderType.UpDown;

            // Robot 1 Load/Unload
            RobotLoad_AlignCyl = _cylinderFactory
                .Create(_inputs.RobotFixtureAlignFw, _inputs.RobotFixtureAlignBw, _outputs.RobotFixtureAlignFw, _outputs.RobotFixtureAlignBw)
                .SetIdentity((int)ECylinder.RobotLoad_AlignCyl, ECylinder.RobotLoad_AlignCyl.ToString());
            RobotLoad_AlignCyl.CylinderType = ECylinderType.ForwardBackward;

            RobotLoad_ClampCyl1 = _cylinderFactory
                .Create(_inputs.RobotFixture1Clamp, _inputs.RobotFixture1Unclamp, _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotLoad_ClampCyl1, ECylinder.RobotLoad_ClampCyl1.ToString());
            RobotLoad_ClampCyl1.CylinderType = ECylinderType.ClampUnclamp;

            RobotLoad_ClampCyl2 = _cylinderFactory
                .Create(_inputs.RobotFixture2Clamp, _inputs.RobotFixture2Unclamp, _outputs.RobotFixtureClamp, _outputs.RobotFixtureUnclamp)
                .SetIdentity((int)ECylinder.RobotLoad_ClampCyl2, ECylinder.RobotLoad_ClampCyl2.ToString());
            RobotLoad_ClampCyl2.CylinderType = ECylinderType.ClampUnclamp;

            // Fixture Align (machine)
            FixtureAlign_AlignCyl1 = _cylinderFactory
                .Create(_inputs.FixtureAlign1CylFw, _inputs.FixtureAlign1CylBw, _outputs.FixtureAlignCyl1Fw, _outputs.FixtureAlignCyl1Bw)
                .SetIdentity((int)ECylinder.FixtureAlign_AlignCyl1, ECylinder.FixtureAlign_AlignCyl1.ToString());
            FixtureAlign_AlignCyl1.CylinderType = ECylinderType.ForwardBackward;

            FixtureAlign_AlignCyl2 = _cylinderFactory
                .Create(_inputs.FixtureAlign2CylFw, _inputs.FixtureAlign2CylBw, _outputs.FixtureAlignCyl2Fw, _outputs.FixtureAlignCyl2Bw)
                .SetIdentity((int)ECylinder.FixtureAlign_AlignCyl2, ECylinder.FixtureAlign_AlignCyl2.ToString());
            FixtureAlign_AlignCyl2.CylinderType = ECylinderType.ForwardBackward;

            // Vinyl Clean
            VinylClean_BwFwCyl = _cylinderFactory
                .Create(_inputs.VinylCleanRollerFw, _inputs.VinylCleanRollerBw, _outputs.VinylCleanRollerFw, _outputs.VinylCleanRollerBw)
                .SetIdentity((int)ECylinder.VinylClean_BwFwCyl, ECylinder.VinylClean_BwFwCyl.ToString());
            VinylClean_BwFwCyl.CylinderType = ECylinderType.ForwardBackward;

            VinylClean_ClampCyl1 = _cylinderFactory
                .Create(_inputs.VinylCleanFixture1Clamp, _inputs.VinylCleanFixture1Unclamp, _outputs.VinylCleanFixtureClamp, _outputs.VinylCleanFixtureUnclamp)
                .SetIdentity((int)ECylinder.VinylClean_ClampCyl1, ECylinder.VinylClean_ClampCyl1.ToString());
            VinylClean_ClampCyl1.CylinderType = ECylinderType.ClampUnclamp;

            VinylClean_ClampCyl2 = _cylinderFactory
                .Create(_inputs.VinylCleanFixture2Clamp, _inputs.VinylCleanFixture2Unclamp, _outputs.VinylCleanFixtureClamp, _outputs.VinylCleanFixtureUnclamp)
                .SetIdentity((int)ECylinder.VinylClean_ClampCyl2, ECylinder.VinylClean_ClampCyl2.ToString());
            VinylClean_ClampCyl2.CylinderType = ECylinderType.ClampUnclamp;

            VinylClean_UpDownCyl = _cylinderFactory
                .Create(_inputs.VinylCleanPusherRollerUp, _inputs.VinylCleanPusherRollerDown, _outputs.VinylCleanPusherRollerUp, null)
                .SetIdentity((int)ECylinder.VinylClean_UpDownCyl, ECylinder.VinylClean_UpDownCyl.ToString());
            VinylClean_UpDownCyl.CylinderType = ECylinderType.UpDown;

            // Transfer Y Fixture
            TransferFixture_UpDownCyl = _cylinderFactory
                .Create(_inputs.TransferFixtureUp, _inputs.TransferFixtureDown, _outputs.TransferFixtureUp, _outputs.TransferFixtureDown)
                .SetIdentity((int)ECylinder.TransferFixture_UpDownCyl, ECylinder.TransferFixture_UpDownCyl.ToString());
            TransferFixture_UpDownCyl.CylinderType = ECylinderType.UpDown;

            TransferFixture_ClampCyl1 = _cylinderFactory
                .Create(_inputs.TransferFixture11Clamp, _inputs.TransferFixture11Unclamp, _outputs.TransferFixture1Clamp, _outputs.TransferFixture1Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture_ClampCyl1, ECylinder.TransferFixture_ClampCyl1.ToString());
            TransferFixture_ClampCyl1.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture_ClampCyl2 = _cylinderFactory
                .Create(_inputs.TransferFixture12Clamp, _inputs.TransferFixture12Unclamp, _outputs.TransferFixture1Clamp, _outputs.TransferFixture1Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture_ClampCyl2, ECylinder.TransferFixture_ClampCyl2.ToString());
            TransferFixture_ClampCyl2.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture_ClampCyl3 = _cylinderFactory
                .Create(_inputs.TransferFixture21Clamp, _inputs.TransferFixture21Unclamp, _outputs.TransferFixture2Clamp, _outputs.TransferFixture2Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture_ClampCyl3, ECylinder.TransferFixture_ClampCyl3.ToString());
            TransferFixture_ClampCyl3.CylinderType = ECylinderType.ClampUnclamp;

            TransferFixture_ClampCyl4 = _cylinderFactory
                .Create(_inputs.TransferFixture22Clamp, _inputs.TransferFixture22Unclamp, _outputs.TransferFixture2Clamp, _outputs.TransferFixture2Unclamp)
                .SetIdentity((int)ECylinder.TransferFixture_ClampCyl4, ECylinder.TransferFixture_ClampCyl4.ToString());
            TransferFixture_ClampCyl4.CylinderType = ECylinderType.ClampUnclamp;

            // Detach Glass
            Detach_UpDownCyl1 = _cylinderFactory
                .Create(_inputs.DetachCyl1Down, _inputs.DetachCyl1Up, _outputs.DetachCyl1Down, _outputs.DetachCyl1Up)
                .SetIdentity((int)ECylinder.Detach_UpDownCyl1, ECylinder.Detach_UpDownCyl1.ToString());
            Detach_UpDownCyl1.CylinderType = ECylinderType.UpDownReverse;

            Detach_UpDownCyl2 = _cylinderFactory
                .Create(_inputs.DetachCyl2Down, _inputs.DetachCyl2Up, _outputs.DetachCyl2Down, _outputs.DetachCyl2Up)
                .SetIdentity((int)ECylinder.Detach_UpDownCyl2, ECylinder.Detach_UpDownCyl2.ToString());
            Detach_UpDownCyl2.CylinderType = ECylinderType.UpDownReverse;

            Detach_ClampCyl1 = _cylinderFactory
                .Create(_inputs.DetachCyl1Clamp, _inputs.DetachCyl1UnClamp, _outputs.DetachClampCyl1Clamp, _outputs.DetachClampCyl1Unclamp)
                .SetIdentity((int)ECylinder.Detach_ClampCyl1, ECylinder.Detach_ClampCyl1.ToString());
            Detach_ClampCyl1.CylinderType = ECylinderType.ClampUnclamp;

            Detach_ClampCyl2 = _cylinderFactory
                .Create(_inputs.DetachCyl2Clamp, _inputs.DetachCyl2UnClamp, _outputs.DetachClampCyl2Clamp, _outputs.DetachClampCyl2Unclamp)
                .SetIdentity((int)ECylinder.Detach_ClampCyl2, ECylinder.Detach_ClampCyl2.ToString());
            Detach_ClampCyl2.CylinderType = ECylinderType.ClampUnclamp;

            Detach_ClampCyl3 = _cylinderFactory
                .Create(_inputs.DetachCyl3Clamp, _inputs.DetachCyl3UnClamp, _outputs.DetachClampCyl3Clamp, _outputs.DetachClampCyl3Unclamp)
                .SetIdentity((int)ECylinder.Detach_ClampCyl3, ECylinder.Detach_ClampCyl3.ToString());
            Detach_ClampCyl3.CylinderType = ECylinderType.ClampUnclamp;

            Detach_ClampCyl4 = _cylinderFactory
                .Create(_inputs.DetachCyl4Clamp, _inputs.DetachCyl4Unclamp, _outputs.DetachClampCyl4Clamp, _outputs.DetachClampCyl4Unclamp)
                .SetIdentity((int)ECylinder.Detach_ClampCyl4, ECylinder.Detach_ClampCyl4.ToString());
            Detach_ClampCyl4.CylinderType = ECylinderType.ClampUnclamp;

            // Remove Zone
            RemoveZone_TransferCyl = _cylinderFactory
                .Create(_inputs.RemoveZoneTrCylFw, _inputs.RemoveZoneTrCylBw, _outputs.RemoveZoneTrCylFw, _outputs.RemoveZoneTrCylBw)
                .SetIdentity((int)ECylinder.RemoveZone_TransferCyl, ECylinder.RemoveZone_TransferCyl.ToString());
            RemoveZone_TransferCyl.CylinderType = ECylinderType.ForwardBackward;

            RemoveZone_UpDownCyl1 = _cylinderFactory
                .Create(_inputs.RemoveZoneZCyl1Down, _inputs.RemoveZoneZCyl1Up, _outputs.RemoveZoneZCyl1Down, null)
                .SetIdentity((int)ECylinder.RemoveZone_UpDownCyl1, ECylinder.RemoveZone_UpDownCyl1.ToString());
            RemoveZone_UpDownCyl1.CylinderType = ECylinderType.UpDownReverse;

            RemoveZone_UpDownCyl2 = _cylinderFactory
                .Create(_inputs.RemoveZoneZCyl2Down, _inputs.RemoveZoneZCyl2Up, _outputs.RemoveZoneZCyl2Down, null)
                .SetIdentity((int)ECylinder.RemoveZone_UpDownCyl2, ECylinder.RemoveZone_UpDownCyl2.ToString());
            RemoveZone_UpDownCyl2.CylinderType = ECylinderType.UpDownReverse;

            RemoveZone_FilmClampCyl1 = _cylinderFactory
                .Create(_inputs.RemoveZoneFilm1Clamp, _inputs.RemoveZoneFilm1Unclamp, _outputs.RemoveZoneFilm1Clamp, _outputs.RemoveZoneFilm1Unclamp)
                .SetIdentity((int)ECylinder.RemoveZone_FilmClampCyl1, ECylinder.RemoveZone_FilmClampCyl1.ToString());
            RemoveZone_FilmClampCyl1.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZone_FilmClampCyl2 = _cylinderFactory
                .Create(_inputs.RemoveZoneFilm2Clamp, _inputs.RemoveZoneFilm2Unclamp, _outputs.RemoveZoneFilm2Clamp, _outputs.RemoveZoneFilm2Unclamp)
                .SetIdentity((int)ECylinder.RemoveZone_FilmClampCyl2, ECylinder.RemoveZone_FilmClampCyl2.ToString());
            RemoveZone_FilmClampCyl2.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZone_FilmClampCyl3 = _cylinderFactory
                .Create(_inputs.RemoveZoneFilm3Clamp, _inputs.RemoveZoneFilm3Unclamp, _outputs.RemoveZoneFilm3Clamp, _outputs.RemoveZoneFilm3Unclamp)
                .SetIdentity((int)ECylinder.RemoveZone_FilmClampCyl3, ECylinder.RemoveZone_FilmClampCyl3.ToString());
            RemoveZone_FilmClampCyl3.CylinderType = ECylinderType.ClampUnclamp;

            RemoveZone_PusherCyl1 = _cylinderFactory
                .Create(_inputs.RemoveZonePusherCyl1Up, _inputs.RemoveZonePusherCyl1Down, _outputs.RemoveZonePusherCyl1Up, _outputs.RemoveZonePusherCyl1Down)
                .SetIdentity((int)ECylinder.RemoveZone_PusherCyl1, ECylinder.RemoveZone_PusherCyl1.ToString());
            RemoveZone_PusherCyl1.CylinderType = ECylinderType.UpDown;

            RemoveZone_PusherCyl2 = _cylinderFactory
                .Create(_inputs.RemoveZonePusherCyl2Up, _inputs.RemoveZonePusherCyl2Down, _outputs.RemoveZonePusherCyl2Up, _outputs.RemoveZonePusherCyl2Down)
                .SetIdentity((int)ECylinder.RemoveZone_PusherCyl2, ECylinder.RemoveZone_PusherCyl2.ToString());
            RemoveZone_PusherCyl2.CylinderType = ECylinderType.UpDown;

            RemoveZone_ClampCyl1 = _cylinderFactory
                .Create(_inputs.RemoveZoneClampCyl1Clamp, _inputs.RemoveZoneClampCyl1Unclamp, _outputs.RemoveZoneClampCyl1Clamp, _outputs.RemoveZoneClampCyl1Unclamp)
                .SetIdentity((int)ECylinder.RemoveZone_ClampCyl1, ECylinder.RemoveZone_ClampCyl1.ToString());
            RemoveZone_ClampCyl1.CylinderType = ECylinderType.ForwardBackward;

            RemoveZone_ClampCyl2 = _cylinderFactory
                .Create(_inputs.RemoveZoneClampCyl2Clamp, _inputs.RemoveZoneClampCyl2Unclamp, _outputs.RemoveZoneClampCyl2Clamp, _outputs.RemoveZoneClampCyl2Unclamp)
                .SetIdentity((int)ECylinder.RemoveZone_ClampCyl2, ECylinder.RemoveZone_ClampCyl2.ToString());
            RemoveZone_ClampCyl2.CylinderType = ECylinderType.ForwardBackward;

            RemoveZone_ClampCyl3 = _cylinderFactory
                .Create(_inputs.RemoveZoneClampCyl3Clamp, _inputs.RemoveZoneClampCyl3Unclamp, _outputs.RemoveZoneClampCyl3Clamp, _outputs.RemoveZoneClampCyl3Unclamp)
                .SetIdentity((int)ECylinder.RemoveZone_ClampCyl3, ECylinder.RemoveZone_ClampCyl3.ToString());
            RemoveZone_ClampCyl3.CylinderType = ECylinderType.ForwardBackward;

            RemoveZone_ClampCyl4 = _cylinderFactory
                .Create(_inputs.RemoveZoneClampCyl4Clamp, _inputs.RemoveZoneClampCyl4Unclamp, _outputs.RemoveZoneClampCyl4Clamp, _outputs.RemoveZoneClampCyl4Unclamp)
                .SetIdentity((int)ECylinder.RemoveZone_ClampCyl4, ECylinder.RemoveZone_ClampCyl4.ToString());
            RemoveZone_ClampCyl4.CylinderType = ECylinderType.ForwardBackward;

            // In Shuttle rotate
            TransferInShuttleL_RotateCyl = _cylinderFactory
                .Create(_inputs.TransferInShuttleL180Degree, _inputs.TransferInShuttleL0Degree, _outputs.TransferInShuttleL180Degree, _outputs.TransferInShuttleL0Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleL_RotateCyl, ECylinder.TransferInShuttleL_RotateCyl.ToString());
            TransferInShuttleL_RotateCyl.CylinderType = ECylinderType.FlipUnflip;

            TransferInShuttleR_RotateCyl = _cylinderFactory
                .Create(_inputs.TransferInShuttleR180Degree, _inputs.TransferInShuttleR0Degree, _outputs.TransferInShuttleR180Degree, _outputs.TransferInShuttleR0Degree)
                .SetIdentity((int)ECylinder.TransferInShuttleR_RotateCyl, ECylinder.TransferInShuttleR_RotateCyl.ToString());
            TransferInShuttleR_RotateCyl.CylinderType = ECylinderType.FlipUnflip;

            // Align Stage L/R
            AlignStageL_BrushCyl = _cylinderFactory
                .Create(_inputs.AlignStageLBrushCylUp, _inputs.AlignStageLBrushCylDown, _outputs.AlignStageLBrushCylUp, null)
                .SetIdentity((int)ECylinder.AlignStageL_BrushCyl, ECylinder.AlignStageL_BrushCyl.ToString());
            AlignStageL_BrushCyl.CylinderType = ECylinderType.UpDown;

            AlignStageL_AlignCyl1 = _cylinderFactory
                .Create(_inputs.AlignStageL1Align, _inputs.AlignStageL1Unalign, _outputs.AlignStageL1Align, null)
                .SetIdentity((int)ECylinder.AlignStageL_AlignCyl1, ECylinder.AlignStageL_AlignCyl1.ToString());
            AlignStageL_AlignCyl1.CylinderType = ECylinderType.UpDown;

            AlignStageL_AlignCyl2 = _cylinderFactory
                .Create(_inputs.AlignStageL2Align, _inputs.AlignStageL2Unalign, _outputs.AlignStageL2Align, null)
                .SetIdentity((int)ECylinder.AlignStageL_AlignCyl2, ECylinder.AlignStageL_AlignCyl2.ToString());
            AlignStageL_AlignCyl2.CylinderType = ECylinderType.UpDown;

            AlignStageL_AlignCyl3 = _cylinderFactory
                .Create(_inputs.AlignStageL3Align, _inputs.AlignStageL3Unalign, _outputs.AlignStageL3Align, null)
                .SetIdentity((int)ECylinder.AlignStageL_AlignCyl3, ECylinder.AlignStageL_AlignCyl3.ToString());
            AlignStageL_AlignCyl3.CylinderType = ECylinderType.UpDown;

            AlignStageR_BrushCyl = _cylinderFactory
                .Create(_inputs.AlignStageRBrushCylUp, _inputs.AlignStageRBrushCylDown, _outputs.AlignStageRBrushCylUp, null)
                .SetIdentity((int)ECylinder.AlignStageR_BrushCyl, ECylinder.AlignStageR_BrushCyl.ToString());
            AlignStageR_BrushCyl.CylinderType = ECylinderType.UpDown;

            AlignStageR_AlignCyl1 = _cylinderFactory
                .Create(_inputs.AlignStageR1Align, _inputs.AlignStageR1Unalign, _outputs.AlignStageR1Align, null)
                .SetIdentity((int)ECylinder.AlignStageR_AlignCyl1, ECylinder.AlignStageR_AlignCyl1.ToString());
            AlignStageR_AlignCyl1.CylinderType = ECylinderType.UpDown;

            AlignStageR_AlignCyl2 = _cylinderFactory
                .Create(_inputs.AlignStageR2Align, _inputs.AlignStageR2Unalign, _outputs.AlignStageR2Align, null)
                .SetIdentity((int)ECylinder.AlignStageR_AlignCyl2, ECylinder.AlignStageR_AlignCyl2.ToString());
            AlignStageR_AlignCyl2.CylinderType = ECylinderType.UpDown;

            AlignStageR_AlignCyl3 = _cylinderFactory
                .Create(_inputs.AlignStageR3Align, _inputs.AlignStageR3Unalign, _outputs.AlignStageR3Align, null)
                .SetIdentity((int)ECylinder.AlignStageR_AlignCyl3, ECylinder.AlignStageR_AlignCyl3.ToString());
            AlignStageR_AlignCyl3.CylinderType = ECylinderType.UpDown;

            // Glass Transfer (Transfer)
            GlassTransfer_UpDownCyl1 = _cylinderFactory
                .Create(_inputs.GlassTransferCyl1Down, _inputs.GlassTransferCyl1Up, _outputs.GlassTransferCyl1Down, _outputs.GlassTransferCyl1Up)
                .SetIdentity((int)ECylinder.GlassTransfer_UpDownCyl1, ECylinder.GlassTransfer_UpDownCyl1.ToString());
            GlassTransfer_UpDownCyl1.CylinderType = ECylinderType.UpDownReverse;

            GlassTransfer_UpDownCyl2 = _cylinderFactory
                .Create(_inputs.GlassTransferCyl2Down, _inputs.GlassTransferCyl2Up, _outputs.GlassTransferCyl2Down, _outputs.GlassTransferCyl2Up)
                .SetIdentity((int)ECylinder.GlassTransfer_UpDownCyl2, ECylinder.GlassTransfer_UpDownCyl2.ToString());
            GlassTransfer_UpDownCyl2.CylinderType = ECylinderType.UpDownReverse;

            GlassTransfer_UpDownCyl3 = _cylinderFactory
                .Create(_inputs.GlassTransferCyl3Down, _inputs.GlassTransferCyl3Up, _outputs.GlassTransferCyl3Down, _outputs.GlassTransferCyl3Up)
                .SetIdentity((int)ECylinder.GlassTransfer_UpDownCyl3, ECylinder.GlassTransfer_UpDownCyl3.ToString());
            GlassTransfer_UpDownCyl3.CylinderType = ECylinderType.UpDownReverse;

            // Wet Clean
            WetCleanR_PusherCyl = _cylinderFactory
                .Create(_inputs.WetCleanPusherRightDown, _inputs.WetCleanPusherRightUp, _outputs.WetCleanPusherRightDown, _outputs.WetCleanPusherRightUp)
                .SetIdentity((int)ECylinder.WetCleanR_PusherCyl, ECylinder.WetCleanR_PusherCyl.ToString());
            WetCleanR_PusherCyl.CylinderType = ECylinderType.UpDownReverse;

            WetCleanL_PusherCyl = _cylinderFactory
                .Create(_inputs.WetCleanPusherLeftDown, _inputs.WetCleanPusherLeftUp, _outputs.WetCleanPusherLeftDown, _outputs.WetCleanPusherLeftUp)
                .SetIdentity((int)ECylinder.WetCleanL_PusherCyl, ECylinder.WetCleanL_PusherCyl.ToString());
            WetCleanL_PusherCyl.CylinderType = ECylinderType.UpDownReverse;

            WetCleanR_BrushCyl = _cylinderFactory
                .Create(_inputs.WetCleanBrushRightDown, _inputs.WetCleanBrushRightUp, _outputs.WetCleanBrushRightDown, null)
                .SetIdentity((int)ECylinder.WetCleanR_BrushCyl, ECylinder.WetCleanR_BrushCyl.ToString());
            WetCleanR_BrushCyl.CylinderType = ECylinderType.UpDownReverse;

            WetCleanL_BrushCyl = _cylinderFactory
                .Create(_inputs.WetCleanBrushLeftDown, _inputs.WetCleanBrushLeftUp, _outputs.WetCleanBrushLeftDown, null)
                .SetIdentity((int)ECylinder.WetCleanL_BrushCyl, ECylinder.WetCleanL_BrushCyl.ToString());
            WetCleanL_BrushCyl.CylinderType = ECylinderType.UpDownReverse;

            WetCleanR_ClampCyl1 = _cylinderFactory
                .Create(_inputs.InShuttleRClamp1FW, _inputs.InShuttleRClamp1BW, _outputs.InShuttleRClampFw, _outputs.InShuttleRClampBw)
                .SetIdentity((int)ECylinder.WetCleanR_ClampCyl1, ECylinder.WetCleanR_ClampCyl1.ToString());
            WetCleanR_ClampCyl1.CylinderType = ECylinderType.ClampUnclamp;

            WetCleanR_ClampCyl2 = _cylinderFactory
                .Create(_inputs.InShuttleRClamp2FW, _inputs.InShuttleRClamp2BW, _outputs.InShuttleRClampFw, _outputs.InShuttleRClampBw)
                .SetIdentity((int)ECylinder.WetCleanR_ClampCyl2, ECylinder.WetCleanR_ClampCyl2.ToString());
            WetCleanR_ClampCyl2.CylinderType = ECylinderType.ClampUnclamp;

            WetCleanL_ClampCyl1 = _cylinderFactory
                .Create(_inputs.InShuttleLClamp1FW, _inputs.InShuttleLClamp1BW, _outputs.InShuttleLClampFw, _outputs.InShuttleLClampBw)
                .SetIdentity((int)ECylinder.WetCleanL_ClampCyl1, ECylinder.WetCleanL_ClampCyl1.ToString());
            WetCleanL_ClampCyl1.CylinderType = ECylinderType.ClampUnclamp;

            WetCleanL_ClampCyl2 = _cylinderFactory
                .Create(_inputs.InShuttleLClamp2FW, _inputs.InShuttleLClamp2BW, _outputs.InShuttleLClampFw, _outputs.InShuttleLClampBw)
                .SetIdentity((int)ECylinder.WetCleanL_ClampCyl2, ECylinder.WetCleanL_ClampCyl2.ToString());
            WetCleanL_ClampCyl2.CylinderType = ECylinderType.ClampUnclamp;

            // Transfer Rotate
            TransferRotationR_RotationCyl = _cylinderFactory
                .Create(_inputs.TrRotateRight180Degree, _inputs.TrRotateRight0Degree, _outputs.TrRotateRight180Degree, _outputs.TrRotateRight0Degree)
                .SetIdentity((int)ECylinder.TrRotationR_RotationCyl, ECylinder.TrRotationR_RotationCyl.ToString());
            TransferRotationR_RotationCyl.CylinderType = ECylinderType.FlipUnflip;

            TransferRotationR_BwFwCyl = _cylinderFactory
                .Create(_inputs.TrRotateRightFw, _inputs.TrRotateRightBw, _outputs.TrRotateRightFw, _outputs.TrRotateRightBw)
                .SetIdentity((int)ECylinder.TrRotationR_BwFwCyl, ECylinder.TrRotationR_BwFwCyl.ToString());
            TransferRotationR_BwFwCyl.CylinderType = ECylinderType.ForwardBackward;

            TransferRotationL_RotationCyl = _cylinderFactory
                .Create(_inputs.TrRotateLeft180Degree, _inputs.TrRotateLeft0Degree, _outputs.TrRotateLeft180Degree, _outputs.TrRotateLeft0Degree)
                .SetIdentity((int)ECylinder.TrRotationL_RotationCyl, ECylinder.TrRotationL_RotationCyl.ToString());
            TransferRotationL_RotationCyl.CylinderType = ECylinderType.FlipUnflip;

            TransferRotationL_BwFwCyl = _cylinderFactory
                .Create(_inputs.TrRotateLeftFw, _inputs.TrRotateLeftBw, _outputs.TrRotateLeftFw, _outputs.TrRotateLeftBw)
                .SetIdentity((int)ECylinder.TrRotationL_BwFwCyl, ECylinder.TrRotationL_BwFwCyl.ToString());
            TransferRotationL_BwFwCyl.CylinderType = ECylinderType.ForwardBackward;

            TransferRotationR_UpDownCyl = _cylinderFactory
                .Create(_inputs.TrRotateRightDown, _inputs.TrRotateRightUp, _outputs.TrRotateRightDown, _outputs.TrRotateRightUp)
                .SetIdentity((int)ECylinder.TrRotationR_UpDownCyl, ECylinder.TrRotationR_UpDownCyl.ToString());
            TransferRotationR_UpDownCyl.CylinderType = ECylinderType.UpDownReverse;

            TransferRotationL_UpDownCyl = _cylinderFactory
                .Create(_inputs.TrRotateLeftDown, _inputs.TrRotateLeftUp, _outputs.TrRotateLeftDown, _outputs.TrRotateLeftUp)
                .SetIdentity((int)ECylinder.TrRotationL_UpDownCyl, ECylinder.TrRotationL_UpDownCyl.ToString());
            TransferRotationL_UpDownCyl.CylinderType = ECylinderType.UpDownReverse;


            // AF Clean
            AFCleanR_PusherCyl = _cylinderFactory
                .Create(_inputs.AfCleanPusherRightDown, _inputs.AfCleanPusherRightUp, _outputs.AfCleanPusherRightDown, _outputs.AfCleanPusherRightUp)
                .SetIdentity((int)ECylinder.AFCleanR_PusherCyl, ECylinder.AFCleanR_PusherCyl.ToString());
            AFCleanR_PusherCyl.CylinderType = ECylinderType.UpDownReverse;

            AFCleanL_PusherCyl = _cylinderFactory
                .Create(_inputs.AfCleanPusherLeftDown, _inputs.AfCleanPusherLeftUp, _outputs.AfCleanPusherLeftDown, _outputs.AfCleanPusherLeftUp)
                .SetIdentity((int)ECylinder.AFCleanL_PusherCyl, ECylinder.AFCleanL_PusherCyl.ToString());
            AFCleanL_PusherCyl.CylinderType = ECylinderType.UpDownReverse;

            AFCleanR_BrushCyl = _cylinderFactory
                .Create(_inputs.AfCleanBrushRightDown, _inputs.AfCleanBrushRightUp, _outputs.AfCleanBrushRightDown, null)
                .SetIdentity((int)ECylinder.AFCleanR_BrushCyl, ECylinder.AFCleanR_BrushCyl.ToString());
            AFCleanR_BrushCyl.CylinderType = ECylinderType.UpDownReverse;

            AFCleanL_BrushCyl = _cylinderFactory
                .Create(_inputs.AfCleanBrushLeftDown, _inputs.AfCleanBrushLeftUp, _outputs.AfCleanBrushLeftDown, null)
                .SetIdentity((int)ECylinder.AFCleanL_BrushCyl, ECylinder.AFCleanL_BrushCyl.ToString());
            AFCleanL_BrushCyl.CylinderType = ECylinderType.UpDownReverse;

            // Robot 2 Unload
            UnloadRobot_UpDownCyl1 = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl1Down, _inputs.UnloadRobotCyl1Up, _outputs.UnloadRobotCyl1Down, null)
                .SetIdentity((int)ECylinder.UnloadRobot_UpDownCyl1, ECylinder.UnloadRobot_UpDownCyl1.ToString());
            UnloadRobot_UpDownCyl1.CylinderType = ECylinderType.UpDownReverse;

            UnloadRobot_UpDownCyl2 = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl2Down, _inputs.UnloadRobotCyl2Up, _outputs.UnloadRobotCyl2Down, null)
                .SetIdentity((int)ECylinder.UnloadRobot_UpDownCyl2, ECylinder.UnloadRobot_UpDownCyl2.ToString());
            UnloadRobot_UpDownCyl2.CylinderType = ECylinderType.UpDownReverse;

            UnloadRobot_UpDownCyl3 = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl3Down, _inputs.UnloadRobotCyl3Up, _outputs.UnloadRobotCyl3Down, null)
                .SetIdentity((int)ECylinder.UnloadRobot_UpDownCyl3, ECylinder.UnloadRobot_UpDownCyl3.ToString());
            UnloadRobot_UpDownCyl3.CylinderType = ECylinderType.UpDownReverse;

            UnloadRobot_UpDownCyl4 = _cylinderFactory
                .Create(_inputs.UnloadRobotCyl4Down, _inputs.UnloadRobotCyl4Up, _outputs.UnloadRobotCyl4Down, null)
                .SetIdentity((int)ECylinder.UnloadRobot_UpDownCyl4, ECylinder.UnloadRobot_UpDownCyl4.ToString());
            UnloadRobot_UpDownCyl4.CylinderType = ECylinderType.UpDownReverse;

            // Unload Stage
            UnloadAlign_UpDownCyl1 = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl1Up, _inputs.UnloadAlignCyl1Down, _outputs.UnloadAlignCyl1Up, null)
                .SetIdentity((int)ECylinder.UnloadAlign_UpDownCyl1, ECylinder.UnloadAlign_UpDownCyl1.ToString());
            UnloadAlign_UpDownCyl1.CylinderType = ECylinderType.UpDown;

            UnloadAlign_UpDownCyl2 = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl2Up, _inputs.UnloadAlignCyl2Down, _outputs.UnloadAlignCyl2Up, null)
                .SetIdentity((int)ECylinder.UnloadAlign_UpDownCyl2, ECylinder.UnloadAlign_UpDownCyl2.ToString());
            UnloadAlign_UpDownCyl2.CylinderType = ECylinderType.UpDown;

            UnloadAlign_UpDownCyl3 = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl3Up, _inputs.UnloadAlignCyl3Down, _outputs.UnloadAlignCyl3Up, null)
                .SetIdentity((int)ECylinder.UnloadAlign_UpDownCyl3, ECylinder.UnloadAlign_UpDownCyl3.ToString());
            UnloadAlign_UpDownCyl3.CylinderType = ECylinderType.UpDown;

            UnloadAlign_UpDownCyl4 = _cylinderFactory
                .Create(_inputs.UnloadAlignCyl4Up, _inputs.UnloadAlignCyl4Down, _outputs.UnloadAlignCyl4Up, null)
                .SetIdentity((int)ECylinder.UnloadAlign_UpDownCyl4, ECylinder.UnloadAlign_UpDownCyl4.ToString());
            UnloadAlign_UpDownCyl4.CylinderType = ECylinderType.UpDown;

        }

        private readonly ICylinderFactory _cylinderFactory;
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
    }
}
