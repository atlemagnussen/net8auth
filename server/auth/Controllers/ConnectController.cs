using Microsoft.AspNetCore.Mvc;
using net8auth.model.Tokens;

namespace net8auth.auth.Controllers;

[ApiController]
[Route("connect")]
public class ConnectController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public ConnectController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("authorize")]
    public async Task<IActionResult> Authorize()
    {
        var jwt = await _tokenService.CreateAndSignJwt(User);
        
        return Ok(jwt);
    }
}