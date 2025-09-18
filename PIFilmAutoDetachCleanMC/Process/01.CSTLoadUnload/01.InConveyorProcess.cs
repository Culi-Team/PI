using EQX.Core.Common;
using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Sequence;
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

        private const string InLightCurtainnMutingActionKey = "InLightCurtainMutingAction";
        #endregion

        #region Constructor
        public InConveyorProcess(Devices devices,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            CommonRecipe commonRecipe,
            [FromKeyedServices("InConveyorInput")] IDInputDevice inConveyorInput,
            [FromKeyedServices("BlinkTimer")] ActionAssignableTimer blinkTimer)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
            _inConveyorInput = inConveyorInput;
            _blinkTimer = blinkTimer;
        }
        #endregion

        #region Inputs
        private IDInput CST_Det1 => _devices.Inputs.InCstDetect1;
        private IDInput CST_Det2 => _devices.Inputs.InCstDetect2;
        private IDInput InCompleteButton => _devices.Inputs.InCompleteButton;
        private IDInput InMutingButton => _devices.Inputs.InMutingButton;
        #endregion

        #region Outputs
        private IDOutput InCompleteButtonLamp => _devices.Outputs.InCompleteButtonLamp;
        private IDOutput InMutingButtonLamp => _devices.Outputs.InMutingButtonLamp;
        private IDOutput InCstMutingLightCurtain1 => _devices.Outputs.InCstLightCurtainMuting1;
        private IDOutput InCstMutingLightCurtain2 => _devices.Outputs.InCstLightCurtainMuting2;
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
        private ISpeedController Roller1 => _devices.SpeedControllerList.InConveyorRoller1;
        private ISpeedController Roller2 => _devices.SpeedControllerList.InConveyorRoller2;
        private ISpeedController Roller3 => _devices.SpeedControllerList.InConveyorRoller3;
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
                case EConveyorOriginStep.CstStopper_Down:
                    Log.Debug("Cassette Stopper Up");
                    StopperCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => StopperCylinder.IsBackward);
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.CstStopper_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Cylinder Down Done");
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
                case ESequence.AlignGlass:
                case ESequence.TransferInShuttlePick:
                case ESequence.WETCleanLoad:
                case ESequence.WETClean:
                case ESequence.WETCleanUnload:
                case ESequence.TransferRotation:
                case ESequence.AFCleanLoad:
                case ESequence.AFClean:
                case ESequence.AFCleanUnload:
                case ESequence.UnloadTransferPlace:
                case ESequence.UnloadAlignGlass:
                case ESequence.UnloadRobotPick:
                case ESequence.UnloadRobotPlasma:
                case ESequence.UnloadRobotPlace:
                default:
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
                    if (CST_Det2.Value && StopperCylinder.IsForward)
                    {
                        Log.Info("Sequence In Work CST Load");
                        Sequence = ESequence.InWorkCSTLoad;
                        break;
                    }
                    if (CST_Det2.Value && StopperCylinder.IsBackward)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Position_Error);
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
                    Wait(_commonRecipe.CylinderMoveTimeout, () => StopperCylinder.IsBackward);
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
                    Wait(3000);
                    SimulationInputSetter.SetSimModbusInput(CST_Det1, false);
                    SimulationInputSetter.SetSimModbusInput(CST_Det2, false);

                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect1, true);
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect2, true);
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect3, true);
                    SimulationInputSetter.SetSimModbusInput(_devices.Inputs.InCstWorkDetect4, true);
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
                    Wait(_commonRecipe.CylinderMoveTimeout, () => StopperCylinder.IsForward);
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
                    if (Parent!.Sequence != ESequence.AutoRun)
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
                    StopperCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => StopperCylinder.IsBackward);
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
                    if (CST_Det2.Value)
                    {
                        Step.RunStep = (int)EInConveyorLoadStep.Conveyor_Stop;
                        break;
                    }
                    if (CST_Det1.Value)
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
            InCstMutingLightCurtain1.Value = bOnOff;
            InCstMutingLightCurtain2.Value = bOnOff;
        }

        private void ConveyorRunStop(bool bRun)
        {
            if (bRun)
            {
                Roller1.Start();
                Roller2.Start();
                Roller3.Start();
            }
            else
            {
                Roller1.Stop();
                Roller2.Stop();
                Roller3.Stop();
            }
        }
        #endregion
    }
}
