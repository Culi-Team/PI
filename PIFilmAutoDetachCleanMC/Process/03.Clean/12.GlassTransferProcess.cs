using EQX.Core.InOut;
using EQX.Core.Motion;
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
    public class GlassTransferProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _glassTransferInput;
        private readonly IDOutputDevice _glassTransferOutput;

        private IMotion GlassTransferYAxis => _devices.MotionsInovance.GlassTransferYAxis;
        private IMotion GlassTransferZAxis => _devices.MotionsInovance.GlassTransferZAxis;

        private IDOutput GlassVac1 => _devices.Outputs.GlassTransferVac1OnOff;
        private IDOutput GlassVac2 => _devices.Outputs.GlassTransferVac2OnOff;
        private IDOutput GlassVac3 => _devices.Outputs.GlassTransferVac3OnOff;

        private bool IsVac1Detect => _devices.Inputs.GlassTransferVac1.Value;
        private bool IsVac2Detect => _devices.Inputs.GlassTransferVac2.Value;
        private bool IsVac3Detect => _devices.Inputs.GlassTransferVac3.Value;

        private bool IsVacDetect => IsVac1Detect && IsVac2Detect && IsVac3Detect;
        #endregion

        #region Flags
        private bool FlagDetachRequestUnloadGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.DETACH_REQ_UNLOAD_GLASS];
            }
        }

        private bool FlagGlassAlignLeftRequestGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_REQ_GLASS];
            }
        }

        private bool FlagGlassAlignRightRequestGlass
        {
            get
            {
                return _glassTransferInput[(int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_REQ_GLASS];
            }
        }

        private bool FlagGlassTransferPickDone
        {
            set
            {
                _glassTransferOutput[(int)EGlassTransferProcessOutput.GLASS_TRANSFER_PICK_DONE] = value;
            }
        }

        private bool FlagGlassTransferLeftPlaceDone
        {
            set
            {
                _glassTransferOutput[(int)EGlassTransferProcessOutput.GLASS_TRANSFER_LEFT_PLACE_DONE] = value;
            }
        }

        private bool FlagGlassTransferRightPlaceDone
        {
            set
            {
                _glassTransferOutput[(int)EGlassTransferProcessOutput.GLASS_TRANSFER_RIGHT_PLACE_DONE] = value;
            }
        }
        #endregion

        #region Constructor
        public GlassTransferProcess(Devices devices,
            CommonRecipe commonRecipe,
            [FromKeyedServices("GlassTransferInput")] IDInputDevice glassTransferInput,
            [FromKeyedServices("GlassTransferOutput")] IDOutputDevice glassTransferOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _glassTransferInput = glassTransferInput;
            _glassTransferOutput = glassTransferOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EGlassTransferOriginStep)Step.OriginStep)
            {
                case EGlassTransferOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.ZAxis_Origin:
                    Log.Debug("Glass Transfer Z Axis Origin Start");
                    GlassTransferZAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return GlassTransferZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Glass Transfer Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Origin:
                    Log.Debug("Glass Transfer Y Axis Origin Start");
                    GlassTransferYAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return GlassTransferYAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Glass Transfer Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EGlassTransferOriginStep.End:
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
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.CSTTilt:
                    break;
                case ESequence.CSTUnTilt:
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
                case ESequence.AlignGlass:
                    Sequence_AlignGlass();
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
        private void VacOnOff(bool bOnOff)
        {
            GlassVac1.Value = bOnOff;
            GlassVac2.Value = bOnOff;
            GlassVac3.Value = bOnOff;
        }

        private void Sequence_AutoRun()
        {
            switch ((EGlassTransferAutoRunStep)Step.RunStep)
            {
                case EGlassTransferAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EGlassTransferAutoRunStep.VacuumDetect_Check:
                    if(IsVacDetect)
                    {
                        Log.Info("Sequence Align Glass");
                        Sequence = ESequence.AlignGlass;
                        break;
                    }

                    Step.RunStep++;
                    break;
                case EGlassTransferAutoRunStep.End:
                    Log.Info("Sequence Glass Transfer Pick");
                    Sequence = ESequence.GlassTransferPick;
                    break;
            }
        }

        private void Sequence_AlignGlass()
        {
            switch ((EGlassTransferAlignGlassStep)Step.RunStep)
            {
                case EGlassTransferAlignGlassStep.Start:
                    Log.Debug("Align Glass Start");
                    Step.RunStep++;
                    break;
                case EGlassTransferAlignGlassStep.ZAxis_Move_ReadyPosition:
                    break;
                case EGlassTransferAlignGlassStep.ZAxis_Move_ReadyPosition_Wait:
                    break;
                case EGlassTransferAlignGlassStep.YAxis_Move_ReadyPosition:
                    break;
                case EGlassTransferAlignGlassStep.YAxis_Move_ReadyPosition_Wait:
                    break;
                case EGlassTransferAlignGlassStep.Wait_GlassAlign_Req_Glass:
                    break;
                case EGlassTransferAlignGlassStep.End:
                    break;
            }
        }
        #endregion
    }
}
