using ReportCatalog.Domain.Abstractions;

namespace ReportCatalog.Formats.Json;

public sealed class JsonReportFactory : IReportFactory
{
    public string Type => "json";

    public IReportGenerator Create()
        => new JsonReportGenerator(); // Cria o Strategy específico
}
