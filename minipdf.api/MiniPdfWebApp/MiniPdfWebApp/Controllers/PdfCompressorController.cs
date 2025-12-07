using Domain.PdfCompressor;
using Microsoft.AspNetCore.Mvc;
using MiniPdfWebApp.Results;
using System.IO.Compression;

namespace MiniPdfWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfCompressorController(IPdfCompressor pdfCompressor) : ControllerBase
    {
        [HttpGet]
        [Route("document")]
        public async Task<IActionResult> GetDocument([FromQuery] string identification)
        {
            string transactionId = identification.Split("\\")[0];
            string rootFolder = string.Concat(AppDomain.CurrentDomain.BaseDirectory, transactionId, "\\mini-output\\");
            Directory.Delete(rootFolder, true);
            // System.IO.File.Delete(rootFolder + pdfDataResults[0].Filename);

            var bytes = await System.IO.File.ReadAllBytesAsync(string.Concat(AppDomain.CurrentDomain.BaseDirectory, identification.Split("\\")[0], "\\", identification, "\\miniminipdf.zip"));
            return File(bytes, "application/zip", "miniminipdf.zip");
        }

        [HttpPost]
        [Route("compress")]
        public async Task<IActionResult> Compress([FromForm] IFormFile[] files, [FromQuery] int quality, [FromQuery] int maxSizeKb)
        {
            if (files != null && files.Length == 0)
                return Ok(new PdfCompressorResult(0, 0, 0, string.Empty, []));

            string transactionId = Guid.NewGuid().ToString();
            string rootFolder = string.Concat(AppDomain.CurrentDomain.BaseDirectory, transactionId, "\\");

            IFormFile pdfFile;
            PdfData[] compressedPdfFiles = new PdfData[files.Length];
            PdfDataResult[] pdfDataResults = new PdfDataResult[files.Length];
            Directory.CreateDirectory(rootFolder);

            for (int i = 0; i < files.Length; i++)
            {
                pdfFile = files[i];

                // Validate PDF uploaded file
                if (!pdfFile.ContentType.ToLowerInvariant().Equals("application/pdf") && !".pdf".Equals(Path.GetExtension(pdfFile.FileName).ToLowerInvariant()))
                    continue;

                PdfData pdfData = new()
                {
                    OriginalSize = pdfFile.Length,
                    RootFolder = rootFolder,
                    FileName = pdfFile.FileName
                };

                await pdfFile.CopyToAsync(new FileStream(pdfData.OriginalPath, FileMode.Create));
                compressedPdfFiles[i] = await pdfCompressor.CompressAsync(pdfData, quality);

                pdfDataResults[i] = new(
                    pdfData.OriginalSize,
                    pdfData.CurrentSize,
                    CalculateReductionPercentage(pdfData.OriginalSize, pdfData.CurrentSize),
                    pdfData.FileName
                );
            }

            string identification = transactionId + "\\" + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            string publicURI = rootFolder + identification + "\\";
            Directory.CreateDirectory(publicURI);
            ZipFile.CreateFromDirectory(rootFolder + "mini-output\\", string.Concat(publicURI, "miniminipdf.zip"));

            pdfDataResults.Sort(new Comparison<PdfDataResult>((x, y) =>
            {
                if (x.ReductionPercentage > y.ReductionPercentage)
                    return -1;
                else if (x.ReductionPercentage < y.ReductionPercentage)
                    return 1;
                return 0;
            }));

            PdfCompressorResult result =
                new(compressedPdfFiles.Sum(f => f.OriginalSize),
                    compressedPdfFiles.Sum(f => f.CurrentSize),
                    CalculateReductionPercentage(compressedPdfFiles.Sum(f => f.OriginalSize), compressedPdfFiles.Sum(f => f.CurrentSize)),
                    identification,
                    pdfDataResults);

            return Ok(result);
        }

        private static int CalculateReductionPercentage(long originalSize, long compressedSize)
        {
            int calc = (int)Math.Round(100.0 * (1.0 - (double)compressedSize / originalSize));
            if (calc < 0) calc = 0;
            return calc;
        }
    }
}