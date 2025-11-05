namespace ReportCatalog.Domain.Models;

public sealed class ReportFile
{
    public required string FileName { get; init; }     
    public required string ContentType { get; init; }
    public required string Extension { get; init; }    
    public required byte[] Bytes { get; init; }

    public Stream AsStream() => new MemoryStream(Bytes, writable: false);
}
