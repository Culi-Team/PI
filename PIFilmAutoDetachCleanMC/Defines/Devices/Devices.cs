using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Motion;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
using PIFilmAutoDetachCleanMC.Process;
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
            MotionsInovance motionsInovance,
            MotionsAjin motionsAjin,
            Cylinders cylinders,
            TorqueControllerList torqueControllers,
            SpeedControllerList speedControllerList,
            Regulators regulators,
            AnalogInputs analogInputs,
            MachineStatus machineStatus)
        {
            Inputs = inputs;
            Outputs = outputs;
            MotionsInovance = motionsInovance;
            MotionsAjin = motionsAjin;
            Cylinders = cylinders;
            TorqueControllers = torqueControllers;
            SpeedControllerList = speedControllerList;
            Regulators = regulators;
            AnalogInputs = analogInputs;
            MachineStatus = machineStatus;
        }

        public Inputs Inputs { get; }
        public Outputs Outputs { get; }
        public MotionsInovance MotionsInovance { get; }
        public MotionsAjin MotionsAjin { get; }
        public Cylinders Cylinders { get; }
        public TorqueControllerList TorqueControllers { get; }
        public SpeedControllerList SpeedControllerList { get; }
        public Regulators Regulators { get; }
        public AnalogInputs AnalogInputs { get; }
        public MachineStatus MachineStatus { get; }

        #region Public Methods

        #region Get Cylinders
        public ObservableCollection<ICylinder> GetInConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.InCstStopperUpDown);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetInWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.InCstStopperUpDown);
            cylinders.Add(Cylinders.InCstFixCyl1FwBw);
            cylinders.Add(Cylinders.InCstFixCyl2FwBw);
            cylinders.Add(Cylinders.InCstTiltCylUpDown);
            cylinders.Add(Cylinders.InCvSupportUpDown);
            cylinders.Add(Cylinders.InCvSupportBufferUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetBufferConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.BufferCvStopper1UpDown);
            cylinders.Add(Cylinders.BufferCvStopper2UpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetOutWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.OutCstStopperUpDown);
            cylinders.Add(Cylinders.OutCstFixCyl1FwBw);
            cylinders.Add(Cylinders.OutCstFixCyl2FwBw);
            cylinders.Add(Cylinders.OutCstTiltCylUpDown);
            cylinders.Add(Cylinders.OutCvSupportUpDown);
            cylinders.Add(Cylinders.OutCvSupportBufferUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetOutConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.OutCstStopperUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetVinylCleanCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.VinylCleanRollerFwBw);
            cylinders.Add(Cylinders.VinylCleanPusherRollerUpDown);
            cylinders.Add(Cylinders.VinylCleanFixtureClampUnclamp);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferFixtureCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.TransferFixtureUpDown);
            cylinders.Add(Cylinders.TransferFixture1ClampUnclamp);
            cylinders.Add(Cylinders.TransferFixture2ClampUnclamp);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetFixtureAlignCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AlignFixtureCyl1FwBw);
            cylinders.Add(Cylinders.AlignFixtureCyl2FwBw);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetRemoveFilmCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.RemoveZoneFixCyl1FwBw);
            cylinders.Add(Cylinders.RemoveZoneFixCyl2FwBw);
            cylinders.Add(Cylinders.RemoveZoneCyl1ClampUnclamp);  
            cylinders.Add(Cylinders.RemoveZoneCyl2ClampUnclamp);  
            cylinders.Add(Cylinders.RemoveZoneCyl3ClampUnclamp);
            cylinders.Add(Cylinders.RemoveZonePusherCyl1UpDown);
            cylinders.Add(Cylinders.RemoveZonePusherCyl2UpDown);
            cylinders.Add(Cylinders.RemoveZoneTrCylFwBw);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetRobotLoadCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.RobotFixtureAlignFwBw);
            cylinders.Add(Cylinders.RobotFixtureClampUnclamp);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetDetachCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.DetachFixFixtureCyl1FwBw);
            cylinders.Add(Cylinders.DetachFixFixtureCyl2FwBw);

            cylinders.Add(Cylinders.DetachCyl1UpDown);
            cylinders.Add(Cylinders.DetachCyl2UpDown);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetGlassTransferCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.GlassTransferCyl1UpDown);
            cylinders.Add(Cylinders.GlassTransferCyl2UpDown);
            cylinders.Add(Cylinders.GlassTransferCyl3UpDown);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferInShuttleLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TransferInShuttleLRotate);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetGlassAlignLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AlignStageL1AlignUnalign);
            cylinders.Add(Cylinders.AlignStageL2AlignUnalign);
            cylinders.Add(Cylinders.AlignStageL3AlignUnalign);
            cylinders.Add(Cylinders.AlignStageLBrushCylUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferInShuttleRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TransferInShuttleRRotate);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetGlassAlignRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AlignStageR1AlignUnalign);
            cylinders.Add(Cylinders.AlignStageR2AlignUnalign);
            cylinders.Add(Cylinders.AlignStageR3AlignUnalign);
            cylinders.Add(Cylinders.AlignStageRBrushCylUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferRotationLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TrRotateLeftRotate);
            cylinders.Add(Cylinders.TrRotateLeftFwBw);
            cylinders.Add(Cylinders.TrRotateLeftUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetTransferRotationRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TrRotateRightRotate);
            cylinders.Add(Cylinders.TrRotateRightFwBw);
            cylinders.Add(Cylinders.TrRotateRightUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadTransferLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl4UpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadTransferRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl4UpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetWETCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.WetCleanPusherLeftUpDown);
            cylinders.Add(Cylinders.WetCleanBrushLeftUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetWETCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.WetCleanPusherRightUpDown);
            cylinders.Add(Cylinders.WetCleanBrushRightUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetAFCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AFCleanPusherLeftUpDown);
            cylinders.Add(Cylinders.AFCleanBrushLeftUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetAFCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.AFCleanPusherRightUpDown);
            cylinders.Add(Cylinders.AFCleanBrushRightUpDown);
            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadAlignCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Cylinders.UnloadAlignCyl4UpDown);

            return cylinders;
        }

        public ObservableCollection<ICylinder> GetUnloadRobotCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.UnloadRobotCyl1UpDown);
            cylinders.Add(Cylinders.UnloadRobotCyl2UpDown);
            cylinders.Add(Cylinders.UnloadRobotCyl3UpDown);
            cylinders.Add(Cylinders.UnloadRobotCyl4UpDown);

            return cylinders;
        }
        #endregion

        #region Get Inputs
        public ObservableCollection<IDInput> GetInConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.InCstDetect1);
            inputs.Add(Inputs.InCstDetect2);
            inputs.Add(Inputs.InCstStopperUp);
            inputs.Add(Inputs.InCstStopperDown);

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
            inputs.Add(Inputs.InCstFixCyl1Fw);
            inputs.Add(Inputs.InCstFixCyl1Bw);
            inputs.Add(Inputs.InCstFixCyl2Fw);
            inputs.Add(Inputs.InCstFixCyl2Bw);
            inputs.Add(Inputs.InCstTiltCylUp);
            inputs.Add(Inputs.InCstTiltCylDown);
            inputs.Add(Inputs.InCstStopperUp);
            inputs.Add(Inputs.InCstStopperDown);

            return inputs;
        }

        public ObservableCollection<IDInput> GetBufferConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.BufferCstDetect1);
            inputs.Add(Inputs.BufferCstDetect2);
            inputs.Add(Inputs.BufferCvStopper1Up);
            inputs.Add(Inputs.BufferCvStopper1Down);
            inputs.Add(Inputs.BufferCvStopper2Up);
            inputs.Add(Inputs.BufferCvStopper2Down);
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
            inputs.Add(Inputs.OutCstFixCyl1Fw);
            inputs.Add(Inputs.OutCstFixCyl1Bw);
            inputs.Add(Inputs.OutCstFixCyl2Fw);
            inputs.Add(Inputs.OutCstFixCyl2Bw);
            inputs.Add(Inputs.OutCstTiltCylUp);
            inputs.Add(Inputs.OutCstTiltCylDown);
            inputs.Add(Inputs.OutCstStopperUp);
            inputs.Add(Inputs.OutCstStopperDown);

            return inputs;
        }

        public ObservableCollection<IDInput> GetOutConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.OutCstDetect1);
            inputs.Add(Inputs.OutCstDetect2);
            inputs.Add(Inputs.OutCstStopperUp);
            inputs.Add(Inputs.OutCstStopperDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetVinylCleanInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.VinylCleanFixtureDetect);
            inputs.Add(Inputs.VinylCleanRunoffDetect);
            inputs.Add(Inputs.VinylCleanFullDetect);
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
            inputs.Add(Inputs.RobotFixture1Clamp);
            inputs.Add(Inputs.RobotFixture1Unclamp);
            inputs.Add(Inputs.RobotFixture2Clamp);
            inputs.Add(Inputs.RobotFixture2Unclamp);
            inputs.Add(Inputs.RobotFixtureAlign1Fw);
            inputs.Add(Inputs.RobotFixtureAlign1Bw);
            inputs.Add(Inputs.RobotFixtureAlign2Fw);
            inputs.Add(Inputs.RobotFixtureAlign2Bw);

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
            inputs.Add(Inputs.RemoveZoneFixCyl11Fw);
            inputs.Add(Inputs.RemoveZoneFixCyl11Bw);
            inputs.Add(Inputs.RemoveZoneFixCyl12Fw);
            inputs.Add(Inputs.RemoveZoneFixCyl12Bw);
            inputs.Add(Inputs.RemoveZoneFixCyl21Fw);
            inputs.Add(Inputs.RemoveZoneFixCyl21Bw);
            inputs.Add(Inputs.RemoveZoneFixCyl22Fw);
            inputs.Add(Inputs.RemoveZoneFixCyl22Bw);
            inputs.Add(Inputs.RemoveZoneCyl1Clamp);
            inputs.Add(Inputs.RemoveZoneCyl1Unclamp);
            inputs.Add(Inputs.RemoveZoneCyl2Clamp);
            inputs.Add(Inputs.RemoveZoneCyl2Unclamp);
            inputs.Add(Inputs.RemoveZoneCyl3Clamp);
            inputs.Add(Inputs.RemoveZoneCyl3Unclamp);
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
            inputs.Add(Inputs.DetachFixFixtureCyl11Fw);
            inputs.Add(Inputs.DetachFixFixtureCyl11Bw);
            inputs.Add(Inputs.DetachFixFixtureCyl12Fw);
            inputs.Add(Inputs.DetachFixFixtureCyl12Bw);
            inputs.Add(Inputs.DetachFixFixtureCyl21Fw);
            inputs.Add(Inputs.DetachFixFixtureCyl21Bw);
            inputs.Add(Inputs.DetachFixFixtureCyl22Fw);
            inputs.Add(Inputs.DetachFixFixtureCyl22Bw);

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
            return inputs;
        }

        public ObservableCollection<IDInput> GetWETCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.WetCleanPusherRightUp);
            inputs.Add(Inputs.WetCleanPusherRightDown);
            inputs.Add(Inputs.WetCleanBrushRightUp);
            inputs.Add(Inputs.WetCleanBrushRightDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetAFCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AfCleanPusherLeftUp);
            inputs.Add(Inputs.AfCleanPusherLeftDown);
            inputs.Add(Inputs.AfCleanBrushLeftUp);
            inputs.Add(Inputs.AfCleanBrushLeftDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetAFCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            inputs.Add(Inputs.AfCleanPusherRightUp);
            inputs.Add(Inputs.AfCleanPusherRightDown);
            inputs.Add(Inputs.AfCleanBrushRightUp);
            inputs.Add(Inputs.AfCleanBrushRightDown);
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

            return inputs;
        }
        #endregion

        #region GetOutputs
        public ObservableCollection<IDOutput> GetInConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.InCstStopperUp);
            outputs.Add(Outputs.InCstStopperDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetInWorkConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.InCstStopperUp);
            outputs.Add(Outputs.InCstStopperDown);
            outputs.Add(Outputs.InCstFixCyl1Fw);
            outputs.Add(Outputs.InCstFixCyl1Bw);
            outputs.Add(Outputs.InCstFixCyl2Fw);
            outputs.Add(Outputs.InCstFixCyl2Bw);
            outputs.Add(Outputs.InCstTiltCylUp);
            outputs.Add(Outputs.InCstTiltCylDown);
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
            outputs.Add(Outputs.OutCstStopperUp);
            outputs.Add(Outputs.OutCstStopperDown);
            outputs.Add(Outputs.OutCstFixCyl1Fw);
            outputs.Add(Outputs.OutCstFixCyl1Bw);
            outputs.Add(Outputs.OutCstFixCyl2Fw);
            outputs.Add(Outputs.OutCstFixCyl2Bw);
            outputs.Add(Outputs.OutCstTiltCylUp);
            outputs.Add(Outputs.OutCstTiltCylDown);
            outputs.Add(Outputs.OutCvSupportUp);
            outputs.Add(Outputs.OutCvSupportDown);
            outputs.Add(Outputs.OutCvSupportBufferUp);
            outputs.Add(Outputs.OutCvSupportBufferDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetOutConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.OutCstStopperUp);
            outputs.Add(Outputs.OutCstStopperDown);
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
            outputs.Add(Outputs.RemoveZoneFixCyl1Fw);
            outputs.Add(Outputs.RemoveZoneFixCyl1Bw);
            outputs.Add(Outputs.RemoveZoneFixCyl2Fw);
            outputs.Add(Outputs.RemoveZoneFixCyl2Bw);
            outputs.Add(Outputs.RemoveZoneCyl1Clamp);
            outputs.Add(Outputs.RemoveZoneCyl1Unclamp);
            outputs.Add(Outputs.RemoveZoneCyl2Clamp);
            outputs.Add(Outputs.RemoveZoneCyl2Unclamp);
            outputs.Add(Outputs.RemoveZoneCyl3Clamp);
            outputs.Add(Outputs.RemoveZoneCyl3Unclamp);
            outputs.Add(Outputs.RemoveZonePusherCyl1Up);
            outputs.Add(Outputs.RemoveZonePusherCyl1Down);
            outputs.Add(Outputs.RemoveZonePusherCyl2Up);
            outputs.Add(Outputs.RemoveZonePusherCyl2Down);
            outputs.Add(Outputs.RemoveZoneTrCylFw);
            outputs.Add(Outputs.RemoveZoneTrCylBw);

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
            outputs.Add(Outputs.DetachFixFixtureCyl1Fw);
            outputs.Add(Outputs.DetachFixFixtureCyl1Bw);
            outputs.Add(Outputs.DetachFixFixtureCyl2Fw);
            outputs.Add(Outputs.DetachFixFixtureCyl2Bw);
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
            return outputs;
        }

        public ObservableCollection<IDOutput> GetWETCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.WetCleanPusherRightUp);
            outputs.Add(Outputs.WetCleanPusherRightDown);
            outputs.Add(Outputs.WetCleanBrushRightDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetAFCleanLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.AfCleanPusherLeftUp);
            outputs.Add(Outputs.AfCleanPusherLeftDown);
            outputs.Add(Outputs.AfCleanBrushLeftDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetAFCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.AfCleanPusherRightUp);
            outputs.Add(Outputs.AfCleanPusherRightDown);
            outputs.Add(Outputs.AfCleanBrushRightDown);
            return outputs;
        }

        public ObservableCollection<IDOutput> GetUnloadAlignOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.UnloadGlassAlignVac1OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac2OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac3OnOff);
            outputs.Add(Outputs.UnloadGlassAlignVac4OnOff);
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
            return outputs;
        }
        #endregion

        #region GetMotions
        public ObservableCollection<IMotion> GetCSTLoadMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.InCassetteTAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetCSTUnloadMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.OutCassetteTAxis);
            return motions;
        }

        // Transfer Fixture Tab Motions
        public ObservableCollection<IMotion> GetTransferFixtureMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.FixtureTransferYAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetDetachMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.DetachGlassZAxis);
            motions.Add(MotionsAjin.ShuttleTransferZAxis);
            motions.Add(MotionsInovance.ShuttleTransferXAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetGlassTransferMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.GlassTransferYAxis);
            motions.Add(MotionsInovance.GlassTransferZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferShutterLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.TransferInShuttleLYAxis);
            motions.Add(MotionsInovance.TransferInShuttleLZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferShutterRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.TransferInShuttleRYAxis);
            motions.Add(MotionsInovance.TransferInShuttleRZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferRotationLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.TransferRotationLZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetTransferRotationRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.TransferRotationRZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetUnloadTransferLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.GlassUnloadLYAxis);
            motions.Add(MotionsInovance.GlassUnloadLZAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetUnloadTransferRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsInovance.GlassUnloadRYAxis);
            motions.Add(MotionsInovance.GlassUnloadRZAxis);
            return motions;
        }

        // Clean Tab Motions
        public ObservableCollection<IMotion> GetWETCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsAjin.InShuttleLXAxis);
            motions.Add(MotionsAjin.InShuttleLYAxis);
            motions.Add(MotionsInovance.InShuttleLTAxis);
            motions.Add(MotionsInovance.WETCleanLFeedingAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetWETCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsAjin.InShuttleRXAxis);
            motions.Add(MotionsAjin.InShuttleRYAxis);
            motions.Add(MotionsInovance.InShuttleRTAxis);
            motions.Add(MotionsInovance.WETCleanRFeedingAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetAFCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsAjin.OutShuttleLXAxis);
            motions.Add(MotionsAjin.OutShuttleLYAxis);
            motions.Add(MotionsInovance.OutShuttleLTAxis);
            motions.Add(MotionsInovance.AFCleanLFeedingAxis);
            return motions;
        }

        public ObservableCollection<IMotion> GetAFCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(MotionsAjin.OutShuttleRXAxis);
            motions.Add(MotionsAjin.OutShuttleRYAxis);
            motions.Add(MotionsInovance.OutShuttleRTAxis);
            motions.Add(MotionsInovance.AFCleanRFeedingAxis);
            return motions;
        }
        #endregion

        #region GetRollers
        public ObservableCollection<ISpeedController> GetInConveyorRollers()
        {
            ObservableCollection<ISpeedController> rollers = new ObservableCollection<ISpeedController>();
            rollers.Add(SpeedControllerList.InConveyorRoller1);
            rollers.Add(SpeedControllerList.InConveyorRoller2);
            rollers.Add(SpeedControllerList.InConveyorRoller3);
            return rollers;
        }

        public ObservableCollection<ISpeedController> GetInWorkConveyorRollers()
        {
            ObservableCollection<ISpeedController> rollers = new ObservableCollection<ISpeedController>();
            rollers.Add(SpeedControllerList.InWorkConveyorRoller1);
            rollers.Add(SpeedControllerList.InWorkConveyorRoller2);
            return rollers;
        }

        public ObservableCollection<ISpeedController> GetBufferConveyorRollers()
        {
            ObservableCollection<ISpeedController> rollers = new ObservableCollection<ISpeedController>();
            rollers.Add(SpeedControllerList.BufferConveyorRoller1);
            rollers.Add(SpeedControllerList.BufferConveyorRoller2);
            return rollers;
        }

        public ObservableCollection<ISpeedController> GetOutWorkConveyorRollers()
        {
            ObservableCollection<ISpeedController> rollers = new ObservableCollection<ISpeedController>();
            rollers.Add(SpeedControllerList.OutWorkConveyorRoller1);
            rollers.Add(SpeedControllerList.OutWorkConveyorRoller2);
            return rollers;
        }

        public ObservableCollection<ISpeedController> GetOutConveyorRollers()
        {
            ObservableCollection<ISpeedController> rollers = new ObservableCollection<ISpeedController>();
            rollers.Add(SpeedControllerList.OutConveyorRoller1);
            rollers.Add(SpeedControllerList.OutConveyorRoller2);
            return rollers;
        }
        #endregion

        #endregion
    }
}
