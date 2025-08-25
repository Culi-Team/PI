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
        private readonly IModbusCommunication _modbusCommunication;

        public List<ISpeedController> All { get; }
        public SpeedControllerList([FromKeyedServices("RollerModbusCommunication")] IModbusCommunication modbusCommunication)
        {
            _modbusCommunication = modbusCommunication;
            var speedCtlList = Enum.GetNames(typeof(ESpeedController)).ToList();
            var speedCtlIndex = (int[])Enum.GetValues(typeof(ESpeedController));

            All = new List<ISpeedController>();

            for (int i = 0; i < speedCtlList.Count; i++)
            {
                All.Add(new SD201SSpeedController(speedCtlIndex[i], speedCtlList[i]) { ModbusCommunication = _modbusCommunication });
            }
        }
    }
}
