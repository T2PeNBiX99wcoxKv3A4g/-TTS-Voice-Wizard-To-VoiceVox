using System.Text.Json.Serialization;

namespace TTSToVOICEVOX.Models;

public class ErrorDetails
{
    [JsonPropertyName("detail")]
    public required Error[] Detail { get; init; }
}