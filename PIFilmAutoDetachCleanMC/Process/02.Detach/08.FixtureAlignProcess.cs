using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class FixtureAlignProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

        #region Constructor
        public FixtureAlignProcess(Devices devices)
        {
            _devices = devices;
        }
        #endregion
    }
}
