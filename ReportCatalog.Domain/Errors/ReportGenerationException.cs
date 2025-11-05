namespace ReportCatalog.Domain.Errors;

public sealed class ReportGenerationException : Exception
{
    public ReportGenerationException(string message, Exception? inner = null)
        : base(message, inner) { }
}
