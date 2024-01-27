using Microsoft.IdentityModel.Tokens;
using net8auth.model.Crypto;

namespace net8auth.model;

public static class CryptoService
{
    /// <summary>
    /// Sign a JWT
    /// </summary>
    /// <param name="jwt">Must be header.payload</param>
    /// <param name="jwk">Must be private key</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static string? SignJwk(string jwt, JsonWebKey jwk)
    {
        
        IKeyService keyService = GetKeyService(jwk);
        return keyService.SignJwt(jwt);
    }

    private static IKeyService GetKeyService(JsonWebKey jwk)
    {
        if (jwk.Kty == "EC")
            return new EcKeyService(jwk);
        else if (jwk.Kty == "RSA")
            return new RsaKeyService(jwk);
        
        throw new ApplicationException($"not supported key type {jwk.Kty}");
        
    }
    public static SecurityKey GetSecurityKeyFromJwk(JsonWebKey jwk)
    {
        IKeyService keyService = GetKeyService(jwk);
        return keyService.GetSecurityKeyFromJwk();
    }
    //public static CryptoKeyPair CreateRsaKey()
    //{
        //using var rsa = RSA.Create(keySizeInBits: 3072);

        //Console.WriteLine("Created key");
        //var privateKey = rsa.ExportPkcs8PrivateKey();
        //var privateKeyUtf8 = Encoding.UTF8.GetString(privateKey);
        //var privateKeyBase64 = Convert.ToBase64String(privateKey);

        //var publicKey = rsa.ExportRSAPublicKey();
        //var publicKeyUtf8 = Encoding.UTF8.GetString(publicKey);
        //var publicKeyBase64 = Convert.ToBase64String(publicKey);

        //RsaSecurityKey privateKey = new RsaSecurityKey(rsa.ExportParameters(true))
        // {
        //     KeyId = "key1"
        // };
        
        // var privateJwk = JsonWebKeyConverter.ConvertFromSecurityKey(privateKey);
        // privateJwk.Alg = SecurityAlgorithms.RsaSha256;
        // privateJwk.Use = "sig";
        
        // RsaSecurityKey publicKey = new(rsa.ExportParameters(false))
        // {
        //     KeyId = "key1"
        // };
        // var publicJwk = JsonWebKeyConverter.ConvertFromSecurityKey(publicKey);
        // publicJwk.Alg = SecurityAlgorithms.RsaSsaPssSha256;
        // publicJwk.Use = "sig";

        // return new CryptoKeyPair(privateJwk, publicJwk);
    //}

    // public static JsonWebKey CreateEcKey()
    // {
    //     using ECDsa ecd = ECDsa.Create(ECCurve.NamedCurves.nistP384);

    //     var privateKeyParams = ecd.ExportParameters(true);

    //     ECDsaSecurityKey privateKey = new ECDsaSecurityKey(ecd);
        
    //     var jwkPrivate = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(privateKey);
    //     return jwkPrivate;
    // }
}