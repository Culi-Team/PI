using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class FixtureAlignProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly MachineStatus _machineStatus;
        private readonly IDInputDevice _fixtureAlignInput;
        private readonly IDOutputDevice _fixtureAlignOutput;

        private ICylinder AlignFixtureCyl => _devices.Cylinders.AlignFixtureCylFwBw;
        private bool IsFixtureDetect => _devices.Inputs.AlignFixtureDetect.Value;
        private bool IsFixtureTiltDetect => _devices.Inputs.AlignFixtureTiltDetect.Value;
        private bool IsFixtureReverseDetect => _devices.Inputs.AlignFixtureReverseDetect.Value;
        #endregion

        #region Flags
        private bool FlagFixtureAlignReqLoad
        {
            set
            {
                _fixtureAlignOutput[(int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_REQ_LOAD] = value;
            }
        }

        private bool FlagFixtureAlignLoadDone
        {
            get
            {
                return _fixtureAlignInput[(int)EFixtureAlignProcessInput.FIXTURE_ALIGN_LOAD_DONE];
            }
        }

        private bool FlagFixtureAlignDone
        {
            set
            {
                _fixtureAlignOutput[(int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_DONE] = value;
            }
        }

        private bool FlagFixtureTransferDone
        {
            get
            {
                return _fixtureAlignInput[(int)EFixtureAlignProcessInput.FIXTURE_ALIGN_TRANSFER_DONE];
            }
        }
        #endregion

        #region Constructor
        public FixtureAlignProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("FixtureAlignInput")] IDInputDevice fixtureAlignInput,
            [FromKeyedServices("FixtureAlignOutput")] IDOutputDevice fixtureAlignOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _fixtureAlignInput = fixtureAlignInput;
            _fixtureAlignOutput = fixtureAlignOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EFixtureAlignOriginStep)Step.OriginStep)
            {
                case EFixtureAlignOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EFixtureAlignOriginStep.AlignFixtureBackward:
                    Log.Debug("Align Fixture Backward");
                    AlignFixtureCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return AlignFixtureCyl.IsBackward; });
                    Step.OriginStep++;
                    break;
                case EFixtureAlignOriginStep.AlignFixtureBackward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Align Fixture Backward Done");
                    Step.OriginStep++;
                    break;
                case EFixtureAlignOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
                default:
                    Wait(20);
                    break;
            }

            return true;
        }

        public override bool ProcessRun()
        {
            switch (Sequence)
            {
                case ESequence.Stop:
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    break;
                case ESequence.RobotPlaceFixtureToAlign:
                    Sequence_RobotPlaceFixtureToAlign();
                    break;
                case ESequence.FixtureAlign:
                    Sequence_FixtureAlign();
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    break;
                case ESequence.TransferFixtureLoad:
                    Sequence_TransferFixtureLoad();
                    break;
                case ESequence.Detach:
                    break;
                case ESequence.TransferFixtureUnload:
                    break;
                case ESequence.DetachUnload:
                    break;
                case ESequence.RemoveFilm:
                    break;
                case ESequence.GlassTransferPick:
                    break;
                case ESequence.GlassTransferPlace:
                    break;
                case ESequence.AlignGlass:
                    break;
                case ESequence.TransferInShuttlePick:
                    break;
                case ESequence.TransferInShuttlePlace:
                    break;
                case ESequence.WETCleanLoad:
                    break;
                case ESequence.WETClean:
                    break;
                case ESequence.WETCleanUnload:
                    break;
                case ESequence.TransferRotationPick:
                    break;
                case ESequence.TransferRotationPlace:
                    break;
                case ESequence.AFCleanLoad:
                    break;
                case ESequence.AFClean:
                    break;
                case ESequence.AFCleanUnload:
                    break;
                case ESequence.UnloadTransferPick:
                    break;
                case ESequence.UnloadTransferPlace:
                    break;
                case ESequence.UnloadAlignGlass:
                    break;
                case ESequence.UnloadRobotPick:
                    break;
                case ESequence.UnloadRobotPlasma:
                    break;
                case ESequence.UnloadRobotPlace:
                    break;

                default:
                    Thread.Sleep(20);
                    break;
            }
            return true;
        }

        #endregion

        #region Private Methods
        private void Sequence_AutoRun()
        {
            switch ((EFixtureAlignAutoRunStep)Step.RunStep)
            {
                case EFixtureAlignAutoRunStep.Start:
                    Log.Debug("AutoRun Start");
                    Step.RunStep++;
                    break;
                case EFixtureAlignAutoRunStep.FixtureDetectCheck:
                    Log.Debug("Fixture Detect Check");
                    if (_machineStatus.MachineRunMode == EMachineRunMode.DryRun)
                    {
                        Log.Info("Sequence Fixture Align");
                        Sequence = ESequence.FixtureAlign;
                        break;
                    }
                    if (IsFixtureDetect)
                    {
                        Log.Info("Sequence Fixture Align");
                        Sequence = ESequence.FixtureAlign;
                        break;
                    }
                    Log.Info("Sequence Robot Place Fixture To Align");
                    Sequence = ESequence.RobotPlaceFixtureToAlign;
                    break;
                case EFixtureAlignAutoRunStep.End:
                    break;
            }
        }

        private void Sequence_RobotPlaceFixtureToAlign()
        {
            switch ((EFixtureAlignRobotPlaceFixtureToAlignStep)Step.RunStep)
            {
                case EFixtureAlignRobotPlaceFixtureToAlignStep.Start:
                    Log.Debug("Robot Place Fixture To Align Start");
                    Step.RunStep++;
                    break;
                case EFixtureAlignRobotPlaceFixtureToAlignStep.SetFlagRequestFixture:
                    Log.Debug("Set Flag Request Load");
                    FlagFixtureAlignReqLoad = true;
                    Log.Debug("Wait Fixture Align Load Done");
                    Step.RunStep++;
                    break;
                case EFixtureAlignRobotPlaceFixtureToAlignStep.WaitFixtureAlignLoadDone:
                    if (FlagFixtureAlignLoadDone == false)
                    {
                        Wait(20);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.AlignFixtureDetect, true);
#endif
                    Log.Debug("Clear Flag Align Done");
                    FlagFixtureAlignDone = false;
                    Step.RunStep++;
                    break;
                case EFixtureAlignRobotPlaceFixtureToAlignStep.FixtureDetectCheck:
                    Log.Debug("Fixture Detect Check");
                    if (!IsFixtureDetect)
                    {
                        RaiseWarning((int)EWarning.FixtureAlignLoadFail);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.AlignFixtureDetect, false);
#endif
                    Step.RunStep++;
                    break;
                case EFixtureAlignRobotPlaceFixtureToAlignStep.End:
                    Log.Debug("Robot Place Fixture To Align End");
                    Log.Info("Sequence Fixture Align");
                    Sequence = ESequence.FixtureAlign;
                    break;
            }
        }

        private void Sequence_FixtureAlign()
        {
            switch ((EFixtureAlignStep)Step.RunStep)
            {
                case EFixtureAlignStep.Start:
                    Log.Debug("Fixture Align Start");
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.Cyl_Align:
                    Log.Debug("Align Fixture");
                    AlignFixtureCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return AlignFixtureCyl.IsForward; });
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.Cyl_Align_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Align Fixture Done");
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.TiltCheck:
                    if (IsFixtureTiltDetect)
                    {
                        RaiseWarning((int)EWarning.FixtureAlignTiltDetect);
                        break;
                    }
                    Log.Debug("Fixture Tilt Check OK");

                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.ReverseCheck:
                    if (IsFixtureReverseDetect)
                    {
                        RaiseWarning((int)EWarning.FixtureAlignReverseDetect);
                        break;
                    }
                    Log.Debug("Fixture Reverse Check OK");

                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.Cyl_UnAlign:
                    Log.Debug("UnAlign Fixture");
                    AlignFixtureCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return AlignFixtureCyl.IsBackward; });
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.Cyl_UnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("UnAlign Fixture Done");
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.SetFlagAlignDone:
                    Log.Debug("Set Flag Align Done");
                    FlagFixtureAlignDone = true;
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer Fixture");
                    Sequence = ESequence.TransferFixtureLoad;
                    break;
            }
        }

        private void Sequence_TransferFixtureLoad()
        {
            switch ((EFixtureAlignTransferStep)Step.RunStep)
            {
                case EFixtureAlignTransferStep.Start:
                    Log.Debug("Fixture Transfer Start");
                    Step.RunStep++;
                    Log.Debug("Wait Fixture Transfer Done");
                    break;
                case EFixtureAlignTransferStep.Wait_TransferDone:
                    if (FlagFixtureTransferDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EFixtureAlignTransferStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Robot Place Fixture To Align");
                    Sequence = ESequence.RobotPlaceFixtureToAlign;
                    break;
            }
        }
        #endregion
    }
}
