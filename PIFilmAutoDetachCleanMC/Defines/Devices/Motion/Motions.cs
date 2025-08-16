using EQX.Core.Motion;
using EQX.Motion;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Motions : MotionList<EMotion>
    {
        public Motions(IMotionFactory<IMotion> motionFactory, List<IMotionParameter> parameterList)
            : base(motionFactory, parameterList)
        {
        }

        public IMotion XAxis => All.First(m => m.Id == (int)EMotion.XAxis);
        public IMotion ZAxis => All.First(m => m.Id == (int)EMotion.XAxis);
        public IMotion Y1Axis => All.First(m => m.Id == (int)EMotion.Y1Axis);
        public IMotion Y2Axis => All.First(m => m.Id == (int)EMotion.Y2Axis);
        public IMotion TAxis => All.First(m => m.Id == (int)EMotion.TAxis);
    }
}
