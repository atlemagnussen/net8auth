using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace net8auth.model;

public static class CryptoService
{
    /// <summary>
    /// Sign a JWT
    /// </summary>
    /// <param name="jwt">Must be header.payload</param>
    /// <param name="keyPair">Must be private key</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static string? SignJwk(string jwt, CryptoKeys keys)
    {
        // The array to store the signed message in bytes
        byte[] signedBytes;
        
        
        // hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
        if (keys.Active == null) 
            throw new ApplicationException("missing private key");
        
        // Write the message to a byte array using UTF8 as the encoding.
        var encoder = new UTF8Encoding();
        byte[] originalData = encoder.GetBytes(jwt);

        try
        {
            SecurityKey parms = ConvertFromJwk(keys.Active);
            using var rsa = RSA.Create(parms);
            
            //ReadOnlySpan<byte> privateKey = Convert.FromBase64String(keyPair.PrivateKey).AsSpan();
            //var intRead = 0;
            
            signedBytes = rsa.SignData(originalData, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
        }
        catch (CryptographicException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
        //}
        // Convert the a base64 string before returning
        return Base64UrlEncoder.Encode(signedBytes);
    }

    public static SecurityKey ConvertFromJwk(JsonWebKey jwk)
    {
        if (jwk.Kty == "EC")
            return ConvertEcFromJwk(jwk);
        else if (jwk.Kty == "RSA")
            return GetSecurityRsaKeyFromJwk(jwk);
        else
            throw new ApplicationException($"not supported key type {jwk.Kty}");
    }

    public static SecurityKey ConvertEcFromJwk(JsonWebKey jwk)
    {
        var curve = jwk.Crv switch
        {
            "P-256" => ECCurve.NamedCurves.nistP256,
            "P-384" => ECCurve.NamedCurves.nistP384,
            "P-521" => ECCurve.NamedCurves.nistP521,
            _ => throw new NotSupportedException()
        };

        var parms = new ECParameters
        {
            Curve = curve,
            Q = new ECPoint
            {
                X = Base64UrlEncoder.DecodeBytes(jwk.X),
                Y = Base64UrlEncoder.DecodeBytes(jwk.Y)
            }
        };
        if (jwk.HasPrivateKey)
            parms.D = Base64UrlEncoder.DecodeBytes(jwk.D);
        
        using ECDsa key = ECDsa.Create();
        var securityKey = new ECDsaSecurityKey(key)
        {
            KeyId = jwk.Kid
        };
        return securityKey;
    }

    public static SecurityKey GetSecurityRsaKeyFromJwk(JsonWebKey jwk)
    {
        var parms = ConvertRsaFromJwk(jwk);
        var rsa = RSA.Create(parms);

        SecurityKey key = new RsaSecurityKey(rsa)
        {
            KeyId = jwk.Kid
        };
        return key;
    }

    public static RSAParameters ConvertRsaFromJwk(JsonWebKey jwk) {
        RSAParameters parms = new RSAParameters
        {
            // PUBLIC KEY PARAMETERS
            Modulus = Base64UrlEncoder.DecodeBytes(jwk.N),
            Exponent = Base64UrlEncoder.DecodeBytes(jwk.E),
        };
        if (jwk.HasPrivateKey) {
            // PRIVATE KEY PARAMETERS (optional)
            parms.D = Base64UrlEncoder.DecodeBytes(jwk.D);
            parms.DP = Base64UrlEncoder.DecodeBytes(jwk.DP);
            parms.DQ = Base64UrlEncoder.DecodeBytes(jwk.DQ);
            parms.P = Base64UrlEncoder.DecodeBytes(jwk.P);
            parms.Q = Base64UrlEncoder.DecodeBytes(jwk.Q);
            parms.InverseQ = Base64UrlEncoder.DecodeBytes(jwk.QI);
        }
        return parms;
    }

    public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
    {
        try
        {
            using var rsa = RSA.Create(Key);
            return rsa.SignData(DataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch(CryptographicException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
    {
        try
        {
            using var rsa = RSA.Create(Key);
            return rsa.VerifyData(DataToVerify, SignedData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch(CryptographicException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static void TestRSA()
    {
        RSA.Create();
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

    public static JsonWebKey CreateEcKey()
    {
        using ECDsa ecd = ECDsa.Create(ECCurve.NamedCurves.nistP384);

        var privateKeyParams = ecd.ExportParameters(true);

        

        ECDsaSecurityKey privateKey = new ECDsaSecurityKey(ecd);
        
        var jwkPrivate = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(privateKey);
        return jwkPrivate;
    }
}