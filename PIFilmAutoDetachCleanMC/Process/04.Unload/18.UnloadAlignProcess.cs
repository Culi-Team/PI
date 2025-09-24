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

        private ICylinder AlignCyl1 => _devices.Cylinders.UnloadAlignCyl1UpDown;
        private ICylinder AlignCyl2 => _devices.Cylinders.UnloadAlignCyl2UpDown;
        private ICylinder AlignCyl3 => _devices.Cylinders.UnloadAlignCyl3UpDown;
        private ICylinder AlignCyl4 => _devices.Cylinders.UnloadAlignCyl4UpDown;
        private bool IsAlign => AlignCyl1.IsForward && AlignCyl2.IsForward && AlignCyl3.IsForward && AlignCyl4.IsForward;
        private bool IsUnalign => AlignCyl1.IsBackward && AlignCyl2.IsBackward && AlignCyl3.IsBackward && AlignCyl4.IsBackward;

        private IDOutput AlignVac1 => _devices.Outputs.UnloadGlassAlignVac1OnOff;
        private IDOutput AlignVac2 => _devices.Outputs.UnloadGlassAlignVac2OnOff;
        private IDOutput AlignVac3 => _devices.Outputs.UnloadGlassAlignVac3OnOff;
        private IDOutput AlignVac4 => _devices.Outputs.UnloadGlassAlignVac4OnOff;

        private IDInput GlassVac1 => _devices.Inputs.UnloadGlassAlignVac1;
        private IDInput GlassVac2 => _devices.Inputs.UnloadGlassAlignVac2;
        private IDInput GlassVac3 => _devices.Inputs.UnloadGlassAlignVac3;
        private IDInput GlassVac4 => _devices.Inputs.UnloadGlassAlignVac4;

        private IDInput GlassDetect1 => _devices.Inputs.UnloadGlassDetect1;
        private IDInput GlassDetect2 => _devices.Inputs.UnloadGlassDetect2;
        private IDInput GlassDetect3 => _devices.Inputs.UnloadGlassDetect3;
        private IDInput GlassDetect4 => _devices.Inputs.UnloadGlassDetect4;

        private bool IsGlassVac1 => _devices.Inputs.UnloadGlassAlignVac1.Value;
        private bool IsGlassVac2 => _devices.Inputs.UnloadGlassAlignVac2.Value;
        private bool IsGlassVac3 => _devices.Inputs.UnloadGlassAlignVac3.Value;
        private bool IsGlassVac4 => _devices.Inputs.UnloadGlassAlignVac4.Value;
        private bool IsGlassVac => IsGlassVac1 && IsGlassVac2 && IsGlassVac3 && IsGlassVac4;

        private bool IsGlassDetect1 => _devices.Inputs.UnloadGlassDetect1.Value;
        private bool IsGlassDetect2 => _devices.Inputs.UnloadGlassDetect2.Value;
        private bool IsGlassDetect3 => _devices.Inputs.UnloadGlassDetect3.Value;
        private bool IsGlassDetect4 => _devices.Inputs.UnloadGlassDetect4.Value;
        private bool IsGlassDetect => IsGlassDetect1 && IsGlassDetect2 && IsGlassDetect3 && IsGlassDetect4;
        #endregion

        #region Flags
        private bool FlagUnloadAlignRequestRobotUnload
        {
            set
            {
                _unloadAlignOutput[(int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD] = value;
            }
        }

        private bool FlagUnloadAlignPlaceDoneReceived
        {
            set
            {
                _unloadAlignOutput[(int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED] = value;
            }
        }
        private bool FlagRobotUnloadDoneReceived
        {
            set
            {
                _unloadAlignOutput[(int)EUnloadAlignProcessOutput.ROBOT_UNLOAD_DONE_RECEIVED] = value;
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
                return _unloadAlignInput[(int)EUnloadAlignProcessInput.ROBOT_UNLOAD_DONE];
            }
        }
        #endregion

        #region Constructor
        public UnloadAlignProcess(Devices devices,
            CommonRecipe commonRecipe,
            [FromKeyedServices("UnloadAlignInput")] IDInputDevice unloadAlignInput,
            [FromKeyedServices("UnloadAlignOutput")] IDOutputDevice unloadAlignOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
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
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsUnalign);
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
                case ESequence.UnloadRobotPlasma:
                    break;
                case ESequence.UnloadRobotPlace:
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
                    ((VirtualOutputDevice<EUnloadAlignProcessOutput>)_unloadAlignOutput).Clear();
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
                    if (IsGlassVac)
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
                    Log.Debug("Clear Flag Robot Unload Done Received");
                    FlagRobotUnloadDoneReceived = false;
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Cyl_Align_Up:
                    Log.Debug("Cylinder Align Up");
                    AlignUnalign(true);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsAlign);
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
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    VacOnOff(true);
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(GlassDetect1, true);
                    SimulationInputSetter.SetSimModbusInput(GlassDetect2, true);
                    SimulationInputSetter.SetSimModbusInput(GlassDetect3, true);
                    SimulationInputSetter.SetSimModbusInput(GlassDetect4, true);
#endif
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.GlassDetect_Check:
                    if (IsGlassDetect == false)
                    {
                        //ALARM
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadAlignStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignUnalign(false);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsAlign == false);
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
                        Parent.ProcessMode = EProcessMode.ToStop;
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
                    SimulationInputSetter.SetSimModbusInput(GlassVac1, false);
                    SimulationInputSetter.SetSimModbusInput(GlassVac2, false);
                    SimulationInputSetter.SetSimModbusInput(GlassVac3, false);
                    SimulationInputSetter.SetSimModbusInput(GlassVac4, false);
#endif
                    Wait(_commonRecipe.VacDelay);
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
                    SimulationInputSetter.SetSimModbusInput(GlassDetect1, false);
                    SimulationInputSetter.SetSimModbusInput(GlassDetect2, false);
                    SimulationInputSetter.SetSimModbusInput(GlassDetect3, false);
                    SimulationInputSetter.SetSimModbusInput(GlassDetect4, false);
#endif
                    Log.Debug("Clear Flag Request Robot Unload");
                    FlagUnloadAlignRequestRobotUnload = false;
                    Log.Debug("Set Flag Robot Unload Done Received");
                    FlagRobotUnloadDoneReceived = true;
                    Step.RunStep++;
                    break;
                case EUnloadAlignRobotPickStep.End:
                    Log.Debug("Unload Robot Pick End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
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
                    if (IsGlassVac)
                    {
                        Step.RunStep = (int)EUnloadAlignUnloadTransferPlaceStep.End;
                        break;
                    }
                    Log.Debug("Clear Flag Unload Align Place Done Received");
                    FlagUnloadAlignPlaceDoneReceived = false;
                    Step.RunStep++;
                    break;
                case EUnloadAlignUnloadTransferPlaceStep.Set_FlagUnloadAlignReady:
                    Log.Debug("Set Flag Unload Align Ready");
                    FlagUnloadAlignReady = true;
                    Log.Debug("Wait Unload Transfer Place Done");
                    Step.RunStep++;
                    break;
                case EUnloadAlignUnloadTransferPlaceStep.Wait_UnloadTransfePlaceDone:
                    if (FlagUnloadTransferLeftPlaceDone == false && FlagUnloadTransferRightPlaceDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Unload Align Ready");
                    FlagUnloadAlignReady = false;

                    Wait(200);
                    Log.Debug("Set Flag Unload Align Place Done Received");
                    FlagUnloadAlignPlaceDoneReceived = true;
                    Step.RunStep = (int)EUnloadAlignUnloadTransferPlaceStep.GlassVac_Check;
                    break;
                case EUnloadAlignUnloadTransferPlaceStep.End:
                    Log.Debug("Unload Transfer Place End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
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