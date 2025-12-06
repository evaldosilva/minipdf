namespace Domain.PdfCompressor;

public class PdfData
{
    public string RootFolder { get; set; }
    public long CurrentSize { get; set; }
    public long OriginalSize { get; set; }
    public string FileName { get; set; }
    public string OriginalPath { get => string.Concat(RootFolder, FileName); }
}