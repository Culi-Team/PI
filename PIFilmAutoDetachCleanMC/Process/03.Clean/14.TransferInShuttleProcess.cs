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
    public class TransferInShuttleProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly TransferInShuttleRecipe _transferInShuttleLeftRecipe;
        private readonly TransferInShuttleRecipe _transferInShuttleRightRecipe;
        private readonly IDInputDevice _transferInShuttleLeftInput;
        private readonly IDOutputDevice _transferInShuttleLeftOutput;
        private readonly IDInputDevice _transferInShuttleRightInput;
        private readonly IDOutputDevice _transferInShuttleRightOutput;
        private readonly MachineStatus _machineStatus;

        private IDInputDevice Inputs => port == EPort.Left ? _transferInShuttleLeftInput : _transferInShuttleRightInput;
        private IDOutputDevice Outputs => port == EPort.Left ? _transferInShuttleLeftOutput : _transferInShuttleRightOutput;

        private bool IsGlassDetect1 => port == EPort.Left
            ? _machineStatus.IsSatisfied(_devices.Inputs.AlignStageLGlassDetect1)
            : _machineStatus.IsSatisfied(_devices.Inputs.AlignStageRGlassDetect1);
        private bool IsGlassDetect2 => port == EPort.Left
            ? _machineStatus.IsSatisfied(_devices.Inputs.AlignStageLGlassDetect2)
            : _machineStatus.IsSatisfied(_devices.Inputs.AlignStageRGlassDetect2);
        private bool IsGlassDetect3 => port == EPort.Left
            ? _machineStatus.IsSatisfied(_devices.Inputs.AlignStageLGlassDetect3)
            : _machineStatus.IsSatisfied(_devices.Inputs.AlignStageRGlassDetect3);

        private EPort port => Name == EProcess.TransferInShuttleLeft.ToString() ? EPort.Left : EPort.Right;

        private IMotion YAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLYAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRYAxis;

        private IMotion ZAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLZAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRZAxis;

        public IDOutput GlassVac => port == EPort.Left ? _devices.Outputs.TransferInShuttleLVacOnOff
                                                        : _devices.Outputs.TransferInShuttleRVacOnOff;

        public ICylinder RotCyl => port == EPort.Left ? _devices.Cylinders.TransferInShuttleLRotate
                                                       : _devices.Cylinders.TransferInShuttleRRotate;

        private IDInput GlassVacuumInput => port == EPort.Left
            ? _devices.Inputs.TransferInShuttleLVac
            : _devices.Inputs.TransferInShuttleRVac;

        private bool IsVacDetect => _machineStatus.IsSatisfied(GlassVacuumInput);

        private double YAxisReadyPosition => port == EPort.Left ? _transferInShuttleLeftRecipe.YAxisReadyPosition
                                                                    : _transferInShuttleRightRecipe.YAxisReadyPosition;

        private double ZAxisReadyPosition => port == EPort.Left ? _transferInShuttleLeftRecipe.ZAxisReadyPosition
                                                                    : _transferInShuttleRightRecipe.ZAxisReadyPosition;

        private double YAxisPickPosition1 => port == EPort.Left ? _transferInShuttleLeftRecipe.YAxisPickPosition1
                                                                    : _transferInShuttleRightRecipe.YAxisPickPosition1;

        private double YAxisPickPosition2 => port == EPort.Left ? _transferInShuttleLeftRecipe.YAxisPickPosition2
                                                                    : _transferInShuttleRightRecipe.YAxisPickPosition2;

        private double YAxisPickPosition3 => port == EPort.Left ? _transferInShuttleLeftRecipe.YAxisPickPosition3
                                                                    : _transferInShuttleRightRecipe.YAxisPickPosition3;

        private double ZAxisPickPosition => port == EPort.Left ? _transferInShuttleLeftRecipe.ZAxisPickPosition
                                                                    : _transferInShuttleRightRecipe.ZAxisPickPosition;

        private double YAxisPlacePosition => port == EPort.Left ? _transferInShuttleLeftRecipe.YAxisPlacePosition
                                                                    : _transferInShuttleRightRecipe.YAxisPlacePosition;

        private double ZAxisPlacePosition => port == EPort.Left ? _transferInShuttleLeftRecipe.ZAxisPlacePosition
                                                                    : _transferInShuttleRightRecipe.ZAxisPlacePosition;
        #endregion

        #region Flags
        private bool FlagGlassAlignRequestPick
        {
            get
            {
                if (port == EPort.Left)
                {
                    return _transferInShuttleLeftInput[(int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK];
                }
                return _transferInShuttleRightInput[(int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK];
            }
        }

        private bool FlagWETCleanRequestLoad
        {
            get
            {
                return Inputs[(int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD];
            }
        }

        private bool FlagGlassAlignPickDoneReceived
        {
            get
            {
                return Inputs[(int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED];
            }
        }

        private bool FlagWETCleanLoadDoneReceived
        {
            get
            {
                return Inputs[(int)ETransferInShuttleProcessInput.WET_CLEAN_LOAD_DONE_RECEIVED];
            }
        }

        private bool FlagTransferInShuttlePickDone
        {
            set
            {
                Outputs[(int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_PICK_DONE] = value;
            }
        }

        private bool FlagWetCleanLoadDone
        {
            set
            {
                Outputs[(int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE] = value;
            }
        }
        #endregion

        #region Constructor
        public TransferInShuttleProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("TransferInShuttleLeftRecipe")] TransferInShuttleRecipe transferInShuttleLeftRecipe,
            [FromKeyedServices("TransferInShuttleRightRecipe")] TransferInShuttleRecipe transferInShuttleRightRecipe,
            [FromKeyedServices("TransferInShuttleLeftInput")] IDInputDevice transferInShuttleLeftInput,
            [FromKeyedServices("TransferInShuttleLeftOutput")] IDOutputDevice transferInShuttleLeftOutput,
            [FromKeyedServices("TransferInShuttleRightInput")] IDInputDevice transferInShuttleRightInput,
            [FromKeyedServices("TransferInShuttleRightOutput")] IDOutputDevice transferInShuttleRightOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _transferInShuttleLeftRecipe = transferInShuttleLeftRecipe;
            _transferInShuttleRightRecipe = transferInShuttleRightRecipe;
            _transferInShuttleLeftInput = transferInShuttleLeftInput;
            _transferInShuttleLeftOutput = transferInShuttleLeftOutput;
            _transferInShuttleRightInput = transferInShuttleRightInput;
            _transferInShuttleRightOutput = transferInShuttleRightOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ETransferInShuttleOriginStep)Step.OriginStep)
            {
                case ETransferInShuttleOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.ZAxis_Origin:
                    Log.Debug("Transfer In Shuttle Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => { return ZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_Origin_Fail : EAlarm.TransferInShuttleRight_ZAxis_Origin_Fail));
                        break;
                    }
                    Log.Debug("Transfer In Shuttle Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.YAxis_Origin:
                    Log.Debug("Transfer In Shuttle Y Axis Origin Start");
                    YAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => { return YAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_Origin_Fail : EAlarm.TransferInShuttleRight_YAxis_Origin_Fail));
                        break;
                    }
                    Log.Debug("Transfer In Shuttle Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.End:
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
                    Sequence_Ready();
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.CSTTilt:
                    break;
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    break;
                case ESequence.VinylClean:
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
                case ESequence.AlignGlassRight:
                    break;
                case ESequence.TransferInShuttleLeftPick:
                    if (port == EPort.Left)
                    {
                        Sequence_TransferInShuttlePick();
                    }
                    break;
                case ESequence.TransferInShuttleRightPick:
                    if (port == EPort.Right)
                    {
                        Sequence_TransferInShuttlePick();
                    }
                    break;
                case ESequence.WETCleanLeftLoad:
                    if (port == EPort.Left)
                    {
                        Sequence_WETCleanLoad();
                    }
                    break;
                case ESequence.WETCleanRightLoad:
                    if (port == EPort.Right)
                    {
                        Sequence_WETCleanLoad();
                    }
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanRight:
                    break;
                case ESequence.WETCleanLeftUnload:
                    break;
                case ESequence.WETCleanRightUnload:
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.TransferRotationRight:
                    break;
                case ESequence.AFCleanLeftLoad:
                    break;
                case ESequence.AFCleanRightLoad:
                    break;
                case ESequence.AFCleanLeft:
                    break;
                case ESequence.AFCleanRight:
                    break;
                case ESequence.AFCleanLeftUnload:
                    break;
                case ESequence.AFCleanRightUnload:
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    break;
                case ESequence.UnloadTransferRightPlace:
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
            switch ((ETransferInShuttleProcessToRunStep)Step.ToRunStep)
            {
                case ETransferInShuttleProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case ETransferInShuttleProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<ETransferInShuttleProcessOutput>)Outputs).Clear();
                    Step.ToRunStep++;
                    break;
                case ETransferInShuttleProcessToRunStep.End:
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
        private void Sequence_Ready()
        {
            switch ((ETransferInShuttleReadyStep)Step.RunStep)
            {
                case ETransferInShuttleReadyStep.Start:
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleReadyStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleReadyStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleReadyStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleReadyStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleReadyStep.End:
                    Log.Debug("Initialize End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((ETransferInShuttleAutoRunStep)Step.RunStep)
            {
                case ETransferInShuttleAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleAutoRunStep.GlassVac_Check:
                    if (IsVacDetect)
                    {
                        Log.Info("Sequence WET Clean Load");
                        Sequence = port == EPort.Left ? ESequence.WETCleanLeftLoad : ESequence.WETCleanRightLoad;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttleAutoRunStep.End:
                    Log.Info("Sequence Transfer In Shuttle Pick");
                    Sequence = port == EPort.Left ? ESequence.TransferInShuttleLeftPick : ESequence.TransferInShuttleRightPick;
                    break;
            }
        }

        private void Sequence_WETCleanLoad()
        {
            switch ((ETransferInShuttlePlaceStep)Step.RunStep)
            {
                case ETransferInShuttlePlaceStep.Start:
                    Log.Debug("WET Clean Load Start");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.YAxis_Move_PlacePosition:
                    Log.Debug("Y Axis Move Place Position");
                    YAxis.MoveAbs(YAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.YAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePlacePosition_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePlacePosition_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Cyl_Rotate_180D:
                    Log.Debug("Cylinder Rotate 180 Degree");
                    RotCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RotCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Cyl_Rotate_180D_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferInShuttleLeft_RotateCylinder_180D_Fail :
                                                          EWarning.TransferInShuttleRight_RotateCylinder_180D_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Rotate 180 Degree Done");
                    Log.Debug("Wait WET Clean Request Load");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Wait_WETCleanRequestLoad:
                    if (FlagWETCleanRequestLoad == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_PlacePosition:
                    Log.Debug("Z Axis Move Place Position");
                    ZAxis.MoveAbs(ZAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MovePlacePosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MovePlacePosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    GlassVac.Value = false;
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Set_FlagWETCleanLoadDone:
                    Log.Debug("Set Flag WET Clean Load Done");
                    FlagWetCleanLoadDone = true;
                    Step.RunStep++;
                    Log.Debug("Wait WET Clean Load Done Received");
                    break;
                case ETransferInShuttlePlaceStep.Wait_WETCleanPlaceDoneReceived:
                    if (FlagWETCleanLoadDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag WET Clean Load Done");
                    FlagWetCleanLoadDone = false;
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer In Shuttle Pick");
                    Sequence = port == EPort.Left ? ESequence.TransferInShuttleLeftPick : ESequence.TransferInShuttleRightPick;
                    break;
            }
        }

        private void Sequence_TransferInShuttlePick()
        {
            switch ((ETransferInShuttlePickStep)Step.RunStep)
            {
                case ETransferInShuttlePickStep.Start:
                    Log.Debug("Transfer In Shuttle Pick Start");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Cyl_Rotate_0D:
                    Log.Debug("Cylinder Rotate 0 Degree");
                    RotCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RotCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Cyl_Rotate_0D_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferInShuttleLeft_RotateCylinder_0D_Fail :
                                                          EWarning.TransferInShuttleRight_RotateCylinder_0D_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Rotate 0 Degree Done");
                    Log.Debug("Wait Align Request Pick");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Wait_GlassAlignRequest_Pick:
                    if (FlagGlassAlignRequestPick == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.GlassDetect_Check:
                    Log.Debug("Glass Detect Check");
                    if (IsGlassDetect1)
                    {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect1 : _devices.Inputs.AlignStageRGlassDetect1, false);
#endif
                        Log.Debug("Transfer In Shuttle Pick Glass 1");
                        Step.RunStep++;
                        break;
                    }
                    if (IsGlassDetect2)
                    {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect2 : _devices.Inputs.AlignStageRGlassDetect2, false);
#endif
                        Log.Debug("Transfer In Shuttle Pick Glass 2");
                        Step.RunStep = (int)ETransferInShuttlePickStep.YAxis_Move_PickPosition2;
                        break;
                    }
                    if (IsGlassDetect3)
                    {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect3 : _devices.Inputs.AlignStageRGlassDetect3, false);
#endif
                        Log.Debug("Transfer In Shuttle Pick Glass 3");
                        Step.RunStep = (int)ETransferInShuttlePickStep.YAxis_Move_PickPosition3;
                        break;
                    }
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition1:
                    Log.Debug("Y Axis Move Pick Position 1");
                    YAxis.MoveAbs(YAxisPickPosition1);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPickPosition1));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition1_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePickPosition1_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePickPosition1_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position 1 Done");
                    Step.RunStep = (int)ETransferInShuttlePickStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition2:
                    Log.Debug("Y Axis Move Pick Position 2");
                    YAxis.MoveAbs(YAxisPickPosition2);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPickPosition2));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition2_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePickPosition2_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePickPosition2_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position 2 Done");
                    Step.RunStep = (int)ETransferInShuttlePickStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition3:
                    Log.Debug("Y Axis Move Pick Position 3");
                    YAxis.MoveAbs(YAxisPickPosition3);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPickPosition3));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition3_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePickPosition3_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePickPosition3_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position 3 Done");
                    Step.RunStep = (int)ETransferInShuttlePickStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_PickPosition:
                    Log.Debug("Z Axis Move Pick Position");
                    ZAxis.MoveAbs(ZAxisPickPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisPickPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_PickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MovePickPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MovePickPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    GlassVac.Value = true;
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => GlassVac.Value || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Time out
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Set_FlagTransferInShuttlePickDone:
                    Log.Debug("Set Flag Transfer In Shuttle Pick Done");
                    FlagTransferInShuttlePickDone = true;
                    Log.Debug("Wait Glass Align Pick Done Received");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Wait_GlassAlignPickDoneReceived:
                    if (FlagGlassAlignPickDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Transfer In Shuttle Pick Done");
                    FlagTransferInShuttlePickDone = false;
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Sequence WET Clean Load");
                    Sequence = port == EPort.Left ? ESequence.WETCleanLeftLoad : ESequence.WETCleanRightLoad;
                    break;
            }
        }
        #endregion
    }
}
