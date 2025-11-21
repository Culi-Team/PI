using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;
using PIFilmAutoDetachCleanMC.Recipe;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class UnloadTransferProcess : ProcessBase<ESequence>
    {
        #region Privates
        private EPort port => Name == EProcess.UnloadTransferLeft.ToString() ? EPort.Left : EPort.Right;

        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly UnloadTransferRecipe _unloadTransferLeftRecipe;
        private readonly UnloadTransferRecipe _unloadTransferRightRecipe;
        private readonly IDInputDevice _unloadTransferLeftInput;
        private readonly IDOutputDevice _unloadTransferLeftOutput;
        private readonly IDInputDevice _unloadTransferRightInput;
        private readonly IDOutputDevice _unloadTransferRightOutput;
        private readonly CWorkData _workData;
        private readonly MachineStatus _machineStatus;

        private int UnloadAlignCurrentIndex { get; set; } = -1;

        private IMotion YAxis => port == EPort.Left ? _devices.Motions.GlassUnloadLYAxis :
                                                  _devices.Motions.GlassUnloadRYAxis;

        private IMotion ZAxis => port == EPort.Left ? _devices.Motions.GlassUnloadLZAxis :
                                                  _devices.Motions.GlassUnloadRZAxis;

        private IDOutput GlassBlowOnOff => port == EPort.Left ? _devices.Outputs.UnloadTransferLBlowOnOff :
                                                  _devices.Outputs.UnloadTransferRBlowOnOff;
        private IDOutput GlassVacOnOff => port == EPort.Left ? _devices.Outputs.UnloadTransferLVacOnOff :
                                                  _devices.Outputs.UnloadTransferRVacOnOff;

        private IDInput GlassVac => port == EPort.Left ? _devices.Inputs.UnloadTransferLVac :
                                                         _devices.Inputs.UnloadTransferRVac;

        private bool UnloadAlignVac1 => _devices.Inputs.UnloadGlassAlignVac1.Value;
        private bool UnloadAlignVac2 => _devices.Inputs.UnloadGlassAlignVac2.Value;
        private bool UnloadAlignVac3 => _devices.Inputs.UnloadGlassAlignVac3.Value;
        private bool UnloadAlignVac4 => _devices.Inputs.UnloadGlassAlignVac4.Value;

        private bool IsVacDetect => GlassVac.Value;

        private UnloadTransferRecipe Recipe => port == EPort.Left ? _unloadTransferLeftRecipe :
                                                    _unloadTransferRightRecipe;

        private IDInput[] UnloadAlignGlassVac => new IDInput[] { _devices.Inputs.UnloadGlassAlignVac1 ,
                                                                 _devices.Inputs.UnloadGlassAlignVac2,
                                                                 _devices.Inputs.UnloadGlassAlignVac3,
                                                                 _devices.Inputs.UnloadGlassAlignVac4};

        private IDInputDevice Inputs => port == EPort.Left ? _unloadTransferLeftInput : _unloadTransferRightInput;
        private IDOutputDevice Outputs => port == EPort.Left ? _unloadTransferLeftOutput : _unloadTransferRightOutput;
        #endregion

        #region Flags
        private bool FlagAFCleanRequestUnload
        {
            get
            {
                return Inputs[(int)EUnloadTransferProcessInput.AF_CLEAN_REQ_UNLOAD];
            }
        }

        private bool FlagAFCleanUnloadDone
        {
            set
            {
                Outputs[(int)EUnloadTransferProcessOutput.AF_CLEAN_UNLOAD_DONE] = value;
            }
        }

        private bool FlagUnloadAlignWorkEnable
        {
            get
            {
                return Inputs[(int)EUnloadTransferProcessInput.WORK_ENABLE];
            }
        }

        private bool FlagUnloadTransferReadyToUnload
        {
            set
            {
                Outputs[(int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_READY_TO_UNLOAD] = value;
            }
        }
        #endregion

        #region Constructor
        public UnloadTransferProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("UnloadTransferLeftRecipe")] UnloadTransferRecipe unloadTransferLeftRecipe,
            [FromKeyedServices("UnloadTransferRightRecipe")] UnloadTransferRecipe unloadTransferRightRecipe,
            [FromKeyedServices("UnloadTransferLeftInput")] IDInputDevice unloadTransferLeftInput,
            [FromKeyedServices("UnloadTransferLeftOutput")] IDOutputDevice unloadTransferLeftOutput,
            [FromKeyedServices("UnloadTransferRightInput")] IDInputDevice unloadTransferRightInput,
            [FromKeyedServices("UnloadTransferRightOutput")] IDOutputDevice unloadTransferRightOutput,
            CWorkData workData)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _unloadTransferLeftRecipe = unloadTransferLeftRecipe;
            _unloadTransferRightRecipe = unloadTransferRightRecipe;
            _unloadTransferLeftInput = unloadTransferLeftInput;
            _unloadTransferLeftOutput = unloadTransferLeftOutput;
            _unloadTransferRightInput = unloadTransferRightInput;
            _unloadTransferRightOutput = unloadTransferRightOutput;
            _workData = workData;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EUnloadTransferOriginStep)Step.OriginStep)
            {
                case EUnloadTransferOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.ZAxis_Origin:
                    Log.Debug("Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => ZAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_ZAxis_Origin_Fail :
                                                        EAlarm.UnloadTransferRight_ZAxis_Origin_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.YAxis_Origin:
                    Log.Debug("Y Axis Origin Start");
                    YAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => YAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_Origin_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_Origin_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.End:
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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.AFCleanLeftUnload:
                    if (port == EPort.Left)
                    {
                        Sequence_AFCleanUnload();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AFCleanRightUnload:
                    if (port == EPort.Right)
                    {
                        Sequence_AFCleanUnload();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    if (port == EPort.Left)
                    {
                        Sequence_UnloadTransferPlace();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.UnloadTransferRightPlace:
                    if (port == EPort.Right)
                    {
                        Sequence_UnloadTransferPlace();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                default:
                    Sequence = ESequence.Stop;
                    break;
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((EUnloadTransferProcessToRunStep)Step.ToRunStep)
            {
                case EUnloadTransferProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EUnloadTransferProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<EUnloadTransferProcessOutput>)Outputs).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case EUnloadTransferProcessToRunStep.End:
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

        public override bool PreProcess()
        {
            if (_devices.Inputs.UnloadTransferAvoidNotCollision.Value == false)
            {
                RaiseWarning(EWarning.UnloadTransferCollisionDetect);
            }

            return base.PreProcess();
        }
        #endregion

        #region Private Methods
        private void Sequence_Ready()
        {
            switch ((EUnloadTransferReadyStep)Step.RunStep)
            {
                case EUnloadTransferReadyStep.Start:
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                case EUnloadTransferReadyStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(Recipe.ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(Recipe.ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferReadyStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.UnloadTransferRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferReadyStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(Recipe.YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(Recipe.YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferReadyStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_MoveReadyPosition_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferReadyStep.End:
                    Log.Debug("Initialize End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((EUnloadTransferAutoRunStep)Step.RunStep)
            {
                case EUnloadTransferAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EUnloadTransferAutoRunStep.GlassVac_Check:
                    if (IsVacDetect)
                    {
                        Log.Info("Sequence Unload Transfer Place");
                        Sequence = port == EPort.Left ? ESequence.UnloadTransferLeftPlace : ESequence.UnloadTransferRightPlace;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadTransferAutoRunStep.End:
                    Log.Info("Sequence AF Clean Unload");
                    Sequence = port == EPort.Left ? ESequence.AFCleanLeftUnload : ESequence.AFCleanRightUnload;
                    break;
            }
        }

        private void Sequence_AFCleanUnload()
        {
            switch ((EUnloadTransferAFCleanUnloadStep)Step.RunStep)
            {
                case EUnloadTransferAFCleanUnloadStep.Start:
                    Log.Debug("AF Clean Unload Start");
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.YAxis_Move_PickPositon:
                    Log.Debug("Y Axis Move Pick Position");
                    YAxis.MoveAbs(Recipe.YAxisPickPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(Recipe.YAxisPickPosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.YAxis_Move_PickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_MovePickPosition_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_MovePickPosition_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position Done");
                    Log.Debug("Wait AF Clean Request Unload");
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.Wait_AFCleanRequestUnload:
                    if (FlagAFCleanRequestUnload == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.ZAxis_Move_PickPosition:
                    Log.Debug("Z Axis Move Pick Position");
                    ZAxis.MoveAbs(Recipe.ZAxisPickPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(Recipe.ZAxisPickPosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.ZAxis_Move_PickPositon_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_ZAxis_MovePickPosition_Fail :
                                                        EAlarm.UnloadTransferRight_ZAxis_MovePickPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    GlassVacuumOnOff(true);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(GlassVac, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.UnloadTransferLeft_VacuumOn_Fail :
                                                        EWarning.UnloadTransferRight_VacuumOn_Fail));
                        break;
                    }
                    if (port == EPort.Left)
                    {
                        _devices.Outputs.OutShuttleLeftBlow.Value = true;
                        Task.Delay(300).ContinueWith(t =>
                        {
                            _devices.Outputs.OutShuttleLeftBlow.Value = false;
                        });
                    }
                    Thread.Sleep(300);
                    Log.Debug("Vacuum On Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.ZAxis_Move_ReadyPositon:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(Recipe.ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(Recipe.ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.ZAxis_Move_ReadyPositon_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.UnloadTransferRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.Set_FlagAFCleanUnloadDone:
                    Log.Debug("Set Flag AF Clean Unload Done");
                    FlagAFCleanUnloadDone = true;
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.Wait_AFCleanUnloadDoneReceived:
                    if (FlagAFCleanRequestUnload == true)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag AF Clean Unload Done");
                    FlagAFCleanUnloadDone = false;
                    Step.RunStep++;
                    break;
                case EUnloadTransferAFCleanUnloadStep.End:
                    Log.Debug("AF Clean Unload End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Unload Transfer Place");
                    Sequence = port == EPort.Left ? ESequence.UnloadTransferLeftPlace : ESequence.UnloadTransferRightPlace;
                    break;
            }
        }

        private void Sequence_UnloadTransferPlace()
        {
            switch ((EUnloadTransferPlaceStep)Step.RunStep)
            {
                case EUnloadTransferPlaceStep.Start:
                    Log.Debug("Unload Transfer Place Start");
                    Log.Debug("Wait Unload Transfer Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.SetFlagUnloadTransferReady:
                    Log.Debug("Set FlagUnloadTransferReadyToUnload");
                    FlagUnloadTransferReadyToUnload = true;
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.Wait_UnloadAlignReady:
                    if (FlagUnloadAlignWorkEnable == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("FlagUnloadAlignWorkEnable Received");
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.UnloadAlign_GlassVacCheck:
                    if (port == EPort.Left)
                    {
                        if (UnloadAlignVac3 == false)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac3, true);
#endif
                            UnloadAlignCurrentIndex = 3;
                            Step.RunStep = (int)EUnloadTransferPlaceStep.YAxis_Move_PlacePosition3;
                            break;
                        }
                        if (UnloadAlignVac4 == false)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac4, true);
#endif
                            UnloadAlignCurrentIndex = 4;
                            Step.RunStep = (int)EUnloadTransferPlaceStep.YAxis_Move_PlacePosition4;
                            break;
                        }

                        if (UnloadAlignVac2 == false)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac2, true);
#endif
                            UnloadAlignCurrentIndex = 2;

                            Step.RunStep = (int)EUnloadTransferPlaceStep.YAxis_Move_PlacePosition2;
                            break;
                        }

                        if (UnloadAlignVac1 == false || _machineStatus.IsDryRunMode)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac1, true);
#endif
                            UnloadAlignCurrentIndex = 1;

                            Step.RunStep++;
                            break;
                        }
                    }
                    else
                    {
                        if (UnloadAlignVac2 == false)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac2, true);
#endif
                            UnloadAlignCurrentIndex = 2;

                            Step.RunStep = (int)EUnloadTransferPlaceStep.YAxis_Move_PlacePosition2;
                            break;
                        }

                        if (UnloadAlignVac1 == false || _machineStatus.IsDryRunMode)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac1, true);
#endif
                            UnloadAlignCurrentIndex = 1;
                            Step.RunStep++;
                            break;
                        }

                        if (UnloadAlignVac3 == false)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac3, true);
#endif
                            UnloadAlignCurrentIndex = 3;
                            Step.RunStep = (int)EUnloadTransferPlaceStep.YAxis_Move_PlacePosition3;
                            break;
                        }
                        if (UnloadAlignVac4 == false)
                        {
#if SIMULATION
                        SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassAlignVac4, true);
#endif
                            UnloadAlignCurrentIndex = 4;
                            Step.RunStep = (int)EUnloadTransferPlaceStep.YAxis_Move_PlacePosition4;
                            break;
                        }
                    }
                    Step.RunStep = (int)EUnloadTransferPlaceStep.Wait_UnloadAlignReady;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition1:
                    Log.Debug("YAxis Move Place Position 1");
                    YAxis.MoveAbs(Recipe.YAxisPlacePosition1);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(Recipe.YAxisPlacePosition1));
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition1_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_MovePlacePosition1_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_MovePlacePosition1_Fail));
                        break;
                    }
                    Log.Debug("YAxis Move Place Position 1 Done");
                    Step.RunStep = (int)EUnloadTransferPlaceStep.ZAxis_Move_PlacePosition;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition2:
                    Log.Debug("YAxis Move Place Position 2");
                    YAxis.MoveAbs(Recipe.YAxisPlacePosition2);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(Recipe.YAxisPlacePosition2));
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition2_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_MovePlacePosition2_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_MovePlacePosition2_Fail));
                        break;
                    }
                    Log.Debug("YAxis Move Place Position 2 Done");
                    Step.RunStep = (int)EUnloadTransferPlaceStep.ZAxis_Move_PlacePosition;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition3:
                    Log.Debug("YAxis Move Place Position 3");
                    YAxis.MoveAbs(Recipe.YAxisPlacePosition3);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(Recipe.YAxisPlacePosition3));
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition3_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_MovePlacePosition3_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_MovePlacePosition3_Fail));
                        break;
                    }
                    Log.Debug("YAxis Move Place Position 3 Done");
                    Step.RunStep = (int)EUnloadTransferPlaceStep.ZAxis_Move_PlacePosition;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition4:
                    Log.Debug("YAxis Move Place Position 4");
                    YAxis.MoveAbs(Recipe.YAxisPlacePosition4);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(Recipe.YAxisPlacePosition4));
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_PlacePosition4_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_MovePlacePosition4_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_MovePlacePosition4_Fail));
                        break;
                    }
                    Log.Debug("YAxis Move Place Position 4 Done");
                    Step.RunStep = (int)EUnloadTransferPlaceStep.ZAxis_Move_PlacePosition;
                    break;
                case EUnloadTransferPlaceStep.ZAxis_Move_PlacePosition:
                    Log.Debug("Z Axis Move Place Position");
                    ZAxis.MoveAbs(Recipe.ZAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(Recipe.ZAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.ZAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_ZAxis_MovePlacePosition_Fail :
                                                        EAlarm.UnloadTransferRight_ZAxis_MovePlacePosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    GlassVacuumOnOff(false);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(GlassVac, false);
#endif
                    Wait(300);
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(Recipe.ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(Recipe.ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.UnloadTransferRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.UnloadAlign_GlassVac_Check:
                    if (UnloadAlignGlassVac[UnloadAlignCurrentIndex - 1].Value == false && !_machineStatus.IsDryRunMode)
                    {
                        RaiseWarning(EWarning.UnloadAlign_Vacuum_Fail);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(Recipe.YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(Recipe.YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.UnloadTransferLeft_YAxis_MoveReadyPosition_Fail :
                                                        EAlarm.UnloadTransferRight_YAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Log.Debug("Clear FlagUnloadTransferReadyToUnload");
                    FlagUnloadTransferReadyToUnload = false;
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.Set_FlagUnloadTransferPlaceDone:
                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.Wait_UnloadAlignPlaceDoneReceived:
                    if (FlagUnloadAlignWorkEnable == true)
                    {
                        Wait(20);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case EUnloadTransferPlaceStep.End:
                    Log.Debug("Unload Transfer Place End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    if (port == EPort.Left)
                    {
                        _workData.CountData.Left += 1;
                    }
                    else
                    {
                        _workData.CountData.Right += 1;
                    }
                    Log.Info("Sequence AF Clean Unload");
                    Sequence = port == EPort.Left ? ESequence.AFCleanLeftUnload : ESequence.AFCleanRightUnload;
                    break;
            }
        }

        private void GlassVacuumOnOff(bool isOn)
        {
            GlassVacOnOff.Value = isOn;
            GlassBlowOnOff.Value = !isOn;

            if (isOn == false)
            {
                Task.Delay(100).ContinueWith(t =>
                {
                    GlassBlowOnOff.Value = false;
                });
            }
        }
        #endregion
    }
}
