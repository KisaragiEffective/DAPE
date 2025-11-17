using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB 非同期セッションの実装
/// </summary>
internal sealed class ArcadeDBAsyncSession : IArcadeDBAsyncSession
{
    private readonly HttpClient httpClient;
    private readonly string database;

    public ArcadeDBAsyncSession(HttpClient httpClient, string database)
    {
        this.httpClient = httpClient;
        this.database = database;
    }

    public async Task<T> ExecuteReadAsync<T>(Func<IArcadeDBAsyncTransaction, Task<T>> work)
    {
        var transaction = new ArcadeDBAsyncTransaction(this.httpClient, this.database);
        return await work(transaction);
    }

    public async Task<T> ExecuteWriteAsync<T>(Func<IArcadeDBAsyncTransaction, Task<T>> work)
    {
        var transaction = new ArcadeDBAsyncTransaction(this.httpClient, this.database);
        return await work(transaction);
    }

    public ValueTask DisposeAsync()
    {
        // セッション自体のリソース解放は不要（HttpClientは共有）
        return ValueTask.CompletedTask;
    }
}
