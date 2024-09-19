namespace TTSToVOICEVOX.Configs;

public class ApiConfig
{
    public const string Name = "TTS_API";

    public string VoiceVoxUrl { get; init; } = string.Empty;
    public string CoeiroLinkUrl { get; init; } = string.Empty;
    public int Retry { get; init; } = -1;
    public int RetryWaitingTime { get; init; } = -1;
    public int SpeakerId { get; init; } = -1;
    public int SpeakerMode { get; init; } = -1;
    public string SpeakerUuid { get; init; } = string.Empty;
    public int SpeakerStyleId { get; init; } = -1;
    public Dictionary<string, string> TextReplace { get; init; } = new();
}