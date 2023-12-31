using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace net8auth.auth.Controllers;

[ApiController]
[Route("connect")]
public class ConnectController : ControllerBase
{
    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        
        var handler = new JsonWebTokenHandler();
        var desc = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            Issuer = "test"
        };

        var jwt = handler.CreateToken(desc);
        return Ok(jwt);
    }
}