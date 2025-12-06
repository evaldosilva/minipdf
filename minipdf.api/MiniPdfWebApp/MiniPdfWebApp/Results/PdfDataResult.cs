namespace MiniPdfWebApp.Results;
public record PdfDataResult(long OriginalSize, long CompressedSize, int ReductionPercentage, string Filename);