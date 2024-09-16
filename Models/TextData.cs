using System.Text.Json.Serialization;

namespace TTSToVOICEVOX.Models;

public class TextData
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }
}