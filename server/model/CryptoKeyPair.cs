using Microsoft.IdentityModel.Tokens;

namespace net8auth.model;

public record CryptoKeyPair
{
    public CryptoKeyPair()
    {
    }
    public CryptoKeyPair(JsonWebKey privateKey, JsonWebKey publicKey)
    {
        PrivateKey = privateKey;
        PublicKey = publicKey;
    }
    public JsonWebKey? PrivateKey { get; init; }
    public JsonWebKey? PublicKey { get; init; }
}