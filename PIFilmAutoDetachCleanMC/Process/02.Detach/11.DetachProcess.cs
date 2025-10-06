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
using PIFilmAutoDetachCleanMC.Services.DryRunServices;
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
        private IMotion DetachGlassZAxis => _devices.MotionsInovance.DetachGlassZAxis;
        private IMotion ShuttleTransferXAxis => _devices.MotionsInovance.ShuttleTransferXAxis;
        private IMotion ShuttleTransferZAxis => _devices.MotionsAjin.ShuttleTransferZAxis;

        private ICylinder FixCyl1_1 => _devices.Cylinders.DetachFixFixtureCyl1_1FwBw;
        private ICylinder FixCyl1_2 => _devices.Cylinders.DetachFixFixtureCyl1_2FwBw;
        private ICylinder FixCyl2_1 => _devices.Cylinders.DetachFixFixtureCyl2_1FwBw;
        private ICylinder FixCyl2_2 => _devices.Cylinders.DetachFixFixtureCyl2_2FwBw;

        private ICylinder DetachCyl1 => _devices.Cylinders.DetachCyl1UpDown;
        private ICylinder DetachCyl2 => _devices.Cylinders.DetachCyl2UpDown;

        private bool IsFixtureDetect => _machineStatus.IsSatisfied(_devices.Inputs.DetachFixtureDetect);

        private bool IsGlassShuttleVac1 => _machineStatus.IsSatisfied(_devices.Inputs.DetachGlassShtVac1);
        private bool IsGlassShuttleVac2 => _machineStatus.IsSatisfied(_devices.Inputs.DetachGlassShtVac2);
        private bool IsGlassShuttleVac3 => _machineStatus.IsSatisfied(_devices.Inputs.DetachGlassShtVac3);
        private bool IsGlassShuttleVacAll => IsGlassShuttleVac1 && IsGlassShuttleVac2 && IsGlassShuttleVac3;

        private IDOutput GlassShuttleVac1 => _devices.Outputs.DetachGlassShtVac1OnOff;
        private IDOutput GlassShuttleVac2 => _devices.Outputs.DetachGlassShtVac2OnOff;
        private IDOutput GlassShuttleVac3 => _devices.Outputs.DetachGlassShtVac3OnOff;

        private bool IsFixCylinderFw => FixCyl1_1.IsForward && FixCyl1_2.IsForward && FixCyl2_1.IsForward && FixCyl2_2.IsForward;
        private bool IsFixCylinderBw => FixCyl1_1.IsBackward && FixCyl1_2.IsBackward && FixCyl2_1.IsBackward && FixCyl2_2.IsBackward;
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
        #region Flags
        private bool FlagOriginDone
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.DETACH_ORIGIN_DONE] = value;
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

        private bool FlagDetachRequestUnloadGlass
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.DETACH_REQ_UNLOAD_GLASS] = value;
            }
        }

        private bool FlagTransferFixtureDoneReceived
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED] = value;
            }
        }

        private bool FlagDetachGlassTransferPickDoneReceived
        {
            set
            {
                _detachOutput[(int)EDetachProcessOutput.GLASS_TRANSFER_PICK_DONE_RECEIVED] = value;
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
            FlagOriginDone = false;
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
                case EDetachProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<EDetachProcessOutput>)_detachOutput).Clear();
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
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => { return DetachGlassZAxis.Status.IsHomeDone && ShuttleTransferZAxis.Status.IsHomeDone; });
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
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return DetachCyl1.IsBackward && DetachCyl2.IsBackward; });
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
                case EDetachProcessOriginStep.Cyl_Fix_Backward:
                    Log.Debug("Cylinder Fix Fixture Backward");
                    FixCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixCylinderBw);
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_FixCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Fix Fixture Backward Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ShtTransferXAxis_Origin:
                    Log.Debug("Shuttle Transfer X Axis Origin Start");
                    ShuttleTransferXAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => { return ShuttleTransferXAxis.Status.IsHomeDone; });
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
                    FlagOriginDone = true;
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
                    Sequence_Detach();
                    break;
                case ESequence.TransferFixtureUnload:
                    Sequence_TransferFixtureUnload();
                    break;
                case ESequence.DetachUnload:
                    Sequence_DetachUnload();
                    break;
                case ESequence.RemoveFilm:
                    break;
                case ESequence.GlassTransferPick:
                    Sequence_GlassTransferPick();
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
        #endregion

        #region Private Methods
        private void Sequence_Ready()
        {
            switch ((EDetachReadyStep)Step.RunStep)
            {
                case EDetachReadyStep.Start:
                    if (IsOriginOrInitSelected == false)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                case EDetachReadyStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisReadyPosition);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition) &&
                                                               ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EDetachReadyStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if(DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition) == false)
                        {
                            RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveReadyPosition_Fail);
                            break;
                        }
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferZAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachReadyStep.End:
                    IsWarning = false;
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
                    Step.RunStep++;
                    break;
                case EDetachAutoRunStep.ShuttleTransfer_Vac_Check:
                    if (IsGlassShuttleVacAll)
                    {
                        Log.Info("Sequence Detach Unload");
                        Sequence = ESequence.DetachUnload;
                        break;
                    }
                    if (_machineStatus.IsDryRunMode)
                    {
                        Log.Info("Dry Run Mode Skip Shuttle Vacuum Check");
                        Step.RunStep = (int)EDetachAutoRunStep.Fixture_Detect_Check;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EDetachAutoRunStep.Fixture_Detect_Check:
                    if (IsFixtureDetect)
                    {
                        Log.Info("Sequence Detach");
                        Sequence = ESequence.Detach;
                        break;
                    }
                    if (_machineStatus.IsDryRunMode)
                    {
                        Log.Info("Dry Run Mode Skip Fixture Detect Check");
                        Step.RunStep = (int)EDetachAutoRunStep.End;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EDetachAutoRunStep.End:
                    Log.Info("Sequence Transfer Fixture Unload");
                    Sequence = ESequence.TransferFixtureUnload;
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
                case EDetachUnloadStep.XAxis_Move_DetachCheck_Position:
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Check Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisDetachCheckPosition,
                        ShuttleTransferXAxis.Parameter.Velocity * 0.1);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisDetachCheckPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.XAxis_Move_DetachCheck_Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferXAxis_MoveDetachCheckPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Check Position Done");
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.Vacuum_Check:
                    if (!IsGlassShuttleVacAll)
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
                        Parent.ProcessMode = EProcessMode.ToStop;
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
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Fix_Forward:
                    Log.Debug("Fix Cylinder Forward");
                    FixCylinderFwBw(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixCylinderFw);
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Fix_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_FixCylinder_Forward_Fail);
                        break;
                    }
                    Log.Debug("Fix Cylinder Forward Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.ShuttleZAxis_Move_ReadyPosition:
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position");
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition); });
                    Step.RunStep++;
                    break;
                case EDetachStep.ShuttleZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferZAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.XAxis_Move_DetachPosition:
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisDetachPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisDetachPosition); });
                    Step.RunStep++;
                    break;
                case EDetachStep.XAxis_Move_DetachPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ShuttleTransferXAxis_MoveDetachPosition_Fail);
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetachPosition_1st:
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 1st");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetachReadyPosition);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () =>
                        {
                            return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetachReadyPosition) &&
                                         ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                        });
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetachPosition_Wait_1st:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 1st Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Detach Position 1st Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach1_Down:
                    Log.Debug("Detach Cylinder 1 Down");
                    DetachCyl1.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return DetachCyl1.IsForward; });
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_DetachCylinder1_Down_Fail);
                        break;
                    }
                    Log.Debug("Detach Cylinder 1 Down Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_Detach1Position:
                    Log.Debug("Detach Glass Z Axis Move to Detach 1 Position");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 1 Position");

                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetach1Position);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetach1Position);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetach1Position)
                              && ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetach1Position));
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_Detach1Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachPosition1_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Detach 1 Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 1 Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Vacuum_On:
                    Log.Debug("Glass Shuttle Vacuum On");
                    GlassShuttleVacOnOff(true);
                    Wait((int)_commonRecipe.VacDelay * 1000, () => IsGlassShuttleVacAll || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EDetachStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetachPosition_2nd:
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 2nd");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetachReadyPosition);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () =>
                        {
                            return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetachReadyPosition) &&
                                         ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                        });
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetachPosition_Wait_2nd:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 2nd Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Detach Position 2nd Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach2_Down:
                    Log.Debug("Detach Cylinder 2 Down");
                    DetachCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return DetachCyl2.IsForward; });
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach2_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_DetachCylinder2_Down_Fail);
                        break;
                    }
                    Log.Debug("Detach Cylinder 2 Down Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_Detach2Position:
                    Log.Debug("Detach Glass Z Axis Move to Detach 2 Position");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 2 Position");

                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetach2Position);
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetach2Position);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () =>
                    {
                        return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetach2Position) &&
                        ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetach2Position);
                    });
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_Detach2Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachPosition2_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Detach 2 Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 2 Position Done");
                    Step.RunStep++;
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
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move Ready Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach1_Up:
                    Log.Debug("Cylinder Detach 1 Up");
                    DetachCyl1.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => DetachCyl1.IsBackward);
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_DetachCylinder1_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Detach 1 Up Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach2_Up:
                    Log.Debug("Cylinder Detach 2 Up");
                    DetachCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => DetachCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach2_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_DetachCylinder2_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Detach 2 Up Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Fix_Backward:
                    Log.Debug("Detach Fix Cylinder Backward");
                    FixCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixCylinderBw);
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_FixCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Detach Fix Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Set_FlagDetachDone:
                    Log.Debug("Set Flag Detach Done");
                    FlagDetachDone = true;
                    Step.RunStep++;
                    break;
                case EDetachStep.End:
                    Log.Debug("Detach End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Sequence Detach Unload");
                    Sequence = ESequence.DetachUnload;
                    break;
            }
        }

        private void Sequence_TransferFixtureUnload()
        {
            switch ((EDetachProcessFixtureTransferStep)Step.RunStep)
            {
                case EDetachProcessFixtureTransferStep.Start:
                    Log.Debug("Transfer Fixture Start");
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.Clear_Flag:
                    Log.Debug("Clear Flag Transfer Fixture Done Received");
                    FlagTransferFixtureDoneReceived = false;
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.DetachZAxis_Move_ReadyPosition:
                    Log.Debug("Detach Glass Z Axis Move Ready Position");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () =>
                    {
                        return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition);
                    });
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.DetachedZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.Detach_ZAxis_MoveDetachReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.Cyl_Fix_Backward:
                    Log.Debug("Detach Fix Cylinder Backward");
                    FixCylinderFwBw(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsFixCylinderBw);
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.Detach_FixCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Detach Fix Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.SetFlagDetachDone:
                    FlagDetachDone = true;
                    Log.Debug("Wait Fixture Transfer Done");
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.Wait_FixtureTransferDone:
                    if (FlagFixtureTransferDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear Flag Detach Done");
                    FlagDetachDone = false;

                    Log.Debug("Set Flag Transfer Fixture Done Received");
                    FlagTransferFixtureDoneReceived = true;
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Detach");
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
                    Log.Debug("Clear Flag Detach Glass Transfer Pick Done Received");
                    FlagDetachGlassTransferPickDoneReceived = false;
                    Step.RunStep++;
                    break;
                case EDetachProcessGlassTransferPickStep.Vacuum_Off:
                    Log.Debug("Glass Shuttle Vacuum Off");
                    GlassShuttleVacOnOff(false);
                    Wait((int)(_commonRecipe.VacDelay * 1000));
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

                    Log.Debug("Set Flag Detach Glass Transfer Pick Done Received");
                    FlagDetachGlassTransferPickDoneReceived = true;
                    Step.RunStep++;
                    break;
                case EDetachProcessGlassTransferPickStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer Fixture Unload");
                    Sequence = ESequence.TransferFixtureUnload;
                    break;
            }
        }

        #endregion
    }
}
