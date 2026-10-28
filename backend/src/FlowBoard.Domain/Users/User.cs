using System.Net.Mail;
using FlowBoard.Domain.Common;

namespace FlowBoard.Domain.Users;

public sealed class User : Entity
{
    public const int DisplayNameMaxLength = 64;
    public const int EmailMaxLength = 256;

    public string Email { get; private set; } = null!;
    public string DisplayName { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;

    // Required by EF Core.
    private User() { }

    private User(string email, string displayName)
    {
        Email = NormalizeEmail(email);
        Rename(displayName);
    }

    public static User Create(string email, string displayName) => new(email, displayName);

    public void Rename(string displayName)
    {
        var trimmed = displayName?.Trim() ?? string.Empty;
        if (trimmed.Length == 0 || trimmed.Length > DisplayNameMaxLength)
            throw new DomainException($"Display name must be 1-{DisplayNameMaxLength} characters.");

        DisplayName = trimmed;
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash must not be empty.");

        PasswordHash = passwordHash;
    }

    public static string NormalizeEmail(string email)
    {
        var trimmed = email?.Trim().ToLowerInvariant() ?? string.Empty;
        if (trimmed.Length == 0 || trimmed.Length > EmailMaxLength || !MailAddress.TryCreate(trimmed, out _))
            throw new DomainException("Email address is not valid.");

        return trimmed;
    }
}
