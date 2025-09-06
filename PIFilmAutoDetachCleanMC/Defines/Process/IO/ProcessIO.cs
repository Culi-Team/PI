using EQX.Core.InOut;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class ProcessIO
    {
        private readonly IDInputDevice _inWorkConveyorInput;
        private readonly IDOutputDevice _inWorkConveyorOutput;
        private readonly IDInputDevice _outWorkConveyorInput;
        private readonly IDOutputDevice _outWorkConveyorOutput;
        private readonly IDInputDevice _robotLoadInput;
        private readonly IDOutputDevice _robotLoadOutput;
        private readonly IDInputDevice _vinylCleanInput;
        private readonly IDOutputDevice _vinylCleanOutput;

        public ProcessIO([FromKeyedServices("InWorkConveyorInput")] IDInputDevice inWorkConveyorInput,
                         [FromKeyedServices("InWorkConveyorOutput")] IDOutputDevice inWorkConveyorOutput,
                         [FromKeyedServices("OutWorkConveyorInput")] IDInputDevice outWorkConveyorInput,
                         [FromKeyedServices("OutWorkConveyorOutput")] IDOutputDevice outWorkConveyorOutput,
                         [FromKeyedServices("RobotLoadInput")]IDInputDevice robotLoadInput,
                         [FromKeyedServices("RobotLoadOutput")] IDOutputDevice robotLoadOutput,
                         [FromKeyedServices("VinylCleanInput")] IDInputDevice vinylCleanInput,
                         [FromKeyedServices("VinylCleanOutput")] IDOutputDevice vinylCleanOutput)
        {
            _inWorkConveyorInput = inWorkConveyorInput;
            _inWorkConveyorOutput = inWorkConveyorOutput;
            _outWorkConveyorInput = outWorkConveyorInput;
            _outWorkConveyorOutput = outWorkConveyorOutput;
            _robotLoadInput = robotLoadInput;
            _robotLoadOutput = robotLoadOutput;
            _vinylCleanInput = vinylCleanInput;
            _vinylCleanOutput = vinylCleanOutput;
        }

        public void Initialize()
        {
            _inWorkConveyorInput.Initialize();
            _inWorkConveyorOutput.Initialize();
            _outWorkConveyorInput.Initialize();
            _outWorkConveyorOutput.Initialize();
            _robotLoadInput.Initialize();
            _robotLoadOutput.Initialize();
            _vinylCleanInput.Initialize();
            _vinylCleanOutput.Initialize();
        }

        public void Mappings()
        {

        }
    }
}
