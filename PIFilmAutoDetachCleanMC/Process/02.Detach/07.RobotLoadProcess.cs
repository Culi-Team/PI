using EQX.Core.Robot;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class RobotLoadProcess : ProcessBase<ESequence>
    {
        private readonly IRobot _robotLoad;

        public RobotLoadProcess([FromKeyedServices("RobotLoad")] IRobot robotLoad)
        {
            _robotLoad = robotLoad;
        }
    }
}
