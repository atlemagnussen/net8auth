using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using net8auth.model.Tokens;

namespace net8auth.model;

public static class ServiceExtensions
{
    public static void AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var sectionCryptoKey = configuration.GetSection("CryptoKey");
        services.Configure<CryptoKeyPair>(sectionCryptoKey);
    }

    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
    }
}