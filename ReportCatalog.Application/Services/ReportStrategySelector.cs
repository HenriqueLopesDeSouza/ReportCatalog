using ReportCatalog.Application.Abstractions;
using ReportCatalog.Domain.Abstractions;
using ReportCatalog.Domain.Errors;

namespace ReportCatalog.Application.Services;

public sealed class ReportStrategySelector : IReportStrategySelector
{
    private readonly IReadOnlyDictionary<string, IReportFactory> _factories;

    public ReportStrategySelector(IEnumerable<IReportFactory> factories)
    {
        _factories = factories.ToDictionary(f => f.Type, StringComparer.OrdinalIgnoreCase);
    }

    public IReportGenerator Resolve(string type)
    {
        if (string.IsNullOrWhiteSpace(type) || !_factories.TryGetValue(type, out var factory))
            throw new UnsupportedFormatException(type ?? "<null>");

        return factory.Create();
    }
}
