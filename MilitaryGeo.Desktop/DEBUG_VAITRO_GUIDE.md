# H??NG D?N DEBUG VÀ KH?C PH?C S? C?

## V?N ?? B?N G?P PH?I

1. **Không hi?n th? danh sách vai trò** 
2. **Nút "Thêm vai trò" không m? form**

## ?Ã S?A

### 1. File `VaiTro.xaml.cs`

**Tr??c ?ây:**
```csharp
public VaiTro()
{
    InitializeComponent();
    // THI?U: Không set DataContext
}
```

**Bây gi?:**
```csharp
public VaiTro()
{
    InitializeComponent();
    
    // ? ?ã thêm: Get ViewModel from DI và set DataContext
    DataContext = App.GetService<VaiTroViewModel>();
}
```

**T?i sao c?n?**
- Không có DataContext ? View không bind ???c v?i ViewModel
- Không bind ? Commands không ho?t ??ng (nút Thêm không làm gì)
- Không bind ? Data không hi?n th? (danh sách tr?ng)

## CÁCH KI?M TRA

### 1. Ch?y ?ng d?ng trong Debug Mode

```bash
# Trong Visual Studio
F5 ho?c Debug > Start Debugging
```

### 2. M? Output Window

```
View > Output
ho?c Ctrl + Alt + O
```

### 3. Tìm các dòng debug log:

Khi m? trang Vai Trò, b?n s? th?y:
```
=== VaiTroViewModel Constructor Called ===
=== Loaded 10 roles ===
=== PagedRoles count: 10 ===
```

N?u KHÔNG th?y ? ViewModel không ???c kh?i t?o ? Ki?m tra l?i DataContext

### 4. Click nút "Thêm vai trò"

B?n s? th?y:
```
=== Add Command Executed ===
=== AddVaiTroViewModel retrieved from DI ===
=== AddVaiTroWindow created ===
```

N?u có l?i:
```
=== ERROR in Add Command: [Message] ===
=== Stack Trace: [Stack] ===
```

## CHECKLIST KH?C PH?C

### ? ?ã hoàn thành:

- [x] Set DataContext trong `VaiTro.xaml.cs`
- [x] ??ng ký `VaiTroViewModel` trong DI (`App.xaml.cs`)
- [x] ??ng ký `AddVaiTroViewModel` trong DI
- [x] T?o fake data (10 vai trò)
- [x] Implement Add command
- [x] T?o `AddVaiTroWindow.xaml` và `.xaml.cs`
- [x] Thêm debug logging

### ?? N?u v?n không ho?t ??ng:

#### 1. Ki?m tra DataContext ?ã ???c set ch?a

Thêm vào `VaiTro.xaml.cs`:
```csharp
public VaiTro()
{
    InitializeComponent();
    DataContext = App.GetService<VaiTroViewModel>();
    
    // Debug
    System.Diagnostics.Debug.WriteLine($"DataContext type: {DataContext?.GetType().Name}");
}
```

N?u in ra `null` ? ViewModel không ???c inject ?úng.

#### 2. Ki?m tra DI Registration

Trong `App.xaml.cs`, ??m b?o có:
```csharp
services.AddTransient<VaiTroViewModel>();
services.AddTransient<AddVaiTroViewModel>();
```

#### 3. Ki?m tra XAML Binding

Trong `VaiTro.xaml`, ??m b?o:
```xaml
<!-- PagedRoles ph?i match v?i property trong ViewModel -->
<syncfusion:SfDataGrid ItemsSource="{Binding PagedRoles}">

<!-- Commands ph?i match v?i RelayCommand trong ViewModel -->
<Button Command="{Binding AddCommand}"/>
<Button Command="{Binding EditCommand}"/>
<Button Command="{Binding DeleteCommand}"/>
```

#### 4. Ki?m tra Entity VaiTro

File `VaiTro.cs` ph?i có ??y ?? properties:
```csharp
public class VaiTro : EntityAuditBase<int>
{
    public string MaVaiTro { get; set; }
    public string TenVaiTro { get; set; }
    public string MoTa { get; set; }
    public bool IsActive { get; set; }
    public string Quyen { get; set; }
    public int ThuTu { get; set; }
    public string? GhiChu { get; set; }
}
```

#### 5. Ki?m tra MessageService

??m b?o `IMessageService` ?ã ???c ??ng ký:
```csharp
services.AddSingleton<IMessageService, MessageService>();
```

## TEST T?NG B??C

### B??c 1: Test DataContext
1. ??t breakpoint t?i `VaiTro.xaml.cs` constructor
2. Ch?y app
3. Navigate ??n trang Vai Trò
4. Ki?m tra `DataContext` có giá tr? không

### B??c 2: Test ViewModel Load Data
1. ??t breakpoint t?i `LoadRoleData()` trong ViewModel
2. Ki?m tra `allRoles.Count` sau khi load
3. Ki?m tra `PagedRoles.Count` sau khi pagination

### B??c 3: Test Add Command
1. ??t breakpoint t?i `Add()` method
2. Click nút "Thêm vai trò"
3. Step through code ?? xem l?i ? ?âu

### B??c 4: Test AddVaiTroWindow
1. ??t breakpoint t?i `AddVaiTroWindow` constructor
2. Click "Thêm vai trò"
3. Ki?m tra window có hi?n th? không

## CÁC L?I TH??NG G?P

### 1. NullReferenceException t?i DataContext
**Nguyên nhân:** ServiceProvider ch?a ???c kh?i t?o ho?c ViewModel ch?a ??ng ký

**Gi?i pháp:**
```csharp
// Ki?m tra App.xaml.cs
private void ConfigureServices(IServiceCollection services)
{
    // ??m b?o dòng này có
    services.AddTransient<VaiTroViewModel>();
}
```

### 2. AddVaiTroWindow không m?
**Nguyên nhân:** File XAML ho?c code-behind có l?i

**Gi?i pháp:**
- Ki?m tra build errors
- Ki?m tra namespace trong XAML: `x:Class="MilitaryGeo.Desktop.Popup.VaiTro.AddVaiTroWindow"`
- ??m b?o file `.xaml.cs` có cùng namespace

### 3. Danh sách r?ng
**Nguyên nhân:** 
- `LoadRoleData()` không ???c g?i
- Pagination có v?n ??
- Binding sai property name

**Gi?i pháp:**
```csharp
// Trong ViewModel constructor
public VaiTroViewModel(IMessageService messageService)
{
    _messageService = messageService;
    LoadRoleData(); // ? Ph?i g?i
}

// Trong XAML
ItemsSource="{Binding PagedRoles}" // ? Ph?i ?úng tên property
```

## K?T QU? MONG ??I

Sau khi s?a, b?n s? th?y:

1. **Trang Vai Trò hi?n th?:**
   - Header: "Qu?n lý vai trò"
   - T?ng s?: "T?ng s?: 10 vai trò"
   - DataGrid v?i 10 rows
   - Pagination controls

2. **Click "Thêm vai trò":**
   - Window popup hi?n th?
   - Form v?i ??y ?? fields
   - Checkboxes phân quy?n
   - Buttons L?u/H?y

3. **Nh?p d? li?u và L?u:**
   - Validation ho?t ??ng
   - L?u thành công
   - Hi?n th? message
   - Danh sách c?p nh?t

4. **Click icon ?? (Edit):**
   - Window popup v?i data ?ã fill
   - Edit và l?u thành công
   - Danh sách c?p nh?t

5. **Click icon ??? (Delete):**
   - Confirmation dialog
   - Xóa thành công
   - Danh sách c?p nh?t

## LIÊN H? H? TR?

N?u v?n g?p v?n ??:
1. Copy toàn b? Output window logs
2. Ch?p screenshot l?i
3. G?i kèm file code b? l?i

---

**L?u ý:** ?ã build thành công ?  
**Tr?ng thái:** Ready to test ??  
**Tác gi?:** GitHub Copilot
