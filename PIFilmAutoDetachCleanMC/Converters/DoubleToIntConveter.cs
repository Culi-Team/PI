using System.Globalization;
using System.Windows.Data;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class DoubleToIntConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return (int)Math.Round(d);
            if(value is int i)
                return i;
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
                return (double)i;
            if(value is double d)
                return d;
            return Binding.DoNothing;
        }
    }
}
