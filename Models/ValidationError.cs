using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TTSToVOICEVOX.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class ValidationError
{
    [JsonPropertyName("loc")]
    public required object[] Loc { get; init; }
    [JsonPropertyName("msg")]
    public required string Msg { get; init; }
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}