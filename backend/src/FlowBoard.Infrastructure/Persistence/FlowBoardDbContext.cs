using FlowBoard.Application.Common.Abstractions;
using FlowBoard.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FlowBoard.Infrastructure.Persistence;

public sealed class FlowBoardDbContext(DbContextOptions<FlowBoardDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlowBoardDbContext).Assembly);
    }

    async Task IUnitOfWork.SaveChangesAsync(CancellationToken ct) => await SaveChangesAsync(ct);
}
