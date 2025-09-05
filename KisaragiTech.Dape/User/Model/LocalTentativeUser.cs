using KisaragiTech.Dape.User.Interface;
using KisaragiTech.Dape.User.Service;

namespace KisaragiTech.Dape.User.Model;

/// <summary>
/// 仮登録したローカルユーザー。仮ユーザーができることは本登録を完了させること、時間切れでキューから削除されること。
/// </summary>
public sealed class LocalTentativeUser : ILocalUser
{
    private readonly UserID identifier;
    private readonly string preferredHandle;

    /// <see cref="TentativeUserFactory.Create"/> を使うこと。
    ///
    /// <param name="identifier"></param>
    /// <param name="preferredHandle"></param>
    internal LocalTentativeUser(UserID identifier, string preferredHandle)
    {
        this.identifier = identifier;
        this.preferredHandle = preferredHandle;
    }

    public UserID GetIdentifier() => this.identifier;

    public string GetPreferredHandle() => this.preferredHandle;

    public LocalRegisteredUser ToRegisteredUser(HashedPassword hashedPassword) => new(this.identifier, this.preferredHandle, hashedPassword);
}
