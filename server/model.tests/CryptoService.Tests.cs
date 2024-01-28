using System.Text.Json;

namespace net8auth.model.tests;

public class CryptoServiceTests
{
    [Fact]
    public void Test_Sign_Jwt_With_EcKey()
    {
        var key = CreateKeys.CreateEcKey();
        ModelServiceExtensions.SetCorrectAlgorithmForEcKey(key);

        var jwtUnsigned = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhdGxlbWFnbnVzc2VuQGdtYWlsLmNvbSIsIm5hbWUiOiJhdGxlIiwicm9sZSI6IkFkbWluIiwiaXNzIjoidGVzdCIsImV4cCI6MTcwNDA5ODM5NywiaWF0IjoxNzA0MTg0Nzk3LCJuYmYiOjE3MDQxODQ3OTd9";

        string signature = CryptoService.SignJwk(jwtUnsigned, key)!;

        //string jwtSigned = $"{jwtUnsigned}.{signature}";
        // jwtUnsigned = $"{jwtUnsigned}append"; append to jwt to see it fail the verification
        var verified = CryptoService.Verify(jwtUnsigned, signature, key);

        Assert.True(verified);
    }

    [Fact]
    public void Test_Sign_Jwt_With_RsaKey()
    {
        var key = CreateKeys.CreateRsaKey();
        // Console.WriteLine(JsonSerializer.Serialize(key));

        var jwtUnsigned = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhdGxlbWFnbnVzc2VuQGdtYWlsLmNvbSIsIm5hbWUiOiJhdGxlIiwicm9sZSI6IkFkbWluIiwiaXNzIjoidGVzdCIsImV4cCI6MTcwNDA5ODM5NywiaWF0IjoxNzA0MTg0Nzk3LCJuYmYiOjE3MDQxODQ3OTd9";

        string signature = CryptoService.SignJwk(jwtUnsigned, key)!;
        // Console.WriteLine(signature);
        //string jwtSigned = $"{jwtUnsigned}.{signature}";
        // jwtUnsigned = $"{jwtUnsigned}append"; append to jwt to see it fail the verification
        var verified = CryptoService.Verify(jwtUnsigned, signature, key);

        Assert.True(verified);
    }
}