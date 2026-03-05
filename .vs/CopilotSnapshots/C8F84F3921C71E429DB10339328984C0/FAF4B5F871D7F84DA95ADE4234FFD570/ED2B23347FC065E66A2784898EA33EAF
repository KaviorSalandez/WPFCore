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
    /// Interaction logic for NguoiDung.xaml
    /// </summary>
    public partial class NguoiDung : UserControl
    {
        public NguoiDung()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var users = new ObservableCollection<UserInfo>
            {
                new UserInfo { Id = 1, Username = "admin", FullName = "Quản trị viên", Email = "admin@militarygeo.vn", Phone = "0123456789", Role = "Admin", Status = "Hoạt động" },
                new UserInfo { Id = 2, Username = "user01", FullName = "Nguyễn Văn A", Email = "nguyenvana@militarygeo.vn", Phone = "0987654321", Role = "User", Status = "Hoạt động" },
                new UserInfo { Id = 3, Username = "user02", FullName = "Trần Thị B", Email = "tranthib@militarygeo.vn", Phone = "0912345678", Role = "User", Status = "Hoạt động" },
                new UserInfo { Id = 4, Username = "manager01", FullName = "Lê Văn C", Email = "levanc@militarygeo.vn", Phone = "0976543210", Role = "Manager", Status = "Hoạt động" },
                new UserInfo { Id = 5, Username = "user03", FullName = "Phạm Văn D", Email = "phamvand@militarygeo.vn", Phone = "0934567890", Role = "User", Status = "Khóa" }
            };

            gridUsers.ItemsSource = users;
        }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
