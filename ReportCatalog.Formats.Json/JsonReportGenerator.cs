using System.Text.Json;
using ReportCatalog.Domain.Abstractions;
using ReportCatalog.Domain.Models;

namespace ReportCatalog.Formats.Json;

public sealed class JsonReportGenerator : IReportGenerator
{
    private readonly JsonSerializerOptions _options;

    public JsonReportGenerator(JsonSerializerOptions? options = null)
    {
        _options = options ?? new JsonSerializerOptions
        {
            WriteIndented = true, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    public string Type => ReportFormat.Json;                     
    public string ContentType => "application/json";
    public string FileExtension => "json";

    public ReportFile Generate<T>(ReportRequest<T> request)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(request.Data, _options);

        var baseName = string.IsNullOrWhiteSpace(request.Spec.FileName)
            ? (string.IsNullOrWhiteSpace(request.Spec.Title) ? "report" : request.Spec.Title)
            : request.Spec.FileName;

        return new ReportFile
        {
            FileName = $"{baseName}.{FileExtension}",
            ContentType = ContentType,
            Extension = FileExtension,
            Bytes = bytes
        };
    }
}
