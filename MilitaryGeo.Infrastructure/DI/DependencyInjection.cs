using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilitaryGeo.Application.Configuration;
using MilitaryGeo.Application.Interfaces;
using MilitaryGeo.Application.Services;

namespace MilitaryGeo.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Configure ArcGIS settings
        services.Configure<ArcGISConfiguration>(
            configuration.GetSection("ArcGIS"));

        // Register application services
        services.AddSingleton<INguoiDungService, NguoiDungService>();
        return services;
    }
}
