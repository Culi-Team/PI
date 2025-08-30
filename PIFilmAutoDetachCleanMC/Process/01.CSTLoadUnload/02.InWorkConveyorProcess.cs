using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class InWorkConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion

        #region Constructor
        public InWorkConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion

        #region Inputs
        private IDInput Detect1 => _devices.Inputs.InCstWorkDetect1;
        private IDInput Detect2 => _devices.Inputs.InCstWorkDetect2;
        private IDInput Detect3 => _devices.Inputs.InCstWorkDetect3;
        private IDInput Detect4 => _devices.Inputs.InCstWorkDetect4;
        #endregion

        #region Outputs
        #endregion

        #region Cylinders
        private ICylinder FixCylinder => _devices.Cylinders.InCstFixCylFwBw;
        private ICylinder TiltCylinder => _devices.Cylinders.InCstTiltCylUpDown;
        private ICylinder RollerCyl => _devices.Cylinders.InCvSupportUpDown;
        #endregion

        #region Motions
        private IMotion InCSTTAxis => _devices.MotionsInovance.InCassetteTAxis;
        #endregion

        #region Rollers
        private ISpeedController RollerSup => _devices.SpeedControllerList.SupportConveyor1Roller;
        private ISpeedController Roller1 => _devices.SpeedControllerList.InWorkConveyorRoller1;
        private ISpeedController Roller2 => _devices.SpeedControllerList.InWorkConveyorRoller2;
        #endregion
    }
}
