using EQX.Core.Communication.Modbus;
using EQX.Core.TorqueController;
using EQX.Motion.Torque;
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
        public List<ITorqueController> All { get; }
        public TorqueControllerList(List<ITorqueController> torqueControllerList)
        {
            All = torqueControllerList;
        }

        public ITorqueController VinylCleanWinder => All.First(m => m.Id == (int)ETorqueController.VinylClean_Winder);
        public ITorqueController WETCleanLeftWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Left_Winder);
        public ITorqueController WETCleanLeftUnWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Left_UnWinder);
        public ITorqueController WETCleanRightWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Right_Winder);
        public ITorqueController WETCleanRightUnWinder => All.First(m => m.Id == (int)ETorqueController.WETClean_Right_UnWinder);
        public ITorqueController AFCleanLeftWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Left_Winder);
        public ITorqueController AFCleanLeftUnWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Left_UnWinder);
        public ITorqueController AFCleanRightWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Right_Winder);
        public ITorqueController AFCleanRightUnWinder => All.First(m => m.Id == (int)ETorqueController.AFClean_Right_UnWinder);
    }
}
