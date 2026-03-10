using MilitaryGeo.Desktop.ViewModels;
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

namespace MilitaryGeo.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for VaiTro.xaml
    /// </summary>
    public partial class VaiTro : UserControl
    {
        public VaiTro()
        {
            InitializeComponent();
            
            // Get ViewModel from DI Container and set as DataContext
            DataContext = App.GetService<VaiTroViewModel>();
        }
    }
}
