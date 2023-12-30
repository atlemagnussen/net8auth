using Microsoft.AspNetCore.Mvc;
using net8auth.auth.Models;
using net8auth.auth.Services;

namespace net8auth.auth.Controllers;

[ApiController]
[Route(".well-known")]
public class WellKnownController : ControllerBase
{
    private readonly WellKnownService _service;

    public WellKnownController(WellKnownService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Index() {
        return Ok("hello");
    }

    [HttpGet("openid-configuration")]
    public DiscoveryDocumentModel OpenIdConfiguration()
    {
        var disco = _service.Get(Request);
        return disco;
    }
}