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
    public class InConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
        private readonly Cylinders _cylinders;
        private readonly SpeedControllerList _speedControllerList;
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion

        #region Constructor
        public InConveyorProcess(Inputs inputs, Outputs outputs, Cylinders cylinders, SpeedControllerList speedControllerList, Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _inputs = inputs;
            _outputs = outputs;
            _cylinders = cylinders;
            _speedControllerList = speedControllerList;
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion

        #region Inputs
        private IDInput CST_Det1 => _inputs.InCstDetect1;
        private IDInput CST_Det2 => _inputs.InCstDetect2;
        private IDInput InButton1 => _inputs.InButton1;
        private IDInput InButton2 => _inputs.InButton2;
        private IDInput InCSTLightCurtain => _inputs.InCstLightCurtainSafetyDetect;

        #endregion
        #region Outputs
        private IDOutput InButton1Lamp => _outputs.InButtonLamp1;
        private IDOutput InButton2Lamp => _outputs.InButtonLamp2;
        private IDOutput InCstMutingLightCurtain => _outputs.InCstLightCurtainMuting;
        #endregion
        #region Cylinders
        private ICylinder CstStopper => _cylinders.InCstStopperUpDown;
        #endregion
        #region Rollers
        private ISpeedController Roller1 => _speedControllerList.InConveyorRoller1;
        private ISpeedController Roller2 => _speedControllerList.InConveyorRoller2;
        private ISpeedController Roller3 => _speedControllerList.InConveyorRoller3;

        #endregion
    }
}
