using System.Text.Json;
using System.Threading.Tasks;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB 結果カーソルの実装
/// </summary>
internal sealed class ArcadeDBResultCursor : IArcadeDBResultCursor
{
    private readonly JsonDocument result;

    public ArcadeDBResultCursor(JsonDocument result)
    {
        this.result = result;
    }

    public Task<bool> FetchAsync()
    {
        // ArcadeDB のレスポンス形式:
        // { "result": [...] } または { "result": [] }

        if (this.result.RootElement.TryGetProperty("result", out var resultProperty))
        {
            // result配列が存在し、要素がある場合はtrue
            if (resultProperty.ValueKind == JsonValueKind.Array)
            {
                return Task.FromResult(resultProperty.GetArrayLength() > 0);
            }
        }

        return Task.FromResult(false);
    }
}
