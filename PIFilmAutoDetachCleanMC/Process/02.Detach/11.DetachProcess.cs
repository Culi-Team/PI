using EQX.Core.InOut;
using EQX.Core.Motion;
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
    public class DetachProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly DetachRecipe _detachRecipe;
        private readonly IDInputDevice _detachInput;
        private readonly IDOutputDevice _detachOutput;
        private readonly MachineStatus _machineStatus;

        private Queue<EDetachStep> DetachSteps = new Queue<EDetachStep>();

        private IMotion DetachGlassZAxis => _devices.Motions.DetachGlassZAxis;
        private IMotion ShuttleTransferXAxis => _devices.Motions.ShuttleTransferXAxis;
        private IMotion ShuttleTransferZAxis => _devices.Motions.ShuttleTransferZAxis;

        private ICylinder ClampCyl1 => _devices.Cylinders.Detach_ClampCyl1;
        private ICylinder ClampCyl2 => _devices.Cylinders.Detach_ClampCyl2;
        private ICylinder ClampCyl3 => _devices.Cylinders.Detach_ClampCyl3;
        private ICylinder ClampCyl4 => _devices.Cylinders.Detach_ClampCyl4;

        private ICylinder DetachCyl1 => _devices.Cylinders.Detach_UpDownCyl1;
        private ICylinder DetachCyl2 => _devices.Cylinders.Detach_UpDownCyl2;

        private bool IsFixtureDetect => _devices.Inputs.DetachFixtureDetect.Value;

        private bool IsGlassShuttleVac1 => _devices.Inputs.DetachGlassShtVac1.Value;
        private bool IsGlassShuttleVac2 => _devices.Inputs.DetachGlassShtVac2.Value;
        private bool IsGlassShuttleVac3 => _devices.Inputs.DetachGlassShtVac3.Value;
        private bool IsGlassShuttleVacAll => IsGlassShuttleVac1 && IsGlassShuttleVac2 && IsGlassShuttleVac3;

        private IDOutput GlassShuttleVac1 => _devices.Outputs.DetachGlassShtVac1OnOff;
        private IDOutput GlassShuttleVac2 => _devices.Outputs.DetachGlassShtVac2OnOff;
        private IDOutput GlassShuttleVac3 => _devices.Outputs.DetachGlassShtVac3OnOff;

        private bool IsClampCylinderFw => ClampCyl1.IsForward && ClampCyl2.IsForward && ClampCyl3.IsForward && ClampCyl4.IsForward;
        private bool IsClampCylinderBw => ClampCyl1.IsBackward && ClampCyl2.IsBackward && ClampCyl3.IsBackward && ClampCyl4.IsBackward;
        #endregion

        private void ClampCylinderFwBw(bool isForward)
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
        #region Flags
        private bool FlagDetachOriginDone
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.DETACH_ORIGIN_DONE] = value;
            }
        }

        private bool FlagDetachReadyDone
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.DETACH_READY_DONE] = value;
            }
        }

        private bool FlagDetachDone
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.DETACH_DONE] = value;
            }
        }

        private bool FlagFixtureTransferDone
        {
            get
            {
                return _detachInput[(int)EDetachProcessInput.FIXTURE_TRANSFER_DONE];
            }
        }

        private bool FlagGlassTransferPickDone
        {
            get
            {
                return _detachInput[(int)EDetachProcessInput.GLASS_TRANSFER_PICK_DONE];
            }
        }

        private bool FlagIn_TransferFixtureClampDone
        {
            get
            {
                return _detachInput[(int)EDetachProcessInput.TRANSFER_FIXTURE_CLAMP_DONE];
            }
        }

        private bool FlagOut_DetachFixtureUnClampDone
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.DETACH_UNCLAMP_DONE] = value;
            }
        }

        private bool FlagDetachRequestUnloadGlass
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.DETACH_REQ_UNLOAD_GLASS] = value;
            }
        }
        #endregion

        #region Private Methods
        private void GlassShuttleVacOnOff(bool onOff)
        {
            GlassShuttleVac1.Value = onOff;
            GlassShuttleVac2.Value = onOff;
            GlassShuttleVac3.Value = onOff;

#if SIMULATION
            SimulationInputSetter.SetSimInput(_devices.Inputs.DetachGlassShtVac1, onOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.DetachGlassShtVac2, onOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.DetachGlassShtVac3, onOff);

#endif
        }
        #endregion

        #region Constructor
        public DetachProcess(Devices devices,
                            CommonRecipe commonRecipe,
                            DetachRecipe detachRecipe,
                            MachineStatus machineStatus,
                            [FromKeyedServices("DetachInput")] IDInputDevice detachInput,
                            [FromKeyedServices("DetachOutput")] IDOutputDevice detachOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _detachRecipe = detachRecipe;
            _machineStatus = machineStatus;
            _detachInput = detachInput;
            _detachOutput = detachOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessToOrigin()
        {
            FlagDetachOriginDone = false;
            return base.ProcessToOrigin();
        }
        public override bool ProcessToRun()
        {
            switch ((EDetachProcessToRunStep)Step.ToRunStep)
            {
                case EDetachProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EDetachProcessToRunStep.Detach_Shuttle_Check:
                    if (IsGlassShuttleVacAll == false && (IsGlassShuttleVac1 || IsGlassShuttleVac2 || IsGlassShuttleVac3))
                    {
                        RaiseWarning(EWarning.Detach_Shuttle_Status_Fail);
                        break;
                    }
                    Log.Debug("Detach Shuttle Check Done");
                    Step.ToRunStep++;
                    break;
                case EDetachProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<EDetachProcessOutput>)_detachOutput).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case EDetachProcessToRunStep.End:
                    Log.Debug("To Run End");
                    ProcessStatus = EProcessStatus.ToRunDone;
                    Step.ToRunStep++;
                    break;
                default:
                    Thread.Sleep(20);
                    break;
            }
            return true;
        }
        public override bool ProcessOrigin()
        {
            switch ((EDetachProcessOriginStep)Step.OriginStep)
            {
                case EDetachProcessOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ZAxis_Origin:
                    Log.Debug("Detach Glass Z Axis Origin Start");
                    Log.Debug("Shuttle Transfer Z Axis Origin Start");
                    DetachGlassZAxis.SearchOrigin();
                    ShuttleTransferZAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => DetachGlassZAxis.Status.IsHomeDone && ShuttleTransferZAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (DetachGlassZAxis.Status.IsHomeDone == false)
                        {
                            RaiseAlarm((int)EAlarm.Detach_ZAxis_Origin_Fail);
                            break;
                        }
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferZAxis_Origin_Fail);
                        break;
                    }

                    Log.Debug("Detach Glass Z Axis Origin Done");
                    Log.Debug("Shuttle Transfer Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.DetachCyl_Up:
                    Log.Debug("Detach Cylinder Up");
                    DetachCyl1.Backward();
                    DetachCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => DetachCyl1.IsBackward && DetachCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.DetachCyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (DetachCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.Detach_DetachCylinder1_Up_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.Detach_DetachCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Detach Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.Cyl_Clamp_Backward:
                    Log.Debug("Cylinder Clamp Fixture Backward");
                    ClampCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderBw);
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.Cyl_Clamp_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_ClampCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Clamp Fixture Backward Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ShtTransferXAxis_Origin:
                    Log.Debug("Shuttle Transfer X Axis Origin Start");
                    ShuttleTransferXAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => ShuttleTransferXAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ShtTransferXAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferXAxis_Origin_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.End:
                    Log.Debug("Origin End");
                    FlagDetachOriginDone = true;
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
                case ESequence.TransferFixture:
                    Sequence_TransferFixture();
                    break;
                case ESequence.Detach:
                    Sequence_Detach();
                    break;
                case ESequence.DetachUnload:
                    Sequence_DetachUnload();
                    break;
                case ESequence.GlassTransferPick:
                    Sequence_GlassTransferPick();
                    break;
                default:
                    Sequence = ESequence.Stop;
                    break;
            }
            return true;
        }
        #endregion

        #region Private Methods
        private void Sequence_Ready()
        {
            switch ((EDetachReadyStep)Step.RunStep)
            {
                case EDetachReadyStep.Start:
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                case EDetachReadyStep.ZAxis_MoveReady_CylDetach_MoveBackward:
                    if (DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition) &&
                        ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition) &&
                        DetachCyl1.IsBackward && DetachCyl2.IsBackward)
                    {
                        Step.RunStep = (int)EDetachReadyStep.End;
                        break;
                    }

                    Log.Debug("Z Axis / Cylinder Move Ready Position");

                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisReadyPosition);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    DetachCyl1.Backward();
                    DetachCyl2.Backward();

                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition)
                        && ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition)
                        && DetachCyl1.IsBackward && DetachCyl2.IsBackward);

                    Step.RunStep++;
                    break;
                case EDetachReadyStep.ZAxis_MoveReady_CylDetach_MoveBackward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition) == false)
                        {
                            RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveReadyPosition_Fail);
                            break;
                        }
                        if (ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition) == false)
                        {
                            RaiseAlarm((int)EAlarm.Detach_ShuttleTransferZAxis_MoveReadyPosition_Fail);
                            break;
                        }
                        if (DetachCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.Detach_DetachCylinder1_Up_Fail);
                            break;
                        }

                        RaiseWarning((int)EWarning.Detach_DetachCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Z Axis / Cylinder Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachReadyStep.End:
                    FlagDetachReadyDone = true;
                    Log.Debug("Initialize End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((EDetachAutoRunStep)Step.RunStep)
            {
                case EDetachAutoRunStep.Start:
                    Log.Debug("AutoRun Start");
                    GlassShuttleVacOnOff(true);
                    Wait((int)(_commonRecipe.VacDelay * 1000),() => IsGlassShuttleVacAll);
                    Step.RunStep++;
                    break;
                case EDetachAutoRunStep.ShuttleTransfer_Vac_Check:
                    if (IsGlassShuttleVacAll)
                    {
                        if (_machineStatus.IsFixtureDetached)
                        {
                            Log.Debug("Fixture Detached -> Set Flag Detach Done");
                            FlagDetachDone = true;
                        }
                        Log.Info("Sequence Detach Unload");
                        Sequence = ESequence.DetachUnload;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EDetachAutoRunStep.Fixture_Detect_Check:
                    if (IsFixtureDetect && _machineStatus.IsFixtureDetached == false)
                    {
                        Log.Info("Sequence Detach");
                        Sequence = ESequence.Detach;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EDetachAutoRunStep.End:
                    Log.Info("Sequence Transfer Fixture");
                    Sequence = ESequence.TransferFixture;
                    break;
            }
        }

        private void Sequence_DetachUnload()
        {
            switch ((EDetachUnloadStep)Step.RunStep)
            {
                case EDetachUnloadStep.Start:
                    Log.Debug("Detach Unload Start");
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.ShuttleZAxis_Move_ReadyPosition:
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position");
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.ShuttleZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferZAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position Done");
                    Step.RunStep++;
                    break;

                case EDetachUnloadStep.XAxis_Move_UnloadPosition:
                    Log.Debug("Shuttle Transfer X Axis Move to Unload Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisUnloadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisUnloadPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.XAxis_Move_UnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferXAxis_MoveUnloadPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Move to Unload Position Done");
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.ShuttleZAxis_Move_UnloadPosition:
                    Log.Debug("Shuttle Transfer Z Axis Move to Unload Position");
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisUnloadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisUnloadPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.ShuttleZAxis_Move_UnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferZAxis_MoveUnloadPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer Z Axis Move to Unload Position Done");
                    Step.RunStep++;
                    break;

                case EDetachUnloadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Glass Transfer Pick");
                    Sequence = ESequence.GlassTransferPick;
                    break;
            }
        }

        private void Sequence_Detach()
        {
            switch ((EDetachStep)Step.RunStep)
            {
                case EDetachStep.Start:
                    Log.Debug("Detach Start");
                    DetachSteps = new Queue<EDetachStep>(ProcessesWorkSequence.DetachSequence);
                    Step.RunStep++;
                    break;
                case EDetachStep.StepQueue_EmptyCheck:
                    if (DetachSteps.Count <= 0)
                    {
                        Step.RunStep = (int)EDetachStep.End;
                        break;
                    }
                    Step.RunStep = (int)DetachSteps.Dequeue();
                    break;
                case EDetachStep.Cyl_Clamp_Forward:
                    Log.Debug("Clamp Cylinder Forward");
                    ClampCylinderFwBw(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderFw);
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Clamp_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_ClampCylinder_Forward_Fail);
                        break;
                    }
                    Log.Debug("Clamp Cylinder Forward Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ShuttleZAxis_Move_ReadyPosition:
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position");
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition); });
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ShuttleZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferZAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.XAxis_Move_DetachPosition:
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisDetachPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisDetachPosition); });
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.XAxis_Move_DetachPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferXAxis_MoveDetachPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetach1Position:
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach 1 Position");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetachReadyPosition1);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () =>
                        {
                            return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetachReadyPosition1) &&
                                         ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                        });
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetach1Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach 1 Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Detach Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Detach1_Down:
                    Log.Debug("Detach Cylinder 1 Down");
                    DetachCyl1.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => DetachCyl1.IsForward);
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Detach1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_DetachCylinder1_Down_Fail);
                        break;
                    }
                    Log.Debug("Detach Cylinder 1 Down Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_Detach1Position:
                    Log.Debug("Detach Glass Z Axis Move to Detach 1 Position");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 1 Position");

                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetach1Position, _detachRecipe.DetachSpeed);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetach1Position, _detachRecipe.DetachSpeed);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetach1Position)
                              && ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetach1Position));
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_Detach1Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachPosition1_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Detach 1 Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 1 Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Vacuum_On:
                    Log.Debug("Glass Shuttle Vacuum On");
                    GlassShuttleVacOnOff(true);
                    Wait((int)_commonRecipe.VacDelay * 1000);
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetach2Position:
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach 2 Position");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetachReadyPosition2);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetach1Position + 6);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () =>
                        {
                            return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetachReadyPosition2)
                                && ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetach1Position + 6);
                        });
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetach2Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach 2 Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Detach Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Detach2_Down:
                    Log.Debug("Detach Cylinder 2 Down");
                    DetachCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => DetachCyl2.IsForward);
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Detach2_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_DetachCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Detach Cylinder 2 Down Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_Detach2Position:
                    Log.Debug("Detach Glass Z Axis Move to Detach 2 Position");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 2 Position");

                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetach2Position, _detachRecipe.DetachSpeed);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetach2Position, _detachRecipe.DetachSpeed);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () =>
                    {
                        return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetach2Position) &&
                        ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetach2Position);
                    });
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_Detach2Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachPosition2_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Detach 2 Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 2 Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Detach Glass Z Axis Move Ready Position");
                    Log.Debug("Shuttle Transfer Z Axis Move Ready Position");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisReadyPosition);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () =>
                    {
                        return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition) &&
                        ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    });
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move Ready Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move Ready Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Detach_Up:
                    Log.Debug("Cylinder Detach 1 Up");
                    Log.Debug("Cylinder Detach 2 Up");
                    DetachCyl2.Backward();
                    DetachCyl1.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => DetachCyl1.IsBackward && DetachCyl2.IsBackward);
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Detach_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (DetachCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.Detach_DetachCylinder1_Up_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.Detach_DetachCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Detach 1 Up Done");
                    Log.Debug("Cylinder Detach 2 Up Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.XAxis_Move_DetachCheck_Position:
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Check Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisDetachCheckPosition,
                        ShuttleTransferXAxis.Parameter.Velocity * 0.3);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisDetachCheckPosition); });
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.XAxis_Move_DetachCheck_Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferXAxis_MoveDetachCheckPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Check Position Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Vacuum_Check:
                    if (IsGlassShuttleVacAll == false && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)EWarning.DetachFail);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.DetachGlassShtVac1, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.DetachGlassShtVac2, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.DetachGlassShtVac3, false);
#endif
                    Log.Debug("Glass Shuttle Vacuum Check Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Clamp_Backward:
                    Log.Debug("Clamp Cylinder Backward");
                    ClampCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderBw);
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Cyl_Clamp_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_ClampCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Clamp Cylinder Backward Done");
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.Set_FlagDetachDone:
                    Log.Debug("Set Flag Detach Done");
                    _machineStatus.IsFixtureDetached = true;
                    FlagDetachDone = true;
                    Step.RunStep = (int)EDetachStep.StepQueue_EmptyCheck;
                    break;
                case EDetachStep.End:
                    Log.Debug("Detach End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Detach Unload");
                    Sequence = ESequence.DetachUnload;
                    break;
            }
        }

        private void Sequence_TransferFixture()
        {
            switch ((EDetachProcessTransferFixtureLoadStep)Step.RunStep)
            {
                case EDetachProcessTransferFixtureLoadStep.Start:
                    Log.Debug("Transfer Fixture Load Start");
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.DetachZAxis_Move_ReadyPosition:
                    Log.Debug("Detach Glass Z Axis Move Ready Position");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.DetachZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Detach_Cylinder_Up:
                    Log.Debug("Detach Cylinder Up");
                    DetachCyl1.Backward();
                    DetachCyl2.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => DetachCyl1.IsBackward && DetachCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Detach_Cylinder_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (DetachCyl1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.Detach_DetachCylinder1_Up_Fail);
                            break;
                        }
                        RaiseWarning((int)EWarning.Detach_DetachCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Detach Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Set_FlagDetachDoneForSemiAutoSequence:
                    Log.Debug("Set Flag Detach Done");
                    FlagDetachDone = true;
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Wait_TransferFixtureClampDone:
                    if (FlagIn_TransferFixtureClampDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("TransferFixtureClampDone received");
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Cyl_Clamp_Backward:
                    Log.Debug("Clamp Cylinder Backward");
                    ClampCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClampCylinderBw);
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Cyl_Clamp_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_ClampCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Clamp Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Set_FlagDetachFixtureUnClampDone:
                    Log.Debug("Set Flag Detach Fixture UnClamp Done");
                    FlagOut_DetachFixtureUnClampDone = true;
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Clear_FlagDetachFixtureUnClampDone:
                    if (FlagIn_TransferFixtureClampDone)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear FlagDetachFixtureUnClampDone done");
                    FlagOut_DetachFixtureUnClampDone = false;
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Wait_FixtureTransferDone:
                    if (FlagFixtureTransferDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Clear_FlagDetachDone:
                    _machineStatus.IsFixtureDetached = false;
                    FlagDetachDone = false;
                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.Fixture_Detect_Check:
                    if (_machineStatus.FixtureExistStatus[0] == false)
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

                    Step.RunStep++;
                    break;
                case EDetachProcessTransferFixtureLoadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Sequence = ESequence.Detach;
                    break;
            }
        }

        private void Sequence_GlassTransferPick()
        {
            switch ((EDetachProcessGlassTransferPickStep)Step.RunStep)
            {
                case EDetachProcessGlassTransferPickStep.Start:
                    Log.Debug("Glass Transfer Pick Start");
                    Step.RunStep++;
                    break;
                case EDetachProcessGlassTransferPickStep.Vacuum_Off:
                    Log.Debug("Glass Shuttle Vacuum Off");
                    GlassShuttleVacOnOff(false);
                    Wait(300);
                    Step.RunStep++;
                    break;
                case EDetachProcessGlassTransferPickStep.Set_FlagDetachRequestUnload:
                    Log.Debug("Set Flag Detach Request Unload");
                    FlagDetachRequestUnloadGlass = true;
                    Step.RunStep++;
                    Log.Debug("Wait Glass Transfer Pick Done");
                    break;
                case EDetachProcessGlassTransferPickStep.Wait_GlassTransferPickDone:
                    if (FlagGlassTransferPickDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Detach Request Unload");
                    FlagDetachRequestUnloadGlass = false;
                    Step.RunStep++;
                    break;
                case EDetachProcessGlassTransferPickStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    if(_machineStatus.IsFixtureDetached == false)
                    {
                        Sequence = ESequence.Detach;
                        break;
                    }
                    Log.Info("Sequence Transfer Fixture");
                    Sequence = ESequence.TransferFixture;
                    break;
            }
        }

        #endregion
    }
}
