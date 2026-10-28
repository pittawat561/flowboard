using FlowBoard.Domain.Users;
using AspNetHasher = Microsoft.AspNetCore.Identity.PasswordHasher<FlowBoard.Domain.Users.User>;
using AspNetResult = Microsoft.AspNetCore.Identity.PasswordVerificationResult;

namespace FlowBoard.Infrastructure.Security;

/// <summary>Adapter over ASP.NET Core Identity's PBKDF2 hasher.</summary>
internal sealed class PasswordHasher : Application.Common.Abstractions.IPasswordHasher
{
    private readonly AspNetHasher _inner = new();

    public string Hash(User user, string password) => _inner.HashPassword(user, password);

    public bool Verify(User user, string password) =>
        _inner.VerifyHashedPassword(user, user.PasswordHash, password) != AspNetResult.Failed;
}
