using ClosedXML.Excel;
using ReportCatalog.Domain.Abstractions;
using ReportCatalog.Domain.Models;
using ReportCatalog.Domain.Utils;
using System.Globalization;

namespace ReportCatalog.Formats.Xlsx;

public sealed class XlsxReportGenerator : IReportGenerator
{
    public string Type => ReportFormat.Xlsx;
    public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileExtension => "xlsx";

    public ReportFile Generate<T>(ReportRequest<T> request)
    {
        var culture = request.Spec.Culture ?? CultureInfo.CurrentCulture;
        var columns = request.Spec.Columns?.ToList() ?? new();

        // Se não vierem colunas, inferimos automaticamente as propriedades do tipo T
        if (!columns.Any())
        {
            columns = typeof(T)
                .GetProperties()
                .Select(p => new ReportColumn(p.Name, p.Name))
                .ToList();
        }

        // Compila os accessors de propriedades (permite acessar "Cliente.Nome", etc)
        var accessors = columns
            .Select(c => new { c, acc = PropertyAccessor.Compile<T>(c.PropertyPath) })
            .ToList();

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add(request.Spec.Title ?? "Relatório");

        int row = 1;

        // ======= TÍTULO =======
        ws.Cell(row, 1).Value = request.Spec.Title ?? "Relatório";
        ws.Cell(row, 1).Style.Font.Bold = true;
        ws.Cell(row, 1).Style.Font.FontSize = 16;
        ws.Range(row, 1, row, columns.Count).Merge();
        ws.Row(row).Height = 25;
        row += 2;

        // ======= CABEÇALHO =======
        for (int i = 0; i < columns.Count; i++)
        {
            ws.Cell(row, i + 1).Value = columns[i].Header ?? columns[i].PropertyPath;
            ws.Cell(row, i + 1).Style.Font.Bold = true;
            ws.Cell(row, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(row, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        }
        row++;

        // ======= DADOS =======
        foreach (var item in request.Data)
        {
            for (int i = 0; i < accessors.Count; i++)
            {
                var raw = accessors[i].acc(item);
                var formatted = FormatSimple(raw, accessors[i].c.Format, culture);
                ws.Cell(row, i + 1).Value = formatted ?? string.Empty;

                ws.Cell(row, i + 1).Style.Alignment.Horizontal = accessors[i].c.Alignment switch
                {
                    TextAlignment.Center => XLAlignmentHorizontalValues.Center,
                    TextAlignment.Right => XLAlignmentHorizontalValues.Right,
                    _ => XLAlignmentHorizontalValues.Left
                };
            }
            row++;
        }

        // ======= AJUSTES =======
        ws.Columns().AdjustToContents();
        ws.SheetView.FreezeRows(3); // congela título e cabeçalho

        // ======= SALVAR =======
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var bytes = stream.ToArray();

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

    private static string? FormatSimple(object? value, string? format, CultureInfo culture)
    {
        if (value is null) return null;

        if (!string.IsNullOrWhiteSpace(format) && value is IFormattable fmt1)
            return fmt1.ToString(format, culture);

        if (value is IFormattable fmt2)
            return fmt2.ToString(null, culture);

        return value.ToString();
    }
}
