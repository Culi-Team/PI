using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
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
    public class TransferFixtrueProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly TransferFixtureRecipe _transferFixtureRecipe;
        private readonly IDInputDevice _transferFixtureInput;
        private readonly IDOutputDevice _transferFixtureOutput;
        private readonly IDOutputDevice _detachOutput;
        private readonly MachineStatus _machineStatus;

        private IMotion TransferFixtureYAxis => _devices.Motions.FixtureTransferYAxis;
        private ICylinder CylUpDown => _devices.Cylinders.TransferFixture_UpDownCyl;
        private ICylinder CylClamp1_1 => _devices.Cylinders.TransferFixture_ClampCyl1;
        private ICylinder CylClamp1_2 => _devices.Cylinders.TransferFixture_ClampCyl2;
        private ICylinder CylClamp2_1 => _devices.Cylinders.TransferFixture_ClampCyl3;
        private ICylinder CylClamp2_2 => _devices.Cylinders.TransferFixture_ClampCyl4;

        private bool IsFixtureDetect1 => !CylClamp1_1.IsBackward && !CylClamp1_1.IsForward && !CylClamp1_2.IsBackward && !CylClamp1_2.IsForward;
        private bool IsFixtureDetect2 => !CylClamp2_1.IsBackward && !CylClamp2_1.IsForward && !CylClamp2_2.IsBackward && !CylClamp2_2.IsForward;

        private bool IsClamp => CylClamp1_1.IsForward && CylClamp1_2.IsForward && CylClamp2_1.IsForward && CylClamp2_2.IsForward;
        private bool IsUnClamp => CylClamp1_1.IsBackward && CylClamp1_2.IsBackward && CylClamp2_1.IsBackward && CylClamp2_2.IsBackward;

        private IDInput RobotInReady => _devices.Inputs.LoadRobInReady;
        private IDInput RobotInHome => _devices.Inputs.LoadRobInHome;
        #endregion

        private void ClampUnClamp(bool bClamp)
        {
            if (bClamp)
            {
                CylClamp1_1.Forward();
                CylClamp1_2.Forward();
                CylClamp2_1.Forward();
                CylClamp2_2.Forward();
            }
            else
            {
                CylClamp1_1.Backward();
                CylClamp1_2.Backward();
                CylClamp2_1.Backward();
                CylClamp2_2.Backward();
            }
        }

        #region Flags
        private bool FlagRemoveFilmOriginDone
        {
            get
            {
                return _transferFixtureInput[(int)ETransferFixtureProcessInput.REMOVE_FILM_ORIGIN_DONE];
            }
        }

        private bool FlagRemoveFilmReadyDone
        {
            get
            {
                return _transferFixtureInput[(int)ETransferFixtureProcessInput.REMOVE_FILM_READY_DONE];
            }
        }

        private bool FlagDetachProcessOriginDone
        {
            get
            {
                return _transferFixtureInput[(int)ETransferFixtureProcessInput.DETACH_ORIGIN_DONE];
            }
        }

        private bool FlagFixtureAlignDone
        {
            get
            {
                return _transferFixtureInput[(int)ETransferFixtureProcessInput.FIXTURE_ALIGN_DONE];
            }
        }

        private bool FlagDetachDone
        {
            get
            {
                return _transferFixtureInput[(int)ETransferFixtureProcessInput.DETACH_DONE];
            }
        }

        private bool FlagRemoveFilmDone
        {
            get
            {
                return _transferFixtureInput[(int)ETransferFixtureProcessInput.REMOVE_FILM_DONE];
            }
        }

        private bool FlagFixtureTransferDone
        {
            set
            {
                _transferFixtureOutput[(int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE] = value;
            }
        }
        #endregion

        #region Constructor
        public TransferFixtrueProcess(Devices devices,
            CommonRecipe commonRecipe,
            TransferFixtureRecipe transferFixtureRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("TransferFixtureInput")] IDInputDevice transferFixtureInput,
            [FromKeyedServices("TransferFixtureOutput")] IDOutputDevice transferFixtureOutput,
            [FromKeyedServices("DetachOutput")] IDOutputDevice detachOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _transferFixtureRecipe = transferFixtureRecipe;
            _machineStatus = machineStatus;
            _transferFixtureInput = transferFixtureInput;
            _transferFixtureOutput = transferFixtureOutput;
            _detachOutput = detachOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ETransferFixtureOriginStep)Step.OriginStep)
            {
                case ETransferFixtureOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.FixtureDetectCheck:
                    Log.Debug("Fixture Detect Check");
                    if (IsFixtureDetect1 || IsFixtureDetect2)
                    {
                        RaiseWarning((int)EWarning.TransferFixtureOriginFixtureDetect);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.Unclamp:
                    Log.Debug("Unclamp");
                    ClampUnClamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsUnClamp);
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.Unclamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Unclamp Done");
                    Step.OriginStep++;
                    Log.Debug("Wait Detach and Robot Origin");
                    break;
                case ETransferFixtureOriginStep.Wait_Detach_RemoveFilm_OriginDone:
                    if (FlagDetachProcessOriginDone == false || FlagRemoveFilmOriginDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.CylUp:
                    Log.Debug("Cylinder Up");
                    CylUpDown.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CylUpDown.IsForward);
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.CylUp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_UpDownCylinder_Up_Fail);
                        break;
                    }

                    Log.Debug("Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.YAxis_Origin:
                    Log.Debug("Fixture Transfer Y Axis Origin Start");
                    TransferFixtureYAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => TransferFixtureYAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.TransferFixture_YAxis_Origin_Fail);
                        break;
                    }
                    Log.Debug("Fixture Transfer Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.End:
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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.TransferFixture:
                    Sequence_TransferFixture();
                    break;
                default:
                    Sequence = ESequence.Stop;
                    break;
            }

            return true;
        }

        public override bool ProcessToRun()
        {
            switch ((ETransferFixtureProcessToRunStep)Step.ToRunStep)
            {
                case ETransferFixtureProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case ETransferFixtureProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<ETransferFixtureProcessOutput>)_transferFixtureOutput).ClearOutputs();
                    Step.ToRunStep++;
                    break;
                case ETransferFixtureProcessToRunStep.End:
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
        private void Sequence_AutoRun()
        {
            Log.Info("Sequence Transfer Fixture Load");
            Sequence = ESequence.TransferFixture;
        }

        private void Sequence_Ready()
        {
            switch ((ETransferFixtureProcess_ReadyStep)Step.RunStep)
            {
                case ETransferFixtureProcess_ReadyStep.Start:
                    Log.Debug("Ready Start");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.Status_Check:
                    if (CylUpDown.IsForward && CylClamp1_1.IsForward && CylClamp1_2.IsForward)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_TransHand1_MayContainsFixture);
                        break;
                    }
                    if (CylUpDown.IsForward && CylClamp2_1.IsForward && CylClamp2_2.IsForward)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_TransHand2_MayContainsFixture);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.RobotReady_Wait:
                    if (RobotInReady.Value == false && RobotInHome.Value == false)
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("Robot Safety Detect");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.Cylinder_Up:
                    if (CylUpDown.IsForward)
                    {
                        Step.RunStep = (int)ETransferFixtureProcess_ReadyStep.YAxis_ReadyMove;
                        break;
                    }

                    Log.Debug("Cylinder Up Move");
                    CylUpDown.Forward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => CylUpDown.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.Cylinder_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_UpDownCylinder_Up_Fail);
                        break;
                    }

                    Log.Debug("Cylinder Up Move Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.YAxis_ReadyMove:
                    if (TransferFixtureYAxis.IsOnPosition(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition))
                    {
                        Step.RunStep = (int)ETransferFixtureProcess_ReadyStep.Cylinder_Down;
                        break;
                    }

                    Log.Debug($"{TransferFixtureYAxis} Move Load Position");
                    TransferFixtureYAxis.MoveAbs(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000),
                        () => TransferFixtureYAxis.IsOnPosition(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition));
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.YAxis_ReadyMove_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.TransferFixture_YAxis_MoveLoadPosition_Fail);
                        break;
                    }

                    Log.Debug($"{TransferFixtureYAxis} Move Load Position Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.Cylinder_Down:
                    if (CylUpDown.IsBackward)
                    {
                        Step.RunStep = (int)ETransferFixtureProcess_ReadyStep.End;
                        break;
                    }

                    Log.Debug("Cylinder Down Move");
                    CylUpDown.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => CylUpDown.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.Cylinder_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_UpDownCylinder_Down_Fail);
                        break;
                    }

                    Log.Debug("Cylinder Down Move Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcess_ReadyStep.End:
                    Log.Debug("Ready End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_TransferFixture()
        {
            switch ((ETransferFixtureProcessLoadStep)Step.RunStep)
            {
                case ETransferFixtureProcessLoadStep.Start:
                    Log.Debug("Fixture Transfer Load Start");
                    Step.RunStep++;
                    Log.Debug("Wait Align and Detach Done");
                    break;
                case ETransferFixtureProcessLoadStep.Wait_Align_And_Detach_Done:
                    if (FlagDetachDone == false || FlagFixtureAlignDone == false)
                    {
                        //Wait Align Fixture and Detach Ready
                        Wait(20);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Check_Y_Position:
                    if (TransferFixtureYAxis.IsOnPosition(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition))
                    {
                        Step.RunStep = (int)ETransferFixtureProcessLoadStep.Cyl_Down;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Up:
                    Log.Debug("Transfer Fixture Cylinder Up");
                    CylUpDown.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CylUpDown.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_UpDownCylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition:
                    Log.Debug("Transfer Fixture Y Axis move Load Position");
                    TransferFixtureYAxis.MoveAbs(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => TransferFixtureYAxis.IsOnPosition(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition));
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.TransferFixture_YAxis_MoveLoadPosition_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Y Axis move Load Position Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Down:
                    Log.Debug("Transfer Fixture Cylinder Down");
                    CylUpDown.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CylUpDown.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_UpDownCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Clamp:
                    Log.Debug("Transfer Fixture Cylinder Clamp");
                    ClampUnClamp(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsClamp);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_ClampCylinder_Clamp_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Clamp Done");
                    Step.RunStep++;
                    break;


                case ETransferFixtureProcessLoadStep.Cyl_Up_2nd:
                    Log.Debug("Transfer Fixture Cylinder Up");
                    CylUpDown.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CylUpDown.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Up_2nd_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_UpDownCylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Up Done");
                    Step.RunStep++;
                    Log.Debug("Wait Remove Film Done");
                    break;
                case ETransferFixtureProcessLoadStep.Wait_RemoveFilm_Done:
                    if (FlagRemoveFilmDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.YAxis_Move_UnloadPosition:
                    Log.Debug("Transfer Fixture Y Axis Move Unload Position");
                    TransferFixtureYAxis.MoveAbs(_transferFixtureRecipe.TransferFixtureYAxisUnloadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => TransferFixtureYAxis.IsOnPosition(_transferFixtureRecipe.TransferFixtureYAxisUnloadPosition));
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.YAxis_Move_UnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseAlarm((int)EAlarm.TransferFixture_YAxis_MoveUnloadPosition_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Y Axis Move Unload Position Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Down_2nd:
                    Log.Debug("Transfer Fixture Cylinder Down");
                    CylUpDown.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => CylUpDown.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Down_2nd_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_UpDownCylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_UnClamp:
                    Log.Debug("Transfer Fixture Cylinder UnClamp");
                    ClampUnClamp(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsUnClamp);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.TransferFixture_ClampCylinder_UnClamp_Fail);
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.SetFlagTransferDone:
                    Log.Debug("Set Flag Transfer Done");
                    FlagFixtureTransferDone = true;
                    Log.Debug("Wait Align , Detach , Remove Film Receive Transfer Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.WaitProcesses_ReceiveTransferDone:
                    if (FlagFixtureAlignDone == true ||
                       FlagDetachDone == true ||
                       FlagRemoveFilmDone == true)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Transfer Done");
                    FlagFixtureTransferDone = false;
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    Log.Info("Sequence Transfer Fixture Load");
                    Sequence = ESequence.TransferFixture;
                    break;
            }
        }
        #endregion
    }
}
