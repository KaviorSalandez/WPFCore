using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilitaryGeo.Application.Interfaces;
using MilitaryGeo.Application.Services;
using MilitaryGeo.Desktop.Services;
using MilitaryGeo.Desktop.ViewModels;
using MilitaryGeo.Infrastructure.DI;
using Syncfusion.Licensing;
using System.Globalization;
using System.Windows;

namespace MilitaryGeo.Desktop
{
    public partial class App : global::System.Windows.Application
    {
        private ServiceProvider? _serviceProvider;
        private IConfiguration? _configuration;

        public App()
        {
            // Build configuration
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var licenseKey = _configuration["Syncfusion:LicenseKey"];
            SyncfusionLicenseProvider.RegisterLicense(licenseKey);

            // Setup DI Container
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Add configuration
            services.AddSingleton(_configuration!);
            services.AddSingleton<IMessageService, MessageService>();

            // Add Infrastructure layer services
            services.AddInfrastructure(_configuration!);

            // Register ViewModels
            services.AddTransient<AddNguoiDungViewModel>();
            services.AddTransient<NguoiDungViewModel>();
            services.AddTransient<VaiTroViewModel>();
            services.AddTransient<AddVaiTroViewModel>();
            services.AddTransient<MapViewModel>();
        }

        public static T GetService<T>() where T : class
        {
            var app = (App)Current;
            return app._serviceProvider?.GetService<T>() 
                ?? throw new InvalidOperationException($"Service {typeof(T)} not registered");
        }

        public static IConfiguration GetConfiguration()
        {
            var app = (App)Current;
            return app._configuration 
                ?? throw new InvalidOperationException("Configuration not initialized");
        }

        protected override void OnStartup(global::System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);
            SetCultureToVietnamese();

        }

        protected override void OnExit(global::System.Windows.ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }

        private void SetCultureToVietnamese()
        {
            var culture = new CultureInfo("vi-VN");
            culture.DateTimeFormat.AMDesignator = "AM";
            culture.DateTimeFormat.PMDesignator = "PM";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
