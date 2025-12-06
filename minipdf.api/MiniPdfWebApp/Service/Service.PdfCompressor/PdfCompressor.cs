using Docnet.Core;
using Docnet.Core.Converters;
using Docnet.Core.Editors;
using Docnet.Core.Models;
using Domain.PdfCompressor;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace Service.PdfCompressor;

public class PdfCompressor : IPdfCompressor
{
    private const string Path = "../../../test.pdf";
    private const string OutputFilePrefix = "img-";
    private const string OutputFileExtension = ".jpeg";
    private const int dimOne = 1080,
                      dimTwo = 1920;
    private readonly NaiveTransparencyRemover bgColor = new(255, 255, 255);
    private int _pdfQualityPercentage = 20;
    private int _memoryStreamInitBytes = 100;

    public Task<byte[]> CompressAsync(byte[] pdfData, int qualityPercentage)
    {
        GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
            PdfToImages(false);
                ImagesToPdf();

        throw new NotImplementedException();
    }

    private void PdfToImages(bool markText = false)
    {
        using var docReader = DocLib.Instance.GetDocReader(Path, new PageDimensions(dimOne, dimTwo));

        int page = 0;
        while (page < docReader.GetPageCount())
        {
            using var pageReader = docReader.GetPageReader(page);

            var rawBytes = pageReader.GetImage(bgColor);

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();

            IEnumerable<Character> characters = null;
            if (markText)
                characters = pageReader.GetCharacters();

            Bitmap bmp = GenerateImage(width, height);

            AddBytes(bmp, rawBytes);

            if (markText)
                DrawRectangles(bmp, characters);

            ResizeImage(bmp, width, height);

            using var stream = new MemoryStream(_memoryStreamInitBytes);

            EncoderParameters myEncoderParameters = new(1);
            EncoderParameter myEncoderParameter = new(Encoder.Quality, _pdfQualityPercentage);
            myEncoderParameters.Param[0] = myEncoderParameter;

            bmp.Save(stream, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);

            File.WriteAllBytes($"../../../{OutputFilePrefix}{page}{OutputFileExtension}", stream.ToArray());
            page++;
        }
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        foreach (ImageCodecInfo codec in codecs)
            if (codec.FormatID == format.Guid)
                return codec;

        return null;
    }

    private static void ResizeImage(Bitmap image, int width, int height)
    {
        using Graphics g = Graphics.FromImage(image);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(image, 0, 0, width, height);
    }

    private static void AddBytes(Bitmap bmp, byte[] rawBytes)
    {
        var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

        var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
        var pNative = bmpData.Scan0;

        Marshal.Copy(rawBytes, 0, pNative, rawBytes.Length);
        bmp.UnlockBits(bmpData);
    }

    private static void DrawRectangles(Bitmap bmp, IEnumerable<Character> characters)
    {
        var pen = new Pen(Color.Red);

        using var graphics = Graphics.FromImage(bmp);

        foreach (var c in characters)
        {
            var rect = new Rectangle(c.Box.Left, c.Box.Top, c.Box.Right - c.Box.Left, c.Box.Bottom - c.Box.Top);
            graphics.DrawRectangle(pen, rect);
        }
    }

    private static void ImagesToPdf()
    {
        string searchFolder = "../../../";
        var filters = new string[] { "jpg", "jpeg" };
        var files = GetFilesFrom(searchFolder, filters, false);
        var images = new List<JpegImage>();

        foreach (var f in files)
        {
            var size = GetImageSize(f);
            var file = new JpegImage
            {
                Bytes = File.ReadAllBytes(f),
                Width = size.Width,
                Height = size.Height
            };
            images.Add(file);
        }

        var bytes = DocLib.Instance.JpegToPdf(images);
        File.WriteAllBytes("../../../output_file.pdf", bytes);
    }

    private static Size GetImageSize(string fullPath)
    {
        using System.Drawing.Image img = System.Drawing.Image.FromFile(fullPath);
        return new Size(img.Width, img.Height);
    }

    private static Bitmap GenerateImage(int width, int height) => new(width, height, PixelFormat.Format32bppArgb);

    public static string[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
    {
        List<string> filesFound = [];
        var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        foreach (var filter in filters)
            filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
        return [.. filesFound];
    }
}