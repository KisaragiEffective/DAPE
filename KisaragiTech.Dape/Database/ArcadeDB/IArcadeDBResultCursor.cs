using System.Threading.Tasks;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB 結果カーソルのインターフェース
/// </summary>
internal interface IArcadeDBResultCursor
{
    /// <summary>
    /// 次の結果を取得
    /// </summary>
    /// <returns>結果が存在する場合は true</returns>
    Task<bool> FetchAsync();
}
