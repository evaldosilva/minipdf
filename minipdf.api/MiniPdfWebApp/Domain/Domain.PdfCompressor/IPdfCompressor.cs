namespace Domain.PdfCompressor;

public interface IPdfCompressor
{
    Task<byte[]> CompressAsync(byte[] pdfData, int qualityPercentage);
}