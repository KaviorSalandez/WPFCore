using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using MenuItemModel = MilitaryGeo.Desktop.ViewModels.MenuItem;

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

        public ObservableCollection<MenuItemModel> MenuItems { get; set; }

        public MainWorkSpace()
        {
            InitializeComponent();
            LoadMenuData();
            DataContext = this;
            // Load Dashboard by default
            LoadView("Dashboard");
        }

        public MainWorkSpace(string username) : this()
        {
            txtUserName.Text = username;
        }

        private void LoadMenuData()
        {
            // Fake data - Sau này sẽ load từ API
            MenuItems = new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel
                {
                    Icon = "📊",
                    Title = "Dashboard",
                    ViewName = "Dashboard"
                },
                new MenuItemModel
                {
                    Icon = "🗺️",
                    Title = "Quản lý bản đồ",
                    ViewName = "MapManagement"
                },
                new MenuItemModel
                {
                    Icon = "📁",
                    Title = "Quản lý dữ liệu",
                    ViewName = "DataManagement"
                },
                new MenuItemModel
                {
                    Icon = "📍",
                    Title = "Tọa độ địa lý",
                    ViewName = "Coordinates"
                },
                new MenuItemModel
                {
                    Icon = "📈",
                    Title = "Báo cáo",
                    ViewName = "Reports"
                },
                new MenuItemModel
                {
                    Icon = "👥",
                    Title = "Quản lý người dùng",
                    ViewName = "UserManagement"
                },
                new MenuItemModel
                {
                    Icon = "🎭",
                    Title = "Quản lý vai trò",
                    ViewName = "RoleManagement"
                },
                new MenuItemModel
                {
                    Icon = "⚙️",
                    Title = "Cài đặt hệ thống",
                    ViewName = "Settings",
                    SubMenuItems = new ObservableCollection<MenuItemModel>
                    {
                        new MenuItemModel
                        {
                            Icon = "○",
                            Title = "Cài đặt chung",
                            ViewName = "GeneralSettings"
                        },
                        new MenuItemModel
                        {
                            Icon = "○",
                            Title = "Cài đặt bảo mật",
                            ViewName = "SecuritySettings"
                        },
                        new MenuItemModel
                        {
                            Icon = "○",
                            Title = "Cài đặt bản đồ",
                            ViewName = "MapSettings"
                        },
                        new MenuItemModel
                        {
                            Icon = "○",
                            Title = "Cơ sở dữ liệu",
                            ViewName = "DatabaseSettings"
                        },
                        new MenuItemModel
                        {
                            Icon = "○",
                            Title = "Thông báo",
                            ViewName = "NotificationSettings"
                        },
                        new MenuItemModel
                        {
                            Icon = "○",
                            Title = "Sao lưu và Phục hồi",
                            ViewName = "BackupSettings"
                        }
                    }
                },
                new MenuItemModel
                {
                    Icon = "❓",
                    Title = "Hướng dẫn",
                    ViewName = "Help"
                }
            };
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
            if (isSidebarCollapsed)
            {
                // Fade out menu header
                FadeOutElement(txtMenuHeader, () => txtMenuHeader.Visibility = Visibility.Collapsed);
                FadeOutElement(txtLogoText, () => txtLogoText.Visibility = Visibility.Collapsed);
                
                // Hide all text in menu items via ItemsControl
                AnimateMenuItemsCollapse();
            }
            else
            {
                // Show menu header
                txtMenuHeader.Visibility = Visibility.Visible;
                txtLogoText.Visibility = Visibility.Visible;
                
                FadeInElement(txtMenuHeader);
                FadeInElement(txtLogoText);
                
                // Show all text in menu items
                AnimateMenuItemsExpand();
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
        }

        private void AnimateMenuItemsCollapse()
        {
            // Find all RadioButtons in MenuItemsControl and adjust their padding
            foreach (var menuItem in MenuItems)
            {
                // This will be handled by XAML styling or triggers
                // For now, just a placeholder
            }
        }

        private void AnimateMenuItemsExpand()
        {
            // Find all RadioButtons in MenuItemsControl and restore their padding
            foreach (var menuItem in MenuItems)
            {
                // This will be handled by XAML styling or triggers
                // For now, just a placeholder
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

        private void OnDynamicMenuItemChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Tag != null)
            {
                string viewName = radioButton.Tag.ToString();
                LoadView(viewName);
            }
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                var menuItem = radioButton.DataContext as MenuItemModel;
                if (menuItem != null && menuItem.HasSubMenu)
                {
                    // Find the SubMenuItemsControl in the visual tree
                    var parent = VisualTreeHelper.GetParent(radioButton);
                    while (parent != null && !(parent is StackPanel))
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                    }

                    if (parent is StackPanel stackPanel)
                    {
                        var subMenuControl = FindChild<ItemsControl>(stackPanel, "SubMenuItemsControl");
                        if (subMenuControl != null)
                        {
                            var isCurrentlyCollapsed = subMenuControl.Visibility == Visibility.Collapsed;
                            
                            if (isCurrentlyCollapsed)
                            {
                                // Expand submenu
                                subMenuControl.Visibility = Visibility.Visible;
                                
                                // Fade in submenu
                                subMenuControl.Opacity = 0;
                                var fadeIn = new DoubleAnimation
                                {
                                    From = 0.0,
                                    To = 1.0,
                                    Duration = TimeSpan.FromMilliseconds(300)
                                };
                                subMenuControl.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                            }
                            else
                            {
                                // Fade out and collapse
                                var fadeOut = new DoubleAnimation
                                {
                                    From = 1.0,
                                    To = 0.0,
                                    Duration = TimeSpan.FromMilliseconds(200)
                                };
                                fadeOut.Completed += (s, args) =>
                                {
                                    subMenuControl.Visibility = Visibility.Collapsed;
                                };
                                subMenuControl.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                            }
                        }
                    }
                }
            }
        }

        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is T typedChild && (string.IsNullOrEmpty(childName) || 
                    (child is FrameworkElement fe && fe.Name == childName)))
                {
                    foundChild = typedChild;
                    break;
                }
                
                foundChild = FindChild<T>(child, childName);
                if (foundChild != null) break;
            }
            return foundChild;
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
                    view = new Map();
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

                case "RoleManagement":
                    view = new VaiTro();
                    break;

                // Settings submenu items
                case "GeneralSettings":
                    view = CreatePlaceholderView("Cài đặt chung", "Chức năng cài đặt chung: Ngôn ngữ, Múi giờ, Đơn vị đo lường...");
                    break;

                case "SecuritySettings":
                    view = CreatePlaceholderView("Cài đặt bảo mật", "Chức năng cài đặt bảo mật: Mật khẩu, Phân quyền, Xác thực 2 yếu tố...");
                    break;

                case "MapSettings":
                    view = CreatePlaceholderView("Cài đặt bản đồ", "Chức năng cài đặt bản đồ: API Keys, Layer mặc định, Cache...");
                    break;

                case "DatabaseSettings":
                    view = CreatePlaceholderView("Cài đặt cơ sở dữ liệu", "Chức năng cài đặt CSDL: Connection string, Backup schedule...");
                    break;

                case "NotificationSettings":
                    view = CreatePlaceholderView("Cài đặt thông báo", "Chức năng cài đặt thông báo: Email, SMS, Push notifications...");
                    break;

                case "BackupSettings":
                    view = CreatePlaceholderView("Sao lưu & Phục hồi", "Chức năng sao lưu dữ liệu tự động và phục hồi hệ thống...");
                    break;

                case "Settings":
                    view = CreatePlaceholderView("Cài đặt hệ thống", "Vui lòng chọn một mục cài đặt cụ thể từ menu bên trái.");
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
