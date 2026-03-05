using Syncfusion.SfSkinManager;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MilitaryGeo.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return;
            }

            // Authentication logic - Fixed credentials
            if (username.ToLower() == "admin" && password == "123456")
            {
                // Login successful - Open MainWorkSpace
                var mainWorkSpace = new MainWorkSpace(username);
                mainWorkSpace.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!\n\nGợi ý: admin / 123456", 
                    "Lỗi đăng nhập",
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                
                // Clear password field
                txtPassword.Password = "";
                txtUsername.Focus();
                txtUsername.SelectAll();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Cho phép kéo cửa sổ
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void lnkForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản trị viên để được hỗ trợ!\n\nEmail: admin@militarygeo.vn\nPhone: 024.xxxx.xxxx", 
                "Quên mật khẩu", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}