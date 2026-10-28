using FlowBoard.Application.Common.Abstractions;
using FlowBoard.Application.Common.Exceptions;
using FlowBoard.Domain.Users;

namespace FlowBoard.Application.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken ct = default);
}

public sealed class AuthService(
    IUserRepository users,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var user = User.Create(request.Email, request.DisplayName);

        if (await users.EmailExistsAsync(user.Email, ct))
            throw new ConflictException("An account with this email already exists.");

        user.SetPasswordHash(passwordHasher.Hash(user, request.Password));

        await users.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return IssueToken(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await users.GetByEmailAsync(User.NormalizeEmail(request.Email), ct);

        // Same error for unknown email and wrong password, so accounts cannot be enumerated.
        if (user is null || !passwordHasher.Verify(user, request.Password))
            throw new InvalidCredentialsException();

        return IssueToken(user);
    }

    public async Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("User not found.");

        return UserDto.From(user);
    }

    private AuthResponse IssueToken(User user)
    {
        var token = tokenGenerator.Generate(user);
        return new AuthResponse(token.Value, token.ExpiresAt, UserDto.From(user));
    }
}
