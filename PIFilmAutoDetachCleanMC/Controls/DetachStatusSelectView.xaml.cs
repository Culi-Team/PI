using System;
using System.Collections.Generic;
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
    /// Interaction logic for DetachStatusSelectView.xaml
    /// </summary>
    public partial class DetachStatusSelectView : Window
    {
        public DetachStatusSelectView()
        {
            InitializeComponent();
        }

        public bool IsDetached { get; set; }

        private void Detached_Select_Click(object sender, RoutedEventArgs e)
        {
            IsDetached = true;
            this.DialogResult = true;
        }

        private void Not_Detached_Select_Click(object sender, RoutedEventArgs e)
        {
            IsDetached = false;
            this.DialogResult = true;
        }
    }
}
