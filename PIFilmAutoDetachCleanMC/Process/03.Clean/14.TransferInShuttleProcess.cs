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

        private bool IsGlassDetect1 => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect1.Value 
                                                            : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool IsGlassDetect2 => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect2.Value
                                                            : _devices.Inputs.AlignStageRGlassDetect2.Value;
        private bool IsGlassDetect3 => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect3.Value
                                                            : _devices.Inputs.AlignStageRGlassDetect3.Value;

        private EPort port => Name == EProcess.TransferInShuttleLeft.ToString() ? EPort.Left : EPort.Right;

        private IMotion YAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLYAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRYAxis;

        private IMotion ZAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLZAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRZAxis;

        public IDOutput GlassVac => port == EPort.Left ? _devices.Outputs.TransferInShuttleLVacOnOff
                                                        : _devices.Outputs.TransferInShuttleRVacOnOff;

        private bool IsVacDetect => port == EPort.Left ? _devices.Inputs.TransferInShuttleLVac.Value
                                                        : _devices.Inputs.TransferInShuttleRVac.Value;

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
                if(port == EPort.Left)
                {
                    return _transferInShuttleLeftInput[(int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD];
                }
                return _transferInShuttleRightInput[(int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD];
            }
        }

        private bool FlagGlassAlignPickDoneReceived
        {
            get
            {
                if (port == EPort.Left)
                {
                    return _transferInShuttleLeftInput[(int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED];
                }
                return _transferInShuttleRightInput[(int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED];
            }
        }

        private bool FlagWETCleanLoadDoneReceived
        {
            get
            {
                if (port == EPort.Left)
                {
                    return _transferInShuttleLeftInput[(int)ETransferInShuttleProcessInput.WET_CLEAN_LOAD_DONE_RECEIVED];
                }
                return _transferInShuttleRightInput[(int)ETransferInShuttleProcessInput.WET_CLEAN_LOAD_DONE_RECEIVED];
            }
        }

        private bool FlagTransferInShuttlePickDone
        {
            set
            {
                if (port == EPort.Left)
                {
                    _transferInShuttleLeftOutput[(int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_PICK_DONE] = value;
                }
                else
                {
                    _transferInShuttleRightOutput[(int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_PICK_DONE] = value;
                }
            }
        }

        private bool FlagWetCleanLoadDone
        {
            set
            {
                if (port == EPort.Left)
                {
                    _transferInShuttleLeftOutput[(int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE] = value;
                }
                else
                {
                    _transferInShuttleRightOutput[(int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE] = value;
                }
            }
        }
        #endregion

            #region Constructor
        public TransferInShuttleProcess(Devices devices,
            CommonRecipe commonRecipe,
            [FromKeyedServices("TransferInShuttleLeftRecipe")] TransferInShuttleRecipe transferInShuttleLeftRecipe,
            [FromKeyedServices("TransferInShuttleRightRecipe")] TransferInShuttleRecipe transferInShuttleRightRecipe,
            [FromKeyedServices("TransferInShuttleLeftInput")] IDInputDevice transferInShuttleLeftInput,
            [FromKeyedServices("TransferInShuttleLeftOutput")] IDOutputDevice transferInShuttleLeftOutput,
            [FromKeyedServices("TransferInShuttleRightInput")] IDInputDevice transferInShuttleRightInput,
            [FromKeyedServices("TransferInShuttleRightOutput")] IDOutputDevice transferInShuttleRightOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
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
            switch((ETransferInShuttleOriginStep)Step.OriginStep)
            {
                case ETransferInShuttleOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.ZAxis_Origin:
                    Log.Debug("Transfer In Shuttle Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return ZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer In Shuttle Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.YAxis_Origin:
                    Log.Debug("Transfer In Shuttle Y Axis Origin Start");
                    YAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return YAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.CSTTilt:
                    break;
                case ESequence.CSTUnTilt:
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
                case ESequence.AlignGlass:
                    break;
                case ESequence.TransferInShuttlePick:
                    Sequence_TransferInShuttlePick();
                    break;
                case ESequence.WETCleanLoad:
                    Sequence_WETCleanLoad();
                    break;
                case ESequence.WETClean:
                    break;
                case ESequence.WETCleanUnload:
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
            }

            return true;
        }
        #endregion

        #region Private Methods
        private void Sequence_AutoRun()
        {
            switch ((ETransferInShuttleAutoRunStep)Step.RunStep)
            {
                case ETransferInShuttleAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleAutoRunStep.GlassVac_Check:
                    if(IsVacDetect)
                    {
                        Log.Info("Sequence WET Clean Load");
                        Sequence = ESequence.WETCleanLoad;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttleAutoRunStep.End:
                    Log.Info("Sequence Transfer In Shuttle Pick");
                    Sequence = ESequence.TransferInShuttlePick;
                    break;
            }
        }

        private void Sequence_WETCleanLoad()
        {
            switch ((ETransferInShuttlePlaceStep)Step.RunStep)
            {
                case ETransferInShuttlePlaceStep.Start:
                    Log.Debug("Transfer In Shuttle Place Start");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.YAxis_Move_PlacePosition:
                    Log.Debug("Y Axis Move Place Position");
                    YAxis.MoveAbs(YAxisPlacePosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => YAxis.IsOnPosition(YAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.YAxis_Move_PlacePosition_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Place Position Done");

                    Log.Debug("Wait WET Clean Request Load");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Wait_WETCleanRequestLoad:
                    if(FlagWETCleanRequestLoad == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_PlacePosition:
                    Log.Debug("Z Axis Move Place Position");
                    ZAxis.MoveAbs(ZAxisPlacePosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => ZAxis.IsOnPosition(ZAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_PlacePosition_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    GlassVac.Value = false;
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.Set_Flag_TransferInShuttlePlaceDone:
                    Log.Debug("Set Flag WET Clean Load Done");
                    FlagWetCleanLoadDone = true;
                    Step.RunStep++;
                    Log.Debug("Wait WET Clean Load Done Received");
                    break;
                case ETransferInShuttlePlaceStep.Wait_WETCleanPlaceDoneReceived:
                    if(FlagWETCleanLoadDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag WET Clean Load Done");
                    FlagWetCleanLoadDone = false;
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePlaceStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer In Shuttle Pick");
                    Sequence = ESequence.TransferInShuttlePick;
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
                    Log.Debug("Wait Align Request Pick");
                    break;
                case ETransferInShuttlePickStep.Wait_GlassAlignRequest_Pick:
                    if(FlagGlassAlignRequestPick == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.GlassDetect_Check:
                    Log.Debug("Glass Detect Check");
                    if(IsGlassDetect1)
                    {
#if SIMULATION
                        SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect1 : _devices.Inputs.AlignStageRGlassDetect1, false);
#endif
                        Log.Debug("Transfer In Shuttle Pick Glass 1");
                        Step.RunStep++;
                        break;
                    }
                    if(IsGlassDetect2)
                    {
#if SIMULATION
                        SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect2 : _devices.Inputs.AlignStageRGlassDetect2, false);
#endif
                        Log.Debug("Transfer In Shuttle Pick Glass 2");
                        Step.RunStep = (int)ETransferInShuttlePickStep.YAxis_Move_PickPosition2;
                        break;
                    }
                    if(IsGlassDetect3)
                    {
#if SIMULATION
                        SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect3 : _devices.Inputs.AlignStageRGlassDetect3, false);
#endif
                        Log.Debug("Transfer In Shuttle Pick Glass 3");
                        Step.RunStep = (int)ETransferInShuttlePickStep.YAxis_Move_PickPosition3;
                        break;
                    }
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition1:
                    Log.Debug("Y Axis Move Pick Position 1");
                    YAxis.MoveAbs(YAxisPickPosition1);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => YAxis.IsOnPosition(YAxisPickPosition1));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition1_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position 1 Done");
                    Step.RunStep = (int)ETransferInShuttlePickStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition2:
                    Log.Debug("Y Axis Move Pick Position 2");
                    YAxis.MoveAbs(YAxisPickPosition2);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => YAxis.IsOnPosition(YAxisPickPosition2));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition2_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position 2 Done");
                    Step.RunStep = (int)ETransferInShuttlePickStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition3:
                    Log.Debug("Y Axis Move Pick Position 3");
                    YAxis.MoveAbs(YAxisPickPosition3);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => YAxis.IsOnPosition(YAxisPickPosition3));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_PickPosition3_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position 3 Done");
                    Step.RunStep = (int)ETransferInShuttlePickStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_PickPosition:
                    Log.Debug("Z Axis Move Pick Position");
                    ZAxis.MoveAbs(ZAxisPickPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => ZAxis.IsOnPosition(ZAxisPickPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_PickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    GlassVac.Value = true;
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(YAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => YAxis.IsOnPosition(YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    if(FlagGlassAlignPickDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Transfer In Shuttle Pick Done");
                    FlagTransferInShuttlePickDone = false;
                    Step.RunStep++;
                    break;
                case ETransferInShuttlePickStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Sequence WET Clean Load");
                    Sequence = ESequence.WETCleanLoad;
                    break;
            }
        }
        #endregion
    }
}
