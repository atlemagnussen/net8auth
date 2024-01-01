using Microsoft.IdentityModel.Tokens;

public class SecurityKeyInfo
{
    public SecurityKeyInfo(SecurityKey key, string algorithm)
    {
        Key = key;
        SigningAlgorithm = algorithm;
    }

    public SecurityKey Key { get; init; }

    public string SigningAlgorithm { get; init; }
}