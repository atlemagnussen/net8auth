using net8auth.auth.Models;

namespace net8auth.auth.Services;

public class WellKnownService
{
    public DiscoveryDocumentModel Get(HttpRequest request)
    {
        var thisBaseUrl = GetBaseUrlHost(request);
        var disco = new DiscoveryDocumentModel
        {
            Issuer = thisBaseUrl,
            JwksUri = $"{thisBaseUrl}/{OpenIdJwks}",
            AuthorizationEndpoint = $"{thisBaseUrl}/{ConnectEndpoint.Base}/{ConnectEndpoint.Authorize}",
            TokenEndpoint = $"{thisBaseUrl}/{ConnectEndpoint.Base}/{ConnectEndpoint.Token}",
            UserinfoEndpoint = $"{thisBaseUrl}/{ConnectEndpoint.Base}/{ConnectEndpoint.UserInfo}",
        };
        return disco;
    }


    private string GetBaseUrlHost(HttpRequest request)
    {
        return $"{request.Scheme}://{request.Host.Value}";
    }

    public static string OpenIdJwks = ".well-known/openid-configuration/jwks";
}
