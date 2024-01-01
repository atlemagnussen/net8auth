using System.Security.Cryptography;
using System.Text;

namespace net8auth.consoleApp;

public static class SignService
{
    public static string? SignData(string message, CryptoKeyPair keyPair)
    {
        //// The array to store the signed message in bytes
        byte[] signedBytes;
        using (var rsa = new RSACryptoServiceProvider())
        {
            //// Write the message to a byte array using UTF8 as the encoding.
            var encoder = new UTF8Encoding();
            byte[] originalData = encoder.GetBytes(message);

            try
            {
                //// Import the private key used for signing the message
                // rsa.ImportParameters(privateKey);
                ReadOnlySpan<byte> privateKey = Convert.FromBase64String(keyPair.PrivateKey).AsSpan();
                var intRead = 0;
                rsa.ImportPkcs8PrivateKey(privateKey, out intRead);

                //// Sign the data, using SHA512 as the hashing algorithm 
                signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                //// Set the keycontainer to be cleared when rsa is garbage collected.
                rsa.PersistKeyInCsp = false;
            }
        }
        //// Convert the a base64 string before returning
        return Convert.ToBase64String(signedBytes);
    }

    public static void CreateKey()
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            Console.WriteLine("Created key");
            var privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            var privateKeyBase64 = Convert.ToBase64String(privateKeyBytes);
            Console.WriteLine("Private key:");
            Console.WriteLine(privateKeyBase64);

            var publicKey = rsa.ExportRSAPublicKey();
            var publicKeyBase64 = Convert.ToBase64String(publicKey);
            Console.WriteLine("Public key:");
            Console.WriteLine(publicKeyBase64);
        }
    }
}