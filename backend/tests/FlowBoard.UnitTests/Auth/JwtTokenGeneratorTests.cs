using System.IdentityModel.Tokens.Jwt;
using FlowBoard.Domain.Users;
using FlowBoard.Infrastructure.Security;
using Microsoft.Extensions.Options;

namespace FlowBoard.UnitTests.Auth;

public sealed class JwtTokenGeneratorTests
{
    [Fact]
    public void Generate_EmbedsUserClaimsAndExpiry()
    {
        var options = Options.Create(new JwtOptions
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SigningKey = new string('k', 32),
            ExpiryMinutes = 15,
        });
        var sut = new JwtTokenGenerator(options, TimeProvider.System);
        var user = User.Create("carol@example.com", "Carol");

        var token = sut.Generate(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token.Value);

        Assert.Equal(user.Id.ToString(), jwt.Subject);
        Assert.Equal("test-issuer", jwt.Issuer);
        Assert.Contains("test-audience", jwt.Audiences);
        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "carol@example.com");
        Assert.InRange(token.ExpiresAt - DateTimeOffset.UtcNow, TimeSpan.FromMinutes(14), TimeSpan.FromMinutes(15));
    }
}
