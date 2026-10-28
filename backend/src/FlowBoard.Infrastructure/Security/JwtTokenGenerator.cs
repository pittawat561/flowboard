using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlowBoard.Application.Common.Abstractions;
using FlowBoard.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FlowBoard.Infrastructure.Security;

internal sealed class JwtTokenGenerator(IOptions<JwtOptions> options, TimeProvider clock) : ITokenGenerator
{
    private readonly JwtOptions _options = options.Value;

    public AccessToken Generate(User user)
    {
        var now = clock.GetUtcNow();
        var expiresAt = now.AddMinutes(_options.ExpiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.DisplayName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
