using System.Windows;
using System.Windows.Input;
using MilitaryGeo.Desktop.ViewModels;

namespace MilitaryGeo.Desktop.Popup.VaiTro
{
    public partial class AddVaiTroWindow : Window
    {
        private readonly AddVaiTroViewModel _viewModel;

        public AddVaiTroWindow(AddVaiTroViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.CloseRequested += (s, e) =>
            {
                DialogResult = true;
                Close();
            };
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // ViewModel will handle the save logic via command
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
