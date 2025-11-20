using EQX.Core.InOut;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Outputs
    {
        private readonly IDOutputDevice _dOutputDevice;

        public Outputs([FromKeyedServices("OutputDevice#1")] IDOutputDevice dOutputDevice)
        {
            _dOutputDevice = dOutputDevice;

            Initialize();
        }

        public bool Initialize()
        {
            return _dOutputDevice.Initialize();
        }

        public bool Connect()
        {
            return _dOutputDevice.Connect();
        }

        public bool Disconnect()
        {
            return _dOutputDevice.Disconnect();
        }

        public List<IDOutput> All => _dOutputDevice.Outputs;

        public IDOutput InCst_StopperUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CV_STOPPER_UP);
        public IDOutput InCst_StopperDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CV_STOPPER_DOWN);
        public IDOutput OutCst_StopperUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CV_STOPPER_UP);
        public IDOutput OutCst_StopperDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CV_STOPPER_DOWN);
        public IDOutput InWorkCst_AlignCyl1Bw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_WORK_CV_ALIGN_CYL_1_BW);
        public IDOutput InWorkCst_AlignCyl1Fw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_WORK_CV_ALIGN_CYL_1_FW);
        public IDOutput InWorkCst_AlignCyl2Bw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_WORK_CV_ALIGN_CYL_2_BW);
        public IDOutput InWorkCst_AlignCyl2Fw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_WORK_CV_ALIGN_CYL_2_FW);
        public IDOutput InWorkCst_TiltCylUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_WORK_CV_TILT_CYL_UP);
        public IDOutput InWorkCst_TiltCylDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_WORK_CV_TILT_CYL_DOWN);
        public IDOutput OutWorkCst_AlignCyl1Bw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_WORK_CV_ALIGN_CYL_1_BW);
        public IDOutput OutWorkCst_AlignCyl1Fw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_WORK_CV_ALIGN_CYL_1_FW);
        public IDOutput OutWorkCst_AlignCyl2Bw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_WORK_CV_ALIGN_CYL_2_BW);
        public IDOutput OutWorkCst_AlignCyl2Fw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_WORK_CV_ALIGN_CYL_2_FW);
        public IDOutput OutWorkCst_TiltCylUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_WORK_CV_TILT_CYL_UP);
        public IDOutput OutWorkCst_TiltCylDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_WORK_CV_TILT_CYL_DOWN);
        public IDOutput BufferCvStopper1Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.BUFFER_CV_STOPPER_1_UP);
        public IDOutput BufferCvStopper1Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.BUFFER_CV_STOPPER_1_DOWN);
        public IDOutput BufferCvStopper2Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.BUFFER_CV_STOPPER_2_UP);
        public IDOutput BufferCvStopper2Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.BUFFER_CV_STOPPER_2_DOWN);
        public IDOutput TowerLampRed => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TOWER_LAMP_RED);
        public IDOutput TowerLampYellow => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TOWER_LAMP_YELLOW);
        public IDOutput TowerLampGreen => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TOWER_LAMP_GREEN);
        public IDOutput TowerBuzzer => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TOWER_BUZZER);
        public IDOutput InCvSupportUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CV_SUPPORT_UP);
        public IDOutput InCvSupportDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CV_SUPPORT_DOWN);
        public IDOutput InCvSupportBufferUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CV_SUPPORT_BUFFER_UP);
        public IDOutput InCvSupportBufferDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CV_SUPPORT_BUFFER_DOWN);
        public IDOutput OutCvSupportBufferUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CV_SUPPORT_BUFFER_UP);
        public IDOutput OutCvSupportBufferDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CV_SUPPORT_BUFFER_DOWN);
        public IDOutput OutCvSupportUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CV_SUPPORT_UP);
        public IDOutput OutCvSupportDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CV_SUPPORT_DOWN);
        public IDOutput InMutingButtonLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_MUTING_BUTTON_LAMP);
        public IDOutput OutMutingButtonLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_MUTING_BUTTON_LAMP);
        public IDOutput InCstLightCurtainReset => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CST_LIGHT_CURTAIN_RESET);
        public IDOutput OutCstLightCurtainReset => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CST_LIGHT_CURTAIN_RESET);
        public IDOutput InCstLightCurtainMuting => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CST_LIGHT_CURTAIN_MUTING);
        public IDOutput InCstLightCurtainInterlock => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_CST_LIGHT_CURTAIN_INTERLOCK);
        public IDOutput OutCstLightCurtainMuting => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CST_LIGHT_CURTAIN_MUTING);
        public IDOutput OutCstLightCurtainInterlock => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_CST_LIGHT_CURTAIN_INTERLOCK);
        public IDOutput LoadRobMoveEnable => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.LOAD_ROB_MOVE_ENABLE);
        public IDOutput LoadRobDrivesOn => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.LOAD_ROB_DRIVES_ON);
        public IDOutput LoadRobDrivesOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.LOAD_ROB_DRIVES_OFF);
        public IDOutput LoadRobConfMess => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.LOAD_ROB_CONF_MESS);
        public IDOutput LoadRobExtStart => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.LOAD_ROB_EXT_START);
        public IDOutput RobotFixtureAlignBw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ROBOT_FIXTURE_ALIGN_BW);
        public IDOutput RobotFixtureAlignFw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ROBOT_FIXTURE_ALIGN_FW);
        public IDOutput RobotFixtureClamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ROBOT_FIXTURE_CLAMP);
        public IDOutput RobotFixtureUnclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ROBOT_FIXTURE_UNCLAMP);
        public IDOutput VinylCleanRollerBw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.VINYL_CLEAN_ROLLER_BW);
        public IDOutput VinylCleanRollerFw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.VINYL_CLEAN_ROLLER_FW);
        public IDOutput VinylCleanFixtureClamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.VINYL_CLEAN_FIXTURE_CLAMP);
        public IDOutput VinylCleanFixtureUnclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.VINYL_CLEAN_FIXTURE_UNCLAMP);
        public IDOutput VinylCleanPusherRollerUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.VINYL_CLEAN_PUSHER_UP);
        public IDOutput VinylCleanMotorOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.VINYL_CLEAN_MOTOR_ON_OFF);
        public IDOutput TransferFixtureUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_FIXTURE_UP);
        public IDOutput TransferFixtureDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_FIXTURE_DOWN);
        public IDOutput TransferFixture1Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_FIXTURE_1_CLAMP);
        public IDOutput TransferFixture1Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_FIXTURE_1_UNCLAMP);
        public IDOutput MachineAutoRun => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.MACHINE_AUTO_RUN);
        public IDOutput TransferFixture2Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_FIXTURE_2_CLAMP);
        public IDOutput TransferFixture2Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_FIXTURE_2_UNCLAMP);
        public IDOutput TransferInShuttleLBlowOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_L_BLOW_ON_OFF);
        public IDOutput TransferInShuttleRBlowOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_R_BLOW_ON_OFF);
        public IDOutput DetachCyl1Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CYL_1_UP);
        public IDOutput DetachCyl1Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CYL_1_DOWN);
        public IDOutput DetachCyl2Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CYL_2_UP);
        public IDOutput DetachCyl2Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CYL_2_DOWN);
        public IDOutput DetachClampCyl1Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_1_CLAMP);
        public IDOutput DetachClampCyl1Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_1_UNCLAMP);
        public IDOutput DetachClampCyl2Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_2_CLAMP);
        public IDOutput DetachClampCyl2Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_2_UNCLAMP);
        public IDOutput DetachClampCyl3Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_3_CLAMP);
        public IDOutput DetachClampCyl3Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_3_UNCLAMP);
        public IDOutput DetachClampCyl4Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_4_CLAMP);
        public IDOutput DetachClampCyl4Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_CLAMP_CYL_4_UNCLAMP);
        public IDOutput RemoveZoneTrCylBw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_TR_CYL_BW);
        public IDOutput RemoveZoneTrCylFw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_TR_CYL_FW);
        public IDOutput GlassTransferBlow1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_BLOW_1_ON_OFF);
        public IDOutput GlassTransferBlow2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_BLOW_2_ON_OFF);
        public IDOutput GlassTransferBlow3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_BLOW_3_ON_OFF);
        public IDOutput RemoveZoneZCyl1Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_Z_CYL_1_DOWN);
        public IDOutput RemoveZoneZCyl2Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_Z_CYL_2_DOWN);
        public IDOutput RemoveZoneFilm1Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_FILM_1_CLAMP);
        public IDOutput RemoveZoneFilm1Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_FILM_1_UNCLAMP);
        public IDOutput RemoveZoneFilm2Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_FILM_2_CLAMP);
        public IDOutput RemoveZoneFilm2Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_FILM_2_UNCLAMP);
        public IDOutput RemoveZoneFilm3Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_FILM_3_CLAMP);
        public IDOutput RemoveZoneFilm3Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_FILM_3_UNCLAMP);
        public IDOutput RemoveZonePusherCyl1Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_PUSHER_CYL_1_UP);
        public IDOutput RemoveZonePusherCyl1Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_PUSHER_CYL_1_DOWN);
        public IDOutput RemoveZonePusherCyl2Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_PUSHER_CYL_2_UP);
        public IDOutput RemoveZonePusherCyl2Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_PUSHER_CYL_2_DOWN);
        public IDOutput RemoveZoneClampCyl1Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_1_CLAMP);
        public IDOutput RemoveZoneClampCyl1Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_1_UNCLAMP);
        public IDOutput RemoveZoneClampCyl2Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_2_CLAMP);
        public IDOutput RemoveZoneClampCyl2Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_2_UNCLAMP);
        public IDOutput RemoveZoneClampCyl3Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_3_CLAMP);
        public IDOutput RemoveZoneClampCyl3Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_3_UNCLAMP);
        public IDOutput RemoveZoneClampCyl4Clamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_4_CLAMP);
        public IDOutput RemoveZoneClampCyl4Unclamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.REMOVE_ZONE_CYL_4_UNCLAMP);
        public IDOutput DetachGlassShtVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_GLASS_SHT_VAC_1_ON_OFF);
        public IDOutput DetachGlassShtVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_GLASS_SHT_VAC_2_ON_OFF);
        public IDOutput DetachGlassShtVac3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DETACH_GLASS_SHT_VAC_3_ON_OFF);
        public IDOutput TransferInShuttleLVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_L_VAC_ON_OFF);
        public IDOutput TransferInShuttleRVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_R_VAC_ON_OFF);
        public IDOutput TransferInShuttleL0Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_L_0_DEGREE);
        public IDOutput TransferInShuttleL180Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_L_180_DEGREE);
        public IDOutput TransferInShuttleR0Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_R_0_DEGREE);
        public IDOutput TransferInShuttleR180Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TRANSFER_IN_SHUTTLE_R_180_DEGREE);
        public IDOutput FixtureAlignCyl1Bw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.FIXTURE_ALIGN_CYL_1_BW);
        public IDOutput FixtureAlignCyl1Fw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.FIXTURE_ALIGN_CYL_1_FW);
        public IDOutput FixtureAlignCyl2Bw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.FIXTURE_ALIGN_CYL_2_BW);
        public IDOutput FixtureAlignCyl2Fw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.FIXTURE_ALIGN_CYL_2_FW);
        public IDOutput AlignStageLVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_VAC_1_ON_OFF);
        public IDOutput AlignStageLVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_VAC_2_ON_OFF);
        public IDOutput AlignStageLVac3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_VAC_3_ON_OFF);
        public IDOutput AlignStageLBlow1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_BLOW_1_ON_OFF);
        public IDOutput AlignStageLBlow2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_BLOW_2_ON_OFF);
        public IDOutput AlignStageLBlow3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_BLOW_3_ON_OFF);
        public IDOutput AlignStageLBrushCylUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_BRUSH_CYL_UP);
        public IDOutput AlignStageL1Align => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_1_ALIGN);
        public IDOutput AlignStageL2Align => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_2_ALIGN);
        public IDOutput AlignStageL3Align => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_L_3_ALIGN);
        public IDOutput AlignStageRVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_VAC_1_ON_OFF);
        public IDOutput AlignStageRVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_VAC_2_ON_OFF);
        public IDOutput AlignStageRVac3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_VAC_3_ON_OFF);
        public IDOutput AlignStageRBlow1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_BLOW_1_ON_OFF);
        public IDOutput AlignStageRBlow2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_BLOW_2_ON_OFF);
        public IDOutput AlignStageRBlow3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_BLOW_3_ON_OFF);
        public IDOutput AlignStageRBrushCylUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_BRUSH_CYL_UP);
        public IDOutput AlignStageR1Align => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_1_ALIGN);
        public IDOutput AlignStageR2Align => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_2_ALIGN);
        public IDOutput AlignStageR3Align => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.ALIGN_STAGE_R_3_ALIGN);
        public IDOutput GlassTransferVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_VAC_1_ON_OFF);
        public IDOutput GlassTransferVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_VAC_2_ON_OFF);
        public IDOutput GlassTransferVac3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_VAC_3_ON_OFF);
        public IDOutput GlassTransferCyl1Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_CYL_1_UP);
        public IDOutput GlassTransferCyl1Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_CYL_1_DOWN);
        public IDOutput GlassTransferCyl2Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_CYL_2_UP);
        public IDOutput GlassTransferCyl2Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_CYL_2_DOWN);
        public IDOutput GlassTransferCyl3Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_CYL_3_UP);
        public IDOutput GlassTransferCyl3Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.GLASS_TRANSFER_CYL_3_DOWN);
        public IDOutput InShuttleRVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_SHUTTLE_R_VAC_ON_OFF);
        public IDOutput InShuttleLVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_SHUTTLE_L_VAC_ON_OFF);
        public IDOutput InShuttleRClampFw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_SHUTTLE_R_CLAMP_FW);
        public IDOutput InShuttleRClampBw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_SHUTTLE_R_CLAMP_BW);
        public IDOutput InShuttleLClampFw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_SHUTTLE_L_CLAMP_FW);
        public IDOutput InShuttleLClampBw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.IN_SHUTTLE_L_CLAMP_BW);
        public IDOutput WetCleanPusherRightUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.WET_CLEAN_PUSHER_RIGHT_UP);
        public IDOutput WetCleanPusherRightDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.WET_CLEAN_PUSHER_RIGHT_DOWN);
        public IDOutput WetCleanPusherLeftUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.WET_CLEAN_PUSHER_LEFT_UP);
        public IDOutput WetCleanPusherLeftDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.WET_CLEAN_PUSHER_LEFT_DOWN);
        public IDOutput WetCleanBrushRightDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.WET_CLEAN_BRUSH_RIGHT_DOWN);
        public IDOutput WetCleanBrushLeftDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.WET_CLEAN_BRUSH_LEFT_DOWN);
        public IDOutput TrRotateRightVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_VAC_1_ON_OFF);
        public IDOutput TrRotateRight0Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_0_DEGREE);
        public IDOutput TrRotateRight180Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_180_DEGREE);
        public IDOutput TrRotateRightBw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_BW);
        public IDOutput TrRotateRightFw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_FW);
        public IDOutput TrRotateRightVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_VAC_2_ON_OFF);
        public IDOutput TrRotateLeftVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_VAC_1_ON_OFF);
        public IDOutput TrRotateLeft0Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_0_DEGREE);
        public IDOutput TrRotateLeft180Degree => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_180_DEGREE);
        public IDOutput TrRotateLeftBw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_BW);
        public IDOutput TrRotateLeftFw => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_FW);
        public IDOutput TrRotateLeftVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_VAC_2_ON_OFF);
        public IDOutput AfCleanPusherRightUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.AF_CLEAN_PUSHER_RIGHT_UP);
        public IDOutput AfCleanPusherRightDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.AF_CLEAN_PUSHER_RIGHT_DOWN);
        public IDOutput AfCleanPusherLeftUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.AF_CLEAN_PUSHER_LEFT_UP);
        public IDOutput AfCleanPusherLeftDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.AF_CLEAN_PUSHER_LEFT_DOWN);
        public IDOutput TransferRotationRightBlow1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_BLOW_1_ON_OFF);
        public IDOutput TransferRotationRightBlow2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_BLOW_2_ON_OFF);
        public IDOutput TransferRotationLeftBlow1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_BLOW_1_ON_OFF);
        public IDOutput TransferRotationLeftBlow2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_BLOW_2_ON_OFF);
        public IDOutput TransferRotationRightRotBlowOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_ROT_BLOW_ON_OFF);
        public IDOutput TransferRotationLeftRotBlowOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_ROT_BLOW_ON_OFF);
        public IDOutput AfCleanBrushRightDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.AF_CLEAN_BRUSH_RIGHT_DOWN);
        public IDOutput AfCleanBrushLeftDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.AF_CLEAN_BRUSH_LEFT_DOWN);
        public IDOutput OutShuttleRVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_SHUTTLE_R_VAC_ON_OFF);
        public IDOutput OutShuttleLVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OUT_SHUTTLE_L_VAC_ON_OFF);
        public IDOutput UnloadTransferLBlowOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_TRANSFER_L_BLOW_ON_OFF);
        public IDOutput UnloadTransferRBlowOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_TRANSFER_R_BLOW_ON_OFF);
        public IDOutput UnloadTransferLVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_TRANSFER_L_VAC_ON_OFF);
        public IDOutput UnloadTransferRVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_TRANSFER_R_VAC_ON_OFF);
        public IDOutput OpRButtonStopLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OP_R_BUTTON_STOP_LAMP);
        public IDOutput OpRButtonStartLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OP_R_BUTTON_START_LAMP);
        public IDOutput OpRButtonResetLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OP_R_BUTTON_RESET_LAMP);
        public IDOutput OpLButtonStopLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OP_L_BUTTON_STOP_LAMP);
        public IDOutput OpLButtonStartLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OP_L_BUTTON_START_LAMP);
        public IDOutput OpLButtonResetLamp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.OP_L_BUTTON_RESET_LAMP);
        public IDOutput DoorOpen => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.DOOR_OPEN);
        public IDOutput UnloadGlassAlignVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_VAC_1_ON_OFF);
        public IDOutput UnloadGlassAlignVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_VAC_2_ON_OFF);
        public IDOutput UnloadGlassAlignVac3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_VAC_3_ON_OFF);
        public IDOutput UnloadGlassAlignVac4OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_VAC_4_ON_OFF);
        public IDOutput UnloadGlassAlignBlow1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_BLOW_1_ON_OFF);
        public IDOutput UnloadGlassAlignBlow2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_BLOW_2_ON_OFF);
        public IDOutput UnloadGlassAlignBlow3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_BLOW_3_ON_OFF);
        public IDOutput UnloadGlassAlignBlow4OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_GLASS_ALIGN_BLOW_4_ON_OFF);
        public IDOutput UnloadAlignCyl1Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ALIGN_CYL_1_UP);
        public IDOutput UnloadAlignCyl2Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ALIGN_CYL_2_UP);
        public IDOutput UnloadAlignCyl3Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ALIGN_CYL_3_UP);
        public IDOutput UnloadAlignCyl4Up => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ALIGN_CYL_4_UP);
        public IDOutput UnloadRobotBlow1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_BLOW_1_ON_OFF);
        public IDOutput UnloadRobotBlow2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_BLOW_2_ON_OFF);
        public IDOutput UnloadRobotBlow3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_BLOW_3_ON_OFF);
        public IDOutput UnloadRobotBlow4OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_BLOW_4_ON_OFF);
        public IDOutput UnloadRobotVac1OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_VAC_1_ON_OFF);
        public IDOutput UnloadRobotVac2OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_VAC_2_ON_OFF);
        public IDOutput UnloadRobotVac3OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_VAC_3_ON_OFF);
        public IDOutput UnloadRobotVac4OnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_VAC_4_ON_OFF);
        public IDOutput UnloadRobotCyl1Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_CYL_1_DOWN);
        public IDOutput UnloadRobotCyl2Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_CYL_2_DOWN);
        public IDOutput UnloadRobotCyl3Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_CYL_3_DOWN);
        public IDOutput UnloadRobotCyl4Down => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROBOT_CYL_4_DOWN);
        public IDOutput UnloadRobMoveEnable => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROB_MOVE_ENABLE);
        public IDOutput UnloadRobDrivesOn => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROB_DRIVES_ON);
        public IDOutput UnloadRobDrivesOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROB_DRIVES_OFF);
        public IDOutput UnloadRobConfMess => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROB_CONF_MESS);
        public IDOutput UnloadRobExtStart => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.UNLOAD_ROB_EXT_START);
        public IDOutput TrRotateRightUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_UP);
        public IDOutput TrRotateRightDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_DOWN);
        public IDOutput TrRotateLeftUp => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_UP);
        public IDOutput TrRotateLeftDown => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_DOWN);
        public IDOutput TrRotateRightRotVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_RIGHT_ROT_VAC_ON_OFF);
        public IDOutput TrRotateLeftRotVacOnOff => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.TR_ROTATE_LEFT_ROT_VAC_ON_OFF);

        //Plasma
        public IDOutput PlasmaRun => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.PLASMA_RUN);
        public IDOutput PlasmaRemoteEnable => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.PLASMA_REMOTE_ENABLE);
        public IDOutput PlasmaN2SolOpen => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.PLASMA_N2_SOL_OPEN);
        public IDOutput PlasmaCDASolOpen => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.PLASMA_CDA_SOL_OPEN);
        public IDOutput PlasmaPowerReset => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.PLASMA_POWER_RESET);
        public IDOutput PlasmaIdleMode => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.PLASMA_IDLE_MODE);
        public IDOutput PlasmaMotorMCOn => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.PLASMA_MOTOR_MC_ON);

        public IDOutput MachineEmergencyStop => _dOutputDevice.Outputs.First(i => i.Id == (int)EOutput.MACHINE_EMERGENCY_STOP);

        public void Lamp_Run()
        {
            Lamp_Clear();

            MachineAutoRun.Value = true;

            TowerLampGreen.Value = true;
            OpLButtonStartLamp.Value = true;
            OpRButtonStartLamp.Value = true;

            TowerBuzzer.Value = false;
        }

        public void Lamp_Stop()
        {
            Lamp_Clear();

            MachineAutoRun.Value = false;

            TowerLampYellow.Value = true;
            OpLButtonStopLamp.Value = true;
            OpRButtonStopLamp.Value = true;
        }

        public void Lamp_Alarm(bool isUseBuzzer)
        {
            Lamp_Clear();

            MachineAutoRun.Value = false;

            TowerLampRed.Value = true;

            if(isUseBuzzer)
            {
                TowerBuzzer.Value = true;
            }
        }
        private void Lamp_Clear()
        {
            TowerLampGreen.Value = false;
            TowerLampRed.Value = false;
            TowerLampYellow.Value = false;

            OpLButtonStartLamp.Value = false;
            OpLButtonStopLamp.Value = false;
            OpLButtonResetLamp.Value = false;

            OpRButtonStartLamp.Value = false;
            OpRButtonStopLamp.Value = false;
            OpRButtonResetLamp.Value = false;
        }

    }
}
