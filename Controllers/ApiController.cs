using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using TTSToVOICEVOX.Configs;
using TTSToVOICEVOX.Helper;
using TTSToVOICEVOX.Models;

namespace TTSToVOICEVOX.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController(ILogger<ApiController> logger, IOptions<ApiConfig> configOptions) : ControllerBase
{
    internal const string Title = "TTS to VOICEVOX (COEIROINK) transfer server";
    internal const string Version = "0.1.0";

    private static readonly HttpClient Client = new();
    private readonly ConfigHelper _configHelper = ConfigHelper.Instance.Init(configOptions);

    private ObjectResult Error(HttpStatusCode statusCode, params object[] text)
    {
        var fullText = string.Join(" ", text);
        logger.LogError("{fullText}", fullText);
        var simpleTexts = text.Length > 1 ? text.Where((_, index) => index != 0) : text;
        var simpleText = string.Join(" ", simpleTexts);

        return StatusCode((int)statusCode, new ErrorDetails
        {
            Detail =
            [
                new Error
                {
                    StatusCode = statusCode,
                    Message = simpleText
                }
            ]
        });
    }

    private async Task<ObjectResult> ErrorHandleSimple(HttpResponseMessage response)
    {
        var json = await response.Content.ReadFromJsonAsync<SimpleError>();
        logger.LogDebug("Error Json {json}", json);

        if (json == null)
            return Error(HttpStatusCode.InternalServerError, "Server Error:", "Unknown error.");

        logger.LogDebug("Error detail {detail}", json.Detail);

        return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorDetails
        {
            Detail =
            [
                new Error
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = json.Detail
                }
            ]
        });
    }

    private async Task<ObjectResult> ErrorHandle(HttpResponseMessage response)
    {
        var json = await response.Content.ReadFromJsonAsync<HttpValidationError>();
        logger.LogDebug("Error Json {json}", json);

        if (json == null)
            return await ErrorHandleSimple(response);

        // ReSharper disable once CoVariantArrayConversion
        logger.LogDebug("Error detail {detail}", json.Detail);
        var errors = new List<Error>();

        foreach (var err in json.Detail)
        {
            var loc = err.Loc;
            var msg = err.Msg;
            var typ = err.Type;
            var errorMsg = $"({typ}) error in ({string.Join(" - ", loc)}), {msg}";

            errors.Add(new Error
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = errorMsg
            });
            logger.LogError("Server Error: {errorMsg}", errorMsg);
        }

        return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorDetails
        {
            Detail = errors.ToArray()
        });
    }

    private async Task<ObjectResult> TryTweVoiceVoxPost(string url, string json)
    {
        var retryNow = 0;
        
        while (true)
        {
            try
            {
                var response2 = await Client.PostAsync(url,
                    new StringContent(json, Encoding.UTF8, "application/json"));

                logger.LogDebug("Status Code 2 {StatusCode}", response2.StatusCode);

                if (response2.StatusCode != HttpStatusCode.OK)
                    return await ErrorHandle(response2);
                return Ok(Convert.ToBase64String(await response2.Content.ReadAsByteArrayAsync()));
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Server Error: {Stack}", ex.Message);

                if (retryNow >= _configHelper.Config.Retry)
                    return Error(HttpStatusCode.InternalServerError,
                        "Tried several times, still can't connect, please try again later.");
                logger.LogInformation("Retry after {waiting} seconds",
                    _configHelper.Config.RetryWaitingTime / 1000);
                await Task.Delay(_configHelper.Config.RetryWaitingTime);
                retryNow++;
            }
        }
    }

    private async Task<ObjectResult> TryOneVoiceVoxPost(string url)
    {
        var retryNow = 0;
        
        while (true)
        {
            try
            {
                var response = await Client.PostAsync(url, null);

                logger.LogDebug("Status Code {StatusCode}", response.StatusCode);

                if (response.StatusCode != HttpStatusCode.OK)
                    return await ErrorHandle(response);

                var json = await response.Content.ReadAsStringAsync();

                logger.LogDebug("Test {json}", json);

                var url2 = _configHelper.Config.VoiceVoxUrl + "/synthesis";

                var query2 = new Dictionary<string, string?>
                {
                    ["speaker"] = _configHelper.Settings.SpeakerId.ToString()
                };

                var newUrl2 = QueryHelpers.AddQueryString(url2, query2);

                return await TryTweVoiceVoxPost(newUrl2, json);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Server Error: {Stack}", ex.Message);

                if (retryNow >= _configHelper.Config.Retry)
                    return Error(HttpStatusCode.InternalServerError,
                        "Tried several times, still can't connect, please try again later.");
                logger.LogInformation("Retry after {waiting} seconds", _configHelper.Config.RetryWaitingTime / 1000);
                await Task.Delay(_configHelper.Config.RetryWaitingTime);
                retryNow++;
            }
        }
    }

    private async Task<ObjectResult> VoiceVoxPost(string text)
    {
        var url = _configHelper.Config.VoiceVoxUrl + "/audio_query";
        var query = new Dictionary<string, string?>
        {
            ["speaker"] = _configHelper.Settings.SpeakerId.ToString(),
            ["text"] = text
        };
        var newUrl = QueryHelpers.AddQueryString(url, query);

        return await TryOneVoiceVoxPost(newUrl);
    }

    private async Task<ObjectResult> TryTweCoeiroLinkPost(string url, Predict predict)
    {
        var retryNow = 0;
        
        while (true)
        {
            try
            {
                var response2 = await Client.PostAsJsonAsync(url, predict);

                logger.LogDebug("Status Code 2 {StatusCode}", response2.StatusCode);

                if (response2.StatusCode != HttpStatusCode.OK)
                    return await ErrorHandle(response2);
                return Ok(Convert.ToBase64String(await response2.Content.ReadAsByteArrayAsync()));
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Server Error: {msg}", ex.Message);

                if (retryNow >= _configHelper.Config.Retry)
                    return Error(HttpStatusCode.InternalServerError,
                        "Tried several times, still can't connect, please try again later.");
                logger.LogInformation("Retry after {waiting} seconds",
                    _configHelper.Config.RetryWaitingTime / 1000);
                await Task.Delay(_configHelper.Config.RetryWaitingTime);
                retryNow++;
            }
        }
    }

    private async Task<ObjectResult> TryOneCoeiroLinkPost(string url, TextData json, string text)
    {
        var retryNow = 0;
        
        while (true)
        {
            try
            {
                var response = await Client.PostAsJsonAsync(url, json);

                logger.LogDebug("Status Code {StatusCode}", response.StatusCode);

                if (response.StatusCode != HttpStatusCode.OK)
                    return await ErrorHandle(response);

                var prosody = await response.Content.ReadFromJsonAsync<EstimateProsody>();

#if DEBUG
                if (prosody != null)
                {
                    foreach (var plain in prosody.Plain)
                    {
                        logger.LogDebug("prosody.Plain.string {plain}", plain);
                    }

                    foreach (var detail in prosody.Detail.SelectMany(list => list))
                    {
                        logger.LogDebug("prosody.Detail.Phoneme: {Phoneme}", detail.Phoneme);
                        logger.LogDebug("prosody.Detail.Hira: {Hira}", detail.Hira);
                        logger.LogDebug("prosody.Detail.Accent: {Accent}", detail.Accent);
                    }
                }
                else
                    logger.LogDebug("prosody is null");
#endif

                if (prosody == null)
                    logger.LogWarning("Can't get prosody data");

                var predict = new Predict
                {
                    SpeakerUuid = _configHelper.Settings.SpeakerUuid,
                    StyleId = _configHelper.Settings.SpeakerStyleId,
                    Text = text,
                    ProsodyDetail = prosody != null ? prosody.Detail : [],
                    SpeedScale = 1
                };

                var url2 = _configHelper.Config.CoeiroLinkUrl + "/v1/predict";

                return await TryTweCoeiroLinkPost(url2, predict);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("Server Error: {msg}", ex.Message);

                if (retryNow >= _configHelper.Config.Retry)
                    return Error(HttpStatusCode.InternalServerError,
                        "Tried several times, still can't connect, please try again later.");
                logger.LogInformation("Retry after {waiting} seconds", _configHelper.Config.RetryWaitingTime / 1000);
                await Task.Delay(_configHelper.Config.RetryWaitingTime);
                retryNow++;
            }
        }
    }

    private async Task<ObjectResult> CoeiroLinkPost(string text)
    {
        var url = _configHelper.Config.CoeiroLinkUrl + "/v1/estimate_prosody";
        var json = new TextData
        {
            Text = text
        };

        return await TryOneCoeiroLinkPost(url, json, text);
    }

    // ReSharper disable once StringLiteralTypo
    [HttpGet("/synthesize")]
    public async Task<ObjectResult> GetSynthesize()
    {
        var keys = Request.Query.Keys;

        if (keys.Count < 1)
            return Error(HttpStatusCode.UnprocessableContent, "No input");

        var text = keys.First();

        if (string.IsNullOrWhiteSpace(text))
            return Error(HttpStatusCode.UnprocessableContent, "No input");

        logger.LogDebug("Synthesize Text: {text}", text);

        ObjectResult ret;
        var newText = "";

        // ReSharper disable AccessToModifiedClosure
        foreach (var textReplace in from textReplace in _configHelper.Config.TextReplace
                 where text.Contains(textReplace.Key)
                 let index = text.IndexOf(textReplace.Key, StringComparison.OrdinalIgnoreCase)
                 where text.IndexOf(textReplace.Key, index + 1, StringComparison.OrdinalIgnoreCase) >= 0
                 select textReplace)
        {
            newText = text.Replace(textReplace.Key, textReplace.Value);
        }
        // ReSharper restore AccessToModifiedClosure

        if (!string.IsNullOrWhiteSpace(newText))
            text = newText;

        if (_configHelper.Settings.SpeakerMode == 1)
            ret = await CoeiroLinkPost(text);
        else
            ret = await VoiceVoxPost(text);
        return ret;
    }

    [HttpGet("/settings")]
    public Setting GetSetting()
    {
        return new Setting
        {
            SpeakerId = _configHelper.Settings.SpeakerId,
            SpeakerMode = _configHelper.Settings.SpeakerMode,
            SpeakerUuid = _configHelper.Settings.SpeakerUuid,
            SpeakerStyleId = _configHelper.Settings.SpeakerStyleId
        };
    }

    [HttpPost("/settings")]
    public MessageData PostSetting(Setting setting)
    {
        _configHelper.Settings.SpeakerId = setting.SpeakerId;
        _configHelper.Settings.SpeakerMode = setting.SpeakerMode;
        _configHelper.Settings.SpeakerUuid = setting.SpeakerUuid;
        _configHelper.Settings.SpeakerStyleId = setting.SpeakerStyleId;

        return new MessageData
        {
            Message = "Apply new setting."
        };
    }
}