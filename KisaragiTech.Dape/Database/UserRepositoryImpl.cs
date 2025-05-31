using System.Threading.Tasks;
using Neo4j.Driver;

namespace KisaragiTech.Dape.Database;

internal sealed class UserRepositoryImpl
{
    private readonly IDriver driver;
    
    internal UserRepositoryImpl(IDriver driver)
    {
        this.driver = driver;
    }
    
    public async Task<bool> HasRootUser()
    {
        await using var session = driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
        
        return await session.ExecuteReadAsync(async tx =>
        {
            // TODO: ORMてきなメソッドの方に置き換えたほうがいいかも
            const string query = "MATCH (u:User { root: true }) RETURN u LIMIT 1";
            var cursor = await tx.RunAsync(query);
            return await cursor.FetchAsync();
        });
    }
}