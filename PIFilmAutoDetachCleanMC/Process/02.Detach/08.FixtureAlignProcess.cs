using EQX.Core.InOut;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;

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

        private ICylinder AlignFixtureCyl1 => _devices.Cylinders.FixtureAlign_AlignCyl1;
        private ICylinder AlignFixtureCyl2 => _devices.Cylinders.FixtureAlign_AlignCyl2;
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

        private bool FlagFixtureAlignLoadDoneReceived
        {
            set
            {
                _fixtureAlignOutput[(int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_LOAD_DONE_RECEIVED] = value;
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
                return _fixtureAlignInput[(int)EFixtureAlignProcessInput.FIXTURE_TRANSFER_DONE];
            }
        }

        private bool FlagTransferFixtureDoneReceive
        {
            set
            {
                _fixtureAlignOutput[(int)EFixtureAlignProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED] = value;
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
                    AlignFixtureCyl1.Backward();
                    AlignFixtureCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignFixtureCyl1.IsBackward && AlignFixtureCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EFixtureAlignOriginStep.AlignFixtureBackward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.FixtureAlign_AlignCylinder_Backward_Fail);
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
                    IsWarning = false;
                    Sequence = ESequence.Stop;
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
                case ESequence.AlignGlassLeft:
                    break;
                case ESequence.TransferInShuttleLeftPick:
                    break;
                case ESequence.WETCleanLeftLoad:
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanLeftUnload:
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.AFCleanLeftLoad:
                    break;
                case ESequence.AFCleanLeft:
                    break;
                case ESequence.AFCleanLeftUnload:
                    break;
                case ESequence.UnloadTransferLeftPlace:
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

        public override bool ProcessToRun()
        {
            switch ((EFixtureAlignProcessToRunStep)Step.ToRunStep)
            {
                case EFixtureAlignProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EFixtureAlignProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<EFixtureAlignProcessOutput>)_fixtureAlignOutput).Clear();
                    Step.ToRunStep++;
                    break;
                case EFixtureAlignProcessToRunStep.End:
                    Log.Debug(" To Run End");
                    Step.ToRunStep++;
                    ProcessStatus = EProcessStatus.ToRunDone;
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
                    if (_machineStatus.IsDryRunMode)
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
                    Log.Debug("Clear Flag Fixture Align Load Done Received");
                    FlagFixtureAlignLoadDoneReceived = false;
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
                    Log.Debug("Clear Flag Request Load");
                    FlagFixtureAlignReqLoad = false;

                    Log.Debug("Set Flag Fixture Align Load Done Received");
                    FlagFixtureAlignLoadDoneReceived = true;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.AlignFixtureDetect, true);
#endif
                    Step.RunStep++;
                    break;
                case EFixtureAlignRobotPlaceFixtureToAlignStep.FixtureDetectCheck:
                    if (IsFixtureDetect == false && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.FixtureAlign_Fixture_NotDetect);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EFixtureAlignRobotPlaceFixtureToAlignStep.End:
                    Log.Debug("Robot Place Fixture To Align End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
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
                    AlignFixtureCyl1.Forward();
                    AlignFixtureCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return AlignFixtureCyl1.IsForward && AlignFixtureCyl2.IsForward; });
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.Cyl_Align_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.FixtureAlign_AlignCylinder_Forward_Fail);
                        break;
                    }
                    Log.Debug("Align Fixture Done");
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.TiltCheck:
                    if (IsFixtureTiltDetect == true && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.FixtureAlign_Fixture_TiltDetect);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.ReverseCheck:
                    if (IsFixtureReverseDetect == true && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.FixtureAlign_Fixture_ReverseDetect);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.Cyl_UnAlign:
                    Log.Debug("UnAlign Fixture");
                    AlignFixtureCyl1.Backward();
                    AlignFixtureCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return AlignFixtureCyl1.IsBackward && AlignFixtureCyl2.IsBackward; });
                    Step.RunStep++;
                    break;
                case EFixtureAlignStep.Cyl_UnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.FixtureAlign_AlignCylinder_Backward_Fail);
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
                    if (Parent?.Sequence != ESequence.AutoRun)
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
                    break;
                case EFixtureAlignTransferStep.Clear_Flag:
                    Log.Debug("Clear Flag Transfer Fixture Done Received");
                    FlagTransferFixtureDoneReceive = true;
                    Log.Debug("Wait Fixture Transfer Done");
                    Step.RunStep++;
                    break;
                case EFixtureAlignTransferStep.Cyl_UnAlign:
                    Log.Debug("UnAlign Fixture");
                    AlignFixtureCyl1.Backward();
                    AlignFixtureCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return AlignFixtureCyl1.IsBackward && AlignFixtureCyl2.IsBackward; });
                    Step.RunStep++;
                    break;
                case EFixtureAlignTransferStep.Cyl_UnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.FixtureAlign_AlignCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("UnAlign Fixture Done");
                    Step.RunStep++;
                    break;
                case EFixtureAlignTransferStep.Set_FlagAlignDone:
                    Log.Debug("Set Flag Align Done");
                    FlagFixtureAlignDone = true;
                    Step.RunStep++;
                    break;
                case EFixtureAlignTransferStep.Wait_TransferDone:
                    if (FlagFixtureTransferDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Fixture Align Done");
                    FlagFixtureAlignDone = false;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.AlignFixtureDetect, false);
#endif
                    Log.Debug("Set Flag Transfer Fixture Done Received");
                    FlagTransferFixtureDoneReceive = true;
                    Step.RunStep++;
                    break;
                case EFixtureAlignTransferStep.End:
                    Log.Debug("Transfer Fixture Load End");
                    if (Parent?.Sequence != ESequence.AutoRun)
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
