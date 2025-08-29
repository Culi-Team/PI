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

        public IMotion InCassetteTAxis => All.First(m => m.Id == (int)EMotionInovance.InCassetteTAxis);
        public IMotion OutCassetteTAxis => All.First(m => m.Id == (int)EMotionInovance.OutCassetteTAxis);
        public IMotion FixtureTransferYAxis => All.First(m => m.Id == (int)EMotionInovance.FixtureTransferYAxis);
        public IMotion DetachGlassZAxis => All.First(m => m.Id == (int)EMotionInovance.DetachGlassZAxis);
        public IMotion ShuttleTransferXAxis => All.First(m => m.Id == (int)EMotionInovance.ShuttleTransferXAxis);
        public IMotion TransferInShuttleRYAxis => All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleRYAxis);
        public IMotion TransferInShuttleLYAxis => All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleLYAxis);
        public IMotion TransferInShuttleRZAxis => All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleRZAxis);
        public IMotion TransferInShuttleLZAxis => All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleLZAxis);
        public IMotion GlassTransferYAxis => All.First(m => m.Id == (int)EMotionInovance.GlassTransferYAxis);
        public IMotion GlassTransferZAxis => All.First(m => m.Id == (int)EMotionInovance.GlassTransferZAxis);
        public IMotion InShuttleRTAxis => All.First(m => m.Id == (int)EMotionInovance.InShuttleRTAxis);
        public IMotion OutShuttleRTAxis => All.First(m => m.Id == (int)EMotionInovance.OutShuttleRTAxis);
        public IMotion InShuttleLTAxis => All.First(m => m.Id == (int)EMotionInovance.InShuttleLTAxis);
        public IMotion OutShuttleLTAxis => All.First(m => m.Id == (int)EMotionInovance.OutShuttleLTAxis);
        public IMotion WETCleanRFeedingAxis => All.First(m => m.Id == (int)EMotionInovance.WETCleanRFeedingAxis);
        public IMotion WETCleanLFeedingAxis => All.First(m => m.Id == (int)EMotionInovance.WETCleanLFeedingAxis);
        public IMotion AFCleanRFeedingAxis => All.First(m => m.Id == (int)EMotionInovance.AFCleanRFeedingAxis);
        public IMotion AFCleanLFeedingAxis => All.First(m => m.Id == (int)EMotionInovance.AFCleanLFeedingAxis);
        public IMotion TransferRotationRZAxis => All.First(m => m.Id == (int)EMotionInovance.TransferRotationRZAxis);
        public IMotion TransferRotationLZAxis => All.First(m => m.Id == (int)EMotionInovance.TransferRotationLZAxis);
        public IMotion GlassUnloadRYAxis => All.First(m => m.Id == (int)EMotionInovance.GlassUnloadRYAxis);
        public IMotion GlassUnloadLYAxis => All.First(m => m.Id == (int)EMotionInovance.GlassUnloadLYAxis);
        public IMotion GlassUnloadRZAxis => All.First(m => m.Id == (int)EMotionInovance.GlassUnloadRZAxis);
        public IMotion GlassUnloadLZAxis => All.First(m => m.Id == (int)EMotionInovance.GlassUnloadLZAxis);

    }
}
