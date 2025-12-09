using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.Common;

namespace PIFilmAutoDetachCleanMC.Services.Validation
{
    public static class AlertDataValidator
    {
        public static AlertValidationResult Validate(IEnumerable<AlertModel> alerts)
        {
            var result = new AlertValidationResult();

            foreach (var alert in alerts)
            {
                if (alert == null)
                {
                    result.Errors.Add("Alert item is null");
                    continue;
                }

                if (alert.AlertOverviewShapes.Any(shape => !IsShapeValid(shape)))
                {
                    result.Errors.Add($"Alert {alert.Id} has invalid overview shapes.");
                }

                if (alert.AlertDetailviewShapes.Any(shape => !IsShapeValid(shape)))
                {
                    result.Errors.Add($"Alert {alert.Id} has invalid detail shapes.");
                }

                if (alert.TroubleshootingSteps == null || !alert.TroubleshootingSteps.Any())
                {
                    result.Warnings.Add($"Alert {alert.Id} is missing troubleshooting steps.");
                }
            }

            return result;
        }

        private static bool IsShapeValid(AlertShapeModel shape)
        {
            if (shape == null) return false;

            return shape.Type switch
            {
                AlertShapeType.Rectangle => shape.Width >= 0 && shape.Height >= 0,
                AlertShapeType.Circle => shape.Radius >= 0,
                _ => false
            };
        }
    }
}
