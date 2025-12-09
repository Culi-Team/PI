using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Services.Validation;

namespace PIFilmAutoDetachCleanMC.Services.AlertServices
{
    public abstract class AlertServiceBase : IAlertService
    {
        private readonly AlertDataLoader _dataLoader;
        private List<AlertModel> _alertModels = new();

        protected AlertServiceBase(string alertFolder)
        {
            _dataLoader = new AlertDataLoader(alertFolder, SupportedCulturesDefaults);
            CurrentCulture = SupportedCulturesDefaults.First();
            ChangeCulture(CurrentCulture);
        }

        public AlertModel GetById(int id)
        {
            return _alertModels.FirstOrDefault(t => t.Id == id) ?? new AlertModel
            {
                Id = id,
                Message = id.ToString(),
                AlertOverviewSource = DefaultImagePath,
                AlertDetailviewSource = DefaultImagePath,
                TroubleshootingSteps = new List<string> { "No troubleshooting content" }
            };
        }

        public void ChangeCulture(string culture)
        {
            CurrentCulture = _dataLoader.NormalizeCulture(culture);
            _alertModels = _dataLoader.Load(CurrentCulture, GetEnumMap(), DefaultImagePath);
        }

        public IReadOnlyCollection<string> SupportedCultures => _dataLoader.SupportedCultures;

        public string CurrentCulture { get; private set; }

        public AlertValidationResult ValidateCurrentData()
        {
            return AlertDataValidator.Validate(_alertModels);
        }

        protected abstract string DefaultImagePath { get; }

        protected abstract IEnumerable<(int Id, string Name)> GetEnumMap();

        private static readonly IReadOnlyCollection<string> SupportedCulturesDefaults = new List<string>
        {
            "English",
            "Vietnamese"
        };

        protected static string ResolveAlertFolder(string relativeFolder)
        {
            var baseDir = AppContext.BaseDirectory;
            var current = new DirectoryInfo(baseDir);

            for (int i = 0; i < 6 && current != null; i++)
            {
                var candidate = Path.Combine(current.FullName, relativeFolder);
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            var fallback = Path.Combine(baseDir, relativeFolder);
            Directory.CreateDirectory(fallback);
            return fallback;
        }
    }
}
