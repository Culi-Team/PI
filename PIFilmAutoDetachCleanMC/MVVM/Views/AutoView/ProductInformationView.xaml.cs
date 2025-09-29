using EQX.UI.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ProductInformationView.xaml
    /// </summary>
    public partial class ProductInformationView : UserControl
    {
        public ProductInformationView()
        {
            InitializeComponent();
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_ResetWorkData"]) == true)
            {
                if (this.DataContext is CWorkData workData)
                {
                    workData.Reset();
                }
            }
        }
    }
}
