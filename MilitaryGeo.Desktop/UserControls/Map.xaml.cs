using MilitaryGeo.Desktop.ViewModels;
using System.Windows.Controls;

namespace MilitaryGeo.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
            
            // Get ViewModel from DI Container and set as DataContext
            DataContext = App.GetService<MapViewModel>();
        }
    }
}
