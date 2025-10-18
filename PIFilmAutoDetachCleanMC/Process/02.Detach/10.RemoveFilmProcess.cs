using EQX.Core.InOut;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.InOut.Virtual;
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
    public class RemoveFilmProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _removeFilmInput;
        private readonly IDOutputDevice _removeFilmOutput;
        private readonly MachineStatus _machineStatus;

        private ICylinder FixCyl1_1 => _devices.Cylinders.RemoveZone_ClampCyl1;
        private ICylinder FixCyl1_2 => _devices.Cylinders.RemoveZone_ClampCyl2;
        private ICylinder FixCyl2_1 => _devices.Cylinders.RemoveZone_ClampCyl3;
        private ICylinder FixCyl2_2 => _devices.Cylinders.RemoveZone_ClampCyl4;

        private ICylinder TransferCyl => _devices.Cylinders.RemoveZone_TransferCyl;
        private ICylinder UpDownCyl1 => _devices.Cylinders.RemoveZone_UpDownCyl1;
        private ICylinder UpDownCyl2 => _devices.Cylinders.RemoveZone_UpDownCyl2;
        private ICylinder ClampCyl1 => _devices.Cylinders.RemoveZone_FilmClampCyl1;
        private ICylinder ClampCyl2 => _devices.Cylinders.RemoveZone_FilmClampCyl2;
        private ICylinder ClampCyl3 => _devices.Cylinders.RemoveZone_FilmClampCyl3;


        private ICylinder PusherCyl1 => _devices.Cylinders.RemoveZone_PusherCyl1;
        private ICylinder PusherCyl2 => _devices.Cylinders.RemoveZone_PusherCyl2;

        private bool IsFixtureDetect => _devices.Inputs.RemoveZoneFixtureDetect.Value;

        private bool IsFixCylinderBw => FixCyl1_1.IsBackward && FixCyl1_2.IsBackward && FixCyl2_1.IsBackward && FixCyl2_2.IsBackward;
        private bool IsFixCylinderFw => FixCyl1_1.IsForward && FixCyl1_2.IsForward && FixCyl2_1.IsForward && FixCyl2_2.IsForward;
        #endregion

        private void FixCylinderFwBw(bool isForward)
        {
            if (isForward)
            {
                FixCyl1_1.Forward();
                FixCyl1_2.Forward();
                FixCyl2_1.Forward();
                FixCyl2_2.Forward();
            }
            else
            {
                FixCyl1_1.Backward();
                FixCyl1_2.Backward();
                FixCyl2_1.Backward();
                FixCyl2_2.Backward();
            }
        }
        #region Contructor
        public RemoveFilmProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("RemoveFilmInput")] IDInputDevice removeFilmInput,
            [FromKeyedServices("RemoveFilmOutput")] IDOutputDevice removeFilmOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _removeFilmInput = removeFilmInput;
            _removeFilmOutput = removeFilmOutput;
        }
        #endregion

        #region Flags
        private bool FlagFixtureTransferDone
        {
            get
            {
                return _removeFilmInput[(int)ERemoveFilmProcessInput.FIXTURE_TRANSFER_DONE];
            }
        }

        private bool FlagTransferFixtureDoneReceived
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED] = value;
            }
        }
        private bool FlagRemoveFilmDone
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_DONE] = value;
            }
        }

        private bool FlagRemoveFilmRequestUnload
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD] = value;
            }
        }

        private bool FlagRemoveFilmUnloadDone
        {
            get
            {
                return _removeFilmInput[(int)ERemoveFilmProcessInput.REMOVE_FILM_UNLOAD_DONE];
            }
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ERemoveFilmProcessOriginStep)Step.OriginStep)
            {
                case ERemoveFilmProcessOriginStep.Start:
                    Log.Debug("Remove Film Process Start");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Fix_Cyl_Backward:
                    Log.Debug("Remove Film Process Fix Cylinder Backward");
                    FixCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixCylinderBw);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Fix_Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_FixCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Remove Film Process Fix Cylinder Backward Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Pusher_Cyl_Down:
                    Log.Debug("Remove Film Process Pusher Cylinder Down");
                    PusherCyl1.Backward();
                    PusherCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl1.IsBackward && PusherCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Pusher_Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (PusherCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder1_Down_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Remove Film Process Pusher Cylinder Down Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Up:
                    Log.Debug("Remove Film Process Cylinder Up");
                    UpDownCyl1.Backward();
                    UpDownCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsBackward && UpDownCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (UpDownCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Up_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_UnClamp:
                    Log.Debug("Remove Film Process Cylinder UnClamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    ClampCyl3.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward && ClampCyl3.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        // TODO: Remove temp code
                        Log.Debug("Remove Film Process Cylinder UnClamp Done");
                        Step.OriginStep++;
                        break;

                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder UnClamp Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Transfer_Backward:
                    Log.Debug("Remove Film Process Cylinder Transfer Backward");
                    TransferCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TransferCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Transfer_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_TransferCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder Transfer Backward Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.End:
                    Log.Debug("Remove Film Process Origin End");
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
                    Sequence_Ready();
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
                    break;
                case ESequence.FixtureAlign:
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    break;
                case ESequence.TransferFixtureLoad:
                    break;
                case ESequence.Detach:
                    break;
                case ESequence.TransferFixtureUnload:
                    Sequence_TransferFixtureUnload();
                    break;
                case ESequence.DetachUnload:
                    break;
                case ESequence.RemoveFilm:
                    Sequence_RemoveFilm();
                    break;
                case ESequence.RemoveFilmThrow:
                    Sequence_RemoveFilmThrow();
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
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((ERemoveFilmProcessToRunStep)Step.ToRunStep)
            {
                case ERemoveFilmProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case ERemoveFilmProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<ERemoveFilmProcessOutput>)_removeFilmOutput).Clear();
                    Step.ToRunStep++;
                    break;
                case ERemoveFilmProcessToRunStep.End:
                    Log.Debug("To Run End");
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
            switch ((ERemoveFilmAutoRunStep)Step.RunStep)
            {
                case ERemoveFilmAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmAutoRunStep.FixtureDetect_Check:
                    if (IsFixtureDetect)
                    {
                        Log.Info("Sequence Remove Film");
                        Sequence = ESequence.RemoveFilm;
                        break;
                    }
                    if (_machineStatus.IsDryRunMode)
                    {
                        Log.Info("Dry Run Mode Skip Remove Film Auto Run");
                        Step.RunStep = (int)ERemoveFilmAutoRunStep.End;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERemoveFilmAutoRunStep.End:
                    Log.Info("Sequence Transfer Fixture Unload");
                    Sequence = ESequence.TransferFixtureUnload;
                    break;
            }
        }

        private void Sequence_Ready()
        {
            switch ((ERemoveFilmReadyStep)Step.RunStep)
            {
                case ERemoveFilmReadyStep.Start:
                    if (IsOriginOrInitSelected == false)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    UpDownCyl1.Backward();
                    UpDownCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsBackward && UpDownCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (UpDownCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Up_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_Puhser_Down:
                    Log.Debug("Cylinder Pusher Down");
                    PusherCyl1.Backward();
                    PusherCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl1.IsBackward && PusherCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_Pusher_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (PusherCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder1_Down_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_Backward:
                    Log.Debug("Cylinder Backward");
                    TransferCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TransferCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    CylClampUnClamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward && ClampCyl3.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmReadyStep.End:
                    Log.Debug("Initialize End");
                    IsWarning = false;
                    Sequence = ESequence.Stop;
                    break;
                default:
                    Wait(20);
                    break;
            }
        }

        private void Sequence_TransferFixtureUnload()
        {
            switch ((ERemoveFilmProcessTransferFixtureUnloadStep)Step.RunStep)
            {
                case ERemoveFilmProcessTransferFixtureUnloadStep.Start:
                    Log.Debug("Remove Film Process Transfer Fixture Unload Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.Cyl_Fix_Backward:
                    Log.Debug("Fix Cylinder Backward");
                    FixCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixCylinderBw);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.Cyl_Fix_Backward_Done:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_FixCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Fix Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.Set_Flag_RemoveFilmDone:
                    Log.Debug("Set Flag Remove Film Done");
                    FlagRemoveFilmDone = true;
                    Step.RunStep++;
                    Log.Debug("Wait Transfer Fixture Done");
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.Wait_TransferFixtureDone:
                    if (FlagFixtureTransferDone == false)
                    {
                        Wait(20);
                        break;
                    }

#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.RemoveZoneFixtureDetect, true);
#endif
                    Log.Debug("Clear Flag Remove Film Done");
                    FlagRemoveFilmDone = false;

                    Log.Debug("Set Flag Transfer Fixture Done Received");
                    FlagTransferFixtureDoneReceived = true;

                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.Fixture_Detect_Check:
                    if (IsFixtureDetect == false && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_Fixture_NotDetect);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.End:
                    Log.Debug("Transfer Fixture Unload End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Remove Film");
                    Sequence = ESequence.RemoveFilm;
                    break;
            }
        }

        private void Sequence_RemoveFilm()
        {
            switch ((ERemoveFilmProcessRemoveStep)Step.RunStep)
            {
                case ERemoveFilmProcessRemoveStep.Start:
                    Log.Debug("Remove Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Fix_Cyl_Forward:
                    Log.Debug("Fix Cylinder Forward");
                    FixCylinderFwBw(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixCylinderFw);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Fix_Cyl_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_FixCylinder_Forward_Fail);
                        break;
                    }
                    Log.Debug("Fix Cylinder Forward");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Forward:
                    Log.Debug("Cylinder Transfer Forward");
                    TransferCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TransferCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_TransferCylinder_Forward_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Transfer Forward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_2_Up:
                    Log.Debug("Pusher Cylinder 2 Up");
                    PusherCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_2_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Pusher Cylinder 2 Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Down:
                    Log.Debug("Cylinder UpDown1 Down");
                    UpDownCyl1.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Down Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Clamp:
                    Log.Debug("Cylinder Clamp");
                    CylClampUnClamp(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsForward && ClampCyl2.IsForward && ClampCyl3.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_1_Up:
                    Log.Debug("Pusher Cylinder 1 Up");
                    PusherCyl1.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl1.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder1_Up_Fail);
                        break;
                    }
                    Log.Debug("Pusher Cylinder 1 Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Up:
                    Log.Debug("Cylinder UpDown1 Up");
                    UpDownCyl1.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Backward:
                    Log.Debug("Cylinder Transfer Backward");
                    TransferCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TransferCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_TransferCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Transfer Backward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Set_Flag_RemoveFilmRequestUnload:
                    Log.Debug("Set Flag Remove Film Request Unload");
                    FlagRemoveFilmRequestUnload = true;
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Remove Film Throw");
                    Sequence = ESequence.RemoveFilmThrow;
                    break;
            }
        }

        private void Sequence_RemoveFilmThrow()
        {
            switch ((ERemoveFilmThrowStep)Step.RunStep)
            {
                case ERemoveFilmThrowStep.Start:
                    Log.Debug("Robot Pick From Remove Zone Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown1_Down:
                    Log.Debug("Cylinder UpDown1 Down");
                    UpDownCyl1.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Down Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    CylClampUnClamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => ClampCyl1.IsBackward && ClampCyl2.IsBackward && ClampCyl3.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Down_1st:
                    Log.Debug("Cylinder UpDown2 Down 1st");
                    UpDownCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Down_1st_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Down 1st Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Up_1st:
                    Log.Debug("Cylinder UpDown2 Up 1st");
                    UpDownCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Up_1st_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Up 1st Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Down_2nd:
                    Log.Debug("Cylinder UpDown2 Down 2nd");
                    UpDownCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Down_2nd_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Down 2nd Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Up_2nd:
                    Log.Debug("Cylinder UpDown2 Up 2nd");
                    UpDownCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown2_Up_2nd_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Up 2nd Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown1_Up:
                    Log.Debug("Cylinder UpDown1 Up");
                    UpDownCyl1.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.Cyl_UpDown1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Up Done");
                    Step.RunStep++;
                    Log.Debug("Wait Remove Film Unload Done");
                    break;
                case ERemoveFilmThrowStep.Wait_RemoveFilmUnloadDone:
                    if (FlagRemoveFilmUnloadDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERemoveFilmThrowStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer Fixture Unload");
                    Sequence = ESequence.TransferFixtureUnload;
                    break;
            }
        }

        private void CylClampUnClamp(bool bClamp)
        {
            if (bClamp)
            {
                ClampCyl1.Forward();
                ClampCyl2.Forward();
                ClampCyl3.Forward();
            }
            else
            {
                ClampCyl1.Backward();
                ClampCyl2.Backward();
                ClampCyl3.Backward();
            }
        }
        #endregion
    }
}
