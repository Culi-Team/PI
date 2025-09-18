using EQX.Core.Common;
using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Core.Units;
using EQX.InOut;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class WorkConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private EPort port => Name == EProcess.InWorkConveyor.ToString() ? EPort.Right : EPort.Left;
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _inWorkConveyorInput;
        private readonly IDOutputDevice _inWorkConveyorOutput;
        private readonly IDInputDevice _outWorkConveyorInput;
        private readonly IDOutputDevice _outWorkConveyorOutput;
        private readonly ActionAssignableTimer _blinkTimer;
        private readonly CassetteList _cassetteList;
        private const string OutLightCurtainMutingActionKey = "OutLightCurtainMutingAction";

        private IDInputDevice Inputs => port == EPort.Right ? _inWorkConveyorInput : _outWorkConveyorInput;
        private IDOutputDevice Outputs => port == EPort.Right ? _inWorkConveyorOutput : _outWorkConveyorOutput;

        private double TAxisWorkPosition => port == EPort.Right ? _cstLoadUnloadRecipe.InCstTAxisWorkPosition : _cstLoadUnloadRecipe.OutCstTAxisWorkPosition;
        private double TAxisLoadPosition => port == EPort.Right ? _cstLoadUnloadRecipe.InCstTAxisLoadPosition : _cstLoadUnloadRecipe.OutCstTAxisLoadPosition;

        private ITray<ETrayCellStatus> Cassette => port == EPort.Right ? _cassetteList.CassetteIn : _cassetteList.CassetteOut;
        #endregion

        #region Constructor
        public WorkConveyorProcess(Devices devices,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            CommonRecipe commonRecipe,
            [FromKeyedServices("InWorkConveyorInput")] IDInputDevice inWorkConveyorInput,
            [FromKeyedServices("InWorkConveyorOutput")] IDOutputDevice inWorkConveyorOutput,
            [FromKeyedServices("OutWorkConveyorInput")] IDInputDevice outWorkConveyorInput,
            [FromKeyedServices("OutWorkConveyorOutput")] IDOutputDevice outWorkConveyorOutput,
            [FromKeyedServices("BlinkTimer")] ActionAssignableTimer blinkTimer,
            CassetteList cassetteList)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
            _inWorkConveyorInput = inWorkConveyorInput;
            _inWorkConveyorOutput = inWorkConveyorOutput;
            _outWorkConveyorInput = outWorkConveyorInput;
            _outWorkConveyorOutput = outWorkConveyorOutput;
            _blinkTimer = blinkTimer;
            _cassetteList = cassetteList;
        }
        #endregion

        #region Inputs
        private IDInput Detect1 => port == EPort.Right ? _devices.Inputs.InCstWorkDetect1 :
                                                         _devices.Inputs.OutCstWorkDetect1;
        private IDInput Detect2 => port == EPort.Right ? _devices.Inputs.InCstWorkDetect2 :
                                                         _devices.Inputs.OutCstWorkDetect2;
        private IDInput Detect3 => port == EPort.Right ? _devices.Inputs.InCstWorkDetect3 :
                                                        _devices.Inputs.OutCstWorkDetect3;
        private IDInput Detect4 => _devices.Inputs.InCstWorkDetect4;

        private bool IsCassetteDetect
        {
            get
            {
                if (port == EPort.Right) // InWorkConveyor
                {
                    return Detect1.Value && Detect2.Value && Detect3.Value && Detect4.Value;
                }
                else // OutWorkConveyor
                {
                    return Detect1.Value && Detect2.Value && Detect3.Value;
                }
            }
        }

        private bool IsCassetteOut
        {
            get
            {
                if (port == EPort.Right)
                {
                    return Detect1.Value == false && Detect2.Value == false && Detect3.Value == false && Detect4.Value == false;
                }
                return Detect1.Value == false && Detect2.Value == false && Detect3.Value == false;
            }
        }

        private bool IsNextConveyorDetect
        {
            get
            {
                if (port == EPort.Right)
                {
                    return _devices.Inputs.BufferCstDetect1.Value && _devices.Inputs.BufferCstDetect2.Value;
                }
                return _devices.Inputs.OutCstDetect1.Value && _devices.Inputs.OutCstDetect2.Value;
            }
        }
        #endregion

        #region Outputs
        #endregion

        #region Cylinders
        private ICylinder FixCylinder1 => port == EPort.Right ? _devices.Cylinders.InCstFixCyl1FwBw :
                                                              _devices.Cylinders.OutCstFixCyl1FwBw;
        private ICylinder FixCylinder2 => port == EPort.Right ? _devices.Cylinders.InCstFixCyl2FwBw :
                                                              _devices.Cylinders.OutCstFixCyl2FwBw;
        private ICylinder TiltCylinder => port == EPort.Right ? _devices.Cylinders.InCstTiltCylUpDown :
                                                               _devices.Cylinders.OutCstTiltCylUpDown;
        private ICylinder CVSupportCyl1 => port == EPort.Right ? _devices.Cylinders.InCvSupportUpDown :
                                                                _devices.Cylinders.OutCvSupportBufferUpDown;
        private ICylinder CVSupportCyl2 => port == EPort.Right ? _devices.Cylinders.InCvSupportBufferUpDown :
                                                                _devices.Cylinders.OutCvSupportUpDown;
        #endregion

        #region Flags
        private bool FlagCSTReady
        {
            set
            {
                Outputs[(int)EWorkConveyorProcessOutput.CST_READY] = value;
            }
        }

        private bool FlagRobotPickPlaceDone
        {
            get
            {
                return Inputs[(int)EWorkConveyorProcessInput.ROBOT_PICK_PLACE_CST_DONE];
            }
        }

        private bool FlagInOutCSTPickPlaceDoneReceived
        {
            set
            {
                Outputs[(int)EWorkConveyorProcessOutput.IN_CST_PICK_PLACE_DONE_RECEIVED] = value;
            }
        }

        private bool FlagRequestCSTIn
        {
            set
            {
                Outputs[(int)EWorkConveyorProcessOutput.REQUEST_CST_IN] = value;
            }
        }

        private bool FlagRequestCSTOut
        {
            set
            {
                Outputs[(int)EWorkConveyorProcessOutput.REQUEST_CST_OUT] = value;
            }
        }

        private bool FlagNextConveyorReady
        {
            get
            {
                return Inputs[(int)EWorkConveyorProcessInput.NEXT_CONVEYOR_READY];
            }
        }
        #endregion

        #region Motions
        private IMotion TAxis => port == EPort.Right ? _devices.MotionsInovance.InCassetteTAxis :
                                                           _devices.MotionsInovance.OutCassetteTAxis;
        #endregion

        #region Rollers
        private ISpeedController RollerSupport1 => port == EPort.Right ? _devices.SpeedControllerList.SupportConveyor1Roller :
                                                                        _devices.SpeedControllerList.SupportConveyor3Roller;

        private ISpeedController RollerSupport2 => port == EPort.Right ? _devices.SpeedControllerList.SupportConveyor2Roller :
                                                                        _devices.SpeedControllerList.SupportConveyor4Roller;

        private ISpeedController Roller1 => port == EPort.Right ? _devices.SpeedControllerList.InWorkConveyorRoller1 :
                                                                 _devices.SpeedControllerList.OutWorkConveyorRoller1;
        private ISpeedController Roller2 => port == EPort.Right ? _devices.SpeedControllerList.InWorkConveyorRoller2 :
                                                                 _devices.SpeedControllerList.OutWorkConveyorRoller2;
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EWorkConveyorOriginStep)Step.OriginStep)
            {
                case EWorkConveyorOriginStep.Start:
                    Log.Debug("Start");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Tilt_Down:
                    Log.Debug("Cylinder Tilt Down");
                    TiltCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TiltCylinder.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Tilt_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_TiltCylinder_Down_Fail :
                                                           EWarning.OutWorkConveyor_TiltCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Tilt Down Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.SupportCV_Down:
                    Log.Debug("Support CV Down");
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.SupportCV_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Down_Fail :
                                                           EWarning.OutWorkConveyor_SupportCV_Down_Fail));
                        break;
                    }
                    Log.Debug("Support CV Down Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.TAxis_Origin:
                    Log.Debug("T Axis Origin");
                    TAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => TAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.TAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Right ? EAlarm.InWorkConveyor_TAxis_Origin_Fail :
                                                         EAlarm.OutWorkConveyor_TAxis_Origin_Fail));
                        break;
                    }
                    Log.Debug("T Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Fix_Backward:
                    Log.Debug("Cylinder Fix Backward");
                    FixCylinder1.Backward();
                    FixCylinder2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCylinder1.IsBackward && FixCylinder2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_FixCylinder_Backward_Fail :
                                                           EWarning.OutWorkConveyor_FixCylinder_Backward_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Fix Backward Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorStop();
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.End:
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
                case ESequence.InConveyorLoad:
                    break;
                case ESequence.InWorkCSTLoad:
                    Sequence_Load();
                    break;
                case ESequence.InWorkCSTUnLoad:
                    Sequence_Unload();
                    break;
                case ESequence.CSTTilt:
                    Sequence_Tilt();
                    break;
                case ESequence.OutWorkCSTLoad:
                    Sequence_Load();
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    Sequence_Unload();
                    break;
                case ESequence.OutConveyorUnload:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    Sequence_PickPlace();
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
                    Sequence_PickPlace();
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
        private void Sequence_AutoRun()
        {
            switch ((EWorkConveyorAutoRunStep)Step.RunStep)
            {
                case EWorkConveyorAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EWorkConveyorAutoRunStep.Cassette_Check:
                    if (IsCassetteDetect)
                    {
                        Log.Info("Sequence Tilt");
                        Sequence = ESequence.CSTTilt;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyorAutoRunStep.End:
                    Log.Info("Sequence Cassette Load");
                    if (port == EPort.Right)
                    {
                        Log.Info("Sequence In Work CST Load");
                        Sequence = ESequence.InWorkCSTLoad;
                        break;
                    }
                    Log.Info("Sequence Out Work CST Load");
                    Sequence = ESequence.OutWorkCSTLoad;
                    break;
            }
        }

        private void Sequence_Tilt()
        {
            switch ((EWorkConveyorTiltStep)Step.RunStep)
            {
                case EWorkConveyorTiltStep.Start:
                    Log.Debug("Tilt Start");
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.Cyl_SupportCV_Down:
                    Log.Debug("Cylinder Support Conveyor Down");
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.Cyl_SupportCV_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Down_Fail :
                                                           EWarning.OutWorkConveyor_SupportCV_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Support Conveyor Down Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.Cyl_Fix_Forward:
                    Log.Debug("Cylinder Fix Forward");
                    FixCylinder1.Forward();
                    FixCylinder2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCylinder1.IsForward && FixCylinder2.IsForward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.Cyl_Fix_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_FixCylinder_Forward_Fail :
                                                           EWarning.OutWorkConveyor_FixCylinder_Forward_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Fix Forward Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.TAxis_Move_WorkPosition:
                    Log.Debug("T Axis Move Work Position");
                    TAxis.MoveAbs(TAxisWorkPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => TAxis.IsOnPosition(TAxisWorkPosition));
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.TAxis_Move_WorkPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Right ? EAlarm.InWorkConveyor_TAxis_MoveWorkPosition_Fail :
                                                         EAlarm.OutWorkConveyor_TAxis_MoveWorkPosition_Fail));
                        break;
                    }
                    Log.Debug("T Axis Move Work Position Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.Cyl_Tilt_Up:
                    Log.Debug("Cylinder Tilt Up");
                    TiltCylinder.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TiltCylinder.IsForward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.Cyl_Tilt_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_TiltCylinder_Up_Fail :
                                                           EWarning.OutWorkConveyor_TiltCylinder_Up_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Tilt Up Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorTiltStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (port == EPort.Right)
                    {
                        Log.Info("Robot Pick Fixture From In CST");
                        Sequence = ESequence.RobotPickFixtureFromCST;
                        break;
                    }
                    Log.Info("Robot Place Fixture To Out CST");
                    Sequence = ESequence.RobotPlaceFixtureToOutWorkCST;
                    break;
            }
        }

        private void Sequence_PickPlace()
        {
            switch ((EWorkConveyorPickPlaceStep)Step.RunStep)
            {
                case EWorkConveyorPickPlaceStep.Start:
                    Log.Debug((port == EPort.Right ? "Pick" : "Place") + "Start");
                    Step.RunStep++;
                    break;
                case EWorkConveyorPickPlaceStep.Cassette_Detect_Check:
                    if (IsCassetteDetect == false)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyorCSTNotDetect :
                                                                 EWarning.OutWorkConveyorCSTNotDetect));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyorPickPlaceStep.Cassette_WorkCondition_Check:
                    Log.Debug("Cassette Work Condition Check");
                    Step.RunStep++;
                    break;
                case EWorkConveyorPickPlaceStep.Set_FlagReady:
                    Log.Debug("Set Flag Ready");
                    FlagCSTReady = true;
                    FlagInOutCSTPickPlaceDoneReceived = false;
                    Step.RunStep++;
                    Log.Debug("Wait Robot" + (port == EPort.Right ? "Pick" : "Place") + " Done");
                    break;
                case EWorkConveyorPickPlaceStep.Wait_Robot_PickPlace_Done:
                    if (FlagRobotPickPlaceDone == false)
                    {
                        break;
                    }
                    Log.Debug("Set Flag In Out Cassette Pick Place Done Received");
                    FlagInOutCSTPickPlaceDoneReceived = true;
                    Log.Debug("Clear Flag Ready");
                    FlagCSTReady = false;
                    Step.RunStep = (int)EWorkConveyorPickPlaceStep.Cassette_WorkCondition_Check;
                    break;
                case EWorkConveyorPickPlaceStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (port == EPort.Right)
                    {
                        Log.Info("Sequence In Work CST Unload");
                        Sequence = ESequence.InWorkCSTUnLoad;
                        break;
                    }
                    Log.Info("Sequence Out Work CST Unload");
                    Sequence = ESequence.OutWorkCSTUnLoad;
                    break;
            }
        }

        private void Sequence_Load()
        {
            switch ((EWorkConveyorProcessLoadStep)Step.RunStep)
            {
                case EWorkConveyorProcessLoadStep.Start:
                    Log.Debug("Load Start");
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Cyl_Tilt_Down:
                    Log.Debug("Cylinder Tilt Down");
                    TiltCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TiltCylinder.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Cyl_Tilt_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_TiltCylinder_Down_Fail :
                                                           EWarning.OutWorkConveyor_TiltCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Tilt Down Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.TAxis_Move_LoadPosition:
                    Log.Debug("T Axis Move Load Position");
                    TAxis.MoveAbs(TAxisLoadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => TAxis.IsOnPosition(TAxisLoadPosition));
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.TAxis_Move_MoveLoadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Right ? EAlarm.InWorkConveyor_TAxis_MoveLoadPosition_Fail :
                                                         EAlarm.OutWorkConveyor_TAxis_MoveLoadPosition_Fail));
                        break;
                    }
                    Log.Debug("T Axis Move Load Position Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Cyl_Fix_Backward:
                    Log.Debug("Cylinder Fix Backward");
                    FixCylinder1.Backward();
                    FixCylinder2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCylinder1.IsBackward && FixCylinder2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_FixCylinder_Backward_Fail :
                                                           EWarning.OutWorkConveyor_FixCylinder_Backward_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Fix Backward Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Support_CV_Up:
                    Log.Debug("Support Conveyor In Up");
                    CVSupportCyl1.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CVSupportCyl1.IsForward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Support_CV_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Up_Fail :
                                                           EWarning.OutWorkConveyor_SupportCV_Up_Fail));
                        break;
                    }
                    Log.Debug("Support Conveyor In Up Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Conveyor_Run:
                    Log.Debug("Conveyor Run In");
                    ConveyorRunInOut(true);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Set_FlagRequestCassetteIn:
                    Log.Debug("Set Flag Request Cassette In");
                    FlagRequestCSTIn = true;
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Wait_CassetteLoadDone:
                    if (IsCassetteDetect == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Request Cassette In");
                    FlagRequestCSTIn = false;
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorStop();
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.End:
                    Log.Debug("Load End");
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence CST Tilt");
                    Sequence = ESequence.CSTTilt;
                    break;
            }
        }

        private void Sequence_Unload()
        {
            switch ((EWorkConveyorUnloadStep)Step.RunStep)
            {
                case EWorkConveyorUnloadStep.Start:
                    Log.Debug("Unload Start");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Cyl_Tilt_Down:
                    Log.Debug("Cylinder Tilt Down");
                    TiltCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TiltCylinder.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Cyl_Tilt_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_TiltCylinder_Down_Fail :
                                                           EWarning.OutWorkConveyor_TiltCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Tilt Down Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Support_CV_Down:
                    Log.Debug("Support Conveyor Down");
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Support_CV_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Down_Fail :
                                                           EWarning.OutWorkConveyor_SupportCV_Down_Fail));
                        break;
                    }
                    Log.Debug("Support Conveyor Down Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.TAxis_MoveLoadPosition:
                    Log.Debug("T Axis Move Load Position");
                    TAxis.MoveAbs(TAxisLoadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => TAxis.IsOnPosition(TAxisLoadPosition));
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.TAxis_MoveLoadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Right ? EAlarm.InWorkConveyor_TAxis_MoveLoadPosition_Fail :
                                                         EAlarm.OutWorkConveyor_TAxis_MoveLoadPosition_Fail));
                        break;
                    }
                    Log.Debug("T Axis Move Load Position Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Support_CV_Up:
                    Log.Debug("Support Conveyor Up");
                    CVSupportCyl1.Forward();
                    CVSupportCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CVSupportCyl1.IsForward && CVSupportCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Support_CV_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Up_Fail :
                                                           EWarning.OutWorkConveyor_SupportCV_Up_Fail));
                        break;
                    }
                    Log.Debug("Support Conveyor Up Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Cyl_Fix_Backward:
                    Log.Debug("Cylinder Fix Backward");
                    FixCylinder1.Backward();
                    FixCylinder2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCylinder1.IsBackward && FixCylinder2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_FixCylinder_Backward_Fail :
                                                           EWarning.OutWorkConveyor_FixCylinder_Backward_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Fix Backward Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Set_FlagRequestCSTOut:
                    Log.Debug("Set Flag Request CST Out");
                    FlagRequestCSTOut = true;
                    Log.Debug("Wait Next Conveyor Ready");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Wait_NextConveyorReady:
                    if (FlagNextConveyorReady == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Muting_LightCurtain:
                    if (port == EPort.Left)
                    {
                        Log.Debug("Muting Light Curtain");
                        _devices.Outputs.OutCstLightCurtainMuting1.Value = true;
                        _devices.Outputs.OutCstLightCurtainMuting2.Value = true;

                        _blinkTimer.EnableAction(OutLightCurtainMutingActionKey,
                            () => _devices.Outputs.OutMutingButtonLamp.Value = true,
                            () => _devices.Outputs.OutMutingButtonLamp.Value = false);
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Conveyor_Run:
                    ConveyorRunInOut(false);
#if SIMULATION
                    Wait(3000);
                    if (port == EPort.Right)
                    {
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect1, false);
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect2, false);
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect3, false);
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect4, false);

                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.BufferCstDetect1, true);
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.BufferCstDetect2, true);
                    }
                    else
                    {
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.OutCstWorkDetect1, false);
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.OutCstWorkDetect2, false);
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.OutCstWorkDetect3, false);

                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.OutCstDetect1, true);
                        SimulationInputSetter.SetSimModbusInput(_devices.Inputs.OutCstDetect2, true);
                    }
#endif
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Wait_CSTOut_Done:
                    if (IsCassetteOut && IsNextConveyorDetect)
                    {
                        if (port == EPort.Left)
                        {
                            Log.Debug("Enable Light Curtain");
                            _devices.Outputs.OutCstLightCurtainMuting1.Value = false;
                            _devices.Outputs.OutCstLightCurtainMuting2.Value = false;

                            _blinkTimer.DisableAction(OutLightCurtainMutingActionKey);
                        }
                        Step.RunStep++;
                        break;
                    }
                    Wait(20);
                    break;
                case EWorkConveyorUnloadStep.End:
                    Log.Debug("Unload End");
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (port == EPort.Right)
                    {
                        Log.Info("Sequence In Work CST Load");
                        Sequence = ESequence.InWorkCSTLoad;
                        break;
                    }
                    Log.Info("Sequence Out Work CST Load");
                    Sequence = ESequence.OutWorkCSTLoad;
                    break;
            }
        }

        private void ConveyorRunInOut(bool bIn)
        {
            if (bIn)
            {
                Roller1.Start();
                Roller2.Start();
                RollerSupport1.Start();
            }
            else
            {
                Roller1.Start();
                Roller2.Start();
                RollerSupport2.Start();
            }
        }

        private void ConveyorStop()
        {
            Roller1.Stop();
            Roller2.Stop();
            RollerSupport1.Stop();
            RollerSupport2.Stop();
        }
        #endregion
    }
}
