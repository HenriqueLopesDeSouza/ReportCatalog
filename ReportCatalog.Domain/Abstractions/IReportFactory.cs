namespace ReportCatalog.Domain.Abstractions;

public interface IReportFactory
{
    string Type { get; } 
    IReportGenerator Create(); 
}
