using KisaragiTech.Dape.User.Interface;
using KisaragiTech.Dape.User.Service;

namespace KisaragiTech.Dape.User.Model;

/// <summary>
/// 仮登録したローカルユーザー。仮ユーザーができることは本登録を完了させること、時間切れでキューから削除されること。
/// </summary>
public sealed class LocalTentativeUser : ILocalUser
{
    private readonly UserID _identifier;
    private readonly string _preferredHandle;

    /// <see cref="TentativeUserFactory.Create"/> を使うこと。
    /// 
    /// <param name="identifier"></param>
    /// <param name="preferredHandle"></param>
    internal LocalTentativeUser(UserID identifier, string preferredHandle)
    {
        _identifier = identifier;
        _preferredHandle = preferredHandle;
    }

    public UserID GetIdentifier()
    {
        return _identifier;
    }

    public string GetPreferredHandle()
    {
        return _preferredHandle;
    }

    public LocalRegisteredUser ToRegisteredUser(string rawPassword)
    {
        return new LocalRegisteredUser(_identifier, _preferredHandle, PasswordHasher.CreateHashedPassword(rawPassword));
    }
}
