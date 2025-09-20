using EQX.Core.Device.SyringePump;
using EQX.Device.SyringePump;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices
{
    public class SyringePumps
    {
        public SyringePumps([FromKeyedServices("WETCleanLeftSyringePump")] ISyringePump wetCleanLeftSyringePump,
            [FromKeyedServices("WETCleanRightSyringePump")] ISyringePump wetCleanRightSyringePump,
            [FromKeyedServices("AFCleanLeftSyringePump")] ISyringePump afCleanLeftSyringePump,
            [FromKeyedServices("AFCleanRightSyringePump")] ISyringePump afCleanRightSyringePump)
        {
            WetCleanLeftSyringePump = wetCleanLeftSyringePump;
            WetCleanRightSyringePump = wetCleanRightSyringePump;
            AfCleanLeftSyringePump = afCleanLeftSyringePump;
            AfCleanRightSyringePump = afCleanRightSyringePump;
        }

        public ISyringePump WetCleanLeftSyringePump { get; }
        public ISyringePump WetCleanRightSyringePump { get; }
        public ISyringePump AfCleanLeftSyringePump { get; }
        public ISyringePump AfCleanRightSyringePump { get; }
    }
}
