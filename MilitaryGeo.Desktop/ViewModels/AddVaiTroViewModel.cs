using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MilitaryGeo.Application.Interfaces;
using MilitaryGeo.Domain.Entities;

namespace MilitaryGeo.Desktop.ViewModels;

public partial class AddVaiTroViewModel : ObservableObject
{
    private readonly IMessageService _messageService;

    [ObservableProperty]
    private string maVaiTro = string.Empty;

    [ObservableProperty]
    private string tenVaiTro = string.Empty;

    [ObservableProperty]
    private string moTa = string.Empty;

    [ObservableProperty]
    private bool isActive = true;

    [ObservableProperty]
    private int thuTu = 0;

    [ObservableProperty]
    private string ghiChu = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    // Permissions
    [ObservableProperty]
    private bool quyenDashboard = false;

    [ObservableProperty]
    private bool quyenBanDo = false;

    [ObservableProperty]
    private bool quyenDuLieu = false;

    [ObservableProperty]
    private bool quyenNguoiDung = false;

    [ObservableProperty]
    private bool quyenVaiTro = false;

    [ObservableProperty]
    private bool quyenCaiDat = false;

    [ObservableProperty]
    private bool quyenBaoCao = false;

    public bool IsSuccess { get; private set; }
    public VaiTro? CreatedRole { get; private set; }
    private VaiTro? _editingRole;

    public AddVaiTroViewModel(IMessageService messageService)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    public void LoadRole(VaiTro role)
    {
        _editingRole = role;
        MaVaiTro = role.MaVaiTro;
        TenVaiTro = role.TenVaiTro;
        MoTa = role.MoTa;
        IsActive = role.IsActive;
        ThuTu = role.ThuTu;
        GhiChu = role.GhiChu ?? string.Empty;

        ParsePermissions(role.Quyen);
    }

    [RelayCommand]
    private void Save()
    {
        try
        {
            IsBusy = true;

            // Validate
            if (string.IsNullOrWhiteSpace(MaVaiTro))
            {
                _messageService.ShowError("Vui lòng nh?p mã vai trò!");
                return;
            }

            if (string.IsNullOrWhiteSpace(TenVaiTro))
            {
                _messageService.ShowError("Vui lòng nh?p tên vai trò!");
                return;
            }

            var permissions = BuildPermissionsJson();

            if (_editingRole == null)
            {
                // Create new
                CreatedRole = new VaiTro
                {
                    MaVaiTro = MaVaiTro.Trim().ToUpper(),
                    TenVaiTro = TenVaiTro.Trim(),
                    MoTa = MoTa.Trim(),
                    IsActive = IsActive,
                    Quyen = permissions,
                    ThuTu = ThuTu,
                    GhiChu = GhiChu.Trim(),
                    CreatedDate = DateTime.Now
                };
            }
            else
            {
                // Update existing
                _editingRole.MaVaiTro = MaVaiTro.Trim().ToUpper();
                _editingRole.TenVaiTro = TenVaiTro.Trim();
                _editingRole.MoTa = MoTa.Trim();
                _editingRole.IsActive = IsActive;
                _editingRole.Quyen = permissions;
                _editingRole.ThuTu = ThuTu;
                _editingRole.GhiChu = GhiChu.Trim();
                _editingRole.ModifiedDate = DateTime.Now;

                CreatedRole = _editingRole;
            }

            IsSuccess = true;
            OnCloseRequested();
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"L?i: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        var confirmed = _messageService.ShowConfirmation(
            "B?n có ch?c ch?n mu?n h?y? D? li?u ?ã nh?p s? không ???c l?u.",
            "Xác nh?n h?y");

        if (confirmed)
        {
            OnCloseRequested();
        }
    }

    private void ParsePermissions(string permissionsJson)
    {
        try
        {
            QuyenDashboard = permissionsJson.Contains("\"Dashboard\":true");
            QuyenBanDo = permissionsJson.Contains("\"BanDo\":true");
            QuyenDuLieu = permissionsJson.Contains("\"DuLieu\":true");
            QuyenNguoiDung = permissionsJson.Contains("\"NguoiDung\":true");
            QuyenVaiTro = permissionsJson.Contains("\"VaiTro\":true");
            QuyenCaiDat = permissionsJson.Contains("\"CaiDat\":true");
            QuyenBaoCao = permissionsJson.Contains("\"BaoCao\":true");
        }
        catch
        {
            // If parsing fails, set all to false
        }
    }

    private string BuildPermissionsJson()
    {
        return $"{{\"Dashboard\":{QuyenDashboard.ToString().ToLower()}," +
               $"\"BanDo\":{QuyenBanDo.ToString().ToLower()}," +
               $"\"DuLieu\":{QuyenDuLieu.ToString().ToLower()}," +
               $"\"NguoiDung\":{QuyenNguoiDung.ToString().ToLower()}," +
               $"\"VaiTro\":{QuyenVaiTro.ToString().ToLower()}," +
               $"\"CaiDat\":{QuyenCaiDat.ToString().ToLower()}," +
               $"\"BaoCao\":{QuyenBaoCao.ToString().ToLower()}}}";
    }

    public event EventHandler? CloseRequested;
    private void OnCloseRequested() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
