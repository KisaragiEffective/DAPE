using System.Text.Json;
using System.Text.Json.Serialization;

namespace KisaragiTech.Dape.Config;

public class RootConfig
{
    [JsonPropertyName("database")] public required DatabaseConfig Database { get; init; }

    public static RootConfig DeserializeFromJson(string json)
    {
        return JsonSerializer.Deserialize<RootConfig>(json, new JsonSerializerOptions() { IgnoreReadOnlyFields = false, IgnoreReadOnlyProperties = false, UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow })!;
    }
}
