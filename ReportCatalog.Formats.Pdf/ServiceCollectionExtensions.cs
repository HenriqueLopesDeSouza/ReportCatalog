using Microsoft.Extensions.DependencyInjection;
using ReportCatalog.Domain.Abstractions;

namespace ReportCatalog.Formats.Pdf;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra a fábrica/gerador de PDF na coleção de serviços.
    /// </summary>
    public static IServiceCollection AddPdfReportFormat(this IServiceCollection services)
    {
        services.AddSingleton<IReportFactory, PdfReportFactory>();
        return services;
    }
}
