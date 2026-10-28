using System.ComponentModel.DataAnnotations;

namespace FlowBoard.Infrastructure.Security;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required] public string Issuer { get; init; } = "FlowBoard";
    [Required] public string Audience { get; init; } = "FlowBoard";

    /// <summary>HMAC-SHA256 signing key. Must be at least 32 characters.</summary>
    [Required, MinLength(32)] public string SigningKey { get; init; } = string.Empty;

    [Range(1, 24 * 60)] public int ExpiryMinutes { get; init; } = 60;
}
