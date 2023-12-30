using net8auth.auth.Models;

namespace net8auth.auth.Services;

public class WellKnownService
{
    public DiscoveryDocumentModel Get(HttpRequest request)
    {
        var thisBaseUrl = GetBaseUrlHost(request);
        var disco = new DiscoveryDocumentModel
        {
            Issuer = thisBaseUrl
        };
        return disco;
    }


    private string GetBaseUrlHost(HttpRequest request)
    {
        return $"{request.Scheme}://{request.Host.Value}";
    }
}