using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
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

        private EPort currentPlacePort;

        private IMotion YAxis => _devices.MotionsInovance.GlassTransferYAxis;
        private IMotion ZAxis => _devices.MotionsInovance.GlassTransferZAxis;

        private IDOutput GlassVac1 => _devices.Outputs.GlassTransferVac1OnOff;
        private IDOutput GlassVac2 => _devices.Outputs.GlassTransferVac2OnOff;
        private IDOutput GlassVac3 => _devices.Outputs.GlassTransferVac3OnOff;

        private ICylinder CylinderUpDown1 => _devices.Cylinders.GlassTransferCyl1UpDown;
        private ICylinder CylinderUpDown2 => _devices.Cylinders.GlassTransferCyl2UpDown;
        private ICylinder CylinderUpDown3 => _devices.Cylinders.GlassTransferCyl3UpDown;
        private bool IsCylinderUp => CylinderUpDown1.IsBackward && CylinderUpDown2.IsBackward && CylinderUpDown3.IsBackward;
        private bool IsCylinderDown => CylinderUpDown1.IsForward && CylinderUpDown2.IsForward && CylinderUpDown3.IsForward;

        private bool IsVac1Detect => _devices.Inputs.GlassTransferVac1.Value;
        private bool IsVac2Detect => _devices.Inputs.GlassTransferVac2.Value;
        private bool IsVac3Detect => _devices.Inputs.GlassTransferVac3.Value;

        private bool IsVacDetect => IsVac1Detect && IsVac2Detect && IsVac3Detect;

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
        private bool FlagDetachRequestUnloadGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.DETACH_REQ_UNLOAD_GLASS];
            }
        }

        private bool FlagGlassAlignLeftRequestGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_REQ_GLASS];
            }
        }

        private bool FlagGlassAlignRightRequestGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_REQ_GLASS];
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
                if(currentPlacePort == EPort.Left)
                {
                    FlagGlassTransferLeftPlaceDone = value;
                }
                else
                {
                    FlagGlassTransferRightPlaceDone = value;
                }
            }
        }

        private bool FlagDetachGlassTransferPickDoneReceived
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.GLASS_TRANSFER_PICK_DONE_RECEIVED];
            }
        }

        private bool FlagGlassAlignPlaceDoneReceived
        {
            get
            {
                if(currentPlacePort == EPort.Left)
                {
                    return _glassTransferInput[(int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_PLACE_DONE_RECEIVED];
                }
                return _glassTransferInput[(int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_PLACE_DONE_RECEIVED];
            }
        }
        #endregion

        #region Constructor
        public GlassTransferProcess(Devices devices,
            CommonRecipe commonRecipe,
            GlassTransferRecipe glassTransferRecipe,
            [FromKeyedServices("GlassTransferInput")] IDInputDevice glassTransferInput,
            [FromKeyedServices("GlassTransferOutput")] IDOutputDevice glassTransferOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
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
                case EGlassTransferToRunStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisReadyPosition));
                    Step.ToRunStep++;
                    break;
                case EGlassTransferToRunStep.ZAxis_Move_ReadyPosition_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.ToRunStep++;
                    break;
                case EGlassTransferToRunStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(_glassTransferRecipe.YAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => YAxis.IsOnPosition(_glassTransferRecipe.YAxisReadyPosition));
                    Step.ToRunStep++;
                    break;
                case EGlassTransferToRunStep.YAxis_Move_ReadyPosition_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.ToRunStep++;
                    break;
                case EGlassTransferToRunStep.Clear_Flag:
                    Log.Debug("Clear Flag");
                    ((VirtualOutputDevice<EGlassTransferProcessOutput>)_glassTransferOutput).Clear();
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
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    CylUpDown(true);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsCylinderUp);
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.Cyl_Up_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.ZAxis_Origin:
                    Log.Debug("Glass Transfer Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return ZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Glass Transfer Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Origin:
                    Log.Debug("Glass Transfer Y Axis Origin Start");
                    YAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return YAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Glass Transfer Y Axis Origin Done");
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
                    Sequence_GlassTransferPick();
                    break;
                case ESequence.GlassTransferPlace:
                    Sequence_GlassTransferPlace();
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
        private void VacOnOff(bool bOnOff)
        {
            GlassVac1.Value = bOnOff;
            GlassVac2.Value = bOnOff;
            GlassVac3.Value = bOnOff;
        }

        private void CylUpDown(bool bUpDown)
        {
            if(bUpDown)
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

        private void Sequence_AutoRun()
        {
            switch ((EGlassTransferAutoRunStep)Step.RunStep)
            {
                case EGlassTransferAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EGlassTransferAutoRunStep.VacuumDetect_Check:
                    if(IsVacDetect)
                    {
                        Log.Info("Sequence Glass Transfer Place");
                        Sequence = ESequence.GlassTransferPlace;
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
                    Log.Debug("Wait Detach Request Unload");
                    break;
                case EGlassTransferPickStep.Wait_DetachRequestUnload:
                    if (FlagDetachRequestUnloadGlass == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Down:
                    Log.Debug("Cylinder Down");
                    CylUpDown(false);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsCylinderDown);
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_MovePickPosition:
                    Log.Debug("Y Axis Move Pick Position");
                    YAxis.MoveAbs(_glassTransferRecipe.YAxisPickPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => YAxis.IsOnPosition(_glassTransferRecipe.YAxisPickPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_MovePickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_MovePickPosition:
                    Log.Debug("Z Axis Move Pick Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisPickPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisPickPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_MovePickPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Pick Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    VacOnOff(true);
                    Wait(_commonRecipe.VacDelay, () => IsVacDetect);
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs((_glassTransferRecipe.YAxisReadyPosition));
                    Wait(_commonRecipe.MotionMoveTimeOut, () => YAxis.IsOnPosition(_glassTransferRecipe.YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    CylUpDown(true);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsCylinderUp);
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    if (FlagDetachGlassTransferPickDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Glass Transfer Pick Done");
                    FlagGlassTransferPickDone = false;
                    Step.RunStep++;
                    break;
                case EGlassTransferPickStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Glass Transfer Place");
                    Sequence = ESequence.GlassTransferPlace;
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

                    Log.Debug("Wait Glass Align Request Glass");
                    break;
                case EGlassTransferPlaceStep.Wait_Glass_AlignRequestGlass:
                    if(FlagGlassAlignLeftRequestGlass)
                    {
                        Log.Debug("Place To Glass Align Left");
                        currentPlacePort = EPort.Left;
                        Step.RunStep++;
                        break;
                    }
                    if(FlagGlassAlignRightRequestGlass)
                    {
                        Log.Debug("Place To Glass Align Right");
                        currentPlacePort = EPort.Right;
                        Step.RunStep++;
                        break;
                    }
                    Wait(20);
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_PlacePosition:
                    Log.Debug("Y Axis Move Place Position");
                    YAxis.MoveAbs(YAxisPlacePosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => YAxis.IsOnPosition(YAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Down:
                    Log.Debug("Cylinder Down");
                    CylUpDown(false);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsCylinderDown);
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Down_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_PlacePosition:
                    Log.Debug("Z Axis Move Place Position");
                    ZAxis.MoveAbs(ZAxisPlacePosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => YAxis.IsOnPosition(ZAxisPlacePosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_PlacePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Place Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    VacOnOff(false);
                    Wait(_commonRecipe.VacDelay, () => IsVacDetect == false);
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_ReadyPosition:
                    Log.Debug("Z Axis Move Ready Position");
                    ZAxis.MoveAbs(_glassTransferRecipe.ZAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => ZAxis.IsOnPosition(_glassTransferRecipe.ZAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.ZAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    CylUpDown(true);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsCylinderUp);
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.Cyl_Up_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_ReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(_glassTransferRecipe.YAxisReadyPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut,() => YAxis.IsOnPosition((_glassTransferRecipe.YAxisReadyPosition)));
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.YAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    if(FlagGlassAlignPlaceDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Place Done");
                    FlagPlaceDone = false;
                    Step.RunStep++;
                    break;
                case EGlassTransferPlaceStep.End:
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

        #endregion
    }
}
