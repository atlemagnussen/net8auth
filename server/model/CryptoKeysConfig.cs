using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace net8auth.model;

public record CryptoKeysConfig
{
    public string ActiveKey { get; init; } = "key1";

    public IEnumerable<JsonWebKey> Keys { get; init; } = [];
}

public record CryptoKeys {
    public CryptoKeys(JsonWebKey active, List<JsonWebKey> others)
    {
        Active = active;
        Others = others;
    }
    public JsonWebKey Active { get; init; }
    public List<JsonWebKey> Others { get; init; }

    public override string ToString()
    {
        var activeStr = JsonSerializer.Serialize(Active);
        var othersStr = JsonSerializer.Serialize(Others);

        return $"active: {activeStr}, others: {othersStr}";
    }
}

public class GetCryptoKeys
{
    private readonly CryptoKeysConfig _config;
    public GetCryptoKeys(IConfiguration configuration)
    {
        var section = configuration.GetSection("CryptoKeys");
        CryptoKeysConfig config = new();
        section.Bind(config);

        if (config == null)
            throw new ApplicationException("missing keys");
        _config = config;
    }
    public CryptoKeys FromConfig()
    {
        var keys = _config.Keys.ToList();
        var active = keys.Find(k => k.Kid == _config.ActiveKey);
        if (active == null)
            throw new ApplicationException($"no active key found by name {_config.ActiveKey}");
        
        keys.Remove(active);
        return new CryptoKeys(active, keys);
    }
}