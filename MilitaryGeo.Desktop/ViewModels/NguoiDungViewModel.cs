using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MilitaryGeo.Application.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MilitaryGeo.Desktop.ViewModels;

public partial class NguoiDungViewModel : ObservableObject
{
    private readonly INguoiDungService _nguoiDungService;
    private readonly IMessageService _messageService; 
    // Collections
    private ObservableCollection<UserInfo> _allUsers = new();
    private ObservableCollection<UserInfo> _filteredUsers = new();

    [ObservableProperty]
    private ObservableCollection<UserInfo> _pagedUsers = new();

    // Search and Filter
    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedRole = "Tất cả vai trò";

    [ObservableProperty]
    private string _selectedStatus = "Tất cả trạng thái";

    // Pagination
    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private int _pageSize = 20;

    [ObservableProperty]
    private int _totalPages = 1;

    [ObservableProperty]
    private string _pageInfo = "Hiển thị 1-20 của 100 kết quả";

    [ObservableProperty]
    private string _totalUsersText = "Tổng số: 0 người dùng";

    // Pagination Button States
    [ObservableProperty]
    private bool _canGoToPreviousPage;

    [ObservableProperty]
    private bool _canGoToNextPage;

    // Filter Collections
    public ObservableCollection<string> RoleFilters { get; } = new()
    {
        "Tất cả vai trò",
        "Admin",
        "Manager",
        "User"
    };

    public ObservableCollection<string> StatusFilters { get; } = new()
    {
        "Tất cả trạng thái",
        "Hoạt động",
        "Khóa"
    };

    public ObservableCollection<int> PageSizes { get; } = new()
    {
        10, 20, 50, 100
    };

    public NguoiDungViewModel(INguoiDungService nguoiDungService, IMessageService messageService    )
    {
        _nguoiDungService = nguoiDungService ?? throw new ArgumentNullException(nameof(nguoiDungService));
        LoadUserDataAsync();
        _messageService = messageService;
    }

    private async void LoadUserDataAsync()
    {
        try
        {
            // Load mock data for now
            _allUsers = new ObservableCollection<UserInfo>
            {
                new UserInfo { Id = 13, Username = "quanly_01", FullName = "Nguyễn Hữu Thắng", Email = "nhthang@militarygeo.vn", Phone = "0912000001", Department = "Ban Chỉ huy", Position = "Trưởng phòng", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-50) },
                new UserInfo { Id = 14, Username = "nv_taphuan", FullName = "Lê Hồng Phong", Email = "lhphong@militarygeo.vn", Phone = "0912000002", Department = "Phòng Kỹ thuật", Position = "Kỹ sư cao cấp", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-45) },
                new UserInfo { Id = 15, Username = "canbo_dt", FullName = "Hoàng Xuân Vinh", Email = "hxvinh@militarygeo.vn", Phone = "0912000003", Department = "Phòng Tác chiến", Position = "Sĩ quan điều hành", Role = "User", Status = "Khóa", CreatedDate = DateTime.Now.AddDays(-40) },
                new UserInfo { Id = 16, Username = "u_thanhnien", FullName = "Phạm Minh Chính", Email = "pmchinh@militarygeo.vn", Phone = "0912000004", Department = "Phòng Kế hoạch", Position = "Chuyên viên", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-35) },
                new UserInfo { Id = 17, Username = "leader_gis", FullName = "Võ Văn Thưởng", Email = "vvthuong@militarygeo.vn", Phone = "0912000005", Department = "Phòng Kỹ thuật", Position = "Tổ trưởng", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-30) },
                new UserInfo { Id = 18, Username = "admin_v2", FullName = "Bùi Thị Minh", Email = "btminh@militarygeo.vn", Phone = "0912000006", Department = "Ban Chỉ huy", Position = "Quản trị viên", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-25) },
                new UserInfo { Id = 19, Username = "user_tc01", FullName = "Trần Thanh Hải", Email = "tthai@militarygeo.vn", Phone = "0912000007", Department = "Phòng Tác chiến", Position = "Trợ lý tác chiến", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-20) },
                new UserInfo { Id = 20, Username = "user_tc02", FullName = "Đỗ Kim Anh", Email = "dkanh@militarygeo.vn", Phone = "0912000008", Department = "Phòng Tác chiến", Position = "Sĩ quan phân tích", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-15) },
                new UserInfo { Id = 21, Username = "kehoach_03", FullName = "Nguyễn Mạnh Hùng", Email = "nmhung@militarygeo.vn", Phone = "0912000009", Department = "Phòng Kế hoạch", Position = "Phó phòng", Role = "Manager", Status = "Khóa", CreatedDate = DateTime.Now.AddDays(-10) },
                new UserInfo { Id = 22, Username = "kt_map01", FullName = "Phan Văn Giang", Email = "pvgiang_kt@militarygeo.vn", Phone = "0912000010", Department = "Phòng Kỹ thuật", Position = "Kỹ thuật viên", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-5) },
                new UserInfo { Id = 23, Username = "hc_nhansu", FullName = "Trịnh Văn Quyết", Email = "tvquyet@militarygeo.vn", Phone = "0912000011", Department = "Phòng Hậu cần", Position = "Nhân viên", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-1) },
                new UserInfo { Id = 24, Username = "tai_chinh_01", FullName = "Nguyễn Việt Anh", Email = "nvanh@militarygeo.vn", Phone = "0912000012", Department = "Phòng Tài chính", Position = "Kế toán trưởng", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-2) },
                new UserInfo { Id = 25, Username = "user_25", FullName = "Lý Thái Tổ", Email = "ltto@militarygeo.vn", Phone = "0912000013", Department = "Phòng Hành chính", Position = "Văn thư", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-3) },
                new UserInfo { Id = 26, Username = "user_26", FullName = "Trần Hưng Đạo", Email = "thdao@militarygeo.vn", Phone = "0912000014", Department = "Phòng Tác chiến", Position = "Chỉ huy trưởng", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-4) },
                new UserInfo { Id = 27, Username = "user_27", FullName = "Nguyễn Trãi", Email = "ntrai@militarygeo.vn", Phone = "0912000015", Department = "Phòng Kế hoạch", Position = "Cố vấn", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-5) },
                new UserInfo { Id = 28, Username = "user_28", FullName = "Lê Lợi", Email = "lloi@militarygeo.vn", Phone = "0912000016", Department = "Ban Chỉ huy", Position = "Thành viên", Role = "User", Status = "Khóa", CreatedDate = DateTime.Now.AddMonths(-6) },
                new UserInfo { Id = 29, Username = "user_29", FullName = "Quang Trung", Email = "qtrung@militarygeo.vn", Phone = "0912000017", Department = "Phòng Tác chiến", Position = "Đội trưởng", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-7) },
                new UserInfo { Id = 30, Username = "user_30", FullName = "Nguyễn Du", Email = "ndu@militarygeo.vn", Phone = "0912000018", Department = "Phòng Hành chính", Position = "Biên tập viên", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-8) },
                new UserInfo { Id = 31, Username = "user_31", FullName = "Phan Bội Châu", Email = "pbchau@militarygeo.vn", Phone = "0912000019", Department = "Phòng Kỹ thuật", Position = "Chuyên gia", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-9) },
                new UserInfo { Id = 32, Username = "user_32", FullName = "Phan Chu Trinh", Email = "pctrinh@militarygeo.vn", Phone = "0912000020", Department = "Phòng Kỹ thuật", Position = "Nghiên cứu viên", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-10) },
                new UserInfo { Id = 33, Username = "user_33", FullName = "Võ Nguyên Giáp", Email = "vngiap@militarygeo.vn", Phone = "0912000021", Department = "Ban Chỉ huy", Position = "Tổng tư lệnh", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddMonths(-11) },
                new UserInfo { Id = 34, Username = "user_34", FullName = "Hồ Chí Minh", Email = "hcm@militarygeo.vn", Phone = "0912000022", Department = "Ban Chỉ huy", Position = "Lãnh đạo", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddYears(-1) },
                new UserInfo { Id = 35, Username = "user_35", FullName = "Tôn Đức Thắng", Email = "tdthang@militarygeo.vn", Phone = "0912000023", Department = "Phòng Kế hoạch", Position = "Cán bộ nguồn", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-100) },
                new UserInfo { Id = 36, Username = "user_36", FullName = "Phạm Văn Đồng", Email = "pvdong@militarygeo.vn", Phone = "0912000024", Department = "Phòng Tài chính", Position = "Thanh tra", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-120) },
                new UserInfo { Id = 37, Username = "user_37", FullName = "Võ Chí Công", Email = "vccong@militarygeo.vn", Phone = "0912000025", Department = "Phòng Hậu cần", Position = "Thủ kho", Role = "User", Status = "Khóa", CreatedDate = DateTime.Now.AddDays(-150) },
                new UserInfo { Id = 38, Username = "user_38", FullName = "Nguyễn Lương Bằng", Email = "nlbang@militarygeo.vn", Phone = "0912000026", Department = "Phòng Hành chính", Position = "Lưu trữ", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-180) },
                new UserInfo { Id = 39, Username = "user_39", FullName = "Lê Duẩn", Email = "lduan@militarygeo.vn", Phone = "0912000027", Department = "Ban Chỉ huy", Position = "Bí thư", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-200) },
                new UserInfo { Id = 40, Username = "user_40", FullName = "Trường Chinh", Email = "tchinh@militarygeo.vn", Phone = "0912000028", Department = "Phòng Kế hoạch", Position = "Lý luận viên", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-210) },
                new UserInfo { Id = 41, Username = "user_41", FullName = "Nguyễn Văn Linh", Email = "nvlinh@militarygeo.vn", Phone = "0912000029", Department = "Phòng Tác chiến", Position = "Sĩ quan liên lạc", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-220) },
                new UserInfo { Id = 42, Username = "user_42", FullName = "Lê Khả Phiêu", Email = "lkphieu@militarygeo.vn", Phone = "0912000030", Department = "Ban Chỉ huy", Position = "Trực chỉ huy", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-230) },
                new UserInfo { Id = 43, Username = "user_43", FullName = "Nông Đức Mạnh", Email = "ndmanh@militarygeo.vn", Phone = "0912000031", Department = "Phòng Hậu cần", Position = "Cán bộ kho", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-240) },
                new UserInfo { Id = 44, Username = "user_44", FullName = "Trần Đức Lương", Email = "tdluong@militarygeo.vn", Phone = "0912000032", Department = "Phòng Kỹ thuật", Position = "Giám sát", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-250) },
                new UserInfo { Id = 45, Username = "user_45", FullName = "Nguyễn Minh Triết", Email = "nmtriet@militarygeo.vn", Phone = "0912000033", Department = "Phòng Tác chiến", Position = "Hoa tiêu", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-260) },
                new UserInfo { Id = 46, Username = "user_46", FullName = "Trương Tấn Sang", Email = "ttsang@militarygeo.vn", Phone = "0912000034", Department = "Phòng Tài chính", Position = "Kiểm soát viên", Role = "Manager", Status = "Khóa", CreatedDate = DateTime.Now.AddDays(-270) },
                new UserInfo { Id = 47, Username = "user_47", FullName = "Nguyễn Tấn Dũng", Email = "ntdung@militarygeo.vn", Phone = "0912000035", Department = "Ban Chỉ huy", Position = "Điều phối viên", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-280) },
                new UserInfo { Id = 48, Username = "user_48", FullName = "Nguyễn Phú Trọng", Email = "nptrong@militarygeo.vn", Phone = "0912000036", Department = "Ban Chỉ huy", Position = "Tổng đạo diễn", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-290) },
                new UserInfo { Id = 49, Username = "user_49", FullName = "Lương Cường", Email = "lcuong@militarygeo.vn", Phone = "0912000037", Department = "Ban Chỉ huy", Position = "Chủ nhiệm", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-300) },
                new UserInfo { Id = 50, Username = "user_50", FullName = "Nguyễn Duy Ngọc", Email = "ndngoc@militarygeo.vn", Phone = "0912000038", Department = "Phòng Tác chiến", Position = "Phó tư lệnh", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-310) },
                new UserInfo { Id = 51, Username = "user_51", FullName = "Trần Cẩm Tú", Email = "tctu@militarygeo.vn", Phone = "0912000039", Department = "Phòng Kế hoạch", Position = "Kế hoạch viên", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-320) },
                new UserInfo { Id = 52, Username = "user_52", FullName = "Phan Đình Trạc", Email = "pdtrac@militarygeo.vn", Phone = "0912000040", Department = "Phòng Hành chính", Position = "Pháp chế", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-330) },
                new UserInfo { Id = 53, Username = "user_53", FullName = "Lê Minh Hưng", Email = "lmhung@militarygeo.vn", Phone = "0912000041", Department = "Phòng Tài chính", Position = "Thủ quỹ", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-340) },
                new UserInfo { Id = 54, Username = "user_54", FullName = "Đỗ Văn Chiến", Email = "dvchien@militarygeo.vn", Phone = "0912000042", Department = "Phòng Hậu cần", Position = "Điều hành vận tải", Role = "User", Status = "Khóa", CreatedDate = DateTime.Now.AddDays(-350) },
                new UserInfo { Id = 55, Username = "user_55", FullName = "Nguyễn Xuân Thắng", Email = "nxthang@militarygeo.vn", Phone = "0912000043", Department = "Ban Chỉ huy", Position = "Giám đốc đào tạo", Role = "Admin", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-360) },
                new UserInfo { Id = 56, Username = "user_56", FullName = "Chu Ngọc Anh", Email = "cnanh@militarygeo.vn", Phone = "0912000044", Department = "Phòng Kỹ thuật", Position = "Nhân viên bảo trì", Role = "User", Status = "Khóa", CreatedDate = DateTime.Now.AddDays(-370) },
                new UserInfo { Id = 57, Username = "user_57", FullName = "Đinh Tiến Dũng", Email = "dtdung@militarygeo.vn", Phone = "0912000045", Department = "Phòng Kế hoạch", Position = "Thẩm định", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-380) },
                new UserInfo { Id = 58, Username = "user_58", FullName = "Nguyễn Thanh Nghị", Email = "ntnghi@militarygeo.vn", Phone = "0912000046", Department = "Phòng Kỹ thuật", Position = "Kỹ sư trẻ", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-390) },
                new UserInfo { Id = 59, Username = "user_59", FullName = "Đặng Quốc Khánh", Email = "dqkhanh@militarygeo.vn", Phone = "0912000047", Department = "Phòng Tác chiến", Position = "Sĩ quan dự bị", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-400) },
                new UserInfo { Id = 60, Username = "user_60", FullName = "Nguyễn Hải Ninh", Email = "nhninh@militarygeo.vn", Phone = "0912000048", Department = "Phòng Tác chiến", Position = "Điều phối tác chiến", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-410) },
                new UserInfo { Id = 61, Username = "user_61", FullName = "Lê Quốc Minh", Email = "lqminh@militarygeo.vn", Phone = "0912000049", Department = "Phòng Hành chính", Position = "Truyền thông", Role = "User", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-420) },
                new UserInfo { Id = 62, Username = "user_62", FullName = "Phạm Tất Thắng", Email = "ptthang@militarygeo.vn", Phone = "0912000050", Department = "Phòng Kế hoạch", Position = "Phó quản lý", Role = "Manager", Status = "Hoạt động", CreatedDate = DateTime.Now.AddDays(-430) }
            };

            ApplyFilters();
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Lỗi khi khởi tạo dữ liệu: {ex.Message}");
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        CurrentPage = 1;
        ApplyFilters();
    }

    partial void OnSelectedRoleChanged(string value)
    {
        CurrentPage = 1;
        ApplyFilters();
    }

    partial void OnSelectedStatusChanged(string value)
    {
        CurrentPage = 1;
        ApplyFilters();
    }

    partial void OnPageSizeChanged(int value)
    {
        CurrentPage = 1;
        ApplyPagination();
    }

    partial void OnCurrentPageChanged(int value)
    {
        ApplyPagination();
    }

    private void ApplyFilters()
    {
        var searchLower = SearchText?.ToLower() ?? string.Empty;

        _filteredUsers = new ObservableCollection<UserInfo>(
            _allUsers.Where(u =>
                (string.IsNullOrWhiteSpace(searchLower) ||
                 u.FullName.ToLower().Contains(searchLower) ||
                 u.Email.ToLower().Contains(searchLower) ||
                 u.Phone.Contains(searchLower) ||
                 u.Username.ToLower().Contains(searchLower)) &&
                (SelectedRole == "Tất cả vai trò" || u.Role == SelectedRole) &&
                (SelectedStatus == "Tất cả trạng thái" || u.Status == SelectedStatus)
            )
        );

        ApplyPagination();
    }

    private void ApplyPagination()
    {
        TotalPages = (int)Math.Ceiling((double)_filteredUsers.Count / PageSize);
        if (TotalPages == 0) TotalPages = 1;

        // Ensure current page is valid
        if (CurrentPage > TotalPages)
            CurrentPage = TotalPages;
        if (CurrentPage < 1)
            CurrentPage = 1;

        var pagedData = _filteredUsers
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        PagedUsers = new ObservableCollection<UserInfo>(pagedData);

        UpdatePaginationInfo();
    }

    private void UpdatePaginationInfo()
    {
        var startIndex = (CurrentPage - 1) * PageSize + 1;
        var endIndex = Math.Min(CurrentPage * PageSize, _filteredUsers.Count);
        var total = _filteredUsers.Count;

        PageInfo = total > 0
            ? $"Hiển thi {startIndex}-{endIndex} của {total} kết quả"
            : "Không có kết quả";

        TotalUsersText = $"Tổng số: {_allUsers.Count} người dùng";

        CanGoToPreviousPage = CurrentPage > 1;
        CanGoToNextPage = CurrentPage < TotalPages;
    }

    [RelayCommand]
    private void FirstPage()
    {
        CurrentPage = 1;
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (CurrentPage > 1)
            CurrentPage--;
    }

    [RelayCommand]
    private void NextPage()
    {
        if (CurrentPage < TotalPages)
            CurrentPage++;
    }

    [RelayCommand]
    private void LastPage()
    {
        CurrentPage = TotalPages;
    }

    [RelayCommand]
    private void Refresh()
    {
        SearchText = string.Empty;
        SelectedRole = "Tất cả vai trò";
        SelectedStatus = "Tất cả trạng thái";
        CurrentPage = 1;
        PageSize = 20;
        LoadUserDataAsync();
    }

    [RelayCommand]
    private void Add()
    {
        try
        {
            var viewModel = App.GetService<AddNguoiDungViewModel>();
            var addWindow = new Popup.NguoiDung.AddNguoiDungWindow(viewModel);
            addWindow.ShowDialog();

            if (viewModel.IsSuccess && viewModel.CreatedUser != null)
            {
                var newUser = new UserInfo
                {
                    Id = viewModel.CreatedUser.Id,
                    Username = viewModel.CreatedUser.Username,
                    FullName = viewModel.CreatedUser.FullName,
                    Email = viewModel.CreatedUser.Email,
                    Phone = viewModel.CreatedUser.Phone,
                    Department = viewModel.CreatedUser.Department,
                    Position = viewModel.CreatedUser.Position,
                    Role = viewModel.CreatedUser.Role,
                    Status = viewModel.CreatedUser.Status,
                    CreatedDate = viewModel.CreatedUser.CreatedDate.DateTime
                };

                _allUsers.Add(newUser);
                ApplyFilters();
            }
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Lỗi khi thêm người dùng: {ex.Message}");
        }
    }

    [RelayCommand]
    private void Edit(UserInfo user)
    {
        if (user == null) return;

        _messageService.ShowInfo($"Chức năng chỉnh sửa người dùng '{user.FullName}' đang phát triển!");
    }

    [RelayCommand]
    private void Delete(UserInfo user)
    {
        if (user == null) return;

        var isConfirmed = _messageService.ShowConfirmation(
            $"Bạn có chắc muốn xóa người dùng '{user.FullName}'?",
            "Xác nhận xóa");

        if (isConfirmed)
        {
            _allUsers.Remove(user);
            ApplyFilters();
            _messageService.ShowInfo("Đã xóa người dùng thành công!");
        }
    }

    [RelayCommand]
    private void Export()
    {
        _messageService.ShowInfo("Chức năng kết xuất đang được phát triển!");
    }

    public void AddUser(UserInfo user)
    {
        _allUsers.Add(user);
        ApplyFilters();
    }
}

public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
