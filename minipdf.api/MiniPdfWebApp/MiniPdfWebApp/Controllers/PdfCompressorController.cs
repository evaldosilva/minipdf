using Microsoft.AspNetCore.Mvc;

namespace MiniPdfWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfCompressorController : ControllerBase
    {
        [HttpPost]
        [Route("compress")]
        public async Task<IActionResult> Compress([FromForm] DataForm[] model)
        {

            return Ok();
        }
    }

    public class DataForm {
        public string Name { get; set; }
        public string File { get; set; }
    }
}
