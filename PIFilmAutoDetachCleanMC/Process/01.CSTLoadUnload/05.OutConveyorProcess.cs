using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Sequence;
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
    public class OutConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _outConveyorInput;
        private readonly IDOutputDevice _outConveyorOutput;
        #endregion

        #region Constructor
        public OutConveyorProcess(Devices devices,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            CommonRecipe commonRecipe,
            [FromKeyedServices("OutConveyorInput")]IDInputDevice outConveyorInput,
            [FromKeyedServices("OutConveyorOutput")]IDOutputDevice outConveyorOutput)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
            _outConveyorInput = outConveyorInput;
            _outConveyorOutput = outConveyorOutput;
        }
        #endregion

        #region Inputs
        private IDInput CSTDetect1 => _devices.Inputs.OutCstDetect1;
        private IDInput CSTDetect2 => _devices.Inputs.OutCstDetect2;
        private IDInput OutCompleteButton => _devices.Inputs.OutCompleteButton;
        private IDInput OutMutingButton => _devices.Inputs.OutMutingButton;
        private IDInput OutCSTLightCurtain => _devices.Inputs.OutCstLightCurtainAlarmDetect;
        #endregion

        #region Outputs
        private IDOutput OutCompleteButtonLamp => _devices.Outputs.OutCompleteButtonLamp;
        private IDOutput OutMutingButtonLamp => _devices.Outputs.OutMutingButtonLamp;
        private IDOutput OutCstMutingLightCurtain1 => _devices.Outputs.OutCstLightCurtainMuting1;
        private IDOutput OutCstMutingLightCurtain2 => _devices.Outputs.OutCstLightCurtainMuting2;
        #endregion

        #region Cylinders
        private ICylinder CstStopper => _devices.Cylinders.OutCstStopperUpDown;
        #endregion

        #region Rollers
        private ISpeedController Roller1 => _devices.SpeedControllerList.OutConveyorRoller1;
        private ISpeedController Roller2 => _devices.SpeedControllerList.OutConveyorRoller2;

        #endregion

        #region Flags
        private bool FlagOutWorkConveyorRequestCSTOut
        {
            get
            {
                return _outConveyorInput[(int)EOutConveyorProcessInput.OUT_WORK_CONVEYOR_REQUEST_CST_OUT];
            }
        }

        private bool FlagOutConveyorReady
        {
            set
            {
                _outConveyorOutput[(int)EOutConveyorProcessOutput.OUT_CONVEYOR_READY] = value;
            }
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EOutConveyorOriginStep)Step.OriginStep)
            {
                case EOutConveyorOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EOutConveyorOriginStep.Roller_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.OriginStep++;
                    break;
                case EOutConveyorOriginStep.End:
                    Log.Debug("Origin End");
                    Step.OriginStep++;
                    ProcessStatus = EProcessStatus.OriginDone;
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
                    Sequence_OutWorkCSTUnload();
                    break;
                case ESequence.OutConveyorUnload:
                    Sequence_OutConveyorUnload();
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
        private void ConveyorRunStop(bool bRun)
        {
            if (bRun)
            {
                Roller1.Start();
                Roller2.Start();
            }
            else
            {
                Roller1.Stop();
                Roller2.Stop();
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((EOutConveyorAutoRunStep)Step.RunStep)
            {
                case EOutConveyorAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EOutConveyorAutoRunStep.CSTDetect_Check:
                    if (CSTDetect1.Value || CSTDetect2.Value)
                    {
                        Log.Info("Sequence Out CST Unload");
                        Sequence = ESequence.OutConveyorUnload;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EOutConveyorAutoRunStep.End:
                    Log.Info("Sequence Out Work CST Unload");
                    Sequence = ESequence.OutWorkCSTUnLoad;
                    break;
            }
        }

        private void Sequence_OutConveyorUnload()
        {
            switch ((EOutConveyorUnloadStep)Step.RunStep)
            {
                case EOutConveyorUnloadStep.Start:
                    Log.Debug("Out Conveyor Unload Start");
                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.Stopper_Up:
                    Log.Debug("Stopper Up");
                    CstStopper.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CstStopper.IsForward);
                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.Stopper_Up_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.OutConveyor_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Up Done");
                    break;
                case EOutConveyorUnloadStep.CSTDetect_Check:
                    if(CSTDetect1.Value == true && CSTDetect2.Value == false)
                    {
                        Step.RunStep++;
                        break;
                    }
                    if(CSTDetect1.Value == true && CSTDetect2.Value == true)
                    {
                        Step.RunStep = (int)EOutConveyorUnloadStep.Conveyor_Stop;
                        break;
                    }
                    break;
                case EOutConveyorUnloadStep.Conveyor_Run:
                    ConveyorRunStop(true);
                    Step.RunStep = (int)EOutConveyorUnloadStep.CSTDetect_Check;
                    break;
                case EOutConveyorUnloadStep.Conveyor_Stop:
                    ConveyorRunStop(false);
                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.Wait_CSTUnload:
                    if(CSTDetect1.Value == false && CSTDetect2.Value == false)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Wait(20);
                    break;
                case EOutConveyorUnloadStep.End:
                    Log.Debug("Out Conveyor Unload End");
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Out Work CST Unload");
                    Sequence = ESequence.OutWorkCSTUnLoad;
                    break;
            }
        }

        private void Sequence_OutWorkCSTUnload()
        {
            switch ((EOutConveyorProcessOutWorkCSTUnloadStep)Step.RunStep)
            {
                case EOutConveyorProcessOutWorkCSTUnloadStep.Start:
                    Log.Debug("Out Work CST Unload Step");
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Stopper_Up:
                    Log.Debug("Stopper Up");
                    CstStopper.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CstStopper.IsForward);
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Stopper_Up_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.OutConveyor_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Up Done");
                    Log.Debug("Wait Out Work CST Request Unload");
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Wait_OutWorkCSTRequestUnload:
                    if(FlagOutWorkConveyorRequestCSTOut == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Set_FlagOutConveyorReady:
                    Log.Debug("Set Flag Out Conveyor Ready");
                    FlagOutConveyorReady = true;
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Conveyor_Run:
                    Log.Debug("Conveyor Run");
                    ConveyorRunStop(true);
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Wait_OutWorkCSTUnloadDone:
                    if(CSTDetect1.Value == true && CSTDetect2.Value == true)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Wait(20);
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.End:
                    Log.Debug("Out Work CST Unload End");
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Out Conveyor Unload");
                    Sequence = ESequence.OutConveyorUnload;
                    break;
            }
        }
        #endregion
    }
}
