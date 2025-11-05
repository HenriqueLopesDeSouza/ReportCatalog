using ReportCatalog.Api.Middleware;
using ReportCatalog.Application.Abstractions;
using ReportCatalog.Application.Extensions;
using ReportCatalog.Domain.Models;
using ReportCatalog.Formats.Json;
using ReportCatalog.Formats.Pdf;
using ReportCatalog.Formats.Xlsx;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReportApplication();

builder.Services.AddJsonReportFormat();
builder.Services.AddPdfReportFormat();
builder.Services.AddXlsxReportFormat();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();


app.MapGet("/reports/demo", (string type, IReportService reportService) =>
{
    var data = new[]
    {
        new { Id = 1, Nome = "Alice", Valor = 123.45m, Data = DateTime.Today },
        new { Id = 2, Nome = "Bob",   Valor = 67.89m,  Data = DateTime.Today.AddDays(-1) }
    };

    var spec = new ReportSpec(
        Title: "Relatório de Vendas",
        Columns: new[]
        {
            new ReportColumn("Código",  "Id",   null, 10, TextAlignment.Right),
            new ReportColumn("Cliente", "Nome", null, 30, TextAlignment.Left),
            new ReportColumn("Valor",   "Valor","n2", 12, TextAlignment.Right),
            new ReportColumn("Data",    "Data", "dd/MM/yyyy", 14, TextAlignment.Left),

        },
        Culture: CultureInfo.GetCultureInfo("pt-BR"),
        FileName: $"relatorio_{type}",
        Meta: new Dictionary<string, string> 
        {
            ["EntityName"] = "Minha Entidade",
            ["Filters"] = "Período: Mês Atual"
        }
    );

    var request = new ReportRequest<object>(data, spec);
    var file = reportService.Generate(type, request);
    return Results.File(file.Bytes, file.ContentType, file.FileName);
})
.WithName("DemoReport")
.Produces(StatusCodes.Status200OK, contentType: "application/octet-stream");



app.Run();


public sealed class ApiReportSpec
{
    public string? Title { get; set; }
    public string? FileName { get; set; }
    public string? Culture { get; set; }           
    public List<ApiReportColumn>? Columns { get; set; }
    public Dictionary<string, string>? Meta { get; set; } 
}

public sealed class ApiReportColumn
{
    public required string Header { get; set; }
    public required string PropertyPath { get; set; }  
    public string? Format { get; set; }                
    public float? Width { get; set; }                
    public string? Alignment { get; set; }           
}
