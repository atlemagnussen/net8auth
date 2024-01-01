namespace net8auth.consoleApp;

public record CryptoKeyPair
{
    public string PrivateKey { get; init; } = string.Empty;
    public string PublicKey { get; init; } = string.Empty;
}