using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MilitaryGeo.Application.Interfaces;
using MilitaryGeo.Domain.Entities;
using System.Collections.ObjectModel;

namespace MilitaryGeo.Desktop.ViewModels;

public partial class VaiTroViewModel : ObservableObject
{
    private readonly IMessageService _messageService;

    // Collections
    private ObservableCollection<VaiTro> allRoles = new();
    private ObservableCollection<VaiTro> filteredRoles = new();

    [ObservableProperty]
    private ObservableCollection<VaiTro> pagedRoles = new();

    // Search
    [ObservableProperty]
    private string searchText = string.Empty;

    // Pagination
    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int pageSize = 20;

    [ObservableProperty]
    private int totalPages = 1;

    [ObservableProperty]
    private string pageInfo = "Hiển thị 1-20 của 100 kết quả";

    [ObservableProperty]
    private string totalRolesText = "Tổng số: 0 vai trò";

    // Pagination Button States
    [ObservableProperty]
    private bool canGoToPreviousPage;

    [ObservableProperty]
    private bool canGoToNextPage;

    public ObservableCollection<int> PageSizes { get; } = new()
    {
        10, 20, 50, 100
    };

    public VaiTroViewModel(IMessageService messageService)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));

        System.Diagnostics.Debug.WriteLine("=== VaiTroViewModel Constructor Called ===");

        LoadRoleData();

        System.Diagnostics.Debug.WriteLine($"=== Loaded {allRoles.Count} roles ===");
        System.Diagnostics.Debug.WriteLine($"=== PagedRoles count: {PagedRoles.Count} ===");
    }

    private void LoadRoleData()
    {
        try
        {
            // Load fake data for testing
            allRoles = new ObservableCollection<VaiTro>
            {
                new VaiTro
                {
                    Id = 1,
                    MaVaiTro = "ADMIN",
                    TenVaiTro = "Quản trị viên",
                    MoTa = "Có toàn quyền truy cập và quản lý hệ thống",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":true,\"NguoiDung\":true,\"VaiTro\":true,\"CaiDat\":true,\"BaoCao\":true}",
                    ThuTu = 1,
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new VaiTro
                {
                    Id = 2,
                    MaVaiTro = "USER",
                    TenVaiTro = "Người dùng",
                    MoTa = "Quyền sử dụng cơ bản của hệ thống",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":false,\"NguoiDung\":false,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":true}",
                    ThuTu = 2,
                    CreatedDate = DateTime.Now.AddDays(-25)
                },
                new VaiTro
                {
                    Id = 3,
                    MaVaiTro = "MANAGER",
                    TenVaiTro = "Quản lý",
                    MoTa = "Quản lý dữ liệu và người dùng trong hệ thống",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":true,\"NguoiDung\":true,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":true}",
                    ThuTu = 3,
                    CreatedDate = DateTime.Now.AddDays(-20)
                },
                new VaiTro
                {
                    Id = 4,
                    MaVaiTro = "VIEWER",
                    TenVaiTro = "Người xem",
                    MoTa = "Chỉ được xem thông tin, không được chỉnh sửa",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":false,\"NguoiDung\":false,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":true}",
                    ThuTu = 4,
                    CreatedDate = DateTime.Now.AddDays(-15)
                },
                new VaiTro
                {
                    Id = 5,
                    MaVaiTro = "ANALYST",
                    TenVaiTro = "Phân tích viên",
                    MoTa = "Phân tích dữ liệu và tạo báo cáo hệ thống",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":true,\"NguoiDung\":false,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":true}",
                    ThuTu = 5,
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                new VaiTro
                {
                    Id = 6,
                    MaVaiTro = "GUEST",
                    TenVaiTro = "Khách",
                    MoTa = "Quyền truy cập hạn chế, chỉ xem thông tin cơ bản",
                    IsActive = false,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":false,\"DuLieu\":false,\"NguoiDung\":false,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":false}",
                    ThuTu = 6,
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new VaiTro
                {
                    Id = 7,
                    MaVaiTro = "SUPERVISOR",
                    TenVaiTro = "Giám sát viên",
                    MoTa = "Giám sát hoạt động và theo dõi hiệu suất hệ thống",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":true,\"NguoiDung\":true,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":true}",
                    ThuTu = 7,
                    CreatedDate = DateTime.Now.AddDays(-8)
                },
                new VaiTro
                {
                    Id = 8,
                    MaVaiTro = "OPERATOR",
                    TenVaiTro = "Vận hành viên",
                    MoTa = "Vận hành và xử lý nghiệp vụ hàng ngày",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":true,\"NguoiDung\":false,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":true}",
                    ThuTu = 8,
                    CreatedDate = DateTime.Now.AddDays(-6)
                },
                new VaiTro
                {
                    Id = 9,
                    MaVaiTro = "AUDITOR",
                    TenVaiTro = "Kiểm toán viên",
                    MoTa = "Kiểm tra và đánh giá các hoạt động hệ thống",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":false,\"NguoiDung\":false,\"VaiTro\":false,\"CaiDat\":false,\"BaoCao\":true}",
                    ThuTu = 9,
                    CreatedDate = DateTime.Now.AddDays(-4)
                },
                new VaiTro
                {
                    Id = 10,
                    MaVaiTro = "SUPPORT",
                    TenVaiTro = "Hỗ trợ kỹ thuật",
                    MoTa = "Hỗ trợ người dùng và xử lý sự cố kỹ thuật",
                    IsActive = true,
                    Quyen = "{\"Dashboard\":true,\"BanDo\":true,\"DuLieu\":false,\"NguoiDung\":true,\"VaiTro\":false,\"CaiDat\":true,\"BaoCao\":false}",
                    ThuTu = 10,
                    CreatedDate = DateTime.Now.AddDays(-2)
                }
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

        filteredRoles = new ObservableCollection<VaiTro>(
            allRoles.Where(r =>
                string.IsNullOrWhiteSpace(searchLower) ||
                r.MaVaiTro.ToLower().Contains(searchLower) ||
                r.TenVaiTro.ToLower().Contains(searchLower) ||
                (r.MoTa != null && r.MoTa.ToLower().Contains(searchLower))
            )
        );

        ApplyPagination();
    }

    private void ApplyPagination()
    {
        TotalPages = (int)Math.Ceiling((double)filteredRoles.Count / PageSize);
        if (TotalPages == 0) TotalPages = 1;

        // Ensure current page is valid
        if (CurrentPage > TotalPages)
            CurrentPage = TotalPages;
        if (CurrentPage < 1)
            CurrentPage = 1;

        var pagedData = filteredRoles
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        PagedRoles = new ObservableCollection<VaiTro>(pagedData);

        UpdatePaginationInfo();
    }

    private void UpdatePaginationInfo()
    {
        var startIndex = filteredRoles.Count > 0 ? (CurrentPage - 1) * PageSize + 1 : 0;
        var endIndex = Math.Min(CurrentPage * PageSize, filteredRoles.Count);
        var total = filteredRoles.Count;

        PageInfo = total > 0
            ? $"Hiển thị {startIndex}-{endIndex} của {total} kết quả"
            : "Không có kết quả";

        TotalRolesText = $"Tổng số: {allRoles.Count} vai trò";

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
        CurrentPage = 1;
        PageSize = 20;
        LoadRoleData();
    }

    [RelayCommand]
    private void Add()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== Add Command Executed ===");

            var viewModel = App.GetService<AddVaiTroViewModel>();
            System.Diagnostics.Debug.WriteLine("=== AddVaiTroViewModel retrieved from DI ===");

            var addWindow = new Popup.VaiTro.AddVaiTroWindow(viewModel);
            System.Diagnostics.Debug.WriteLine("=== AddVaiTroWindow created ===");

            addWindow.ShowDialog();

            if (viewModel.IsSuccess && viewModel.CreatedRole != null)
            {
                var newRole = viewModel.CreatedRole;
                newRole.Id = allRoles.Count > 0 ? allRoles.Max(r => r.Id) + 1 : 1;
                allRoles.Add(newRole);
                ApplyFilters();
                _messageService.ShowInfo($"Đã thêm vai trò '{newRole.TenVaiTro}' thành công!");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== ERROR in Add Command: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== Stack Trace: {ex.StackTrace} ===");
            _messageService.ShowError($"Lỗi khi thêm vai trò: {ex.Message}");
        }
    }

    [RelayCommand]
    private void Edit(VaiTro role)
    {
        if (role == null) return;

        try
        {
            var viewModel = App.GetService<AddVaiTroViewModel>();
            viewModel.LoadRole(role);

            var editWindow = new Popup.VaiTro.AddVaiTroWindow(viewModel);
            editWindow.Title = "Chỉnh sửa vai trò";
            editWindow.ShowDialog();

            if (viewModel.IsSuccess)
            {
                // Refresh the list to show updated data
                var index = allRoles.IndexOf(role);
                if (index >= 0)
                {
                    allRoles[index] = role;
                }
                ApplyFilters();
                _messageService.ShowInfo($"Đã cập nhật vai trò '{role.TenVaiTro}' thành công!");
            }
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Lỗi khi chỉnh sửa vai trò: {ex.Message}");
        }
    }

    [RelayCommand]
    private void Delete(VaiTro role)
    {
        if (role == null) return;

        var isConfirmed = _messageService.ShowConfirmation(
            $"Bạn có chắc muốn xóa vai trò '{role.TenVaiTro}'?",
            "Xác nhận xóa");

        if (isConfirmed)
        {
            allRoles.Remove(role);
            ApplyFilters();
            _messageService.ShowInfo("Đã xóa vai trò thành công!");
        }
    }

    [RelayCommand]
    private void Export()
    {
        _messageService.ShowInfo("Chức năng xuất Excel đang được phát triển!");
    }
}