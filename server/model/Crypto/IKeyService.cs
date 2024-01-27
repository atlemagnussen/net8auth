using Microsoft.IdentityModel.Tokens;

namespace net8auth.model.Crypto;

public interface IKeyService
{
    string SignJwt(string jwt);
    SecurityKey GetSecurityKeyFromJwk();
    bool VerifySignedHash(string dataBase64, string signatureBase64);
}