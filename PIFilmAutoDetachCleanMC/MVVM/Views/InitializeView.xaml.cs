using EQX.Core.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
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

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for InitializeView.xaml
    /// </summary>
    public partial class InitializeView : UserControl
    {
        public InitializeView()
        {
            InitializeComponent();
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label == false) return;

            var binding = BindingOperations.GetBindingExpression(label, Control.BackgroundProperty);
            if (binding == null) return;

            var dataItem = binding.DataItem;
            var path = binding.ParentBinding.Path.Path;

            if (string.IsNullOrWhiteSpace(path) || dataItem == null)
                return;

            var parts = path.Split('.');
            object currentObject = dataItem;

            for (int i = 0; i < parts.Length; i++)
            {
                var prop = currentObject.GetType().GetProperty(parts[i], BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return;

                if (i == parts.Length - 1)
                {
                    if (prop.PropertyType == typeof(bool) && prop.CanRead && prop.CanWrite)
                    {
                        bool currentValue = (bool)prop.GetValue(currentObject);
                        prop.SetValue(currentObject, !currentValue);
                    }
                }
                else
                {
                    currentObject = prop.GetValue(currentObject);
                    if (currentObject == null) return;
                }
            }
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is InitializeViewModel originVM == false) return;
            if (sender is Border border == false) return;
            if (border.DataContext is IProcess<ESequence> process == false) return;

            bool currentValue = process.IsOriginOrInitSelected;
            process.IsOriginOrInitSelected = !currentValue;
        }
    }
}
