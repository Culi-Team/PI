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
    public class GlassTransferProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly GlassTransferRecipe _glassTransferRecipe;
        private readonly IDInputDevice _glassTransferInput;
        private readonly IDOutputDevice _glassTransferOutput;
        private readonly MachineStatus _machineStatus;

        private EPort currentPlacePort;

        private IMotion YAxis => _devices.Motions.GlassTransferYAxis;
        private IMotion ZAxis => _devices.Motions.GlassTransferZAxis;

        private IDOutput GlassBlow1 => _devices.Outputs.GlassTransferBlow1OnOff;
        private IDOutput GlassBlow2 => _devices.Outputs.GlassTransferBlow2OnOff;
        private IDOutput GlassBlow3 => _devices.Outputs.GlassTransferBlow3OnOff;

        private IDOutput GlassVac1 => _devices.Outputs.GlassTransferVac1OnOff;
        private IDOutput GlassVac2 => _devices.Outputs.GlassTransferVac2OnOff;
        private IDOutput GlassVac3 => _devices.Outputs.GlassTransferVac3OnOff;

        private ICylinder CylinderUpDown1 => _devices.Cylinders.GlassTransfer_UpDownCyl1;
        private ICylinder CylinderUpDown2 => _devices.Cylinders.GlassTransfer_UpDownCyl2;
        private ICylinder CylinderUpDown3 => _devices.Cylinders.GlassTransfer_UpDownCyl3;
        private bool IsCylinderUp => CylinderUpDown1.IsBackward && CylinderUpDown2.IsBackward && CylinderUpDown3.IsBackward;
        private bool IsCylinderDown => CylinderUpDown1.IsForward && CylinderUpDown2.IsForward && CylinderUpDown3.IsForward;

        private bool GlassTransferVac1 => _devices.Inputs.GlassTransferVac1.Value;
        private bool GlassTransferVac2 => _devices.Inputs.GlassTransferVac2.Value;
        private bool GlassTransferVac3 => _devices.Inputs.GlassTransferVac3.Value;

        private bool IsVacDetect => GlassTransferVac1 && GlassTransferVac2 && GlassTransferVac3;

        private double YAxisPlacePosition
        {
            get
            {
                if (currentPlacePort == EPort.Left)
                {
                    return _glassTransferRecipe.YAxisLeftPlacePosition;
                }
                return _glassTransferRecipe.YAxisRightPlacePosition;
            }
        }

        private double ZAxisPlacePosition
        {
            get
            {
                if (currentPlacePort == EPort.Left)
                {
                    return _glassTransferRecipe.ZAxisLeftPlacePosition;
                }
                return _glassTransferRecipe.ZAxisRightPlacePosition;
            }
        }
        #endregion

        #region Flags
        private bool FlagTransferInShuttleOriginDone
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_L_ORIGIN_DONE] &&
                    _glassTransferInput[(int)EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_R_ORIGIN_DONE];
            }
        }
        private bool FlagDetachRequestUnloadGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.DETACH_REQ_UNLOAD_GLASS];
            }
        }

        private bool InFlag_TransferInShuttleLeftRequestGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_LEFT_GLASS_REQUEST];
            }
        }

        private bool InFlag_TransfferInShuttleRightRequestGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_RIGHT_GLASS_REQUEST];
            }
        }

        private bool InFlag_TransfferInShuttleLeft_InSafePos
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_L_IN_SAFE_POS];
            }
        }

        private bool InFlag_TransfferInShuttleRight_InSafePos
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_R_IN_SAFE_POS];
            }
        }

        private bool FlagGlassTransferPickDone
        {
            set
            {
                _glassTransferOutput[(int)EGlassTransferProcessOutput.GLASS_TRANSFER_PICK_DONE] = value;
            }
        }

        private bool FlagGlassTransferLeftPlaceDone
        {
            set
            {
                _glassTransferOutput[(int)EGlassTransferProcessOutput.GLASS_TRANSFER_LEFT_PLACE_DONE] = value;
            }
        }

        private bool FlagGlassTransferRightPlaceDone
        {
            set
            {
                _glassTransferOutput[(int)EGlassTransferProcessOutput.GLASS_TRANSFER_RIGHT_PLACE_DONE] = value;
            }
        }

        private bool FlagPlaceDone
        {
            set
            {
                if (currentPlacePort == EPort.Left)
                {
                    FlagGlassTransferLeftPlaceDone = value;
                }
                else
                {
                    FlagGlassTransferRightPlaceDone = value;
                }
            }
        }
        #endregion

        #region Constructor
        public GlassTransferProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            GlassTransferRecipe glassTransferRecipe,
            [FromKeyedServices("GlassTransferInput")] IDInputDevice glassTransferInput,
            [FromKeyedServices("GlassTransferOutput")] IDOutputDevice glassTransferOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _glassTransferRecipe = glassTransferRecipe;
            _glassTransferInput = glassTransferInput;
            _glassTransferOutput = glassTransferOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessToRun()
        {
            switch ((EGlassTransferToRunStep)Step.ToRunStep)
            {
                case EGlassTransferToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EGlassTransferToRunStep.Clear_Flag:
                    Log.Debug("Clear Flag");
                    ((MappableOutputDevice<EGlassTransferProcessOutput>)_glassTransferOutput).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case EGlassTransferToRunStep.End:
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

        public override bool ProcessOrigin()
        {
            switch ((EGlassTransferOriginStep)Step.OriginStep)
            {
                case EGlassTransferOriginStep.Start:
                    Log.Debug("Origin Start");
                    Log.Debug("Wait Transfer In Shuttle Origin Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.Wait_TransferInShuttleOriginDone:
                    if (FlagTransferInShuttleOriginDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    CylUpDown(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylinderUp);
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.GlassTransfer_UpDownCylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.ZAxis_Origin:
                    Log.Debug("Glass Transfer Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => ZAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_ZAxis_Origin_Fail);
                        break;
                    }
                    Log.Debug("Glass Transfer Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Origin:
                    Log.Debug("Glass Transfer Y Axis Origin Start");
                    YAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => YAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_YAxis_Origin_Fail);
                        break;
                    }

                    Wait(200);
                    Log.Debug("Glass Transfer Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(_glassTransferRecipe.YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(_glassTransferRecipe.YAxisReadyPosition));
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Move_ReadyPositionWait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_YAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.End:
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
                case ESequence.GlassTransferPick:
                    // Pick from detach shuttle
                    Sequence_GlassTransferPick();
                    break;
                case ESequence.GlassTransferLeft:
                case ESequence.GlassTransferRight:
                    Sequence_GlassTransferPlace();
                    break;
                default:
                    Sequence = ESequence.Stop;
                    break;
            }

            return true;
        }
        #endregion

        #region Private Methods
        private void VacOnOff(bool bOnOff)
        {
            GlassVac1.Value = bOnOff;
            GlassVac2.Value = bOnOff;
            GlassVac3.Value = bOnOff;

            GlassBlow1.Value = !bOnOff;
            GlassBlow2.Value = !bOnOff;
            GlassBlow3.Value = !bOnOff;

            if (bOnOff == false)
            {
                Task.Delay(100).ContinueWith(t =>
                {
                    GlassBlow1.Value = false;
                    GlassBlow2.Value = false;
                    GlassBlow3.Value = false;
                });
            }
#if SIMULATION
            SimulationInputSetter.SetSimInput(_devices.Inputs.GlassTransferVac1, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.GlassTransferVac2, bOnOff);
            SimulationInputSetter.SetSimInput(_devices.Inputs.GlassTransferVac3, bOnOff);
#endif
        }

        private void CylUpDown(bool bUpDown)
        {
            if (bUpDown)
            {
                CylinderUpDown1.Backward();
                CylinderUpDown2.Backward();
                CylinderUpDown3.Backward();
            }
            else
            {
                CylinderUpDown1.Forward();
                CylinderUpDown2.Forward();
                CylinderUpDown3.Forward();
            }
        }

        private void Sequence_Ready()
        {
            switch ((EGlassTransferReadyStep)Step.RunStep)
            {
                case EGlassTransferReadyStep.Start:
                    Log.Debug("Initialize Start");
                    Step.RunStep++;
                    break;
                case EGlassTransferReadyStep.TransInShuttle_SafePos_Wait:
                    if (InFlag_TransfferInShuttleLeft_InSafePos == false &&
                        InFlag_TransfferInShuttleRight_InSafePos == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Both TransInShuttle_SafePos detect");
                    Step.RunStep++;
                    break;
                case EGlassTransferReadyStep.ZAxis_MoveReady_CylUp:
                    if (ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisReadyPosition) &&
                        CylinderUpDown1.IsBackward && CylinderUpDown2.IsBackward && CylinderUpDown3.IsBackward)
                    {
                        Step.RunStep = (int)EGlassTransferReadyStep.YAxis_Move_ReadyPosition;
                        break;
                    }

                    Log.Debug("Z Axis and Cylinders Move Ready Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisReadyPosition);
                    CylinderUpDown1.Backward();
                    CylinderUpDown2.Backward();
                    CylinderUpDown3.Backward();
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisReadyPosition) &&
                        CylinderUpDown1.IsBackward && CylinderUpDown2.IsBackward && CylinderUpDown3.IsBackward);

                    Step.RunStep++;
                    break;
                case EGlassTransferReadyStep.ZAxis_MoveReady_CylUp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (CylinderUpDown1.IsBackward == false ||
                            CylinderUpDown2.IsBackward == false ||
                            CylinderUpDown3.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.GlassTransfer_UpDownCylinder_Up_Fail);
                            break;
                        }

                        RaiseAlarm((int)EAlarm.GlassTransfer_ZAxis_MoveReadyPosition_Fail);
                        break;
                    }

                    Log.Debug("Z Axis and Cylinders Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferReadyStep.YAxis_Move_ReadyPosition:
                    if (YAxis.IsOnPosition(_glassTransferRecipe.YAxisReadyPosition))
                    {
                        Step.RunStep = (int)EGlassTransferReadyStep.End;
                        break;
                    }

                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(_glassTransferRecipe.YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(_glassTransferRecipe.YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferReadyStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_YAxis_MoveReadyPosition_Fail);
                        break;
                    }

                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferReadyStep.End:
                    Log.Debug("Initialize End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }
        private void Sequence_AutoRun()
        {
            switch ((EGlassTransferAutoRunStep)Step.RunStep)
            {
                case EGlassTransferAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EGlassTransferAutoRunStep.VacuumDetect_Check:
                    if (GlassTransferVac1 || GlassTransferVac2 || GlassTransferVac3)
                    {
                        Log.Info("Sequence Glass Transfer Place");
                        Sequence = ESequence.GlassTransferLeft;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EGlassTransferAutoRunStep.End:
                    Log.Info("Sequence Glass Transfer Pick");
                    Sequence = ESequence.GlassTransferPick;
                    break;
            }
        }

        private void Sequence_GlassTransferPick()
        {
            switch ((EGlassTransferPickStep)Step.RunStep)
            {
                case EGlassTransferPickStep.Start:
                    Log.Debug("Glass Transfer Pick Start");
                    Step.RunStep++;

                    Log.Debug("Wait Detach Unit Unload Request");
                    break;
                case EGlassTransferPickStep.Wait_DetachRequestUnload:
                    if (FlagDetachRequestUnloadGlass == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Received Detach Unit Unload Request");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Down:
                    Log.Debug("Cylinder Down");
                    CylUpDown(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylinderDown);
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.GlassTransfer_UpDownCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_MovePickPosition:
                    Log.Debug("Y Axis Move Pick Position");
                    YAxis.MoveAbs(_glassTransferRecipe.YAxisPickPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(_glassTransferRecipe.YAxisPickPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_MovePickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_YAxis_MovePickPosition_Fail);
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_MovePickPosition:
                    Log.Debug("Z Axis Move Pick Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisPickPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisPickPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_MovePickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_ZAxis_MovePickPosition_Fail);
                        break;
                    }
                    Log.Debug("Z Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    VacOnOff(true);
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.GlassTransfer_Vacuum_Fail);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_ZAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs((_glassTransferRecipe.YAxisReadyPosition));
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(_glassTransferRecipe.YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_YAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    CylUpDown(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylinderUp);
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.GlassTransfer_UpDownCylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Set_FlagPickDone:
                    Log.Debug("Set Flag Glass Transfer Pick Done");
                    FlagGlassTransferPickDone = true;
                    Log.Debug("Wait Detach Glass Transfer Pick Done Received");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Wait_DetachGlassTransferPickDone_Received:
                    if (FlagDetachRequestUnloadGlass == true)
                    {
                        // Wait until Detach clears the request unload signal
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Glass Transfer Pick Done");
                    FlagGlassTransferPickDone = false;
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Glass Transfer Place");
                    Sequence = ESequence.GlassTransferLeft;
                    break;
            }
        }

        private void Sequence_GlassTransferPlace()
        {
            switch ((EGlassTransferPlaceStep)Step.RunStep)
            {
                case EGlassTransferPlaceStep.Start:
                    Log.Debug("Glass Transfer Place Start");
                    Step.RunStep++;

                    if (Parent?.Sequence != ESequence.AutoRun && (GlassTransferVac1 || GlassTransferVac2 || GlassTransferVac3) == false)
                    {
                        break;
                    }

                    Log.Debug("Wait TransferIn Shuttle Request Glass");
                    break;
                case EGlassTransferPlaceStep.Wait_Glass_AlignRequestGlass:
                    if (InFlag_TransferInShuttleLeftRequestGlass)
                    {
                        Log.Debug("Received TransferIn Shuttle Left Request Glass");
                        currentPlacePort = EPort.Left;
                        Step.RunStep++;
                        break;
                    }
                    if (InFlag_TransfferInShuttleRightRequestGlass)
                    {
                        Log.Debug("Received TransferIn Shuttle Right Request Glass");
                        currentPlacePort = EPort.Right;
                        Step.RunStep++;
                        break;
                    }

                    Wait(20);
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_PlacePosition:
                    Log.Debug("Y Axis Move Place Position");
                    YAxis.MoveAbs(YAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_YAxis_MovePlacePosition_Fail);
                        break;
                    }
                    Log.Debug("Y Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Down:
                    Log.Debug("Cylinder Down");
                    CylUpDown(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylinderDown);
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.GlassTransfer_UpDownCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_PlacePosition:
                    Log.Debug("Z Axis Move Place Position");
                    ZAxis.MoveAbs(ZAxisPlacePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(ZAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_ZAxis_MovePlacePosition_Fail);
                        break;
                    }
                    Log.Debug("Z Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    VacOnOff(false);
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect == false);
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_ZAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    CylUpDown(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylinderUp);
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.GlassTransfer_UpDownCylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(_glassTransferRecipe.YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition((_glassTransferRecipe.YAxisReadyPosition)));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.GlassTransfer_YAxis_MoveReadyPosition_Fail);
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Set_Flag_PlaceDone:
                    Log.Debug("Set Flag Place Done");
                    FlagPlaceDone = true;
                    Log.Debug("Wait Glass Align Place Done Received");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Wait_GlassAlignPlaceDoneReceived:
                    bool DownStreamRequestFlag = currentPlacePort == EPort.Left ? InFlag_TransferInShuttleLeftRequestGlass : InFlag_TransfferInShuttleRightRequestGlass;
                    if (DownStreamRequestFlag == true)
                    {
                        // Wait until Glass Align clears the request glass signal
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Place Done");
                    FlagPlaceDone = false;
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.End:
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

        #endregion
    }
}
