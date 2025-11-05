using ReportCatalog.Domain.Abstractions;

namespace ReportCatalog.Application.Abstractions;

public interface IReportStrategySelector
{
    /// <summary>
    /// Resolve um gerador (Strategy) pelo identificador do formato (ex.: "pdf", "xlsx", "json").
    /// </summary>
    IReportGenerator Resolve(string type);
}
