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
            #region InWorkConveyor Process & Robot Process Mapping
            _inWorkConveyorInput.Inputs.First(i => i.Id == (int)EInWorkConveyorProcessInput.ROBOT_PICK_IN_CST_DONE)
                .Mapping(_robotLoadOutput.Outputs.First(o => o.Id == (int)ERobotLoadProcessOutput.ROBOT_PICK_IN_CST_DONE));

            _robotLoadInput.Inputs.First(i => i.Id == (int)ERobotLoadProcessInput.IN_CST_READY)
                .Mapping(_inWorkConveyorOutput.Outputs.First (o => o.Id == (int)EInWorkConveyorProcessOutput.IN_CST_READY));
            #endregion

            #region OutWorkConveyorProcess & Robot Process Mapping
            _outWorkConveyorInput.Inputs.First(i => i.Id == (int)EOutWorkConveyorProcessInput.ROBOT_PLACE_OUT_CST_DONE)
                .Mapping(_robotLoadOutput.Outputs.First(o => o.Id == (int)ERobotLoadProcessOutput.ROBOT_PLACE_OUT_CST_DONE));

            _robotLoadInput.Inputs.First (i => i.Id == (int)ERobotLoadProcessInput.OUT_CST_READY)
                .Mapping(_outWorkConveyorOutput.Outputs.First(o => o.Id == (int)EOutWorkConveyorProcessOutput.OUT_CST_READY));
            #endregion

            #region Robot Process & Vinyl Clean Process Mapping
            _robotLoadInput.Inputs.First(i => i.Id == (int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_FIXTURE)
                .Mapping(_vinylCleanOutput.Outputs.First(o => o.Id == (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_FIXTURE));
            _robotLoadInput.Inputs.First(i => i.Id == (int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_UNLOAD)
                .Mapping(_vinylCleanOutput.Outputs.First(o => o.Id == (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_UNLOAD));

            _vinylCleanInput.Inputs.First(i => i.Id == (int)EVinylCleanProcessInput.VINYL_CLEAN_LOAD_DONE)
                .Mapping(_robotLoadOutput.Outputs.First(o => o.Id == (int)ERobotLoadProcessOutput.VINYL_CLEAN_LOAD_DONE));
            _vinylCleanInput.Inputs.First(i =>i.Id == (int)EVinylCleanProcessInput.VINYL_CLEAN_UNLOAD_DONE)
                .Mapping (_robotLoadOutput.Outputs.First(o => o.Id == (int)ERobotLoadProcessOutput.VINYL_CLEAN_UNLOAD_DONE));
            #endregion
        }
    }
}
