using ReportCatalog.Domain.Models;

namespace ReportCatalog.Application.Abstractions;

public interface IReportService
{
    /// <summary>
    /// Gera um arquivo de relatório para os dados e especificação informados.
    /// </summary>
    ReportFile Generate<T>(string type, ReportRequest<T> request);
}
