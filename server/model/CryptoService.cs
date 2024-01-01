using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace net8auth.model;

public static class CryptoService
{
    public static string? SignData(string message, CryptoKeyPair keyPair)
    {
        // The array to store the signed message in bytes
        byte[] signedBytes;
        using var rsa = new RSACryptoServiceProvider();
        
        // hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
        if (keyPair.PrivateKey == null) 
            throw new ApplicationException("missing private key");
        
        // Write the message to a byte array using UTF8 as the encoding.
        var encoder = new UTF8Encoding();
        byte[] originalData = encoder.GetBytes(message);

        try
        {
            var parms = ConvertFromJwk(keyPair.PrivateKey);
            rsa.ImportParameters(parms);
            //ReadOnlySpan<byte> privateKey = Convert.FromBase64String(keyPair.PrivateKey).AsSpan();
            //var intRead = 0;
            //rsa.ImportPkcs8PrivateKey(privateKey, out intRead);

            // Sign the data, using SHA512 as the hashing algorithm 
            //signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
            signedBytes = rsa.SignData(originalData, SHA256.Create());
        }
        catch (CryptographicException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        finally
        {
            // Set the keycontainer to be cleared when rsa is garbage collected.
            rsa.PersistKeyInCsp = false;
        }
        //}
        // Convert the a base64 string before returning
        return Convert.ToBase64String(signedBytes);
    }

    public static RSAParameters ConvertFromJwk(JsonWebKey jwk, bool isPrivate = false) {
        RSAParameters rsaParameters = new RSAParameters
        {
            // PUBLIC KEY PARAMETERS
            // n parameter - public modulus
            Modulus = Base64UrlEncoder.DecodeBytes(jwk.N),
            // e parameter - public exponent
            Exponent = Base64UrlEncoder.DecodeBytes(jwk.E),
            
            // PRIVATE KEY PARAMETERS (optional)
            // d parameter - the private exponent value for the RSA key 
            D = Base64UrlEncoder.DecodeBytes(jwk.D),
            // dp parameter - CRT exponent of the first factor
            DP = Base64UrlEncoder.DecodeBytes(jwk.DP),
            // dq parameter - CRT exponent of the second factor
            DQ = Base64UrlEncoder.DecodeBytes(jwk.DQ),
            // p parameter - first prime factor
            P = Base64UrlEncoder.DecodeBytes(jwk.P),
            // q parameter - second prime factor
            Q = Base64UrlEncoder.DecodeBytes(jwk.Q),
            // qi parameter - CRT coefficient of the second factor
            InverseQ = Base64UrlEncoder.DecodeBytes(jwk.QI)
        };
        return rsaParameters;
    }

    public static SecurityKey GetSecurityKeyFromJwk(JsonWebKey jwk, bool isPrivate)
    {
        var parms = ConvertFromJwk(jwk, isPrivate);
        var rsaProvider = new RSACryptoServiceProvider();
        rsaProvider.ImportParameters(parms);
        SecurityKey key = new RsaSecurityKey(rsaProvider);
        return key;
    }

    public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
    {
        try
        {
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

            RSAalg.ImportParameters(Key);

            // Hash and sign the data. Pass a new instance of SHA256
            // to specify the hashing algorithm.
            return RSAalg.SignData(DataToSign, SHA256.Create());
        }
        catch(CryptographicException e)
        {
            Console.WriteLine(e.Message);

            return null;
        }
    }

    public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
    {
        try
        {
            // Create a new instance of RSACryptoServiceProvider using the
            // key from RSAParameters.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

            RSAalg.ImportParameters(Key);

            // Verify the data using the signature.  Pass a new instance of SHA256
            // to specify the hashing algorithm.
            return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
        }
        catch(CryptographicException e)
        {
            Console.WriteLine(e.Message);

            return false;
        }
    }

    public static CryptoKeyPair CreateKey()
    {
        using var rsa = new RSACryptoServiceProvider(2048);

        Console.WriteLine("Created key");
        //var privateKey = rsa.ExportPkcs8PrivateKey();
        //var privateKeyUtf8 = Encoding.UTF8.GetString(privateKey);
        //var privateKeyBase64 = Convert.ToBase64String(privateKey);

        //var publicKey = rsa.ExportRSAPublicKey();
        //var publicKeyUtf8 = Encoding.UTF8.GetString(publicKey);
        //var publicKeyBase64 = Convert.ToBase64String(publicKey);

        RsaSecurityKey privateKey = new(rsa.ExportParameters(true))
        {
            KeyId = "key1"
        };
        var privateJwk = JsonWebKeyConverter.ConvertFromSecurityKey(privateKey);

        RsaSecurityKey publicKey = new(rsa.ExportParameters(false))
        {
            KeyId = "key1"
        };
        var publicJwk = JsonWebKeyConverter.ConvertFromSecurityKey(publicKey);

        return new CryptoKeyPair(privateJwk, publicJwk);
    }
}