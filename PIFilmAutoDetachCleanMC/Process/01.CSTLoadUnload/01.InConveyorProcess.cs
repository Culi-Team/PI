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
        private bool CST_Exist => CST_Det1 || CST_Det2;
        private IDInput InCompleteButton => _devices.Inputs.InCompleteButton;
        private IDInput InMutingButton => _devices.Inputs.InMutingButton;
        #endregion

        #region Outputs
        private IDOutput InMutingButtonLamp => _devices.Outputs.InMutingButtonLamp;
        private IDOutput InCstMutingLightCurtain => _devices.Outputs.InCstLightCurtainMuting;
        private IDOutput InCstInterlockLightCurtain => _devices.Outputs.InCstLightCurtainInterlock;
        #endregion

        #region Flags
        private bool FlagDownStreamRequestCassetteIn
        {
            get
            {
                return _inConveyorInput[(int)EInConveyorProcessInput.REQUEST_CST_IN];
            }
        }
        #endregion

        #region Cylinders
        private ICylinder StopperCylinder => _devices.Cylinders.InCV_StopperCyl;
        #endregion

        #region Rollers
        private BD201SRollerController Roller1 => _devices.RollerList.InConveyorRoller1;
        private BD201SRollerController Roller2 => _devices.RollerList.InConveyorRoller2;
        private BD201SRollerController Roller3 => _devices.RollerList.InConveyorRoller3;
        #endregion

        #region Override Method
        public override bool ProcessToStop()
        {
            if (ProcessStatus == EProcessStatus.ToStopDone)
            {
                Thread.Sleep(50);
                return true;
            }

            Roller1.Stop();
            Roller2.Stop();
            Roller3.Stop();

            return base.ProcessToStop();
        }

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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.InConveyorLoad:
                    Sequence_InConveyorLoad();
                    break;
                case ESequence.InWorkCSTLoad:
                    Sequence_InWorkCSTLoad();
                    break;
                default:
                    Sequence = ESequence.Stop;
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
            switch ((EInConveyor_AutoRunStep)Step.RunStep)
            {
                case EInConveyor_AutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EInConveyor_AutoRunStep.CSTDetect_Check:
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
                        Step.RunStep = (int)EInConveyor_AutoRunStep.End;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EInConveyor_AutoRunStep.End:
                    Log.Info("Sequence In Conveyor Load");
                    Sequence = ESequence.InConveyorLoad;
                    break;
            }
        }

        private void Sequence_Ready()
        {
            switch ((EInConveyor_ReadyStep)Step.RunStep)
            {
                case EInConveyor_ReadyStep.Start:
                    Log.Debug("Ready Start");
                    Step.RunStep++;
                    break;
                case EInConveyor_ReadyStep.SensorStatus_Check:
                    if (CST_Det2 && StopperCylinder.IsBackward)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Position_Error);
                        break;
                    }
                    if (StopperCylinder.IsForward)
                    {
                        Step.RunStep = (int)EInConveyor_ReadyStep.End;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case EInConveyor_ReadyStep.StopperUp:
                    Log.Debug($"Move {StopperCylinder} up");
                    StopperCylinder.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => StopperCylinder.IsForward);
                    Step.RunStep++;
                    break;
                case EInConveyor_ReadyStep.StopperUp_Check:

                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.InConveyor_CST_Stopper_Up_Fail);
                        break;
                    }

                    Log.Debug($"Move {StopperCylinder} up done");
                    Step.RunStep++;
                    break;
                case EInConveyor_ReadyStep.End:
                    Log.Debug("Ready End");
                    Sequence = ESequence.Stop;
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
                    if (FlagDownStreamRequestCassetteIn == false)
                    {
                        Wait(20);
                        break;
                    }

                    // NO CST DETECT ON SEMI AUTO, STOP PROCESS
                    if (CST_Exist == false)
                    {
                        if (Parent?.Sequence != ESequence.AutoRun)
                        {
                            Sequence = ESequence.Stop;
                            break;
                        }
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
                    SimulationInputSetter.SetSimInput(_devices.Inputs.InCstWorkFixtureDetect, true);
#endif
                    Step.RunStep++;
                    break;
                case EInWorkConveyorProcessInWorkCSTLoadStep.Wait_InWorkCSTLoadDone:
                    if (FlagDownStreamRequestCassetteIn == true)
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
                        Wait(1000);
                        Step.RunStep = (int)EInConveyorLoadStep.Conveyor_Stop;
                        break;
                    }
                    if (CST_Det2)
                    {
                        Wait(1000);
                        Step.RunStep = (int)EInConveyorLoadStep.Conveyor_Stop;
                        break;
                    }
                    if (CST_Det1)
                    {
                        Step.RunStep++;
                        break;
                    }

                    // IF NO CST DETECT ON SEMI AUTO
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    // Wait for CST on AUTO RUN
                    Wait(50);
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
            Thread.Sleep(300);
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
