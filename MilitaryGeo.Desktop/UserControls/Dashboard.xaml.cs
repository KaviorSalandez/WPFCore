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

namespace MilitaryGeo.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadRecentActivity();
        }

        private void LoadRecentActivity()
        {
            var activities = new ObservableCollection<RecentActivity>
            {
                new RecentActivity { Time = "10:30 AM", Activity = "Cập nhật bản đồ khu vực A", User = "Nguyễn Văn A", Status = "Hoàn thành" },
                new RecentActivity { Time = "09:15 AM", Activity = "Thêm mới tọa độ địa lý", User = "Trần Thị B", Status = "Hoàn thành" },
                new RecentActivity { Time = "08:45 AM", Activity = "Import dữ liệu từ file Excel", User = "Lê Văn C", Status = "Đang xử lý" },
                new RecentActivity { Time = "08:20 AM", Activity = "Xuất báo cáo tháng 12", User = "Phạm Văn D", Status = "Hoàn thành" },
                new RecentActivity { Time = "07:50 AM", Activity = "Cập nhật thông tin người dùng", User = "Hoàng Thị E", Status = "Hoàn thành" }
            };

            gridRecentActivity.ItemsSource = activities;
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
