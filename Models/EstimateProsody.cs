using System.Text.Json.Serialization;
// ReSharper disable ClassNeverInstantiated.Global

namespace TTSToVOICEVOX.Models;

public class EstimateProsody
{
    [JsonPropertyName("plain")]
    public required string[] Plain { get; set; }
    [JsonPropertyName("detail")]
    public required List<List<EstimateProsodyDetail>> Detail { get; set; }
}