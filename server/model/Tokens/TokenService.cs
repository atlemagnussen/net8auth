using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace net8auth.model.Tokens;

public class TokenService : ITokenService
{
    private readonly CryptoKeys _keys;
    
    public TokenService(GetCryptoKeys getCryptoKeys)
    {
        var keys = getCryptoKeys.FromConfig();
        if (keys == null)
            throw new ApplicationException("Missing crypto key pair");
        
        _keys = keys;
    }

    public Task<string> CreateAndSignJwt(ClaimsPrincipal user)
    {
        var claims = new Dictionary<string, object>()
        {
            { JwtRegisteredClaimNames.Sub, "atlemagnussen@gmail.com" },
            { JwtRegisteredClaimNames.Name, "atle" },
            { "role", "Admin" }
        };
        
        if (_keys.Active == null)
            throw new ApplicationException("missing private key");
        
        // SecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("hello_world_whathello_world_whathello_world_whathello_world_what"));
        // var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var key = CryptoService.GetSecurityKeyFromJwk(_keys.Active);
        var signingCred = new SigningCredentials(key, _keys.Active.Alg);

        var handler = new JsonWebTokenHandler();
        var desc = new SecurityTokenDescriptor
        {
            IssuedAt = DateTime.UtcNow,
            Issuer = "me",
            Audience = "you",
            Claims = claims,
            Expires = DateTime.UtcNow.AddDays(-1) 
            // should be of SecurityAlgorithms.RsaSha256
        };

        var jwtUnsigned = handler.CreateToken(desc);
        var token = new JsonWebToken(jwtUnsigned);
        
        var jwtPayload = Base64UrlEncoder.Decode(token.EncodedPayload);

        var additionalHeader = new Dictionary<string, object>
        {
            { "typ", "at+jwt" }
        };

        string jwt = handler.CreateToken(jwtPayload, signingCred, additionalHeader);
        return Task.FromResult(jwt);
        //jwtUnsigned = jwtUnsigned.Trim('.');
        
        //var jwtSignature = CryptoService.SignData(jwtUnsigned, keyPair);

        //var jwt = $"{jwtUnsigned}.{jwtSignature}";
    }
}
