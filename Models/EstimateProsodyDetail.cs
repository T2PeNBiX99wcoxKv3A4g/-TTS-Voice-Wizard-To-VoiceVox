using System.Text.Json.Serialization;
// ReSharper disable ClassNeverInstantiated.Global

namespace TTSToVOICEVOX.Models;

public class EstimateProsodyDetail
{
    [JsonPropertyName("phoneme")]
    public required string Phoneme { get; set; }
    [JsonPropertyName("hira")]
    public required string Hira { get; set; }
    [JsonPropertyName("accent")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required decimal Accent { get; set; }
}