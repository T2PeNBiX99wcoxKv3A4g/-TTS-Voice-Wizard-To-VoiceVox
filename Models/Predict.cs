using System.Text.Json.Serialization;

namespace TTSToVOICEVOX.Models;

public class Predict
{
    [JsonPropertyName("speakerUuid")]
    public required string SpeakerUuid { get; set; }
    [JsonPropertyName("styleId")]
    public required int StyleId { get; set; }
    [JsonPropertyName("text")]
    public required string Text { get; set; }
    [JsonPropertyName("prosodyDetail")]
    public required List<List<EstimateProsodyDetail>> ProsodyDetail { get; set; }
    [JsonPropertyName("speedScale")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required decimal SpeedScale { get; set; }
}