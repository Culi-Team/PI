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

        public IMotion InCassetteTAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.InCassetteTAxis);
        public IMotion OutCassetteTAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.OutCassetteTAxis);
        public IMotion FixtureTransferYAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.FixtureTransferYAxis);
        public IMotion DetachGlassZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.DetachGlassZAxis);
        public IMotion ShuttleTransferXAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.ShuttleTransferXAxis);
        public IMotion TransferInShuttleRYAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.TransferInShuttleRYAxis);
        public IMotion TransferInShuttleLYAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.TransferInShuttleLYAxis);
        public IMotion TransferInShuttleRZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.TransferInShuttleRZAxis);
        public IMotion TransferInShuttleLZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.TransferInShuttleLZAxis);
        public IMotion GlassTransferYAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.GlassTransferYAxis);
        public IMotion GlassTransferZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.GlassTransferZAxis);
        public IMotion InShuttleRTAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.InShuttleRTAxis);
        public IMotion OutShuttleRTAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.OutShuttleRTAxis);
        public IMotion InShuttleLTAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.InShuttleLTAxis);
        public IMotion OutShuttleLTAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.OutShuttleLTAxis);
        public IMotion WETCleanRFeedingAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.WETCleanRFeedingAxis);
        public IMotion WETCleanLFeedingAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.WETCleanLFeedingAxis);
        public IMotion AFCleanRFeedingAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.AFCleanRFeedingAxis);
        public IMotion AFCleanLFeedingAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.AFCleanLFeedingAxis);
        public IMotion TransferRotationRZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.TransferRotationRZAxis);
        public IMotion TransferRotationLZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.TransferRotationLZAxis);
        public IMotion GlassUnloadRYAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.GlassUnloadRYAxis);
        public IMotion GlassUnloadLYAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.GlassUnloadLYAxis);
        public IMotion GlassUnloadRZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.GlassUnloadRZAxis);
        public IMotion GlassUnloadLZAxis => All.FirstOrDefault(m => m.Id == (int)EMotionInovance.GlassUnloadLZAxis);

    }
}
