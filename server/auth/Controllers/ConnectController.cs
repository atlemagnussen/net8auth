using Microsoft.AspNetCore.Mvc;

namespace net8auth.auth.Controllers;

[ApiController]
[Route("connect")]
public class ConnectController : ControllerBase
{
    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        return Ok();
    }
}