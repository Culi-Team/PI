using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class CylinderButtonBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bValue)
            {
                // True = ON (Green), False = OFF (Gray)
                return bValue ? Brushes.LimeGreen : Brushes.LightGray;
            }
            return Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
