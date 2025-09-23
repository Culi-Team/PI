using PIFilmAutoDetachCleanMC.Language;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class LanguageToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SupportedLanguage language)
            {
                return language switch
                {
                    SupportedLanguage.English => "English",
                    SupportedLanguage.Korean => "한국어",
                    SupportedLanguage.Vietnamese => "Tiếng Việt",
                    SupportedLanguage.Chinese => "中文",
                    _ => language.ToString()
                };
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
