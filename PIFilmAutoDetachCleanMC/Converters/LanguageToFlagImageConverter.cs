using PIFilmAutoDetachCleanMC.Language;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class LanguageToFlagImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SupportedLanguage language)
            {
                var resourceKey = language switch
                {
                    SupportedLanguage.English => "USA_Flag",
                    SupportedLanguage.Korean => "South_Korea_Flag",
                    SupportedLanguage.Vietnamese => "Vietnam_Flag",
                    SupportedLanguage.Chinese => "China_Flag",
                    _ => "USA_Flag"
                };

                return Application.Current.TryFindResource(resourceKey) as ImageSource;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
