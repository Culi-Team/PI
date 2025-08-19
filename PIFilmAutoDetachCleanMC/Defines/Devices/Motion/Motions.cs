using EQX.Core.Motion;
using EQX.Motion;
using EQX.Motion.ByVendor.Inovance;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Motions : MotionList<EMotion>
    {
        public Motions(IMotionFactory<IMotion> motionFactory, List<MotionInovanceParameter> parameterList)
            : base(motionFactory, parameterList)
        {
        }

        public IMotion X1Axis => All.First(m => m.Id == (int)EMotion.X1Axis);
        public IMotion ZAxis => All.First(m => m.Id == (int)EMotion.ZAxis);
        public IMotion Y1Axis => All.First(m => m.Id == (int)EMotion.Y1Axis);
    }
}
