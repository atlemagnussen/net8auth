using System.Security.Claims;

namespace net8auth.model.Tokens;

public interface ITokenService
{
    Task<string> CreateAndSignJwt(ClaimsPrincipal user);
}