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
        public List<ISpeedController> All { get; }
        public SpeedControllerList(List<ISpeedController> speedControllers)
        {
            All = speedControllers;
        }

        public ISpeedController InConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.IN_CV_ROLLER1);
        public ISpeedController InConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.IN_CV_ROLLER2);
        public ISpeedController InConveyorRoller3 => All.First(m => m.Id == (int)ESpeedController.IN_CV_ROLLER3);
        public ISpeedController SupportConveyor1Roller => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV1_ROLLER);
        public ISpeedController InWorkConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.IN_WORK_CV_ROLLER1);
        public ISpeedController InWorkConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.IN_WORK_CV_ROLLER2);
        public ISpeedController SupportConveyor2Roller => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV2_ROLLER);
        public ISpeedController BufferConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.BUFFER_CV_ROLLER1);
        public ISpeedController BufferConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.BUFFER_CV_ROLLER2);
        public ISpeedController SupportConveyor3Roller => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV3_ROLLER);
        public ISpeedController OutWorkConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.OUT_WORK_CV_ROLLER1);
        public ISpeedController OutWorkConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.OUT_WORK_CV_ROLLER2);
        public ISpeedController SupportConveyor4Roller => All.First(m => m.Id == (int)ESpeedController.SUPPORT_CV4_ROLLER);
        public ISpeedController OutConveyorRoller1 => All.First(m => m.Id == (int)ESpeedController.OUT_CV_ROLLER1);
        public ISpeedController OutConveyorRoller2 => All.First(m => m.Id == (int)ESpeedController.OUT_CV_ROLLER2);
    }
}
