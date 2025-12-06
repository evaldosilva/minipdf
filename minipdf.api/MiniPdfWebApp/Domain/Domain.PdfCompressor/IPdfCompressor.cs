namespace Domain.PdfCompressor;

public interface IPdfCompressor
{
    Task<PdfData> CompressAsync(PdfData pdfData, int qualityPercentage);
}