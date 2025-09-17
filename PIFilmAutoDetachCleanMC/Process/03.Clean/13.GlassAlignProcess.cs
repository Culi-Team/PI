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

        private IDOutput AlignStageVac1 => port == EPort.Left ? _devices.Outputs.AlignStageLVac1OnOff : _devices.Outputs.AlignStageRVac1OnOff;
        private IDOutput AlignStageVac2 => port == EPort.Left ? _devices.Outputs.AlignStageLVac2OnOff : _devices.Outputs.AlignStageRVac2OnOff;
        private IDOutput AlignStageVac3 => port == EPort.Left ? _devices.Outputs.AlignStageLVac3OnOff : _devices.Outputs.AlignStageRVac3OnOff;

        private bool IsVacDetect1 => port == EPort.Left ? _devices.Inputs.AlignStageLVac1.Value : _devices.Inputs.AlignStageRVac1.Value;
        private bool IsVacDetect2 => port == EPort.Left ? _devices.Inputs.AlignStageLVac2.Value : _devices.Inputs.AlignStageRVac2.Value;
        private bool IsVacDetect3 => port == EPort.Left ? _devices.Inputs.AlignStageLVac3.Value : _devices.Inputs.AlignStageRVac3.Value;
        private bool IsVacDetect => IsVacDetect1 || IsVacDetect2 || IsVacDetect3;

        private ICylinder AlignCyl1 => port == EPort.Left ? _devices.Cylinders.AlignStageL1AlignUnalign : _devices.Cylinders.AlignStageR1AlignUnalign;
        private ICylinder AlignCyl2 => port == EPort.Left ? _devices.Cylinders.AlignStageL2AlignUnalign : _devices.Cylinders.AlignStageR2AlignUnalign;
        private ICylinder AlignCyl3 => port == EPort.Left ? _devices.Cylinders.AlignStageL3AlignUnalign : _devices.Cylinders.AlignStageR3AlignUnalign;

        private bool IsGlass1Detect => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool IsGlass2Detect => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect2.Value : _devices.Inputs.AlignStageRGlassDetect2.Value;
        private bool IsGlass3Detect => port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect3.Value : _devices.Inputs.AlignStageRGlassDetect3.Value;
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
                if(port == EPort.Left)
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
                if(port == EPort.Left)
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
                if( port == EPort.Left)
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
            [FromKeyedServices("GlassAlignLeftInput")] IDInputDevice glassAlignLeftInput,
            [FromKeyedServices("GlassAlignLeftOutput")] IDOutputDevice glassAlignLeftOutput,
            [FromKeyedServices("GlassAlignRightInput")] IDInputDevice glassAlignRightInput,
            [FromKeyedServices("GlassAlignRightOutput")] IDOutputDevice glassAlignRightOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
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
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsUnalign);
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
                    Sequence_GlassTransferPlace();
                    break;
                case ESequence.AlignGlass:
                    Sequence_AlignGlass();
                    break;
                case ESequence.TransferInShuttlePick:
                    Sequence_TransferInShuttlePick();
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

        public override bool ProcessToRun()
        {
            switch ((EGlassAlignProcessToRunStep)Step.ToRunStep)
            {
                case EGlassAlignProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case EGlassAlignProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<EGlassAlignProcessOutput>)_glassAlignRightOutput).Clear();
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
                    if(IsVacDetect || IsGlassDetect)
                    {
                        Log.Info("Sequence Align Glass");
                        Sequence = ESequence.AlignGlass;
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
                    Wait(_commonRecipe.CylinderMoveTimeout,() => IsUnalign);
                    Step.RunStep++;
                    break;
                case EGlassAlignGlassTransferPlaceStep.Cyl_Align_Down_Wait:
                    if(WaitTimeOutOccurred)
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
                    if(FlagGlassTransferPlaceDone == false)
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
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Align Glass");
                    Sequence = ESequence.AlignGlass;
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
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac1 : _devices.Inputs.AlignStageRVac1, true);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac2 : _devices.Inputs.AlignStageRVac2, true);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac3 : _devices.Inputs.AlignStageRVac3, true);
#endif
                    Wait(_commonRecipe.VacDelay, () => IsVacDetect);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Up:
                    Log.Debug("Cylinder Align Up");
                    AlignUnAlign(true);
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac1 : _devices.Inputs.AlignStageRVac1, false);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac2 : _devices.Inputs.AlignStageRVac2, false);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLVac3 : _devices.Inputs.AlignStageRVac3, false);
#endif
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsAlign);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Up_Wait:
                    if(WaitTimeOutOccurred)
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
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect1 : _devices.Inputs.AlignStageRGlassDetect1, true);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect2 : _devices.Inputs.AlignStageRGlassDetect2, true);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect3 : _devices.Inputs.AlignStageRGlassDetect3, true);
#endif
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Wait_GlassDetect:
                    if (IsGlassDetect == false)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_GlassNotDetect : EWarning.GlassAlignRight_GlassNotDetect));
                        break;
                    }
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect1 : _devices.Inputs.AlignStageRGlassDetect1, false);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect2 : _devices.Inputs.AlignStageRGlassDetect2, false);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect3 : _devices.Inputs.AlignStageRGlassDetect3, false);
#endif
                    Log.Debug("Glass Align Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Vacuum_On_2nd:
                    Log.Debug("Vacuum On");
                    VacOnOff(true);
                    Wait(_commonRecipe.VacDelay, () => IsVacDetect);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Down:
                    Log.Debug("Cylinder Align Down");
                    AlignUnAlign(false);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => IsUnalign);
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.Cyl_Align_Down_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.GlassAlignLeft_AlignCylinder_Down_Fail : EWarning.GlassAlignRight_AlignCylinder_Down_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Align Down Done");
                    Step.RunStep++;
                    break;
                case EGlassAlignStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer In Shuttle Pick");
                    Sequence = ESequence.TransferInShuttlePick;
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
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect1 : _devices.Inputs.AlignStageRGlassDetect1, true);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect2 : _devices.Inputs.AlignStageRGlassDetect2, true);
                    SimulationInputSetter.SetSimModbusInput(port == EPort.Left ? _devices.Inputs.AlignStageLGlassDettect3 : _devices.Inputs.AlignStageRGlassDetect3, true);
#endif
                    break;
                case EGlassAlignTransferInShuttlePickStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    VacOnOff(false);
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case EGlassAlignTransferInShuttlePickStep.Glass_Detect_Check:
                    Log.Debug("Glass Detect Check");

                    Log.Debug("Clear Flag Glass Align Pick Done Received");
                    FlagGlassAlignPickDoneReceived = false;

                    if (IsGlassDetect)
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
                    if(FlagTransferInShuttlePickDone == false)
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
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
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