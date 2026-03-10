# H??NG D?N S? D?NG DYNAMIC MENU

## T?ng quan
H? th?ng menu ?ã ???c chuy?n t? static sang dynamic (d?a trên d? li?u), cho phép d? dàng load menu t? API.

## C?u trúc hi?n t?i

### 1. Model (MenuItem.cs)
```csharp
public class MenuItem
{
    public string Icon { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ViewName { get; set; } = string.Empty;
    public bool HasSubMenu => SubMenuItems != null && SubMenuItems.Count > 0;
    public ObservableCollection<MenuItem>? SubMenuItems { get; set; }
    public bool IsExpanded { get; set; }
}
```

### 2. Fake Data (MainWorkSpace.xaml.cs)
Method `LoadMenuData()` hi?n ?ang load fake data:
```csharp
private void LoadMenuData()
{
    // Fake data - Sau này s? load t? API
    MenuItems = new ObservableCollection<MenuItemModel>
    {
        new MenuItemModel
        {
            Icon = "??",
            Title = "Dashboard",
            ViewName = "Dashboard"
        },
        // ... các menu items khác ...
    };
}
```

## CÁCH TÍCH H?P API

### B??c 1: T?o Service ?? call API
T?o file `MenuService.cs` trong folder `Services`:

```csharp
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.Json;
using MilitaryGeo.Desktop.ViewModels;

namespace MilitaryGeo.Desktop.Services
{
    public interface IMenuService
    {
        Task<ObservableCollection<MenuItem>> GetMenuItemsAsync();
    }

    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;
        private const string API_URL = "https://your-api.com/api/menu";

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<MenuItem>> GetMenuItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(API_URL);
                response.EnsureSuccessStatusCode();
                
                var json = await response.ReadAsStringAsync();
                var menuItems = JsonSerializer.Deserialize<List<MenuItem>>(json);
                
                return new ObservableCollection<MenuItem>(menuItems);
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error loading menu: {ex.Message}");
                
                // Return empty collection ho?c fake data
                return GetFallbackMenu();
            }
        }

        private ObservableCollection<MenuItem> GetFallbackMenu()
        {
            // Tr? v? menu m?c ??nh n?u API fail
            return new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Icon = "??",
                    Title = "Dashboard",
                    ViewName = "Dashboard"
                }
            };
        }
    }
}
```

### B??c 2: Update MainWorkSpace.xaml.cs

Thay ??i method `LoadMenuData()`:

```csharp
using MilitaryGeo.Desktop.Services;

// Trong class MainWorkSpace:
private readonly IMenuService _menuService;

// Constructor v?i dependency injection:
public MainWorkSpace(IMenuService menuService)
{
    _menuService = menuService;
    InitializeComponent();
    LoadMenuDataAsync(); // Changed to async
    DataContext = this;
}

// Ho?c n?u ch?a dùng DI:
public MainWorkSpace()
{
    InitializeComponent();
    LoadMenuDataAsync();
    DataContext = this;
}

private async Task LoadMenuDataAsync()
{
    try
    {
        // Show loading indicator (optional)
        // LoadingIndicator.Visibility = Visibility.Visible;
        
        // Call API
        var httpClient = new HttpClient();
        var menuService = new MenuService(httpClient);
        MenuItems = await menuService.GetMenuItemsAsync();
        
        // Load default view
        LoadView("Dashboard");
    }
    catch (Exception ex)
    {
        MessageBox.Show($"L?i t?i menu: {ex.Message}", "L?i", 
            MessageBoxButton.OK, MessageBoxImage.Error);
        
        // Fallback to fake data
        LoadMenuData(); // Use existing fake data method
    }
    finally
    {
        // Hide loading indicator
        // LoadingIndicator.Visibility = Visibility.Collapsed;
    }
}
```

### B??c 3: Format JSON t? API

API c?n tr? v? JSON theo format:

```json
[
    {
        "icon": "??",
        "title": "Dashboard",
        "viewName": "Dashboard",
        "subMenuItems": null
    },
    {
        "icon": "??",
        "title": "Cài ??t h? th?ng",
        "viewName": "Settings",
        "subMenuItems": [
            {
                "icon": "?",
                "title": "Cài ??t chung",
                "viewName": "GeneralSettings"
            },
            {
                "icon": "?",
                "title": "Cài ??t b?o m?t",
                "viewName": "SecuritySettings"
            }
        ]
    }
]
```

### B??c 4: Thêm Authorization (n?u c?n)

```csharp
public class MenuService : IMenuService
{
    private readonly HttpClient _httpClient;
    private readonly string _bearerToken;

    public MenuService(HttpClient httpClient, string bearerToken)
    {
        _httpClient = httpClient;
        _bearerToken = bearerToken;
    }

    public async Task<ObservableCollection<MenuItem>> GetMenuItemsAsync()
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
            
        // ... rest of code ...
    }
}
```

## L?U Ý QUAN TR?NG

1. **Error Handling**: Luôn có fallback data khi API fail
2. **Loading State**: Hi?n th? loading indicator khi call API
3. **Caching**: Có th? cache menu ?? gi?m API calls
4. **Security**: Validate d? li?u t? API tr??c khi hi?n th?
5. **ViewName Mapping**: ??m b?o ViewName t? API match v?i switch-case trong `LoadView()`

## Testing

Test v?i fake data tr??c:
1. Ch?y ?ng d?ng - menu s? hi?n th? fake data
2. Verify t?t c? menu items ho?t ??ng
3. Verify submenu expand/collapse
4. Verify navigation gi?a các views

Sau khi API ready:
1. Thay ??i `LoadMenuData()` thành `LoadMenuDataAsync()`
2. Test v?i API endpoint
3. Test error scenarios (API down, timeout, etc.)
4. Test v?i các role khác nhau n?u có phân quy?n

## Demo Example: Call API khi login

```csharp
// Trong MainWindow.xaml.cs (login screen):
private async void btnLogin_Click(object sender, RoutedEventArgs e)
{
    // ... authentication logic ...
    
    if (loginSuccess)
    {
        var workspace = new MainWorkSpace(username);
        // Menu s? t? ??ng load t? API trong constructor
        workspace.Show();
        this.Close();
    }
}
```

Chúc b?n thành công! ??
