using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace net8auth.model.Test;

public static class CreateKeys
{
    public static JsonWebKey CreateEcKey()
    {
        using ECDsa ecd = ECDsa.Create(ECCurve.NamedCurves.nistP521);

        var privateKeyParams = ecd.ExportParameters(true);

        ECDsaSecurityKey privateKey = new ECDsaSecurityKey(ecd);
        
        var jwkPrivate = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(privateKey);
        return jwkPrivate;
    }

    public static JsonWebKey CreateRsaKey()
    {
        using var rsa = RSA.Create(keySizeInBits: 3072);

        // Console.WriteLine("Created key");
        // var privateKey = rsa.ExportPkcs8PrivateKey();
        // var privateKeyUtf8 = Encoding.UTF8.GetString(privateKey);
        // var privateKeyBase64 = Convert.ToBase64String(privateKey);

        // var publicKey = rsa.ExportRSAPublicKey();
        // var publicKeyUtf8 = Encoding.UTF8.GetString(publicKey);
        // var publicKeyBase64 = Convert.ToBase64String(publicKey);

        RsaSecurityKey privateKey = new RsaSecurityKey(rsa.ExportParameters(true))
        {
            KeyId = "key1"
        };
        
        var privateJwk = JsonWebKeyConverter.ConvertFromSecurityKey(privateKey);
        privateJwk.Alg = SecurityAlgorithms.RsaSha256;
        privateJwk.Use = "sig";
        
        // RsaSecurityKey publicKey = new(rsa.ExportParameters(false))
        // {
        //     KeyId = "key1"
        // };
        // var publicJwk = JsonWebKeyConverter.ConvertFromSecurityKey(publicKey);
        // publicJwk.Alg = SecurityAlgorithms.RsaSsaPssSha256;
        // publicJwk.Use = "sig";

        return privateJwk;
    }
}