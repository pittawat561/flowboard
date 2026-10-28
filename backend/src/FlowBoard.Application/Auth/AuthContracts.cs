using System.ComponentModel.DataAnnotations;
using FlowBoard.Domain.Users;

namespace FlowBoard.Application.Auth;

public sealed record RegisterRequest(
    [Required, EmailAddress, MaxLength(User.EmailMaxLength)] string Email,
    [Required, MaxLength(User.DisplayNameMaxLength)] string DisplayName,
    [Required, MinLength(8), MaxLength(128)] string Password);

public sealed record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public sealed record UserDto(Guid Id, string Email, string DisplayName)
{
    public static UserDto From(User user) => new(user.Id, user.Email, user.DisplayName);
}

public sealed record AuthResponse(string AccessToken, DateTimeOffset ExpiresAt, UserDto User);
