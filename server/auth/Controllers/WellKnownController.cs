using Microsoft.AspNetCore.Mvc;
using net8auth.auth.Models;

namespace net8auth.api.Controllers;

[ApiController]
[Route(".well-known")]
public class WellKnownController : ControllerBase
{
    [HttpGet]
    public IActionResult Index() {
        return Ok("hello");
    }

    [HttpGet("openid-configuration")]
    public WellknownModel OpenIdConfiguration()
    {
        return new WellknownModel
        {
            Issuer = "this"
        };
    }
}