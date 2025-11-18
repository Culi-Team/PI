using EQX.Core.Common;
using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Core.Units;
using EQX.Device.SpeedController;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using PIFilmAutoDetachCleanMC.Helpers;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Windows.Markup;

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
        private readonly MachineStatus _machineStatus;
        private const string OutLightCurtainMutingActionKey = "OutLightCurtainMutingAction";

        private IDInputDevice Inputs => port == EPort.Right ? _inWorkConveyorInput : _outWorkConveyorInput;
        private IDOutputDevice Outputs => port == EPort.Right ? _inWorkConveyorOutput : _outWorkConveyorOutput;
        private IAInput LaserSensor => _devices.AnalogInputs.Laser;

        private double TAxisWorkPosition => port == EPort.Right ? _cstLoadUnloadRecipe.InCstTAxisWorkPosition : _cstLoadUnloadRecipe.OutCstTAxisWorkPosition;
        private double TAxisLoadPosition => port == EPort.Right ? _cstLoadUnloadRecipe.InCstTAxisLoadPosition : _cstLoadUnloadRecipe.OutCstTAxisLoadPosition;
        private double TAxisUnloadPosition => port == EPort.Right ? _cstLoadUnloadRecipe.InCstTAxisUnloadPosition : _cstLoadUnloadRecipe.OutCstTAxisUnloadPosition;
        private ITray<ETrayCellStatus> Cassette => port == EPort.Right ? _cassetteList.CassetteIn : _cassetteList.CassetteOut;
        private bool CassetteWorkDone => Cassette.Cells.Any(c => c.Status == ETrayCellStatus.Ready || c.Status == ETrayCellStatus.Working) == false;
        private double DistanceFirstFixture => AnalogConverter.Convert(LaserSensor.Volt, 0, 8191.0, 0.0, 800.0);
        #endregion

        #region Constructor
        public WorkConveyorProcess(Devices devices,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
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
            _machineStatus = machineStatus;
            _inWorkConveyorInput = inWorkConveyorInput;
            _inWorkConveyorOutput = inWorkConveyorOutput;
            _outWorkConveyorInput = outWorkConveyorInput;
            _outWorkConveyorOutput = outWorkConveyorOutput;
            _blinkTimer = blinkTimer;
            _cassetteList = cassetteList;
        }
        #endregion

        #region Inputs
        private bool Detect1 => port == EPort.Right ? _devices.Inputs.InCstWorkDetect1.Value :
                                                         _devices.Inputs.OutCstWorkDetect1.Value;
        private bool Detect2 => port == EPort.Right ? _devices.Inputs.InCstWorkDetect2.Value :
                                                         _devices.Inputs.OutCstWorkDetect2.Value;
        private bool Detect3 => port == EPort.Right ? _devices.Inputs.InCstWorkDetect3.Value :
                                                         _devices.Inputs.OutCstWorkDetect3.Value;

        private bool IsCassetteDetect
        {
            get
            {
                return Detect1 && Detect2 && Detect3;
            }
        }

        private bool IsCassetteOut
        {
            get
            {
                return Detect1 == false && Detect2 == false && Detect3 == false;
            }
        }

        private bool IsNextConveyorDetect
        {
            get
            {
                if (port == EPort.Right)
                {
                    return _devices.Inputs.BufferCstDetect1.Value
                        && _devices.Inputs.BufferCstDetect2.Value;
                }
                return _devices.Inputs.OutCstDetect1.Value
                    && _devices.Inputs.OutCstDetect2.Value;
            }
        }

        #endregion

        #region Outputs
        #endregion

        #region Cylinders
        private ICylinder AlignCylinder1 => port == EPort.Right ? _devices.Cylinders.InWorkCV_AlignCyl1 :
                                                              _devices.Cylinders.OutWorkCV_AlignCyl1;
        private ICylinder AlignCylinder2 => port == EPort.Right ? _devices.Cylinders.InWorkCV_AlignCyl2 :
                                                              _devices.Cylinders.OutWorkCV_AlignCyl2;
        private ICylinder TiltCylinder => port == EPort.Right ? _devices.Cylinders.InWorkCV_TiltCyl :
                                                               _devices.Cylinders.OutWorkCV_TiltCyl;
        private ICylinder CVSupportCyl1 => port == EPort.Right ? _devices.Cylinders.InWorkCV_SupportCyl1 :
                                                                _devices.Cylinders.OutWorkCV_SupportCyl1;
        private ICylinder CVSupportCyl2 => port == EPort.Right ? _devices.Cylinders.InWorkCV_SupportCyl2 :
                                                                _devices.Cylinders.OutWorkCV_SupportCyl2;
        #endregion

        #region Flags
        private bool FlagRobotOriginDone
        {
            get
            {
                return Inputs[(int)EWorkConveyorProcessInput.ROBOT_ORIGIN_DONE];
            }
        }
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

        private bool FlagDownStreamReady
        {
            get
            {
                return Inputs[(int)EWorkConveyorProcessInput.DOWN_STREAM_READY];
            }
        }

        private bool IsUpStreamCSTExist
        {
            get
            {
                if (port == EPort.Left)
                {
                    return _devices.Inputs.BufferCstDetect1.Value || _devices.Inputs.BufferCstDetect2.Value;
                }

                return _devices.Inputs.InCstDetect1.Value || _devices.Inputs.InCstDetect2.Value;

            }
        }
        #endregion

        #region Motions
        private IMotion TAxis => port == EPort.Right ? _devices.Motions.InCassetteTAxis :
                                                       _devices.Motions.OutCassetteTAxis;
        #endregion

        #region Rollers
        private BD201SRollerController RollerSupport1 => port == EPort.Right ? _devices.RollerList.SupportConveyorRoller1 :
                                                                        _devices.RollerList.SupportConveyorRoller3;

        private BD201SRollerController RollerSupport2 => port == EPort.Right ? _devices.RollerList.SupportConveyorRoller2 :
                                                                        _devices.RollerList.SupportConveyorRoller4;

        private BD201SRollerController Roller1 => port == EPort.Right ? _devices.RollerList.InWorkConveyorRoller1 :
                                                                 _devices.RollerList.OutWorkConveyorRoller1;
        private BD201SRollerController Roller2 => port == EPort.Right ? _devices.RollerList.InWorkConveyorRoller2 :
                                                                 _devices.RollerList.OutWorkConveyorRoller2;
        #endregion

        #region Override Methods
        public override bool ProcessToStop()
        {
            if (ProcessStatus == EProcessStatus.ToStopDone)
            {
                Thread.Sleep(50);
                return true;
            }

            Roller1.Stop();
            Roller1.Stop();
            RollerSupport1.Stop();
            RollerSupport2.Stop();

            return base.ProcessToStop();
        }

        public override bool ProcessOrigin()
        {
            switch ((EWorkConveyorOriginStep)Step.OriginStep)
            {
                case EWorkConveyorOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.SensorStatus_Check:
                    if (IsCassetteDetect == false &&
                        (Detect1 || Detect2 || Detect3) == true)
                    {
                        // Just some of one all sensor detect cassette
                        // cassette may not in safety position
                        RaiseWarning((int)(port == EPort.Right
                            ? EWarning.InWorkConveyor_CSTSensorStatus_Fail
                            : EWarning.OutWorkConveyor_CSTSensorStatus_Fail));
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Wait_RobotLoadOriginDone:
                    if (FlagRobotOriginDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Align_Forward:
                    if (IsCassetteDetect == false)
                    {
                        Step.OriginStep = (int)EWorkConveyorOriginStep.Cyl_Tilt_Down;
                        break;
                    }

                    Log.Debug($"{AlignCylinder1}, {AlignCylinder2} move align");
                    AlignCylinder1.Forward();
                    AlignCylinder2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => AlignCylinder1.IsForward && AlignCylinder2.IsForward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Align_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right
                            ? EWarning.InWorkConveyor_AlignCylinder_Forward_Fail
                            : EWarning.OutWorkConveyor_AlignCylinder_Forward_Fail));
                        break;
                    }

                    Log.Debug($"{AlignCylinder1}, {AlignCylinder2} move align done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Tilt_Down:
                    Log.Debug("Cylinder Tilt Down");
                    TiltCylinder.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TiltCylinder.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Tilt_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right
                            ? EWarning.InWorkConveyor_TiltCylinder_Down_Fail
                            : EWarning.OutWorkConveyor_TiltCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Tilt Down Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.SupportCV_Down:
                    Log.Debug("Support CV Down");
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
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
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => TAxis.Status.IsHomeDone);
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
                case EWorkConveyorOriginStep.Cyl_Align_Backward:
                    Log.Debug("Align Cylinder Backward");
                    AlignCylinder1.Backward();
                    AlignCylinder2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCylinder1.IsBackward && AlignCylinder2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_Align_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_AlignCylinder_Backward_Fail :
                                                           EWarning.OutWorkConveyor_AlignCylinder_Backward_Fail));
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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.InWorkCSTLoad:
                    if (port == EPort.Right) Sequence_Load();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.InWorkCSTUnLoad:
                    if (port == EPort.Right) Sequence_Unload();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.InWorkCSTTilt:
                    if (port == EPort.Right) Sequence_Tilt();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.OutWorkCSTLoad:
                    if (port == EPort.Left) Sequence_Load();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    if (port == EPort.Left) Sequence_Unload();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.OutWorkCSTTilt:
                    if (port == EPort.Left) Sequence_Tilt();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    if (port == EPort.Right) Sequence_PickPlace();
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    if (port == EPort.Left) Sequence_PickPlace();
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
            switch ((EWorkConveyorToRunStep)Step.ToRunStep)
            {
                case EWorkConveyorToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EWorkConveyorToRunStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorStop();
                    Step.ToRunStep++;
                    break;
                case EWorkConveyorToRunStep.Set_ConveyorSpeed:
                    Log.Debug("Conveyor Set Speed");
                    ConveyorSetSpeed((int)_cstLoadUnloadRecipe.ConveyorSpeed);
                    Step.ToRunStep++;
                    break;
                case EWorkConveyorToRunStep.Set_ConveyorAccel:
                    Log.Debug("Conveyor Set Accel");
                    ConveyorSetAccel((int)_cstLoadUnloadRecipe.ConveyorAcc);
                    Step.ToRunStep++;
                    break;
                case EWorkConveyorToRunStep.Set_ConveyorDeccel:
                    Log.Debug("Conveyor Set Deccel");
                    ConveyorSetDeccel((int)_cstLoadUnloadRecipe.ConveyorDec);
                    Step.ToRunStep++;
                    break;
                case EWorkConveyorToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<EWorkConveyorProcessOutput>)Outputs).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case EWorkConveyorToRunStep.End:
                    Log.Debug("To Run End");
                    Step.ToRunStep++;
                    ProcessStatus = EProcessStatus.ToRunDone;
                    break;
                default:
                    Wait(20);
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
                        ConveyorRun(true);
                        Wait(1000);
                        Log.Info("Sequence Tilt");
                        Sequence = port == EPort.Right ? ESequence.InWorkCSTTilt : ESequence.OutWorkCSTTilt;
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

        private void Sequence_Ready()
        {
            switch ((EWorkConveyor_ReadyStep)Step.RunStep)
            {
                case EWorkConveyor_ReadyStep.Start:
                    Log.Debug("Ready Start");
                    Step.RunStep++;
                    break;
                case EWorkConveyor_ReadyStep.SensorStatus_Check:
                    if (IsCassetteDetect == false &&
                        (Detect1 || Detect2 || Detect3) == true)
                    {
                        // Just some of one all sensor detect cassette
                        // cassette may not in safety position
                        RaiseWarning((int)(port == EPort.Right
                            ? EWarning.InWorkConveyor_CSTSensorStatus_Fail
                            : EWarning.OutWorkConveyor_CSTSensorStatus_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyor_ReadyStep.Support_Down:
                    Log.Debug($"{CVSupportCyl1} and {CVSupportCyl2} move down");
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyor_ReadyStep.Support_DownWait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Down_Fail :
                                                           EWarning.OutWorkConveyor_SupportCV_Down_Fail));
                        break;
                    }

                    Log.Debug($"{CVSupportCyl1} and {CVSupportCyl2} move down done");
                    Step.RunStep++;
                    break;
                case EWorkConveyor_ReadyStep.End:
                    Log.Debug("Ready End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_Tilt()
        {
            switch ((EWorkConveyor_TiltStep)Step.RunStep)
            {
                case EWorkConveyor_TiltStep.Start:
                    Log.Debug("Tilt Start");
                    ConveyorStop();
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.Cyl_SupportCV_Down:
                    Log.Debug("Cylinder Support Conveyor Down");
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.Cyl_SupportCV_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Down_Fail :
                                                           EWarning.OutWorkConveyor_SupportCV_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Support Conveyor Down Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.Cyl_Fix_Forward:
                    Log.Debug("Align Cylinder Forward");
                    AlignCylinder1.Forward();
                    AlignCylinder2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCylinder1.IsForward && AlignCylinder2.IsForward);
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.Cyl_Fix_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_AlignCylinder_Forward_Fail :
                                                           EWarning.OutWorkConveyor_AlignCylinder_Forward_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Fix Forward Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.TAxis_Move_WorkPosition:
                    Log.Debug("T Axis Move Work Position");
                    TAxis.MoveAbs(TAxisWorkPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => TAxis.IsOnPosition(TAxisWorkPosition));
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.TAxis_Move_WorkPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Right ? EAlarm.InWorkConveyor_TAxis_MoveWorkPosition_Fail :
                                                         EAlarm.OutWorkConveyor_TAxis_MoveWorkPosition_Fail));
                        break;
                    }
                    Log.Debug("T Axis Move Work Position Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.Cyl_Tilt_Up:
                    Log.Debug("Cylinder Tilt Up");
                    TiltCylinder.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => TiltCylinder.IsForward);
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.Cyl_Tilt_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_TiltCylinder_Up_Fail :
                                                           EWarning.OutWorkConveyor_TiltCylinder_Up_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Tilt Up Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyor_TiltStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
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
                    if (IsCassetteDetect == false && !_machineStatus.IsDryRunMode)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyorCSTNotDetect :
                                                                 EWarning.OutWorkConveyorCSTNotDetect));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyorPickPlaceStep.Cassette_WorkCondition_Check:
                    Log.Debug("Cassette Work Condition Check");
                    if (CassetteWorkDone)
                    {
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

                    // CassetteWorkDone = false -> Atlest 1 ETrayCellStatus.Ready cell exist
                    if (Cassette.GetFirstIndex(ETrayCellStatus.Working) == -1)
                    {
                        Cassette[(uint)Cassette.GetFirstIndex(ETrayCellStatus.Ready)] = ETrayCellStatus.Working;
                    }

                    Step.RunStep++;
                    break;
                case EWorkConveyorPickPlaceStep.Set_FlagReady:
                    Log.Debug("Set Flag Ready");
                    FlagCSTReady = true;
                    Step.RunStep++;
                    Log.Debug("Wait Robot" + (port == EPort.Right ? "Pick" : "Place") + " Done");
                    break;
                case EWorkConveyorPickPlaceStep.Wait_Robot_PickPlace_Done:
                    if (FlagRobotPickPlaceDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Ready");
                    FlagCSTReady = false;

                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Step.RunStep = (int)EWorkConveyorPickPlaceStep.Cassette_WorkCondition_Check;
                    break;
                case EWorkConveyorPickPlaceStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
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
                case EWorkConveyorProcessLoadStep.CylTilt_Supporter_Down:
                    if (TiltCylinder.IsBackward && CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward)
                    {
                        Step.RunStep = (int)EWorkConveyorProcessLoadStep.TAxis_Move_LoadPosition;
                        break;
                    }

                    Log.Debug($"{TiltCylinder}, {CVSupportCyl1}, {CVSupportCyl2} Move Down");
                    TiltCylinder.Backward();
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => TiltCylinder.IsBackward && CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.CylTilt_Supporter_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (TiltCylinder.IsBackward == false)
                        {
                            RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_TiltCylinder_Down_Fail :
                                                           EWarning.OutWorkConveyor_TiltCylinder_Down_Fail));
                            break;
                        }

                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Down_Fail :
                                   EWarning.OutWorkConveyor_SupportCV_Down_Fail));
                        break;
                    }

                    Log.Debug($"{TiltCylinder}, {CVSupportCyl1}, {CVSupportCyl2} Move Down Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.TAxis_Move_LoadPosition:
                    Log.Debug("T Axis Move Load Position");
                    TAxis.MoveAbs(TAxisLoadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => TAxis.IsOnPosition(TAxisLoadPosition));
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
                    Log.Debug("Align Cylinder Backward");
                    AlignCylinder1.Backward();
                    AlignCylinder2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCylinder1.IsBackward && AlignCylinder2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_AlignCylinder_Backward_Fail :
                                                           EWarning.OutWorkConveyor_AlignCylinder_Backward_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Fix Backward Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Support_CV_Up:
                    Log.Debug("Support Conveyor In Up");
                    CVSupportCyl1.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CVSupportCyl1.IsForward);
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
                case EWorkConveyorProcessLoadStep.CheckInputStopValue:
                    //if (_machineStatus.IsInputStop == true)
                    //{
                    //    Wait(20);
                    //    break;
                    //}
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Wait_UpStreamCSTExist:
                    if (IsUpStreamCSTExist == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Conveyor_Run:
                    Log.Debug("Conveyor Run In");
                    ConveyorRun(true);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Set_FlagRequestCassetteIn:
                    Log.Debug("Set Flag Request Cassette In");
                    FlagRequestCSTIn = true;
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Wait_CassetteLoadDone:
                    if (IsCassetteDetect == false && _machineStatus.IsDryRunMode == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Cassette Arrived, keep running for 1 seconds");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Clear_FlagRequestCst:
                    Log.Debug("Clear Flag Request Cassette In");
                    FlagRequestCSTIn = false;
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Reset_Cassette_Status:
                    Log.Debug("Reset Cassette Status");
                    Cassette.Cells.ToList().ForEach(c => c.Status = ETrayCellStatus.Ready);
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorStop();
                    Step.RunStep++;
                    break;
                case EWorkConveyorProcessLoadStep.End:
                    Log.Debug("Load End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence CST Tilt");
                    Sequence = port == EPort.Right ? ESequence.InWorkCSTTilt : ESequence.OutWorkCSTTilt;
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
                case EWorkConveyorUnloadStep.CylTilt_Supporter_Down:
                    if (TiltCylinder.IsBackward && CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward)
                    {
                        Step.RunStep = (int)EWorkConveyorProcessLoadStep.TAxis_Move_LoadPosition;
                        break;
                    }

                    Log.Debug($"{TiltCylinder}, {CVSupportCyl1}, {CVSupportCyl2} Move Down");
                    TiltCylinder.Backward();
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => TiltCylinder.IsBackward && CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.CylTilt_Supporter_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (TiltCylinder.IsBackward == false)
                        {
                            RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_TiltCylinder_Down_Fail :
                                                           EWarning.OutWorkConveyor_TiltCylinder_Down_Fail));
                            break;
                        }

                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_SupportCV_Down_Fail :
                                   EWarning.OutWorkConveyor_SupportCV_Down_Fail));
                        break;
                    }

                    Log.Debug($"{TiltCylinder}, {CVSupportCyl1}, {CVSupportCyl2} Move Down Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.TAxis_MoveLoadPosition:
                    Log.Debug("T Axis Move Unload Position");
                    TAxis.MoveAbs(TAxisUnloadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => TAxis.IsOnPosition(TAxisUnloadPosition));
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.TAxis_MoveLoadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)(port == EPort.Right ? EAlarm.InWorkConveyor_TAxis_MoveUnloadPosition_Fail :
                                                         EAlarm.OutWorkConveyor_TAxis_MoveUnloadPosition_Fail));
                        break;
                    }
                    Log.Debug("T Axis Move Load Position Done");
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Support_CV_Up:
                    Log.Debug("Support Conveyor Up");
                    CVSupportCyl2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CVSupportCyl2.IsForward);
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
                    Log.Debug("Align Cylinder Backward");
                    AlignCylinder1.Backward();
                    AlignCylinder2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => AlignCylinder1.IsBackward && AlignCylinder2.IsBackward);
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Cyl_Fix_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Right ? EWarning.InWorkConveyor_AlignCylinder_Backward_Fail :
                                                           EWarning.OutWorkConveyor_AlignCylinder_Backward_Fail));
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
                    if (FlagDownStreamReady == false)
                    {
                        Wait(20);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Muting_LightCurtain:
                    Log.Debug("Muting Light Curtain");

                    if (port == EPort.Left)
                    {
                        _devices.Outputs.OutCstLightCurtainInterlock.Value = true;
                        Thread.Sleep(300);
                        _devices.Outputs.OutCstLightCurtainMuting.Value = true;
                    }
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Conveyor_Run:
                    ConveyorRun(false);
#if SIMULATION
                    Wait(1000);
                    if (port == EPort.Right)
                    {
                        SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkDetect1, false);
                        SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkDetect2, false);
                        SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkDetect3, false);
                        SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkFixtureDetect, false);

                        SimulationInputSetter.SetSimInput(_devices.Inputs.BufferCstDetect1, true);
                        SimulationInputSetter.SetSimInput(_devices.Inputs.BufferCstDetect2, true);
                    }
                    else
                    {
                        SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstWorkDetect1, false);
                        SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstWorkDetect2, false);
                        SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstWorkDetect3, false);

                        SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstDetect1, true);
                        SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstDetect2, true);
                    }
#endif
                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.Wait_CSTOut_Done:
                    if (FlagDownStreamReady == true || IsCassetteOut == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Request CST Out");
                    FlagRequestCSTOut = false;

                    ConveyorStop();

                    if (port == EPort.Left)
                    {
                        Log.Debug("Enable Light Curtain");
                        _devices.Outputs.OutCstLightCurtainInterlock.Value = false;
                        Wait(50);
                        _devices.Outputs.OutCstLightCurtainMuting.Value = false;
                    }

                    Step.RunStep++;
                    break;
                case EWorkConveyorUnloadStep.End:
                    Log.Debug("Unload End");

                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
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

        private void ConveyorRun(bool bInDirection)
        {
            if (bInDirection)
            {
                //InWorkConveyor
                if (port == EPort.Right)
                {
                    Roller1.SetDirection(true);
                    Roller2.SetDirection(true);
                }
                //OutWorkConveyor
                else
                {
                    Roller1.SetDirection(false);
                    Roller2.SetDirection(false);
                }
                Roller1.Run();
                Roller2.Run();
                RollerSupport1.Run();
            }
            else
            {
                //InWorkConveyor
                if (port == EPort.Right)
                {
                    Roller1.SetDirection(false);
                    Roller2.SetDirection(false);
                }
                //OutWorkConveyor
                else
                {
                    Roller1.SetDirection(true);
                    Roller2.SetDirection(true);
                }
                Roller1.Run();
                Roller2.Run();
                RollerSupport2.Run();
            }
        }

        private void ConveyorStop()
        {
            Roller1.Stop();
            Roller2.Stop();
            RollerSupport1.Stop();
            RollerSupport2.Stop();
        }

        private void ConveyorSetSpeed(int speed)
        {
            Roller1.SetSpeed(speed);
            Roller2.SetSpeed(speed);
            RollerSupport1.SetSpeed(speed);
            RollerSupport2.SetSpeed(speed);
        }

        private void ConveyorSetAccel(int accel)
        {
            Roller1.SetAcceleration(accel);
            Roller2.SetAcceleration(accel);
            RollerSupport1.SetAcceleration(accel);
            RollerSupport2.SetAcceleration(accel);
        }

        private void ConveyorSetDeccel(int deccel)
        {
            Roller1.SetDeceleration(deccel);
            Roller2.SetDeceleration(deccel);
            RollerSupport1.SetDeceleration(deccel);
            RollerSupport2.SetDeceleration(deccel);
        }
        #endregion
    }
}
