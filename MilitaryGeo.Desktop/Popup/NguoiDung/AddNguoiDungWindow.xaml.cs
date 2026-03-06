using MilitaryGeo.Desktop.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MilitaryGeo.Desktop.Popup.NguoiDung
{
    /// <summary>
    /// Interaction logic for AddNguoiDungWindow.xaml
    /// </summary>
    public partial class AddNguoiDungWindow : Window
    {
        private readonly AddNguoiDungViewModel _viewModel;

        public AddNguoiDungViewModel ViewModel => _viewModel;

        public AddNguoiDungWindow(AddNguoiDungViewModel viewModel)
        {
            InitializeComponent();
            
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = _viewModel;

            // Subscribe to close event
            _viewModel.CloseRequested += OnCloseRequested;
            
            // Clean up on window close
            Closed += (s, e) => _viewModel.CloseRequested -= OnCloseRequested;
        }

        private void OnCloseRequested(object? sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // PasswordBox doesn't support binding Password property for security reasons
        // We need to handle this manually
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }
    }
}
