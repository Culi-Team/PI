using EQX.Core.Motion;
using EQX.Motion;
using EQX.Motion.ByVendor.Inovance;
using Microsoft.Extensions.DependencyInjection;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class MotionsInovance : MotionList<EMotionInovance>
    {
        public MotionsInovance(IMotionFactory<IMotion> motionFactory,
            List<IMotionParameter> parameterList,
            [FromKeyedServices("InovanceController#1")] IMotionController motionControllerInovance)
            : base(motionFactory, parameterList)
        {
            MotionControllerInovance = motionControllerInovance;
        }

        public IMotionController MotionControllerInovance { get; }
    }
}
