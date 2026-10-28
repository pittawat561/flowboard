using FlowBoard.Domain.Common;
using FlowBoard.Domain.Users;

namespace FlowBoard.UnitTests.Domain;

public sealed class UserTests
{
    [Fact]
    public void Create_NormalizesEmailAndTrimsName()
    {
        var user = User.Create("  Alice@Example.COM ", "  Alice  ");

        Assert.Equal("alice@example.com", user.Email);
        Assert.Equal("Alice", user.DisplayName);
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void Create_RejectsInvalidEmail(string email)
    {
        Assert.Throws<DomainException>(() => User.Create(email, "Alice"));
    }

    [Fact]
    public void Rename_RejectsTooLongName()
    {
        var user = User.Create("a@b.com", "Alice");

        Assert.Throws<DomainException>(() => user.Rename(new string('x', User.DisplayNameMaxLength + 1)));
    }
}
