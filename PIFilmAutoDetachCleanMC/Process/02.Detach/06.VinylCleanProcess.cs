using EQX.Core.Motion;
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
    public class VinylCleanProcess : ProcessBase<ESequence>
    {
        private readonly IMotion _vinylCleanEncoder;

        public VinylCleanProcess([FromKeyedServices("VinylCleanEncoder")] IMotion vinylCleanEncoder)
        {
            _vinylCleanEncoder = vinylCleanEncoder;
        }
    }
}
