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

        public IMotion ShuttleTransferZAxis => All.First(m => m.Id == (int)EMotionAjin.ShuttleTransferZAxis);
        public IMotion InShuttleRXAxis => All.First(m => m.Id == (int)EMotionAjin.InShuttleRXAxis);
        public IMotion OutShuttleRXAxis => All.First(m => m.Id == (int)EMotionAjin.OutShuttleRXAxis);
        public IMotion InShuttleLXAxis => All.First(m => m.Id == (int)EMotionAjin.InShuttleLXAxis);
        public IMotion OutShuttleLXAxis => All.First(m => m.Id == (int)EMotionAjin.OutShuttleLXAxis);
        public IMotion InShuttleRYAxis => All.First(m => m.Id == (int)EMotionAjin.InShuttleRYAxis);
        public IMotion OutShuttleRYAxis => All.First(m => m.Id == (int)EMotionAjin.OutShuttleRYAxis);
        public IMotion InShuttleLYAxis => All.First(m => m.Id == (int)EMotionAjin.InShuttleLYAxis);
        public IMotion OutShuttleLYAxis => All.First(m => m.Id == (int)EMotionAjin.OutShuttleLYAxis);
    }
}
