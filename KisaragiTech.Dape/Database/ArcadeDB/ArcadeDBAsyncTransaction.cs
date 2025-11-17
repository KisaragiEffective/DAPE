using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KisaragiTech.Dape.Database.ArcadeDB;

/// <summary>
/// ArcadeDB 非同期トランザクションの実装
/// </summary>
internal sealed class ArcadeDBAsyncTransaction : IArcadeDBAsyncTransaction
{
    private readonly HttpClient httpClient;
    private readonly string database;

    public ArcadeDBAsyncTransaction(HttpClient httpClient, string database)
    {
        this.httpClient = httpClient;
        this.database = database;
    }

    public async Task<IArcadeDBResultCursor> RunAsync(string query, object? parameters = null)
    {
        // ArcadeDB の HTTP API エンドポイント: POST /api/v1/command/{database}
        var endpoint = $"/api/v1/command/{this.database}";

        // パラメータを展開したクエリを作成
        var expandedQuery = this.ExpandParameters(query, parameters);

        // リクエストボディを作成
        var requestBody = new
        {
            language = "cypher",
            command = expandedQuery
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // HTTPリクエストを送信
        var response = await this.httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        // レスポンスを解析
        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonDocument.Parse(responseJson);

        return new ArcadeDBResultCursor(result);
    }

    /// <summary>
    /// クエリパラメータを展開
    /// </summary>
    private string ExpandParameters(string query, object? parameters)
    {
        if (parameters == null)
        {
            return query;
        }

        var result = query;
        var properties = parameters.GetType().GetProperties();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(parameters);
            var placeholder = $"${prop.Name}";

            // 値の型に応じて適切にエスケープ
            string escapedValue;
            if (value is string strValue)
            {
                // 文字列の場合はクォートでエスケープ
                escapedValue = $"'{strValue.Replace("'", "\\'")}'";
            }
            else if (value is bool boolValue)
            {
                escapedValue = boolValue ? "true" : "false";
            }
            else
            {
                escapedValue = value?.ToString() ?? "null";
            }

            result = result.Replace(placeholder, escapedValue);
        }

        return result;
    }
}
