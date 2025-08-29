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

        private ICylinder alignFixtureCyl => _devices.Cylinders.AlignFixtureBwFw;
        private bool isFixtureDetect => _devices.Inputs.AlignFixtureDetect.Value;
        private bool isFixtureTiltDetect => _devices.Inputs.AlignFixtureTiltDetect.Value;
        private bool isFixtureReverseDetect => _devices.Inputs.AlignFixtureReverseDetect.Value;

        #region Constructor
        public FixtureAlignProcess(Devices devices)
        {
            _devices = devices;
        }
        #endregion
    }
}
