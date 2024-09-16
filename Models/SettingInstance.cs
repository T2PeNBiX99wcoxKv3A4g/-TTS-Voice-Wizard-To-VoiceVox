namespace TTSToVOICEVOX.Models;

public class SettingInstance
{
    public required int SpeakerId { get; set; }
    public required int SpeakerMode { get; set; }
    public required string SpeakerUuid { get; set; }
    public required int SpeakerStyleId { get; set; }
}