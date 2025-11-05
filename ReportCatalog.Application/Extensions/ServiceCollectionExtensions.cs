using Microsoft.Extensions.DependencyInjection;
using ReportCatalog.Application.Abstractions;
using ReportCatalog.Application.Services;

namespace ReportCatalog.Application.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra os serviços de aplicação (Application Layer).
    /// </summary>
    public static IServiceCollection AddReportApplication(this IServiceCollection services)
    {
        services.AddScoped<IReportStrategySelector, ReportStrategySelector>();
        services.AddScoped<IReportService, ReportService>();
        return services;
    }
}
