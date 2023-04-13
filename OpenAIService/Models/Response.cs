namespace HopLind.OpenAIService.Models;

public record EmbeddingData(decimal[] Embedding);

public record EmbeddingResponse(EmbeddingData[] Data);

public class OpenAIUsage
{
    public int CompletionTokens { get; set; }
    public int PromptTokens { get; set; }
    public int TotalTokens { get; set; }
}

public abstract class Choice
{
    public ChoiceFinishReason FinishReason { get; set; }
}

public class GPTChoice : Choice
{
    public string Text { get; set; }
}

public class ChatGPTChoice : Choice
{
    public ChatGPTMessage Message { get; set; }
}

public abstract record GPTResponseBase(OpenAIUsage Usage);

public record GPTResponse(GPTChoice[] Choices, OpenAIUsage Usage) : GPTResponseBase(Usage);

public record ChatGPTResponse(ChatGPTChoice[] Choices, OpenAIUsage Usage) : GPTResponseBase(Usage);

public record WhisperResponse(string Text);