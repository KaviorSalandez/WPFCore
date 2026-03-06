using MilitaryGeo.Desktop.ViewModels;
using System.Windows.Controls;

namespace MilitaryGeo.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for NguoiDung.xaml
    /// </summary>
    public partial class NguoiDung : UserControl
    {
        public NguoiDung()
        {
            InitializeComponent();
            
            // Get ViewModel from DI Container and set as DataContext
            DataContext = App.GetService<NguoiDungViewModel>();
        }
    }
}
