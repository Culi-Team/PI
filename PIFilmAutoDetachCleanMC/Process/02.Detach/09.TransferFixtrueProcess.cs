using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Process;
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
        private readonly VirtualIO<EFlags> _virtualIO;
        private readonly TransferFixtureRecipe _transferFixtureRecipe;

        private IMotion TransferFixtureYAxis => _devices.MotionsInovance.FixtureTransferYAxis;
        private Inputs Inputs => _devices.Inputs;
        private Outputs Outputs => _devices.Outputs;
        private ICylinder CylUpDown => _devices.Cylinders.TransferFixtureUpDown;
        private ICylinder CylClamp1 => _devices.Cylinders.TransferFixture1ClampUnclamp;
        private ICylinder CylClamp2 => _devices.Cylinders.TransferFixture2ClampUnclamp;

        private bool IsFixtureDetect1 => !CylClamp1.IsBackward && !CylClamp1.IsForward;
        private bool IsFixtureDetect2 => !CylClamp2.IsBackward && !CylClamp2.IsForward;
        #endregion

        #region Flags
        private bool FlagDetachProcessOriginDone
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.DetachProcessOriginDone);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.DetachProcessOriginDone, value);
            }
        }

        private bool FlagFixtureAlignDone
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.FixtureAlignDone);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.FixtureAlignDone, value);
            }
        }

        private bool FlagDetachDone
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.DetachDone);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.DetachDone, value);
            }
        }

        private bool FlagFixtureTransferAlignDone
        {
            set
            {
                _virtualIO.SetFlag(EFlags.FixtureTransferAlignDone, value);
            }
        }

        private bool FlagFixtureTransferDetachDone
        {
            set
            {
                _virtualIO.SetFlag(EFlags.FixtureTransferDetachDone, value);
            }
        }

        private bool FlagFixtureTransferRemoveFilmDone
        {
            set
            {
                _virtualIO.SetFlag(EFlags.FixtureTransferRemoveFilmDone, value);
            }
        }

        private bool FlagRemoveFilmDone
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.RemoveFilmDone);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.RemoveFilmDone,value);
            }
        }
        #endregion
        public TransferFixtrueProcess(Devices devices,
            CommonRecipe commonRecipe,
            VirtualIO<EFlags> virtualIO,
            TransferFixtureRecipe transferFixtureRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _virtualIO = virtualIO;
            _transferFixtureRecipe = transferFixtureRecipe;
        }

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
                    CylClamp1.Backward();
                    CylClamp2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return CylClamp1.IsBackward && CylClamp2.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.Unclamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Unclamp Done");

                    Step.OriginStep++;
                    Log.Debug("Wait Detach Origin");
                    break;
                case ETransferFixtureOriginStep.Wait_Detach_Origin:
                    if (!FlagDetachProcessOriginDone)
                    {
                        //Wait Detach Process Origin Done
                        break;
                    }
                    FlagDetachProcessOriginDone = false;

                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.CylUp:
                    Log.Debug("Cylinder Up");
                    CylUpDown.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return CylUpDown.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.CylUp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Up Done");

                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.YAxis_Origin:
                    Log.Debug("Fixture Transfer Y Axis Origin Start");
                    TransferFixtureYAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return TransferFixtureYAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Fixture Transfer Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.End:
                    Log.Debug("Origin End");
                    Step.OriginStep++;
                    ProcessStatus = EProcessStatus.OriginDone;
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
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    break;
                case ESequence.TransferFixtureLoad:
                    Sequence_TransferFixtureLoad();
                    break;
                case ESequence.Detach:
                    break;
                case ESequence.TransferFixtureUnload:
                    Sequence_TransferFixtureUnload();
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
                case ESequence.TransferInShuttlePlace:
                    break;
                case ESequence.WETCleanLoad:
                    break;
                case ESequence.WETClean:
                    break;
                case ESequence.WETCleanUnload:
                    break;
                case ESequence.TransferRotationPick:
                    break;
                case ESequence.TransferRotationPlace:
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
            Log.Info("Sequence Transfer Fixture Load");
            Sequence = ESequence.TransferFixtureLoad;
        }

        private void Sequence_TransferFixtureLoad()
        {
            switch ((ETransferFixtureProcessLoadStep)Step.RunStep)
            {
                case ETransferFixtureProcessLoadStep.Start:
                    Log.Debug("Fixture Transfer Load Start");
                    Step.RunStep++;
                    Log.Debug("Wait Align and Detach Done");
                    break;
                case ETransferFixtureProcessLoadStep.Wait_Align_And_Detach_Done:
                    if (!FlagDetachDone || !FlagFixtureAlignDone)
                    {
                        //Wait Align Fixture and Detach Ready
                        break;
                    }
                    FlagDetachDone = false;
                    FlagFixtureAlignDone = false;
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
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return CylUpDown.IsForward; });
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition:
                    Log.Debug("Transfer Fixture Y Axis move Load Position");
                    TransferFixtureYAxis.MoveAbs(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => { return TransferFixtureYAxis.IsOnPosition(_transferFixtureRecipe.TransferFixtureYAxisLoadPosition); });
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Y Axis move Load Position Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Down:
                    Log.Debug("Transfer Fixture Cylinder Up");
                    CylUpDown.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return CylUpDown.IsBackward; });
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Clamp:
                    Log.Debug("Transfer Fixture Cylinder Clamp");
                    CylClamp1.Forward();
                    CylClamp2.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return CylClamp1.IsForward && CylClamp2.IsForward; });
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessLoadStep.End:
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

        private void Sequence_TransferFixtureUnload()
        {
            switch ((ETransferFixtureProcessUnloadStep)Step.RunStep)
            {
                case ETransferFixtureProcessUnloadStep.Start:
                    Log.Debug("Transfer Fixture Unload Start");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.Cyl_Up:
                    Log.Debug("Transfer Fixture Cylinder Up");
                    CylUpDown.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CylClamp1.IsForward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.Cyl_Up_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Up Done");
                    Step.RunStep++;
                    Log.Debug("Wait Remove Film Done");
                    break;
                case ETransferFixtureProcessUnloadStep.Wait_RemoveFilm_Done:
                    if(FlagRemoveFilmDone == false)
                    {
                        break;
                    }
                    FlagRemoveFilmDone = false;
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.YAxis_Move_UnloadPosition:
                    Log.Debug("Transfer Fixture Y Axis Move Unload Position");
                    TransferFixtureYAxis.MoveAbs(_transferFixtureRecipe.TransferFixtureYAxisUnloadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => TransferFixtureYAxis.IsOnPosition(_transferFixtureRecipe.TransferFixtureYAxisUnloadPosition));
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.YAxis_Move_UnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Y Axis Move Unload Position Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.Cyl_Down:
                    Log.Debug("Transfer Fixture Cylinder Down");
                    CylUpDown.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CylUpDown.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.Cyl_UnClamp:
                    Log.Debug("Transfer Fixture Cylinder UnClamp");
                    CylClamp1.Backward();
                    CylClamp2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CylClamp1.IsBackward && CylClamp2.IsBackward);
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Fixture Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.SetFlagTransferDone:
                    Log.Debug("Set Flag Transfer Done");
                    FlagFixtureTransferAlignDone = true;
                    FlagFixtureTransferDetachDone = true;
                    FlagFixtureTransferRemoveFilmDone = true;
                    Step.RunStep++;
                    break;
                case ETransferFixtureProcessUnloadStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Transfer Fixture Load");
                    Sequence = ESequence.TransferFixtureLoad;
                    break;
            }
            #endregion
        }
    }
}
