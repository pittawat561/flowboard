using FlowBoard.Domain.Users;

namespace FlowBoard.Application.Common.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IPasswordHasher
{
    string Hash(User user, string password);
    bool Verify(User user, string password);
}

public interface ITokenGenerator
{
    AccessToken Generate(User user);
}

public sealed record AccessToken(string Value, DateTimeOffset ExpiresAt);
