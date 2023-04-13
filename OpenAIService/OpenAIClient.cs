using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using HopLind.OpenAIService.Models;

namespace HopLind.OpenAIService;

public enum APIType
{
    OpenAI,
    Azure
}

public class Config
{
    public APIType APIType { get; set; }
    public string APIBase { get; set; }
    public string APIKey { get; set; }
    public string APIVersion { get; set; }
}

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var sb = new StringBuilder();
        var charArray = name.ToCharArray();
        bool lastWasUpper = false;

        for (int i = 0; i < charArray.Length; i++)
        {
            char ch = charArray[i];

            if (ch == ' ')
            {
                sb.Append('_');
                lastWasUpper = false;
                continue;
            }

            if (char.IsUpper(ch))
            {
                if (!lastWasUpper && i > 0) sb.Append('_');
                lastWasUpper = true;
            }
            else
                lastWasUpper = false;

            sb.Append(char.ToLower(ch));
        }

        return sb.ToString();
    }
}

public class OpenAIService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly Config _config;

    private readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        Converters = { new JsonStringEnumConverter(new SnakeCaseNamingPolicy()) },
    };

    public OpenAIService(IHttpClientFactory httpClientFactory, IOptions<Config> config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config.Value;
    }

    /// <summary>
    /// OpenAI Embeddings
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    public async Task<EmbeddingResponse> EmbeddingAsync(EmbeddingReuqest Data)
    {
        string APIEndPoint = _config.APIType is APIType.OpenAI ? $"{_config.APIBase}/embeddings" :
            $"{_config.APIBase}/openai/deployments/{Data.Model}/embeddings?api-version={_config.APIVersion}";
        var Content = JsonContent.Create(Data, options: JsonSerializerOptions);
        var Request = new HttpRequestMessage(HttpMethod.Post, APIEndPoint)
        {
            Content = Content,
        };
        if (_config.APIType is APIType.OpenAI)
            Request.Headers.Add(HeaderNames.Authorization, $"Bearer {_config.APIKey}");
        else
            Request.Headers.Add("api-key", _config.APIKey);

        var Response = await _httpClientFactory.CreateClient().SendAsync(Request);
        string ContentString = await Response.Content.ReadAsStringAsync();
        var Result = Response.IsSuccessStatusCode ?
            JsonSerializer.Deserialize<EmbeddingResponse>(ContentString, JsonSerializerOptions) : null;
        return Result;
    }

    /// <summary>
    /// OpenAI Chat Completions
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    public async Task<ChatGPTResponse> ChatCompletAsync(ChatGPTRequest Data)
    {
        string APIEndPoint = _config.APIType is APIType.OpenAI ? $"{_config.APIBase}/chat/completions" :
            $"{_config.APIBase}/openai/deployments/{Data.Model}/chat/completions?api-version={_config.APIVersion}";
        var Content = JsonContent.Create(Data, options: JsonSerializerOptions);
        var Request = new HttpRequestMessage(HttpMethod.Post, APIEndPoint)
        {
            Content = Content,
        };
        if (_config.APIType is APIType.OpenAI)
            Request.Headers.Add(HeaderNames.Authorization, $"Bearer {_config.APIKey}");
        else
            Request.Headers.Add("api-key", _config.APIKey);

        var Response = await _httpClientFactory.CreateClient().SendAsync(Request);
        string ContentString = await Response.Content.ReadAsStringAsync();
        var Result = Response.IsSuccessStatusCode ?
            JsonSerializer.Deserialize<ChatGPTResponse>(ContentString, JsonSerializerOptions) : null;
        return Result;
    }

    /// <summary>
    /// OpenAI Whisper
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="FileData"></param>
    /// <param name="FileName"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public async Task<WhisperResponse> WhisperAsync(WhisperRequest Data, byte[] FileData, string FileName)
    {
        const string WhisperModel = "whisper-1";

        if (_config.APIType is APIType.Azure) throw new NotSupportedException("Only support OpenAI Chat Completions API");

        string APIEndPoint = $"{_config.APIBase}/audio/transcriptions";

        var Content = new MultipartFormDataContent
        {
            { new StringContent(WhisperModel), "model" },
            { new ByteArrayContent(FileData), "file", FileName }
        };
        if (!string.IsNullOrWhiteSpace(Data.Prompt)) Content.Add(new StringContent(Data.Prompt), "prompt");
        if (Data.ResponseFormat != null) Content.Add(new StringContent(Data.ResponseFormat.ToString()), "response_format");

        var Request = new HttpRequestMessage(HttpMethod.Post, APIEndPoint)
        {
            Content = Content,
        };
        Request.Headers.Add(HeaderNames.Authorization, $"Bearer {_config.APIKey}");
        var Response = await _httpClientFactory.CreateClient().SendAsync(Request);
        string ContentString = await Response.Content.ReadAsStringAsync();
        var Result = Response.IsSuccessStatusCode ?
            Data.ResponseFormat == Speech2TextResponseFormat.Json || Data.ResponseFormat == Speech2TextResponseFormat.VerboseJson ?
            JsonSerializer.Deserialize<WhisperResponse>(ContentString, JsonSerializerOptions) : new WhisperResponse(await Response.Content.ReadAsStringAsync()) : null;
        return Result;
    }
}
