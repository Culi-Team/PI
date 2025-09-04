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
    public class VinylCleanProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly IMotion _vinylCleanEncoder;
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private bool IsFixtureDetect => _devices.Inputs.VinylCleanFixtureDetect.Value;
        private ICylinder FixtureClampCyl1 => _devices.Cylinders.VinylCleanFixture1ClampUnclamp;
        private ICylinder FixtureClampCyl2 => _devices.Cylinders.VinylCleanFixture2ClampUnclamp;

        private ICylinder RollerBwFwCyl => _devices.Cylinders.VinylCleanRollerBwFw;
        private ICylinder RollerUpDownCyl => _devices.Cylinders.VinylCleanPusherRollerUpDown;

        private bool IsUnWinderFullDetect => _devices.Inputs.VinylCleanFullDetect.Value;
        private bool IsWinderRunOffDetect => _devices.Inputs.VinylCleanRunoffDetect.Value;
        #endregion

        #region Constructor
        public VinylCleanProcess([FromKeyedServices("VinylCleanEncoder")] IMotion vinylCleanEncoder,
            Devices devices,
            CommonRecipe commonRecipe)
        {
            _vinylCleanEncoder = vinylCleanEncoder;
            _devices = devices;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((EVinylCleanOriginStep)Step.OriginStep)
            {
                case EVinylCleanOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Down:
                    Log.Debug("Vinyl Clean Cylinder Roller Down");
                    RollerUpDownCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => RollerUpDownCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Roller Down Done");
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Backward:
                    Log.Debug("Vinyl Clean Cylinder Roller Backward");
                    RollerBwFwCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => RollerBwFwCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.Cyl_Roller_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Vinyl Clean Cylinder Roller Backward Done");
                    Step.OriginStep++;
                    break;
                case EVinylCleanOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
            }
            return true;
        }
        #endregion
    }
}
