using EQX.Core.InOut;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace PIFilmAutoDetachCleanMC.Converters
{
    public class VacuumListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<IDOutput> vacuums = new List<IDOutput>();
            if (value is not ObservableCollection<IDOutput> vacuumsList) return Binding.DoNothing;

            PropertyInfo[] properties = value.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var propertyValue = property.GetValue(vacuumsList, null);
                if (propertyValue is IDOutput output)
                {
                    vacuums.Add(output);
                }
            }
            return vacuums;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

    }
}
