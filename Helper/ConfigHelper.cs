using Microsoft.Extensions.Options;
using TTSToVOICEVOX.Configs;
using TTSToVOICEVOX.Models;

namespace TTSToVOICEVOX.Helper;

public class ConfigHelper
{
    private static ConfigHelper? _instance;

    public static ConfigHelper Instance
    {
        get
        {
            _instance ??= new ConfigHelper();
            return _instance;
        }
    }

    private SettingInstance? _settingInstance;

    private IOptions<ApiConfig>? _config;

    public SettingInstance Settings
    {
        get
        {
            _settingInstance ??= new SettingInstance
            {
                SpeakerId = Config.SpeakerId,
                SpeakerMode = Config.SpeakerMode,
                SpeakerUuid = Config.SpeakerUuid,
                SpeakerStyleId = Config.SpeakerStyleId
            };

            return _settingInstance;
        }
    }

    public ApiConfig Config
    {
        get
        {
            if (_config is null)
                throw new ArgumentNullException(nameof(_config));
            return _config.Value;
        }
    }

    // When file hot reload, the config should be also reload I guess.
    public ConfigHelper Init(IOptions<ApiConfig> config)
    {
        _config = config;

        return this;
    }

    // public ConfigHelper Init(IOptions<ApiConfig> config)
    // {
    //     _config ??= config.Value;
    //     
    //     return this;
    // }
}