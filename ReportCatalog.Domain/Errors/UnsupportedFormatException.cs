namespace ReportCatalog.Domain.Errors;

public sealed class UnsupportedFormatException : Exception
{
    public UnsupportedFormatException(string type)
        : base($"Report format '{type}' is not supported.") { }
}
