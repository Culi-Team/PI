using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class RollerButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected && parameter is string buttonType)
            {
                if (isConnected)
                {
                    // Khi đã kết nối, button có thể sử dụng được
                    return buttonType == "Start" ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                }
                else
                {
                    // Khi chưa kết nối, button bị vô hiệu hóa
                    return new SolidColorBrush(Colors.Gray);
                }
            }
            
            // Mặc định trả về màu xám
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
