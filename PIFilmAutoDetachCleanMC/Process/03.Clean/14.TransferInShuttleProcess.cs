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
    public class TransferInShuttleProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private EPort port => Name == EProcess.TransferInShuttleLeft.ToString() ? EPort.Left : EPort.Right;

        private IMotion TransferInShuttleYAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLYAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRYAxis;

        private IMotion TransferInShuttleZAxis => port == EPort.Left ? _devices.MotionsInovance.TransferInShuttleLZAxis
                                                                        : _devices.MotionsInovance.TransferInShuttleRZAxis;

        public IDOutput glassVac => port == EPort.Left ? _devices.Outputs.TransferInShuttleLVacOnOff
                                                        : _devices.Outputs.TransferInShuttleRVacOnOff;

        private bool isVacDetect => port == EPort.Left ? _devices.Inputs.TransferInShuttleLVac.Value
                                                        : _devices.Inputs.TransferInShuttleRVac.Value;
        #endregion

        #region Constructor
        public TransferInShuttleProcess(Devices devices,CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch((ETransferInShuttleOriginStep)Step.OriginStep)
            {
                case ETransferInShuttleOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.ZAxis_Origin:
                    Log.Debug("Transfer In Shuttle Z Axis Origin Start");
                    TransferInShuttleZAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return TransferInShuttleZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer In Shuttle Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.YAxis_Origin:
                    Log.Debug("Transfer In Shuttle Y Axis Origin Start");
                    TransferInShuttleYAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return TransferInShuttleYAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer In Shuttle Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferInShuttleOriginStep.End:
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
        #endregion
    }
}
