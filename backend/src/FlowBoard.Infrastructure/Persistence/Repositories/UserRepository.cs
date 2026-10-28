using FlowBoard.Application.Common.Abstractions;
using FlowBoard.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FlowBoard.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(FlowBoardDbContext db) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken ct = default) =>
        db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail, ct);

    public Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken ct = default) =>
        db.Users.AnyAsync(u => u.Email == normalizedEmail, ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await db.Users.AddAsync(user, ct);
}
