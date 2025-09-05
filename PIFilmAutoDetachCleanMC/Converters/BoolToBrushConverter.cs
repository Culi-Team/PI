using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isActive = (bool)value;
            return isActive ? Brushes.Lime : Brushes.Tomato; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
