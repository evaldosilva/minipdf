namespace MiniPdfWebApp.Results;
public record PdfCompressorResult(long OriginalSize, long CompressedSize, string PublicURI);