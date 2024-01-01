using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace net8auth.auth.Controllers;

[ApiController]
[Route("connect")]
public class ConnectController : ControllerBase
{
    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        var claims = new Dictionary<string, object>()
        {
            { JwtRegisteredClaimNames.Sub, "atlemagnussen@gmail.com" },
            { JwtRegisteredClaimNames.Name, "atle" },
            { "role", "Admin" }
        };
        
        

        SecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("hello_world_whathello_world_whathello_world_whathello_world_what"));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        
        var handler = new JsonWebTokenHandler();
        var desc = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            Issuer = "test",
            Claims = claims,
            Expires = DateTime.UtcNow.AddDays(-1),
        };

        var jwtUnsigned = handler.CreateToken(desc);
        
        return Ok(jwtUnsigned);
    }
}