using KisaragiTech.Dape.User.Service;

namespace KisaragiTech.Dape.Test.User.Service;

public class PasswordHasherTests
{
    [Fact]
    private void IdentityCase()
    {
        var password = PasswordHasher.CreateHashedPassword("Hello, world!");
        var isCorrect = password.Verify("Hello, world!");
        Assert.True(isCorrect);
    }
}
