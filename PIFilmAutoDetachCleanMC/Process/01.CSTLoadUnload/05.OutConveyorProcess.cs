using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Sequence;
using EQX.Device.SpeedController;
using EQX.InOut.Virtual;
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
        private readonly MachineStatus _machineStatus;
        #endregion

        #region Constructor
        public OutConveyorProcess(Devices devices,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("OutConveyorInput")] IDInputDevice outConveyorInput,
            [FromKeyedServices("OutConveyorOutput")] IDOutputDevice outConveyorOutput)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _outConveyorInput = outConveyorInput;
            _outConveyorOutput = outConveyorOutput;
        }
        #endregion

        #region Inputs
        private bool CSTDetect1 => _devices.Inputs.OutCstDetect1.Value;
        private bool CSTDetect2 => _devices.Inputs.OutCstDetect2.Value;
        private bool CSTExist => CSTDetect1 || CSTDetect2;
        private IDInput OutCompleteButton => _devices.Inputs.OutCompleteButton;
        private IDInput OutMutingButton => _devices.Inputs.OutMutingButton;
        private IDInput OutCSTLightCurtain => _devices.Inputs.OutCstLightCurtainAlarmDetect;
        #endregion

        #region Outputs
        private IDOutput OutMutingButtonLamp => _devices.Outputs.OutMutingButtonLamp;
        private IDOutput OutCstMutingLightCurtain => _devices.Outputs.OutCstLightCurtainMuting;
        private IDOutput OutCstInterlockLightCurtain => _devices.Outputs.OutCstLightCurtainInterlock;
        #endregion

        #region Cylinders
        private ICylinder CstStopper => _devices.Cylinders.OutCV_StopperCyl;
        #endregion

        #region Rollers
        private BD201SRollerController Roller1 => _devices.RollerList.OutConveyorRoller1;
        private BD201SRollerController Roller2 => _devices.RollerList.OutConveyorRoller2;

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
        public override bool PreProcess()
        {
            if (_devices.Inputs.OutCompleteButton.Value)
            {
                if (CstStopper.IsForward)
                {
                    CstStopper.Backward();
                    Task.Delay(30000).ContinueWith(t =>
                    {
                        CstStopper.Forward();
                    });
                }
            }

            return base.PreProcess();
        }
        public override bool ProcessToStop()
        {
            if (ProcessStatus == EProcessStatus.ToStopDone)
            {
                Thread.Sleep(50);
                return true;
            }

            Roller1.Stop();
            Roller2.Stop();

            return base.ProcessToStop();
        }

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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    Sequence_OutWorkCSTUnload();
                    break;
                case ESequence.OutConveyorUnload:
                    Sequence_OutConveyorUnload();
                    break;
                default:
                    Sequence = ESequence.Stop;
                    break;
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((EOutConveyorToRunStep)Step.ToRunStep)
            {
                case EOutConveyorToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EOutConveyorToRunStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.ToRunStep++;
                    break;
                case EOutConveyorToRunStep.Set_ConveyorSpeed:
                    Log.Debug("Conveyor Set Speed");
                    ConveyorSetSpeed((int)_cstLoadUnloadRecipe.ConveyorSpeed);
                    Step.ToRunStep++;
                    break;
                case EOutConveyorToRunStep.Set_ConveyorAccel:
                    Log.Debug("Conveyor Set Accel");
                    ConveyorSetAccel((int)_cstLoadUnloadRecipe.ConveyorAcc);
                    Step.ToRunStep++;
                    break;
                case EOutConveyorToRunStep.Set_ConveyorDeccel:
                    Log.Debug("Conveyor Set Deccel");
                    ConveyorSetDeccel((int)_cstLoadUnloadRecipe.ConveyorDec);
                    Step.ToRunStep++;
                    break;
                case EOutConveyorToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<EOutConveyorProcessOutput>)_outConveyorOutput).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case EOutConveyorToRunStep.End:
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
        private void ConveyorRunStop(bool bRun)
        {
            if (bRun)
            {
                Roller1.Run();
                Roller2.Run();
            }
            else
            {
                Roller1.Stop();
                Roller2.Stop();
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((EOutConveyor_AutoRunStep)Step.RunStep)
            {
                case EOutConveyor_AutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EOutConveyor_AutoRunStep.CSTDetect_Check:
                    if (CSTDetect1 || CSTDetect2)
                    {
                        Log.Info("Sequence Out CST Unload");
                        Sequence = ESequence.OutConveyorUnload;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EOutConveyor_AutoRunStep.End:
                    Log.Info("Sequence Out Work CST Unload");
                    Sequence = ESequence.OutWorkCSTUnLoad;
                    break;
            }
        }

        private void Sequence_Ready()
        {
            switch ((EOutConveyor_ReadyStep)Step.RunStep)
            {
                case EOutConveyor_ReadyStep.Start:
                    Log.Debug("Ready Start");
                    Step.RunStep++;
                    break;
                case EOutConveyor_ReadyStep.StopperUp:
                    if (CstStopper.IsForward)
                    {
                        Step.RunStep = (int)EOutConveyor_ReadyStep.End;
                        break;
                    }

                    Log.Debug($"Move {CstStopper} Up");
                    CstStopper.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CstStopper.IsForward);
                    Step.RunStep++;
                    break;
                case EOutConveyor_ReadyStep.StopperUp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.OutConveyor_Stopper_Up_Fail);
                        break;
                    }

                    Log.Debug($"{CstStopper} is up now");
                    Step.RunStep++;
                    break;
                case EOutConveyor_ReadyStep.End:
                    Log.Debug("Ready End");
                    Sequence = ESequence.Stop;
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
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CstStopper.IsForward);
                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.Stopper_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.OutConveyor_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Up Done");
                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.CSTDetect_Check:
                    if (CSTDetect2 == true && !_machineStatus.IsDryRunMode)
                    {
                        Step.RunStep = (int)EOutConveyorUnloadStep.Conveyor_Stop;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.Conveyor_Run:
                    ConveyorRunStop(true);
                    if (!_machineStatus.IsDryRunMode)
                    {
                        Step.RunStep = (int)EOutConveyorUnloadStep.CSTDetect_Check;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.Conveyor_Stop:
                    ConveyorRunStop(false);
                    Step.RunStep++;
                    break;
                case EOutConveyorUnloadStep.Wait_CSTUnload:
                    if ((CSTDetect1 == false && CSTDetect2 == false) || _machineStatus.IsDryRunMode)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Wait(20);
                    break;
                case EOutConveyorUnloadStep.End:
                    Log.Debug("Out Conveyor Unload End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
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
                case EOutConveyorProcessOutWorkCSTUnloadStep.CstExist_Check:
                    if (CSTExist)
                    {
                        if (Parent?.Sequence != ESequence.AutoRun)
                        {
                            Sequence = ESequence.Stop;
                            break;
                        }

                        Wait(50);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Wait_OutWorkCSTRequestUnload:
                    if (FlagOutWorkConveyorRequestCSTOut == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Stopper_Up:
                    Log.Debug("Stopper Up");
                    CstStopper.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CstStopper.IsForward);
                    Step.RunStep++;
                    break;
                case EOutConveyorProcessOutWorkCSTUnloadStep.Stopper_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.OutConveyor_Stopper_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper Up Done");
                    Log.Debug("Wait Out Work CST Request Unload");
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
                    if ((CSTDetect2 == true) || _machineStatus.IsDryRunMode)
                    {
                        Log.Debug("Clear Flag Out Conveyor Ready");
                        FlagOutConveyorReady = false;

                        Wait(2000);
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
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Out Conveyor Unload");
                    Sequence = ESequence.OutConveyorUnload;
                    break;
            }
        }

        private void ConveyorSetSpeed(int speed)
        {
            Roller1.SetSpeed(speed);
            Roller2.SetSpeed(speed);
        }
        private void ConveyorSetAccel(int accel)
        {
            Roller1.SetAcceleration(accel);
            Roller2.SetAcceleration(accel);
        }
        private void ConveyorSetDeccel(int deccel)
        {
            Roller1.SetDeceleration(deccel);
            Roller2.SetDeceleration(deccel);
        }
        #endregion
    }
}
