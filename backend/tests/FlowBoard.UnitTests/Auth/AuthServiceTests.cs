using FlowBoard.Application.Auth;
using FlowBoard.Application.Common.Abstractions;
using FlowBoard.Application.Common.Exceptions;
using FlowBoard.Domain.Users;
using FlowBoard.Infrastructure.Security;

namespace FlowBoard.UnitTests.Auth;

public sealed class AuthServiceTests
{
    private readonly FakeUserRepository _users = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_users, _users, new PasswordHasher(), new FakeTokenGenerator());
    }

    [Fact]
    public async Task Register_CreatesUserWithHashedPassword()
    {
        var result = await _sut.RegisterAsync(new RegisterRequest("bob@example.com", "Bob", "Passw0rd!"));

        var stored = Assert.Single(_users.Items);
        Assert.Equal("bob@example.com", result.User.Email);
        Assert.NotEqual("Passw0rd!", stored.PasswordHash);
        Assert.Equal(1, _users.SaveCount);
        Assert.Equal($"token-{stored.Id}", result.AccessToken);
    }

    [Fact]
    public async Task Register_RejectsDuplicateEmailCaseInsensitive()
    {
        await _sut.RegisterAsync(new RegisterRequest("bob@example.com", "Bob", "Passw0rd!"));

        await Assert.ThrowsAsync<ConflictException>(() =>
            _sut.RegisterAsync(new RegisterRequest("BOB@example.com", "Bob 2", "Passw0rd!")));
    }

    [Fact]
    public async Task Login_SucceedsWithCorrectPassword()
    {
        await _sut.RegisterAsync(new RegisterRequest("bob@example.com", "Bob", "Passw0rd!"));

        var result = await _sut.LoginAsync(new LoginRequest("Bob@Example.com", "Passw0rd!"));

        Assert.Equal("Bob", result.User.DisplayName);
    }

    [Theory]
    [InlineData("bob@example.com", "wrong-password")]
    [InlineData("nobody@example.com", "Passw0rd!")]
    public async Task Login_FailsWithSameErrorForWrongPasswordOrUnknownEmail(string email, string password)
    {
        await _sut.RegisterAsync(new RegisterRequest("bob@example.com", "Bob", "Passw0rd!"));

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _sut.LoginAsync(new LoginRequest(email, password)));
    }

    [Fact]
    public async Task GetCurrentUser_ThrowsWhenMissing()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetCurrentUserAsync(Guid.NewGuid()));
    }

    private sealed class FakeUserRepository : IUserRepository, IUnitOfWork
    {
        public List<User> Items { get; } = [];
        public int SaveCount { get; private set; }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            Task.FromResult(Items.FirstOrDefault(u => u.Id == id));

        public Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken ct = default) =>
            Task.FromResult(Items.FirstOrDefault(u => u.Email == normalizedEmail));

        public Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken ct = default) =>
            Task.FromResult(Items.Any(u => u.Email == normalizedEmail));

        public Task AddAsync(User user, CancellationToken ct = default)
        {
            Items.Add(user);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            SaveCount++;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeTokenGenerator : ITokenGenerator
    {
        public AccessToken Generate(User user) => new($"token-{user.Id}", DateTimeOffset.UtcNow.AddHours(1));
    }
}
