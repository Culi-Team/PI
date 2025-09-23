using EQX.Core.InOut;
using EQX.Core.Motion;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
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
            AnalogInputs analogInputs)
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

        #region Public Methods

        #region Get Cylinders
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

        public ObservableCollection<ICylinder> GetTransferFixtureCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();

            cylinders.Add(Cylinders.TransferFixtureUpDown);
            cylinders.Add(Cylinders.TransferFixture1ClampUnclamp);
            cylinders.Add(Cylinders.TransferFixture2ClampUnclamp);

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

        public ObservableCollection<ICylinder> GetTransferInShuttleRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            cylinders.Add(Cylinders.TransferInShuttleRRotate);
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
        #endregion

        #region Get Inputs
        // CSTLoadUnload Tab Inputs
        public ObservableCollection<IDInput> GetInWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add In Cassette detection inputs (essential for teaching)
            inputs.Add(Inputs.InCstDetect1);
            inputs.Add(Inputs.InCstDetect2);
            // Add In CV Support detection inputs (essential for teaching)
            inputs.Add(Inputs.InCvSupportUp);
            inputs.Add(Inputs.InCvSupportDown);
            inputs.Add(Inputs.InCvSupportBufferUp);
            inputs.Add(Inputs.InCvSupportBufferDown);
            // Add In CST Fix cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.InCstFixCyl1Fw);
            inputs.Add(Inputs.InCstFixCyl1Bw);
            inputs.Add(Inputs.InCstFixCyl2Fw);
            inputs.Add(Inputs.InCstFixCyl2Bw);
            // Add In CST Tilt cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.InCstTiltCylUp);
            inputs.Add(Inputs.InCstTiltCylDown);
            // Add In CST Stopper detection inputs (essential for teaching)
            inputs.Add(Inputs.InCstStopperUp);
            inputs.Add(Inputs.InCstStopperDown);

            return inputs;
        }

        public ObservableCollection<IDInput> GetOutWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Out Cassette detection inputs (essential for teaching)
            inputs.Add(Inputs.OutCstDetect1);
            inputs.Add(Inputs.OutCstDetect2);
            // Add Out CV Support detection inputs (essential for teaching)
            inputs.Add(Inputs.OutCvSupportUp);
            inputs.Add(Inputs.OutCvSupportDown);
            inputs.Add(Inputs.OutCvSupportBufferUp);
            inputs.Add(Inputs.OutCvSupportBufferDown);
            // Add Out CST Fix cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.OutCstFixCyl1Fw);
            inputs.Add(Inputs.OutCstFixCyl1Bw);
            inputs.Add(Inputs.OutCstFixCyl2Fw);
            inputs.Add(Inputs.OutCstFixCyl2Bw);
            // Add Out CST Tilt cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.OutCstTiltCylUp);
            inputs.Add(Inputs.OutCstTiltCylDown);
            // Add Out CST Stopper detection inputs (essential for teaching)
            inputs.Add(Inputs.OutCstStopperUp);
            inputs.Add(Inputs.OutCstStopperDown);

            return inputs;
        }

        // Detach Tab Inputs
        public ObservableCollection<IDInput> GetTransferFixtureInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Transfer Fixture detection inputs
            inputs.Add(Inputs.TransferFixtureUp);
            inputs.Add(Inputs.TransferFixtureDown);
            // Add Transfer Fixture clamp detection inputs
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

        public ObservableCollection<IDInput> GetDetachInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Detach fixture detection input
            inputs.Add(Inputs.DetachFixtureDetect);
            // Add Detach glass shuttle vacuum inputs
            inputs.Add(Inputs.DetachGlassShtVac1);
            inputs.Add(Inputs.DetachGlassShtVac2);
            inputs.Add(Inputs.DetachGlassShtVac3);
            // Add Detach cylinder detection inputs
            inputs.Add(Inputs.DetachCyl1Up);
            inputs.Add(Inputs.DetachCyl1Down);
            inputs.Add(Inputs.DetachCyl2Up);
            inputs.Add(Inputs.DetachCyl2Down);
            // Add Detach fix fixture cylinder detection inputs
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

        // Transfer Shutter Tab Inputs
        public ObservableCollection<IDInput> GetTransferShutterLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass detection inputs
            inputs.Add(Inputs.AlignStageLGlassDettect1);
            inputs.Add(Inputs.AlignStageLGlassDettect2);
            inputs.Add(Inputs.AlignStageLGlassDettect3);
            // Add Vacuum detection inputs
            inputs.Add(Inputs.TransferInShuttleLVac);
            // Add Transfer In Shuttle Left rotate detection inputs
            inputs.Add(Inputs.TransferInShuttleL0Degree);
            inputs.Add(Inputs.TransferInShuttleL180Degree);
            return inputs;
        }

        public ObservableCollection<IDInput> GetTransferShutterRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass detection inputs
            inputs.Add(Inputs.AlignStageRGlassDetect1);
            inputs.Add(Inputs.AlignStageRGlassDetect2);
            inputs.Add(Inputs.AlignStageRGlassDetect3);
            // Add Vacuum detection inputs
            inputs.Add(Inputs.TransferInShuttleRVac);
            // Add Transfer In Shuttle Right rotate detection inputs
            inputs.Add(Inputs.TransferInShuttleR0Degree);
            inputs.Add(Inputs.TransferInShuttleR180Degree);
            return inputs;
        }

        // Transfer Rotation Tab Inputs
        public ObservableCollection<IDInput> GetTransferRotationLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Vacuum detection inputs
            inputs.Add(Inputs.TrRotateLeftVac1);
            inputs.Add(Inputs.TrRotateLeftVac2);
            inputs.Add(Inputs.TrRotateLeftRotVac);
            // Add Transfer Rotation Left cylinder detection inputs
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
            // Add Vacuum detection inputs
            inputs.Add(Inputs.TrRotateRightVac1);
            inputs.Add(Inputs.TrRotateRightVac2);
            inputs.Add(Inputs.TrRotateRightRotVac);
            // Add Transfer Rotation Right cylinder detection inputs
            inputs.Add(Inputs.TrRotateRight0Degree);
            inputs.Add(Inputs.TrRotateRight180Degree);
            inputs.Add(Inputs.TrRotateRightFw);
            inputs.Add(Inputs.TrRotateRightBw);
            inputs.Add(Inputs.TrRotateRightUp);
            inputs.Add(Inputs.TrRotateRightDown);
            return inputs;
        }

        // Unload Transfer Tab Inputs
        public ObservableCollection<IDInput> GetUnloadTransferLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Unload Transfer Left vacuum detection inputs
            inputs.Add(Inputs.UnloadTransferLVac);
            // Add Unload Align vacuum detection inputs
            inputs.Add(Inputs.UnloadGlassAlignVac1);
            inputs.Add(Inputs.UnloadGlassAlignVac2);
            // Add Unload Robot cylinder detection inputs
            inputs.Add(Inputs.UnloadRobotCyl1Up);
            inputs.Add(Inputs.UnloadRobotCyl1Down);
            inputs.Add(Inputs.UnloadRobotCyl2Up);
            inputs.Add(Inputs.UnloadRobotCyl2Down);
            inputs.Add(Inputs.UnloadRobotCyl3Up);
            inputs.Add(Inputs.UnloadRobotCyl3Down);
            inputs.Add(Inputs.UnloadRobotCyl4Up);
            inputs.Add(Inputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder detection inputs
            inputs.Add(Inputs.UnloadAlignCyl1Up);
            inputs.Add(Inputs.UnloadAlignCyl1Down);
            inputs.Add(Inputs.UnloadAlignCyl2Up);
            inputs.Add(Inputs.UnloadAlignCyl2Down);
            return inputs;
        }

        public ObservableCollection<IDInput> GetUnloadTransferRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Unload Transfer Right vacuum detection inputs
            inputs.Add(Inputs.UnloadTransferRVac);
            // Add Unload Align vacuum detection inputs
            inputs.Add(Inputs.UnloadGlassAlignVac3);
            inputs.Add(Inputs.UnloadGlassAlignVac4);
            // Add Unload Robot cylinder detection inputs
            inputs.Add(Inputs.UnloadRobotCyl1Up);
            inputs.Add(Inputs.UnloadRobotCyl1Down);
            inputs.Add(Inputs.UnloadRobotCyl2Up);
            inputs.Add(Inputs.UnloadRobotCyl2Down);
            inputs.Add(Inputs.UnloadRobotCyl3Up);
            inputs.Add(Inputs.UnloadRobotCyl3Down);
            inputs.Add(Inputs.UnloadRobotCyl4Up);
            inputs.Add(Inputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder detection inputs
            inputs.Add(Inputs.UnloadAlignCyl3Up);
            inputs.Add(Inputs.UnloadAlignCyl3Down);
            inputs.Add(Inputs.UnloadAlignCyl4Up);
            inputs.Add(Inputs.UnloadAlignCyl4Down);
            return inputs;
        }

        // Clean Tab Inputs
        public ObservableCollection<IDInput> GetWETCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add WET Clean Left pusher cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.WetCleanPusherLeftUp);
            inputs.Add(Inputs.WetCleanPusherLeftDown);
            // Add WET Clean Left brush cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.WetCleanBrushLeftUp);
            inputs.Add(Inputs.WetCleanBrushLeftDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetWETCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add WET Clean Right pusher cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.WetCleanPusherRightUp);
            inputs.Add(Inputs.WetCleanPusherRightDown);
            // Add WET Clean Right brush cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.WetCleanBrushRightUp);
            inputs.Add(Inputs.WetCleanBrushRightDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetAFCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add AF Clean Left pusher cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.AfCleanPusherLeftUp);
            inputs.Add(Inputs.AfCleanPusherLeftDown);
            // Add AF Clean Left brush cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.AfCleanBrushLeftUp);
            inputs.Add(Inputs.AfCleanBrushLeftDown);
            return inputs;
        }

        public ObservableCollection<IDInput> GetAFCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add AF Clean Right pusher cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.AfCleanPusherRightUp);
            inputs.Add(Inputs.AfCleanPusherRightDown);
            // Add AF Clean Right brush cylinder detection inputs (essential for teaching)
            inputs.Add(Inputs.AfCleanBrushRightUp);
            inputs.Add(Inputs.AfCleanBrushRightDown);
            return inputs;
        }

        // Glass Transfer Tab Inputs
        public ObservableCollection<IDInput> GetGlassTransferInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass Transfer vacuum detection inputs
            inputs.Add(Inputs.GlassTransferVac1);
            inputs.Add(Inputs.GlassTransferVac2);
            inputs.Add(Inputs.GlassTransferVac3);
            // Add Glass Transfer cylinder detection inputs
            inputs.Add(Inputs.GlassTransferCyl1Up);
            inputs.Add(Inputs.GlassTransferCyl1Down);
            inputs.Add(Inputs.GlassTransferCyl2Up);
            inputs.Add(Inputs.GlassTransferCyl2Down);
            inputs.Add(Inputs.GlassTransferCyl3Up);
            inputs.Add(Inputs.GlassTransferCyl3Down);
            return inputs;
        }
        #endregion

        #region GetOutputs
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

        public ObservableCollection<IDOutput> GetTransferShutterRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            outputs.Add(Outputs.TransferInShuttleRVacOnOff);
            outputs.Add(Outputs.TransferInShuttleR0Degree);
            outputs.Add(Outputs.TransferInShuttleR180Degree);
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

        #endregion
    }
}
