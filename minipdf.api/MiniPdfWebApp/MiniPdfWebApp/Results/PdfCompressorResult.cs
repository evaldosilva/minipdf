namespace MiniPdfWebApp.Results;
public record PdfCompressorResult(long OriginalSize, long CompressedSize, int ReductionPercentage, string PublicURI, PdfDataResult[] PdfDataResults);