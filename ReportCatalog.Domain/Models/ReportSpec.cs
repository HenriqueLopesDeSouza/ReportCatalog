using System.Globalization;

namespace ReportCatalog.Domain.Models;

public sealed record ReportSpec(
    string Title,
    IReadOnlyList<ReportColumn>? Columns = null,
    CultureInfo? Culture = null,
    string? FileName = null,                      
    IReadOnlyDictionary<string, string>? Meta = null 
);
