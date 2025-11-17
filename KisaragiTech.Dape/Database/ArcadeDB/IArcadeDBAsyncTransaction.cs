using System.Collections.Generic;
using System.Threading.Tasks;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB 非同期トランザクションのインターフェース
/// </summary>
internal interface IArcadeDBAsyncTransaction
{
    /// <summary>
    /// クエリを実行
    /// </summary>
    /// <param name="query">実行するクエリ (Cypher または SQL)</param>
    /// <param name="parameters">クエリパラメータ</param>
    /// <returns>結果カーソル</returns>
    Task<IArcadeDBResultCursor> RunAsync(string query, object? parameters = null);
}
