# H??NG D?N CH?C N?NG QU?N LÝ VAI TRÒ

## T?ng quan
Ch?c n?ng qu?n lý vai trò ?ã ???c hoàn thi?n v?i các tính n?ng CRUD c? b?n (Create, Read, Update, Delete).

## Các file ?ã t?o

### 1. Domain Layer
- `MilitaryGeo.Domain\Entities\VaiTro.cs` - Entity ??nh ngh?a vai trò

### 2. Desktop Layer
- `MilitaryGeo.Desktop\UserControls\VaiTro.xaml` - View hi?n th? danh sách vai trò
- `MilitaryGeo.Desktop\UserControls\VaiTro.xaml.cs` - Code-behind
- `MilitaryGeo.Desktop\ViewModels\VaiTroViewModel.cs` - ViewModel x? lý logic

### 3. Configuration
- ?ã ??ng ký `VaiTroViewModel` trong `App.xaml.cs`
- ?ã thêm menu "Qu?n lý vai trò" vào sidebar

## C?u trúc VaiTro Entity

```csharp
public class VaiTro : EntityAuditBase<int>
{
    public string MaVaiTro { get; set; }        // Mã vai trò (ADMIN, USER, MANAGER...)
    public string TenVaiTro { get; set; }       // Tên vai trò
    public string MoTa { get; set; }            // Mô t?
    public bool IsActive { get; set; }          // Tr?ng thái kích ho?t
    public string Quyen { get; set; }           // JSON string ch?a các quy?n
    public int ThuTu { get; set; }              // Th? t? hi?n th?
    public string? GhiChu { get; set; }         // Ghi chú
}
```

## Fake Data hi?n có

Hi?n t?i có 6 vai trò m?u:
1. **ADMIN** - Qu?n tr? viên (Full quy?n)
2. **USER** - Ng??i dùng (Quy?n c? b?n)
3. **MANAGER** - Qu?n lý (Qu?n lý d? li?u và ng??i dùng)
4. **VIEWER** - Ng??i xem (Ch? xem)
5. **ANALYST** - Phân tích viên (Phân tích và báo cáo)
6. **GUEST** - Khách (Quy?n h?n ch? - T?t)

## Các ch?c n?ng ?ã implement

### ? Hoàn thành
1. **Hi?n th? danh sách** - DataGrid v?i pagination
2. **Tìm ki?m** - Theo mã, tên, mô t? vai trò
3. **Phân trang** - 10/20/50/100 items per page
4. **Xóa vai trò** - V?i confirmation dialog
5. **Làm m?i** - Refresh danh sách

### ?? ?ang phát tri?n (Hi?n th? thông báo placeholder)
1. **Thêm vai trò m?i**
2. **Ch?nh s?a vai trò**
3. **Xu?t Excel**

## Format JSON Quy?n (Permissions)

```json
{
  "Dashboard": true,
  "BanDo": true,
  "DuLieu": true,
  "NguoiDung": true,
  "VaiTro": true,
  "CaiDat": true,
  "BaoCao": true
}
```

## Cách s? d?ng

### 1. Truy c?p ch?c n?ng
- ??ng nh?p vào h? th?ng
- Click menu "Qu?n lý vai trò" trên sidebar

### 2. Tìm ki?m vai trò
- Nh?p t? khóa vào ô tìm ki?m
- H? th?ng t? ??ng filter khi b?n nh?p

### 3. Phân trang
- Ch?n s? l??ng items/trang t? dropdown
- S? d?ng các nút ?i?u h??ng ?? chuy?n trang

### 4. Xóa vai trò
- Click nút xóa (icon thùng rác) trên row
- Confirm trong dialog
- Vai trò s? b? xóa kh?i danh sách

## Tích h?p API sau này

### B??c 1: T?o Service Interface

```csharp
// MilitaryGeo.Application\Interfaces\IVaiTroService.cs
public interface IVaiTroService
{
    Task<List<VaiTroDto>> GetAllAsync();
    Task<VaiTroDto> GetByIdAsync(int id);
    Task<VaiTroDto> CreateAsync(CreateVaiTroDto dto);
    Task<VaiTroDto> UpdateAsync(int id, UpdateVaiTroDto dto);
    Task<bool> DeleteAsync(int id);
}
```

### B??c 2: T?o DTOs

```csharp
// MilitaryGeo.Application\DTOs\VaiTro\VaiTroDto.cs
public class VaiTroDto
{
    public int Id { get; set; }
    public string MaVaiTro { get; set; }
    public string TenVaiTro { get; set; }
    public string MoTa { get; set; }
    public bool IsActive { get; set; }
    public string Quyen { get; set; }
    public int ThuTu { get; set; }
    public string? GhiChu { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

public class CreateVaiTroDto
{
    public string MaVaiTro { get; set; }
    public string TenVaiTro { get; set; }
    public string MoTa { get; set; }
    public bool IsActive { get; set; } = true;
    public string Quyen { get; set; }
    public int ThuTu { get; set; }
    public string? GhiChu { get; set; }
}

public class UpdateVaiTroDto
{
    public string TenVaiTro { get; set; }
    public string MoTa { get; set; }
    public bool IsActive { get; set; }
    public string Quyen { get; set; }
    public int ThuTu { get; set; }
    public string? GhiChu { get; set; }
}
```

### B??c 3: Update ViewModel

```csharp
// Trong VaiTroViewModel.cs
private readonly IVaiTroService _vaiTroService;

public VaiTroViewModel(IMessageService messageService, IVaiTroService vaiTroService)
{
    _messageService = messageService;
    _vaiTroService = vaiTroService;
    LoadRoleDataAsync(); // Change to async
}

private async Task LoadRoleDataAsync()
{
    try
    {
        var roles = await _vaiTroService.GetAllAsync();
        allRoles = new ObservableCollection<VaiTro>(
            roles.Select(r => new VaiTro
            {
                Id = r.Id,
                MaVaiTro = r.MaVaiTro,
                TenVaiTro = r.TenVaiTro,
                MoTa = r.MoTa,
                IsActive = r.IsActive,
                Quyen = r.Quyen,
                ThuTu = r.ThuTu,
                GhiChu = r.GhiChu,
                CreatedDate = r.CreatedDate.DateTime
            })
        );
        ApplyFilters();
    }
    catch (Exception ex)
    {
        _messageService.ShowError($"Loi khi tai du lieu: {ex.Message}");
    }
}

[RelayCommand]
private async Task DeleteAsync(VaiTro role)
{
    if (role == null) return;

    var isConfirmed = _messageService.ShowConfirmation(
        $"Ban co chac muon xoa vai tro '{role.TenVaiTro}'?",
        "Xac nhan xoa");

    if (isConfirmed)
    {
        try
        {
            await _vaiTroService.DeleteAsync(role.Id);
            allRoles.Remove(role);
            ApplyFilters();
            _messageService.ShowInfo("Da xoa vai tro thanh cong!");
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Loi khi xoa: {ex.Message}");
        }
    }
}
```

### B??c 4: ??ng ký Service

```csharp
// MilitaryGeo.Infrastructure\DI\DependencyInjection.cs
public static IServiceCollection AddInfrastructure(
    this IServiceCollection services, 
    IConfiguration configuration)
{
    // ...existing code...
    
    services.AddSingleton<IVaiTroService, VaiTroService>();
    
    return services;
}
```

## T?o Dialog Add/Edit

### T?o AddVaiTroViewModel.cs

```csharp
public partial class AddVaiTroViewModel : ObservableObject
{
    private readonly IVaiTroService _vaiTroService;
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

    // Permissions
    [ObservableProperty]
    private bool quyenDashboard = false;

    [ObservableProperty]
    private bool quyenBanDo = false;

    // ... other permissions ...

    public bool IsSuccess { get; private set; }
    public VaiTroDto? CreatedRole { get; private set; }

    public AddVaiTroViewModel(IVaiTroService vaiTroService, IMessageService messageService)
    {
        _vaiTroService = vaiTroService;
        _messageService = messageService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            var permissions = BuildPermissionsJson();
            
            var dto = new CreateVaiTroDto
            {
                MaVaiTro = MaVaiTro,
                TenVaiTro = TenVaiTro,
                MoTa = MoTa,
                IsActive = IsActive,
                Quyen = permissions,
                ThuTu = ThuTu,
                GhiChu = GhiChu
            };

            CreatedRole = await _vaiTroService.CreateAsync(dto);
            IsSuccess = true;
            
            _messageService.ShowInfo("Them vai tro thanh cong!");
            OnCloseRequested();
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Loi: {ex.Message}");
        }
    }

    private string BuildPermissionsJson()
    {
        return $"{{\"Dashboard\":{QuyenDashboard.ToString().ToLower()}," +
               $"\"BanDo\":{QuyenBanDo.ToString().ToLower()}}}";
    }

    public event EventHandler? CloseRequested;
    private void OnCloseRequested() => CloseRequested?.Invoke(this, EventArgs.Empty);
}
```

## Testing

### Test checklist
- [ ] Hi?n th? danh sách vai trò
- [ ] Tìm ki?m ho?t ??ng
- [ ] Phân trang chuy?n trang ?úng
- [ ] Xóa vai trò thành công
- [ ] Làm m?i danh sách
- [ ] Responsive UI
- [ ] Error handling

## L?u ý quan tr?ng

1. **Encoding**: File XAML ?ã s? d?ng HTML entities (&#...) ?? tránh l?i encoding v?i ti?ng Vi?t
2. **Fake Data**: Hi?n ?ang dùng fake data, c?n thay b?ng API call sau này
3. **Permissions**: Format JSON có th? customize theo nhu c?u
4. **Validation**: Ch?a có validation cho form, c?n thêm khi implement Add/Edit
5. **Authorization**: C?n ki?m tra quy?n user tr??c khi cho phép CRUD

## Roadmap

### Phase 1 (?ã hoàn thành) ?
- [x] Entity VaiTro
- [x] ViewModel v?i fake data
- [x] View danh sách
- [x] Search & Pagination
- [x] Delete function

### Phase 2 (Ti?p theo)
- [ ] Add/Edit Dialog v?i form ??y ??
- [ ] Validation
- [ ] Permission management UI
- [ ] API Integration
- [ ] Export Excel

### Phase 3 (T??ng lai)
- [ ] Role assignment to users
- [ ] Role-based access control
- [ ] Audit log for role changes
- [ ] Advanced permissions matrix

---

**Tác gi?**: GitHub Copilot  
**Ngày t?o**: {DateTime.Now:dd/MM/yyyy}  
**Version**: 1.0
