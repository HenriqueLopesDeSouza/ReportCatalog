using ReportCatalog.Application.Abstractions;
using ReportCatalog.Domain.Models;

namespace ReportCatalog.Application.Services;

public sealed class ReportService : IReportService
{
    private readonly IReportStrategySelector _selector;

    public ReportService(IReportStrategySelector selector) => _selector = selector;

    public ReportFile Generate<T>(string type, ReportRequest<T> request)
    {
        var generator = _selector.Resolve(type);
        var file = generator.Generate(request);

        // Se o FileName não vier preenchido, sugere um padrão
        var name = string.IsNullOrWhiteSpace(file.FileName)
            ? (request.Spec.FileName ?? $"report_{DateTime.UtcNow:yyyyMMdd_HHmmss}")
            : file.FileName;

        return new ReportFile
        {
            FileName = name.EndsWith($".{file.Extension}", StringComparison.OrdinalIgnoreCase)
                ? name
                : $"{name}.{file.Extension}",
            ContentType = file.ContentType,
            Extension = file.Extension,
            Bytes = file.Bytes
        };
    }
}
