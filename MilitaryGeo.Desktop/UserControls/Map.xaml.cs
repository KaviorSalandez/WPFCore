using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.UI;
using MilitaryGeo.Desktop.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp = System.Windows.Application;

namespace MilitaryGeo.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        private MapViewModel _viewModel = null!;

        // GraphicsOverlay rieng cho tung loai
        private readonly GraphicsOverlay _markerOverlay  = new() { Id = "markers" };
        private readonly GraphicsOverlay _measureOverlay = new() { Id = "measure" };

        // Trang thai do luong
        private readonly List<MapPoint> _measurePoints   = new();
        private bool _measureDistanceActive;
        private bool _measureAreaActive;

        public Map()
        {
            InitializeComponent();

            _viewModel = App.GetService<MapViewModel>();
            DataContext = _viewModel;

            Loaded += Map_Loaded;
        }

        // ===================== INIT =====================

        private async void Map_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ArcGISRuntimeEnvironment.ApiKey = _viewModel.ArcGISApiKey;

                // Khoi tao map
                MainMapView.Map = new Esri.ArcGISRuntime.Mapping.Map(CreateBasemap(_viewModel.MapStyle));
                MainMapView.GraphicsOverlays.Add(_markerOverlay);
                MainMapView.GraphicsOverlays.Add(_measureOverlay);

                // Di chuyen den Ha Noi
                await MainMapView.SetViewpointAsync(new Viewpoint(
                    _viewModel.Latitude, _viewModel.Longitude,
                    GetScaleFromZoom(_viewModel.ZoomLevel)));

                // Ve cac marker demo len map
                foreach (var marker in _viewModel.Markers)
                    AddMarkerGraphic(marker);

                // Gan callbacks cho ViewModel
                WireViewModelCallbacks();

                // Lang nghe su kien map
                MainMapView.GeoViewTapped    += MapView_Tapped;
                MainMapView.MouseMove        += MapView_MouseMove;
                MainMapView.ViewpointChanged += MapView_ViewpointChanged;
                _viewModel.PropertyChanged   += ViewModel_PropertyChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo bản đồ:\n{ex.Message}", "ArcGIS Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WireViewModelCallbacks()
        {
            _viewModel.GeocodeAsync = GeocodeLocationAsync;

            _viewModel.AddMarkerToMap = marker =>
                WpfApp.Current.Dispatcher.Invoke(() => AddMarkerGraphic(marker));

            _viewModel.RemoveMarkerFromMap = marker =>
                WpfApp.Current.Dispatcher.Invoke(() => RemoveMarkerGraphic(marker));

            _viewModel.ClearMarkersFromMap = () =>
                WpfApp.Current.Dispatcher.Invoke(() => _markerOverlay.Graphics.Clear());

            _viewModel.NavigateToAsync = async (lat, lon, zoom) =>
                await MainMapView.SetViewpointAsync(new Viewpoint(lat, lon, GetScaleFromZoom(zoom)));

            _viewModel.SetMeasureDistanceMode = active =>
            {
                _measureDistanceActive = active;
                _measureAreaActive     = false;
                if (!active) ClearMeasureOverlay();
                else _measurePoints.Clear();
            };

            _viewModel.SetMeasureAreaMode = active =>
            {
                _measureAreaActive     = active;
                _measureDistanceActive = false;
                if (!active) ClearMeasureOverlay();
                else _measurePoints.Clear();
            };

            _viewModel.ExportMapAsync = ExportMapToFileAsync;
        }

        // ===================== MAP EVENT HANDLERS =====================

        private async void MapView_Tapped(object? sender, Esri.ArcGISRuntime.UI.Controls.GeoViewInputEventArgs e)
        {
            if (e.Location is not MapPoint tappedPoint) return;
            var wgs84 = (MapPoint)GeometryEngine.Project(tappedPoint, SpatialReferences.Wgs84)!;

            if (_measureDistanceActive) { HandleDistanceMeasure(wgs84); return; }
            if (_measureAreaActive)     { HandleAreaMeasure(wgs84);     return; }

            // Kiem tra click vao marker
            var hit = await MainMapView.IdentifyGraphicsOverlayAsync(
                _markerOverlay, e.Position, tolerance: 10, returnPopupsOnly: false);

            if (hit.Graphics.Count > 0)
            {
                var g = hit.Graphics[0];
                var name = g.Attributes.TryGetValue("Name",        out var n) ? n?.ToString() : "";
                var desc = g.Attributes.TryGetValue("Description", out var d) ? d?.ToString() : "";
                var lat  = g.Attributes.TryGetValue("Latitude",    out var la) ? $"{la:F6}" : "";
                var lon  = g.Attributes.TryGetValue("Longitude",   out var lo) ? $"{lo:F6}" : "";
                MessageBox.Show($"📍 {name}\n\n{desc}\n\nTọa độ: {lat}, {lon}",
                    "Thông tin điểm", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MapView_MouseMove(object sender, MouseEventArgs e)
        {
            var screen   = e.GetPosition(MainMapView);
            var mapPoint = MainMapView.ScreenToLocation(new System.Windows.Point(screen.X, screen.Y));
            if (mapPoint == null) return;
            var wgs84 = (MapPoint)GeometryEngine.Project(mapPoint, SpatialReferences.Wgs84)!;
            _viewModel.UpdateLiveCoordinates(wgs84.Y, wgs84.X);
        }

        private void MapView_ViewpointChanged(object? sender, EventArgs e)
        {
            var vp = MainMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale);
            if (vp == null) return;
            var zoom = (int)Math.Round(Math.Log(591_657_550.5 / vp.TargetScale, 2) + 1);
            zoom = Math.Clamp(zoom, 1, 20);
            if (zoom != _viewModel.ZoomLevel)
                _viewModel.ZoomLevel = zoom;
        }

        private async void ViewModel_PropertyChanged(object? sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MapViewModel.MapStyle) && MainMapView.Map != null)
                MainMapView.Map.Basemap = CreateBasemap(_viewModel.MapStyle);
        }

        // ===================== MARKER GRAPHICS =====================

        private void AddMarkerGraphic(MapMarker marker)
        {
            var point  = new MapPoint(marker.Longitude, marker.Latitude, SpatialReferences.Wgs84);
            var symbol = new CompositeSymbol();
            symbol.Symbols.Add(new SimpleMarkerSymbol(
                SimpleMarkerSymbolStyle.Circle, GetMarkerColor(marker.Type), 14));
            symbol.Symbols.Add(new TextSymbol
            {
                Text  = marker.Name,
                Color = System.Drawing.Color.FromArgb(33, 37, 41),
                Size  = 11, OffsetY = 14,
                HorizontalAlignment = Esri.ArcGISRuntime.Symbology.HorizontalAlignment.Center,
                VerticalAlignment   = Esri.ArcGISRuntime.Symbology.VerticalAlignment.Bottom
            });

            var graphic = new Graphic(point, symbol);
            graphic.Attributes["Id"]          = marker.Id;
            graphic.Attributes["Name"]        = marker.Name;
            graphic.Attributes["Type"]        = marker.Type;
            graphic.Attributes["Description"] = marker.Description;
            graphic.Attributes["Latitude"]    = marker.Latitude;
            graphic.Attributes["Longitude"]   = marker.Longitude;
            _markerOverlay.Graphics.Add(graphic);
        }

        private void RemoveMarkerGraphic(MapMarker marker)
        {
            var graphic = _markerOverlay.Graphics
                .FirstOrDefault(g => g.Attributes.TryGetValue("Id", out var id) && (int)id! == marker.Id);
            if (graphic != null) _markerOverlay.Graphics.Remove(graphic);
        }

        private static System.Drawing.Color GetMarkerColor(string type) => type switch
        {
            "Headquarters" => System.Drawing.Color.FromArgb(46, 90, 39),
            "Observation"  => System.Drawing.Color.FromArgb(33, 150, 243),
            "Arsenal"      => System.Drawing.Color.FromArgb(244, 67, 54),
            _              => System.Drawing.Color.FromArgb(255, 152, 0)
        };

        // ===================== MEASURE DISTANCE =====================

        private void HandleDistanceMeasure(MapPoint point)
        {
            _measurePoints.Add(point);
            _measureOverlay.Graphics.Add(new Graphic(point,
                new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle,
                    System.Drawing.Color.FromArgb(33, 150, 243), 10)));

            if (_measurePoints.Count >= 2)
            {
                // Xoa polyline cu, ve lai toan tuyen
                _measureOverlay.Graphics
                    .Where(g => g.Geometry is Polyline).ToList()
                    .ForEach(g => _measureOverlay.Graphics.Remove(g));

                var line = new Polyline(_measurePoints, SpatialReferences.Wgs84);
                _measureOverlay.Graphics.Add(new Graphic(line,
                    new SimpleLineSymbol(SimpleLineSymbolStyle.Dash,
                        System.Drawing.Color.FromArgb(33, 150, 243), 2)));

                var totalM = 0.0;
                for (int i = 0; i < _measurePoints.Count - 1; i++)
                    totalM += GeometryEngine.DistanceGeodetic(
                        _measurePoints[i], _measurePoints[i + 1],
                        LinearUnits.Meters, AngularUnits.Degrees,
                        GeodeticCurveType.Geodesic).Distance;

                var display = totalM >= 1000 ? $"{totalM / 1000:F2} km" : $"{totalM:F1} m";
                _viewModel.SetMeasurementResult(
                    $"📏 Khoảng cách: {display}  ({_measurePoints.Count} điểm)");
            }
        }

        // ===================== MEASURE AREA =====================

        private void HandleAreaMeasure(MapPoint point)
        {
            _measurePoints.Add(point);
            _measureOverlay.Graphics.Add(new Graphic(point,
                new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle,
                    System.Drawing.Color.FromArgb(156, 39, 176), 10)));

            if (_measurePoints.Count >= 3)
            {
                // Xoa polygon cu
                _measureOverlay.Graphics
                    .Where(g => g.Geometry is Polygon).ToList()
                    .ForEach(g => _measureOverlay.Graphics.Remove(g));

                var closed = new List<MapPoint>(_measurePoints) { _measurePoints[0] };
                var poly   = new Polygon(closed, SpatialReferences.Wgs84);
                _measureOverlay.Graphics.Add(new Graphic(poly, new SimpleFillSymbol(
                    SimpleFillSymbolStyle.Solid,
                    System.Drawing.Color.FromArgb(60, 156, 39, 176),
                    new SimpleLineSymbol(SimpleLineSymbolStyle.Solid,
                        System.Drawing.Color.FromArgb(156, 39, 176), 2))));

                var areaM2  = Math.Abs(GeometryEngine.AreaGeodetic(poly,
                    AreaUnits.SquareMeters, GeodeticCurveType.Geodesic));
                var display = areaM2 >= 1_000_000
                    ? $"{areaM2 / 1_000_000:F4} km²"
                    : areaM2 >= 10_000
                        ? $"{areaM2 / 10_000:F2} ha"
                        : $"{areaM2:F1} m²";

                _viewModel.SetMeasurementResult(
                    $"📐 Diện tích: {display}  ({_measurePoints.Count} điểm)");
            }
            else
            {
                _viewModel.SetMeasurementResult(
                    $"Đã chọn {_measurePoints.Count} điểm — cần ít nhất 3 điểm");
            }
        }

        private void ClearMeasureOverlay()
        {
            _measureOverlay.Graphics.Clear();
            _measurePoints.Clear();
            _viewModel.SetMeasurementResult(string.Empty);
        }

        // ===================== GEOCODING =====================

        private async Task<(double Lat, double Lon, string DisplayName)?> GeocodeLocationAsync(string address)
        {
            var locator = await LocatorTask.CreateAsync(
                new Uri("https://geocode-api.arcgis.com/arcgis/rest/services/World/GeocodeServer"));

            var results = await locator.GeocodeAsync(address, new GeocodeParameters
            {
                OutputSpatialReference = SpatialReferences.Wgs84,
                MaxResults             = 1
            });

            if (results.Count == 0) return null;
            var pt = (MapPoint)results[0].DisplayLocation!;
            return (pt.Y, pt.X, results[0].Label);
        }

        // ===================== EXPORT MAP =====================

        private async Task ExportMapToFileAsync()
        {
            var dialog = new SaveFileDialog
            {
                Title      = "Xuất bản đồ",
                Filter     = "PNG Image (*.png)|*.png",
                DefaultExt = "png",
                FileName   = $"BanDo_{DateTime.Now:yyyyMMdd_HHmmss}"
            };
            if (dialog.ShowDialog() != true) return;

            // Render MapView thanh bitmap WPF
            await Task.Delay(100); // Cho map hoan tat render
            var rtb = new System.Windows.Media.Imaging.RenderTargetBitmap(
                (int)MainMapView.ActualWidth,
                (int)MainMapView.ActualHeight,
                96, 96,
                System.Windows.Media.PixelFormats.Pbgra32);
            rtb.Render(MainMapView);

            using var stream = File.Create(dialog.FileName);
            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rtb));
            encoder.Save(stream);

            MessageBox.Show($"✅ Xuất thành công!\n\n{dialog.FileName}",
                "Xuất bản đồ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ===================== HELPERS =====================

        private static Basemap CreateBasemap(string style) => style switch
        {
            "ArcGIS:Streets"    => new Basemap(BasemapStyle.ArcGISStreets),
            "ArcGIS:Navigation" => new Basemap(BasemapStyle.ArcGISNavigation),
            "ArcGIS:Imagery"    => new Basemap(BasemapStyle.ArcGISImagery),
            "ArcGIS:Oceans"     => new Basemap(BasemapStyle.ArcGISOceans),
            "OpenStreetMap"     => new Basemap(BasemapStyle.OSMStreets),
            _                   => new Basemap(BasemapStyle.ArcGISTopographic)
        };

        private static double GetScaleFromZoom(int zoom) =>
            591_657_550.5 / Math.Pow(2, zoom - 1);
    }
}
