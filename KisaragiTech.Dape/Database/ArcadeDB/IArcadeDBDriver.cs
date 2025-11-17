using System;
using System.Threading.Tasks;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB ドライバーのインターフェース
/// </summary>
internal interface IArcadeDBDriver : IDisposable
{
    /// <summary>
    /// 非同期セッションを作成
    /// </summary>
    /// <returns>非同期セッション</returns>
    IArcadeDBAsyncSession AsyncSession();
}
