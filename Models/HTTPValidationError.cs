using System.Text.Json.Serialization;

namespace TTSToVOICEVOX.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class HttpValidationError
{
    [JsonPropertyName("detail")]
    public required ValidationError[] Detail { get; init; }
}