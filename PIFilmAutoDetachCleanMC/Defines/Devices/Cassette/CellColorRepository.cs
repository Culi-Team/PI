using EQX.UI.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class CellColorRepository : ICellColorRepository
    {
        public SolidColorBrush GetColorByStatus(object status)
        {
            switch ((ETrayCellStatus)status)
            {
                case ETrayCellStatus.Ready:
                    return new SolidColorBrush(Colors.MintCream);
                case ETrayCellStatus.Skip:
                    return new SolidColorBrush(Colors.Gray);
                case ETrayCellStatus.Working:
                    return new SolidColorBrush(Color.FromArgb(0x99, 0xff, 0xff, 0x00));
                case ETrayCellStatus.NeedConfirm:
                    return new SolidColorBrush(Colors.Goldenrod);
                case ETrayCellStatus.Done:
                    return new SolidColorBrush(Colors.Lime);
                case ETrayCellStatus.PickPlaceFail:
                    return new SolidColorBrush(Colors.Tomato);
                default:
                    return new SolidColorBrush(Colors.Black);
            }
        }
    }
}
