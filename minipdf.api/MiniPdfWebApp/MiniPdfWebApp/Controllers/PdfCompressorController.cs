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
        [HttpPost]
        [Route("compress")]
        public async Task<IActionResult> Compress([FromForm] IFormFile[] files)
        {
            int quality = 50;
            string transactionId = Guid.NewGuid().ToString();
            string rootFolder = string.Concat(AppDomain.CurrentDomain.BaseDirectory, transactionId, "\\");

            IFormFile pdfFile;
            PdfData[] compressedPdfFiles = new PdfData[files.Length];
            Directory.CreateDirectory(rootFolder);

            for (int i = 0; i < files.Length; i++)
            {
                pdfFile = files[i];

                PdfData pdfData = new()
                {
                    OriginalSize = pdfFile.Length,
                    RootFolder = rootFolder,
                    FileName = pdfFile.FileName
                };

                await pdfFile.CopyToAsync(new FileStream(pdfData.OriginalPath, FileMode.Create));
                compressedPdfFiles[i] = await pdfCompressor.CompressAsync(pdfData, quality);
            }

            string publicURI = rootFolder + Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + "\\";
            Directory.CreateDirectory(publicURI);
            ZipFile.CreateFromDirectory(rootFolder + "mini-output\\", string.Concat(publicURI, "miniminipdf.zip"));

            PdfCompressorResult result = 
                new(compressedPdfFiles.Sum(f => f.OriginalSize), 
                    compressedPdfFiles.Sum(f => f.CurrentSize), 
                    string.Concat(publicURI, "miniminipdf.zip"));

            return Ok(result);
        }
    }
}