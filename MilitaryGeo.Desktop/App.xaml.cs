using Microsoft.Extensions.Configuration;
using Syncfusion.Licensing;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MilitaryGeo.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            
            var licenseKey = configuration["Syncfusion:LicenseKey"];
            SyncfusionLicenseProvider.RegisterLicense(licenseKey);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
