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
    public class UnloadTransferProcess : ProcessBase<ESequence>
    {
        #region Privates
        private EPort port => Name == EProcess.UnloadTransferLeft.ToString() ? EPort.Left : EPort.Right;

        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly UnloadTransferRecipe _unloadTransferLeftRecipe;
        private readonly UnloadTransferRecipe _unloadTransferRightRecipe;

        private IMotion YAxis => port == EPort.Left ? _devices.MotionsInovance.GlassUnloadLYAxis :
                                                  _devices.MotionsInovance.GlassUnloadRYAxis;

        private IMotion ZAxis => port == EPort.Left ? _devices.MotionsInovance.GlassUnloadLZAxis :
                                                  _devices.MotionsInovance.GlassUnloadRZAxis;

        private IDOutput GlassVac => port == EPort.Left ? _devices.Outputs.UnloadTransferLVacOnOff:
                                                  _devices.Outputs.UnloadTransferRVacOnOff;

        private bool IsVacDetect => port == EPort.Left ? _devices.Inputs.UnloadTransferLVac.Value :
                                                         _devices.Inputs.UnloadTransferRVac.Value;

        private UnloadTransferRecipe Recipe => port == EPort.Left ? _unloadTransferLeftRecipe :
                                                    _unloadTransferRightRecipe;
        #endregion

        #region Constructor
        public UnloadTransferProcess(Devices devices,
            CommonRecipe commonRecipe,
            [FromKeyedServices("UnloadTransferLeftRecipe")] UnloadTransferRecipe unloadTransferLeftRecipe,
            [FromKeyedServices("UnloadTransferRightRecipe")] UnloadTransferRecipe unloadTransferRightRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _unloadTransferLeftRecipe = unloadTransferLeftRecipe;
            _unloadTransferRightRecipe = unloadTransferRightRecipe;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch((EUnloadTransferOriginStep)Step.OriginStep)
            {
                case EUnloadTransferOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.ZAxis_Origin:
                    Log.Debug("Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return ZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.YAxis_Origin:
                    Log.Debug("Y Axis Origin Start");
                    YAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return YAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EUnloadTransferOriginStep.End:
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
            switch ((EUnloadTransferAutoRunStep)Step.RunStep)
            {
                case EUnloadTransferAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case EUnloadTransferAutoRunStep.GlassVac_Check:
                    if(IsVacDetect)
                    {
                        Log.Info("Sequence Unload Transfer Place");
                        Sequence = ESequence.UnloadTransferPlace;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadTransferAutoRunStep.End:
                    Log.Info("Sequence AF Clean Unload");
                    Sequence = ESequence.AFCleanUnload;
                    break;
            }
        }
        #endregion

    }
}
