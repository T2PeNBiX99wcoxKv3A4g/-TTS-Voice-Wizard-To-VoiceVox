using System.Net;
using System.Text.Json.Serialization;

namespace TTSToVOICEVOX.Models;

public class Error
{
    [JsonPropertyName("status_code")]
    public required HttpStatusCode StatusCode { get; init; }
    
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}