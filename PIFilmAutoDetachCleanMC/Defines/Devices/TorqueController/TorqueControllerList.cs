using EQX.Core.Communication.Modbus;
using EQX.Core.TorqueController;
using EQX.Device.Torque;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class TorqueControllerList
    {
        public List<DX3000TorqueController> All { get; }
        public TorqueControllerList(List<DX3000TorqueController> torqueControllerList)
        {
            All = torqueControllerList;
        }

        public DX3000TorqueController VinylCleanUnWinder => All.First(m => m.Id == (int)ETorqueController.VinylClean_UnWinder);
        public DX3000TorqueController WETCleanLeftWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Left_Winder);
        public DX3000TorqueController WETCleanLeftUnWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Left_UnWinder);
        public DX3000TorqueController WETCleanRightWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Right_Winder);
        public DX3000TorqueController WETCleanRightUnWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Right_UnWinder);
        public DX3000TorqueController AFCleanLeftWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Left_Winder);
        public DX3000TorqueController AFCleanLeftUnWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Left_UnWinder);
        public DX3000TorqueController AFCleanRightWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Right_Winder);
        public DX3000TorqueController AFCleanRightUnWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Right_UnWinder);
    }
}
