using EQX.Core.Motion;
using EQX.Motion;
using EQX.Motion.ByVendor.Inovance;
using Microsoft.Extensions.DependencyInjection;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Motions
    {
        public Motions([FromKeyedServices("InovanceMaster#1")] IMotionMaster inovanceMaster,
            [FromKeyedServices("AjinMaster#1")] IMotionMaster ajinMaster,
            [FromKeyedServices("InovanceMotionFactory")] IMotionFactory<IMotion> inovanceFactory,
            [FromKeyedServices("MotionInovanceParameters")] List<IMotionParameter> inovanceParameters,
            [FromKeyedServices("AjinMotionFactory")] IMotionFactory<IMotion> ajinFactory,
            [FromKeyedServices("MotionAjinParameters")] List<IMotionParameter> ajinParameters)
        {
            InovanceMaster = inovanceMaster;
            AjinMaster = ajinMaster;

            InovanceMotions = new MotionList<EMotionInovance>(inovanceFactory, inovanceParameters);
            AjinMotions = new MotionList<EMotionAjin>(ajinFactory, ajinParameters);

            All = InovanceMotions.All.Concat(AjinMotions.All).ToList();
        }

        #region Publics
        public readonly MotionList<EMotionInovance> InovanceMotions;
        public readonly MotionList<EMotionAjin> AjinMotions;

        public List<IMotion> All { get; }
        public IMotionMaster InovanceMaster { get; }
        public IMotionMaster AjinMaster { get; }
        #endregion

        #region INOVANCE MOTIONS
        public IMotion InCassetteTAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.InCassetteTAxis);
        public IMotion OutCassetteTAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.OutCassetteTAxis);
        public IMotion FixtureTransferYAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.FixtureTransferYAxis);
        public IMotion DetachGlassZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.DetachGlassZAxis);
        public IMotion ShuttleTransferXAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.ShuttleTransferXAxis);
        public IMotion TransferInShuttleRYAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleRYAxis);
        public IMotion TransferInShuttleLYAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleLYAxis);
        public IMotion TransferInShuttleRZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleRZAxis);
        public IMotion TransferInShuttleLZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.TransferInShuttleLZAxis);
        public IMotion GlassTransferYAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.GlassTransferYAxis);
        public IMotion GlassTransferZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.GlassTransferZAxis);
        public IMotion InShuttleRTAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.InShuttleRTAxis);
        public IMotion OutShuttleRTAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.OutShuttleRTAxis);
        public IMotion InShuttleLTAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.InShuttleLTAxis);
        public IMotion OutShuttleLTAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.OutShuttleLTAxis);
        public IMotion WETCleanRFeedingAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.WETCleanRFeedingAxis);
        public IMotion WETCleanLFeedingAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.WETCleanLFeedingAxis);
        public IMotion AFCleanRFeedingAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.AFCleanRFeedingAxis);
        public IMotion AFCleanLFeedingAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.AFCleanLFeedingAxis);
        public IMotion TransferRotationRZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.TransferRotationRZAxis);
        public IMotion TransferRotationLZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.TransferRotationLZAxis);
        public IMotion GlassUnloadRYAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.GlassUnloadRYAxis);
        public IMotion GlassUnloadLYAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.GlassUnloadLYAxis);
        public IMotion GlassUnloadRZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.GlassUnloadRZAxis);
        public IMotion GlassUnloadLZAxis => InovanceMotions.All.First(m => m.Id == (int)EMotionInovance.GlassUnloadLZAxis);
        #endregion

        #region AJINEXTEK MOTIONS
        public IMotion ShuttleTransferZAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.ShuttleTransferZAxis);
        public IMotion InShuttleRXAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.InShuttleRXAxis);
        public IMotion OutShuttleRXAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.OutShuttleRXAxis);
        public IMotion InShuttleLXAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.InShuttleLXAxis);
        public IMotion OutShuttleLXAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.OutShuttleLXAxis);
        public IMotion InShuttleRYAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.InShuttleRYAxis);
        public IMotion OutShuttleRYAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.OutShuttleRYAxis);
        public IMotion InShuttleLYAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.InShuttleLYAxis);
        public IMotion OutShuttleLYAxis => AjinMotions.All.First(m => m.Id == (int)EMotionAjin.OutShuttleLYAxis);

        public void CleanHorizontal(EClean cleanUnit, double positionX, double positionY, int count, double vel = 500, double acc = 2000, double dec = 2000)
        {
            switch (cleanUnit)
            {
                case EClean.WETCleanLeft:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.InShuttleLXAxis, (int)EMotionAjin.InShuttleLYAxis });
                    break;
                case EClean.WETCleanRight:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.InShuttleRXAxis, (int)EMotionAjin.InShuttleRYAxis });
                    break;
                case EClean.AFCleanLeft:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.OutShuttleLXAxis, (int)EMotionAjin.OutShuttleLYAxis });
                    break;
                case EClean.AFCleanRight:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.OutShuttleRXAxis, (int)EMotionAjin.OutShuttleRYAxis });
                    break;
            }

            AXM.AxmContiBeginNode((int)cleanUnit);

            double YStep = 5.0 / count;

            double x = 0;
            double y = positionY;
            bool toRight = true;

            double[] startPos = { positionX, positionY };
            AXM.AxmLineMove((int)cleanUnit, startPos, vel, acc, dec);

            while (y <= positionY + 5)
            {
                x = toRight ? positionX - 150 : positionX;
                double[] pos1 = { x, y };

                AXM.AxmLineMove((int)cleanUnit, pos1, vel, acc, dec);

                y += YStep;

                if (y <= positionY + 5)
                {
                    double[] pos2 = { x, y };
                    AXM.AxmLineMove((int)cleanUnit, pos2, vel, acc, dec);
                }

                toRight = !toRight;
            }
            AXM.AxmContiEndNode((int)cleanUnit);
            AXM.AxmContiStart((int)cleanUnit, 0, 0);
        }

        public bool IsContiMotioning(EClean cleanUnit)
        {
            uint uInMotion = 1;
            AXM.AxmContiIsMotion((int)cleanUnit, ref uInMotion);

            return uInMotion == 1;
        }

        public void CleanVertical(EClean cleanUnit, double positionX, double positionY, int count, double vel = 500, double acc = 2000, double dec = 2000)
        {
            switch (cleanUnit)
            {
                case EClean.WETCleanLeft:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.InShuttleLXAxis, (int)EMotionAjin.InShuttleLYAxis });
                    break;
                case EClean.WETCleanRight:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.InShuttleRXAxis, (int)EMotionAjin.InShuttleRYAxis });
                    break;
                case EClean.AFCleanLeft:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.OutShuttleLXAxis, (int)EMotionAjin.OutShuttleLYAxis });
                    break;
                case EClean.AFCleanRight:
                    AXM.AxmContiSetAxisMap((int)cleanUnit, 2, new int[] { (int)EMotionAjin.OutShuttleRXAxis, (int)EMotionAjin.OutShuttleRYAxis });
                    break;
            }

            AXM.AxmContiBeginNode((int)cleanUnit);

            double YStep = 100.0 / count;

            double x = 0;
            double y = positionY;
            bool toRight = true;

            double[] startPos = { positionX, positionY };
            AXM.AxmLineMove((int)cleanUnit, startPos, vel, acc, dec);

            while (y <= positionY + 100)
            {
                x = toRight ? positionX - 60 : positionX;
                double[] pos1 = { x, y };

                AXM.AxmLineMove((int)cleanUnit, pos1, vel, acc, dec);

                y += YStep;

                if (y <= positionY + 100)
                {
                    double[] pos2 = { x, y };
                    AXM.AxmLineMove((int)cleanUnit, pos2, vel, acc, dec);
                }

                toRight = !toRight;
            }
            AXM.AxmContiEndNode((int)cleanUnit);
            AXM.AxmContiStart((int)cleanUnit, 0, 0);
        }
        #endregion
    }
}