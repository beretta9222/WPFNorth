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

namespace NorthWind.Views
{
    /// <summary>
    /// Interaction logic for ProductDetailsUC.xaml
    /// </summary>
    public partial class ProductDetailsUC : UserControl
    {
        ViewModels.ProductDatails model;
        public ProductDetailsUC()
        {
            InitializeComponent();
            model = new ViewModels.ProductDatails();
            this.DataContext = model;
        }
    }
}
