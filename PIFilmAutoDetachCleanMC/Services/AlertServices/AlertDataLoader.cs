using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EQX.Core.Common;
using EQX.UI.Converters;

namespace PIFilmAutoDetachCleanMC.Services.AlertServices
{
    public class AlertDataLoader
    {
        private readonly string _alertFolder;
        private readonly IReadOnlyCollection<string> _supportedCultures;
        private readonly Dictionary<string, List<AlertModel>> _cache = new();

        public AlertDataLoader(string alertFolder, IReadOnlyCollection<string> supportedCultures)
        {
            _alertFolder = alertFolder;
            _supportedCultures = supportedCultures;
        }

        public IReadOnlyCollection<string> SupportedCultures => _supportedCultures;

        public string NormalizeCulture(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture)) return _supportedCultures.First();
            var matched = _supportedCultures.FirstOrDefault(c => c.Equals(culture, StringComparison.OrdinalIgnoreCase));
            return matched ?? _supportedCultures.First();
        }

        public List<AlertModel> Load(string culture, IEnumerable<(int Id, string Name)> enumMap, string defaultImagePath)
        {
            culture = NormalizeCulture(culture);
            if (_cache.TryGetValue(culture, out var cached))
            {
                return cached;
            }

            Directory.CreateDirectory(_alertFolder);

            var primaryData = LoadDataFile(culture, enumMap, defaultImagePath);
            var defaultCulture = _supportedCultures.First();
            var fallbackData = culture.Equals(defaultCulture, StringComparison.OrdinalIgnoreCase)
                ? primaryData
                : LoadDataFile(defaultCulture, enumMap, defaultImagePath);

            var merged = enumMap.Select(map =>
            {
                var primary = primaryData.FirstOrDefault(a => a.Id == map.Id);
                var fallback = fallbackData.FirstOrDefault(a => a.Id == map.Id);
                var source = primary ?? fallback ?? CreateDefaultAlert(map.Id, map.Name, defaultImagePath);

                var message = !string.IsNullOrWhiteSpace(primary?.Message)
                    ? primary!.Message
                    : (!string.IsNullOrWhiteSpace(fallback?.Message) ? fallback!.Message : map.Name);

                var troubleshooting = primary?.TroubleshootingSteps?.Any() == true
                    ? primary.TroubleshootingSteps
                    : (fallback?.TroubleshootingSteps?.Any() == true
                        ? fallback.TroubleshootingSteps
                        : new List<string> { "Fix problem" });

                return new AlertModel
                {
                    Id = map.Id,
                    Message = message,
                    AlertOverviewSource = string.IsNullOrWhiteSpace(primary?.AlertOverviewSource)
                        ? (fallback?.AlertOverviewSource ?? source.AlertOverviewSource)
                        : primary!.AlertOverviewSource,
                    AlertOverviewShapes = NormalizeShapes(primary?.AlertOverviewShapes ?? new List<AlertShapeModel>(), source.AlertOverviewHighlightRectangle, fallback?.AlertOverviewShapes),
                    AlertOverviewHighlightRectangle = source.AlertOverviewHighlightRectangle,
                    AlertDetailviewSource = string.IsNullOrWhiteSpace(primary?.AlertDetailviewSource)
                        ? (fallback?.AlertDetailviewSource ?? source.AlertDetailviewSource)
                        : primary!.AlertDetailviewSource,
                    AlertDetailviewShapes = NormalizeShapes(primary?.AlertDetailviewShapes ?? new List<AlertShapeModel>(), source.AlertDetailviewHighlightRectangle, fallback?.AlertDetailviewShapes),
                    AlertDetailviewHighlightRectangle = source.AlertDetailviewHighlightRectangle,
                    TroubleshootingSteps = troubleshooting
                };
            }).ToList();

            _cache[culture] = merged;
            return merged;
        }

        private List<AlertShapeModel> NormalizeShapes(List<AlertShapeModel> shapes, System.Drawing.Rectangle legacyRectangle, List<AlertShapeModel>? fallbackShapes = null)
        {
            if (shapes != null && shapes.Any())
            {
                return shapes;
            }

            if (fallbackShapes != null && fallbackShapes.Any())
            {
                return fallbackShapes;
            }

            if (legacyRectangle.Width > 0 && legacyRectangle.Height > 0)
            {
                return new List<AlertShapeModel>
                {
                    new AlertShapeModel
                    {
                        Type = AlertShapeType.Rectangle,
                        Left = legacyRectangle.Left,
                        Top = legacyRectangle.Top,
                        Width = legacyRectangle.Width,
                        Height = legacyRectangle.Height
                    }
                };
            }

            return new List<AlertShapeModel>();
        }

        private List<AlertModel> LoadDataFile(string culture, IEnumerable<(int Id, string Name)> enumMap, string defaultImagePath)
        {
            string dataPath = Path.Combine(_alertFolder, $"data_{culture}.json");
            List<AlertModel> alerts;

            if (File.Exists(dataPath))
            {
                string content = File.ReadAllText(dataPath);
                alerts = JsonSerializer.Deserialize<List<AlertModel>>(content, SerializerOptions)
                    ?? new List<AlertModel>();
            }
            else
            {
                alerts = new List<AlertModel>();
            }

            var updated = enumMap.Select(map =>
            {
                var existing = alerts.FirstOrDefault(a => a.Id == map.Id);
                if (existing != null)
                {
                    existing.Message = string.IsNullOrWhiteSpace(existing.Message) ? map.Name : existing.Message;
                    existing.TroubleshootingSteps ??= new List<string> { "Fix problem" };
                    existing.AlertOverviewShapes = NormalizeShapes(existing.AlertOverviewShapes, existing.AlertOverviewHighlightRectangle);
                    existing.AlertDetailviewShapes = NormalizeShapes(existing.AlertDetailviewShapes, existing.AlertDetailviewHighlightRectangle);
                    existing.AlertOverviewSource ??= defaultImagePath;
                    existing.AlertDetailviewSource ??= defaultImagePath;
                    return existing;
                }

                var @default = CreateDefaultAlert(map.Id, map.Name, defaultImagePath);
                @default.TroubleshootingSteps = new List<string> { "Fix problem" };
                return @default;
            }).ToList();

            File.WriteAllText(dataPath, JsonSerializer.Serialize(updated, SerializerOptions));
            return updated;
        }

        private AlertModel CreateDefaultAlert(int id, string name, string defaultImagePath)
        {
            return new AlertModel
            {
                Id = id,
                Message = name,
                AlertOverviewSource = defaultImagePath,
                AlertDetailviewSource = defaultImagePath,
                AlertOverviewHighlightRectangle = new System.Drawing.Rectangle(0, 0, 0, 0),
                AlertDetailviewHighlightRectangle = new System.Drawing.Rectangle(0, 0, 0, 0),
                AlertOverviewShapes = new List<AlertShapeModel>(),
                AlertDetailviewShapes = new List<AlertShapeModel>(),
                TroubleshootingSteps = new List<string> { "Fix problem" }
            };
        }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new RectangleConverter(),
                new JsonStringEnumConverter(allowIntegerValues: true)
            }
        };
    }
}
