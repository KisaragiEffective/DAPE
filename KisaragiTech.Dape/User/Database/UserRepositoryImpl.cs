using System;
using System.Threading.Tasks;
using KisaragiTech.Dape.Database.ArcadeDB;
using KisaragiTech.Dape.User.Interface;
using KisaragiTech.Dape.User.Model;

namespace KisaragiTech.Dape.User.Database;

internal sealed class UserRepositoryImpl
{
    private readonly IArcadeDBDriver driver;

    internal UserRepositoryImpl(IArcadeDBDriver driver)
    {
        this.driver = driver;
    }

    public async Task<bool> HasRootUser()
    {
        await using var session = this.driver.AsyncSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            // language=cypher
            const string query = "MATCH (u:User { root: true }) RETURN u LIMIT 1";
            var cursor = await tx.RunAsync(query);
            return await cursor.FetchAsync();
        });
    }

    public async Task<bool> FindUserByPreferredHandle(string preferredHandle)
    {
        await using var session = this.driver.AsyncSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            // language=cypher
            const string query = "MATCH (n:Person { handle: $handle }) RETURN true AS exists LIMIT 1";
            var cursor = await tx.RunAsync(query, new { handle = preferredHandle });

            return await cursor.FetchAsync();
        });
    }

    private async Task AssertPreferredHandleUniqueness(IUser user)
    {
        if (await this.FindUserByPreferredHandle(user.GetPreferredHandle()))
        {
            throw new ArgumentException($"すでに {user.GetPreferredHandle()} は登録されています。");
        }
    }

    public async Task InsertUser(LocalRegisteredUser user)
    {
        await this.AssertPreferredHandleUniqueness(user);

        await using var session = this.driver.AsyncSession();

        await session.ExecuteWriteAsync(tx =>
        {
            // language=cypher
            const string query = "CREATE (n:Person { handle: $handle, id: $id })";
            var parameters = new { handle = user.GetPreferredHandle(), id = user.GetIdentifier().Raw.ToString() };

            return tx.RunAsync(query, parameters);
        });
    }

    public async Task CreateRootUser(LocalRegisteredUser user)
    {
        if (await this.HasRootUser())
        {
            throw new InvalidOperationException("ルートユーザーはすでに存在します");
        }

        await this.InsertUser(user);
    }
}
