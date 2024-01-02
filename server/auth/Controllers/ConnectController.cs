using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using net8auth.model;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace net8auth.auth.Controllers;

[ApiController]
[Route("connect")]
public class ConnectController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly CryptoKeyPair _keyPair;

    public ConnectController(IConfiguration configuration,
        IOptions<CryptoKeyPair> cryptoKeyOptions)
    {
        _configuration = configuration;
        if (cryptoKeyOptions == null)
            throw new ApplicationException("Missing crypto key pair");
        
        _keyPair = cryptoKeyOptions.Value;
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
        
        if (_keyPair.PrivateKey == null)
            throw new ApplicationException("missing private key");
        
        // SecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("hello_world_whathello_world_whathello_world_whathello_world_what"));
        // var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var key = CryptoService.GetSecurityKeyFromJwk(_keyPair.PrivateKey, true);
        
        var handler = new JsonWebTokenHandler();
        var desc = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            Issuer = "test",
            Claims = claims,
            Expires = DateTime.UtcNow.AddDays(-1),
            SigningCredentials = new SigningCredentials(key, _keyPair.PrivateKey.Alg) // should be of SecurityAlgorithms.RsaSha256
        };

        var jwt = handler.CreateToken(desc);
        //jwtUnsigned = jwtUnsigned.Trim('.');
        
        //var jwtSignature = CryptoService.SignData(jwtUnsigned, keyPair);

        //var jwt = $"{jwtUnsigned}.{jwtSignature}";
        return Ok(jwt);
    }
}