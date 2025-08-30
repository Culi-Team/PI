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
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion

        #region Constructor
        public BufferConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion

        #region Inputs
        private IDInput BufferDect1 => _devices.Inputs.BufferCstDetect1;
        private IDInput BufferDect2 => _devices.Inputs.BufferCstDetect2;

        #endregion

        #region Cylinders
        private ICylinder BufferStopper1 => _devices.Cylinders.BufferCvStopper1UpDown;
        private ICylinder BufferStopper2 => _devices.Cylinders.BufferCvStopper2UpDown;
        private ICylinder InCvSupBuffer => _devices.Cylinders.InCvSupportBufferUpDown;
        private ICylinder OutCvSupBuffer => _devices.Cylinders.OutCvSupportBufferUpDown;
        #endregion

        #region Rollers
        private ISpeedController BufferRoller1 => _devices.SpeedControllerList.BufferConveyorRoller1;
        private ISpeedController BufferRoller2 => _devices.SpeedControllerList.BufferConveyorRoller2;
        private ISpeedController InSupRoller => _devices.SpeedControllerList.SupportConveyor2Roller;
        private ISpeedController OutSupRoller => _devices.SpeedControllerList.SupportConveyor3Roller;
        #endregion

    }
}
