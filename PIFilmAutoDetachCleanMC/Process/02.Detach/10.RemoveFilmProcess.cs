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
        private Queue<ERemoveFilmRobotPickFromRemoveZoneStep> RobotPickFixtureFromRemoveZoneSteps = new Queue<ERemoveFilmRobotPickFromRemoveZoneStep>();


        private ICylinder ClampCyl1 => _devices.Cylinders.RemoveZone_ClampCyl1;
        private ICylinder ClampCyl2 => _devices.Cylinders.RemoveZone_ClampCyl2;
        private ICylinder ClampCyl3 => _devices.Cylinders.RemoveZone_ClampCyl3;
        private ICylinder ClampCyl4 => _devices.Cylinders.RemoveZone_ClampCyl4;

        private ICylinder TransferCyl => _devices.Cylinders.RemoveZone_TransferCyl;
        private ICylinder UpDownCyl1 => _devices.Cylinders.RemoveZone_UpDownCyl1;
        private ICylinder UpDownCyl2 => _devices.Cylinders.RemoveZone_UpDownCyl2;
        private ICylinder FilmClampCyl1 => _devices.Cylinders.RemoveZone_FilmClampCyl1;
        private ICylinder FilmClampCyl2 => _devices.Cylinders.RemoveZone_FilmClampCyl2;
        private ICylinder FilmClampCyl3 => _devices.Cylinders.RemoveZone_FilmClampCyl3;

        private ICylinder PusherCyl1 => _devices.Cylinders.RemoveZone_PusherCyl1;
        private ICylinder PusherCyl2 => _devices.Cylinders.RemoveZone_PusherCyl2;

        private bool IsFixtureDetect
        {
            get
            {
                return _devices.Inputs.RemoveZoneFixtureDetect.Value;
            }
        }

        private bool IsFilmClampCylinderUnClamp => FilmClampCyl1.IsBackward && FilmClampCyl2.IsBackward && FilmClampCyl3.IsBackward;
        private bool IsFilmClampCylinderClamp => FilmClampCyl1.IsForward && FilmClampCyl2.IsForward && FilmClampCyl3.IsForward;

        private bool IsClampCylinderUnClamp => ClampCyl1.IsBackward && ClampCyl2.IsBackward && ClampCyl3.IsBackward && ClampCyl4.IsBackward;
        private bool IsClampCylinderClamp => ClampCyl1.IsForward && ClampCyl2.IsForward && ClampCyl3.IsForward && ClampCyl4.IsForward;

        public IDInput RobotInReady => _devices.Inputs.LoadRobInReady;
        public IDInput RobotInHome => _devices.Inputs.LoadRobInHome;
        #endregion

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
        private bool FlagRobotOriginDone
        {
            get
            {
                return _removeFilmInput[(int)ERemoveFilmProcessInput.ROBOT_ORIGIN_DONE];
            }
        }

        private bool FlagFixtureTransferDone
        {
            get
            {
                return _removeFilmInput[(int)ERemoveFilmProcessInput.FIXTURE_TRANSFER_DONE];
            }
        }

        private bool FlagRemoveFilmLoadReady
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_LOAD_READY] = value;
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

        private bool FlagRemoveFilmOriginDone
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_ORIGIN_DONE] = value;
            }
        }

        private bool FlagRemoveFilmReadyDone
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_READY_DONE] = value;
            }
        }

        private bool FlagRemoveFilmUnclampFixtureDone
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_UNCLAMP_FIXTURE_DONE] = value;
            }
        }

        private bool FlagRobotClampRemoveFilmFixtureDone
        {
            get
            {
                return _removeFilmInput[(int)ERemoveFilmProcessInput.ROBOT_CLAMP_REMOVE_FILM_FIXTURE_DONE];
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
                case ERemoveFilmProcessOriginStep.Wait_Robot_Origin:
                    if (FlagRobotOriginDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Clamp_Cyl_Backward:
                    Log.Debug("Remove Film Process Clamp Cylinder Backward");
                    ClampCylClampUnclamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderUnClamp);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Clamp_Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Remove Film Process Clamp Cylinder Backward Done");
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
                    FilmClampCyl1.Backward();
                    FilmClampCyl2.Backward();
                    FilmClampCyl3.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => FilmClampCyl1.IsBackward && FilmClampCyl2.IsBackward && FilmClampCyl3.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_FilmClampCylinder_UnClamp_Fail);
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
                    FlagRemoveFilmOriginDone = true;
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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    Sequence_RobotPickFixtureFromRemoveZone();
                    break;
                case ESequence.TransferFixture:
                    Sequence_TransferFixture();
                    break;
                case ESequence.RemoveFilm:
                    Sequence_RemoveFilm();
                    break;
                default:
                    Sequence = ESequence.Stop;
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
                    ((MappableOutputDevice<ERemoveFilmProcessOutput>)_removeFilmOutput).ClearOutputs();
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
            switch ((ERemoveFilm_AutoRunStep)Step.RunStep)
            {
                case ERemoveFilm_AutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilm_AutoRunStep.FixtureDetect_Check:
                    if (IsFixtureDetect)
                    {
                        Log.Info("Sequence Remove Film");
                        Sequence = ESequence.RemoveFilm;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERemoveFilm_AutoRunStep.End:
                    Log.Info("Sequence Transfer Fixture Unload");
                    Sequence = ESequence.TransferFixture;
                    break;
            }
        }

        private void Sequence_Ready()
        {
            switch ((ERemoveFilm_ReadyStep)Step.RunStep)
            {
                case ERemoveFilm_ReadyStep.Start:
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                    // TODO: REMOVE THIS STEP
                case ERemoveFilm_ReadyStep.Wait_Robot_Ready:
                    Step.RunStep++;
                    break;
                case ERemoveFilm_ReadyStep.CylUpDown_CylPusher_MoveBackward:
                    if (UpDownCyl1.IsBackward && UpDownCyl2.IsBackward && PusherCyl1.IsBackward && PusherCyl2.IsBackward)
                    {
                        Step.RunStep = (int)ERemoveFilm_ReadyStep.CylTransfer_Backward;
                        break;
                    }

                    Log.Debug("Cylinder Up");
                    UpDownCyl1.Backward();
                    UpDownCyl2.Backward();
                    PusherCyl1.Backward();
                    PusherCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => UpDownCyl1.IsBackward && UpDownCyl2.IsBackward && PusherCyl1.IsBackward && PusherCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilm_ReadyStep.CylUpDown_CylPusher_MoveBackward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (UpDownCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Up_Fail);
                            break;
                        }
                        if (UpDownCyl2.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Up_Fail);
                            break;
                        }
                        if (PusherCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder1_Down_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.RemoveFilm_PusherCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilm_ReadyStep.CylTransfer_Backward:
                    if (TransferCyl.IsBackward)
                    {
                        Step.RunStep = (int)ERemoveFilm_ReadyStep.Cyl_UnClamp;
                        break;
                    }

                    Log.Debug("Cylinder Backward");
                    TransferCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TransferCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilm_ReadyStep.CylTransfer_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_TransferCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilm_ReadyStep.Cyl_UnClamp:
                    if (FilmClampCyl1.IsBackward && FilmClampCyl2.IsBackward && FilmClampCyl3.IsBackward)
                    {
                        Step.RunStep = (int)ERemoveFilm_ReadyStep.End;
                        break;
                    }

                    Log.Debug("Cylinder UnClamp");
                    FilmClampCylClampUnClamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFilmClampCylinderUnClamp);
                    Step.RunStep++;
                    break;
                case ERemoveFilm_ReadyStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilm_ReadyStep.End:
                    FlagRemoveFilmReadyDone = true;
                    Log.Debug("Initialize End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_TransferFixture()
        {
            switch ((ERemoveFilmProcessTransferFixtureStep)Step.RunStep)
            {
                case ERemoveFilmProcessTransferFixtureStep.Start:
                    Log.Debug("Remove Film Process Transfer Fixture Unload Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureStep.Cyl_Fix_Backward:
                    Log.Debug("Fix Cylinder Backward");
                    ClampCylClampUnclamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderUnClamp);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureStep.Cyl_Fix_Backward_Done:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Fix Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureStep.Wait_PreviousTransferDone:
                    //if (FlagFixtureTransferDone == true)
                    //{
                    //    Wait(20);
                    //    break;
                    //}

                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureStep.Set_Flag_RemoveFilmDone:
                    Log.Debug("Set Flag Remove Film Done");
                    FlagRemoveFilmLoadReady = true;
                    Step.RunStep++;
                    Log.Debug("Wait Transfer Fixture Done");
                    break;
                case ERemoveFilmProcessTransferFixtureStep.Wait_TransferFixtureDone:
                    if (FlagFixtureTransferDone == false)
                    {
                        Wait(20);
                        break;
                    }

#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.RemoveZoneFixtureDetect, true);
#endif
                    Log.Debug("Clear Flag Remove Film Done");
                    FlagRemoveFilmLoadReady = false;

                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureStep.Wait_TransferFixtureClear:
                    if (FlagFixtureTransferDone == true)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("FlagFixtureTransferDone Clear");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureStep.Fixture_Detect_Check:
                    if (_machineStatus.FixtureExistStatus[1] == false)
                    {
                        // No up-stream fixture exist before transfer
                        if (Parent?.Sequence != ESequence.AutoRun)
                        {
                            Sequence = ESequence.Stop;
                            break;
                        }

                        Sequence = ESequence.TransferFixture;
                        break;
                    }

                    if (IsFixtureDetect == false && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_Fixture_NotDetect);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureStep.End:
                    Log.Debug("Transfer Fixture Unload End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
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
                case ERemoveFilmProcessRemoveStep.Clamp_Cyl_Forward:
                    Log.Debug("Clamp Cylinder Forward");
                    ClampCylClampUnclamp(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderClamp || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Clamp_Cyl_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Clamp Cylinder Forward");
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
                    Log.Debug("Cylinder Film Clamp");
                    FilmClampCylClampUnClamp(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFilmClampCylinderClamp);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_FilmClampCylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Film Clamp Done");
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
                case ERemoveFilmProcessRemoveStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                    Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                    break;
            }
        }

        private void Sequence_RobotPickFixtureFromRemoveZone()
        {
            switch ((ERemoveFilmRobotPickFromRemoveZoneStep)Step.RunStep)
            {
                case ERemoveFilmRobotPickFromRemoveZoneStep.Start:
                    Log.Debug("Robot Pick From Remove Zone Start");
                    RobotPickFixtureFromRemoveZoneSteps = new Queue<ERemoveFilmRobotPickFromRemoveZoneStep>(ProcessesWorkSequence.RemoveFilmRobotPickFromRemoveZoneSequence);
                    Step.RunStep++;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck:
                    if (RobotPickFixtureFromRemoveZoneSteps.Count <= 0)
                    {
                        Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.End;
                        break;
                    }
                    Step.RunStep = (int)RobotPickFixtureFromRemoveZoneSteps.Dequeue();
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Set_Flag_RemoveFilmRequestUnload:
                    Log.Debug("Set Flag Remove Film Request Unload");
                    FlagRemoveFilmRequestUnload = true;
                    Log.Debug("Wait Robot Clamp Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Wait_RobotClampDone:
                    if(FlagRobotClampRemoveFilmFixtureDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    ClampCylClampUnclamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderUnClamp);
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.SetFlag_RemoveFilmUnclampFixtureDone:
                    Log.Debug("Set Flag Remove Film Unclamp Fixture Done");
                    FlagRemoveFilmUnclampFixtureDone = true;
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Wait_RobotRemoveFilmUnclampFixtureDoneReceived:
                    if(FlagRobotClampRemoveFilmFixtureDone == true)
                    {
                        Wait(20);
                        break;
                    }
                    FlagRemoveFilmUnclampFixtureDone = false;
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Down:
                    Log.Debug("Cylinder UpDown1 Down");
                    UpDownCyl1.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsForward);
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Down Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.FilmCyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    FilmClampCylClampUnClamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFilmClampCylinderUnClamp);
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.FilmCyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_FilmClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Down:
                    Log.Debug("Cylinder UpDown2 Down");
                    UpDownCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl2.IsForward);
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Down Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Up:
                    Log.Debug("Cylinder UpDown2 Up");
                    UpDownCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl2.IsBackward);
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Up Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Up:
                    Log.Debug("Cylinder UpDown1 Up");
                    UpDownCyl1.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl1.IsBackward);
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RemoveFilm_UpDownCylinder1_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Up Done");
                    Log.Debug("Wait Remove Film Unload Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_Pusher_Down:
                    Log.Debug("Pusher Cylinder Down");
                    PusherCyl1.Backward();
                    PusherCyl2.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => PusherCyl1.IsBackward && PusherCyl2.IsBackward);
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_Pusher_Down_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        if(PusherCyl1.IsBackward == false)
                        {
                            RaiseWarning(EWarning.RemoveFilm_PusherCylinder1_Down_Fail);
                            break;
                        }
                        RaiseWarning(EWarning.RemoveFilm_PusherCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Pusher Cylinder Down Done");
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.Wait_RemoveFilmUnloadDone:
                    if (FlagRemoveFilmUnloadDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Info("FlagRemoveFilmRequestUnload clear");
                    FlagRemoveFilmRequestUnload = false;
                    Step.RunStep = (int)ERemoveFilmRobotPickFromRemoveZoneStep.StepQueue_EmptyCheck;
                    break;
                case ERemoveFilmRobotPickFromRemoveZoneStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Info("Sequence Transfer Fixture Unload");
                    Sequence = ESequence.TransferFixture;
                    break;
            }
        }

        private void FilmClampCylClampUnClamp(bool bClamp)
        {
            if (bClamp)
            {
                FilmClampCyl1.Forward();
                FilmClampCyl2.Forward();
                FilmClampCyl3.Forward();
            }
            else
            {
                FilmClampCyl1.Backward();
                FilmClampCyl2.Backward();
                FilmClampCyl3.Backward();
            }
        }
        private void ClampCylClampUnclamp(bool isForward)
        {
            if (isForward)
            {
                ClampCyl1.Forward();
                ClampCyl2.Forward();
                ClampCyl3.Forward();
                ClampCyl4.Forward();
            }
            else
            {
                ClampCyl1.Backward();
                ClampCyl2.Backward();
                ClampCyl3.Backward();
                ClampCyl4.Backward();
            }
        }

        #endregion
    }
}
