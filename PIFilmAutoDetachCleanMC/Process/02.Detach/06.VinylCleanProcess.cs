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
    public class VinylCleanProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly IMotion _vinylCleanEncoder;
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly VinylCleanRecipe _vinylCleanRecipe;
        private readonly IDInputDevice _vinylCleanInput;
        private readonly IDOutputDevice _vinylCleanOutput;
        private readonly MachineStatus _machineStatus;

        private bool IsFixtureDetect
        {
            get
            {
                if (_machineStatus.IsDryRunMode) return true;
                return _devices.Inputs.VinylCleanFixtureDetect.Value;
            }
        }

        private bool IsFixtureClamp => (!FixtureClampCyl1.IsBackward && !FixtureClampCyl1.IsForward && !FixtureClampCyl2.IsBackward && !FixtureClampCyl2.IsForward) &&
                                        (_devices.Outputs.VinylCleanFixtureClamp.Value);
        private ICylinder FixtureClampCyl1 => _devices.Cylinders.VinylClean_ClampCyl1;
        private ICylinder FixtureClampCyl2 => _devices.Cylinders.VinylClean_ClampCyl2;

        private ICylinder RollerFwBwCyl => _devices.Cylinders.VinylClean_BwFwCyl;
        private ICylinder PusherCyl => _devices.Cylinders.VinylClean_UpDownCyl;

        private IDOutput MotorOnOff => _devices.Outputs.VinylCleanMotorOnOff;
        private bool IsUnWinderFullDetect => _devices.Inputs.VinylCleanFullNotDetect.Value == false;
        private bool IsWinderRunOffDetect => _devices.Inputs.VinylCleanRunoffDetect.Value;
        #endregion

        #region Flags
        private bool FlagVinylCleanRequestUnload
        {
            set
            {
                _vinylCleanOutput[(int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_UNLOAD] = value;
            }
        }

        private bool FlagVinylCleanUnloadDone
        {
            get
            {
                return _vinylCleanInput[(int)EVinylCleanProcessInput.VINYL_CLEAN_UNLOAD_DONE];
            }
        }

        private bool FlagIn_RobotMoveVinylCleanDone
        {
            get
            {
                return _vinylCleanInput[(int)EVinylCleanProcessInput.ROBOT_MOVE_VINYL_CLEAN_DONE];
            }
        }

        private bool FlagVinylCleanRequestLoad
        {
            set
            {
                _vinylCleanOutput[(int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_LOAD] = value;
            }
        }

        private bool FlagVinylCleanLoadDone
        {
            get
            {
                return _vinylCleanInput[(int)EVinylCleanProcessInput.VINYL_CLEAN_LOAD_DONE];
            }
        }

        private bool FlagOut_VinylCleanClampUnClampDone
        {
            set
            {
                _vinylCleanOutput[(int)EVinylCleanProcessOutput.VINYL_CLEAN_CLAMP_UNCLAMP_DONE] = value;
            }
        }
        #endregion

        #region Constructor
        public VinylCleanProcess([FromKeyedServices("VinylCleanEncoder")] IMotion vinylCleanEncoder,
            Devices devices,
            CommonRecipe commonRecipe,
            VinylCleanRecipe vinylCleanRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("VinylCleanInput")] IDInputDevice vinylCleanInput,
            [FromKeyedServices("VinylCleanOutput")] IDOutputDevice vinylCleanOutput)
        {
            _vinylCleanEncoder = vinylCleanEncoder;
            _devices = devices;
            _commonRecipe = commonRecipe;
            _vinylCleanRecipe = vinylCleanRecipe;
            _machineStatus = machineStatus;
            _vinylCleanInput = vinylCleanInput;
            _vinylCleanOutput = vinylCleanOutput;
        }
        #endregion

        #region Override Methods
        public override bool PreProcess()
        {
            if (ProcessMode != EProcessMode.Run) return base.PreProcess();

            if (_machineStatus.IsDryRunMode)
            {
                return base.PreProcess();
            }

            if (IsUnWinderFullDetect & _machineStatus.MachineTestMode == false)
            {
                RaiseWarning((int)EWarning.VinylClean_Full_Detect);
            }

            if (IsWinderRunOffDetect & _machineStatus.MachineTestMode == false)
            {
                RaiseWarning((int)EWarning.VinylClean_Vinyl_NotDetect);
            }
            return base.PreProcess();
        }

        public override bool ProcessToStop()
        {
            MotorOnOff.Value = false;
            return base.ProcessToStop();
        }

        public override bool ProcessToAlarm()
        {
            MotorOnOff.Value = false;
            return base.ProcessToAlarm();
        }

        public override bool ProcessToWarning()
        {
            MotorOnOff.Value = false;
            return base.ProcessToWarning();
        }
        public override bool ProcessOrigin()
        {
            switch ((EVinylCleanOriginStep)Step.OriginStep)
            {
                case EVinylCleanOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Down:
                    Log.Debug("Vinyl Clean Cylinder Roller Down");
                    PusherCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_PusherCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Roller Down Done");
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Backward:
                    Log.Debug("Vinyl Clean Cylinder Roller Backward");
                    RollerFwBwCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RollerFwBwCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_RollerBwFwCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Roller Backward Done");
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
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
                case ESequence.RobotPlaceFixtureToVinylClean:
                    Sequence_RobotPlaceFixtureToVinylClean();
                    break;
                case ESequence.VinylClean:
                    Sequence_VinylClean();
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    Sequence_RobotPickFixtureFromVinylClean();
                    break;
                default:
                    Sequence = ESequence.Stop;
                    break;
            }
            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((EVinylCleanProcessToRunStep)Step.ToRunStep)
            {
                case EVinylCleanProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EVinylCleanProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<EVinylCleanProcessOutput>)_vinylCleanOutput).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case EVinylCleanProcessToRunStep.End:
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
        private void Sequence_AutoRun()
        {
            switch ((EVinylCleanProcess_AutoRunStep)Step.RunStep)
            {
                case EVinylCleanProcess_AutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcess_AutoRunStep.FixtureDetect_Check:
                    if (IsFixtureDetect)
                    {
                        Log.Info("Sequence Vinyl Clean");
                        Sequence = ESequence.VinylClean;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EVinylCleanProcess_AutoRunStep.End:
                    Log.Info("Sequence Robot Place Fixture To Vinyl Clean");
                    Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                    break;
            }
        }

        private void Sequence_Ready()
        {
            switch ((EVinylCleanProcess_ReadyStep)Step.RunStep)
            {
                case EVinylCleanProcess_ReadyStep.Start:
                    Log.Debug("Ready Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcess_ReadyStep.PusherCylDown:
                    if (PusherCyl.IsBackward)
                    {
                        Step.RunStep = (int)EVinylCleanProcess_ReadyStep.RollerCylBackward;
                        break;
                    }

                    Log.Debug($"{PusherCyl} move down");
                    PusherCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcess_ReadyStep.PusherCylDown_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_PusherCylinder_Down_Fail);
                        break;
                    }

                    Log.Debug("Vinyl Clean Cylinder Roller Down Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcess_ReadyStep.RollerCylBackward:
                    if (RollerFwBwCyl.IsBackward)
                    {
                        Step.RunStep = (int)EVinylCleanProcess_ReadyStep.End;
                        break;
                    }

                    Log.Debug("Vinyl Clean Cylinder Roller Backward");
                    RollerFwBwCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RollerFwBwCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcess_ReadyStep.RollerCylBackward_Check:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_RollerBwFwCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Roller Backward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcess_ReadyStep.End:
                    Log.Debug("Ready End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Info("Sequence Robot Place Fixture To Vinyl Clean");
                    Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                    break;
            }
        }

        private void Sequence_VinylClean()
        {
            switch ((EVinylCleanProcessVinylCleanStep)Step.RunStep)
            {
                case EVinylCleanProcessVinylCleanStep.Start:
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.VinylCleanFixtureDetect, true);
#endif
                    Log.Debug("Vinyl Clean Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.FixtureDetect_Check:
                    if (IsFixtureDetect == false)
                    {
                        RaiseWarning((int)EWarning.VinylClean_Fixture_NotDetect);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.VinylCleanFixtureDetect, false);
#endif
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Clamp:
                    Log.Debug("Vinyl Clean Cylinder Clamp");
                    FixtureClampCyl1.Forward();
                    FixtureClampCyl2.Forward();
                    Wait(200);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Clamp_Delay:
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => IsFixtureClamp || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_ClampCylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Up:
                    Log.Debug("Vinyl Clean Cylinder Pusher Up");
                    PusherCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_PusherCylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Pusher Up Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Forward:
                    Log.Debug("Vinyl Clean Cylinder Forward");
                    RollerFwBwCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RollerFwBwCyl.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_RollerBwFwCylinder_Forward_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Forward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Backward:
                    Log.Debug("Vinyl Clean Cylinder Backward");
                    RollerFwBwCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RollerFwBwCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_RollerBwFwCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Down:
                    Log.Debug("Vinyl Clean Cylinder Pusher Down");
                    PusherCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_PusherCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.End:
                    Log.Debug("Vinyl Clean End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                    Sequence = ESequence.RobotPickFixtureFromVinylClean;
                    break;
            }
        }

        private void Sequence_RobotPickFixtureFromVinylClean()
        {
            switch ((EVinylCleanProcessRobotPickFixtureFromVinylClean)Step.RunStep)
            {
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Start:
                    Log.Debug("Robot Pick Fixture From Vinyl Clean Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.SetFlag_VinylCleanRequestUnload:
                    Log.Debug("Set Flag Vinyl Clean Request Unload");
                    FlagVinylCleanRequestUnload = true;
                    Step.RunStep++;
                    Log.Debug("Wait Robot Move Vinyl Clean Done");
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Wait_RobotMoveVinylCleanDone:
                    if(FlagIn_RobotMoveVinylCleanDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Robot Move Vinyl Clean Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Cyl_UnClamp:
                    Log.Debug("Vinyl Clean Cylinder UnClamp");
                    FixtureClampCyl1.Backward();
                    FixtureClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => FixtureClampCyl1.IsBackward && FixtureClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder UnClamp Done");
                    Log.Debug("Wait Vinyl Clean Unload Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.SetFlag_VinylCleanUnClampDone:
                    Log.Debug("Set Flag Vinyl Clean UnClamp Done");
                    FlagOut_VinylCleanClampUnClampDone = true;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Wait_VinylCleanUnloadDone:
                    if (FlagVinylCleanUnloadDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear Flag Vinyl Clean Unclamp Done");
                    FlagOut_VinylCleanClampUnClampDone = false;
                    Log.Debug("Clear Flag Vinyl Clean Request Unload");
                    FlagVinylCleanRequestUnload = false;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Robot Place Fixture To Vinyl Clean");
                    Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                    break;
            }
        }

        private void Sequence_RobotPlaceFixtureToVinylClean()
        {
            switch ((EVinylCleanProcessRobotPlaceFixtureToVinylClean)Step.RunStep)
            {
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Start:
                    Log.Debug("Robot Place Fixture To Vinyl Clean Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_UnClamp:
                    Log.Debug("Vinyl Clean Cylinder UnClamp");
                    FixtureClampCyl1.Backward();
                    FixtureClampCyl2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => FixtureClampCyl1.IsBackward && FixtureClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Pusher_Down:
                    Log.Debug("Vinyl Clean Cylinder Pusher Down");
                    PusherCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PusherCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Pusher_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_PusherCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Backward:
                    Log.Debug("Vinyl Clean Cylinder Backward");
                    RollerFwBwCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => RollerFwBwCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_RollerBwFwCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Encoder_Clear_Position:
                    Log.Debug("Vinyl Clean Encoder Clear Position");
                    _vinylCleanEncoder.ClearPosition();
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Motor_UnWinder_Run:
                    Log.Debug("Motor UnWinder Run");
#if SIMULATION
                    _vinylCleanEncoder.MoveInc(_vinylCleanRecipe.VinylLengthPerCleaning);
#endif
                    MotorOnOff.Value = true;
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => Math.Abs(_vinylCleanEncoder.Status.ActualPosition) >= _vinylCleanRecipe.VinylLengthPerCleaning);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Motor_UnWinder_Run_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.VinylClean_Motor_UnWinder_Run_Fail);
                        break;
                    }

                    Log.Debug("Motor UnWinder Stop");
                    MotorOnOff.Value = false;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Set_Flag_RequestFixture:
                    Log.Debug("Set Flag Vinyl Clean Request Load");
                    FlagVinylCleanRequestLoad = true;
                    Step.RunStep++;
                    Log.Debug("Wait Fixture Load Done");
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Wait_RobotMoveVinylCleanDone:
                    if(FlagIn_RobotMoveVinylCleanDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Robot Move Vinyl Clean Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Clamp:
                    Log.Debug("Vinyl Clean Cylinder Clamp");
                    FixtureClampCyl1.Forward();
                    FixtureClampCyl2.Forward();
                    Wait(200);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Clamp_Delay:
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,
                        () => IsFixtureClamp || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_ClampCylinder_Clamp_Fail);
                        break;
                    }

                    Log.Debug("Vinyl Clean Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.SetFlag_VinylCleanClampDone:
                    Log.Debug("Set Flag Vinyl Clean Clamp Done");
                    FlagOut_VinylCleanClampUnClampDone = true;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Wait_FixtureLoadDone:
                    if (FlagVinylCleanLoadDone == false)
                    {
                        Wait(20);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.VinylCleanFixtureDetect, true);
#endif
                    Log.Debug("Clear Flag Vinyl Clean Clamp Done");
                    FlagOut_VinylCleanClampUnClampDone = false;
                    Log.Debug("Clear Flag Vinyl Clean Request Load");
                    FlagVinylCleanRequestLoad = false;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Fixture_Detect_Check:
                    if (IsFixtureDetect == false && !_machineStatus.IsDryRunMode)
                    {
                        RaiseWarning((int)EWarning.VinylClean_Fixture_NotDetect);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.VinylCleanFixtureDetect, false);
#endif
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Vinyl Clean");
                    Sequence = ESequence.VinylClean;
                    break;
            }
        }
        #endregion
    }
}
