using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using net8auth.model;
using System.Security.Cryptography;

namespace net8auth.auth.Controllers;

[ApiController]
[Route("connect")]
public class ConnectController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ConnectController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        var claims = new Dictionary<string, object>()
        {
            { JwtRegisteredClaimNames.Sub, "atlemagnussen@gmail.com" },
            { JwtRegisteredClaimNames.Name, "atle" },
            { "role", "Admin" }
        };
        
        CryptoKeyPair keyPair = new CryptoKeyPair();
        var sectionKey = _configuration.GetSection("CryptoKey");
        sectionKey.Bind(keyPair);
        
        
        // SecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("hello_world_whathello_world_whathello_world_whathello_world_what"));
        // var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var key = CryptoService.GetSecurityKeyFromJwk(keyPair.PrivateKey!, true);
        
        var handler = new JsonWebTokenHandler();
        var desc = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            Issuer = "test",
            Claims = claims,
            Expires = DateTime.UtcNow.AddDays(-1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
        };

        var jwt = handler.CreateToken(desc);
        //jwtUnsigned = jwtUnsigned.Trim('.');
        
        //var jwtSignature = CryptoService.SignData(jwtUnsigned, keyPair);

        //var jwt = $"{jwtUnsigned}.{jwtSignature}";
        return Ok(jwt);
    }
}