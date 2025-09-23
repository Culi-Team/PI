using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PIFilmAutoDetachCleanMC.Language
{
    public enum SupportedLanguage
    {
        English,
        Korean,
        Vietnamese,
        Chinese
    }

    public class LanguageService : ObservableObject
    {
        private SupportedLanguage _currentLanguage;
        private readonly Dictionary<SupportedLanguage, string> _languageResources;

        public SupportedLanguage CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnPropertyChanged(nameof(CurrentLanguage));
                    OnPropertyChanged(nameof(CurrentLanguageDisplayName));
                    SwitchLanguage(value);
                }
            }
        }

        public string CurrentLanguageDisplayName => GetLanguageDisplayName(_currentLanguage);

        public List<SupportedLanguage> AvailableLanguages => Enum.GetValues<SupportedLanguage>().ToList();

        public LanguageService()
        {
            _languageResources = new Dictionary<SupportedLanguage, string>
            {
                { SupportedLanguage.English, "/EQX.UI;component/Resources/Language/EnglishLanguage.xaml" },
                { SupportedLanguage.Korean, "/EQX.UI;component/Resources/Language/KoreanLanguage.xaml" },
                { SupportedLanguage.Vietnamese, "/EQX.UI;component/Resources/Language/VietnameseLanguage.xaml" },
                { SupportedLanguage.Chinese, "/EQX.UI;component/Resources/Language/ChineseLanguage.xaml" }
            };

            // Set default language to English
            _currentLanguage = SupportedLanguage.English;
        }

        public void SwitchLanguage(SupportedLanguage language)
        {
            try
            {
                var app = Application.Current;
                if (app?.Resources?.MergedDictionaries != null)
                {
                    // Find and remove existing language dictionaries
                    var dictionariesToRemove = app.Resources.MergedDictionaries
                        .Where(dict => dict.Source?.ToString().Contains("Language") == true)
                        .ToList();

                    foreach (var dict in dictionariesToRemove)
                    {
                        app.Resources.MergedDictionaries.Remove(dict);
                    }

                    // Add new language dictionary
                    var newLanguageDict = new ResourceDictionary
                    {
                        Source = new Uri(_languageResources[language], UriKind.Relative)
                    };

                    app.Resources.MergedDictionaries.Add(newLanguageDict);

                    // Set culture for number and date formatting
                    var culture = GetCultureInfo(language);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                    FrameworkElement.LanguageProperty.OverrideMetadata(
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
                }
            }
            catch (Exception ex)
            {
                // Log error if logging is available
                System.Diagnostics.Debug.WriteLine($"Error switching language: {ex.Message}");
            }
        }

        public string GetLanguageDisplayName(SupportedLanguage language)
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

        private CultureInfo GetCultureInfo(SupportedLanguage language)
        {
            return language switch
            {
                SupportedLanguage.English => new CultureInfo("en-US"),
                SupportedLanguage.Korean => new CultureInfo("ko-KR"),
                SupportedLanguage.Vietnamese => new CultureInfo("vi-VN"),
                SupportedLanguage.Chinese => new CultureInfo("zh-CN"),
                _ => new CultureInfo("en-US")
            };
        }
    }
}
