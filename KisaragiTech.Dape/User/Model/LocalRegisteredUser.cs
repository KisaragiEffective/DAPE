using KisaragiTech.Dape.User.Interface;

namespace KisaragiTech.Dape.User.Model;

/// <summary>
/// 本登録したローカルユーザー。必要な権限を持っていれば各種操作が行えるようになる。
/// </summary>
/// <param name="Identifier"></param>
/// <param name="PreferredHandle"></param>
public class LocalRegisteredUser(UserID Identifier, string preferredHandle) : IUser
{
    public UserID GetIdentifier()
    {
        return Identifier;
    }

    public string GetPreferredHandle()
    {
        return preferredHandle;
    }
}