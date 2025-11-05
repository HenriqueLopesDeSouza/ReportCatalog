using ReportCatalog.Domain.Abstractions;

namespace ReportCatalog.Formats.Pdf
{
    public sealed class PdfReportFactory : IReportFactory
    {
        public string Type => "Pdf";

        public IReportGenerator Create()
            => new PdfReportGenerator(); // Cria o Strategy específico
    }
}
