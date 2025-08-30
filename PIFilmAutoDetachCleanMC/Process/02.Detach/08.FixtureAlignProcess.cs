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
    public class FixtureAlignProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private ICylinder AlignFixtureCyl => _devices.Cylinders.AlignFixtureBwFw;
        private bool IsFixtureDetect => _devices.Inputs.AlignFixtureDetect.Value;
        private bool IsFixtureTiltDetect => _devices.Inputs.AlignFixtureTiltDetect.Value;
        private bool IsFixtureReverseDetect => _devices.Inputs.AlignFixtureReverseDetect.Value;

        #region Constructor
        public FixtureAlignProcess(Devices devices,
            CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch((EFixtureAlignOriginStep)Step.OriginStep)
            {
                case EFixtureAlignOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EFixtureAlignOriginStep.AlignFixtureBackward:
                    Log.Debug("Align Fixture Backward");
                    AlignFixtureCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return AlignFixtureCyl.IsBackward; });
                    Step.OriginStep++;
                    break;
                case EFixtureAlignOriginStep.AlignFixtureBackward_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Align Fixture Backward Done");
                    Step.OriginStep++;
                    break;
                case EFixtureAlignOriginStep.End:
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
