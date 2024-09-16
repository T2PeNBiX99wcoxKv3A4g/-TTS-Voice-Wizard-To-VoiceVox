using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TTSToVOICEVOX.Models;

public class MessageData
{
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}