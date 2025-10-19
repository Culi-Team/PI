using EQX.Core.Communication.Modbus;
using EQX.Core.Device.SpeedController;
using EQX.Device.SpeedController;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class SpeedControllerList
    {
        public List<SD201SSpeedController> All { get; }
        public SpeedControllerList(List<SD201SSpeedController> speedControllers)
        {
            All = speedControllers;
        }

        public SD201SSpeedController InConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.IN_CV_ROLLER1);
        public SD201SSpeedController InConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.IN_CV_ROLLER2);
        public SD201SSpeedController InConveyorRoller3 => All.First(m => m.Id == (int)ESpeedController.IN_CV_ROLLER3);
        public SD201SSpeedController SupportConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV_ROLLER1);
        public SD201SSpeedController InWorkConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.IN_WORK_CV_ROLLER1);
        public SD201SSpeedController InWorkConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.IN_WORK_CV_ROLLER2);
        public SD201SSpeedController SupportConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV_ROLLER2);
        public SD201SSpeedController BufferConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.BUFFER_CV_ROLLER1);
        public SD201SSpeedController BufferConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.BUFFER_CV_ROLLER2);
        public SD201SSpeedController SupportConveyorRoller3 => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV_ROLLER3);
        public SD201SSpeedController OutWorkConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.OUT_WORK_CV_ROLLER1);
        public SD201SSpeedController OutWorkConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.OUT_WORK_CV_ROLLER2);
        public SD201SSpeedController SupportConveyorRoller4 => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV_ROLLER4);
        public SD201SSpeedController OutConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.OUT_CV_ROLLER1);
        public SD201SSpeedController OutConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.OUT_CV_ROLLER2);

        public void SetDirection()
        {
            InConveyorRoller1.SetDirection(false);
            InConveyorRoller2.SetDirection(false);
            InConveyorRoller3.SetDirection(false);
            SupportConveyorRoller1.SetDirection(false);
            InWorkConveyorRoller1.SetDirection(false);
            InWorkConveyorRoller2.SetDirection(false);

            SupportConveyorRoller2.SetDirection(true);
            BufferConveyorRoller1.SetDirection(true);
            BufferConveyorRoller2.SetDirection(true);

            SupportConveyorRoller3.SetDirection(true);
            OutWorkConveyorRoller1.SetDirection(true);
            OutWorkConveyorRoller2.SetDirection(true);
            SupportConveyorRoller4.SetDirection(true);
            OutConveyorRoller1.SetDirection(true);
            OutConveyorRoller2.SetDirection(true);
        }
    }
}
