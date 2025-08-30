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
    public class UnloadTransferProcess : ProcessBase<ESequence>
    {
        #region Privates
        private EPort port => Name == EProcess.UnloadTransferLeft.ToString() ? EPort.Left : EPort.Right;

        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private IMotion YAxis => port == EPort.Left ? _devices.MotionsInovance.GlassUnloadLYAxis :
                                                  _devices.MotionsInovance.GlassUnloadRYAxis;

        private IMotion ZAxis => port == EPort.Left ? _devices.MotionsInovance.GlassUnloadLZAxis :
                                                  _devices.MotionsInovance.GlassUnloadRZAxis;

        private IDOutput GlassVac => port == EPort.Left ? _devices.Outputs.UnloadTransferLVacOnOff:
                                                  _devices.Outputs.UnloadTransferRVacOnOff;

        private bool IsVacDetect => port == EPort.Left ? _devices.Inputs.UnloadTransferLVac.Value :
                                                         _devices.Inputs.UnloadTransferRVac.Value;
        #endregion

        #region Constructor
        public UnloadTransferProcess(Devices devices,CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
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
