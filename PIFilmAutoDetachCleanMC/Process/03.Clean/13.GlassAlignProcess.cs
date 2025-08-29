using EQX.Core.InOut;
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
    public class GlassAlignProcess : ProcessBase<ESequence>
    {
        private EPort port => Name == EProcess.GlassAlignLeft.ToString() ? EPort.Left : EPort.Right;
        private readonly Devices _devices;

        private IDOutput alignStageVac1 => port == EPort.Left ? _devices.Outputs.AlignStageLVac1OnOff : _devices.Outputs.AlignStageRVac1OnOff;
        private IDOutput alignStageVac2 => port == EPort.Left ? _devices.Outputs.AlignStageLVac2OnOff : _devices.Outputs.AlignStageRVac2OnOff;
        private IDOutput alignStageVac3 => port == EPort.Left ? _devices.Outputs.AlignStageLVac3OnOff : _devices.Outputs.AlignStageRVac3OnOff;

        private ICylinder alignCyl1 => port == EPort.Left ? _devices.Cylinders.AlignStageL1AlignUnalign : _devices.Cylinders.AlignStageR1AlignUnalign;
        private ICylinder alignCyl2 => port == EPort.Left ? _devices.Cylinders.AlignStageL2AlignUnalign : _devices.Cylinders.AlignStageR2AlignUnalign;
        private ICylinder alignCyl3 => port == EPort.Left ? _devices.Cylinders.AlignStageL3AlignUnalign : _devices.Cylinders.AlignStageR3AlignUnalign;

        private bool isGlass1Detect => port == EPort.Left ? _devices.Inputs.AlignStageRGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool isGlass2Detect => port == EPort.Left ? _devices.Inputs.AlignStageRGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool isGlass3Detect => port == EPort.Left ? _devices.Inputs.AlignStageRGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;

        private bool isAlign => alignCyl1.IsForward && alignCyl2.IsForward && alignCyl3.IsForward;
        private bool isUnalign => alignCyl1.IsBackward && alignCyl2.IsBackward && alignCyl3.IsBackward;

        private void AlignUnAlign(bool bAlignUnalign)
        {
            if(bAlignUnalign)
            {
                alignCyl1.Forward();
                alignCyl2.Forward();
                alignCyl3.Forward();
            }
            else
            {
                alignCyl1.Backward();
                alignCyl2.Backward();
                alignCyl3.Backward();
            }
        }

        public GlassAlignProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
