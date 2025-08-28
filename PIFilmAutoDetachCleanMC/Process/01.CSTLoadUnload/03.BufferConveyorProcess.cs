using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
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
    public class BufferConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Inputs _inputs;
        private readonly Cylinders _cylinders;
        private readonly SpeedControllerList _speedControllerList;
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion
        #region Constructor
        public BufferConveyorProcess(Inputs inputs, Cylinders cylinders, SpeedControllerList speedControllerList, Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _inputs = inputs;
            _cylinders = cylinders;
            _speedControllerList = speedControllerList;
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion
        #region Inputs
        private IDInput BufferDect1 => _inputs.BufferCstDetect1;
        private IDInput BufferDect2 => _inputs.BufferCstDetect2;

        #endregion
        #region Cylinders
        private ICylinder BufferStopper1 => _cylinders.BufferCvStopper1UpDown;
        private ICylinder BufferStopper2 => _cylinders.BufferCvStopper2UpDown;
        private ICylinder InCvSupBuffer => _cylinders.InCvSupportBufferUpDown;
        private ICylinder OutCvSupBuffer => _cylinders.OutCvSupportBufferUpDown;
        #endregion
        #region Rollers
        private ISpeedController BufferRoller1 => _speedControllerList.BufferConveyorRoller1;
        private ISpeedController BufferRoller2 => _speedControllerList.BufferConveyorRoller2;
        private ISpeedController InSupRoller => _speedControllerList.SupportConveyor2Roller;
        private ISpeedController OutSupRoller => _speedControllerList.SupportConveyor3Roller;
        #endregion
    }
}
