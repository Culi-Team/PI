using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.VirtualIO;
using PIFilmAutoDetachCleanMC.Recipe;
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
        private readonly VirtualIO<EFlags> _virtualIO;

        private bool IsFixtureDetect => _devices.Inputs.VinylCleanFixtureDetect.Value;
        private ICylinder FixtureClampCyl1 => _devices.Cylinders.VinylCleanFixture1ClampUnclamp;
        private ICylinder FixtureClampCyl2 => _devices.Cylinders.VinylCleanFixture2ClampUnclamp;

        private ICylinder RollerBwFwCyl => _devices.Cylinders.VinylCleanRollerBwFw;
        private ICylinder PusherCyl => _devices.Cylinders.VinylCleanPusherRollerUpDown;

        private IDOutput MotorOnOff => _devices.Outputs.VinylCleanMotorOnOff;
        private bool IsUnWinderFullDetect => _devices.Inputs.VinylCleanFullDetect.Value;
        private bool IsWinderRunOffDetect => _devices.Inputs.VinylCleanRunoffDetect.Value;
        #endregion

        #region Flags
        private bool FlagVinylCleanRequestUnload
        {
            set
            {
                _virtualIO.SetFlag(EFlags.VinylCleanRequestUnload, value);
            }
        }

        private bool FlagVinylCleanUnloadDone
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.VinylCleanUnloadDone);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.VinylCleanUnloadDone, value);
            }
        }

        private bool FlagVinylCleanRequestFixture
        {
            set
            {
                _virtualIO.SetFlag(EFlags.VinylCleanRequestFixture, value);
            }
        }

        private bool FlagVinylCleanLoadDone
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.VinylCleanLoadDone);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.VinylCleanLoadDone, value);
            }
        }
        #endregion

        #region Constructor
        public VinylCleanProcess([FromKeyedServices("VinylCleanEncoder")] IMotion vinylCleanEncoder,
            Devices devices,
            CommonRecipe commonRecipe,
            VinylCleanRecipe vinylCleanRecipe,
            VirtualIO<EFlags> virtualIO)
        {
            _vinylCleanEncoder = vinylCleanEncoder;
            _devices = devices;
            _commonRecipe = commonRecipe;
            _vinylCleanRecipe = vinylCleanRecipe;
            _virtualIO = virtualIO;
        }
        #endregion

        #region Override Methods
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
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PusherCyl.IsBackward);
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
                    RollerBwFwCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => RollerBwFwCyl.IsBackward);
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
                case ESequence.AlignGlass:
                    break;
                case ESequence.TransferInShuttlePick:
                    break;
                case ESequence.TransferInShuttlePlace:
                    break;
                case ESequence.WETCleanLoad:
                    break;
                case ESequence.WETClean:
                    break;
                case ESequence.WETCleanUnload:
                    break;
                case ESequence.TransferRotationPick:
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
                    Log.Debug("Vinyl Clean Start");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.FixtureDetect_Check:
                    if(IsFixtureDetect == false)
                    {
                        RaiseWarning((int)EWarning.VinylCleanFixtureNotDetect);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Clamp:
                    Log.Debug("Vinyl Clean Cylinder Clamp");
                    FixtureClampCyl1.Forward();
                    FixtureClampCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => FixtureClampCyl1.IsForward && FixtureClampCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Clamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Up:
                    Log.Debug("Vinyl Clean Cylinder Pusher Up");
                    PusherCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => PusherCyl.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Up_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Pusher Up Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Forward:
                    Log.Debug("Vinyl Clean Cylinder Forward");
                    RollerBwFwCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => RollerBwFwCyl.IsForward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Forward_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Forward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Down:
                    Log.Debug("Vinyl Clean Cylinder Pusher Down");
                    PusherCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => PusherCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Pusher_Down_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Backward:
                    Log.Debug("Vinyl Clean Cylinder Backward");
                    RollerBwFwCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => RollerBwFwCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.Cyl_Backward_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Backward Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessVinylCleanStep.End:
                    Log.Debug("Vinyl Clean End");
                    if (Parent!.Sequence != ESequence.AutoRun)
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
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Cyl_UnClamp:
                    Log.Debug("Vinyl Clean Cylinder UnClamp");
                    FixtureClampCyl1.Backward();
                    FixtureClampCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => FixtureClampCyl1.IsBackward && FixtureClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Cyl_UnClamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.SetFlag_VinylCleanRequestUnload:
                    Log.Debug("Set Flag Vinyl Clean Request Unload");
                    Step.RunStep++;
                    Log.Debug("Wait Vinyl Clean Unload Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.Wait_VinylCleanUnloadDone:
                    if(FlagVinylCleanUnloadDone == false)
                    {
                        break;
                    }
                    FlagVinylCleanUnloadDone = false;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPickFixtureFromVinylClean.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
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
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_UnClamp:
                    Log.Debug("Vinyl Clean Cylinder UnClamp");
                    FixtureClampCyl1.Backward();
                    FixtureClampCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => FixtureClampCyl1.IsBackward && FixtureClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_UnClamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Pusher_Down:
                    Log.Debug("Vinyl Clean Cylinder Pusher Down");
                    PusherCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => PusherCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Pusher_Down_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Backward:
                    Log.Debug("Vinyl Clean Cylinder Backward");
                    RollerBwFwCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout,() => RollerBwFwCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Cyl_Backward_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
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
                    Wait(_commonRecipe.MotionMoveTimeOut, () => _vinylCleanEncoder.IsOnPosition(_vinylCleanRecipe.VinylLengthPerCleaning));
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
                    Log.Debug("Vinyl Clean Request Fixture");
                    FlagVinylCleanRequestFixture = true;
                    Step.RunStep++;
                    Log.Debug("Wait Fixture Load Done");
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.Wait_FixtureLoadDone:
                    if(FlagVinylCleanLoadDone == false)
                    {
                        break;
                    }
                    FlagVinylCleanLoadDone = false;
                    Step.RunStep++;
                    break;
                case EVinylCleanProcessRobotPlaceFixtureToVinylClean.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
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
