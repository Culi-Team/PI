using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Inputs : ObservableObject
    {
        private readonly IDInputDevice _dInputDevice;

        public Inputs([FromKeyedServices("InputDevice#1")]IDInputDevice dInputDevice)
        {
            _dInputDevice = dInputDevice;

            Initialize();

            System.Timers.Timer inputUpdateTimer = new System.Timers.Timer(10);
            inputUpdateTimer.Elapsed += InputUpdateTimer_Elapsed;
            inputUpdateTimer.AutoReset = true;
            inputUpdateTimer.Enabled = true;

        }

        public bool Initialize()
        {
            return _dInputDevice.Initialize();
        }

        public bool Connect()
        {
            return _dInputDevice.Connect();
        }

        public bool Disconnect()
        {
            return _dInputDevice.Disconnect();
        }

        private void InputUpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var input in _dInputDevice.Inputs)
            {
                input.RaiseValueUpdated();
            }
        }

        public List<IDInput> All => _dInputDevice.Inputs;

        public IDInput InCstStopperUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_STOPPER_UP);
        public IDInput InCstStopperDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_STOPPER_DOWN);
        public IDInput OutCstStopperUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_STOPPER_UP);
        public IDInput OutCstStopperDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_STOPPER_DOWN);
        public IDInput InCstFixCyl1Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_FIX_CYL_1_BW);
        public IDInput InCstFixCyl1Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_FIX_CYL_1_FW);
        public IDInput InCstFixCyl2Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_FIX_CYL_2_BW);
        public IDInput InCstFixCyl2Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_FIX_CYL_2_FW);
        public IDInput InCstTiltCylUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_TILT_CYL_UP);
        public IDInput InCstTiltCylDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_TILT_CYL_DOWN);
        public IDInput OutCstFixCyl1Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_FIX_CYL_1_BW);
        public IDInput OutCstFixCyl1Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_FIX_CYL_1_FW);
        public IDInput OutCstFixCyl2Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_FIX_CYL_2_BW);
        public IDInput OutCstFixCyl2Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_FIX_CYL_2_FW);
        public IDInput OutCstTiltCylUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_TILT_CYL_UP);
        public IDInput OutCstTiltCylDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_TILT_CYL_DOWN);
        public IDInput BufferCvStopper1Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.BUFFER_CV_STOPPER_1_UP);
        public IDInput BufferCvStopper1Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.BUFFER_CV_STOPPER_1_DOWN);
        public IDInput BufferCvStopper2Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.BUFFER_CV_STOPPER_2_UP);
        public IDInput BufferCvStopper2Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.BUFFER_CV_STOPPER_2_DOWN);
        public IDInput BufferCstDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.BUFFER_CST_DETECT_1);
        public IDInput BufferCstDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.BUFFER_CST_DETECT_2);
        public IDInput EmoLoadR => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.EMO_LOAD_R);
        public IDInput EmoLoadL => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.EMO_LOAD_L);
        public IDInput InCvSupportUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CV_SUPPORT_UP);
        public IDInput InCvSupportDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CV_SUPPORT_DOWN);
        public IDInput InCvSupportBufferUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CV_SUPPORT_BUFFER_UP);
        public IDInput InCvSupportBufferDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CV_SUPPORT_BUFFER_DOWN);
        public IDInput OutCvSupportBufferUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CV_SUPPORT_BUFFER_UP);
        public IDInput OutCvSupportBufferDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CV_SUPPORT_BUFFER_DOWN);
        public IDInput OutCvSupportUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CV_SUPPORT_UP);
        public IDInput OutCvSupportDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CV_SUPPORT_DOWN);
        public IDInput InCstDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_DETECT_1);
        public IDInput InCstDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_DETECT_2);
        public IDInput OutCstDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_DETECT_1);
        public IDInput OutCstDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_DETECT_2);
        public IDInput InCstWorkDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_WORK_DETECT_1);
        public IDInput InCstWorkDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_WORK_DETECT_2);
        public IDInput InCstWorkDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_WORK_DETECT_3);
        public IDInput InCstWorkDetect4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_WORK_DETECT_4);
        public IDInput OutCstWorkDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_WORK_DETECT_1);
        public IDInput OutCstWorkDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_WORK_DETECT_2);
        public IDInput OutCstWorkDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_WORK_DETECT_3);
        public IDInput InButton1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_BUTTON_1);
        public IDInput InButton2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_BUTTON_2);
        public IDInput OutButton1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_BUTTON_1);
        public IDInput OutButton2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_BUTTON_2);
        public IDInput InCstLightCurtainAlarmDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_CST_LIGHT_CURTAIN_ALARM_DETECT);
        public IDInput OutCstLightCurtainAlarmDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_CST_LIGHT_CURTAIN_ALARM_DETECT);
        public IDInput LoadRobStopmess => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_STOPMESS);
        public IDInput LoadRobPeriRdy => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_PERI_RDY);
        public IDInput LoadRobAlarmStop => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_ALARM_STOP);
        public IDInput LoadRobUserSaf => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_USER_SAF);
        public IDInput LoadRobIoActconf => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_IO_ACTCONF);
        public IDInput LoadRobOnPath => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_ON_PATH);
        public IDInput LoadRobProAct => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_PRO_ACT);
        public IDInput LoadRobInHome => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.LOAD_ROB_IN_HOME);
        public IDInput RobotFixtureAlign1Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_ALIGN_1_BW);
        public IDInput RobotFixtureAlign1Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_ALIGN_1_FW);
        public IDInput RobotFixtureAlign2Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_ALIGN_2_BW);
        public IDInput RobotFixtureAlign2Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_ALIGN_2_FW);
        public IDInput RobotFixture1Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_1_CLAMP);
        public IDInput RobotFixture1Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_1_UNCLAMP);
        public IDInput RobotFixture2Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_2_CLAMP);
        public IDInput RobotFixture2Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ROBOT_FIXTURE_2_UNCLAMP);
        public IDInput PowerMCOn1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.POWER_MC_ON_1);
        public IDInput PowerMCOn2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.POWER_MC_ON_2);
        public IDInput VinylCleanFixtureDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_FIXTURE_DETECT);
        public IDInput VinylCleanFullDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_FULL_DETECT);
        public IDInput VinylCleanRunoffDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_RUNOFF_DETECT);
        public IDInput VinylCleanRollerBw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_ROLLER_BW);
        public IDInput VinylCleanRollerFw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_ROLLER_FW);
        public IDInput VinylCleanFixture1Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_FIXTURE_1_CLAMP);
        public IDInput VinylCleanFixture1Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_FIXTURE_1_UNCLAMP);
        public IDInput VinylCleanFixture2Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_FIXTURE_2_CLAMP);
        public IDInput VinylCleanFixture2Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_FIXTURE_2_UNCLAMP);
        public IDInput VinylCleanPusherRollerUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_PUSHER_ROLLER_UP);
        public IDInput VinylCleanPusherRollerDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.VINYL_CLEAN_PUSHER_ROLLER_DOWN);
        public IDInput DoorLock1R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_1_R);
        public IDInput DoorLock1L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_1_L);
        public IDInput DoorLatch1R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_1_R);
        public IDInput DoorLatch1L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_1_L);
        public IDInput DoorLock2R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_2_R);
        public IDInput DoorLock2L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_2_L);
        public IDInput DoorLatch2R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_2_R);
        public IDInput DoorLatch2L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_2_L);
        public IDInput DoorLock3R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_3_R);
        public IDInput DoorLock3L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_3_L);
        public IDInput DoorLatch3R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_3_R);
        public IDInput DoorLatch3L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_3_L);
        public IDInput DoorLock4R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_4_R);
        public IDInput DoorLock4L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_4_L);
        public IDInput DoorLatch4R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_4_R);
        public IDInput DoorLatch4L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_4_L);
        public IDInput DoorLock5R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_5_R);
        public IDInput DoorLock5L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_5_L);
        public IDInput DoorLatch5R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_5_R);
        public IDInput DoorLatch5L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_5_L);
        public IDInput DoorLock6R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_6_R);
        public IDInput DoorLock6L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_6_L);
        public IDInput DoorLatch6R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_6_R);
        public IDInput DoorLatch6L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_6_L);
        public IDInput DoorLock7R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_7_R);
        public IDInput DoorLock7L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LOCK_7_L);
        public IDInput DoorLatch7R => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_7_R);
        public IDInput DoorLatch7L => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DOOR_LATCH_7_L);
        public IDInput TransferFixtureUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_UP);
        public IDInput TransferFixtureDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_DOWN);
        public IDInput TransferFixture11Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_1_1_CLAMP);
        public IDInput TransferFixture11Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_1_1_UNCLAMP);
        public IDInput TransferFixture12Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_1_2_CLAMP);
        public IDInput TransferFixture12Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_1_2_UNCLAMP);
        public IDInput TransferFixture21Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_2_1_CLAMP);
        public IDInput TransferFixture21Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_2_1_UNCLAMP);
        public IDInput TransferFixture22Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_2_2_CLAMP);
        public IDInput TransferFixture22Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_FIXTURE_2_2_UNCLAMP);
        public IDInput DetachFixtureDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIXTURE_DETECT);
        public IDInput DetachCyl1Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_CYL_1_UP);
        public IDInput DetachCyl1Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_CYL_1_DOWN);
        public IDInput DetachCyl2Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_CYL_2_UP);
        public IDInput DetachCyl2Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_CYL_2_DOWN);
        public IDInput DetachFixFixtureCyl11Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_1_1_BW);
        public IDInput DetachFixFixtureCyl11Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_1_1_FW);
        public IDInput DetachFixFixtureCyl12Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_1_2_BW);
        public IDInput DetachFixFixtureCyl12Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_1_2_FW);
        public IDInput DetachFixFixtureCyl21Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_2_1_BW);
        public IDInput DetachFixFixtureCyl21Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_2_1_FW);
        public IDInput DetachFixFixtureCyl22Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_2_2_BW);
        public IDInput DetachFixFixtureCyl22Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_FIX_FIXTURE_CYL_2_2_FW);
        public IDInput RemoveZoneTrCylBw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_TR_CYL_BW);
        public IDInput RemoveZoneTrCylFw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_TR_CYL_FW);
        public IDInput RemoveZoneZCyl1Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_Z_CYL_1_UP);
        public IDInput RemoveZoneZCyl1Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_Z_CYL_1_DOWN);
        public IDInput RemoveZoneZCyl2Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_Z_CYL_2_UP);
        public IDInput RemoveZoneZCyl2Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_Z_CYL_2_DOWN);
        public IDInput RemoveZoneCyl1Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_CYL_1_CLAMP);
        public IDInput RemoveZoneCyl1Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_CYL_1_UNCLAMP);
        public IDInput RemoveZoneCyl2Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_CYL_2_CLAMP);
        public IDInput RemoveZoneCyl2Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_CYL_2_UNCLAMP);
        public IDInput RemoveZoneCyl3Clamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_CYL_3_CLAMP);
        public IDInput RemoveZoneCyl3Unclamp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_CYL_3_UNCLAMP);
        public IDInput RemoveZonePusherCyl1Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_PUSHER_CYL_1_UP);
        public IDInput RemoveZonePusherCyl1Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_PUSHER_CYL_1_DOWN);
        public IDInput RemoveZonePusherCyl2Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_PUSHER_CYL_2_UP);
        public IDInput RemoveZonePusherCyl2Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_PUSHER_CYL_2_DOWN);
        public IDInput RemoveZoneFixCyl11Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_1_1_FW);
        public IDInput RemoveZoneFixCyl11Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_1_1_BW);
        public IDInput RemoveZoneFixCyl12Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_1_2_FW);
        public IDInput RemoveZoneFixCyl12Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_1_2_BW);
        public IDInput RemoveZoneFixCyl21Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_2_1_FW);
        public IDInput RemoveZoneFixCyl21Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_2_1_BW);
        public IDInput RemoveZoneFixCyl22Fw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_2_2_FW);
        public IDInput RemoveZoneFixCyl22Bw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIX_CYL_2_2_BW);
        public IDInput RemoveZoneFullTapeDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FULL_TAPE_DETECT);
        public IDInput RemoveZoneFixtureDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.REMOVE_ZONE_FIXTURE_DETECT);
        public IDInput DetachGlassShtVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_GLASS_SHT_VAC_1);
        public IDInput DetachGlassShtVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_GLASS_SHT_VAC_2);
        public IDInput DetachGlassShtVac3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.DETACH_GLASS_SHT_VAC_3);
        public IDInput TransferInShuttleLVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_IN_SHUTTLE_L_VAC);
        public IDInput TransferInShuttleRVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_IN_SHUTTLE_R_VAC);
        public IDInput TransferInShuttleL0Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_IN_SHUTTLE_L_0_DEGREE);
        public IDInput TransferInShuttleL180Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_IN_SHUTTLE_L_180_DEGREE);
        public IDInput TransferInShuttleR0Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_IN_SHUTTLE_R_0_DEGREE);
        public IDInput TransferInShuttleR180Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TRANSFER_IN_SHUTTLE_R_180_DEGREE);
        public IDInput AlignFixtureDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_FIXTURE_DETECT);
        public IDInput AlignFixtureTiltDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_FIXTURE_TILT_DETECT);
        public IDInput AlignFixtureReverseDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_FIXTURE_REVERSE_DETECT);
        public IDInput FixtureAlign1CylBw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.FIXTURE_ALIGN_1_CYL_BW);
        public IDInput FixtureAlign1CylFw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.FIXTURE_ALIGN_1_CYL_FW);
        public IDInput FixtureAlign2CylBw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.FIXTURE_ALIGN_2_CYL_BW);
        public IDInput FixtureAlign2CylFw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.FIXTURE_ALIGN_2_CYL_FW);
        public IDInput AlignStageLVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_VAC_1);
        public IDInput AlignStageLVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_VAC_2);
        public IDInput AlignStageLVac3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_VAC_3);
        public IDInput AlignStageLGlassDettect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_GLASS_DETTECT_1);
        public IDInput AlignStageLGlassDettect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_GLASS_DETTECT_2);
        public IDInput AlignStageLGlassDettect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_GLASS_DETTECT_3);
        public IDInput AlignStageLBrushCylUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_BRUSH_CYL_UP);
        public IDInput AlignStageLBrushCylDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_BRUSH_CYL_DOWN);
        public IDInput AlignStageL1Align => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_1_ALIGN);
        public IDInput AlignStageL1Unalign => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_1_UNALIGN);
        public IDInput AlignStageL2Align => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_2_ALIGN);
        public IDInput AlignStageL2Unalign => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_2_UNALIGN);
        public IDInput AlignStageL3Align => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_3_ALIGN);
        public IDInput AlignStageL3Unalign => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_L_3_UNALIGN);
        public IDInput AlignStageRVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_VAC_1);
        public IDInput AlignStageRVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_VAC_2);
        public IDInput AlignStageRVac3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_VAC_3);
        public IDInput AlignStageRGlassDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_GLASS_DETECT_1);
        public IDInput AlignStageRGlassDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_GLASS_DETECT_2);
        public IDInput AlignStageRGlassDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_GLASS_DETECT_3);
        public IDInput AlignStageRBrushCylUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_BRUSH_CYL_UP);
        public IDInput AlignStageRBrushCylDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_BRUSH_CYL_DOWN);
        public IDInput AlignStageR1Align => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_1_ALIGN);
        public IDInput AlignStageR1Unalign => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_1_UNALIGN);
        public IDInput AlignStageR2Align => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_2_ALIGN);
        public IDInput AlignStageR2Unalign => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_2_UNALIGN);
        public IDInput AlignStageR3Align => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_3_ALIGN);
        public IDInput AlignStageR3Unalign => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.ALIGN_STAGE_R_3_UNALIGN);
        public IDInput GlassTransferVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_VAC_1);
        public IDInput GlassTransferVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_VAC_2);
        public IDInput GlassTransferVac3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_VAC_3);
        public IDInput GlassTransferCyl1Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_CYL_1_UP);
        public IDInput GlassTransferCyl1Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_CYL_1_DOWN);
        public IDInput GlassTransferCyl2Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_CYL_2_UP);
        public IDInput GlassTransferCyl2Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_CYL_2_DOWN);
        public IDInput GlassTransferCyl3Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_CYL_3_UP);
        public IDInput GlassTransferCyl3Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.GLASS_TRANSFER_CYL_3_DOWN);
        public IDInput Shuttle1RVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.SHUTTLE_1_R_VAC);
        public IDInput Shuttle2RVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.SHUTTLE_2_R_VAC);
        public IDInput Shuttle1LVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.SHUTTLE_1_L_VAC);
        public IDInput Shuttle2LVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.SHUTTLE_2_L_VAC);
        public IDInput WetCleanPusherRightUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_PUSHER_RIGHT_UP);
        public IDInput WetCleanPusherRightDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_PUSHER_RIGHT_DOWN);
        public IDInput WetCleanRightPumpLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_RIGHT_PUMP_LEAK_DETECT);
        public IDInput WetCleanRightWiperCleanDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1);
        public IDInput WetCleanRightWiperCleanDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2);
        public IDInput WetCleanRightWiperCleanDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3);
        public IDInput WetCleanRightDoorLock => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_RIGHT_DOOR_LOCK);
        public IDInput WetCleanRightAlcoholLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT);
        public IDInput WetCleanPusherLeftUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_PUSHER_LEFT_UP);
        public IDInput WetCleanPusherLeftDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_PUSHER_LEFT_DOWN);
        public IDInput WetCleanLeftPumpLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_LEFT_PUMP_LEAK_DETECT);
        public IDInput WetCleanLeftWiperCleanDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_1);
        public IDInput WetCleanLeftWiperCleanDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_2);
        public IDInput WetCleanLeftWiperCleanDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_3);
        public IDInput WetCleanLeftDoorLock => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_LEFT_DOOR_LOCK);
        public IDInput WetCleanLeftAlcoholLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_LEFT_ALCOHOL_LEAK_DETECT);
        public IDInput WetCleanBrushRightUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_BRUSH_RIGHT_UP);
        public IDInput WetCleanBrushRightDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_BRUSH_RIGHT_DOWN);
        public IDInput WetCleanBrushLeftUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_BRUSH_LEFT_UP);
        public IDInput WetCleanBrushLeftDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_BRUSH_LEFT_DOWN);
        public IDInput TrRotateRightVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_VAC_1);
        public IDInput TrRotateRight0Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_0_DEGREE);
        public IDInput TrRotateRight180Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_180_DEGREE);
        public IDInput TrRotateRightBw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_BW);
        public IDInput TrRotateRightFw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_FW);
        public IDInput TrRotateRightVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_VAC_2);
        public IDInput WetCleanRightFeedingRollerDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_RIGHT_FEEDING_ROLLER_DETECT);
        public IDInput WetCleanLeftFeedingRollerDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.WET_CLEAN_LEFT_FEEDING_ROLLER_DETECT);
        public IDInput AfCleanRightFeedingRollerDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_RIGHT_FEEDING_ROLLER_DETECT);
        public IDInput AfCleanLeftFeedingRollerDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_LEFT_FEEDING_ROLLER_DETECT);
        public IDInput TrRotateLeftVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_VAC_1);
        public IDInput TrRotateLeft0Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_0_DEGREE);
        public IDInput TrRotateLeft180Degree => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_180_DEGREE);
        public IDInput TrRotateLeftBw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_BW);
        public IDInput TrRotateLeftFw => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_FW);
        public IDInput TrRotateLeftVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_VAC_2);
        public IDInput AfCleanPusherRightUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_PUSHER_RIGHT_UP);
        public IDInput AfCleanPusherRightDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_PUSHER_RIGHT_DOWN);
        public IDInput AfCleanRightPumpLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_RIGHT_PUMP_LEAK_DETECT);
        public IDInput AfCleanRightWiperCleanDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1);
        public IDInput AfCleanRightWiperCleanDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2);
        public IDInput AfCleanRightWiperCleanDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3);
        public IDInput AfCleanRightDoorLock => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_RIGHT_DOOR_LOCK);
        public IDInput AfCleanRightAlcoholLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT);
        public IDInput AfCleanPusherLeftUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_PUSHER_LEFT_UP);
        public IDInput AfCleanPusherLeftDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_PUSHER_LEFT_DOWN);
        public IDInput AfCleanLeftPumpLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_LEFT_PUMP_LEAK_DETECT);
        public IDInput AfCleanLeftWiperCleanDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_1);
        public IDInput AfCleanLeftWiperCleanDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_2);
        public IDInput AfCleanLeftWiperCleanDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_3);
        public IDInput AfCleanLeftDoorLock => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_LEFT_DOOR_LOCK);
        public IDInput AfCleanLeftAlcoholLeakDetect => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_LEFT_ALCOHOL_LEAK_DETECT);
        public IDInput AfCleanBrushRightUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_BRUSH_RIGHT_UP);
        public IDInput AfCleanBrushRightDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_BRUSH_RIGHT_DOWN);
        public IDInput AfCleanBrushLeftUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_BRUSH_LEFT_UP);
        public IDInput AfCleanBrushLeftDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AF_CLEAN_BRUSH_LEFT_DOWN);
        public IDInput OutShuttleGlassCoatingDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_SHUTTLE_GLASS_COATING_DETECT_1);
        public IDInput OutShuttleGlassCoatingDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_SHUTTLE_GLASS_COATING_DETECT_2);
        public IDInput OutShuttleVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_SHUTTLE_VAC_1);
        public IDInput OutShuttleVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_SHUTTLE_VAC_2);
        public IDInput UnloadTransferLVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_TRANSFER_L_VAC);
        public IDInput UnloadTransferRVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_TRANSFER_R_VAC);
        public IDInput OpRButtonStop => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_R_BUTTON_STOP);
        public IDInput OpRButtonStart => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_R_BUTTON_START);
        public IDInput OpRButtonReset => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_R_BUTTON_RESET);
        public IDInput AutoModeSwitchR => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AUTO_MODE_SWITCH_R);
        public IDInput TeachModeSwitchR => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TEACH_MODE_SWITCH_R);
        public IDInput AutoModeSwitchL => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.AUTO_MODE_SWITCH_L);
        public IDInput TeachModeSwitchL => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TEACH_MODE_SWITCH_L);
        public IDInput OpREmo => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_R_EMO);
        public IDInput OpLButtonStop => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_L_BUTTON_STOP);
        public IDInput OpLButtonStart => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_L_BUTTON_START);
        public IDInput OpLButtonReset => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_L_BUTTON_RESET);
        public IDInput OpLEmo => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OP_L_EMO);
        public IDInput UnloadGlassAlignVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_ALIGN_VAC_1);
        public IDInput UnloadGlassAlignVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_ALIGN_VAC_2);
        public IDInput UnloadGlassAlignVac3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_ALIGN_VAC_3);
        public IDInput UnloadGlassAlignVac4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_ALIGN_VAC_4);
        public IDInput UnloadGlassDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_DETECT_1);
        public IDInput UnloadGlassDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_DETECT_2);
        public IDInput UnloadGlassDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_DETECT_3);
        public IDInput UnloadGlassDetect4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_GLASS_DETECT_4);
        public IDInput UnloadAlignCyl1Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_1_UP);
        public IDInput UnloadAlignCyl1Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_1_DOWN);
        public IDInput UnloadAlignCyl2Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_2_UP);
        public IDInput UnloadAlignCyl2Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_2_DOWN);
        public IDInput UnloadAlignCyl3Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_3_UP);
        public IDInput UnloadAlignCyl3Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_3_DOWN);
        public IDInput UnloadAlignCyl4Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_4_UP);
        public IDInput UnloadAlignCyl4Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ALIGN_CYL_4_DOWN);
        public IDInput UnloadRobotVac1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_VAC_1);
        public IDInput UnloadRobotVac2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_VAC_2);
        public IDInput UnloadRobotVac3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_VAC_3);
        public IDInput UnloadRobotVac4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_VAC_4);
        public IDInput UnloadRobotDetect1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_DETECT_1);
        public IDInput UnloadRobotDetect2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_DETECT_2);
        public IDInput UnloadRobotDetect3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_DETECT_3);
        public IDInput UnloadRobotDetect4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_DETECT_4);
        public IDInput UnloadRobotCyl1Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_1_UP);
        public IDInput UnloadRobotCyl1Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_1_DOWN);
        public IDInput UnloadRobotCyl2Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_2_UP);
        public IDInput UnloadRobotCyl2Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_2_DOWN);
        public IDInput UnloadRobotCyl3Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_3_UP);
        public IDInput UnloadRobotCyl3Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_3_DOWN);
        public IDInput UnloadRobotCyl4Up => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_4_UP);
        public IDInput UnloadRobotCyl4Down => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROBOT_CYL_4_DOWN);
        public IDInput UnloadRobStopmess => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_STOPMESS);
        public IDInput UnloadRobPeriRdy => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_PERI_RDY);
        public IDInput UnloadRobAlarmStop => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_ALARM_STOP);
        public IDInput UnloadRobUserSaf => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_USER_SAF);
        public IDInput UnloadRobIoActconf => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_IO_ACTCONF);
        public IDInput UnloadRobOnPath => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_ON_PATH);
        public IDInput UnloadRobProAct => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_PRO_ACT);
        public IDInput UnloadRobInHome => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.UNLOAD_ROB_IN_HOME);
        public IDInput EmoUnloadR => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.EMO_UNLOAD_R);
        public IDInput EmoUnloadL => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.EMO_UNLOAD_L);
        public IDInput TrRotateRightUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_UP);
        public IDInput TrRotateRightDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_DOWN);
        public IDInput TrRotateLeftUp => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_UP);
        public IDInput TrRotateLeftDown => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_DOWN);
        public IDInput TrRotateRightRotVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_RIGHT_ROTATE_VAC);
        public IDInput TrRotateLeftRotVac => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.TR_ROTATE_LEFT_ROTATE_VAC);

        //AGV
        public IDInput InAGVSensorOut1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_1);
        public IDInput InAGVSensorOut2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_2);
        public IDInput InAGVSensorOut3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_3);
        public IDInput InAGVSensorOut4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_4);
        public IDInput InAGVSensorOut5 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_5);
        public IDInput InAGVSensorOut6 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_6);
        public IDInput InAGVSensorOut7 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_7);
        public IDInput InAGVSensorOut8 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_OUT_8);
        public IDInput InAGVSensorGo => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.IN_AGV_SENSOR_GO);
        public IDInput OutAGVSensorOut1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_1);
        public IDInput OutAGVSensorOut2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_2);
        public IDInput OutAGVSensorOut3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_3);
        public IDInput OutAGVSensorOut4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_4);
        public IDInput OutAGVSensorOut5 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_5);
        public IDInput OutAGVSensorOut6 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_6);
        public IDInput OutAGVSensorOut7 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_7);
        public IDInput OutAGVSensorOut8 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_OUT_8);
        public IDInput OutAGVSensorGo => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.OUT_AGV_SENSOR_GO);

        //Plasma
        public IDInput PlasmaStatus1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.PLASMA_STATUS_1);
        public IDInput PlasmaStatus2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.PLASMA_STATUS_2);
        public IDInput PlasmaStatus3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.PLASMA_STATUS_3);
        public IDInput PlasmaStatus4 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.PLASMA_STATUS_4);
        public IDInput RelayPlasmaMotorError => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.RELAY_PLASMA_MOTOR_ERROR);

        //Main Air
        public IDInput MainAir1 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.MAIN_AIR_1);
        public IDInput MainAir2 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.MAIN_AIR_2);
        public IDInput MainAir3 => _dInputDevice.Inputs.First(i => i.Id == (int)EInput.MAIN_AIR_3);

    }
}
