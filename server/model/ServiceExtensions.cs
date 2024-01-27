using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using net8auth.model.Tokens;

namespace net8auth.model;

public static class ServiceExtensions
{
    public static void AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // CryptoKeys cryptoKeys = new CryptoKeys(active, keys);
        services.Configure<CryptoKeys>(ck => {
            var section = configuration.GetSection("CryptoKeys");
            CryptoKeysConfig config = new();
            section.Bind(config);
            var keys = config.Keys.ToList();
            var active = keys.Find(k => k.Kid == config.ActiveKey) ?? throw new ApplicationException($"no active key found by name {config.ActiveKey}");
            keys.Remove(active);
            ck.Active = active;
            ck.Others = keys;
        });
    }

    
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddSingleton<GetCryptoKeys>();
    }
}