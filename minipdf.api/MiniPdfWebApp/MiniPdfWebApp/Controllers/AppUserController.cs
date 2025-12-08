using Domain.AppUser;
using Microsoft.AspNetCore.Mvc;

namespace MiniPdfWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AppUserController(IAppUser appUser) : ControllerBase
{
    [HttpGet]
    [Route("RemainingConvertions")]
    public ActionResult GetRemainingConvertions()
    {
        return Ok(appUser.GetRemainingConvertions("test"));
    }
}