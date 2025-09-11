using EQX.Core.InOut;
using EQX.Core.Sequence;
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
    public class RemoveFilmProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _removeFilmInput;
        private readonly IDOutputDevice _removeFilmOutput;

        private ICylinder FixCyl1 => _devices.Cylinders.RemoveZoneFixCyl1FwBw;
        private ICylinder FixCyl2 => _devices.Cylinders.RemoveZoneFixCyl2FwBw;

        private ICylinder TransferCyl => _devices.Cylinders.RemoveZoneTrCylFwBw;
        private ICylinder UpDownCyl1 => _devices.Cylinders.RemoveZoneZCyl1UpDown;
        private ICylinder UpDownCyl2 => _devices.Cylinders.RemoveZoneZCyl2UpDown;
        private ICylinder ClampCyl => _devices.Cylinders.RemoveZoneCylClampUnclamp;

        private ICylinder PusherCyl1 => _devices.Cylinders.RemoveZonePusherCyl1UpDown;
        private ICylinder PusherCyl2 => _devices.Cylinders.RemoveZonePusherCyl2UpDown;
        #endregion

        #region Contructor
        public RemoveFilmProcess(Devices devices,
            CommonRecipe commonRecipe,
            [FromKeyedServices("RemoveFilmInput")] IDInputDevice removeFilmInput,
            [FromKeyedServices("RemoveFilmOutput")] IDOutputDevice removeFilmOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _removeFilmInput = removeFilmInput;
            _removeFilmOutput = removeFilmOutput;
        }
        #endregion

        #region Flags
        private bool FlagFixtureTransferDone
        {
            get
            {
                return _removeFilmInput[(int)ERemoveFilmProcessInput.FIXTURE_TRANSFER_DONE];
            }
        }

        private bool FlagTransferFixtureDoneReceived
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED] = value;
            }
        }
        private bool FlagRemoveFilmDone
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_DONE] = value;
            }
        }

        private bool FlagRemoveFilmRequestUnload
        {
            set
            {
                _removeFilmOutput[(int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD] = value;
            }
        }

        private bool FlagRemoveFilmUnloadDone
        {
            get
            {
                return _removeFilmInput[(int)ERemoveFilmProcessInput.REMOVE_FILM_UNLOAD_DONE];
            }
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ERemoveFilmProcessOriginStep)Step.OriginStep)
            {
                case ERemoveFilmProcessOriginStep.Start:
                    Log.Debug("Remove Film Process Start");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Fix_Cyl_Backward:
                    Log.Debug("Remove Film Process Fix Cylinder Backward");
                    FixCyl1.Backward();
                    FixCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCyl1.IsBackward && FixCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Fix_Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Fix Cylinder Backward Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Pusher_Cyl_Down:
                    Log.Debug("Remove Film Process Pusher Cylinder Down");
                    PusherCyl1.Backward();
                    PusherCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PusherCyl1.IsBackward && PusherCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Pusher_Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Pusher Cylinder Down Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Up:
                    Log.Debug("Remove Film Process Cylinder Up");
                    UpDownCyl1.Backward();
                    UpDownCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl1.IsBackward && UpDownCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_UnClamp:
                    Log.Debug("Remove Film Process Cylinder UnClamp");
                    ClampCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder UnClamp Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Transfer_Backward:
                    Log.Debug("Remove Film Process Cylinder Transfer Backward");
                    TransferCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TransferCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Transfer_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder Transfer Backward Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.End:
                    Log.Debug("Remove Film Process Origin End");
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
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    break;
                case ESequence.RobotPlaceFixtureToAlign:
                    break;
                case ESequence.FixtureAlign:
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    Sequence_RobotPickFromRemoveZone();
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    break;
                case ESequence.TransferFixtureLoad:
                    break;
                case ESequence.Detach:
                    break;
                case ESequence.TransferFixtureUnload:
                    Sequence_TransferFixtureUnload();
                    break;
                case ESequence.DetachUnload:
                    break;
                case ESequence.RemoveFilm:
                    Sequence_RemoveFilm();
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
                case ESequence.UnloadTransferPick:
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
        private void Sequence_AutoRun()
        {
            Log.Info("SequenceTransfer Fixture Unload");
            Sequence = ESequence.TransferFixtureUnload;
        }

        private void Sequence_TransferFixtureUnload()
        {
            switch ((ERemoveFilmProcessTransferFixtureUnloadStep)Step.RunStep)
            {
                case ERemoveFilmProcessTransferFixtureUnloadStep.Start:
                    Log.Debug("Remove Film Process Transfer Fixture Unload Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.Set_Flag_RemoveFilmDone:
                    Log.Debug("Set Flag Remove Film Done");
                    FlagRemoveFilmDone = true;
                    Step.RunStep++;
                    Log.Debug("Wait Transfer Fixture Done");
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.Wait_TransferFixtureDone:
                    if (FlagFixtureTransferDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Clear Flag Remove Film Done");
                    FlagRemoveFilmDone = false;

                    Log.Debug("Set Flag Transfer Fixture Done Received");
                    FlagTransferFixtureDoneReceived = true;

                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessTransferFixtureUnloadStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Remove Film");
                    Sequence = ESequence.RemoveFilm;
                    break;
            }
        }

        private void Sequence_RemoveFilm()
        {
            switch ((ERemoveFilmProcessRemoveStep)Step.RunStep)
            {
                case ERemoveFilmProcessRemoveStep.Start:
                    Log.Debug("Remove Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Align_Fixture:
                    Log.Debug("Align Fixture");
                    FixCyl1.Forward();
                    FixCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCyl1.IsForward && FixCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Align_Fixture_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Align Fixture Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Forward:
                    Log.Debug("Cylinder Transfer Forward");
                    TransferCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TransferCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Forward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Transfer Forward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_1_Up:
                    Log.Debug("Pusher Cylinder 1 Up");
                    PusherCyl1.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PusherCyl1.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Pusher Cylinder 1 Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Down:
                    Log.Debug("Cylinder UpDown1 Down");
                    UpDownCyl1.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl1.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Down Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Clamp:
                    Log.Debug("Cylinder Clamp");
                    ClampCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_2_Up:
                    Log.Debug("Pusher Cylinder 2 Up");
                    PusherCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PusherCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Pusher_Cyl_2_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Pusher Cylinder 2 Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Up:
                    Log.Debug("Cylinder UpDown1 Up");
                    UpDownCyl1.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl1.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_UpDown1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Up Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Backward:
                    Log.Debug("Cylinder Transfer Backward");
                    TransferCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TransferCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Cyl_Transfer_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Transfer Backward Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.Set_Flag_RemoveFilmRequestUnload:
                    Log.Debug("Set Flag Remove Film Request Unload");
                    FlagRemoveFilmRequestUnload = true;
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRemoveStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Robot Pick From Remove Zone");
                    Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                    break;
            }
        }

        private void Sequence_RobotPickFromRemoveZone()
        {
            switch ((ERemoveFilmProcessRobotPickStep)Step.RunStep)
            {
                case ERemoveFilmProcessRobotPickStep.Start:
                    Log.Debug("Robot Pick From Remove Zone Start");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown1_Down:
                    Log.Debug("Cylinder UpDown1 Down");
                    UpDownCyl1.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl1.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown1_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Down Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    ClampCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Down_1st:
                    Log.Debug("Cylinder UpDown2 Down 1st");
                    UpDownCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Down_1st_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Down 1st Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Up_1st:
                    Log.Debug("Cylinder UpDown2 Up 1st");
                    UpDownCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Up_1st_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Up 1st Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Down_2nd:
                    Log.Debug("Cylinder UpDown2 Down 2nd");
                    UpDownCyl2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Down_2nd_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Down 2nd Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Up_2nd:
                    Log.Debug("Cylinder UpDown2 Up 2nd");
                    UpDownCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown2_Up_2nd_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown2 Up 2nd Done");
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown1_Up:
                    Log.Debug("Cylinder UpDown1 Up");
                    UpDownCyl1.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl1.IsBackward);
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.Cyl_UpDown1_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UpDown1 Up Done");
                    Step.RunStep++;
                    Log.Debug("Wait Remove Film Unload Done");
                    break;
                case ERemoveFilmProcessRobotPickStep.Wait_RemoveFilmUnloadDone:
                    if(FlagRemoveFilmUnloadDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERemoveFilmProcessRobotPickStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer Fixture Unload");
                    Sequence = ESequence.TransferFixtureUnload;
                    break;
            }
        }
        #endregion
    }
}
