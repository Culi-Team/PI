using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferRotationOriginStep
    {
        Start,
        ZAxis_Origin,
        ZAxis_Origin_Wait,
        TransferRotation_Cyl_Backward,
        TransferRotation_Cyl_Backward_Wait,
        TransferRotation_Cyl_Unclamp,
        TransferRotation_Cyl_Unclamp_Wait,
        TransferRotation_0Degree,
        TransferRotation_0Degree_Wait,
        End
    }
}
