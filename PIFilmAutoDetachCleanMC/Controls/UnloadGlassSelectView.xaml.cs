using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PIFilmAutoDetachCleanMC.Controls
{
    /// <summary>
    /// Interaction logic for UnloadGlassSelectView.xaml
    /// </summary>
    public partial class UnloadGlassSelectView : Window, INotifyPropertyChanged
    {
        public UnloadGlassSelectView()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool isUnloadGlass1 = true;
        private bool isUnloadGlass2 = true;
        private bool isUnloadGlass3 = true;
        private bool isUnloadGlass4 = true;

        public bool IsUnloadGlass1
        {
            get { return isUnloadGlass1; }
            set { isUnloadGlass1 = value; OnPropertyChanged(); }
        }

        public bool IsUnloadGlass2
        {
            get { return isUnloadGlass2; }
            set { isUnloadGlass2 = value; OnPropertyChanged(); }
        }

        public bool IsUnloadGlass3
        {
            get { return isUnloadGlass3; }
            set { isUnloadGlass3 = value; OnPropertyChanged(); }
        }

        public bool IsUnloadGlass4
        {
            get { return isUnloadGlass4; }
            set { isUnloadGlass4 = value; OnPropertyChanged(); }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void GlassSelect_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.Tag.ToString() == "1")
                {
                    IsUnloadGlass1 = !IsUnloadGlass1;
                }
                if (border.Tag.ToString() == "2")
                {
                    IsUnloadGlass2 = !IsUnloadGlass2;
                }
                if (border.Tag.ToString() == "3")
                {
                    IsUnloadGlass3 = !IsUnloadGlass3;
                }
                if (border.Tag.ToString() == "4")
                {
                    IsUnloadGlass4 = !IsUnloadGlass4;
                }
            }
        }
    }
}
