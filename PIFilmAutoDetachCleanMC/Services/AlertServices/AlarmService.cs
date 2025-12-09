using EQX.Core.Common;
using EQX.UI.Converters;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Services.AlertServices;
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
    public class AlarmService : AlertServiceBase
    {
        private const string DefaultImage = "/PIFilmAutoDetachCleanMC;component/Resource/Image/PictureName.png";
        public AlarmService() : base(GetAlertFolder())
        {
        }

        protected override string DefaultImagePath => DefaultImage;

        protected override IEnumerable<(int Id, string Name)> GetEnumMap()
        {
            return Enum.GetValues(typeof(EAlarm))
                       .Cast<EAlarm>()
                       .Select(x => ((int)x, x.ToString()))
                       .ToList();
        }

        private static string GetAlertFolder()
        {
            return ResolveAlertFolder(Path.Combine("Alert", "Alarm"));

        }
    }
}
