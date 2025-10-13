using EQX.Core.Common;
using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Sequence;
using EQX.Device.SpeedController;
using EQX.InOut;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class InConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _inConveyorInput;
        private readonly ActionAssignableTimer _blinkTimer;
        private readonly MachineStatus _machineStatus;

        private const string InLightCurtainnMutingActionKey = "InLightCurtainMutingAction";
        #endregion

        #region Constructor
        public InConveyorProcess(Devices devices,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("InConveyorInput")] IDInputDevice inConveyorInput,
            [FromKeyedServices("BlinkTimer")] ActionAssignableTimer blinkTimer)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _inConveyorInput = inConveyorInput;
            _blinkTimer = blinkTimer;
        }
        #endregion

        #region Inputs
        private bool CST_Det1 => _devices.Inputs.InCstDetect1.Value;
        private bool CST_Det2 => _devices.Inputs.InCstDetect2.Value;
        private IDInput InCompleteButton => _devices.Inputs.InCompleteButton;
        private IDInput InMutingButton => _devices.Inputs.InMutingButton;
        #endregion

        #region Outputs
        private IDOutput InCompleteButtonLamp => _devices.Outputs.InCompleteButtonLamp;
        private IDOutput InMutingButtonLamp => _devices.Outputs.InMutingButtonLamp;
        private IDOutput InCstMutingLightCurtain => _devices.Outputs.InCstLightCurtainMuting;
        private IDOutput InCstInterlockLightCurtain => _devices.Outputs.InCstLightCurtainInterlock;
        #endregion

        #region Flags
        private bool FlagInWorkCSTRequestCSTIn
        {
            get
            {
                return _inConveyorInput[(int)EInConveyorProcessInput.REQUEST_CST_IN];
            }
        }
        #endregion

        #region Cylinders
        private ICylinder StopperCylinder => _devices.Cylinders.InCstStopperUpDown;
        #endregion

        #region Rollers
        private SD201SSpeedController Roller1 => _devices.SpeedControllerList.InConveyorRoller1;
        private SD201SSpeedController Roller2 => _devices.SpeedControllerList.InConveyorRoller2;
        private SD201SSpeedController Roller3 => _devices.SpeedControllerList.InConveyorRoller3;
        #endregion

        #region Override Method
        public override bool ProcessOrigin()
        {
            switch ((EConveyorOriginStep)Step.OriginStep)
            {
                case EConveyorOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.CstStopper_Up:
                    Log.Debug("Cassette Stopper Up");
                    StopperCylinder.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => StopperCylinder.IsForward);
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.CstStopper_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
                default:
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
                    IsWarning = false;
                    Sequence = ESequence.Stop;
                    break;
                case ESequence.InConveyorLoad:
                    Sequence_InConveyorLoad();
                    break;
                case ESequence.InWorkCSTLoad:
                    Sequence_InWorkCSTLoad();
                    break;
                case ESequence.InWorkCSTUnLoad:
                case ESequence.CSTTilt:
                case ESequence.OutWorkCSTLoad:
                case ESequence.OutWorkCSTUnLoad:
                case ESequence.OutConveyorUnload:
                case ESequence.RobotPickFixtureFromCST:
                case ESequence.RobotPlaceFixtureToVinylClean:
                case ESequence.VinylClean:
                case ESequence.RobotPickFixtureFromVinylClean:
                case ESequence.RobotPlaceFixtureToAlign:
                case ESequence.FixtureAlign:
                case ESequence.RobotPickFixtureFromRemoveZone:
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                case ESequence.TransferFixtureLoad:
                case ESequence.Detach:
                case ESequence.TransferFixtureUnload:
                case ESequence.DetachUnload:
                case ESequence.RemoveFilm:
                case ESequence.GlassTransferPick:
                case ESequence.GlassTransferPlace:
                case ESequence.AlignGlassLeft:
                case ESequence.AlignGlassRight:
                case ESequence.TransferInShuttleLeftPick:
                case ESequence.TransferInShuttleRightPick:
                case ESequence.WETCleanLeftLoad:
                case ESequence.WETCleanRightLoad:
                case ESequence.WETCleanLeft:
                case ESequence.WETCleanRight:
                case ESequence.WETCleanLeftUnload:
                case ESequence.WETCleanRightUnload:
                case ESequence.TransferRotationLeft:
                case ESequence.TransferRotationRight:
                case ESequence.AFCleanLeftLoad:
                case ESequence.AFCleanRightLoad:
                case ESequence.AFCleanLeft:
                case ESequence.AFCleanRight:
                case ESequence.AFCleanLeftUnload:
                case ESequence.AFCleanRightUnload:
                case ESequence.UnloadTransferLeftPlace:
                case ESequence.UnloadTransferRightPlace:
                case ESequence.UnloadAlignGlass:
                case ESequence.UnloadRobotPick:
                case ESequence.UnloadRobotPlasma:
                case ESequence.UnloadRobotPlace:
                default:
                    break;
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((EInConveyorToRunStep)Step.ToRunStep)
            {
                case EInConveyorToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EInConveyorToRunStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.ToRunStep++;
                    break;
                case EInConveyorToRunStep.Set_ConveyorSpeed:
                    Log.Debug("Set Conveyor Speed");
                    SetConveyorSpeed((int)_cstLoadUnloadRecipe.ConveyorSpeed);
                    Step.ToRunStep++;
                    break;
                case EInConveyorToRunStep.Set_ConveyorAccel:
                    Log.Debug("Set Conveyor Accel");
                    SetConveyorAccel((int)_cstLoadUnloadRecipe.ConveyorAcc);
                    Step.ToRunStep++;
                    break;
                case EInConveyorToRunStep.Set_ConveyorDeccel:
                    Log.Debug("Set Conveyor Deccel");
                    SetConveyorDeccel((int)_cstLoadUnloadRecipe.ConveyorDec);
                    Step.ToRunStep++;
                    break;
                case EInConveyorToRunStep.End:
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
            switch ((EInConveyorAutoRunStep)Step.RunStep)
            {
                case EInConveyorAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EInConveyorAutoRunStep.CSTDetect_Check:
                    if (CST_Det2 && StopperCylinder.IsForward)
                    {
                        Log.Info("Sequence In Work CST Load");
                        Sequence = ESequence.InWorkCSTLoad;
                        break;
                    }
                    if (CST_Det2 && StopperCylinder.IsBackward)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Position_Error);
                        break;
                    }
                    if (_machineStatus.IsDryRunMode)
                    {
                        Log.Info("Dry Run Mode Skip In Conveyor Auto Run");
                        Step.RunStep = (int)EInConveyorAutoRunStep.End;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EInConveyorAutoRunStep.End:
                    Log.Info("Sequence In Conveyor Load");
                    Sequence = ESequence.InConveyorLoad;
                    break;
            }
        }

        private void Sequence_InWorkCSTLoad()
        {
            switch ((EInWorkConveyorProcessInWorkCSTLoadStep)Step.RunStep)
            {
                case EInWorkConveyorProcessInWorkCSTLoadStep.Start:
                    Log.Debug("In Work CST Load Start");
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Wait_InWorkCSTRequestLoad:
                    if (FlagInWorkCSTRequestCSTIn == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Stopper_Down:
                    Log.Debug("Stopper Down");
                    StopperCylinder.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => StopperCylinder.IsBackward);
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Stopper_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Stopper_Down_Fail);
                        break;
                    }
                    Log.Debug("Stopper Down Done");
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Muting_LightCurtain:
                    Log.Debug("Disable Light Curtain");

                    _blinkTimer.EnableAction(InLightCurtainnMutingActionKey,
                        () => InMutingButtonLamp.Value = true,
                        () => InMutingButtonLamp.Value = false);

                    MutingLightCurtain(true);
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Conveyor_Run:
                    Log.Debug("Conveyor Run");
                    ConveyorRunStop(true);
#if SIMULATION
                    Wait(1000);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.InCstDetect1, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.InCstDetect2, false);

                    SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkDetect1, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkDetect2, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkDetect3, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkDetect4, true);
#endif
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Wait_InWorkCSTLoadDone:
                    if (FlagInWorkCSTRequestCSTIn == true)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Enable_LightCurtain:
                    Log.Debug("Enable Light Curtain");

                    _blinkTimer.DisableAction(InLightCurtainnMutingActionKey);

                    MutingLightCurtain(false);
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Stopper_Up:
                    Log.Debug("Stopper Up");
                    StopperCylinder.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => StopperCylinder.IsForward);
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Stopper_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Up Done");
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.End:
                    Log.Debug("In Work CST Load End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence In Conveyor Load");
                    Sequence = ESequence.InConveyorLoad;
                    break;
            }
        }

        private void Sequence_InConveyorLoad()
        {
            switch ((EInConveyorLoadStep)Step.RunStep)
            {
                case EInConveyorLoadStep.Start:
                    Log.Debug("In Conveyor Load Start");
                    Step.RunStep++;
                    break;
                case EInConveyorLoadStep.Stopper_Up:
                    Log.Debug("Stopper Up");
                    StopperCylinder.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => StopperCylinder.IsForward);
                    Step.RunStep++;
                    break;
                case EInConveyorLoadStep.Stopper_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Up Done");
                    Step.RunStep++;
                    break;
                case EInConveyorLoadStep.CSTDetect_Check:
                    if(_machineStatus.IsDryRunMode)
                    {
                        Step.RunStep = (int)EInConveyorLoadStep.Conveyor_Stop;
                        break;
                    }
                    if (CST_Det2)
                    {
                        Step.RunStep = (int)EInConveyorLoadStep.Conveyor_Stop;
                        break;
                    }
                    if (CST_Det1)
                    {
                        Step.RunStep++;
                        break;
                    }
                    break;
                case EInConveyorLoadStep.Conveyor_Run:
                    ConveyorRunStop(true);
                    Step.RunStep = (int)EInConveyorLoadStep.CSTDetect_Check;
                    break;
                case EInConveyorLoadStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.RunStep++;
                    break;
                case EInConveyorLoadStep.End:
                    Log.Debug("In Conveyor Load End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence In Work CST Load");
                    Sequence = ESequence.InWorkCSTLoad;
                    break;
            }
        }

        private void MutingLightCurtain(bool bOnOff)
        {
            InCstInterlockLightCurtain.Value = bOnOff;
            Wait(50);
            InCstMutingLightCurtain.Value = bOnOff;
        }

        private void ConveyorRunStop(bool bRun)
        {
            if (bRun)
            {
                Roller1.Run();
                Roller2.Run();
                Roller3.Run();
            }
            else
            {
                Roller1.Stop();
                Roller2.Stop();
                Roller3.Stop();
            }
        }

        private void SetConveyorSpeed(int speed)
        {
            Roller1.SetSpeed(speed);
            Roller2.SetSpeed(speed);
            Roller3.SetSpeed(speed);
        }
        private void SetConveyorAccel(int accel)
        {
            Roller1.SetAcceleration(accel);
            Roller2.SetAcceleration(accel);
            Roller3.SetAcceleration(accel);
        }
        private void SetConveyorDeccel(int deccel)
        {
            Roller1.SetDeceleration(deccel);
            Roller2.SetDeceleration(deccel);
            Roller3.SetDeceleration(deccel);
        }
        #endregion
    }
}
