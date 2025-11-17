using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB ドライバーの実装
/// </summary>
internal sealed class ArcadeDBDriver : IArcadeDBDriver
{
    private readonly HttpClient httpClient;
    private readonly string database;
    private bool disposed;

    /// <summary>
    /// ArcadeDB ドライバーを初期化
    /// </summary>
    /// <param name="baseUrl">ArcadeDB サーバーのベースURL (例: http://localhost:2480)</param>
    /// <param name="database">データベース名</param>
    /// <param name="username">ユーザー名</param>
    /// <param name="password">パスワード</param>
    public ArcadeDBDriver(string baseUrl, string database, string username, string password)
    {
        this.database = database;
        this.httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        // Basic認証の設定
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    }

    public IArcadeDBAsyncSession AsyncSession()
    {
        return new ArcadeDBAsyncSession(this.httpClient, this.database);
    }

    public void Dispose()
    {
        if (!this.disposed)
        {
            this.httpClient.Dispose();
            this.disposed = true;
        }
    }
}
