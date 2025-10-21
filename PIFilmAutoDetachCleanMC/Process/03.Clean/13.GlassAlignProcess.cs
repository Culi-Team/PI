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
    public class GlassAlignProcess : ProcessBase<ESequence>
    {
        #region Privates
        private EPort port => Name == EProcess.GlassAlignLeft.ToString() ? EPort.Left : EPort.Right;
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _glassAlignLeftInput;
        private readonly IDOutputDevice _glassAlignLeftOutput;
        private readonly IDInputDevice _glassAlignRightInput;
        private readonly IDOutputDevice _glassAlignRightOutput;
        private readonly MachineStatus _machineStatus;

        private IDOutput AlignStageVac1 => port == EPort.Left ? _devices.Outputs.AlignStageLVac1OnOff : _devices.Outputs.AlignStageRVac1OnOff;
        private IDOutput AlignStageVac2 => port == EPort.Left ? _devices.Outputs.AlignStageLVac2OnOff : _devices.Outputs.AlignStageRVac2OnOff;
        private IDOutput AlignStageVac3 => port == EPort.Left ? _devices.Outputs.AlignStageLVac3OnOff : _devices.Outputs.AlignStageRVac3OnOff;
        private IDOutput AlignStageBlow1 => port == EPort.Left ? _devices.Outputs.AlignStageLBlow1OnOff : _devices.Outputs.AlignStageRBlow1OnOff;
        private IDOutput AlignStageBlow2 => port == EPort.Left ? _devices.Outputs.AlignStageLBlow2OnOff : _devices.Outputs.AlignStageRBlow2OnOff;
        private IDOutput AlignStageBlow3 => port == EPort.Left ? _devices.Outputs.AlignStageLBlow3OnOff : _devices.Outputs.AlignStageRBlow3OnOff;

        private bool AlignStageVac1Sensor => port == EPort.Left ? _devices.Inputs.AlignStageLVac1.Value : _devices.Inputs.AlignStageRVac1.Value;
        private bool AlignStageVac2Sensor => port == EPort.Left ? _devices.Inputs.AlignStageLVac2.Value : _devices.Inputs.AlignStageRVac2.Value;
        private bool AlignStageVac3Sensor => port == EPort.Left ? _devices.Inputs.AlignStageLVac3.Value : _devices.Inputs.AlignStageRVac3.Value;

        private bool IsVacDetect1 => AlignStageVac1Sensor == true;
        private bool IsVacDetect2 => AlignStageVac2Sensor == true;
        private bool IsVacDetect3 => AlignStageVac3Sensor == true;
        private bool IsVacDetect => IsVacDetect1 || IsVacDetect2 || IsVacDetect3;

        private ICylinder AlignCyl1 => port == EPort.Left ? _devices.Cylinders.AlignStageL_AlignCyl1 : _devices.Cylinders.AlignStageR_AlignCyl1;
        private ICylinder AlignCyl2 => port == EPort.Left ? _devices.Cylinders.AlignStageL_AlignCyl2 : _devices.Cylinders.AlignStageR_AlignCyl2;
        private ICylinder AlignCyl3 => port == EPort.Left ? _devices.Cylinders.AlignStageL_AlignCyl3 : _devices.Cylinders.AlignStageR_AlignCyl3;

        private ICylinder BrushCyl => port == EPort.Left ? _devices.Cylinders.AlignStageL_BrushCyl : _devices.Cylinders.AlignStageR_BrushCyl;

        private bool IsGlass1Detect => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool IsGlass2Detect => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect2.Value : _devices.Inputs.AlignStageRGlassDetect2.Value;
        private bool IsGlass3Detect => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect3.Value : _devices.Inputs.AlignStageRGlassDetect3.Value;
        private bool IsGlassDetect => IsGlass1Detect || IsGlass2Detect || IsGlass3Detect;

        private bool IsAlign => AlignCyl1.IsForward && AlignCyl2.IsForward && AlignCyl3.IsForward;
        private bool IsUnalign => AlignCyl1.IsBackward && AlignCyl2.IsBackward && AlignCyl3.IsBackward;
        #endregion

        #region Flags
        private bool FlagGlassAlignRequestGlass
        {
            set
            {
                if (port == EPort.Left)
                {
                    _glassAlignLeftOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS] = value;
                }
                else
                {
                    _glassAlignRightOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS] = value;
                }
            }
        }

        private bool FlagGlassAlignRequestPick
        {
            set
            {
                if (port == EPort.Left)
                {
                    _glassAlignLeftOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK] = value;
                }
                else
                {
                    _glassAlignRightOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK] = value;
                }
            }
        }

        private bool FlagGlassAlignPickDoneReceived
        {
            set
            {
                if (port == EPort.Left)
                {
                    _glassAlignLeftOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED] = value;
                }
                else
                {
                    _glassAlignRightOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED] = value;
                }
            }
        }
        private bool FlagGlassTransferPlaceDone
        {
            get
            {
                if (port == EPort.Left)
                {
                    return _glassAlignLeftInput[(int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE];
                }

                return _glassAlignRightInput[(int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE];
            }
        }

        private bool FlagTransferInShuttlePickDone
        {
            get
            {
                if (port == EPort.Left)
                {
                    return _glassAlignLeftInput[(int)EGlassAlignProcessInput.TRANSFER_IN_SHUTTLE_PICK_DONE];
                }

                return _glassAlignRightInput[(int)EGlassAlignProcessInput.TRANSFER_IN_SHUTTLE_PICK_DONE];
            }
        }

        private bool FlagGlassAlignPlaceDoneReceived
        {
            set
            {
                if (port == EPort.Left)
                {
                    _glassAlignLeftOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED] = value;
                }
                else
                {
                    _glassAlignRightOutput[(int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED] = value;
                }
            }
        }
        #endregion

        #region Constructor
        public GlassAlignProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("GlassAlignLeftInput")] IDInputDevice glassAlignLeftInput,
            [FromKeyedServices("GlassAlignLeftOutput")] IDOutputDevice glassAlignLeftOutput,
            [FromKeyedServices("GlassAlignRightInput")] IDInputDevice glassAlignRightInput,
            [FromKeyedServices("GlassAlignRightOutput")] IDOutputDevice glassAlignRightOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _glassAlignLeftInput = glassAlignLeftInput;
            _glassAlignLeftOutput = glassAlignLeftOutput;
            _glassAlignRightInput = glassAlignRightInput;
            _glassAlignRightOutput = glassAlignRightOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EGlassAlignOriginStep)Step.OriginStep)
            {
                case EGlassAlignOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EGlassAlignOriginStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignUnAlign(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsUnalign);
                    Step.OriginStep++;
                    break;
                case EGlassAlignOriginStep.Cyl_Align_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_AlignCylinder_Down_Fail :
                                                          EWarning.GlassAlignRight_AlignCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Align Down Done");
                    Step.OriginStep++;
                    break;
                case EGlassAlignOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
                default:
                    Wait(20);
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
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.InWorkCSTTilt:
                    break;
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
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
                    Sequence_GlassTransferPlace();
                    break;
                case ESequence.AlignGlassLeft:
                    if (port == EPort.Left)
                    {
                        Sequence_AlignGlass();
                    }
                    break;
                case ESequence.AlignGlassRight:
                    if (port == EPort.Right)
                    {
                        Sequence_AlignGlass();
                    }
                    break;
                case ESequence.TransferInShuttleLeftPick:
                    if (port == EPort.Left)
                    {
                        Sequence_TransferInShuttlePick();
                    }
                    break;
                case ESequence.TransferInShuttleRightPick:
                    if (port == EPort.Right)
                    {
                        Sequence_TransferInShuttlePick();
                    }
                    break;
                case ESequence.WETCleanLeftLoad:
                    break;
                case ESequence.WETCleanRightLoad:
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanRight:
                    break;
                case ESequence.WETCleanLeftUnload:
                    break;
                case ESequence.WETCleanRightUnload:
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.TransferRotationRight:
                    break;
                case ESequence.AFCleanLeftLoad:
                    break;
                case ESequence.AFCleanRightLoad:
                    break;
                case ESequence.AFCleanLeft:
                    break;
                case ESequence.AFCleanRight:
                    break;
                case ESequence.AFCleanLeftUnload:
                    break;
                case ESequence.AFCleanRightUnload:
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    break;
                case ESequence.UnloadTransferRightPlace:
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
            switch ((EGlassAlignProcessToRunStep)Step.ToRunStep)
            {
                case EGlassAlignProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EGlassAlignProcessToRunStep.BrushCyl_Up:
                    BrushCyl.Forward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => BrushCyl.IsForward);
                    Step.ToRunStep++;
                    break;
                case EGlassAlignProcessToRunStep.Brush_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_BrushCylinder_Up_Fail :
                                                        EWarning.GlassAlignRight_BrushCylinder_Up_Fail));
                        break;
                    }
                    Step.ToRunStep++;
                    break;
                case EGlassAlignProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    if (port == EPort.Left)
                    {
                        ((MappableOutputDevice<EGlassAlignProcessOutput>)_glassAlignLeftOutput).ClearOutputs();
                    }
                    else
                    {
                        ((MappableOutputDevice<EGlassAlignProcessOutput>)_glassAlignRightOutput).ClearOutputs();
                    }
                    Step.ToRunStep++;
                    break;
                case EGlassAlignProcessToRunStep.End:
                    Log.Debug("To Run End");
                    Step.ToRunStep++;
                    ProcessStatus = EProcessStatus.ToRunDone;
                    break;
                default:
                    Thread.Sleep(10);
                    break;
            }
            return true;
        }
        #endregion

        #region Private Methods
        private void AlignUnAlign(bool bAlignUnalign)
        {
            if (bAlignUnalign)
            {
                AlignCyl1.Forward();
                AlignCyl2.Forward();
                AlignCyl3.Forward();
            }
            else
            {
                AlignCyl1.Backward();
                AlignCyl2.Backward();
                AlignCyl3.Backward();
            }
        }

        private void VacOnOff(bool bOnOff)
        {
            AlignStageVac1.Value = bOnOff;
            AlignStageVac2.Value = bOnOff;
            AlignStageVac3.Value = bOnOff;

            AlignStageBlow1.Value = !bOnOff;
            AlignStageBlow2.Value = !bOnOff;
            AlignStageBlow3.Value = !bOnOff;
            if(bOnOff == false)
            {
                Task.Delay(100).ContinueWith(t =>
                {
                    AlignStageBlow1.Value = false;
                    AlignStageBlow2.Value = false;
                    AlignStageBlow3.Value = false;
                });
            }    
#if SIMULATION
            SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac1 : _devices.Inputs.AlignStageRVac1, bOnOff);
            SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac2 : _devices.Inputs.AlignStageRVac2, bOnOff);
            SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac3 : _devices.Inputs.AlignStageRVac3, bOnOff);
#endif
        }

        private void Sequence_AutoRun()
        {
            switch ((EGlassAlignAutoRunStep)Step.RunStep)
            {
                case EGlassAlignAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EGlassAlignAutoRunStep.GlassVacDetect_Check:
                    if ((IsVacDetect || IsGlassDetect) && _machineStatus.IsDryRunMode == false)
                    {
                        Log.Info("Sequence Align Glass");
                        Sequence = port == EPort.Left ? ESequence.AlignGlassLeft : ESequence.AlignGlassRight;
                    }
                    Step.RunStep++;
                    break;
                case EGlassAlignAutoRunStep.End:
                    Log.Info("Sequence Glass Transfer Place");
                    Sequence = ESequence.GlassTransferPlace;
                    break;
            }
        }

        private void Sequence_GlassTransferPlace()
        {
            switch ((EGlassAlignGlassTransferPlaceStep)Step.RunStep)
            {
                case EGlassAlignGlassTransferPlaceStep.Start:
                    Log.Debug("Glass Transfer Place Start");
                    Log.Debug("Clear Flag Glass Align Place Done Received");
                    FlagGlassAlignPlaceDoneReceived = false;
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignUnAlign(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsUnalign);
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.Cyl_Align_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_AlignCylinder_Down_Fail :
                                                          EWarning.GlassAlignRight_AlignCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Align Down Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.Set_FlagRequestGlass:
                    Log.Debug("Set Flag Request Glass");
                    FlagGlassAlignRequestGlass = true;
                    Log.Debug("Wait Glass Transfer Place Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.Wait_GlassTransferPlace_Done:
                    if (FlagGlassTransferPlaceDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Log.Debug("Clear Flag Request Glass");
                    FlagGlassAlignRequestGlass = false;

                    Log.Debug("Set Flag Glass Align Place Done Received");
                    FlagGlassAlignPlaceDoneReceived = true;
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Align Glass");
                    Sequence = port == EPort.Left ? ESequence.AlignGlassLeft : ESequence.AlignGlassRight;
                    break;
            }
        }

        private void Sequence_AlignGlass()
        {
            switch ((EGlassAlignStep)Step.RunStep)
            {
                case EGlassAlignStep.Start:
                    Log.Debug("Align Glass Start");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_1st:
                    Log.Debug("Vacuum On");
                    VacOnOff(true);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac1 : _devices.Inputs.AlignStageRVac1, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac2 : _devices.Inputs.AlignStageRVac2, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac3 : _devices.Inputs.AlignStageRVac3, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_1st_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_Vacuum_Fail
                                                                : EWarning.GlassAlignRight_Vacuum_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Up:
                    Log.Debug("Cylinder Align Up");
                    AlignUnAlign(true);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac1 : _devices.Inputs.AlignStageRVac1, false);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac2 : _devices.Inputs.AlignStageRVac2, false);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac3 : _devices.Inputs.AlignStageRVac3, false);
#endif
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsAlign);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_AlignCylinder_Up_Fail : EWarning.GlassAlignRight_AlignCylinder_Up_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Align Up Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    VacOnOff(false);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect1 : _devices.Inputs.AlignStageRGlassDetect1, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect2 : _devices.Inputs.AlignStageRGlassDetect2, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect3 : _devices.Inputs.AlignStageRGlassDetect3, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Wait_GlassDetect:
                    if (IsGlassDetect == false && _machineStatus.IsDryRunMode == false)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_GlassNotDetect : EWarning.GlassAlignRight_GlassNotDetect));
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect1 : _devices.Inputs.AlignStageRGlassDetect1, false);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect2 : _devices.Inputs.AlignStageRGlassDetect2, false);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect3 : _devices.Inputs.AlignStageRGlassDetect3, false);
#endif
                    Log.Debug("Glass Align Done");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_2nd:
                    Log.Debug("Vacuum On");
                    VacOnOff(true);
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_2nd_Wait:
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignUnAlign(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsUnalign);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_AlignCylinder_Down_Fail : EWarning.GlassAlignRight_AlignCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Align Down Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer In Shuttle Pick");
                    Sequence = port == EPort.Left ? ESequence.TransferInShuttleLeftPick : ESequence.TransferInShuttleRightPick;
                    break;
            }
        }

        private void Sequence_TransferInShuttlePick()
        {
            switch ((EGlassAlignTransferInShuttlePickStep)Step.RunStep)
            {
                case EGlassAlignTransferInShuttlePickStep.Start:
                    Log.Debug("Transfer In Shuttle Pick Start");
                    Step.RunStep++;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect1 : _devices.Inputs.AlignStageRGlassDetect1, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect2 : _devices.Inputs.AlignStageRGlassDetect2, true);
                    SimulationInputSetter.SetSimInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDetect3 : _devices.Inputs.AlignStageRGlassDetect3, true);
#endif
                    break;
                case EGlassAlignTransferInShuttlePickStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    VacOnOff(false);
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case EGlassAlignTransferInShuttlePickStep.Glass_Detect_Check:
                    Log.Debug("Glass Detect Check");

                    Log.Debug("Clear Flag Glass Align Pick Done Received");
                    FlagGlassAlignPickDoneReceived = false;

                    if (IsGlassDetect || _machineStatus.IsDryRunMode)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Step.RunStep = (int)EGlassAlignTransferInShuttlePickStep.End;
                    break;
                case EGlassAlignTransferInShuttlePickStep.Set_FlagGlassAlignRequestPick:
                    Log.Debug("Set Flag Glass Align Request Pick");
                    FlagGlassAlignRequestPick = true;
                    Step.RunStep++;
                    Log.Debug("Wait Transfer In Shuttle Pick Done");
                    break;
                case EGlassAlignTransferInShuttlePickStep.Wait_TransferInShuttlePickDone:
                    if (FlagTransferInShuttlePickDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear Flag Glass Align Request Pick");
                    FlagGlassAlignRequestPick = false;

                    Log.Debug("Set Flag Glass Align Pick Done Received");
                    FlagGlassAlignPickDoneReceived = true;
                    Step.RunStep = (int)EGlassAlignTransferInShuttlePickStep.Glass_Detect_Check;
                    break;
                case EGlassAlignTransferInShuttlePickStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent!.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Glass Transfer Place");
                    Sequence = ESequence.GlassTransferPlace;
                    break;
            }
        }
        #endregion
    }
}