namespace HopLind.OpenAIService.Models;

public record EmbeddingReuqest(string Model, string Input);

public abstract class GPTRequestBase
{
    public string Model { get; set; }

    public int MaxTokens { get; set; } = 800;

    public decimal Temperature { get; set; } = 1;

    public decimal FrequencyPenalty { get; set; }

    public decimal PresencePenalty { get; set; }

    public decimal TopP { get; set; } = 1;

    public string Stop { get; set; }
}

public class ChatGPTRequest : GPTRequestBase
{
    public ChatGPTMessage[] Messages { get; set; }
}

public record WhisperRequest(string Prompt, Speech2TextResponseFormat ResponseFormat);