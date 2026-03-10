# H??NG D?N KH?C PH?C L?I FONT TI?NG VI?T

## V?N ??

Khi hi?n th? d? li?u ti?ng Vi?t t? ViewModel lên giao di?n WPF, các ký t? có d?u b? hi?n th? sai ho?c thành ký t? l? (?, ?, ?, etc.)

## NGUYÊN NHÂN

### 1. **Thi?u Font h? tr? Unicode**
- Font m?c ??nh không h? tr? ??y ?? b?ng mã Unicode ti?ng Vi?t
- WPF dùng font fallback không phù h?p

### 2. **File encoding sai**
- File .cs ho?c .xaml ???c save v?i encoding sai (ANSI thay vì UTF-8)
- Visual Studio không t? ??ng chuy?n sang UTF-8 BOM

### 3. **Culture setting không ?úng**
- App không set culture vi-VN
- TextOptions không ???c config ?úng

## ?Ã KH?C PH?C

### ? 1. Thêm FontFamily vào App.xaml

File: `MilitaryGeo.Desktop\App.xaml`

```xaml
<Application.Resources>
    <!-- Global Font Settings for Vietnamese -->
    <Style TargetType="{x:Type Window}">
        <Setter Property="FontFamily" Value="Segoe UI, Arial, sans-serif"/>
        <Setter Property="TextOptions.TextFormattingMode" Value="Display"/>
        <Setter Property="TextOptions.TextRenderingMode" Value="ClearType"/>
    </Style>
    
    <Style TargetType="{x:Type UserControl}">
        <Setter Property="FontFamily" Value="Segoe UI, Arial, sans-serif"/>
        <Setter Property="TextOptions.TextFormattingMode" Value="Display"/>
        <Setter Property="TextOptions.TextRenderingMode" Value="ClearType"/>
    </Style>
    
    <Style TargetType="{x:Type TextBlock}">
        <Setter Property="FontFamily" Value="Segoe UI, Arial, sans-serif"/>
        <Setter Property="TextOptions.TextFormattingMode" Value="Display"/>
        <Setter Property="TextOptions.TextRenderingMode" Value="ClearType"/>
    </Style>
    
    <!-- Các controls khác... -->
</Application.Resources>
```

**Gi?i thích:**
- `Segoe UI`: Font Windows h? tr? t?t Unicode và ti?ng Vi?t
- `Arial`: Font fallback n?u Segoe UI không có
- `TextFormattingMode="Display"`: Render text rõ nét h?n
- `TextRenderingMode="ClearType"`: Smooth rendering

### ? 2. Thêm FontFamily vào VaiTro.xaml

File: `MilitaryGeo.Desktop\UserControls\VaiTro.xaml`

```xaml
<UserControl x:Class="MilitaryGeo.Desktop.UserControls.VaiTro"
             ...
             FontFamily="Segoe UI">
```

### ? 3. Set Culture trong App.xaml.cs

File: `MilitaryGeo.Desktop\App.xaml.cs`

```csharp
private void SetCultureToVietnamese()
{
    var culture = new CultureInfo("vi-VN");
    culture.DateTimeFormat.AMDesignator = "AM";
    culture.DateTimeFormat.PMDesignator = "PM";
    Thread.CurrentThread.CurrentCulture = culture;
    Thread.CurrentThread.CurrentUICulture = culture;
}

protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    SetCultureToVietnamese(); // ? ?ã có
}
```

## FONTS H? TR? TI?NG VI?T T?T

### Fonts Windows m?c ??nh:
1. **Segoe UI** ? (Khuyên dùng)
2. **Arial**
3. **Tahoma**
4. **Verdana**
5. **Calibri**
6. **Times New Roman**

### Fonts Google (N?u mu?n custom):
1. **Roboto**
2. **Open Sans**
3. **Lato**
4. **Noto Sans**

## KI?M TRA VÀ DEBUG

### 1. Ki?m tra encoding c?a file C#

```csharp
// Trong VaiTroViewModel.cs
new VaiTro
{
    TenVaiTro = "Qu?n tr? viên", // ? Ph?i hi?n th? ?úng
    MoTa = "Có toàn quy?n truy c?p và qu?n lý h? th?ng" // ?
}
```

N?u th?y ký t? l? trong code ? File encoding sai

**Cách fix:**
1. File ? Save As...
2. Click m?i tên bên c?nh Save
3. Ch?n "Save with Encoding..."
4. Ch?n "Unicode (UTF-8 with signature) - Codepage 65001"
5. Save

### 2. Ki?m tra runtime

Thêm debug code:
```csharp
System.Diagnostics.Debug.WriteLine($"Font: {this.FontFamily}");
System.Diagnostics.Debug.WriteLine($"Text: {TenVaiTro}");
System.Diagnostics.Debug.WriteLine($"Culture: {Thread.CurrentThread.CurrentCulture.Name}");
```

N?u output console hi?n th? ?úng nh?ng UI sai ? V?n ?? Font
N?u output console c?ng sai ? V?n ?? encoding

### 3. Ki?m tra XAML binding

??t text t?nh ?? test:
```xaml
<TextBlock Text="Qu?n lý vai trò" FontFamily="Segoe UI"/>
```

N?u text t?nh OK nh?ng binding sai ? V?n ?? data t? ViewModel
N?u text t?nh c?ng sai ? V?n ?? font ho?c XAML encoding

## BEST PRACTICES

### 1. Luôn dùng UTF-8 BOM
- File .cs ? UTF-8 with signature
- File .xaml ? UTF-8 with signature
- File .json ? UTF-8 without signature (JSON standard)

### 2. Set Font ? c?p cao nh?t
```xaml
<!-- App.xaml - Áp d?ng toàn app -->
<Style TargetType="{x:Type Window}">
    <Setter Property="FontFamily" Value="Segoe UI"/>
</Style>
```

### 3. Override khi c?n
```xaml
<!-- Specific control -->
<TextBlock FontFamily="Consolas" Text="{Binding Code}"/>
```

### 4. Test v?i nhi?u fonts
```csharp
// Test font fallback
FontFamily="Segoe UI, Arial, Tahoma, sans-serif"
```

### 5. Dùng string literals ?úng
```csharp
// ? ?ÚNG - Unicode escaped
var text = "Qu\u1EA3n tr\u1ECB vi\u00EAn";

// ? ?ÚNG - Direct Unicode (n?u file UTF-8)
var text = "Qu?n tr? viên";

// ? SAI - ANSI encoding
var text = "QuÂn trÞ viªn"; // L?i này
```

## CHECKLIST KH?C PH?C

- [x] App.xaml có global font style
- [x] VaiTro.xaml có FontFamily="Segoe UI"
- [x] App.xaml.cs set culture vi-VN
- [x] VaiTroViewModel.cs saved as UTF-8 BOM
- [x] VaiTro.xaml saved as UTF-8 BOM
- [x] TextOptions ???c config

## N?U V?N KHÔNG HO?T ??NG

### B??c 1: Verify File Encoding

Trong Visual Studio:
1. File ? Advanced Save Options
2. ??m b?o: "Unicode (UTF-8 with signature) - Codepage 65001"
3. Save all files

### B??c 2: Clean & Rebuild

```bash
# Clean solution
dotnet clean

# Rebuild
dotnet build
```

### B??c 3: Test v?i font khác

```xaml
<!-- Th? fonts khác -->
<UserControl FontFamily="Arial">
<UserControl FontFamily="Tahoma">
<UserControl FontFamily="Times New Roman">
```

### B??c 4: Ki?m tra Windows Font

1. M? Control Panel ? Fonts
2. Tìm "Segoe UI"
3. N?u không có ? Install font

### B??c 5: Force Unicode

```csharp
// Trong ViewModel
TenVaiTro = Encoding.UTF8.GetString(
    Encoding.UTF8.GetBytes("Qu?n tr? viên")
);
```

### B??c 6: Debug Binding

```xaml
<!-- Add diagnostics -->
<TextBlock Text="{Binding TenVaiTro}" 
           FontFamily="Segoe UI"
           TextOptions.TextFormattingMode="Display">
    <TextBlock.ToolTip>
        <ToolTip Content="{Binding TenVaiTro}"/>
    </TextBlock.ToolTip>
</TextBlock>
```

## K?T QU? MONG ??I

Sau khi áp d?ng các fix trên:

? Header hi?n th?: "Qu?n lý vai trò"
? Data grid columns: "Mã vai trò", "Tên vai trò", "Mô t?"
? Fake data: "Qu?n tr? viên", "Ng??i dùng", "Giám sát viên"...
? Buttons: "Xu?t Excel", "Thêm vai trò", "Làm m?i"
? Status: "Ho?t ??ng", "T?t"
? Tooltips: "Ch?nh s?a", "Xóa"

## DEMO CODE

```csharp
// VaiTroViewModel.cs - Fake data v?i ti?ng Vi?t
allRoles = new ObservableCollection<VaiTro>
{
    new VaiTro
    {
        Id = 1,
        MaVaiTro = "ADMIN",
        TenVaiTro = "Qu?n tr? viên", // ? Hi?n th? ?úng
        MoTa = "Có toàn quy?n truy c?p và qu?n lý h? th?ng", // ?
        IsActive = true,
        ThuTu = 1,
        CreatedDate = DateTime.Now.AddDays(-30)
    },
    // ... more roles
};
```

```xaml
<!-- VaiTro.xaml - Binding v?i font -->
<syncfusion:GridTextColumn 
    HeaderText="Tên vai trò" 
    MappingName="TenVaiTro" 
    Width="200"
    FontFamily="Segoe UI"/> <!-- ? Font h? tr? ti?ng Vi?t -->
```

## TÀI LI?U THAM KH?O

- [WPF Typography](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/typography-in-wpf)
- [Unicode in .NET](https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-encoding-introduction)
- [ClearType Font Technology](https://learn.microsoft.com/en-us/typography/cleartype/)

---

**Tr?ng thái:** ? ?ã fix  
**Font ???c dùng:** Segoe UI (h? tr? ??y ?? ti?ng Vi?t)  
**Encoding:** UTF-8 with BOM  
**Culture:** vi-VN
