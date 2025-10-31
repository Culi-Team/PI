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
        private IDOutputDevice OutputFlagss => port == EPort.Left ? _transferInShuttleLeftOutput : _transferInShuttleRightOutput;

        private EPort port => Name == EProcess.TransferInShuttleLeft.ToString() ? EPort.Left : EPort.Right;
        #endregion

        #region Flags
        private bool OutFlag_OriginDone
        {
            set
            {
                OutputFlagss[(int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_ORIGIN_DONE] = value;
            }
        }
        private bool OutFlag_TransferInShuttleGlassRequest
        {
            set
            {
                OutputFlagss[(int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_GLASS_REQUEST] = value;
            }
        }

        private bool OutFlag_TransferInShuttleInSafePosition
        {
            set
            {
                OutputFlagss[(int)ETransferInShuttleProcessOutput.IN_SAFETY_POSITION] = value;
            }
        }

        private bool OutFlag_WetCleanLoadDone
        {
            set
            {
                OutputFlagss[(int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE] = value;
            }
        }

        private bool InFlag_WETCleanRequestLoad
        {
            get
            {
                return Inputs[(int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD];
            }
        }

        private bool InFlag_GlassTransferPlaceDone
        {
            get
            {
                return Inputs[(int)ETransferInShuttleProcessInput.GLASS_TRANSFER_PLACE_DONE];
            }
        }
        #endregion

        #region Motions
        private IMotion YAxis => port == EPort.Left ? _devices.Motions.TransferInShuttleLYAxis :
                                              _devices.Motions.TransferInShuttleRYAxis;

        private IMotion ZAxis => port == EPort.Left ? _devices.Motions.TransferInShuttleLZAxis :
                                                      _devices.Motions.TransferInShuttleRZAxis;

        #endregion

        #region Cylinders
        private ICylinder AlignCyl1 => port == EPort.Left ? _devices.Cylinders.AlignStageL_AlignCyl1 : _devices.Cylinders.AlignStageR_AlignCyl1;
        private ICylinder AlignCyl2 => port == EPort.Left ? _devices.Cylinders.AlignStageL_AlignCyl2 : _devices.Cylinders.AlignStageR_AlignCyl2;
        private ICylinder AlignCyl3 => port == EPort.Left ? _devices.Cylinders.AlignStageL_AlignCyl3 : _devices.Cylinders.AlignStageR_AlignCyl3;

        private ICylinder BrushCyl => port == EPort.Left ? _devices.Cylinders.AlignStageL_BrushCyl : _devices.Cylinders.AlignStageR_BrushCyl;

        public ICylinder RotCyl => port == EPort.Left ? _devices.Cylinders.TransferInShuttleL_RotateCyl
                                               : _devices.Cylinders.TransferInShuttleR_RotateCyl;
        #endregion

        #region Recipes Value
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
        private double ZAxisTransferPosition => port == EPort.Left ? _transferInShuttleLeftRecipe.ZAxisTransferPosition
                                                                : _transferInShuttleRightRecipe.ZAxisTransferPosition;

        private double YAxisPlacePosition => port == EPort.Left ? _transferInShuttleLeftRecipe.YAxisPlacePosition
                                                                : _transferInShuttleRightRecipe.YAxisPlacePosition;

        private double ZAxisPlacePosition => port == EPort.Left ? _transferInShuttleLeftRecipe.ZAxisPlacePosition
                                                                : _transferInShuttleRightRecipe.ZAxisPlacePosition;

        private bool IsPortDisable => port == EPort.Left ? _commonRecipe.DisableLeftPort : _commonRecipe.DisableRightPort;

        private bool IsInSafePosition
        {
            get
            {
                bool isZAxisSafePos = ZAxis.IsOnPosition(ZAxisReadyPosition);
                bool isYAxisSafePos = YAxis.IsOnPosition(YAxisReadyPosition);

                return isZAxisSafePos && isYAxisSafePos;
            }
        }
        #endregion

        #region Inputs
        private bool AlignStageVac1Sensor => port == EPort.Left ? _devices.Inputs.AlignStageLVac1.Value : _devices.Inputs.AlignStageRVac1.Value;
        private bool AlignStageVac2Sensor => port == EPort.Left ? _devices.Inputs.AlignStageLVac2.Value : _devices.Inputs.AlignStageRVac2.Value;
        private bool AlignStageVac3Sensor => port == EPort.Left ? _devices.Inputs.AlignStageLVac3.Value : _devices.Inputs.AlignStageRVac3.Value;

        private bool IsAlign_VacDetect1 => AlignStageVac1Sensor == true;
        private bool IsAlign_VacDetect2 => AlignStageVac2Sensor == true;
        private bool IsAlign_VacDetect3 => AlignStageVac3Sensor == true;
        private bool IsAlign_VacDetect => IsAlign_VacDetect1 || IsAlign_VacDetect2 || IsAlign_VacDetect3;

        private bool IsAlign_GlassDetect1 => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool IsAlign_GlassDetect2 => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect2.Value : _devices.Inputs.AlignStageRGlassDetect2.Value;
        private bool IsAlign_GlassDetect3 => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect3.Value : _devices.Inputs.AlignStageRGlassDetect3.Value;
        private bool IsAlign_GlassDetect => IsAlign_GlassDetect1 || IsAlign_GlassDetect2 || IsAlign_GlassDetect3;

        private IDInput TransferVacuumInput => port == EPort.Left
            ? _devices.Inputs.TransferInShuttleLVac
            : _devices.Inputs.TransferInShuttleRVac;

        private bool IsTransfer_VacDetect => TransferVacuumInput.Value;

        private bool IsAlignCylUp => AlignCyl1.IsForward && AlignCyl2.IsForward && AlignCyl3.IsForward;
        private bool IsAlignCylDown => AlignCyl1.IsBackward && AlignCyl2.IsBackward && AlignCyl3.IsBackward;
        #endregion

        #region Outputs
        private IDOutput AlignVac1 => port == EPort.Left ? _devices.Outputs.AlignStageLVac1OnOff : _devices.Outputs.AlignStageRVac1OnOff;
        private IDOutput AlignVac2 => port == EPort.Left ? _devices.Outputs.AlignStageLVac2OnOff : _devices.Outputs.AlignStageRVac2OnOff;
        private IDOutput AlignVac3 => port == EPort.Left ? _devices.Outputs.AlignStageLVac3OnOff : _devices.Outputs.AlignStageRVac3OnOff;
        private IDOutput AlignBlow1 => port == EPort.Left ? _devices.Outputs.AlignStageLBlow1OnOff : _devices.Outputs.AlignStageRBlow1OnOff;
        private IDOutput AlignBlow2 => port == EPort.Left ? _devices.Outputs.AlignStageLBlow2OnOff : _devices.Outputs.AlignStageRBlow2OnOff;
        private IDOutput AlignBlow3 => port == EPort.Left ? _devices.Outputs.AlignStageLBlow3OnOff : _devices.Outputs.AlignStageRBlow3OnOff;

        private IDOutput TransferVac => port == EPort.Left
            ? _devices.Outputs.TransferInShuttleLVacOnOff
            : _devices.Outputs.TransferInShuttleRVacOnOff;

        private IDOutput TransferBlow => port == EPort.Left
            ? _devices.Outputs.TransferInShuttleLBlowOnOff
            : _devices.Outputs.TransferInShuttleRBlowOnOff;
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
        public override bool ProcessToOrigin()
        {
            OutFlag_OriginDone = false;
            return base.ProcessToOrigin();
        }

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
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => ZAxis.Status.IsHomeDone);
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
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => YAxis.Status.IsHomeDone);
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
                    OutFlag_OriginDone = true;
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
                case ESequence.GlassTransferLeft:
                    if (port == EPort.Left) Sequence_GlassTransferToAlign();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.GlassTransferRight:
                    if (port == EPort.Right) Sequence_GlassTransferToAlign();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AlignGlassLeft:
                    if (port == EPort.Left) Sequence_AlignGlass();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AlignGlassRight:
                    if (port == EPort.Right) Sequence_AlignGlass();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.WETCleanLeftLoad:
                    if (port == EPort.Left)
                    {
                        Sequence_WETCleanLoad();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.WETCleanRightLoad:
                    if (port == EPort.Right)
                    {
                        Sequence_WETCleanLoad();
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
            switch ((ETransferInShuttleProcessToRunStep)Step.ToRunStep)
            {
                case ETransferInShuttleProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case ETransferInShuttleProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    OutputFlagss.ClearOutputs();
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
                case ETransferInShuttleReadyStep.SafetyPosition_Check:
                    if (IsInSafePosition)
                    {
                        Step.RunStep = (int)ETransferInShuttleReadyStep.Flag_InSafePosition_Set;
                        break;
                    }

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
                case ETransferInShuttleReadyStep.Flag_InSafePosition_Set:
                    Log.Info("Set TransferInShuttleInSafePosition Flag");

                    OutFlag_TransferInShuttleInSafePosition = true;
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
                    AlignVacOnOff(true);
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleAutoRunStep.Transfer_VacCheck:
                    if (IsTransfer_VacDetect)
                    {
                        Log.Info("Sequence WET Clean Load");
                        Sequence = port == EPort.Left ? ESequence.WETCleanLeftLoad : ESequence.WETCleanRightLoad;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ETransferInShuttleAutoRunStep.Align_GlassCheck:
                    if (IsAlign_GlassDetect || IsAlign_VacDetect)
                    {
                        Log.Info("Glass Detect on Align unit, Now align glass");
                        Sequence = port == EPort.Left ? ESequence.AlignGlassLeft : ESequence.AlignGlassRight;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ETransferInShuttleAutoRunStep.End:
                    Log.Info("Sequence Glass Transfer");
                    Sequence = port == EPort.Left ? ESequence.GlassTransferLeft : ESequence.GlassTransferRight;
                    break;
            }
        }

        private void Sequence_GlassTransferToAlign()
        {
            switch ((EGlassAlignGlassTransferPlaceStep)Step.RunStep)
            {
                case EGlassAlignGlassTransferPlaceStep.Start:
                    if (IsPortDisable)
                    {
                        Log.Info("Port is disabled, now just STOP");
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Debug("Glass Transfer to Align Start");
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.CylDown_ZAxisUp:
                    Log.Debug("Move Align Cylinder Down / ZAxis Ready");
                    AlignCylUpDown(false);
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsAlignCylDown && ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.CylDown_ZAxisUp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (IsAlignCylDown == false)
                        {
                            RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_AlignCylinder_Down_Fail
                                : EWarning.GlassAlignRight_AlignCylinder_Down_Fail));
                            break;
                        }

                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferInShuttleLeft_ZAxis_MoveReady_Fail
                            : EWarning.TransferInShuttleRight_ZAxis_MoveReady_Fail));
                        break;
                    }
                    Log.Debug("Move Align Cylinder Down / ZAxis Ready Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.YAxis_MoveReady:
                    Log.Debug($"{YAxis} move to Ready");
                    YAxis.MoveAbs(YAxisReadyPosition);
                    Wait((int)_commonRecipe.MotionMoveTimeOut * 1000, () => YAxis.IsOnPosition(YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.YAxis_MoveReady_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferInShuttleLeft_YAxis_MoveReady_Fail :
                                                          EWarning.TransferInShuttleRight_YAxis_MoveReady_Fail));
                        break;
                    }
                    Log.Debug($"{YAxis} move to Ready Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.Set_FlagRequestGlass:
                    Log.Debug("Set Flag Request Glass");
                    OutFlag_TransferInShuttleGlassRequest = true;

                    Log.Debug("Wait Glass Transfer Place Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.Wait_GlassTransferPlace_Done:
                    if (InFlag_GlassTransferPlaceDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear Flag Request Glass");
                    OutFlag_TransferInShuttleGlassRequest = false;
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Set Sequence Align Glass");
                    Sequence = port == EPort.Left ? ESequence.AlignGlassLeft : ESequence.AlignGlassRight;
                    break;
            }
        }

        private void Sequence_AlignGlass()
        {
            switch ((EGlassAlignStep)Step.RunStep)
            {
                case EGlassAlignStep.Start:
                    Log.Debug("Align Glass Start");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_1st:
                    Log.Debug("Vacuum On");
                    AlignVacOnOff(true);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac1 : _devices.Inputs.AlignStageRVac1, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac2 : _devices.Inputs.AlignStageRVac2, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac3 : _devices.Inputs.AlignStageRVac3, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsAlign_VacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_1st_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_Vacuum_Fail
                                                                : EWarning.GlassAlignRight_Vacuum_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Up:
                    Log.Debug("Cylinder Align Up");
                    AlignCylUpDown(true);
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => IsAlignCylUp);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_AlignCylinder_Up_Fail : EWarning.GlassAlignRight_AlignCylinder_Up_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Align Up Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    AlignVacOnOff(false);
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Wait_GlassDetect:
                    if (IsAlign_GlassDetect == false && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_GlassNotDetect : EWarning.GlassAlignRight_GlassNotDetect));
                        break;
                    }

                    AlignBlow1.Value = true;
                    AlignBlow2.Value = true;
                    AlignBlow3.Value = true;

                    Log.Debug("Glass Align Done");
                    Wait(500);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_2nd:
                    Log.Debug("Vacuum On 2nd");
                    AlignVacOnOff(true);
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsAlign_VacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_2nd_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left
                            ? EWarning.GlassAlignLeft_Vacuum_Fail
                            : EWarning.GlassAlignRight_Vacuum_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignCylUpDown(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsAlignCylDown);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left
                            ? EWarning.GlassAlignLeft_AlignCylinder_Down_Fail
                            : EWarning.GlassAlignRight_AlignCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Align Down Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Info("Sequence WET Clean Load");
                    Sequence = port == EPort.Left ? ESequence.WETCleanLeftLoad : ESequence.WETCleanRightLoad;
                    break;
            }
        }

        private void Sequence_WETCleanLoad()
        {
            switch ((ETransferInShuttleWETCleanLoadStep)Step.RunStep)
            {
                case ETransferInShuttleWETCleanLoadStep.Start:
                    Log.Debug("WET Clean Load Start");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Cyl_Rotate_0D:
                    Log.Debug("Cylinder Rotate 0 Degree");
                    RotCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RotCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Cyl_Rotate_0D_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferInShuttleLeft_RotateCylinder_0D_Fail :
                                                          EWarning.TransferInShuttleRight_RotateCylinder_0D_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Rotate 0 Degree Done");
                    if(IsTransfer_VacDetect)
                    {
                        Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.YAxis_Move_PlacePosition;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.GlassDetect_Check:
                    if (_machineStatus.IsDryRunMode || IsAlign_GlassDetect1)
                    {
                        Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition1;
                        break;
                    }
                    if (IsAlign_GlassDetect2)
                    {
                        Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition2;
                        break;
                    }
                    if (IsAlign_GlassDetect3)
                    {
                        Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition3;
                        break;
                    }

                    Log.Info("Sequence Glass Transfer");
                    Sequence = port == EPort.Left ? ESequence.GlassTransferLeft : ESequence.GlassTransferRight;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition1:
                    Log.Debug("Y Axis Move Pick Position 1");
                    YAxis.MoveAbs(YAxisPickPosition1);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPickPosition1));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition1_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePickPosition1_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePickPosition1_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position 1 Done");
                    AlignVac1.Value = false;
                    Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition2:
                    Log.Debug("Y Axis Move Pick Position 2");
                    YAxis.MoveAbs(YAxisPickPosition2);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPickPosition2));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition2_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePickPosition2_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePickPosition2_Fail));
                        break;
                    }
                    AlignVac2.Value = false;
                    Log.Debug("Y Axis Move Pick Position 2 Done");
                    Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition3:
                    Log.Debug("Y Axis Move Pick Position 3");
                    YAxis.MoveAbs(YAxisPickPosition3);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPickPosition3));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PickPosition3_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePickPosition3_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePickPosition3_Fail));
                        break;
                    }
                    AlignVac3.Value = false;
                    Log.Debug("Y Axis Move Pick Position 3 Done");
                    Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.ZAxis_Move_PickPosition;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_PickPosition:
                    Log.Debug("Z Axis Move Pick Position");
                    ZAxis.MoveAbs(ZAxisPickPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisPickPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_PickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MovePickPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MovePickPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    TransferVacOnOff(true);
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsTransfer_VacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferInShuttleLeft_Vacuum_Fail :
                                                        EWarning.TransferInShuttleRight_Vacuum_Fail));
                        break;
                    }
                    Log.Debug("Vacuum On Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Brush_Cyl_Up:
                    Log.Debug("Brush Cylinder Up");
                    BrushCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BrushCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Brush_Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferInShuttleLeft_BrushCylinder_Up_Fail :
                                                          EWarning.TransferInShuttleRight_BrushCylinder_Up_Fail));
                        break;
                    }
                    Log.Debug("Brush Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_TransferReadyPosition:
                    Log.Debug("Z Axis Move Transfer Position");
                    ZAxis.MoveAbs(ZAxisTransferPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisTransferPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_TransferReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Transfer Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PlacePosition:
                    Log.Debug("Y Axis Move Place Position");
                    YAxis.MoveAbs(YAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.YAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_YAxis_MovePlacePosition_Fail :
                                                        EAlarm.TransferInShuttleRight_YAxis_MovePlacePosition_Fail));
                        break;
                    }
                    Log.Debug("Y Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Cyl_Rotate_180D:
                    Log.Debug("Cylinder Rotate 180 Degree");
                    RotCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RotCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Cyl_Rotate_180D_Wait:
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
                case ETransferInShuttleWETCleanLoadStep.Wait_WETCleanRequestLoad:
                    if (InFlag_WETCleanRequestLoad == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_PlacePosition:
                    Log.Debug("Z Axis Move Place Position");
                    ZAxis.MoveAbs(ZAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MovePlacePosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MovePlacePosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    TransferVacOnOff(false);
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferInShuttleLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferInShuttleRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.Set_FlagWETCleanLoadDone:
                    Log.Debug("Set Flag WET Clean Load Done");
                    OutFlag_WetCleanLoadDone = true;
                    Step.RunStep++;
                    Log.Debug("Wait WET Clean Load Done Received");
                    break;
                case ETransferInShuttleWETCleanLoadStep.Wait_WETCleanPlaceDoneReceived:
                    if (InFlag_WETCleanRequestLoad == true)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag WET Clean Load Done");
                    OutFlag_WetCleanLoadDone = false;
                    Step.RunStep++;
                    break;
                case ETransferInShuttleWETCleanLoadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Step.RunStep = (int)ETransferInShuttleWETCleanLoadStep.Cyl_Rotate_0D;
                    break;
            }
        }
        #endregion

        #region Privates Methods
        private void AlignCylUpDown(bool isUp)
        {
            if (isUp)
            {
                AlignCyl1.Forward();
                AlignCyl2.Forward();
                AlignCyl3.Forward();
            }
            else
            {
                AlignCyl1.Backward();
                AlignCyl2.Backward();
                AlignCyl3.Backward();
            }
        }

        private void TransferVacOnOff(bool bOnOff)
        {
            TransferVac.Value = bOnOff;
            TransferBlow.Value = !bOnOff;

            if (bOnOff == false)
            {
                Task.Delay(100).ContinueWith(t =>
                {
                    TransferBlow.Value = false;
                });
            }
        }

        private void AlignVacOnOff(bool bOnOff)
        {
            AlignVac1.Value = bOnOff;
            AlignVac2.Value = bOnOff;
            AlignVac3.Value = bOnOff;

            AlignBlow1.Value = !bOnOff;
            AlignBlow2.Value = !bOnOff;
            AlignBlow3.Value = !bOnOff;
            if (bOnOff == false)
            {
                Task.Delay(100).ContinueWith(t =>
                {
                    AlignBlow1.Value = false;
                    AlignBlow2.Value = false;
                    AlignBlow3.Value = false;
                });
            }
            else
            {
                AlignBlow1.Value = false;
                AlignBlow2.Value = false;
                AlignBlow3.Value = false;
            }
#if SIMULATION
            SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac1 : _devices.Inputs.AlignStageRVac1, bOnOff);
            SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac2 : _devices.Inputs.AlignStageRVac2, bOnOff);
            SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac3 : _devices.Inputs.AlignStageRVac3, bOnOff);
#endif
        }
        #endregion
    }
}
