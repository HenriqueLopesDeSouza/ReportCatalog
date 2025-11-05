namespace ReportCatalog.Domain.Models;

public sealed record ReportRequest<T>(
    IEnumerable<T> Data,
    ReportSpec Spec
);
