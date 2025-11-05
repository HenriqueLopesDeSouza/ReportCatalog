using ReportCatalog.Domain.Abstractions;

namespace ReportCatalog.Formats.Xlsx;

public sealed class XlsxReportFactory : IReportFactory
{
    public string Type => "xlsx";
    public IReportGenerator Create() => new XlsxReportGenerator();
}
