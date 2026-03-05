using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MilitaryGeo.Desktop.UserControls;

namespace MilitaryGeo.Desktop
{
    /// <summary>
    /// Interaction logic for MainWorkSpace.xaml
    /// </summary>
    public partial class MainWorkSpace : Window
    {
        private bool isSidebarCollapsed = false;
        private const double CollapsedWidth = 70;
        private const double ExpandedWidth = 250;

        public MainWorkSpace()
        {
            InitializeComponent();
            // Load Dashboard by default
            LoadView("Dashboard");
        }

        public MainWorkSpace(string username) : this()
        {
            txtUserName.Text = username;
        }

        private void btnToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            isSidebarCollapsed = !isSidebarCollapsed;

            // Create animation for sidebar width
            var duration = TimeSpan.FromMilliseconds(300);
            var easing = new CubicEase { EasingMode = EasingMode.EaseInOut };

            // Animate sidebar width using custom animation
            var startWidth = isSidebarCollapsed ? ExpandedWidth : CollapsedWidth;
            var endWidth = isSidebarCollapsed ? CollapsedWidth : ExpandedWidth;
            
            AnimateSidebarWidth(startWidth, endWidth, duration);

            // Toggle visibility of menu text with fade animation
            var visibility = isSidebarCollapsed ? Visibility.Collapsed : Visibility.Visible;
            
            if (isSidebarCollapsed)
            {
                // Fade out then collapse
                FadeOutElement(txtMenuHeader, () => txtMenuHeader.Visibility = Visibility.Collapsed);
                FadeOutElement(txtDashboard, () => txtDashboard.Visibility = Visibility.Collapsed);
                FadeOutElement(txtMapManagement, () => txtMapManagement.Visibility = Visibility.Collapsed);
                FadeOutElement(txtDataManagement, () => txtDataManagement.Visibility = Visibility.Collapsed);
                FadeOutElement(txtCoordinates, () => txtCoordinates.Visibility = Visibility.Collapsed);
                FadeOutElement(txtReports, () => txtReports.Visibility = Visibility.Collapsed);
                FadeOutElement(txtUserManagement, () => txtUserManagement.Visibility = Visibility.Collapsed);
                FadeOutElement(txtSettings, () => txtSettings.Visibility = Visibility.Collapsed);
                FadeOutElement(txtHelp, () => txtHelp.Visibility = Visibility.Collapsed);
                FadeOutElement(txtLogoText, () => txtLogoText.Visibility = Visibility.Collapsed);
                MenuSeparator.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show then fade in
                txtMenuHeader.Visibility = Visibility.Visible;
                txtDashboard.Visibility = Visibility.Visible;
                txtMapManagement.Visibility = Visibility.Visible;
                txtDataManagement.Visibility = Visibility.Visible;
                txtCoordinates.Visibility = Visibility.Visible;
                txtReports.Visibility = Visibility.Visible;
                txtUserManagement.Visibility = Visibility.Visible;
                txtSettings.Visibility = Visibility.Visible;
                txtHelp.Visibility = Visibility.Visible;
                txtLogoText.Visibility = Visibility.Visible;
                MenuSeparator.Visibility = Visibility.Visible;
                
                FadeInElement(txtMenuHeader);
                FadeInElement(txtDashboard);
                FadeInElement(txtMapManagement);
                FadeInElement(txtDataManagement);
                FadeInElement(txtCoordinates);
                FadeInElement(txtReports);
                FadeInElement(txtUserManagement);
                FadeInElement(txtSettings);
                FadeInElement(txtHelp);
                FadeInElement(txtLogoText);
            }

            // Rotate toggle button icon
            var rotateTransform = btnToggleMenu.RenderTransform as RotateTransform;
            if (rotateTransform == null)
            {
                rotateTransform = new RotateTransform();
                btnToggleMenu.RenderTransform = rotateTransform;
                btnToggleMenu.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            var rotateAnimation = new DoubleAnimation
            {
                To = isSidebarCollapsed ? 180 : 0,
                Duration = duration,
                EasingFunction = easing
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

            // Adjust padding for collapsed state
            if (isSidebarCollapsed)
            {
                // Center align menu items when collapsed
                rbDashboard.Padding = new Thickness(0);
                rbMapManagement.Padding = new Thickness(0);
                rbDataManagement.Padding = new Thickness(0);
                rbCoordinates.Padding = new Thickness(0);
                rbReports.Padding = new Thickness(0);
                rbUserManagement.Padding = new Thickness(0);
                rbSettings.Padding = new Thickness(0);
                rbHelp.Padding = new Thickness(0);

                rbDashboard.HorizontalContentAlignment = HorizontalAlignment.Center;
                rbMapManagement.HorizontalContentAlignment = HorizontalAlignment.Center;
                rbDataManagement.HorizontalContentAlignment = HorizontalAlignment.Center;
                rbCoordinates.HorizontalContentAlignment = HorizontalAlignment.Center;
                rbReports.HorizontalContentAlignment = HorizontalAlignment.Center;
                rbUserManagement.HorizontalContentAlignment = HorizontalAlignment.Center;
                rbSettings.HorizontalContentAlignment = HorizontalAlignment.Center;
                rbHelp.HorizontalContentAlignment = HorizontalAlignment.Center;
            }
            else
            {
                // Restore left alignment when expanded
                rbDashboard.Padding = new Thickness(20, 0, 0, 0);
                rbMapManagement.Padding = new Thickness(20, 0, 0, 0);
                rbDataManagement.Padding = new Thickness(20, 0, 0, 0);
                rbCoordinates.Padding = new Thickness(20, 0, 0, 0);
                rbReports.Padding = new Thickness(20, 0, 0, 0);
                rbUserManagement.Padding = new Thickness(20, 0, 0, 0);
                rbSettings.Padding = new Thickness(20, 0, 0, 0);
                rbHelp.Padding = new Thickness(20, 0, 0, 0);

                rbDashboard.HorizontalContentAlignment = HorizontalAlignment.Left;
                rbMapManagement.HorizontalContentAlignment = HorizontalAlignment.Left;
                rbDataManagement.HorizontalContentAlignment = HorizontalAlignment.Left;
                rbCoordinates.HorizontalContentAlignment = HorizontalAlignment.Left;
                rbReports.HorizontalContentAlignment = HorizontalAlignment.Left;
                rbUserManagement.HorizontalContentAlignment = HorizontalAlignment.Left;
                rbSettings.HorizontalContentAlignment = HorizontalAlignment.Left;
                rbHelp.HorizontalContentAlignment = HorizontalAlignment.Left;
            }
        }

        private void AnimateSidebarWidth(double from, double to, TimeSpan duration)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            var startTime = DateTime.Now;
            var easing = new CubicEase { EasingMode = EasingMode.EaseInOut };

            timer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            timer.Tick += (s, e) =>
            {
                var elapsed = DateTime.Now - startTime;
                var progress = Math.Min(1.0, elapsed.TotalMilliseconds / duration.TotalMilliseconds);
                
                // Apply easing
                var easedProgress = easing.Ease(progress);
                
                var currentWidth = from + (to - from) * easedProgress;
                SidebarColumn.Width = new GridLength(currentWidth);

                if (progress >= 1.0)
                {
                    timer.Stop();
                    SidebarColumn.Width = new GridLength(to);
                }
            };

            timer.Start();
        }

        private void FadeOutElement(UIElement element, Action onComplete = null)
        {
            var fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            if (onComplete != null)
            {
                fadeOut.Completed += (s, e) => onComplete();
            }

            element.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void FadeInElement(UIElement element)
        {
            element.Opacity = 0;
            var fadeIn = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(200),
                BeginTime = TimeSpan.FromMilliseconds(100)
            };

            element.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void OnMenuItemChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Tag != null)
            {
                string viewName = radioButton.Tag.ToString();
                LoadView(viewName);
            }
        }

        private void LoadView(string viewName)
        {
            UserControl view = null;

            switch (viewName)
            {
                case "Dashboard":
                    view = new Dashboard();
                    break;

                case "MapManagement":
                    view = CreatePlaceholderView("Quản lý bản đồ", "Chức năng quản lý bản đồ đang được phát triển...");
                    break;

                case "DataManagement":
                    view = CreatePlaceholderView("Quản lý dữ liệu", "Chức năng quản lý dữ liệu đang được phát triển...");
                    break;

                case "Coordinates":
                    view = CreatePlaceholderView("Tọa độ địa lý", "Chức năng quản lý tọa độ địa lý đang được phát triển...");
                    break;

                case "Reports":
                    view = CreatePlaceholderView("Báo cáo", "Chức năng báo cáo đang được phát triển...");
                    break;

                case "UserManagement":
                    view = new NguoiDung();
                    break;

                case "Settings":
                    view = CreatePlaceholderView("Cài đặt hệ thống", "Chức năng cài đặt hệ thống đang được phát triển...");
                    break;

                case "Help":
                    view = CreatePlaceholderView("Hướng dẫn", "Tài liệu hướng dẫn sử dụng hệ thống đang được cập nhật...");
                    break;

                default:
                    view = new Dashboard();
                    break;
            }

            MainContentArea.Content = view;
        }

        private UserControl CreatePlaceholderView(string title, string message)
        {
            var userControl = new UserControl
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"))
            };

            var border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(40),
                Margin = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var titleText = new TextBlock
            {
                Text = title,
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")),
                Margin = new Thickness(0, 0, 0, 15),
                TextAlignment = TextAlignment.Center
            };

            var iconText = new TextBlock
            {
                Text = "🚧",
                FontSize = 64,
                Margin = new Thickness(0, 0, 0, 20),
                TextAlignment = TextAlignment.Center
            };

            var messageText = new TextBlock
            {
                Text = message,
                FontSize = 14,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666")),
                TextAlignment = TextAlignment.Center
            };

            stackPanel.Children.Add(iconText);
            stackPanel.Children.Add(titleText);
            stackPanel.Children.Add(messageText);
            border.Child = stackPanel;
            
            var grid = new Grid();
            grid.Children.Add(border);
            userControl.Content = grid;

            return userControl;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tìm kiếm...";
                txtSearch.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"));
            }
        }
    }

    public class RecentActivity
    {
        public string Time { get; set; }
        public string Activity { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
    }
}
