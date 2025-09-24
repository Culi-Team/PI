using EQX.Core.Common;
using EQX.UI.Converters;
using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Services
{
    public class WarningService : IAlertService
    {
        public WarningService()
        {
            ChangeCulture("English");
        }

        public AlertModel GetById(int id)
        {
            AlertModel alertModel = alertModels.FirstOrDefault(t => t.Id == id);
            return alertModel;
        }

        public void ChangeCulture(string culture)
        {
            string filePath = @$"D:\\PIFilmAutoDetachCleanMC\\Alert\\Warning\\data_{culture}.json";
            if (!File.Exists(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            string content = File.ReadAllText(filePath);
            alertModels = JsonSerializer.Deserialize<List<AlertModel>>(content) ?? new List<AlertModel>();

            SyncWithEnum();
        }

        private List<AlertModel> alertModels;
        private readonly List<string> supportedCultures = new List<string> { "English", "Vietnamese" };

        private void SyncWithEnum()
        {
            var enumValues = Enum.GetValues(typeof(EWarning))
                                 .Cast<EWarning>()
                                 .ToList();

            foreach (var culture in supportedCultures)
            {
                string filePath = @$"D:\\PIFilmAutoDetachCleanMC\\Alert\\Warning\\data_{culture}.json";
                List<AlertModel> cultureSpecificAlerts;

                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    cultureSpecificAlerts = JsonSerializer.Deserialize<List<AlertModel>>(content) ?? new List<AlertModel>();
                }
                else
                {
                    cultureSpecificAlerts = new List<AlertModel>();
                }

                var updatedModels = new List<AlertModel>();

                for (int i = 0; i < enumValues.Count; i++)
                {
                    var enumValue = enumValues[i];
                    var enumId = (int)enumValue;

                    var existingModel = cultureSpecificAlerts.FirstOrDefault(t => t.Id == enumId);

                    if (existingModel != null)
                    {
                        existingModel.Message = enumValue.ToString();
                        updatedModels.Add(existingModel);
                    }
                    else
                    {
                        updatedModels.Add(new AlertModel
                        {
                            Id = enumId,
                            Message = enumValue.ToString(),
                            AlertOverviewSource = "/PIFilmAutoDetachCleanMC;component/Resources/Images/PictureName.jpg",
                            AlertOverviewHighlightRectangle = new Rectangle(0, 0, 0, 0),
                            AlertDetailviewSource = "/PIFilmAutoDetachCleanMC;component/Resources/Images/PictureName.jpg",
                            AlertDetailviewHighlightRectangle = new Rectangle(0, 0, 0, 0),
                            TroubleshootingSteps = new List<string> { "Fix problem" }
                        });
                    }
                }

                cultureSpecificAlerts = updatedModels;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters = { new EQX.UI.Converters.RectangleConverter() }
                };

                string updatedContent = JsonSerializer.Serialize(cultureSpecificAlerts, options);
                File.WriteAllText(filePath, updatedContent);
            }
        }
    }
}
