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
        private readonly Inputs _inputs;
        private readonly Cylinders _cylinders;
        private readonly MotionsInovance _motionInovance;
        private readonly SpeedControllerList _speedControllerList;
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion

        #region Constructor
        public OutWorkConveyorProcess(Inputs inputs, Cylinders cylinders, MotionsInovance motionInovance, SpeedControllerList speedControllerList, Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _inputs = inputs;
            _cylinders = cylinders;
            _motionInovance = motionInovance;
            _speedControllerList = speedControllerList;
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion

        #region Inputs
        private IDInput Detect1 => _inputs.OutCstWorkDetect1;
        private IDInput Detect2 => _inputs.OutCstWorkDetect2;
        private IDInput Detect3 => _inputs.OutCstWorkDetect3;

        #endregion
        #region Cylinders
        private ICylinder Fix1 => _cylinders.OutCstFixCyl1FwBw;
        private ICylinder Fix2 => _cylinders.OutCstFixCyl2FwBw;
        private ICylinder Tilt => _cylinders.OutCstTiltCylUpDown;
        private ICylinder RollerCyl => _cylinders.OutCvSupportUpDown;
        #endregion
        #region Motions
        private IMotion OutCSTTAxis => _motionInovance.OutCassetteTAxis;
        #endregion
        #region Rollers
        private ISpeedController RollerSup => _speedControllerList.SupportConveyor4Roller;
        private ISpeedController Roller1 => _speedControllerList.OutWorkConveyorRoller1;
        private ISpeedController Roller2 => _speedControllerList.OutWorkConveyorRoller2;
        #endregion
    }
}
