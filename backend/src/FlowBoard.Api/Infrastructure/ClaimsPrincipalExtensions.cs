using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FlowBoard.Api.Infrastructure;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(value, out var id)
            ? id
            : throw new InvalidOperationException("Authenticated principal has no valid 'sub' claim.");
    }
}
