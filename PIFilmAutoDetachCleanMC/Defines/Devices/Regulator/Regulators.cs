using EQX.Core.Device.Regulator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Regulator
{
    public class Regulators
    {
        public Regulators([FromKeyedServices("WETCleanLeft")]IRegulator wetCleanLRegulator,
                          [FromKeyedServices("WETCleanRight")]IRegulator wetCleanRRegulator,
                          [FromKeyedServices("AFCleanLeft")]IRegulator afCleanLRegulator,
                          [FromKeyedServices("AFCleanRight")]IRegulator afCleanRRegulator)
        {
            WetCleanLRegulator = wetCleanLRegulator;
            WetCleanRRegulator = wetCleanRRegulator;
            AfCleanLRegulator = afCleanLRegulator;
            AfCleanRRegulator = afCleanRRegulator;

            All = new[]
            {
                WetCleanLRegulator,
                WetCleanRRegulator,
                AfCleanLRegulator,
                AfCleanRRegulator
            };
        }

        public IRegulator WetCleanLRegulator { get; }
        public IRegulator WetCleanRRegulator { get; }
        public IRegulator AfCleanLRegulator { get; }
        public IRegulator AfCleanRRegulator { get; }
        public IReadOnlyList<IRegulator> All { get; }
    }
}
