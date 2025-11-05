using ReportCatalog.Domain.Models;

namespace ReportCatalog.Domain.Abstractions;

public interface IReportGenerator
{
    string Type { get; }           
    string ContentType { get; }    
    string FileExtension { get; }   

    ReportFile Generate<T>(ReportRequest<T> request);
}
