namespace Helpful.Api.Auth;

using System.Security.Claims;

public static class IdentityExtensions
{
    public static ClaimsPrincipal ToClaimsPrincipal(this ClaimsIdentity identity)
    {
        return new ClaimsPrincipal(identity);
    }
}