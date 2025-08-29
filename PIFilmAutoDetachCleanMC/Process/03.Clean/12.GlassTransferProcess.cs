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
    public class GlassTransferProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private IMotion GlassTransferYAxis => _devices.MotionsInovance.GlassTransferYAxis;
        private IMotion GlassTransferZAxis => _devices.MotionsInovance.GlassTransferZAxis;

        private IDOutput glassVac1 => _devices.Outputs.GlassTransferVac1OnOff;
        private IDOutput glassVac2 => _devices.Outputs.GlassTransferVac2OnOff;
        private IDOutput glassVac3 => _devices.Outputs.GlassTransferVac3OnOff;

        private bool isVac1Detect => _devices.Inputs.GlassTransferVac1.Value;
        private bool isVac2Detect => _devices.Inputs.GlassTransferVac2.Value;
        private bool isVac3Detect => _devices.Inputs.GlassTransferVac3.Value;
        #endregion

        #region Constructor
        public GlassTransferProcess(Devices devices, CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
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
        #endregion
    }
}
