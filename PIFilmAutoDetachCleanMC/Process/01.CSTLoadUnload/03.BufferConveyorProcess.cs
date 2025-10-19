using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Sequence;
using EQX.Device.SpeedController;
using EQX.InOut;
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
    public class BufferConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _bufferConveyorInput;
        private readonly IDOutputDevice _bufferConveyorOutput;
        private readonly MachineStatus _machineStatus;
        #endregion

        #region Constructor
        public BufferConveyorProcess(Devices devices,
            CSTLoadUnloadRecipe cstLoadUnloadRecipe,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("BufferConveyorInput")] IDInputDevice bufferConveyorInput,
            [FromKeyedServices("BufferConveyorOutput")] IDOutputDevice bufferConveyorOutput)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _bufferConveyorInput = bufferConveyorInput;
            _bufferConveyorOutput = bufferConveyorOutput;
        }
        #endregion

        #region Inputs
        private bool BufferDetect1 => _devices.Inputs.BufferCstDetect1.Value;
        private bool BufferDetect2 => _devices.Inputs.BufferCstDetect2.Value;

        #endregion

        #region Cylinders
        private ICylinder BufferStopper1 => _devices.Cylinders.BufferCV_StopperCyl1;
        private ICylinder BufferStopper2 => _devices.Cylinders.BufferCV_StopperCyl2;
        #endregion

        #region Rollers
        private SD201SSpeedController BufferRoller1 => _devices.SpeedControllerList.BufferConveyorRoller1;
        private SD201SSpeedController BufferRoller2 => _devices.SpeedControllerList.BufferConveyorRoller2;
        #endregion

        #region Flags
        private bool FlagInWorkConveyorRequestCSTOut
        {
            get
            {
                return _bufferConveyorInput[(int)EBufferConveyorProcessInput.IN_WORK_CONVEYOR_REQUEST_CST_OUT];
            }
        }

        private bool FlagBufferConveyorReady
        {
            set
            {
                _bufferConveyorOutput[(int)EBufferConveyorProcessOutput.BUFFER_CONVEYOR_READY] = value;
            }
        }

        private bool FlagOutWorkConveyorRequestCSTIn
        {
            get
            {
                return _bufferConveyorInput[(int)EBufferConveyorProcessInput.OUT_WORK_CONVEYOR_REQUEST_CST_IN];
            }
        }
        #endregion

        #region Override Method
        public override bool ProcessOrigin()
        {
            switch ((EBufferConveyorOriginStep)Step.OriginStep)
            {
                case EBufferConveyorOriginStep.Start:
                    Log.Debug("Origin start");
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.Stopper_Cylinder_Down:
                    Log.Debug("Stopper Down");
                    BufferStopper1.Backward();
                    BufferStopper2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BufferStopper1.IsBackward && BufferStopper2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.Stopper_Cylinder_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (BufferStopper1.IsBackward == false)
                        {
                            RaiseWarning((int)EWarning.BufferConveyor_Stopper1_Down_Fail);
                            break;
                        }

                        RaiseWarning((int)EWarning.BufferConveyor_Stopper2_Down_Fail);
                        break;
                    }
                    Log.Debug("Stopper Cylinder Down Done");
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.Roller_Stop:
                    BufferRoller1.Stop();
                    BufferRoller2.Stop();
                    Log.Debug("Roller Stop");
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.End:
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
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    Sequence_InWorkCSTUnload();
                    break;
                case ESequence.InWorkCSTTilt:
                    break;
                case ESequence.OutWorkCSTLoad:
                    Sequence_OutWorkCSTLoad();
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.OutConveyorUnload:
                    break;
                case ESequence.OutWorkCSTTilt:
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
            switch ((EBufferConveyorToRunStep)Step.ToRunStep)
            {
                case EBufferConveyorToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EBufferConveyorToRunStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.ToRunStep++;
                    break;
                case EBufferConveyorToRunStep.Set_ConveyorSpeed:
                    Log.Debug("Set Conveyor Speed");
                    ConveyorSetSpeed((int)_cstLoadUnloadRecipe.ConveyorSpeed);
                    Step.ToRunStep++;
                    break;
                case EBufferConveyorToRunStep.Set_ConveyorAccel:
                    Log.Debug("Set Conveyor Accel");
                    ConveyorSetAccel((int)_cstLoadUnloadRecipe.ConveyorAcc);
                    Step.ToRunStep++;
                    break;
                case EBufferConveyorToRunStep.Set_ConveyorDeccel:
                    Log.Debug("Set Conveyor Deccel");
                    ConveyorSetDeccel((int)_cstLoadUnloadRecipe.ConveyorDec);
                    Step.ToRunStep++;
                    break;
                case EBufferConveyorToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<EBufferConveyorProcessOutput>)_bufferConveyorOutput).Clear();
                    Step.ToRunStep++;
                    break;
                case EBufferConveyorToRunStep.End:
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
            switch ((EBufferConveyorAutoRunStep)Step.RunStep)
            {
                case EBufferConveyorAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EBufferConveyorAutoRunStep.CSTDetect_Check:
                    if(_machineStatus.IsDryRunMode)
                    {
                        Log.Info("Sequence Out Work CST Load");
                        Sequence = ESequence.OutWorkCSTLoad;
                        break;
                    }    
                    if (BufferDetect1 == false && BufferDetect2 == false)
                    {
                        Log.Info("Sequence In Work CST Unload");
                        Sequence = ESequence.InWorkCSTUnLoad;
                        break;
                    }
                    if (BufferDetect1 == true && BufferDetect2 == true)
                    {
                        Log.Info("Sequence Out Work CST Load");
                        Sequence = ESequence.OutWorkCSTLoad;
                        break;
                    }
                    if (BufferDetect1 == true && BufferStopper1.IsBackward)
                    {
                        Log.Info("Sequence In Work CST Unload");
                        Sequence = ESequence.InWorkCSTUnLoad;
                        break;
                    }
                    if (BufferDetect2 == true && BufferStopper2.IsBackward)
                    {
                        RaiseWarning((int)EWarning.BufferConveyor_CST_Position_Error);
                        break;
                    }
                    break;
                case EBufferConveyorAutoRunStep.End:
                    break;
            }
        }

        private void Sequence_InWorkCSTUnload()
        {
            switch ((EBufferConveyorInWorkCSTUnloadStep)Step.RunStep)
            {
                case EBufferConveyorInWorkCSTUnloadStep.Start:
                    Log.Debug("In Work CST Unload Start");
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.Stopper1_Down:
                    Log.Debug("Stopper 1 Down");
                    BufferStopper1.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BufferStopper1.IsBackward);
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.Stopper1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.BufferConveyor_Stopper1_Down_Fail);
                        break;
                    }
                    Log.Debug("Stopper 1 Down Done");
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.Stopper2_Up:
                    Log.Debug("Stopper 2 Up");
                    BufferStopper2.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BufferStopper2.IsForward);
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.Stopper2_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.BufferConveyor_Stopper2_Up_Fail);
                        break;
                    }
                    Log.Debug("Stopper 2 Up Done");
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.CSTDetect_Check:
                    if (_machineStatus.IsDryRunMode == false)
                    {
                        if (BufferDetect1 == true && BufferDetect2 == false)
                        {
                            Step.RunStep = (int)EBufferConveyorInWorkCSTUnloadStep.Conveyor_Run;
                            break;
                        }
                        if (BufferDetect1 == true && BufferDetect2 == true)
                        {
                            Step.RunStep = (int)EBufferConveyorInWorkCSTUnloadStep.Conveyor_Stop;
                            break;
                        }
                        if (BufferDetect1 == false && BufferDetect2 == true)
                        {
                            RaiseWarning((int)EWarning.BufferConveyor_CST_Position_Error);
                            break;
                        }
                    }

                    Log.Debug("Set Flag Buffer Conveyor Ready");
                    FlagBufferConveyorReady = true;
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.Wait_InWorkCSTRequestCSTOut:
                    if (FlagInWorkConveyorRequestCSTOut == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.Conveyor_Run:
                    ConveyorRunStop(true);
#if SIMULATION
                    Wait(2000);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.BufferCstDetect1, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.BufferCstDetect2, true);
#endif
                    Step.RunStep = (int)EBufferConveyorInWorkCSTUnloadStep.CSTDetect_Check;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Log.Debug("Clear Flag Buffer Conveyor Ready");
                    FlagBufferConveyorReady = false;
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.BothStopper_Up:
                    BufferStopper1.Forward();
                    BufferStopper2.Forward();

                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BufferStopper1.IsForward && BufferStopper2.IsForward);

                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.BothStopper_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.BufferConveyor_Stopper_Up_Fail);
                        break;
                    }

                    Log.Debug("Both Stopper Up Done.");
                    Step.RunStep++;
                    break;
                case EBufferConveyorInWorkCSTUnloadStep.End:
                    Log.Debug("In Work CST Unload End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Out Work CST Load");
                    Sequence = ESequence.OutWorkCSTLoad;
                    break;
            }
        }

        private void Sequence_OutWorkCSTLoad()
        {
            switch ((EBufferConveyorOutWorkCSTLoadStep)Step.RunStep)
            {
                case EBufferConveyorOutWorkCSTLoadStep.Start:
                    Log.Debug("Out Work CST Load Start");
                    Step.RunStep++;
                    break;
                case EBufferConveyorOutWorkCSTLoadStep.Wait_OutWorkConveyorRequestCSTIn:
                    if (FlagOutWorkConveyorRequestCSTIn == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EBufferConveyorOutWorkCSTLoadStep.Stopper2_Down:
                    Log.Debug("Stopper 2 Down");
                    BufferStopper2.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BufferStopper2.IsBackward);
                    Step.RunStep++;
                    break;
                case EBufferConveyorOutWorkCSTLoadStep.Stopper2_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.BufferConveyor_Stopper2_Down_Fail);
                        break;
                    }
                    Log.Debug("Stopper 2 Down Done");
                    Step.RunStep++;
                    break;
                case EBufferConveyorOutWorkCSTLoadStep.Conveyor_Run:
                    Log.Debug("Conveyor Run");
                    ConveyorRunStop(true);
                    Log.Debug("Wait Out Work Conveyor Load Done");
#if SIMULATION
                    Thread.Sleep(1000);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstWorkDetect1, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstWorkDetect2, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.OutCstWorkDetect3, true);

                    SimulationInputSetter.SetSimInput(_devices.Inputs.BufferCstDetect1, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.BufferCstDetect2, false);
#endif
                    Step.RunStep++;
                    break;
                case EBufferConveyorOutWorkCSTLoadStep.Wait_OutWorkConveyorLoadDone:
                    if (FlagOutWorkConveyorRequestCSTIn == true)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EBufferConveyorOutWorkCSTLoadStep.Conveyor_Stop:
                    Log.Debug("Conveyor Stop");
                    ConveyorRunStop(false);
                    Step.RunStep++;
                    break;
                case EBufferConveyorOutWorkCSTLoadStep.End:
                    Log.Debug("Out Work CST Load End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence In Work CST Unload");
                    Sequence = ESequence.InWorkCSTUnLoad;
                    break;
            }
        }

        private void ConveyorRunStop(bool bRun)
        {
            if (bRun)
            {
                BufferRoller1.Run();
                BufferRoller2.Run();
            }
            else
            {
                BufferRoller1.Stop();
                BufferRoller2.Stop();
            }
        }

        private void ConveyorSetSpeed(int speed)
        {
            BufferRoller1.SetSpeed(speed);
            BufferRoller2.SetSpeed(speed);
        }
        private void ConveyorSetAccel(int accel)
        {
            BufferRoller1.SetAcceleration(accel);
            BufferRoller2.SetAcceleration(accel);
        }
        private void ConveyorSetDeccel(int deccel)
        {
            BufferRoller1.SetDeceleration(deccel);
            BufferRoller2.SetDeceleration(deccel);
        }
        #endregion
    }
}
