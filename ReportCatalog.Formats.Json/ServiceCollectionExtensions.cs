using Microsoft.Extensions.DependencyInjection;
using ReportCatalog.Domain.Abstractions;

namespace ReportCatalog.Formats.Json;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra a fábrica/gerador de JSON na coleção de serviços.
    /// </summary>
    public static IServiceCollection AddJsonReportFormat(this IServiceCollection services)
    {
        // Como IReportFactory é o ponto de extensão, basta adicionar a Factory.
        services.AddSingleton<IReportFactory, JsonReportFactory>();
        return services;
    }
}
