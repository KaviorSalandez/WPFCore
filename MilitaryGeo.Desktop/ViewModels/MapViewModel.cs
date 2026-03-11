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

    // ===================== MAP PROPERTIES =====================

    [ObservableProperty] private double _latitude = 21.0285;
    [ObservableProperty] private double _longitude = 105.8542;
    [ObservableProperty] private int _zoomLevel = 10;
    [ObservableProperty] private string _mapStyle = "ArcGIS:Topographic";
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _searchLocation = string.Empty;
    [ObservableProperty] private int _totalMarkers;
    [ObservableProperty] private string _currentCoordinates = string.Empty;
    [ObservableProperty] private string _measurementResult = string.Empty;
    [ObservableProperty] private bool _isMeasuringDistance;
    [ObservableProperty] private bool _isMeasuringArea;

    // ===================== COLLECTIONS =====================

    [ObservableProperty]
    private ObservableCollection<MapMarker> _markers = new();

    public ObservableCollection<string> MapStyles { get; } = new()
    {
        "ArcGIS:Topographic",
        "ArcGIS:Streets",
        "ArcGIS:Navigation",
        "ArcGIS:Imagery",
        "ArcGIS:Oceans",
        "OpenStreetMap"
    };

    // ===================== ACTION CALLBACKS ? Code-behind thuc thi map operations =====================

    public Func<string, Task<(double Lat, double Lon, string DisplayName)?>>? GeocodeAsync  { get; set; }
    public Action<MapMarker>?                                                  AddMarkerToMap       { get; set; }
    public Action<MapMarker>?                                                  RemoveMarkerFromMap  { get; set; }
    public Action?                                                             ClearMarkersFromMap  { get; set; }
    public Func<double, double, int, Task>?                                    NavigateToAsync      { get; set; }
    public Action<bool>?                                                       SetMeasureDistanceMode { get; set; }
    public Action<bool>?                                                       SetMeasureAreaMode     { get; set; }
    public Func<Task>?                                                         ExportMapAsync       { get; set; }

    // ===================== CONSTRUCTOR =====================

    public MapViewModel(IOptions<ArcGISConfiguration> arcGISConfig)
    {
        _arcGISConfig = arcGISConfig?.Value ?? throw new ArgumentNullException(nameof(arcGISConfig));
        InitializeMap();
    }

    private void InitializeMap()
    {
        Markers.Add(new MapMarker { Id = 1, Name = "Tr? s? B? T? l?nh", Latitude = 21.0285, Longitude = 105.8542, Type = "Headquarters", Description = "V? trí tr? s? chính" });
        Markers.Add(new MapMarker { Id = 2, Name = "?i?m quan sát A1",   Latitude = 21.0385, Longitude = 105.8642, Type = "Observation",  Description = "?i?m quan sát khu v?c phía B?c" });
        Markers.Add(new MapMarker { Id = 3, Name = "Kho v? khí B2",      Latitude = 21.0185, Longitude = 105.8442, Type = "Arsenal",      Description = "Kho v? khí d? tr? chi?n l??c" });
        TotalMarkers = Markers.Count;
        UpdateCurrentCoordinates();
    }

    // ===================== PARTIAL PROPERTY CALLBACKS =====================

    partial void OnLatitudeChanged(double value)  => UpdateCurrentCoordinates();
    partial void OnLongitudeChanged(double value) => UpdateCurrentCoordinates();

    partial void OnIsMeasuringDistanceChanged(bool value)
    {
        if (value)
            OnPropertyChanging(nameof(IsMeasuringArea));
        _isMeasuringArea = value ? false : _isMeasuringArea;
        if (value)
            OnPropertyChanged(nameof(IsMeasuringArea));
        SetMeasureDistanceMode?.Invoke(value);
    }

    partial void OnIsMeasuringAreaChanged(bool value)
    {
        if (value)
            OnPropertyChanging(nameof(IsMeasuringDistance));
        _isMeasuringDistance = value ? false : _isMeasuringDistance;
        if (value)
            OnPropertyChanged(nameof(IsMeasuringDistance));
        SetMeasureAreaMode?.Invoke(value);
    }

    private void UpdateCurrentCoordinates()
        => CurrentCoordinates = $"Lat: {Latitude:F6}°  |  Lon: {Longitude:F6}°";

    // ===================== COMMANDS =====================

    [RelayCommand]
    private async Task ZoomInAsync()
    {
        if (ZoomLevel >= 20) return;
        ZoomLevel++;
        if (NavigateToAsync != null) await NavigateToAsync(Latitude, Longitude, ZoomLevel);
    }

    [RelayCommand]
    private async Task ZoomOutAsync()
    {
        if (ZoomLevel <= 1) return;
        ZoomLevel--;
        if (NavigateToAsync != null) await NavigateToAsync(Latitude, Longitude, ZoomLevel);
    }

    [RelayCommand]
    private async Task ResetViewAsync()
    {
        Latitude            = 21.0285;
        Longitude           = 105.8542;
        ZoomLevel           = 10;
        MapStyle            = "ArcGIS:Topographic";
        MeasurementResult   = string.Empty;
        IsMeasuringDistance = false;
        IsMeasuringArea     = false;
        if (NavigateToAsync != null) await NavigateToAsync(Latitude, Longitude, ZoomLevel);
    }

    [RelayCommand]
    private async Task SearchLocationAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchLocation)) return;
        IsLoading = true;
        try
        {
            if (GeocodeAsync != null)
            {
                var result = await GeocodeAsync(SearchLocation);
                if (result.HasValue)
                {
                    Latitude  = result.Value.Lat;
                    Longitude = result.Value.Lon;
                    ZoomLevel = 14;
                    if (NavigateToAsync != null)
                        await NavigateToAsync(Latitude, Longitude, ZoomLevel);
                }
                else
                    MessageBox.Show($"Không tìm th?y: \"{SearchLocation}\"", "Tìm ki?m", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"L?i tìm ki?m: {ex.Message}", "L?i", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private void AddMarker()
    {
        var marker = new MapMarker
        {
            Id          = Markers.Count > 0 ? Markers.Max(m => m.Id) + 1 : 1,
            Name        = $"?i?m {Markers.Count + 1}",
            Latitude    = Latitude  + (Random.Shared.NextDouble() - 0.5) * 0.02,
            Longitude   = Longitude + (Random.Shared.NextDouble() - 0.5) * 0.02,
            Type        = "Custom",
            Description = "?i?m ?ánh d?u tùy ch?nh"
        };
        Markers.Add(marker);
        TotalMarkers = Markers.Count;
        AddMarkerToMap?.Invoke(marker);
    }

    // Goi tu code-behind khi click len map
    public void AddMarkerAtCoordinate(double lat, double lon)
    {
        var marker = new MapMarker
        {
            Id          = Markers.Count > 0 ? Markers.Max(m => m.Id) + 1 : 1,
            Name        = $"?i?m {Markers.Count + 1}",
            Latitude    = lat,
            Longitude   = lon,
            Type        = "Custom",
            Description = $"T?a ??: {lat:F6}, {lon:F6}"
        };
        Markers.Add(marker);
        TotalMarkers = Markers.Count;
        AddMarkerToMap?.Invoke(marker);
    }

    [RelayCommand]
    private void RemoveMarker(MapMarker? marker)
    {
        if (marker == null) return;
        if (MessageBox.Show($"Xóa marker \"{marker.Name}\"?", "Xác nh?n",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            RemoveMarkerFromMap?.Invoke(marker);
            Markers.Remove(marker);
            TotalMarkers = Markers.Count;
        }
    }

    [RelayCommand]
    private void ClearAllMarkers()
    {
        if (Markers.Count == 0) return;
        if (MessageBox.Show("Xóa t?t c? markers?", "Xác nh?n",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            ClearMarkersFromMap?.Invoke();
            Markers.Clear();
            TotalMarkers = 0;
        }
    }

    [RelayCommand]
    private void ToggleMeasureDistance()
    {
        IsMeasuringDistance = !IsMeasuringDistance;
        MeasurementResult   = IsMeasuringDistance
            ? "Nh?p các ?i?m trên b?n ?? ?? ?o kho?ng cách..."
            : string.Empty;
    }

    [RelayCommand]
    private void ToggleMeasureArea()
    {
        IsMeasuringArea   = !IsMeasuringArea;
        MeasurementResult = IsMeasuringArea
            ? "Nh?p ít nh?t 3 ?i?m ?? ?o di?n tích..."
            : string.Empty;
    }

    [RelayCommand]
    private async Task GetMyLocationAsync()
    {
        Latitude  = 21.0285;
        Longitude = 105.8542;
        ZoomLevel = 14;
        if (NavigateToAsync != null) await NavigateToAsync(Latitude, Longitude, ZoomLevel);
    }

    [RelayCommand]
    private async Task ExportMapToImageAsync()
    {
        IsLoading = true;
        try   { if (ExportMapAsync != null) await ExportMapAsync(); }
        finally { IsLoading = false; }
    }

    // Cap nhat tu code-behind
    public void UpdateLiveCoordinates(double lat, double lon)
        => CurrentCoordinates = $"Lat: {lat:F6}°  |  Lon: {lon:F6}°";

    public void SetMeasurementResult(string result)
        => MeasurementResult = result;
}

public class MapMarker
{
    public int      Id          { get; set; }
    public string   Name        { get; set; } = string.Empty;
    public double   Latitude    { get; set; }
    public double   Longitude   { get; set; }
    public string   Type        { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
