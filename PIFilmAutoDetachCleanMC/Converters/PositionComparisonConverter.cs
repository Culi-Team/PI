using System;
using System.Globalization;
using System.Windows.Data;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class PositionComparisonConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null)
                return false;

            if (double.TryParse(values[0].ToString(), out double actualPosition) && 
                double.TryParse(values[1].ToString(), out double targetPosition))
            {
                // So sánh với độ chính xác 0.001 để tránh lỗi floating point
                return Math.Abs(actualPosition - targetPosition) < 0.001;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
