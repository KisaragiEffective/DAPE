using KisaragiTech.Dape.User.Interface;
using KisaragiTech.Dape.User.Service;

namespace KisaragiTech.Dape.User.Model;

/// <summary>
/// 本登録したローカルユーザー。必要な権限を持っていれば各種操作が行えるようになる。
/// </summary>
/// <param name="identifier"></param>
/// <param name="preferredHandle"></param>
public sealed class LocalRegisteredUser(UserID identifier, string preferredHandle, HashedPassword hashedPassword) : ILocalUser
{
    public UserID GetIdentifier()
    {
        return identifier;
    }

    public string GetPreferredHandle()
    {
        return preferredHandle;
    }

    public bool VerifyPassword(string raw)
    {
        return hashedPassword.Equals(PasswordHasher.CreateHashedPassword(raw));
    }
}