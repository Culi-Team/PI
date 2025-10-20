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
    public class RollerList
    {
        public List<BD201SRollerController> All { get; }
        public RollerList(List<BD201SRollerController> speedControllers)
        {
            All = speedControllers;
        }

        public BD201SRollerController InConveyorRoller1 => All.First(m => m.Id == (int)ERoller.IN_CV_ROLLER1);
        public BD201SRollerController InConveyorRoller2 => All.First(m => m.Id == (int)ERoller.IN_CV_ROLLER2);
        public BD201SRollerController InConveyorRoller3 => All.First(m => m.Id == (int)ERoller.IN_CV_ROLLER3);
        public BD201SRollerController SupportConveyorRoller1 => All.First(m => m.Id == (int)ERoller.SUPPORT_CV_ROLLER1);
        public BD201SRollerController InWorkConveyorRoller1 => All.First(m => m.Id == (int)ERoller.IN_WORK_CV_ROLLER1);
        public BD201SRollerController InWorkConveyorRoller2 => All.First(m => m.Id == (int)ERoller.IN_WORK_CV_ROLLER2);
        public BD201SRollerController SupportConveyorRoller2 => All.First(m => m.Id == (int)ERoller.SUPPORT_CV_ROLLER2);
        public BD201SRollerController BufferConveyorRoller1 => All.First(m => m.Id == (int)ERoller.BUFFER_CV_ROLLER1);
        public BD201SRollerController BufferConveyorRoller2 => All.First(m => m.Id == (int)ERoller.BUFFER_CV_ROLLER2);
        public BD201SRollerController SupportConveyorRoller3 => All.First(m => m.Id == (int)ERoller.SUPPORT_CV_ROLLER3);
        public BD201SRollerController OutWorkConveyorRoller1 => All.First(m => m.Id == (int)ERoller.OUT_WORK_CV_ROLLER1);
        public BD201SRollerController OutWorkConveyorRoller2 => All.First(m => m.Id == (int)ERoller.OUT_WORK_CV_ROLLER2);
        public BD201SRollerController SupportConveyorRoller4 => All.First(m => m.Id == (int)ERoller.SUPPORT_CV_ROLLER4);
        public BD201SRollerController OutConveyorRoller1 => All.First(m => m.Id == (int)ERoller.OUT_CV_ROLLER1);
        public BD201SRollerController OutConveyorRoller2 => All.First(m => m.Id == (int)ERoller.OUT_CV_ROLLER2);

        public void SetDirection()
        {
            InConveyorRoller1.SetDirection(false);
            InConveyorRoller2.SetDirection(false);
            InConveyorRoller3.SetDirection(false);
            SupportConveyorRoller1.SetDirection(true);
            InWorkConveyorRoller1.SetDirection(true);
            InWorkConveyorRoller2.SetDirection(true);

            SupportConveyorRoller2.SetDirection(false);
            BufferConveyorRoller1.SetDirection(false);
            BufferConveyorRoller2.SetDirection(false);

            SupportConveyorRoller3.SetDirection(false);
            OutWorkConveyorRoller1.SetDirection(false);
            OutWorkConveyorRoller2.SetDirection(false);
            SupportConveyorRoller4.SetDirection(true);

            OutConveyorRoller1.SetDirection(false);
            OutConveyorRoller2.SetDirection(false);
        }
    }
}
