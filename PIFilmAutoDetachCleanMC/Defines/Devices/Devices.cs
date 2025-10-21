using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Device.SpeedController;
using EQX.Motion.ByVendor.Inovance;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices
{
    public class Devices
    {
        public Devices(Inputs inputs,
            Outputs outputs,
            Motions motions,
            Cylinders cylinders,
            TorqueControllerList torqueControllers,
            RollerList speedControllerList,
            Regulators regulators,
            AnalogInputs analogInputs,
            MachineStatus machineStatus,
            SyringePumps syringePumps,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            Inputs = inputs;
            Outputs = outputs;
            Motions = motions;
            Cylinders = cylinders;
            TorqueControllers = torqueControllers;
            RollerList = speedControllerList;
            Regulators = regulators;
            AnalogInputs = analogInputs;
            SyringePumps = syringePumps;
            CylinderInterlockConfigurator.Configure(this, cstLoadUnloadRecipe);
        }

        public Inputs Inputs { get; }
        public Outputs Outputs { get; }
        public Motions Motions { get; }
        public Cylinders Cylinders { get; }
        public TorqueControllerList TorqueControllers { get; }
        public RollerList RollerList { get; }
        public Regulators Regulators { get; }
        public AnalogInputs AnalogInputs { get; }
        public SyringePumps SyringePumps { get; }

        #region Public Methods

        #region Get Cylinders
        public ObservableCollection<ICylinder> GetInConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.InCV_StopperCyl);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetInWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.InCV_StopperCyl);
            cylinders.Add(Cylinders.InWorkCV_AlignCyl1);
            cylinders.Add(Cylinders.InWorkCV_AlignCyl2);
            cylinders.Add(Cylinders.InWorkCV_TiltCyl);
            cylinders.Add(Cylinders.InWorkCV_SupportCyl1);
            cylinders.Add(Cylinders.InWorkCV_SupportCyl2);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetBufferConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.BufferCV_StopperCyl1);
            cylinders.Add(Cylinders.BufferCV_StopperCyl2);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetOutWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.OutCV_StopperCyl);
            cylinders.Add(Cylinders.OutWorkCV_AlignCyl1);
            cylinders.Add(Cylinders.OutWorkCV_AlignCyl2);
            cylinders.Add(Cylinders.OutWorkCV_TiltCyl);
            cylinders.Add(Cylinders.OutWorkCV_SupportCyl2);
            cylinders.Add(Cylinders.OutWorkCV_SupportCyl1);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetOutConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.OutCV_StopperCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetVinylCleanCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.VinylClean_BwFwCyl);
            cylinders.Add(Cylinders.VinylClean_UpDownCyl);
            cylinders.Add(Cylinders.VinylClean_ClampCyl1);
            cylinders.Add(Cylinders.VinylClean_ClampCyl2);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferFixtureCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.TransferFixture_UpDownCyl);
            cylinders.Add(Cylinders.TransferFixture_ClampCyl1);
            cylinders.Add(Cylinders.TransferFixture_ClampCyl2);
            cylinders.Add(Cylinders.TransferFixture_ClampCyl3);
            cylinders.Add(Cylinders.TransferFixture_ClampCyl4);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetFixtureAlignCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.FixtureAlign_AlignCyl1);
            cylinders.Add(Cylinders.FixtureAlign_AlignCyl2);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetRemoveFilmCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.RemoveZone_ClampCyl1);
            cylinders.Add(Cylinders.RemoveZone_ClampCyl2);
            cylinders.Add(Cylinders.RemoveZone_ClampCyl3);
            cylinders.Add(Cylinders.RemoveZone_ClampCyl4);
            cylinders.Add(Cylinders.RemoveZone_FilmClampCyl1);
            cylinders.Add(Cylinders.RemoveZone_FilmClampCyl2);
            cylinders.Add(Cylinders.RemoveZone_FilmClampCyl3);
            cylinders.Add(Cylinders.RemoveZone_PusherCyl1);
            cylinders.Add(Cylinders.RemoveZone_PusherCyl2);
            cylinders.Add(Cylinders.RemoveZone_TransferCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetRobotLoadCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.RobotLoad_AlignCyl);
            cylinders.Add(Cylinders.RobotLoad_ClampCyl1);
            cylinders.Add(Cylinders.RobotLoad_ClampCyl2);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetDetachCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.Detach_ClampCyl1);
            cylinders.Add(Cylinders.Detach_ClampCyl2);
            cylinders.Add(Cylinders.Detach_ClampCyl3);
            cylinders.Add(Cylinders.Detach_ClampCyl4);

            cylinders.Add(Cylinders.Detach_UpDownCyl1);
            cylinders.Add(Cylinders.Detach_UpDownCyl2);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetGlassTransferCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.GlassTransfer_UpDownCyl1);
            cylinders.Add(Cylinders.GlassTransfer_UpDownCyl2);
            cylinders.Add(Cylinders.GlassTransfer_UpDownCyl3);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferInShuttleLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TransferInShuttleL_RotateCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetGlassAlignLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AlignStageL_AlignCyl1);
            cylinders.Add(Cylinders.AlignStageL_AlignCyl2);
            cylinders.Add(Cylinders.AlignStageL_AlignCyl3);
            cylinders.Add(Cylinders.AlignStageL_BrushCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferInShuttleRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TransferInShuttleR_RotateCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetGlassAlignRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AlignStageR_AlignCyl1);
            cylinders.Add(Cylinders.AlignStageR_AlignCyl2);
            cylinders.Add(Cylinders.AlignStageR_AlignCyl3);
            cylinders.Add(Cylinders.AlignStageR_BrushCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferRotationLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TransferRotationL_RotationCyl);
            cylinders.Add(Cylinders.TransferRotationL_BwFwCyl);
            cylinders.Add(Cylinders.TransferRotationLeft_UpDownCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferRotationRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TransferRotationR_RotationCyl);
            cylinders.Add(Cylinders.TransferRotationR_BwFwCyl);
            cylinders.Add(Cylinders.TransferRotationR_UpDownCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadTransferLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl1);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl2);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl3);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl4);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadTransferRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl1);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl2);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl3);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl4);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetWETCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.WetCleanL_PusherCyl);
            cylinders.Add(Cylinders.WetCleanL_BrushCyl);
            cylinders.Add(Cylinders.WetCleanL_ClampCyl1);
            cylinders.Add(Cylinders.WetCleanL_ClampCyl2);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetWETCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.WetCleanR_PusherCyl);
            cylinders.Add(Cylinders.WetCleanR_BrushCyl);
            cylinders.Add(Cylinders.WetCleanR_ClampCyl1);
            cylinders.Add(Cylinders.WetCleanR_ClampCyl2);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetAFCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AFCleanL_PusherCyl);
            cylinders.Add(Cylinders.AFCleanL_BrushCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetAFCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AFCleanR_PusherCyl);
            cylinders.Add(Cylinders.AFCleanR_BrushCyl);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadAlignCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl1);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl2);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl3);
            cylinders.Add(Cylinders.UnloadAlign_UpDownCyl4);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadRobotCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadRobot_UpDownCyl1);
            cylinders.Add(Cylinders.UnloadRobot_UpDownCyl2);
            cylinders.Add(Cylinders.UnloadRobot_UpDownCyl3);
            cylinders.Add(Cylinders.UnloadRobot_UpDownCyl4);

            return cylinders;
        }
        #endregion

        #region Get Inputs
        public ObservableCollection<IDInput> GetInConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.InCstDetect1);
            inputs.Add(Inputs.InCstDetect2);
            inputs.Add(Inputs.InCV_StopperUp);
            inputs.Add(Inputs.InCV_StopperDown);

            return inputs;
        }

        public ObservableCollection<IDInput> GetInWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.InCstDetect1);
            inputs.Add(Inputs.InCstDetect2);
            inputs.Add(Inputs.InCvSupportUp);
            inputs.Add(Inputs.InCvSupportDown);
            inputs.Add(Inputs.InCvSupportBufferUp);
            inputs.Add(Inputs.InCvSupportBufferDown);
            inputs.Add(Inputs.InWorkCV_AlignCyl1Fw);
            inputs.Add(Inputs.InWorkCV_AlignCyl1Bw);
            inputs.Add(Inputs.InWorkCV_AlignCyl2Fw);
            inputs.Add(Inputs.InWorkCV_AlignCyl2Bw);
            inputs.Add(Inputs.InWorkCV_TiltCylUp);
            inputs.Add(Inputs.InWorkCV_TiltCylDown);
            inputs.Add(Inputs.InCV_StopperUp);
            inputs.Add(Inputs.InCV_StopperDown);

            return inputs;
        }

        public ObservableCollection<IDInput> GetBufferConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.BufferCstDetect1);
            inputs.Add(Inputs.BufferCstDetect2);
            inputs.Add(Inputs.BufferCV_StopperCyl1Up);
            inputs.Add(Inputs.BufferCV_StopperCyl1Down);
            inputs.Add(Inputs.BufferCV_StopperCyl2Up);
            inputs.Add(Inputs.BufferCV_StopperCyl2Down);
            return inputs;
        }

        public ObservableCollection<IDInput> GetOutWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.OutCstDetect1);
            inputs.Add(Inputs.OutCstDetect2);
            inputs.Add(Inputs.OutCvSupportUp);
            inputs.Add(Inputs.OutCvSupportDown);
            inputs.Add(Inputs.OutCvSupportBufferUp);
            inputs.Add(Inputs.OutCvSupportBufferDown);
            inputs.Add(Inputs.OutWorkCV_AlignCyl1Fw);
            inputs.Add(Inputs.OutWorkCV_AlignCyl1Bw);
            inputs.Add(Inputs.OutWorkCV_AlignCyl2Fw);
            inputs.Add(Inputs.OutWorkCV_AlignCyl2Bw);
            inputs.Add(Inputs.OutWorkCV_TiltCylUp);
            inputs.Add(Inputs.OutWorkCV_TiltCylDown);
            inputs.Add(Inputs.OutCV_StopperUp);
            inputs.Add(Inputs.OutCV_StopperDown);

            return inputs;
        }

        public ObservableCollection<IDInput> GetOutConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.OutCstDetect1);
            inputs.Add(Inputs.OutCstDetect2);
            inputs.Add(Inputs.OutCV_StopperUp);
            inputs.Add(Inputs.OutCV_StopperDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetVinylCleanInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.VinylCleanFixtureDetect);
            inputs.Add(Inputs.VinylCleanRunoffDetect);
            inputs.Add(Inputs.VinylCleanFullNotDetect);
            inputs.Add(Inputs.VinylCleanFixture1Clamp);
            inputs.Add(Inputs.VinylCleanFixture2Clamp);
            inputs.Add(Inputs.VinylCleanFixture1Unclamp);
            inputs.Add(Inputs.VinylCleanFixture2Unclamp);
            inputs.Add(Inputs.VinylCleanPusherRollerUp);
            inputs.Add(Inputs.VinylCleanPusherRollerDown);
            inputs.Add(Inputs.VinylCleanRollerFw);
            inputs.Add(Inputs.VinylCleanRollerBw);
            return inputs;
        }

        public ObservableCollection<IDInput> GetRobotLoadInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.LoadRobStopmess);
            inputs.Add(Inputs.LoadRobPeriRdy);
            inputs.Add(Inputs.LoadRobAlarmStop);
            inputs.Add(Inputs.LoadRobUserSaf);
            inputs.Add(Inputs.LoadRobIoActconf);
            inputs.Add(Inputs.LoadRobOnPath);
            inputs.Add(Inputs.LoadRobProAct);
            inputs.Add(Inputs.LoadRobInHome);
            inputs.Add(Inputs.RobotFixtureAlignFw);
            inputs.Add(Inputs.RobotFixtureAlignBw);
            inputs.Add(Inputs.RobotFixture1Clamp);
            inputs.Add(Inputs.RobotFixture1Unclamp);
            inputs.Add(Inputs.RobotFixture2Clamp);
            inputs.Add(Inputs.RobotFixture2Unclamp);

            inputs.Add(Inputs.LoadRobStopmess);
            inputs.Add(Inputs.LoadRobPeriRdy);
            inputs.Add(Inputs.LoadRobAlarmStop);
            inputs.Add(Inputs.LoadRobUserSaf);
            inputs.Add(Inputs.LoadRobIoActconf);
            inputs.Add(Inputs.LoadRobOnPath);
            inputs.Add(Inputs.LoadRobProAct);
            inputs.Add(Inputs.LoadRobInHome);
            return inputs;
        }

        public ObservableCollection<IDInput> GetTransferFixtureInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.TransferFixtureUp);
            inputs.Add(Inputs.TransferFixtureDown);
            inputs.Add(Inputs.TransferFixture11Clamp);
            inputs.Add(Inputs.TransferFixture11Unclamp);
            inputs.Add(Inputs.TransferFixture12Clamp);
            inputs.Add(Inputs.TransferFixture12Unclamp);
            inputs.Add(Inputs.TransferFixture21Clamp);
            inputs.Add(Inputs.TransferFixture21Unclamp);
            inputs.Add(Inputs.TransferFixture22Clamp);
            inputs.Add(Inputs.TransferFixture22Unclamp);

            return inputs;
        }

        public ObservableCollection<IDInput> GetFixtureAlignInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AlignFixtureDetect);
            inputs.Add(Inputs.AlignFixtureReverseDetect);
            inputs.Add(Inputs.AlignFixtureTiltDetect);
            inputs.Add(Inputs.FixtureAlign1CylBw);
            inputs.Add(Inputs.FixtureAlign1CylFw);
            inputs.Add(Inputs.FixtureAlign2CylBw);
            inputs.Add(Inputs.FixtureAlign2CylFw);

            return inputs;
        }

        public ObservableCollection<IDInput> GetRemoveFilmInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.RemoveZoneFixtureDetect);
            inputs.Add(Inputs.RemoveZoneClampCyl1Clamp);
            inputs.Add(Inputs.RemoveZoneClampCyl1Unclamp);
            inputs.Add(Inputs.RemoveZoneClampCyl2Clamp);
            inputs.Add(Inputs.RemoveZoneClampCyl2Unclamp);
            inputs.Add(Inputs.RemoveZoneClampCyl3Clamp);
            inputs.Add(Inputs.RemoveZoneClampCyl3Unclamp);
            inputs.Add(Inputs.RemoveZoneClampCyl4Clamp);
            inputs.Add(Inputs.RemoveZoneClampCyl4Unclamp);
            inputs.Add(Inputs.RemoveZoneFilm1Clamp);
            inputs.Add(Inputs.RemoveZoneFilm1Unclamp);
            inputs.Add(Inputs.RemoveZoneFilm2Clamp);
            inputs.Add(Inputs.RemoveZoneFilm2Unclamp);
            inputs.Add(Inputs.RemoveZoneFilm3Clamp);
            inputs.Add(Inputs.RemoveZoneFilm3Unclamp);
            inputs.Add(Inputs.RemoveZonePusherCyl1Up);
            inputs.Add(Inputs.RemoveZonePusherCyl1Down);
            inputs.Add(Inputs.RemoveZonePusherCyl2Up);
            inputs.Add(Inputs.RemoveZonePusherCyl2Down);
            inputs.Add(Inputs.RemoveZoneTrCylFw);
            inputs.Add(Inputs.RemoveZoneTrCylBw);
            return inputs;
        }

        public ObservableCollection<IDInput> GetDetachInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.DetachFixtureDetect);
            inputs.Add(Inputs.DetachGlassShtVac1);
            inputs.Add(Inputs.DetachGlassShtVac2);
            inputs.Add(Inputs.DetachGlassShtVac3);
            inputs.Add(Inputs.DetachCyl1Up);
            inputs.Add(Inputs.DetachCyl1Down);
            inputs.Add(Inputs.DetachCyl2Up);
            inputs.Add(Inputs.DetachCyl2Down);
            inputs.Add(Inputs.DetachCyl1Clamp);
            inputs.Add(Inputs.DetachCyl1UnClamp);
            inputs.Add(Inputs.DetachCyl2Clamp);
            inputs.Add(Inputs.DetachCyl2UnClamp);
            inputs.Add(Inputs.DetachCyl3Clamp);
            inputs.Add(Inputs.DetachCyl3UnClamp);
            inputs.Add(Inputs.DetachCyl4Clamp);
            inputs.Add(Inputs.DetachCyl4Unclamp);

            return inputs;
        }

        public ObservableCollection<IDInput> GetTransferShutterLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AlignStageLGlassDetect1);
            inputs.Add(Inputs.AlignStageLGlassDetect2);
            inputs.Add(Inputs.AlignStageLGlassDetect3);
            inputs.Add(Inputs.TransferInShuttleLVac);
            inputs.Add(Inputs.TransferInShuttleL0Degree);
            inputs.Add(Inputs.TransferInShuttleL180Degree);
            return inputs;
        }

        public ObservableCollection<IDInput> GetGlassAlignLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AlignStageLGlassDetect1);
            inputs.Add(Inputs.AlignStageLGlassDetect2);
            inputs.Add(Inputs.AlignStageLGlassDetect3);
            inputs.Add(Inputs.AlignStageL1Align);
            inputs.Add(Inputs.AlignStageL1Unalign);
            inputs.Add(Inputs.AlignStageL2Align);
            inputs.Add(Inputs.AlignStageL2Unalign);
            inputs.Add(Inputs.AlignStageL3Align);
            inputs.Add(Inputs.AlignStageL3Unalign);
            inputs.Add(Inputs.AlignStageLBrushCylUp);
            inputs.Add(Inputs.AlignStageLBrushCylDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetTransferShutterRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AlignStageRGlassDetect1);
            inputs.Add(Inputs.AlignStageRGlassDetect2);
            inputs.Add(Inputs.AlignStageRGlassDetect3);
            inputs.Add(Inputs.TransferInShuttleRVac);
            inputs.Add(Inputs.TransferInShuttleR0Degree);
            inputs.Add(Inputs.TransferInShuttleR180Degree);
            return inputs;
        }

        public ObservableCollection<IDInput> GetGlassAlignRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AlignStageRGlassDetect1);
            inputs.Add(Inputs.AlignStageRGlassDetect2);
            inputs.Add(Inputs.AlignStageRGlassDetect3);
            inputs.Add(Inputs.AlignStageR1Align);
            inputs.Add(Inputs.AlignStageR1Unalign);
            inputs.Add(Inputs.AlignStageR2Align);
            inputs.Add(Inputs.AlignStageR2Unalign);
            inputs.Add(Inputs.AlignStageR3Align);
            inputs.Add(Inputs.AlignStageR3Unalign);
            inputs.Add(Inputs.AlignStageRBrushCylUp);
            inputs.Add(Inputs.AlignStageRBrushCylDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetTransferRotationLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Vacuum detection inputs
            inputs.Add(Inputs.TrRotateLeftVac1);
            inputs.Add(Inputs.TrRotateLeftVac2);
            inputs.Add(Inputs.TrRotateLeftRotVac);
            inputs.Add(Inputs.TrRotateLeft0Degree);
            inputs.Add(Inputs.TrRotateLeft180Degree);
            inputs.Add(Inputs.TrRotateLeftFw);
            inputs.Add(Inputs.TrRotateLeftBw);
            inputs.Add(Inputs.TrRotateLeftUp);
            inputs.Add(Inputs.TrRotateLeftDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetTransferRotationRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.TrRotateRightVac1);
            inputs.Add(Inputs.TrRotateRightVac2);
            inputs.Add(Inputs.TrRotateRightRotVac);
            inputs.Add(Inputs.TrRotateRight0Degree);
            inputs.Add(Inputs.TrRotateRight180Degree);
            inputs.Add(Inputs.TrRotateRightFw);
            inputs.Add(Inputs.TrRotateRightBw);
            inputs.Add(Inputs.TrRotateRightUp);
            inputs.Add(Inputs.TrRotateRightDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetUnloadTransferLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.UnloadTransferLVac);
            inputs.Add(Inputs.UnloadGlassAlignVac1);
            inputs.Add(Inputs.UnloadGlassAlignVac2);
            inputs.Add(Inputs.UnloadRobotCyl1Up);
            inputs.Add(Inputs.UnloadRobotCyl1Down);
            inputs.Add(Inputs.UnloadRobotCyl2Up);
            inputs.Add(Inputs.UnloadRobotCyl2Down);
            inputs.Add(Inputs.UnloadRobotCyl3Up);
            inputs.Add(Inputs.UnloadRobotCyl3Down);
            inputs.Add(Inputs.UnloadRobotCyl4Up);
            inputs.Add(Inputs.UnloadRobotCyl4Down);
            inputs.Add(Inputs.UnloadAlignCyl1Up);
            inputs.Add(Inputs.UnloadAlignCyl1Down);
            inputs.Add(Inputs.UnloadAlignCyl2Up);
            inputs.Add(Inputs.UnloadAlignCyl2Down);
            return inputs;
        }

        public ObservableCollection<IDInput> GetUnloadTransferRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.UnloadTransferRVac);
            inputs.Add(Inputs.UnloadGlassAlignVac3);
            inputs.Add(Inputs.UnloadGlassAlignVac4);
            inputs.Add(Inputs.UnloadRobotCyl1Up);
            inputs.Add(Inputs.UnloadRobotCyl1Down);
            inputs.Add(Inputs.UnloadRobotCyl2Up);
            inputs.Add(Inputs.UnloadRobotCyl2Down);
            inputs.Add(Inputs.UnloadRobotCyl3Up);
            inputs.Add(Inputs.UnloadRobotCyl3Down);
            inputs.Add(Inputs.UnloadRobotCyl4Up);
            inputs.Add(Inputs.UnloadRobotCyl4Down);
            inputs.Add(Inputs.UnloadAlignCyl3Up);
            inputs.Add(Inputs.UnloadAlignCyl3Down);
            inputs.Add(Inputs.UnloadAlignCyl4Up);
            inputs.Add(Inputs.UnloadAlignCyl4Down);
            return inputs;
        }

        public ObservableCollection<IDInput> GetWETCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.WetCleanPusherLeftUp);
            inputs.Add(Inputs.WetCleanPusherLeftDown);
            inputs.Add(Inputs.WetCleanBrushLeftUp);
            inputs.Add(Inputs.WetCleanBrushLeftDown);
            inputs.Add(Inputs.InShuttleLClamp1FW);
            inputs.Add(Inputs.InShuttleLClamp1BW);
            inputs.Add(Inputs.InShuttleLClamp2FW);
            inputs.Add(Inputs.InShuttleLClamp2BW);
            inputs.Add(Inputs.InShuttleLVac);
            inputs.Add(Inputs.WetCleanLeftAlcoholLeakNotDetect);
            inputs.Add(Inputs.WetCleanLeftPumpLeakNotDetect);
            inputs.Add(Inputs.WetCleanLeftAlcoholPumpDetect);
            inputs.Add(Inputs.WetCleanLeftDoorLock);
            inputs.Add(Inputs.WetCleanLeftFeedingRollerDetect);
            inputs.Add(Inputs.WetCleanLeftWiperCleanDetect1);
            inputs.Add(Inputs.WetCleanLeftWiperCleanDetect2);
            inputs.Add(Inputs.WetCleanLeftWiperCleanDetect3);

            return inputs;
        }

        public ObservableCollection<IDInput> GetWETCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.WetCleanPusherRightUp);
            inputs.Add(Inputs.WetCleanPusherRightDown);
            inputs.Add(Inputs.WetCleanBrushRightUp);
            inputs.Add(Inputs.WetCleanBrushRightDown);
            inputs.Add(Inputs.InShuttleRClamp1FW);
            inputs.Add(Inputs.InShuttleRClamp1BW);
            inputs.Add(Inputs.InShuttleRClamp2FW);
            inputs.Add(Inputs.InShuttleRClamp2BW);
            inputs.Add(Inputs.InShuttleRVac);
            inputs.Add(Inputs.WetCleanRightAlcoholLeakNotDetect);
            inputs.Add(Inputs.WetCleanRightPumpLeakNotDetect);
            inputs.Add(Inputs.WetCleanRightAlcoholPumpDetect);
            inputs.Add(Inputs.WetCleanRightDoorLock);
            inputs.Add(Inputs.WetCleanRightFeedingRollerDetect);
            inputs.Add(Inputs.WetCleanRightWiperCleanDetect1);
            inputs.Add(Inputs.WetCleanRightWiperCleanDetect2);
            inputs.Add(Inputs.WetCleanRightWiperCleanDetect3);
            return inputs;
        }

        public ObservableCollection<IDInput> GetAFCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AfCleanPusherLeftUp);
            inputs.Add(Inputs.AfCleanPusherLeftDown);
            inputs.Add(Inputs.AfCleanBrushLeftUp);
            inputs.Add(Inputs.AfCleanBrushLeftDown);
            inputs.Add(Inputs.OutShuttleLVac);
            inputs.Add(Inputs.AfCleanLeftAlcoholLeakNotDetect);
            inputs.Add(Inputs.AfCleanLeftPumpLeakNotDetect);
            inputs.Add(Inputs.AfCleanLeftAlcoholPumpDetect);
            inputs.Add(Inputs.AfCleanLeftDoorLock);
            inputs.Add(Inputs.AfCleanLeftFeedingRollerDetect);
            inputs.Add(Inputs.AfCleanLeftWiperCleanDetect1);
            inputs.Add(Inputs.AfCleanLeftWiperCleanDetect2);
            inputs.Add(Inputs.AfCleanLeftWiperCleanDetect3);
            return inputs;
        }

        public ObservableCollection<IDInput> GetAFCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AfCleanPusherRightUp);
            inputs.Add(Inputs.AfCleanPusherRightDown);
            inputs.Add(Inputs.AfCleanBrushRightUp);
            inputs.Add(Inputs.AfCleanBrushRightDown);
            inputs.Add(Inputs.OutShuttleRVac);
            inputs.Add(Inputs.AfCleanRightAlcoholLeakNotDetect);
            inputs.Add(Inputs.AfCleanRightPumpLeakNotDetect);
            inputs.Add(Inputs.AfCleanRightAlcoholPumpDetect);
            inputs.Add(Inputs.AfCleanRightDoorLock);
            inputs.Add(Inputs.AfCleanRightFeedingRollerDetect);
            inputs.Add(Inputs.AfCleanRightWiperCleanDetect1);
            inputs.Add(Inputs.AfCleanRightWiperCleanDetect2);
            inputs.Add(Inputs.AfCleanRightWiperCleanDetect3);
            return inputs;
        }

        public ObservableCollection<IDInput> GetGlassTransferInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.GlassTransferVac1);
            inputs.Add(Inputs.GlassTransferVac2);
            inputs.Add(Inputs.GlassTransferVac3);
            inputs.Add(Inputs.GlassTransferCyl1Up);
            inputs.Add(Inputs.GlassTransferCyl1Down);
            inputs.Add(Inputs.GlassTransferCyl2Up);
            inputs.Add(Inputs.GlassTransferCyl2Down);
            inputs.Add(Inputs.GlassTransferCyl3Up);
            inputs.Add(Inputs.GlassTransferCyl3Down);
            return inputs;
        }

        public ObservableCollection<IDInput> GetUnloadAlignInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.UnloadGlassDetect1);
            inputs.Add(Inputs.UnloadGlassDetect2);
            inputs.Add(Inputs.UnloadGlassDetect3);
            inputs.Add(Inputs.UnloadGlassDetect4);
            inputs.Add(Inputs.UnloadAlignCyl1Up);
            inputs.Add(Inputs.UnloadAlignCyl1Down);
            inputs.Add(Inputs.UnloadAlignCyl2Up);
            inputs.Add(Inputs.UnloadAlignCyl2Down);
            inputs.Add(Inputs.UnloadAlignCyl3Up);
            inputs.Add(Inputs.UnloadAlignCyl3Down);
            inputs.Add(Inputs.UnloadAlignCyl4Up);
            inputs.Add(Inputs.UnloadAlignCyl4Down);
            return inputs;
        }

        public ObservableCollection<IDInput> GetUnloadRobotInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.UnloadRobotVac1);
            inputs.Add(Inputs.UnloadRobotVac2);
            inputs.Add(Inputs.UnloadRobotVac3);
            inputs.Add(Inputs.UnloadRobotVac4);
            inputs.Add(Inputs.UnloadRobotDetect1);
            inputs.Add(Inputs.UnloadRobotDetect2);
            inputs.Add(Inputs.UnloadRobotDetect3);
            inputs.Add(Inputs.UnloadRobotDetect4);
            inputs.Add(Inputs.UnloadRobotCyl1Up);
            inputs.Add(Inputs.UnloadRobotCyl1Down);
            inputs.Add(Inputs.UnloadRobotCyl2Up);
            inputs.Add(Inputs.UnloadRobotCyl2Down);
            inputs.Add(Inputs.UnloadRobotCyl3Up);
            inputs.Add(Inputs.UnloadRobotCyl3Down);
            inputs.Add(Inputs.UnloadRobotCyl4Up);
            inputs.Add(Inputs.UnloadRobotCyl4Down);
            inputs.Add(Inputs.UnloadRobStopmess);
            inputs.Add(Inputs.UnloadRobPeriRdy);
            inputs.Add(Inputs.UnloadRobAlarmStop);
            inputs.Add(Inputs.UnloadRobUserSaf);
            inputs.Add(Inputs.UnloadRobIoActconf);
            inputs.Add(Inputs.UnloadRobOnPath);
            inputs.Add(Inputs.UnloadRobProAct);
            inputs.Add(Inputs.UnloadRobInHome);

            return inputs;
        }
        #endregion

        #region GetOutputs
        public ObservableCollection<IDOutput> GetInConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.InCst_StopperUp);
            outputs.Add(Outputs.InCst_StopperDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetInWorkConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.InCst_StopperUp);
            outputs.Add(Outputs.InCst_StopperDown);
            outputs.Add(Outputs.InWorkCst_AlignCyl1Fw);
            outputs.Add(Outputs.InWorkCst_AlignCyl1Bw);
            outputs.Add(Outputs.InWorkCst_AlignCyl2Fw);
            outputs.Add(Outputs.InWorkCst_AlignCyl2Bw);
            outputs.Add(Outputs.InWorkCst_TiltCylUp);
            outputs.Add(Outputs.InWorkCst_TiltCylDown);
            outputs.Add(Outputs.InCvSupportUp);
            outputs.Add(Outputs.InCvSupportDown);
            outputs.Add(Outputs.InCvSupportBufferUp);
            outputs.Add(Outputs.InCvSupportBufferDown);

            return outputs;
        }

        public ObservableCollection<IDOutput> GetBufferConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.BufferCvStopper1Up);
            outputs.Add(Outputs.BufferCvStopper1Down);
            outputs.Add(Outputs.BufferCvStopper2Up);
            outputs.Add(Outputs.BufferCvStopper2Down);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetOutWorkConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.OutCst_StopperUp);
            outputs.Add(Outputs.OutCst_StopperDown);
            outputs.Add(Outputs.OutWorkCst_AlignCyl1Fw);
            outputs.Add(Outputs.OutWorkCst_AlignCyl1Bw);
            outputs.Add(Outputs.OutWorkCst_AlignCyl2Fw);
            outputs.Add(Outputs.OutWorkCst_AlignCyl2Bw);
            outputs.Add(Outputs.OutWorkCst_TiltCylUp);
            outputs.Add(Outputs.OutWorkCst_TiltCylDown);
            outputs.Add(Outputs.OutCvSupportUp);
            outputs.Add(Outputs.OutCvSupportDown);
            outputs.Add(Outputs.OutCvSupportBufferUp);
            outputs.Add(Outputs.OutCvSupportBufferDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetOutConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.OutCst_StopperUp);
            outputs.Add(Outputs.OutCst_StopperDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetVinylCleanOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.VinylCleanFixtureClamp);
            outputs.Add(Outputs.VinylCleanFixtureUnclamp);
            outputs.Add(Outputs.VinylCleanPusherRollerUp);
            outputs.Add(Outputs.VinylCleanRollerFw);
            outputs.Add(Outputs.VinylCleanRollerBw);
            outputs.Add(Outputs.VinylCleanMotorOnOff);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetRobotLoadOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.RobotFixtureClamp);
            outputs.Add(Outputs.RobotFixtureUnclamp);
            outputs.Add(Outputs.RobotFixtureAlignFw);
            outputs.Add(Outputs.RobotFixtureAlignBw);
            outputs.Add(Outputs.LoadRobMoveEnable);
            outputs.Add(Outputs.LoadRobDrivesOn);
            outputs.Add(Outputs.LoadRobDrivesOff);
            outputs.Add(Outputs.LoadRobConfMess);
            outputs.Add(Outputs.LoadRobExtStart);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetTransferFixtureOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.TransferFixtureUp);
            outputs.Add(Outputs.TransferFixtureDown);
            outputs.Add(Outputs.TransferFixture1Clamp);
            outputs.Add(Outputs.TransferFixture1Unclamp);
            outputs.Add(Outputs.TransferFixture2Clamp);
            outputs.Add(Outputs.TransferFixture2Unclamp);

            return outputs;
        }

        public ObservableCollection<IDOutput> GetRemoveFilmOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.RemoveZoneTrCylFw);
            outputs.Add(Outputs.RemoveZoneTrCylBw);
            outputs.Add(Outputs.RemoveZoneZCyl1Down);
            outputs.Add(Outputs.RemoveZoneZCyl2Down);
            outputs.Add(Outputs.RemoveZoneClampCyl1Clamp);
            outputs.Add(Outputs.RemoveZoneClampCyl1Unclamp);
            outputs.Add(Outputs.RemoveZoneClampCyl2Clamp);
            outputs.Add(Outputs.RemoveZoneClampCyl2Unclamp);
            outputs.Add(Outputs.RemoveZoneClampCyl3Clamp);
            outputs.Add(Outputs.RemoveZoneClampCyl3Unclamp);
            outputs.Add(Outputs.RemoveZoneClampCyl4Clamp);
            outputs.Add(Outputs.RemoveZoneClampCyl4Unclamp);
            outputs.Add(Outputs.RemoveZoneFilm1Clamp);
            outputs.Add(Outputs.RemoveZoneFilm1Unclamp);
            outputs.Add(Outputs.RemoveZoneFilm2Clamp);
            outputs.Add(Outputs.RemoveZoneFilm2Unclamp);
            outputs.Add(Outputs.RemoveZoneFilm3Clamp);
            outputs.Add(Outputs.RemoveZoneFilm3Unclamp);
            outputs.Add(Outputs.RemoveZonePusherCyl1Up);
            outputs.Add(Outputs.RemoveZonePusherCyl1Down);
            outputs.Add(Outputs.RemoveZonePusherCyl2Up);
            outputs.Add(Outputs.RemoveZonePusherCyl2Down);
            outputs.Add(Outputs.RemoveZoneIonRunStop);

            return outputs;
        }

        public ObservableCollection<IDOutput> GetFixtureAlignOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.FixtureAlignCyl1Bw);
            outputs.Add(Outputs.FixtureAlignCyl1Fw);
            outputs.Add(Outputs.FixtureAlignCyl2Bw);
            outputs.Add(Outputs.FixtureAlignCyl2Fw);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetDetachOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.DetachGlassShtVac1OnOff);
            outputs.Add(Outputs.DetachGlassShtVac2OnOff);
            outputs.Add(Outputs.DetachGlassShtVac3OnOff);
            outputs.Add(Outputs.DetachCyl1Up);
            outputs.Add(Outputs.DetachCyl1Down);
            outputs.Add(Outputs.DetachCyl2Up);
            outputs.Add(Outputs.DetachCyl2Down);
            outputs.Add(Outputs.DetachClampCyl1Unclamp);
            outputs.Add(Outputs.DetachClampCyl1Clamp);
            outputs.Add(Outputs.DetachClampCyl2Unclamp);
            outputs.Add(Outputs.DetachClampCyl2Clamp);
            outputs.Add(Outputs.DetachClampCyl3Unclamp);
            outputs.Add(Outputs.DetachClampCyl3Clamp);
            outputs.Add(Outputs.DetachClampCyl4Unclamp);
            outputs.Add(Outputs.DetachClampCyl4Clamp);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetGlassTransferOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.GlassTransferVac1OnOff);
            outputs.Add(Outputs.GlassTransferVac2OnOff);
            outputs.Add(Outputs.GlassTransferVac3OnOff);
            outputs.Add(Outputs.GlassTransferCyl1Up);
            outputs.Add(Outputs.GlassTransferCyl1Down);
            outputs.Add(Outputs.GlassTransferCyl2Up);
            outputs.Add(Outputs.GlassTransferCyl2Down);
            outputs.Add(Outputs.GlassTransferCyl3Up);
            outputs.Add(Outputs.GlassTransferCyl3Down);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetTransferShutterLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.TransferInShuttleLVacOnOff);
            outputs.Add(Outputs.TransferInShuttleL0Degree);
            outputs.Add(Outputs.TransferInShuttleL180Degree);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetGlassAlignLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.AlignStageL1Align);
            outputs.Add(Outputs.AlignStageL2Align);
            outputs.Add(Outputs.AlignStageL3Align);
            outputs.Add(Outputs.AlignStageLBrushCylUp);
            outputs.Add(Outputs.AlignStageLVac1OnOff);
            outputs.Add(Outputs.AlignStageLVac2OnOff);
            outputs.Add(Outputs.AlignStageLVac3OnOff);
            outputs.Add(Outputs.AlignStageLBlow1OnOff);
            outputs.Add(Outputs.AlignStageLBlow2OnOff);
            outputs.Add(Outputs.AlignStageLBlow3OnOff);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetTransferShutterRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.TransferInShuttleRVacOnOff);
            outputs.Add(Outputs.TransferInShuttleR0Degree);
            outputs.Add(Outputs.TransferInShuttleR180Degree);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetGlassAlignRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.AlignStageR1Align);
            outputs.Add(Outputs.AlignStageR2Align);
            outputs.Add(Outputs.AlignStageR3Align);
            outputs.Add(Outputs.AlignStageRBrushCylUp);
            outputs.Add(Outputs.AlignStageRVac1OnOff);
            outputs.Add(Outputs.AlignStageRVac2OnOff);
            outputs.Add(Outputs.AlignStageRVac3OnOff);
            outputs.Add(Outputs.AlignStageRBlow1OnOff);
            outputs.Add(Outputs.AlignStageRBlow2OnOff);
            outputs.Add(Outputs.AlignStageRBlow3OnOff);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetTransferRotationLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.TrRotateLeftVac1OnOff);
            outputs.Add(Outputs.TrRotateLeftVac2OnOff);
            outputs.Add(Outputs.TrRotateLeftRotVacOnOff);
            outputs.Add(Outputs.TrRotateLeft0Degree);
            outputs.Add(Outputs.TrRotateLeft180Degree);
            outputs.Add(Outputs.TrRotateLeftFw);
            outputs.Add(Outputs.TrRotateLeftBw);
            outputs.Add(Outputs.TrRotateLeftUp);
            outputs.Add(Outputs.TrRotateLeftDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetTransferRotationRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.TrRotateRightVac1OnOff);
            outputs.Add(Outputs.TrRotateRightVac2OnOff);
            outputs.Add(Outputs.TrRotateRightRotVacOnOff);
            outputs.Add(Outputs.TrRotateRight0Degree);
            outputs.Add(Outputs.TrRotateRight180Degree);
            outputs.Add(Outputs.TrRotateRightFw);
            outputs.Add(Outputs.TrRotateRightBw);
            outputs.Add(Outputs.TrRotateRightUp);
            outputs.Add(Outputs.TrRotateRightDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetUnloadTransferLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.UnloadTransferLVacOnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac1OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac2OnOff);
            outputs.Add(Outputs.UnloadRobotCyl1Down);
            outputs.Add(Outputs.UnloadRobotCyl2Down);
            outputs.Add(Outputs.UnloadRobotCyl3Down);
            outputs.Add(Outputs.UnloadRobotCyl4Down);
            outputs.Add(Outputs.UnloadAlignCyl1Up);
            outputs.Add(Outputs.UnloadAlignCyl2Up);
            outputs.Add(Outputs.UnloadAlignCyl3Up);
            outputs.Add(Outputs.UnloadAlignCyl4Up);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetUnloadTransferRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.UnloadTransferRVacOnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac3OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac4OnOff);
            outputs.Add(Outputs.UnloadRobotCyl1Down);
            outputs.Add(Outputs.UnloadRobotCyl2Down);
            outputs.Add(Outputs.UnloadRobotCyl3Down);
            outputs.Add(Outputs.UnloadRobotCyl4Down);
            outputs.Add(Outputs.UnloadAlignCyl1Up);
            outputs.Add(Outputs.UnloadAlignCyl2Up);
            outputs.Add(Outputs.UnloadAlignCyl3Up);
            outputs.Add(Outputs.UnloadAlignCyl4Up);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetWETCleanLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.WetCleanPusherLeftUp);
            outputs.Add(Outputs.WetCleanPusherLeftDown);
            outputs.Add(Outputs.WetCleanBrushLeftDown);
            outputs.Add(Outputs.InShuttleLVacOnOff);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetWETCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.WetCleanPusherRightUp);
            outputs.Add(Outputs.WetCleanPusherRightDown);
            outputs.Add(Outputs.WetCleanBrushRightDown);
            outputs.Add(Outputs.InShuttleRVacOnOff);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetAFCleanLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.AfCleanPusherLeftUp);
            outputs.Add(Outputs.AfCleanPusherLeftDown);
            outputs.Add(Outputs.AfCleanBrushLeftDown);
            outputs.Add(Outputs.OutShuttleLVacOnOff);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetAFCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.AfCleanPusherRightUp);
            outputs.Add(Outputs.AfCleanPusherRightDown);
            outputs.Add(Outputs.AfCleanBrushRightDown);
            outputs.Add(Outputs.OutShuttleRVacOnOff);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetUnloadAlignOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.UnloadGlassAlignVac1OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac2OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac3OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac4OnOff);
            outputs.Add(Outputs.UnloadGlassAlignBlow1OnOff);
            outputs.Add(Outputs.UnloadGlassAlignBlow2OnOff);
            outputs.Add(Outputs.UnloadGlassAlignBlow3OnOff);
            outputs.Add(Outputs.UnloadGlassAlignBlow4OnOff);
            outputs.Add(Outputs.UnloadAlignCyl1Up);
            outputs.Add(Outputs.UnloadAlignCyl2Up);
            outputs.Add(Outputs.UnloadAlignCyl3Up);
            outputs.Add(Outputs.UnloadAlignCyl4Up);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetUnloadRobotOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.UnloadRobotVac1OnOff);
            outputs.Add(Outputs.UnloadRobotVac2OnOff);
            outputs.Add(Outputs.UnloadRobotVac3OnOff);
            outputs.Add(Outputs.UnloadRobotVac4OnOff);
            outputs.Add(Outputs.UnloadRobotCyl1Down);
            outputs.Add(Outputs.UnloadRobotCyl2Down);
            outputs.Add(Outputs.UnloadRobotCyl3Down);
            outputs.Add(Outputs.UnloadRobotCyl4Down);
            outputs.Add(Outputs.UnloadRobMoveEnable);
            outputs.Add(Outputs.UnloadRobDrivesOn);
            outputs.Add(Outputs.UnloadRobDrivesOff);
            outputs.Add(Outputs.UnloadRobConfMess);
            outputs.Add(Outputs.UnloadRobExtStart);
            return outputs;
        }
        #endregion

        #region GetMotions
        public ObservableCollection<IMotion> GetCSTLoadMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.InCassetteTAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetCSTUnloadMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.OutCassetteTAxis);
            return motions;
        }

        // Transfer Fixture Tab Motions
        public ObservableCollection<IMotion> GetTransferFixtureMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.FixtureTransferYAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetDetachMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.DetachGlassZAxis);
            motions.Add(Motions.ShuttleTransferZAxis);
            motions.Add(Motions.ShuttleTransferXAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetGlassTransferMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.GlassTransferYAxis);
            motions.Add(Motions.GlassTransferZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferShutterLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.TransferInShuttleLYAxis);
            motions.Add(Motions.TransferInShuttleLZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferShutterRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.TransferInShuttleRYAxis);
            motions.Add(Motions.TransferInShuttleRZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferRotationLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.TransferRotationLZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferRotationRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.TransferRotationRZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetUnloadTransferLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.GlassUnloadLYAxis);
            motions.Add(Motions.GlassUnloadLZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetUnloadTransferRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.GlassUnloadRYAxis);
            motions.Add(Motions.GlassUnloadRZAxis);
            return motions;
        }

        // Clean Tab Motions
        public ObservableCollection<IMotion> GetWETCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.InShuttleLXAxis);
            motions.Add(Motions.InShuttleLYAxis);
            motions.Add(Motions.InShuttleLTAxis);
            motions.Add(Motions.WETCleanLFeedingAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetWETCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.InShuttleRXAxis);
            motions.Add(Motions.InShuttleRYAxis);
            motions.Add(Motions.InShuttleRTAxis);
            motions.Add(Motions.WETCleanRFeedingAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetAFCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.OutShuttleLXAxis);
            motions.Add(Motions.OutShuttleLYAxis);
            motions.Add(Motions.OutShuttleLTAxis);
            motions.Add(Motions.AFCleanLFeedingAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetAFCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Motions.OutShuttleRXAxis);
            motions.Add(Motions.OutShuttleRYAxis);
            motions.Add(Motions.OutShuttleRTAxis);
            motions.Add(Motions.AFCleanRFeedingAxis);
            return motions;
        }
        #endregion

        #region GetRollers
        public ObservableCollection<BD201SRollerController> GetInConveyorRollers()
        {
            ObservableCollection<BD201SRollerController> rollers = new ObservableCollection<BD201SRollerController>();
            rollers.Add(RollerList.InConveyorRoller1);
            rollers.Add(RollerList.InConveyorRoller2);
            rollers.Add(RollerList.InConveyorRoller3);
            return rollers;
        }

        public ObservableCollection<BD201SRollerController> GetInWorkConveyorRollers()
        {
            ObservableCollection<BD201SRollerController> rollers = new ObservableCollection<BD201SRollerController>();
            rollers.Add(RollerList.SupportConveyorRoller1);
            rollers.Add(RollerList.InWorkConveyorRoller1);
            rollers.Add(RollerList.InWorkConveyorRoller2);
            rollers.Add(RollerList.SupportConveyorRoller2);
            return rollers;
        }

        public ObservableCollection<BD201SRollerController> GetBufferConveyorRollers()
        {
            ObservableCollection<BD201SRollerController> rollers = new ObservableCollection<BD201SRollerController>();
            rollers.Add(RollerList.BufferConveyorRoller1);
            rollers.Add(RollerList.BufferConveyorRoller2);
            return rollers;
        }

        public ObservableCollection<BD201SRollerController> GetOutWorkConveyorRollers()
        {
            ObservableCollection<BD201SRollerController> rollers = new ObservableCollection<BD201SRollerController>();
            rollers.Add(RollerList.SupportConveyorRoller3);
            rollers.Add(RollerList.OutWorkConveyorRoller1);
            rollers.Add(RollerList.OutWorkConveyorRoller2);
            rollers.Add(RollerList.SupportConveyorRoller4);
            return rollers;
        }

        public ObservableCollection<BD201SRollerController> GetOutConveyorRollers()
        {
            ObservableCollection<BD201SRollerController> rollers = new ObservableCollection<BD201SRollerController>();
            rollers.Add(RollerList.OutConveyorRoller1);
            rollers.Add(RollerList.OutConveyorRoller2);
            return rollers;
        }
        #endregion

        #endregion
    }
}
