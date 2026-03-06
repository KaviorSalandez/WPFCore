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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MilitaryGeo.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        private DispatcherTimer _timer;
        private ObservableCollection<RecentActivity> _activities;

        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
            InitializeTimer();
        }

        private void LoadDashboardData()
        {
            // Load recent activities
            _activities = new ObservableCollection<RecentActivity>
            {
                new RecentActivity 
                { 
                    Time = "10:35:21", 
                    Activity = "Tải lên bản đồ 'Khu vực chiến lược Bắc Bộ'", 
                    User = "Nguyễn Văn An", 
                    Status = "Thành công" 
                },
                new RecentActivity 
                { 
                    Time = "10:28:45", 
                    Activity = "Cập nhật tọa độ điểm quan sát #4521", 
                    User = "Trần Thị Bình", 
                    Status = "Thành công" 
                },
                new RecentActivity 
                { 
                    Time = "10:15:33", 
                    Activity = "Xuất báo cáo tuần 47/2024", 
                    User = "Lê Văn Cường", 
                    Status = "Đang xử lý" 
                },
                new RecentActivity 
                { 
                    Time = "09:52:18", 
                    Activity = "Xóa bản đồ 'Khu vực cũ 2020'", 
                    User = "Phạm Văn Đức", 
                    Status = "Thành công" 
                },
                new RecentActivity 
                { 
                    Time = "09:43:56", 
                    Activity = "Thêm người dùng 'Hoàng Văn Em'", 
                    User = "Administrator", 
                    Status = "Thành công" 
                },
                new RecentActivity 
                { 
                    Time = "09:31:22", 
                    Activity = "Import dữ liệu từ file GPS_Data_2024.csv", 
                    User = "Ngô Thị Phương", 
                    Status = "Thất bại" 
                },
                new RecentActivity 
                { 
                    Time = "09:18:47", 
                    Activity = "Chỉnh sửa thông tin bản đồ 'Vùng biên giới phía Bắc'", 
                    User = "Đặng Văn Giang", 
                    Status = "Thành công" 
                },
                new RecentActivity 
                { 
                    Time = "09:05:12", 
                    Activity = "Sao lưu dữ liệu hệ thống", 
                    User = "System", 
                    Status = "Thành công" 
                },
                new RecentActivity 
                { 
                    Time = "08:52:38", 
                    Activity = "Tạo báo cáo phân tích địa hình", 
                    User = "Vũ Thị Hằng", 
                    Status = "Đang xử lý" 
                },
                new RecentActivity 
                { 
                    Time = "08:41:05", 
                    Activity = "Cập nhật quyền truy cập cho nhóm 'Tác chiến'", 
                    User = "Administrator", 
                    Status = "Thành công" 
                }
            };

            gridRecentActivity.ItemsSource = _activities;
        }

        private void InitializeTimer()
        {
            // Update current time every second
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Initial update
            UpdateDateTime();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            var now = DateTime.Now;
            txtCurrentTime.Text = $"📅 {now:dddd, dd/MM/yyyy} • 🕐 {now:HH:mm:ss}";
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
