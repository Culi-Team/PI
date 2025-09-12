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
    public class OutWorkConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion

        #region Constructor
        public OutWorkConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion

        #region Inputs
        private IDInput Detect1 => _devices.Inputs.OutCstWorkDetect1;
        private IDInput Detect2 => _devices.Inputs.OutCstWorkDetect2;
        private IDInput Detect3 => _devices.Inputs.OutCstWorkDetect3;

        #endregion

        #region Cylinders
        private ICylinder Fix1 => _devices.Cylinders.OutCstFixCyl1FwBw;
        private ICylinder Fix2 => _devices.Cylinders.OutCstFixCyl2FwBw;
        private ICylinder Tilt => _devices.Cylinders.OutCstTiltCylUpDown;
        private ICylinder RollerCyl => _devices.Cylinders.OutCvSupportUpDown;
        #endregion

        #region Motions
        private IMotion OutCSTTAxis => _devices.MotionsInovance.OutCassetteTAxis;
        #endregion

        #region Rollers
        private ISpeedController RollerSup => _devices.SpeedControllerList.SupportConveyor4Roller;
        private ISpeedController Roller1 => _devices.SpeedControllerList.OutWorkConveyorRoller1;
        private ISpeedController Roller2 => _devices.SpeedControllerList.OutWorkConveyorRoller2;
        #endregion
    }
}
