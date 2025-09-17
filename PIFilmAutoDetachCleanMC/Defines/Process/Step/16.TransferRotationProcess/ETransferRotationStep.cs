using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferRotationStep
    {
        Start,
        TransferCyl_Forward,
        TransferCyl_Forward_Wait,
        ZAxis_Move_TransferBeforeRotatePosition,
        ZAxis_Move_TransferBeforeRotatePosition_Wait,
        GlassRotVac_On,
        GlassVac1_Off,
        GlassRotVac_On_Check,
        ZAxis_Move_ReadyPosition,
        ZAxis_Move_ReadyPosition_Wait,
        Cyl_Rotate_180D,
        Cyl_Rotate_180D_Wait,

        Cyl_Down,
        Cyl_Down_Wait,

        ZAxis_Move_TransferAfterRotatePositon,
        ZAxis_Move_TransferAfterRotatePositon_Wait,

        GlassVac2_On,
        GlassRotVac_Off,

        GlassVac2_On_Check,

        Cyl_Backward,
        Cyl_Backward_Wait,

        End

    }
}
