namespace ReportCatalog.Domain.Models;

public static class ReportFormat
{
    public const string Pdf = "pdf";
    public const string Xlsx = "xlsx";
    public const string Json = "json";

    public static readonly IReadOnlySet<string> All =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase) { Pdf, Xlsx, Json };
}
