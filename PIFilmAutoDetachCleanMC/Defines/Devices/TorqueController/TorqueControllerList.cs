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
        private readonly IModbusCommunication _modbusCommunication;

        public List<ITorqueController> All { get; }
        public TorqueControllerList([FromKeyedServices("TorqueControllerModbusCommunication")] IModbusCommunication modbusCommunication)
        {
            _modbusCommunication = modbusCommunication;

            var torqueCtlList = Enum.GetNames(typeof(ESpeedController)).ToList();
            var torqueCtlIndex = (int[])Enum.GetValues(typeof(ESpeedController));

            All = new List<ITorqueController>();

            for (int i = 0; i < torqueCtlList.Count; i++)
            {
                All.Add(new DX3000TorqueController(torqueCtlIndex[i], torqueCtlList[i]) { ModbusCommunication = _modbusCommunication });
            }
        }
    }
}
