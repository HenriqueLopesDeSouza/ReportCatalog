namespace ReportCatalog.Domain.Models;

public sealed record ReportColumn(
    string Header,
    string PropertyPath,          
    string? Format = null,          
    float? Width = null,        
    TextAlignment Alignment = TextAlignment.Left
);

public enum TextAlignment { Left, Center, Right }
