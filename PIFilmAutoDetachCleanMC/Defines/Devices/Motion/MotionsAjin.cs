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

        public void CleanHorizontal(EClean cleanUnit, double centerX, double centerY, double radiusX, double radiusY, int count, double vel = 500, double acc = 2000, double dec = 2000)
        {
            int pointCount = 100;
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
            for (int k = 0; k < count; k++)
            {
                for (int i = 1; i <= pointCount; i++)
                {
                    double theta = 2 * Math.PI * i / pointCount;
                    double x = centerX + radiusX * Math.Cos(theta);
                    double y = centerY + radiusY * Math.Sin(theta);
                    double[] pos = { x, y };
                    AXM.AxmLineMove((int)cleanUnit, pos, vel, acc, dec);
                }
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
    }
}
