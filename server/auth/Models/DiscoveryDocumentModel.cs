using System.Text.Json.Serialization;

namespace net8auth.auth.Models;

public record DiscoveryDocumentModel
{
    [JsonPropertyName("issuer")]
    public string Issuer { get; init; } = string.Empty;

    [JsonPropertyName("jwks_uri")]
    public string JwksUri { get; init; } = string.Empty;

    [JsonPropertyName("authorization_endpoint")]
    public string AuthorizationEndpoint { get; init; } = string.Empty;

    [JsonPropertyName("token_endpoint")]
    public string TokenEndpoint { get; init; } = string.Empty;

    [JsonPropertyName("userinfo_endpoint")]
    public string UserinfoEndpoint { get; init; } = string.Empty;
}