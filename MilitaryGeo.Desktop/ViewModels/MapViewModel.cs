using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using MilitaryGeo.Application.Configuration;
using System.Collections.ObjectModel;
using System.Windows;

namespace MilitaryGeo.Desktop.ViewModels;

public partial class MapViewModel : ObservableObject
{
    private readonly ArcGISConfiguration _arcGISConfig;

    public string ArcGISApiKey => _arcGISConfig.ApiKey;

    // Map Properties
    [ObservableProperty]
    private double _latitude = 21.0285; // Hanoi coordinates

    [ObservableProperty]
    private double _longitude = 105.8542;

    [ObservableProperty]
    private int _zoomLevel = 10;

    [ObservableProperty]
    private string _mapStyle = "ArcGIS:Topographic";

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _searchLocation = string.Empty;

    // Map Markers Collection
    [ObservableProperty]
    private ObservableCollection<MapMarker> _markers = new();

    // Map Drawing Tools
    [ObservableProperty]
    private bool _isDrawingMode;

    [ObservableProperty]
    private string _selectedTool = "None";

    // Statistics
    [ObservableProperty]
    private int _totalMarkers;

    [ObservableProperty]
    private string _currentCoordinates = string.Empty;

    // Available Map Styles
    public ObservableCollection<string> MapStyles { get; } = new()
    {
        "ArcGIS:Topographic",
        "ArcGIS:Streets",
        "ArcGIS:Navigation",
        "ArcGIS:Imagery",
        "ArcGIS:Oceans",
        "OpenStreetMap"
    };

    // Drawing Tools
    public ObservableCollection<string> DrawingTools { get; } = new()
    {
        "None",
        "Marker",
        "Line",
        "Polygon",
        "Circle",
        "Rectangle"
    };

    public MapViewModel(IOptions<ArcGISConfiguration> arcGISConfig)
    {
        _arcGISConfig = arcGISConfig?.Value ?? throw new ArgumentNullException(nameof(arcGISConfig));
        InitializeMap();
    }

    private void InitializeMap()
    {
        // Add some demo markers
        Markers.Add(new MapMarker
        {
            Id = 1,
            Name = "Tr? s? B? T? l?nh",
            Latitude = 21.0285,
            Longitude = 105.8542,
            Type = "Headquarters",
            Description = "V? trí tr? s? chính"
        });

        Markers.Add(new MapMarker
        {
            Id = 2,
            Name = "?i?m quan sát A1",
            Latitude = 21.0385,
            Longitude = 105.8642,
            Type = "Observation",
            Description = "?i?m quan sát khu v?c phía B?c"
        });

        Markers.Add(new MapMarker
        {
            Id = 3,
            Name = "Kho v? khí B2",
            Latitude = 21.0185,
            Longitude = 105.8442,
            Type = "Arsenal",
            Description = "Kho v? khí d? tr? chi?n l??c"
        });

        TotalMarkers = Markers.Count;
        UpdateCurrentCoordinates();
    }

    partial void OnLatitudeChanged(double value)
    {
        UpdateCurrentCoordinates();
    }

    partial void OnLongitudeChanged(double value)
    {
        UpdateCurrentCoordinates();
    }

    private void UpdateCurrentCoordinates()
    {
        CurrentCoordinates = $"Lat: {Latitude:F6}, Lon: {Longitude:F6}";
    }

    [RelayCommand]
    private void ZoomIn()
    {
        if (ZoomLevel < 20)
            ZoomLevel++;
    }

    [RelayCommand]
    private void ZoomOut()
    {
        if (ZoomLevel > 1)
            ZoomLevel--;
    }

    [RelayCommand]
    private void ResetView()
    {
        Latitude = 21.0285;
        Longitude = 105.8542;
        ZoomLevel = 10;
        MapStyle = "ArcGIS:Topographic";
    }

    [RelayCommand]
    private async Task SearchLocationAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchLocation))
            return;

        IsLoading = true;
        try
        {
            // TODO: Implement geocoding service
            await Task.Delay(1000); // Simulate API call

            MessageBox.Show(
                $"?ang tìm ki?m v? trí: {SearchLocation}\n\nCh?c n?ng geocoding s? ???c tích h?p sau.",
                "Tìm ki?m v? trí",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"L?i khi tìm ki?m: {ex.Message}",
                "L?i",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void AddMarker()
    {
        var newMarker = new MapMarker
        {
            Id = Markers.Count + 1,
            Name = $"Marker {Markers.Count + 1}",
            Latitude = Latitude + (Random.Shared.NextDouble() - 0.5) * 0.1,
            Longitude = Longitude + (Random.Shared.NextDouble() - 0.5) * 0.1,
            Type = "Custom",
            Description = "?i?m ?ánh d?u tùy ch?nh"
        };

        Markers.Add(newMarker);
        TotalMarkers = Markers.Count;

        MessageBox.Show(
            $"?ã thêm marker: {newMarker.Name}\nT?a ??: {newMarker.Latitude:F6}, {newMarker.Longitude:F6}",
            "Thêm Marker",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    [RelayCommand]
    private void RemoveMarker(MapMarker marker)
    {
        if (marker == null) return;

        var result = MessageBox.Show(
            $"B?n có ch?c ch?n mu?n xóa marker '{marker.Name}'?",
            "Xác nh?n xóa",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            Markers.Remove(marker);
            TotalMarkers = Markers.Count;
        }
    }

    [RelayCommand]
    private void ClearAllMarkers()
    {
        var result = MessageBox.Show(
            "B?n có ch?c ch?n mu?n xóa t?t c? markers?",
            "Xác nh?n xóa t?t c?",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            Markers.Clear();
            TotalMarkers = 0;
            MessageBox.Show("?ã xóa t?t c? markers!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    [RelayCommand]
    private void MeasureDistance()
    {
        MessageBox.Show(
            "Ch?c n?ng ?o kho?ng cách s? ???c tích h?p.\n\n" +
            "S? d?ng công c? này ?? ?o kho?ng cách gi?a các ?i?m trên b?n ??.",
            "?o kho?ng cách",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    [RelayCommand]
    private void MeasureArea()
    {
        MessageBox.Show(
            "Ch?c n?ng ?o di?n tích s? ???c tích h?p.\n\n" +
            "S? d?ng công c? này ?? ?o di?n tích khu v?c trên b?n ??.",
            "?o di?n tích",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    [RelayCommand]
    private void ExportMap()
    {
        MessageBox.Show(
            "Ch?c n?ng xu?t b?n ?? s? ???c tích h?p.\n\n" +
            "H? tr? xu?t ??nh d?ng: PNG, JPEG, PDF, KML",
            "Xu?t b?n ??",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    [RelayCommand]
    private void ToggleDrawingMode()
    {
        IsDrawingMode = !IsDrawingMode;
        if (!IsDrawingMode)
            SelectedTool = "None";
    }

    [RelayCommand]
    private void GetMyLocation()
    {
        // Default to Hanoi
        Latitude = 21.0285;
        Longitude = 105.8542;
        ZoomLevel = 12;

        MessageBox.Show(
            "?ã di chuy?n v? v? trí m?c ??nh: Hà N?i\n\n" +
            "Tích h?p GPS ?? l?y v? trí th?c t? s? ???c thêm sau.",
            "V? trí c?a tôi",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}

public class MapMarker
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
