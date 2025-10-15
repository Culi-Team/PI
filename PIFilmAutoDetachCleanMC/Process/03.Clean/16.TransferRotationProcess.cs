using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using OpenCvSharp;
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
    public class TransferRotationProcess : ProcessBase<ESequence>
    {
        #region Privates
        private EPort port => Name == EProcess.TransferRotationLeft.ToString() ? EPort.Left : EPort.Right;

        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly TransferRotationRecipe _transferRotationLeftRecipe;
        private readonly TransferRotationRecipe _transferRotationRightRecipe;
        private readonly IDInputDevice _transferRotationLeftInput;
        private readonly IDOutputDevice _transferRotationLeftOutput;
        private readonly IDInputDevice _transferRotationRightInput;
        private readonly IDOutputDevice _transferRotationRightOutput;
        private readonly MachineStatus _machineStatus;

        private IMotion ZAxis => port == EPort.Left ? _devices.Motions.TransferRotationLZAxis :
                                                      _devices.Motions.TransferRotationRZAxis;
        private ICylinder RotateCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftRotate :
                                                      _devices.Cylinders.TrRotateRightRotate;

        private ICylinder TransferCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftFwBw :
                                                      _devices.Cylinders.TrRotateRightFwBw;

        private ICylinder UpDownCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftUpDown :
                                                            _devices.Cylinders.TrRotateRightUpDown;

        public IDOutput GlassVac1OnOff => port == EPort.Left ? _devices.Outputs.TrRotateLeftVac1OnOff :
                                                      _devices.Outputs.TrRotateRightVac1OnOff;

        public IDOutput GlassVac2OnOff => port == EPort.Left ? _devices.Outputs.TrRotateLeftVac2OnOff :
                                                      _devices.Outputs.TrRotateRightVac2OnOff;

        public IDOutput GlassRotateVacOnOff => port == EPort.Left ? _devices.Outputs.TrRotateLeftRotVacOnOff :
                                                      _devices.Outputs.TrRotateRightRotVacOnOff;

        public bool GlassVac1 => port == EPort.Left ? _devices.Inputs.TrRotateLeftVac1.Value :
                                                      _devices.Inputs.TrRotateRightVac1.Value;

        public bool GlassVac2 => port == EPort.Left ? _devices.Inputs.TrRotateLeftVac2.Value :
                                                      _devices.Inputs.TrRotateRightVac2.Value;

        public bool GlassRotVac => port == EPort.Left ? _devices.Inputs.TrRotateLeftRotVac.Value :
                                                      _devices.Inputs.TrRotateRightRotVac.Value;

        private TransferRotationRecipe Recipe => port == EPort.Left ? _transferRotationLeftRecipe : _transferRotationRightRecipe;

        private double ZAxisReadyPosition => Recipe.ZAxisReadyPosition;
        private double ZAxisPickPosition => Recipe.ZAxisPickPosition;
        private double ZAxisTransferBeforeRotatePosition => Recipe.ZAxisTransferBeforeRotatePosition;
        private double ZAxisTransferAfterRotatePosition => Recipe.ZAxisTransferAfterRotatePosition;
        private double ZAxisPlacePosition => Recipe.ZAxisPlacePosition;

        private IDInputDevice Inputs => port == EPort.Left ? _transferRotationLeftInput : _transferRotationRightInput;
        private IDOutputDevice Outputs => port == EPort.Left ? _transferRotationLeftOutput : _transferRotationRightOutput;
        #endregion

        #region Flags
        private bool FlagWETCleanRequestUnload
        {
            get
            {
                return Inputs[(int)ETransferRotationProcessInput.WET_CLEAN_REQ_UNLOAD];
            }
        }

        private bool FlagWETCleanUnloadDoneReceived
        {
            get
            {
                return Inputs[(int)ETransferRotationProcessInput.WET_CLEAN_UNLOAD_DONE_RECEIVED];
            }
        }

        private bool FlagWETCleanUnloadDone
        {
            set
            {
                Outputs[(int)ETransferRotationProcessOutput.WET_CLEAN_UNLOAD_DONE] = value;
            }
        }

        private bool FlagAFCleanRequestLoad
        {
            get
            {
                return Inputs[(int)ETransferRotationProcessInput.AF_CLEAN_REQ_LOAD];
            }
        }

        private bool FlagAFCleanLoadDoneReceived
        {
            get
            {
                return Inputs[(int)ETransferRotationProcessInput.AF_CLEAN_LOAD_DONE_RECEIVED];
            }
        }

        private bool FlagAFCleanLoadDone
        {
            set
            {
                Outputs[(int)ETransferRotationProcessOutput.AF_CLEAN_LOAD_DONE] = value;
            }
        }

        private bool FlagTransferRotationReadyPick
        {
            set
            {
                Outputs[(int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PICK] = value;
            }
        }

        private bool FlagTransferRotationReadyPlace
        {
            set
            {
                Outputs[(int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PLACE] = value;
            }
        }
        #endregion

        #region Constructor
        public TransferRotationProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("TransferRotationLeftRecipe")] TransferRotationRecipe transferRotationLeftRecipe,
            [FromKeyedServices("TransferRotationRightRecipe")] TransferRotationRecipe transferRotationRightRecipe,
            [FromKeyedServices("TransferRotationLeftInput")] IDInputDevice transferRotationLeftInput,
            [FromKeyedServices("TransferRotationLeftOutput")] IDOutputDevice transferRotationLeftOutput,
            [FromKeyedServices("TransferRotationRightInput")] IDInputDevice transferRotationRightInput,
            [FromKeyedServices("TransferRotationRightOutput")] IDOutputDevice transferRotationRightOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _transferRotationLeftRecipe = transferRotationLeftRecipe;
            _transferRotationRightRecipe = transferRotationRightRecipe;
            _transferRotationLeftInput = transferRotationLeftInput;
            _transferRotationLeftOutput = transferRotationLeftOutput;
            _transferRotationRightInput = transferRotationRightInput;
            _transferRotationRightOutput = transferRotationRightOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ETransferRotationOriginStep)Step.OriginStep)
            {
                case ETransferRotationOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.ZAxis_Origin:
                    Log.Debug("Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => { return ZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_Origin_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_Origin_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_Cyl_Backward:
                    Log.Debug("Transfer Rotation Cylinder Backward");
                    TransferCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return TransferCyl.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_Cylinder_Backward_Fail :
                                                          EWarning.TransferRotationRight_Cylinder_Backward_Fail));
                        break;
                    }
                    Log.Debug("Transfer Rotation Cylinder Backward Done");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_0Degree:
                    Log.Debug("Transfer Rotation to 0 Degree");
                    RotateCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return RotateCyl.IsForward; });
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_0Degree_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_RotationCylinder_0D_Fail :
                                                          EWarning.TransferRotationRight_RotationCylinder_0D_Fail));
                        break;
                    }
                    Log.Debug("Transfer Rotation to 0 Degree Done");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.End:
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
                    break;
                case ESequence.TransferInShuttleRightPick:
                    break;
                case ESequence.WETCleanLeftLoad:
                    break;
                case ESequence.WETCleanRightLoad:
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanRight:
                    break;
                case ESequence.WETCleanLeftUnload:
                    if (port == EPort.Left)
                    {
                        Sequence_WETCleanUnload();
                    }
                    break;
                case ESequence.WETCleanRightUnload:
                    if (port == EPort.Right)
                    {
                        Sequence_WETCleanUnload();
                    }
                    break;
                case ESequence.TransferRotationLeft:
                    if (port == EPort.Left)
                    {
                        Sequence_TransferRotation();
                    }
                    break;
                case ESequence.TransferRotationRight:
                    if (port == EPort.Right)
                    {
                        Sequence_TransferRotation();
                    }
                    break;
                case ESequence.AFCleanLeftLoad:
                    if (port == EPort.Left)
                    {
                        Sequence_AFCleanLoad();
                    }
                    break;
                case ESequence.AFCleanRightLoad:
                    if (port == EPort.Right)
                    {
                        Sequence_AFCleanLoad();
                    }
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
            switch ((ETransferRotationProcessToRunStep)Step.ToRunStep)
            {
                case ETransferRotationProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case ETransferRotationProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<ETransferRotationProcessOutput>)Outputs).Clear();
                    Step.ToRunStep++;
                    break;
                case ETransferRotationProcessToRunStep.End:
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
            switch ((ETransferRotationReadyStep)Step.RunStep)
            {
                case ETransferRotationReadyStep.Start:
                    if (IsOriginOrInitSelected == false)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                case ETransferRotationReadyStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationReadyStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationReadyStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    UpDownCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferRotationReadyStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_Cylinder_Up_Fail :
                                                          EWarning.TransferRotationRight_Cylinder_Up_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationReadyStep.End:
                    IsWarning = false;
                    Log.Debug("Initialize End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((ETransferRotationAutoRunStep)Step.RunStep)
            {
                case ETransferRotationAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case ETransferRotationAutoRunStep.GlassVac_Check:
                    if ((GlassVac1 || GlassRotVac) && _machineStatus.IsDryRunMode == false)
                    {
                        Log.Info("Sequence Transfer Rotation");
                        Sequence = port == EPort.Left ? ESequence.TransferRotationLeft : ESequence.TransferRotationRight;
                        break;
                    }
                    if (GlassVac2 && _machineStatus.IsDryRunMode == false)
                    {
                        Log.Info("Sequence AF Clean Load");
                        Sequence = port == EPort.Left ? ESequence.AFCleanLeftLoad : ESequence.AFCleanRightLoad;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferRotationAutoRunStep.End:
                    Log.Info("Sequence WET Clean Unload");
                    Sequence = port == EPort.Left ? ESequence.WETCleanLeftUnload : ESequence.WETCleanRightUnload;
                    break;
            }
        }

        private void Sequence_WETCleanUnload()
        {
            switch ((ETransferRotationWETCleanUnloadStep)Step.RunStep)
            {
                case ETransferRotationWETCleanUnloadStep.Start:
                    Log.Debug("WET Clean Unload Start");
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.Set_FlagTransferRotationReadyPick:
                    Log.Debug("Set Flag Transfer Rotation Ready Pick");
                    FlagTransferRotationReadyPick = true;
                    FlagTransferRotationReadyPlace = false;
                    Log.Debug("Wait WET Clean Request Unload");
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.Wait_WETCleanRequestUnload:
                    if (FlagWETCleanRequestUnload == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.ZAxis_Move_PickPosition:
                    Log.Debug("Z Axis Move Pick Position");
                    ZAxis.MoveAbs(ZAxisPickPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisPickPosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.ZAxis_Move_PickPositionWait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_MovePickPosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_MovePickPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    GlassVac1OnOff.Value = true;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.TrRotateLeftVac1 : _devices.Inputs.TrRotateRightVac1, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => GlassVac1 || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_GlassVacuum1_Check_Fail :
                                                          EWarning.TransferRotationRight_GlassVacuum1_Check_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.ZAxis_Move_ReadyPositionWait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Positon Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.Set_FlagWETCleanUnloadDone:
                    Log.Debug("Set Flag WET Clean Unload Done");
                    FlagWETCleanUnloadDone = true;
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.Wait_WETCleanUnloadDoneReceived:
                    if (FlagWETCleanUnloadDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag WET Clean Unload Done");
                    FlagWETCleanUnloadDone = false;

                    Log.Debug("Clear Flag Transfer Rotation Ready Pick");
                    FlagTransferRotationReadyPick = false;
                    Step.RunStep++;
                    break;
                case ETransferRotationWETCleanUnloadStep.End:
                    Log.Debug("WET Clean Unload End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer Rotation");
                    Sequence = port == EPort.Left ? ESequence.TransferRotationLeft : ESequence.TransferRotationRight;
                    break;
            }
        }

        private void Sequence_TransferRotation()
        {
            switch ((ETransferRotationStep)Step.RunStep)
            {
                case ETransferRotationStep.Start:
                    Log.Debug("Transfer Rotation Start");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.TransferCyl_Forward:
                    Log.Debug("Transfer Cylinder Forward");
                    TransferCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TransferCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.TransferCyl_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_Cylinder_Forward_Fail :
                                                          EWarning.TransferRotationRight_Cylinder_Forward_Fail));
                        break;
                    }
                    Log.Debug("Transfer Cylinder Forward Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.ZAxis_Move_TransferBeforeRotatePosition:
                    Log.Debug("Z Axis Move Transfer Before Rotate Position");
                    ZAxis.MoveAbs(ZAxisTransferBeforeRotatePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisTransferBeforeRotatePosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.ZAxis_Move_TransferBeforeRotatePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_Move_TransferBeforeRotatePosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_Move_TransferBeforeRotatePosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Transfer Before Rotate Position Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.GlassRotVac_On:
                    Log.Debug("Glass Rotation Vacuum On");
                    GlassRotateVacOnOff.Value = true;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.TrRotateLeftRotVac : _devices.Inputs.TrRotateRightRotVac, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.GlassVac1_Off:
                    Log.Debug("Glass Vacuum 1 Off");
                    GlassVac1OnOff.Value = false;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.TrRotateLeftVac1 : _devices.Inputs.TrRotateRightVac1, false);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.GlassRotVac_On_Check:
                    if (GlassRotVac == false && !_machineStatus.IsDryRunMode)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_RotateVac_Check_Fail :
                                                          EWarning.TransferRotationRight_RotateVac_Check_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.Cyl_Rotate_180D:
                    Log.Debug("Cylinder Rotate 180 Degree");
                    RotateCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RotateCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.Cyl_Rotate_180D_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_RotationCylinder_180D_Fail :
                                                          EWarning.TransferRotationRight_RotationCylinder_180D_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Rotate 180 Degree Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.Cyl_Down:
                    Log.Debug("Cylinder Down");
                    UpDownCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => UpDownCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_Cylinder_Down_Fail :
                                                          EWarning.TransferRotationRight_Cylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.ZAxis_Move_TransferAfterRotatePositon:
                    Log.Debug("Z Axis Move Transfer After Rotate Position");
                    ZAxis.MoveAbs(ZAxisTransferAfterRotatePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisTransferAfterRotatePosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.ZAxis_Move_TransferAfterRotatePositon_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_Move_TransferAfterRotatePosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_Move_TransferAfterRotatePosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Transfer After Rotate Position Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.GlassVac2_On:
                    Log.Debug("Glass Vacuum 2 On");
                    GlassVac2OnOff.Value = true;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.TrRotateLeftVac2 : _devices.Inputs.TrRotateRightVac2, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => GlassVac2 || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.GlassVac2_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_GlassVacuum2_Check_Fail :
                                                                EWarning.TransferRotationRight_GlassVacuum2_Check_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.GlassRotVac_Off:
                    Log.Debug("Glass Rotation Vacuum Off");
                    GlassRotateVacOnOff.Value = false;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.TrRotateLeftRotVac : _devices.Inputs.TrRotateRightRotVac, false);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.GlassVac2_On_Check:
                    if (GlassVac2 != true && !_machineStatus.IsDryRunMode)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_GlassVacAfterRotate_Check_Fail :
                                                          EWarning.TransferRotationRight_GlassVacAfterRotate_Check_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.Cyl_Backward:
                    Log.Debug("Cylinder Backward");
                    TransferCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TransferCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.TransferRotationLeft_Cylinder_Backward_Fail :
                                                          EWarning.TransferRotationRight_Cylinder_Backward_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationStep.End:
                    Log.Debug("Transfer Rotation End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence AF Clean Load");
                    Sequence = port == EPort.Left ? ESequence.AFCleanLeftLoad : ESequence.AFCleanRightLoad;
                    break;
            }
        }

        private void Sequence_AFCleanLoad()
        {
            switch ((ETransferRotationAFCleanLoad)Step.RunStep)
            {
                case ETransferRotationAFCleanLoad.Start:
                    Log.Debug("AF Clean Load Start");
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.Set_FlagTransferRotationReadyPlace:
                    Log.Debug("Set Flag Transfer Rotation Ready Place");
                    FlagTransferRotationReadyPlace = true;
                    FlagTransferRotationReadyPick = false;
                    Log.Debug("Wait AF Clean Request Load");
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.Wait_AFCleanRequestLoad:
                    if (FlagAFCleanRequestLoad == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.ZAxis_Move_PlacePosition:
                    Log.Debug("Z Axis Move Place Position");
                    ZAxis.MoveAbs(ZAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.ZAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_MovePlacePosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_MovePlacePosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.Vacuum_Off:
                    Log.Debug("Glass Vacuum 2 Off");
                    GlassVac2OnOff.Value = false;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.TrRotateLeftVac2 : _devices.Inputs.TrRotateRightVac2, false);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Left ? EAlarm.TransferRotationLeft_ZAxis_MoveReadyPosition_Fail :
                                                        EAlarm.TransferRotationRight_ZAxis_MoveReadyPosition_Fail));
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.Set_FlagAFCleanLoadDone:
                    Log.Debug("Set Flag AF Clean Load Done");
                    FlagAFCleanLoadDone = true;
                    Log.Debug("Wait AF Clean Load Done Received");
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.Wait_AFCleanLoadDoneReceived:
                    if (FlagAFCleanLoadDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag AF Clean Load Done");
                    FlagAFCleanLoadDone = false;
                    Log.Debug("Clear Flag Transfer Rotation Ready Place");
                    FlagTransferRotationReadyPlace = false;
                    Step.RunStep++;
                    break;
                case ETransferRotationAFCleanLoad.End:
                    Log.Debug("AF Clean Load End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Sequence WET Clean Unload");
                    Sequence = port == EPort.Left ? ESequence.WETCleanLeftUnload : ESequence.WETCleanRightUnload;
                    break;
            }
        }
        #endregion
    }
}
