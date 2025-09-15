using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Sequence;
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
        private EPort port => Name == EProcess.InConveyor.ToString() ? EPort.Right : EPort.Left;
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly CommonRecipe _commonRecipe;
        #endregion

        #region Constructor
        public InConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe, CommonRecipe commonRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Inputs
        private IDInput CST_Det1 => _devices.Inputs.InCstDetect1;
        private IDInput CST_Det2 => _devices.Inputs.InCstDetect2;
        private IDInput InButton1 => _devices.Inputs.InButton1;
        private IDInput InButton2 => _devices.Inputs.InButton2;
        private IDInput InCSTLightCurtain => _devices.Inputs.InCstLightCurtainAlarmDetect;

        #endregion

        #region Outputs
        private IDOutput InButton1Lamp => _devices.Outputs.InButtonLamp1;
        private IDOutput InButton2Lamp => _devices.Outputs.InButtonLamp2;
        private IDOutput InCstMutingLightCurtain1 => _devices.Outputs.InCstLightCurtainMuting1;
        private IDOutput InCstMutingLightCurtain2 => _devices.Outputs.InCstLightCurtainMuting2;
        #endregion

        #region Cylinders
        private ICylinder StopperCylinder => port == EPort.Right ? _devices.Cylinders.InCstStopperUpDown :
                                                              _devices.Cylinders.OutCstStopperUpDown;
        #endregion

        #region Rollers
        private ISpeedController Roller1 => port == EPort.Right ? _devices.SpeedControllerList.InConveyorRoller1 :
                                                                 _devices.SpeedControllerList.OutConveyorRoller1;
        private ISpeedController Roller2 => port == EPort.Right ? _devices.SpeedControllerList.InConveyorRoller2 :
                                                                 _devices.SpeedControllerList.OutConveyorRoller2;
        private ISpeedController Roller3 => _devices.SpeedControllerList.InConveyorRoller3;
        #endregion

        #region Override Method
        public override bool ProcessOrigin()
        {
            switch ((EConveyorOriginStep)Step.OriginStep)
            {
                case EConveyorOriginStep.Start:
                    Log.Debug("Orign start");
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.CstStopper_Down:
                    Log.Debug("CstStopper_Down");
                    StopperCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => StopperCylinder.IsBackward);
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.CstStopper_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Stopper cylinder down done");
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.Roller_Stop:
                    Roller1.Stop();
                    Roller2.Stop();
                    Roller3.Stop();
                    Log.Debug("Roller Stop");
                    Step.OriginStep++;
                    break;
                case EConveyorOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
                default:
                    break;
            }
            return true;
        }
        #endregion
    }
}
