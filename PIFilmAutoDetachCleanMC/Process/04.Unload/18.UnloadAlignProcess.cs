using EQX.Core.InOut;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class UnloadAlignProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _unloadAlignInput;
        private readonly IDOutputDevice _unloadAlignOutput;
        private readonly MachineStatus _machineStatus;

        private ICylinder AlignCyl1 => _devices.Cylinders.UnloadAlign_UpDownCyl1;
        private ICylinder AlignCyl2 => _devices.Cylinders.UnloadAlign_UpDownCyl2;
        private ICylinder AlignCyl3 => _devices.Cylinders.UnloadAlign_UpDownCyl3;
        private ICylinder AlignCyl4 => _devices.Cylinders.UnloadAlign_UpDownCyl4;
        private bool IsAlign => AlignCyl1.IsForward && AlignCyl2.IsForward && AlignCyl3.IsForward && AlignCyl4.IsForward;
        private bool IsUnalign => AlignCyl1.IsBackward && AlignCyl2.IsBackward && AlignCyl3.IsBackward && AlignCyl4.IsBackward;

        private IDOutput AlignVac1 => _devices.Outputs.UnloadGlassAlignVac1OnOff;
        private IDOutput AlignVac2 => _devices.Outputs.UnloadGlassAlignVac2OnOff;
        private IDOutput AlignVac3 => _devices.Outputs.UnloadGlassAlignVac3OnOff;
        private IDOutput AlignVac4 => _devices.Outputs.UnloadGlassAlignVac4OnOff;
        private IDOutput AlignBlow1 => _devices.Outputs.UnloadGlassAlignBlow1OnOff;
        private IDOutput AlignBlow2 => _devices.Outputs.UnloadGlassAlignBlow2OnOff;
        private IDOutput AlignBlow3 => _devices.Outputs.UnloadGlassAlignBlow3OnOff;
        private IDOutput AlignBlow4 => _devices.Outputs.UnloadGlassAlignBlow4OnOff;

        private IDInput GlassVac1 => _devices.Inputs.UnloadGlassAlignVac1;
        private IDInput GlassVac2 => _devices.Inputs.UnloadGlassAlignVac2;
        private IDInput GlassVac3 => _devices.Inputs.UnloadGlassAlignVac3;
        private IDInput GlassVac4 => _devices.Inputs.UnloadGlassAlignVac4;

        private bool GlassDetect1 => _devices.Inputs.UnloadGlassDetect1.Value;
        private bool GlassDetect2 => _devices.Inputs.UnloadGlassDetect2.Value;
        private bool GlassDetect3 => _devices.Inputs.UnloadGlassDetect3.Value;
        private bool GlassDetect4 => _devices.Inputs.UnloadGlassDetect4.Value;

        private bool IsGlassVac1 => _devices.Inputs.UnloadGlassAlignVac1.Value;
        private bool IsGlassVac2 => _devices.Inputs.UnloadGlassAlignVac2.Value;
        private bool IsGlassVac3 => _devices.Inputs.UnloadGlassAlignVac3.Value;
        private bool IsGlassVac4 => _devices.Inputs.UnloadGlassAlignVac4.Value;
        private bool IsGlassVac => IsGlassVac1 && IsGlassVac2 && IsGlassVac3 && IsGlassVac4;

        private bool IsGlassDetect => GlassDetect1 && GlassDetect2 && GlassDetect3 && GlassDetect4;
        #endregion

        #region Flags
        private bool FlagUnloadAlignRequestRobotUnload
        {
            set
            {
                _unloadAlignOutput[(int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD] = value;
            }
        }

        private bool FlagUnloadAlignReady
        {
            set
            {
                _unloadAlignOutput[(int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_READY] = value;
            }
        }

        private bool FlagUnloadTransferLeftPlaceDone
        {
            get
            {
                return _unloadAlignInput[(int)EUnloadAlignProcessInput.UNLOAD_TRANSFER_LEFT_PLACE_DONE];
            }
        }

        private bool FlagUnloadTransferRightPlaceDone
        {
            get
            {
                return _unloadAlignInput[(int)EUnloadAlignProcessInput.UNLOAD_TRANSFER_RIGHT_PLACE_DONE];
            }
        }

        private bool FlagRobotUnloadDone
        {
            get
            {
                return _unloadAlignInput[(int)EUnloadAlignProcessInput.ROBOT_UNLOAD_PICK_DONE];
            }
        }
        #endregion

        #region Constructor
        public UnloadAlignProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("UnloadAlignInput")] IDInputDevice unloadAlignInput,
            [FromKeyedServices("UnloadAlignOutput")] IDOutputDevice unloadAlignOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _unloadAlignInput = unloadAlignInput;
            _unloadAlignOutput = unloadAlignOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EUnloadAlignOriginStep)Step.OriginStep)
            {
                case EUnloadAlignOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EUnloadAlignOriginStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignUnalign(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsUnalign);
                    Step.OriginStep++;
                    break;
                case EUnloadAlignOriginStep.Cyl_Align_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.UnloadAlign_AlignCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Align Down Done");
                    Step.OriginStep++;
                    break;
                case EUnloadAlignOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    return true;
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
                    Sequence = ESequence.Stop;
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    Sequence_UnloadTransferPlace();
                    break;
                case ESequence.UnloadTransferRightPlace:
                    Sequence_UnloadTransferPlace();
                    break;
                case ESequence.UnloadAlignGlass:
                    Sequence_UnloadAlignGlass();
                    break;
                case ESequence.UnloadRobotPick:
                    Sequence_UnloadRobotPick();
                    break;
                default:
                    Sequence = ESequence.Stop;
                    break;
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((EUnloadAlignProcessToRunStep)Step.ToRunStep)
            {
                case EUnloadAlignProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EUnloadAlignProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<EUnloadAlignProcessOutput>)_unloadAlignOutput).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case EUnloadAlignProcessToRunStep.End:
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
        private void AlignUnalign(bool bAlignUnAlign)
        {
            if (bAlignUnAlign)
            {
                AlignCyl1.Forward();
                AlignCyl2.Forward();
                AlignCyl3.Forward();
                AlignCyl4.Forward();
            }
            else
            {
                AlignCyl1.Backward();
                AlignCyl2.Backward();
                AlignCyl3.Backward();
                AlignCyl4.Backward();
            }
        }

        private void VacOnOff(bool bOnOff)
        {
            AlignVac1.Value = bOnOff;
            AlignVac2.Value = bOnOff;
            AlignVac3.Value = bOnOff;
            AlignVac4.Value = bOnOff;

            AlignBlow1.Value = !bOnOff;
            AlignBlow2.Value = !bOnOff;
            AlignBlow3.Value = !bOnOff;
            AlignBlow4.Value = !bOnOff;

            if (bOnOff == false)
            {
                Task.Delay(100).ContinueWith(t =>
                {
                    AlignBlow1.Value = false;
                    AlignBlow2.Value = false;
                    AlignBlow3.Value = false;
                    AlignBlow4.Value = false;
                });
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((EUnloadAlignAutoRunStep)Step.RunStep)
            {
                case EUnloadAlignAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EUnloadAlignAutoRunStep.GlassVac_Check:
                    if ((IsGlassVac || IsGlassDetect) && _machineStatus.IsDryRunMode == false)
                    {
                        Log.Info("Sequence Unload Align Glass");
                        Sequence = ESequence.UnloadAlignGlass;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadAlignAutoRunStep.End:
                    Log.Info("Sequence Unload Transfer Place");
                    Sequence = ESequence.UnloadTransferLeftPlace;
                    break;
            }
        }

        private void Sequence_UnloadAlignGlass()
        {
            switch ((EUnloadAlignStep)Step.RunStep)
            {
                case EUnloadAlignStep.Start:
                    Log.Debug("Unload Align Start");
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Cyl_Align_Up:
                    Log.Debug("Cylinder Align Up");
                    AlignUnalign(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsAlign);
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Cyl_Align_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.UnloadAlign_AlignCylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Align Up Done");
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Vacuum_Off_Align:
                    Log.Debug("Vacuum Off");
                    VacOnOff(false);
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    VacOnOff(true);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect1, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect2, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect3, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect4, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsGlassVac || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.UnloadAlign_Vacuum_Fail);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.GlassDetect_Check:
                    if (IsGlassDetect == false && !_machineStatus.IsDryRunMode)
                    {
                        RaiseWarning((int)EWarning.UnloadAlign_Glass_NotDetect);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignUnalign(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsAlign == false);
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Cyl_Align_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.UnloadAlign_AlignCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinder Align Down Done");
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.End:
                    Log.Debug("Unload Align End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Info("Unload Robot Pick");
                    Sequence = ESequence.UnloadRobotPick;
                    break;
            }
        }

        private void Sequence_UnloadRobotPick()
        {
            switch ((EUnloadAlignRobotPickStep)Step.RunStep)
            {
                case EUnloadAlignRobotPickStep.Start:
                    Log.Debug("Robot Pick Start");
                    Step.RunStep++;
                    break;
                case EUnloadAlignRobotPickStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    VacOnOff(false);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(GlassVac1, false);
                    SimulationInputSetter.SetSimInput(GlassVac2, false);
                    SimulationInputSetter.SetSimInput(GlassVac3, false);
                    SimulationInputSetter.SetSimInput(GlassVac4, false);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case EUnloadAlignRobotPickStep.Set_FlagRequestRobotUnload:
                    Log.Debug("Set Flag Request Robot Unload");
                    FlagUnloadAlignRequestRobotUnload = true;
                    Step.RunStep++;
                    break;
                case EUnloadAlignRobotPickStep.Wait_RobotUnloadDone:
                    if (FlagRobotUnloadDone == false)
                    {
                        Wait(20);
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect1, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect2, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect3, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadGlassDetect4, false);
#endif
                    Log.Debug("Clear Flag Request Robot Unload");
                    FlagUnloadAlignRequestRobotUnload = false;
                    Step.RunStep++;
                    break;
                case EUnloadAlignRobotPickStep.End:
                    Log.Debug("Unload Robot Pick End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Unload Transfer Place");
                    Sequence = ESequence.UnloadTransferLeftPlace;
                    break;
            }
        }

        private void Sequence_UnloadTransferPlace()
        {
            switch ((EUnloadAlignUnloadTransferPlaceStep)Step.RunStep)
            {
                case EUnloadAlignUnloadTransferPlaceStep.Start:
                    Log.Debug("Unload Transfer Place Start");
                    Step.RunStep++;
                    break;
                case EUnloadAlignUnloadTransferPlaceStep.GlassVac_Check:
                    Log.Debug("Glass Vacuum Check");
                    if (IsGlassVac || _machineStatus.IsDryRunMode)
                    {
                        Step.RunStep = (int)EUnloadAlignUnloadTransferPlaceStep.End;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadAlignUnloadTransferPlaceStep.Set_FlagUnloadAlignReady:
                    Log.Debug("Set Flag Unload Align Ready");
                    FlagUnloadAlignReady = true;
                    Log.Debug("Wait Unload Transfer Place Done");
                    Step.RunStep++;
                    break;
                case EUnloadAlignUnloadTransferPlaceStep.Wait_UnloadTransferPlaceDone:
                    if (FlagUnloadTransferLeftPlaceDone == false && FlagUnloadTransferRightPlaceDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Unload Align Ready");
                    FlagUnloadAlignReady = false;

                    Wait(200);
                    Step.RunStep = (int)EUnloadAlignUnloadTransferPlaceStep.GlassVac_Check;
                    break;
                case EUnloadAlignUnloadTransferPlaceStep.End:
                    Log.Debug("Unload Transfer Place End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }
                    Log.Info("Sequence Unload Align Glass");
                    Sequence = ESequence.UnloadAlignGlass;
                    break;
            }
        }
        #endregion
    }
}