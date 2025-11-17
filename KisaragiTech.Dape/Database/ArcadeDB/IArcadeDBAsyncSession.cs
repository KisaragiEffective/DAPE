using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB 非同期セッションのインターフェース
/// </summary>
internal interface IArcadeDBAsyncSession : IAsyncDisposable
{
    /// <summary>
    /// 読み取りトランザクションを実行
    /// </summary>
    /// <typeparam name="T">戻り値の型</typeparam>
    /// <param name="work">実行する処理</param>
    /// <returns>処理結果</returns>
    Task<T> ExecuteReadAsync<T>(Func<IArcadeDBAsyncTransaction, Task<T>> work);

    /// <summary>
    /// 書き込みトランザクションを実行
    /// </summary>
    /// <typeparam name="T">戻り値の型</typeparam>
    /// <param name="work">実行する処理</param>
    /// <returns>処理結果</returns>
    Task<T> ExecuteWriteAsync<T>(Func<IArcadeDBAsyncTransaction, Task<T>> work);
}
