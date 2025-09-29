using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;

namespace PIFilmAutoDetachCleanMC.Helpers
{
    public static class DataHelper
    {
        public static void Copy(this CCountData countData, CCountData newCountData)
        {
            countData.Left = newCountData.Left;
            countData.Right = newCountData.Right;

        }

        public static void Copy(this CTaktTime taktTime, CTaktTime newTaktTime)
        {
            taktTime.Maximum = newTaktTime.Maximum;
            taktTime.CycleCurrent = newTaktTime.CycleCurrent;
            taktTime.Total = newTaktTime.Total;
        }
    }
}
