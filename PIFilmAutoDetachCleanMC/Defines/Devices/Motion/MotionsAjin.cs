using EQX.Core.Motion;
using EQX.Motion;
using EQX.Motion.ByVendor.Inovance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class MotionsAjin : MotionList<EMotionAjin>
    {
        public MotionsAjin(IMotionFactory<IMotion> motionFactory,
                           List<IMotionParameter> parameterList)
            : base(motionFactory, parameterList)
        {
            
        }
    }
}
