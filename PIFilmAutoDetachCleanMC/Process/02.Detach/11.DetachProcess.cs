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
    public class DetachProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly DetachRecipe _detachRecipe;
        private readonly IDInputDevice _detachInput;
        private readonly IDOutputDevice _detachOutput;

        private IMotion DetachGlassZAxis => _devices.MotionsInovance.DetachGlassZAxis;
        private IMotion ShuttleTransferXAxis => _devices.MotionsInovance.ShuttleTransferXAxis;
        private IMotion ShuttleTransferZAxis => _devices.MotionsAjin.ShuttleTransferZAxis;

        private ICylinder DetachFixFixtureCyl1 => _devices.Cylinders.DetachFixFixtureCyl1FwBw;
        private ICylinder DetachFixFixtureCyl2 => _devices.Cylinders.DetachFixFixtureCyl2FwBw;

        private ICylinder DetachCyl1 => _devices.Cylinders.DetachCyl1UpDown;
        private ICylinder DetachCyl2 => _devices.Cylinders.DetachCyl2UpDown;

        private ICylinder FixCyl1 => _devices.Cylinders.DetachFixFixtureCyl1FwBw;
        private ICylinder FixCyl2 => _devices.Cylinders.DetachFixFixtureCyl2FwBw;

        private bool IsFixtureDetect => _devices.Inputs.DetachFixtureDetect.Value;

        private bool IsGlassShuttleVac1 => _devices.Inputs.DetachGlassShtVac1.Value;
        private bool IsGlassShuttleVac2 => _devices.Inputs.DetachGlassShtVac2.Value;
        private bool IsGlassShuttleVac3 => _devices.Inputs.DetachGlassShtVac3.Value;
        private bool IsGlassShuttleVacAll => IsGlassShuttleVac1 && IsGlassShuttleVac2 && IsGlassShuttleVac3;

        private IDOutput GlassShuttleVac1 => _devices.Outputs.DetachGlassShtVac1OnOff;
        private IDOutput GlassShuttleVac2 => _devices.Outputs.DetachGlassShtVac2OnOff;
        private IDOutput GlassShuttleVac3 => _devices.Outputs.DetachGlassShtVac3OnOff;
        #endregion

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
        }
        #endregion

        #region Constructor
        public DetachProcess(Devices devices,
                            CommonRecipe commonRecipe,
                            DetachRecipe detachRecipe,
                            [FromKeyedServices("DetachInput")] IDInputDevice detachInput,
                            [FromKeyedServices("DetachOutput")] IDOutputDevice detachOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _detachRecipe = detachRecipe;
            _detachInput = detachInput;
            _detachOutput = detachOutput;
        }
        #endregion

        #region Override Methods
        public override bool PreProcess()
        {
            if(FlagFixtureTransferDone)
            {
                Log.Debug("Clear Flag Detach Done");
                FlagDetachDone = false;
            }

            return base.PreProcess();
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
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return DetachGlassZAxis.Status.IsHomeDone && ShuttleTransferZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return DetachCyl1.IsBackward && DetachCyl2.IsBackward; });
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.DetachCyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.Cyl_Fix_Backward:
                    Log.Debug("Cylinder Fix Fixture Backward");
                    FixCyl1.Backward();
                    FixCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCyl1.IsBackward && FixCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Fix Fixture Backward Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ShtTransferXAxis_Origin:
                    Log.Debug("Shuttle Transfer X Axis Origin Start");
                    ShuttleTransferXAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return ShuttleTransferXAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ShtTransferXAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                case ESequence.AlignGlass:
                    break;
                case ESequence.TransferInShuttlePick:
                    break;
                case ESequence.WETCleanLoad:
                    break;
                case ESequence.WETClean:
                    break;
                case ESequence.WETCleanUnload:
                    break;
                case ESequence.TransferRotation:
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
            }
            return true;
        }
        #endregion

        #region Private Methods
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
                    Step.RunStep++;
                    break;
                case EDetachAutoRunStep.Fixture_Detect_Check:
                    if (IsFixtureDetect)
                    {
                        Log.Info("Sequence Detach");
                        Sequence = ESequence.Detach;
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
                    Wait(_commonRecipe.MotionMoveTimeOut,
                        () => { return ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.ShuttleZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.XAxis_Move_DetachCheck_Position:
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Check Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisDetachCheckPosition,
                        ShuttleTransferXAxis.Parameter.Velocity * 0.1);
                    Wait(_commonRecipe.MotionMoveTimeOut,
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisDetachCheckPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.XAxis_Move_DetachCheck_Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.DetachGlassShtVac1, false);
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.DetachGlassShtVac2, false);
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.DetachGlassShtVac3, false);
#endif
                    Log.Debug("Glass Shuttle Vacuum Check Done");
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.XAxis_Move_UnloadPosition:
                    Log.Debug("Shuttle Transfer X Axis Move to Unload Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisUnloadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisUnloadPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.XAxis_Move_UnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Move to Unload Position Done");
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.ShuttleZAxis_Move_UnloadPosition:
                    Log.Debug("Shuttle Transfer Z Axis Move to Unload Position");
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisUnloadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,
                        () => { return ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisUnloadPosition); });
                    Step.RunStep++;
                    break;
                case EDetachUnloadStep.ShuttleZAxis_Move_UnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Shuttle Transfer Z Axis Move to Unload Position Done");
                    Step.RunStep++;
                    break;

                case EDetachUnloadStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
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
                    Log.Debug("Detach Fix Cylinder 1 Forward");
                    Log.Debug("Detach Fix Cylinder 2 Forward");
                    DetachFixFixtureCyl1.Forward();
                    DetachFixFixtureCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () =>
                    {
                        return DetachFixFixtureCyl1.IsForward && DetachFixFixtureCyl2.IsForward;
                    });
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Fix_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Fix Cylinder 1 Forward Done");
                    Log.Debug("Detach Fix Cylinder 2 Forward Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.ShuttleZAxis_Move_ReadyPosition:
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position");
                    ShuttleTransferZAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,
                        () => { return ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition); });
                    Step.RunStep++;
                    break;
                case EDetachStep.ShuttleZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.XAxis_Move_DetachPosition:
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Position");
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferXAxisDetachPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,
                        () => { return ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferXAxisDetachPosition); });
                    Step.RunStep++;
                    break;
                case EDetachStep.XAxis_Move_DetachPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Move to Detach Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetachPosition_1st:
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 1st");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetachReadyPosition);
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,
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
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 1st Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Detach Position 1st Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach1_Down:
                    Log.Debug("Detach Cylinder 1 Down");
                    DetachCyl1.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return DetachCyl1.IsForward; });
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    Wait(_commonRecipe.MotionMoveTimeOut,
                        () =>
                        {
                            return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetach1Position) &&
                                         ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetach1Position);
                        });
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_Detach1Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Detach 1 Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 1 Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Vacuum_On:
                    Log.Debug("Glass Shuttle Vacuum On");
                    GlassShuttleVacOnOff(true);
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.DetachGlassShtVac1, true);
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.DetachGlassShtVac2, true);
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.DetachGlassShtVac3, true);

#endif
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyDetachPosition_2nd:
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 2nd");
                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetachReadyPosition);
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetachReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,
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
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move to Ready Detach Position 2nd Done");
                    Log.Debug("Shuttle Transfer Z Axis Move to Ready Detach Position 2nd Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach2_Down:
                    Log.Debug("Detach Cylinder 2 Down");
                    DetachCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return DetachCyl2.IsForward; });
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Detach2_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Cylinder 2 Down Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_Detach2Position:
                    Log.Debug("Detach Glass Z Axis Move to Detach 2 Position");
                    Log.Debug("Shuttle Transfer Z Axis Move to Detach 2 Position");

                    DetachGlassZAxis.MoveAbs(_detachRecipe.DetachZAxisDetach2Position);
                    ShuttleTransferXAxis.MoveAbs(_detachRecipe.ShuttleTransferZAxisDetach2Position);
                    Wait(_commonRecipe.MotionMoveTimeOut, () =>
                    {
                        return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisDetach2Position) &&
                        ShuttleTransferXAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisDetach2Position);
                    });
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_Detach2Position_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    Wait(_commonRecipe.MotionMoveTimeOut, () =>
                    {
                        return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition) &&
                        ShuttleTransferZAxis.IsOnPosition(_detachRecipe.ShuttleTransferZAxisReadyPosition);
                    });
                    Step.RunStep++;
                    break;
                case EDetachStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move Ready Position Done");
                    Log.Debug("Shuttle Transfer Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Fix_Backward:
                    Log.Debug("Detach Fix Cylinder 1 Backward");
                    Log.Debug("Detach Fix Cylinder 2 Backward");
                    DetachFixFixtureCyl1.Backward();
                    DetachFixFixtureCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () =>
                    {
                        return DetachFixFixtureCyl1.IsBackward && DetachFixFixtureCyl2.IsBackward;
                    });
                    Step.RunStep++;
                    break;
                case EDetachStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Fix Cylinder 1 Backward Done");
                    Log.Debug("Detach Fix Cylinder 2 Backward Done");
                    Step.RunStep++;
                    break;
                case EDetachStep.Set_FlagDetachDone:
                    Log.Debug("Set Flag Detach Done");
                    FlagDetachDone = true;
                    Step.RunStep++;
                    break;
                case EDetachStep.End:
                    Log.Debug("Detach End");
                    if (Parent!.Sequence != ESequence.AutoRun)
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
                    Wait(_commonRecipe.MotionMoveTimeOut, () =>
                    {
                        return DetachGlassZAxis.IsOnPosition(_detachRecipe.DetachZAxisReadyPosition);
                    });
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.DetachedZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.Cyl_Fix_Backward:
                    Log.Debug("Detach Fix Cylinder 1 Backward");
                    Log.Debug("Detach Fix Cylinder 2 Backward");
                    DetachFixFixtureCyl1.Backward();
                    DetachFixFixtureCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return DetachFixFixtureCyl1.IsBackward && DetachFixFixtureCyl2.IsBackward; });
                    Step.RunStep++;
                    break;
                case EDetachProcessFixtureTransferStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Fix Cylinder 1 Backward Done");
                    Log.Debug("Detach Fix Cylinder 2 Backward Done");
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
                    if (Parent!.Sequence != ESequence.AutoRun)
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
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case EDetachProcessGlassTransferPickStep.Set_FlagDetachRequestUnload:
                    Log.Debug("Set Flag Detach Request Unload");
                    FlagDetachRequestUnloadGlass = true;
                    Step.RunStep++;
                    Log.Debug("Wait Glass Transfer Pick Done");
                    break;
                case EDetachProcessGlassTransferPickStep.Wait_GlassTransferPickDone:
                    if(FlagGlassTransferPickDone == false)
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
                    if (Parent!.Sequence != ESequence.AutoRun)
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
