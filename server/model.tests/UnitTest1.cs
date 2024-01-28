using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using net8auth.model.Tokens;

namespace net8auth.model.tests;

public class UnitTest1
{
    [Fact]
    public void Test()
    {
        var key = CreateKeys.CreateEcKey();
        ModelServiceExtensions.SetCorrectAlgorithmForEcKey(key);

        IOptions<CryptoKeys> config = Options.Create(new CryptoKeys(key, []));

        var tokenService = new TokenService(config);

        var jwt = tokenService.CreateAndSignJwt(new ClaimsPrincipal());
        Assert.True(jwt != null);
    }
}