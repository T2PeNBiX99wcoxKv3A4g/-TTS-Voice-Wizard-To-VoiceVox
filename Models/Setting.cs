using System.Text.Json.Serialization;

namespace TTSToVOICEVOX.Models;

public class Setting
{
    [JsonPropertyName("speaker_id")] public required int SpeakerId { get; init; }
    [JsonPropertyName("speaker_mode")] public required int SpeakerMode { get; init; }
    [JsonPropertyName("speaker_uuid")] public required string SpeakerUuid { get; init; }
    [JsonPropertyName("speaker_style_id")] public required int SpeakerStyleId { get; init; }
}