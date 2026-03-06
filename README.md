Dưới đây là nội dung file README.md hoàn chỉnh, được thiết kế chuyên nghiệp cho dự án MilitaryGeo.QC của bạn. Nội dung này phản ánh đúng cấu trúc Clean Architecture và các công nghệ bạn đang sử dụng (ArcGIS, Syncfusion, CommunityToolkit MVVM).

🛡️ MilitaryGeo.QC - Hệ Thống Quản Lý Thông Tin Địa Lý Quân Sự
MilitaryGeo.QC là một giải pháp phần mềm chuyên dụng được xây dựng trên nền tảng .NET 8, phục vụ mục đích quản lý dữ liệu địa lý, tác chiến và điều hành quân sự. Dự án áp dụng kiến trúc Clean Architecture để đảm bảo tính mở rộng, bảo mật và dễ dàng bảo trì.

🏗️ Kiến Trúc Dự Án (Clean Architecture)
Dự án được chia thành 4 lớp chính nhằm tách biệt hoàn toàn logic nghiệp vụ và giao diện người dùng:

MilitaryGeo.Domain: Chứa các Entity, Enum, Exception và các quy tắc cốt lõi của hệ thống.

MilitaryGeo.Application: Chứa các Interface (Service/Repository), DTOs, Mapping và logic xử lý nghiệp vụ chính.

MilitaryGeo.Infrastructure: Triển khai thực tế các dịch vụ (như ArcGIS, lưu trữ MinIO, SQL Server) và các cấu hình Dependency Injection (DI).

MilitaryGeo.Desktop: Giao diện người dùng (WPF) được xây dựng theo mô hình MVVM, sử dụng bộ điều khiển cao cấp từ Syncfusion.

🚀 Công Nghệ Sử Dụng
Framework: .NET 8 (WPF)

MVVM Toolkit: CommunityToolkit.Mvvm (Microsoft)

UI Controls: Syncfusion WPF (DataGrid, Buttons, Layouts)

GIS Engine: Esri.ArcGISRuntime.WPF (Hiển thị và phân tích bản đồ)

Validation: FluentValidation

Dependency Injection: Microsoft.Extensions.DependencyInjection

Logging & Config: Microsoft.Extensions.Configuration

Object Storage: Hỗ trợ kết nối MinIO cho việc lưu trữ tệp tin.

🛠️ Hướng Dẫn Cài Đặt
1. Yêu cầu hệ thống
Visual Studio 2022 (v17.8 trở lên)

.NET 8 SDK

Cung cấp API Key cho ArcGIS và License Key cho Syncfusion.

2. Cấu hình
Mở file appsettings.json trong dự án MilitaryGeo.Desktop và cập nhật các thông số:

JSON
{
  "Syncfusion": {
    "LicenseKey": "YOUR_SYNCFUSION_KEY"
  },
  "ArcGIS": {
    "ApiKey": "YOUR_ARCGIS_API_KEY"
  },
  "ApiSettings": {
    "BaseUrl": "https://your-api-url.com/v1/"
  }
}
3. Build dự án
Restore NuGet Packages.

Chọn dự án MilitaryGeo.Desktop làm Startup Project.

Nhấn F5 để chạy ứng dụng.

📂 Cấu Trúc Thư Mục Quan Trọng
Desktop/ViewModels: Chứa logic điều khiển cho các màn hình (NguoiDung, Map, AddUser...).

Desktop/UserControls: Chứa giao diện XAML được thiết kế hiện đại.

Infrastructure/DI: Nơi đăng ký tập trung các dịch vụ hệ thống.

Application/Interfaces: Các định nghĩa service giúp tách biệt logic UI và Backend.

📝 Ghi Chú Phát Triển
Localization: Hệ thống đã được cấu hình mặc định ngôn ngữ Tiếng Việt (vi-VN).

Messaging: Sử dụng IMessageService để quản lý thông báo, giúp ViewModel hoàn toàn độc lập với UI.

Styles: Các Button và Control được quản lý tập trung trong Resources/Styles để đảm bảo tính đồng nhất giao diện.

🤝 Liên Hệ & Hỗ Trợ
Dự án được phát triển bởi đội ngũ kỹ thuật thuộc MilitaryGeo. Mọi đóng góp hoặc báo lỗi vui lòng gửi qua hệ thống quản lý mã nguồn nội bộ.

Bản quyền © 2026 MilitaryGeo. Bảo lưu mọi quyền.
