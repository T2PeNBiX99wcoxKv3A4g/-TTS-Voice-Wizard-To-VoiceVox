using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TTSToVOICEVOX.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class SimpleError
{
    [JsonPropertyName("detail")]
    public required string Detail { get; init; }
}