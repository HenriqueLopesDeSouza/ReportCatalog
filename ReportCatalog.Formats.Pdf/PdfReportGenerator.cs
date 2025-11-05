using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ReportCatalog.Domain.Abstractions;
using ReportCatalog.Domain.Models;

namespace ReportCatalog.Formats.Pdf;

public sealed class PdfReportGenerator : IReportGenerator
{
    public string Type => ReportFormat.Pdf;
    public string ContentType => "application/pdf";
    public string FileExtension => "pdf";

    static PdfReportGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public ReportFile Generate<T>(ReportRequest<T> request)
    {
        var bytes = Document.Create(doc =>
        {
            doc.Page(p =>
            {
                p.Size(PageSizes.A4);
                p.Margin(30);
                p.DefaultTextStyle(x => x.FontSize(16));

                p.Content()
                 .AlignCenter()
                 .AlignMiddle()
                 .Text("teste")
                 .SemiBold();

                p.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Página "); t.CurrentPageNumber();
                    t.Span(" de "); t.TotalPages();
                });
            });
        }).GeneratePdf();

        var name = string.IsNullOrWhiteSpace(request.Spec.FileName)
            ? (string.IsNullOrWhiteSpace(request.Spec.Title) ? "report" : request.Spec.Title)
            : request.Spec.FileName;

        return new ReportFile
        {
            FileName = $"{name}.{FileExtension}",
            ContentType = ContentType,
            Extension = FileExtension,
            Bytes = bytes
        };
    }
}