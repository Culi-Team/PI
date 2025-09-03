using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.VirtualIO;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class DetachProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly VirtualIO<EFlags> _virtualIO;

        private IMotion DetachGlassZAxis => _devices.MotionsInovance.DetachGlassZAxis;
        private IMotion ShuttleTransferXAxis => _devices.MotionsInovance.ShuttleTransferXAxis;
        private IMotion ShuttleTransferZAxis => _devices.MotionsAjin.ShuttleTransferZAxis;

        private ICylinder DetachFixFixtureCyl1 => _devices.Cylinders.DetachFixFixtureCyl1FwBw;
        private ICylinder DetachFixFixtureCyl2 => _devices.Cylinders.DetachFixFixtureCyl2FwBw;

        private ICylinder DetachCyl1 => _devices.Cylinders.DetachCyl1UpDown;
        private ICylinder DetachCyl2 => _devices.Cylinders.DetachCyl2UpDown;

        private bool isFixtureDetect => _devices.Inputs.DetachFixtureDetect.Value;
        
        private bool isGlassShuttleVac1 => _devices.Inputs.DetachGlassShtVac1.Value;
        private bool isGlassShuttleVac2 => _devices.Inputs.DetachGlassShtVac2.Value;
        private bool isGlassShuttleVac3 => _devices.Inputs.DetachGlassShtVac3.Value;

        private IDOutput glassShuttleVac1 => _devices.Outputs.DetachGlassShtVac1OnOff;
        private IDOutput glassShuttleVac2 => _devices.Outputs.DetachGlassShtVac2OnOff;
        private IDOutput glassShuttleVac3 => _devices.Outputs.DetachGlassShtVac3OnOff;
        #endregion

        #region Flags
        private bool FlagOriginDone
        {
            set
            {
                _virtualIO.SetFlag(EFlags.DetachProcessOriginDone, value);
            }
        }
        #endregion

        #region Private Methods
        private void GlassShuttleVacOnOff(bool onOff)
        {
            glassShuttleVac1.Value = onOff;
            glassShuttleVac2.Value = onOff;
            glassShuttleVac3.Value = onOff;
        }
        #endregion

        #region Constructor
        public DetachProcess(Devices devices, CommonRecipe commonRecipe,
                            VirtualIO<EFlags> virtualIO)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _virtualIO = virtualIO;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch((EDetachProcessOriginStep)Step.OriginStep)
            {
                case EDetachProcessOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ZAxis_Origin:
                    Log.Debug("Detach Glass Z Axis Origin Start");
                    Log.Debug("Shuttle Transfer Z Axis Origin Start");
                    DetachGlassZAxis.SearchOrigin();
                    ShuttleTransferZAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return DetachGlassZAxis.Status.IsHomeDone && ShuttleTransferZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ZAxis_Origin_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Glass Z Axis Origin Done");
                    Log.Debug("Shuttle Transfer Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.DetachCyl_Up:
                    Log.Debug("Detach Cylinder Up");
                    DetachCyl1.Backward();
                    DetachCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return DetachCyl1.IsBackward && DetachCyl2.IsBackward; });
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.DetachCyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Detach Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ShtTransferXAxis_Origin:
                    Log.Debug("Shuttle Transfer X Axis Origin Start");
                    ShuttleTransferXAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return ShuttleTransferXAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.ShtTransferXAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Shuttle Transfer X Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EDetachProcessOriginStep.End:
                    Log.Debug("Origin End");
                    FlagOriginDone = true;
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    return true;
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
                case ESequence.FixtureTransfer:
                    break;
                case ESequence.Detach:
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
            switch ((EDetachAutoRunStep)Step.RunStep)
            {
                case EDetachAutoRunStep.Start:
                    break;
                case EDetachAutoRunStep.ShuttleTransfer_Vac_Check:
                    break;
                case EDetachAutoRunStep.Fixture_Detect_Check:
                    break;
                case EDetachAutoRunStep.End:
                    break;
            }
        }
        #endregion
    }
}
