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

        private bool IsFixtureDetect => _machineStatus.IsSatisfied(_devices.Inputs.VinylCleanFixtureDetect);
        private ICylinder FixtureClampCyl => _devices.Cylinders.VinylCleanFixtureClampUnclamp;

        private ICylinder RollerFwBwCyl => _devices.Cylinders.VinylCleanRollerFwBw;
        private ICylinder PusherCyl => _devices.Cylinders.VinylCleanPusherRollerUpDown;

        private IDOutput MotorOnOff => _devices.Outputs.VinylCleanMotorOnOff;
        private bool IsUnWinderFullDetect => _machineStatus.IsSatisfied(_devices.Inputs.VinylCleanFullDetect);
        private bool IsWinderRunOffDetect => _machineStatus.IsSatisfied(_devices.Inputs.VinylCleanRunoffDetect);
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

        private bool FlagVinylCleanReceiveUnloadDone
        {
            set
            {
                _vinylCleanOutput[(int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE] = value;
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

        private bool FlagVinylCleanReceiveLoadDone
        {
            set
            {
                _vinylCleanOutput[(int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_LOAD_DONE] = value;
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
            if(ProcessMode != EProcessMode.Run) return base.PreProcess();

            if(IsUnWinderFullDetect)
            {
                RaiseWarning((int)EWarning.Vinylclean_Full_Detect);
            }

            if(IsWinderRunOffDetect)
            {
                RaiseWarning((int)EWarning.Vinylclean_Vinyl_Not_Detect);
            }
            return base.PreProcess();
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
                        //Timeout ALARM
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
                        //Timeout ALARM
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
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence = ESequence.Stop;
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
                    Sequence_RobotPlaceFixtureToVinylClean();
                    break;
                case ESequence.VinylClean:
                    Sequence_VinylClean();
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    Sequence_RobotPickFixtureFromVinylClean();
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
                case ESequence.TransferInShuttleLeftPick:
                    break;
                case ESequence.WETCleanLeftLoad:
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanLeftUnload:
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.AFCleanLeftLoad:
                    break;
                case ESequence.AFCleanLeft:
                    break;
                case ESequence.AFCleanLeftUnload:
                    break;
                case ESequence.UnloadTransferLeftPlace:
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
            switch ((EVinylCleanProcessToRunStep)Step.ToRunStep)
            {
                case EVinylCleanProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EVinylCleanProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<EVinylCleanProcessOutput>)_vinylCleanOutput).Clear();
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
            switch ((EVinylCleanProcessAutoRunStep)Step.RunStep)
            {
                case EVinylCleanProcessAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessAutoRunStep.FixtureDetect_Check:
                    if(IsFixtureDetect)
                    {
                        Log.Info("Sequence Vinyl Clean");
                        Sequence = ESequence.VinylClean;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessAutoRunStep.End:
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
                    SimulationInputSetter.SetSimInput(_devices.Inputs.VinylCleanFixtureDetect,true);
#endif
                    Log.Debug("Vinyl Clean Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.FixtureDetect_Check:
                    if(IsFixtureDetect == false)
                    {
                        RaiseWarning((int)EWarning.VinylCleanFixtureNotDetect);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.VinylCleanFixtureDetect, false);
#endif
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Clamp:
                    Log.Debug("Vinyl Clean Cylinder Clamp");
                    FixtureClampCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => FixtureClampCyl.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Clamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_FixtureCylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Up:
                    Log.Debug("Vinyl Clean Cylinder Pusher Up");
                    PusherCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => PusherCyl.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Up_Wait:
                    if(WaitTimeOutOccurred)
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
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => RollerFwBwCyl.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Forward_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_RollerBwFwCylinder_Forward_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Forward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Down:
                    Log.Debug("Vinyl Clean Cylinder Pusher Down");
                    PusherCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => PusherCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Down_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_PusherCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Backward:
                    Log.Debug("Vinyl Clean Cylinder Backward");
                    RollerFwBwCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => RollerFwBwCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Backward_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_RollerBwFwCylinder_Backward_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.End:
                    Log.Debug("Vinyl Clean End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
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
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Clear_Flag:
                    Log.Debug("Clear Flag Vinyl Clean Receive Unload Done");
                    FlagVinylCleanReceiveUnloadDone = false;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Cyl_UnClamp:
                    Log.Debug("Vinyl Clean Cylinder UnClamp");
                    FixtureClampCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => FixtureClampCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Cyl_UnClamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_FixtureCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.SetFlag_VinylCleanRequestUnload:
                    Log.Debug("Set Flag Vinyl Clean Request Unload");
                    FlagVinylCleanRequestUnload = true;
                    Step.RunStep++;
                    Log.Debug("Wait Vinyl Clean Unload Done");
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Wait_VinylCleanUnloadDone:
                    if(FlagVinylCleanUnloadDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Vinyl Clean Request Unload");
                    FlagVinylCleanRequestUnload = false;

                    Log.Debug("Set Flag Vinyl Clean Receive Unload Done");
                    FlagVinylCleanReceiveUnloadDone = true;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
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
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Clear_Flag:
                    Log.Debug("Clear Flag Vinyl Clean Receive Load Done");
                    FlagVinylCleanReceiveLoadDone = false;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_UnClamp:
                    Log.Debug("Vinyl Clean Cylinder UnClamp");
                    FixtureClampCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => FixtureClampCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_UnClamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.VinylClean_FixtureCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Pusher_Down:
                    Log.Debug("Vinyl Clean Cylinder Pusher Down");
                    PusherCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => PusherCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Pusher_Down_Wait:
                    if(WaitTimeOutOccurred)
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
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000,() => RollerFwBwCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Backward_Wait:
                    if(WaitTimeOutOccurred)
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
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _vinylCleanEncoder.IsOnPosition(_vinylCleanRecipe.VinylLengthPerCleaning));
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Motor_UnWinder_Run_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Motor UnWinder Run Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Motor_UnWinder_Stop:
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
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Wait_FixtureLoadDone:
                    if(FlagVinylCleanLoadDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Vinyl Clean Request Load");
                    FlagVinylCleanRequestLoad = false;

                    Log.Debug("Set Flag Vinyl Clean Receive Load Done");
                    FlagVinylCleanReceiveLoadDone = true;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
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
