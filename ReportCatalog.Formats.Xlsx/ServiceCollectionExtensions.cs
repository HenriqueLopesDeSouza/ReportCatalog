using Microsoft.Extensions.DependencyInjection;
using ReportCatalog.Domain.Abstractions;

namespace ReportCatalog.Formats.Xlsx;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXlsxReportFormat(this IServiceCollection services)
    {
        services.AddSingleton<IReportFactory, XlsxReportFactory>();
        return services;
    }
}
