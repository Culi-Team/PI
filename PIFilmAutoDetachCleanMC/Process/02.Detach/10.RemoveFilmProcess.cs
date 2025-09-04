using EQX.Core.InOut;
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
    public class RemoveFilmProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private ICylinder FixCyl1 => _devices.Cylinders.RemoveZoneFixCyl1FwBw;
        private ICylinder FixCyl2 => _devices.Cylinders.RemoveZoneFixCyl2FwBw;

        private ICylinder TransferCyl => _devices.Cylinders.RemoveZoneTrCylFwBw;
        private ICylinder UpDownCyl1 => _devices.Cylinders.RemoveZoneZCyl1UpDown;
        private ICylinder UpDownCyl2 => _devices.Cylinders.RemoveZoneZCyl2UpDown;
        private ICylinder ClampCyl => _devices.Cylinders.RemoveZoneCylClampUnclamp;

        private ICylinder PusherCyl1 => _devices.Cylinders.RemoveZonePusherCyl1UpDown;
        private ICylinder PusherCyl2 => _devices.Cylinders.RemoveZonePusherCyl2UpDown;
        #endregion

        #region Contructor
        public RemoveFilmProcess(Devices devices,
            CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ERemoveFilmProcessOriginStep)Step.OriginStep)
            {
                case ERemoveFilmProcessOriginStep.Start:
                    Log.Debug("Remove Film Process Start");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Fix_Cyl_Backward:
                    Log.Debug("Remove Film Process Fix Cylinder Backward");
                    FixCyl1.Backward();
                    FixCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCyl1.IsBackward && FixCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Fix_Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Fix Cylinder Backward Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Pusher_Cyl_Down:
                    Log.Debug("Remove Film Process Pusher Cylinder Down");
                    PusherCyl1.Backward();
                    PusherCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PusherCyl1.IsBackward && PusherCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Pusher_Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Pusher Cylinder Down Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Up:
                    Log.Debug("Remove Film Process Cylinder Up");
                    UpDownCyl1.Backward();
                    UpDownCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => UpDownCyl1.IsBackward && UpDownCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_UnClamp:
                    Log.Debug("Remove Film Process Cylinder UnClamp");
                    ClampCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder UnClamp Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Transfer_Backward:
                    Log.Debug("Remove Film Process Cylinder Transfer Backward");
                    TransferCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TransferCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.Cyl_Transfer_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Remove Film Process Cylinder Transfer Backward Done");
                    Step.OriginStep++;
                    break;
                case ERemoveFilmProcessOriginStep.End:
                    Log.Debug("Remove Film Process End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
            }

            return true;
        }
        #endregion

    }
}
