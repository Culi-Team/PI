using EQX.Core.Recipe;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PIFilmAutoDetachCleanMC.MVVM.Views.Teaching
{
    /// <summary>
    /// Interaction logic for VinylCleanTeachingView.xaml
    /// </summary>
    public partial class VinylCleanTeachingView : UserControl
    {
        public VinylCleanTeachingView()
        {
            InitializeComponent();
        }

        private void LoadPositionTeaching()
        {
            if (this.DataContext is UnitTeachingViewModel vm == false) return;
            PositionTeaching_StackPanel.Children.Clear();
            PositionTeaching_StackPanel.Children.Add(new SinglePositionTeaching(null, null) { IsHeader = true });

            int index = 0;

            PropertyInfo[] props = vm.Recipe.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                // 1. Get all attributes of property
                List<object> attrs = prop.GetCustomAttributes(false).ToList();

                // 2. Ignore properties of base clas (Head / Name properties)
                if (attrs.Count <= 0) continue;

                // 3. Get DataDescriptionAttribute
                if (attrs.FirstOrDefault(att => (att is SingleRecipeDescriptionAttribute)) == null)
                {
                    continue;
                }
                SingleRecipeDescriptionAttribute dataAttr = (SingleRecipeDescriptionAttribute)attrs.First(att => (att as SingleRecipeDescriptionAttribute) != null);
                if (dataAttr.DescriptionKey != null)
                    dataAttr.Description = Application.Current.Resources[dataAttr.DescriptionKey].ToString();
                if (dataAttr.DetailKey != null)
                    dataAttr.Detail = Application.Current.Resources[dataAttr.DetailKey].ToString();
                // 4. Adding spacer if it's
                if (dataAttr == null)
                {
                    throw new Exception("Attribute need to be add to recipe properties");
                }
                else if (dataAttr.IsSpacer)
                {
                    PositionTeaching_StackPanel.Children.Add(new SinglePositionTeaching(dataAttr, null));
                    continue;
                }

                // 5. Extract SingleRecipePositionAttribute
                SinglePositionTeachingAttribute positionAttribute = null;
                if (attrs.FirstOrDefault(att => att is SinglePositionTeachingAttribute) != null)
                {
                    positionAttribute = (SinglePositionTeachingAttribute)attrs.FirstOrDefault(att => att is SinglePositionTeachingAttribute);
                }

                if (positionAttribute == null) continue;

                // 6. Add recipe DataView to the view
                if (prop.PropertyType.Name == nameof(Double)
                    || prop.PropertyType.Name == nameof(Int32) || prop.PropertyType.Name == nameof(UInt32)
                    || prop.PropertyType.Name == nameof(Int64) || prop.PropertyType.Name == nameof(UInt64))
                {
                    dataAttr.Index = ++index;
                    Binding binding = new Binding(prop.Name)
                    {
                        Source = vm.Recipe,
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };

                    SinglePositionTeaching singlePositionTeaching = new SinglePositionTeaching(dataAttr, positionAttribute);
                    singlePositionTeaching.SetBinding(SinglePositionTeaching.ValueProperty, binding);

                    singlePositionTeaching.MovePositionTeachingCommand = vm.MovePositionTeachingCommand;
                    singlePositionTeaching.GetCurrentPositionCommand = vm.GetCurrentPositionCommand;
                    singlePositionTeaching.SaveCommand = vm.SaveCommand;

                    PositionTeaching_StackPanel.Children.Add(singlePositionTeaching);
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPositionTeaching();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label)
            {
                DataEditor dataEditor = new DataEditor(Convert.ToDouble(label.Content), null);
                if (dataEditor.ShowDialog() == true)
                {
                    label.Content = dataEditor.NewValue;
                }
            }
        }

        private void root_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadPositionTeaching();
        }
    }
}
