using Microsoft.AspNetCore.Mvc;
using HopLind.OpenAIService.Models;

namespace HopLind.OpenAIAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CompletionController : ControllerBase
{
    private readonly ILogger<CompletionController> _logger;

    private readonly OpenAIService.OpenAIService _openAIService;

    public CompletionController(ILogger<CompletionController> logger, OpenAIService.OpenAIService openAIService)
    {
        _logger = logger;
        _openAIService = openAIService;
    }

    /// <summary>
    /// OpenAI Embeddings
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("Embeddings")]
    public async Task<ActionResult<IEnumerable<decimal>>> CompleteAsync(EmbeddingReuqest Data)
    {
        if (Data is null) throw new ArgumentNullException(nameof(Data));

        var Completion = await _openAIService.EmbeddingAsync(Data);
        _logger.LogDebug("{Completion}", Completion);
        return Completion.Data[0].Embedding;
    }

    /// <summary>
    /// OpenAI Chat Completions
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("Chat/Completions")]
    public async Task<ActionResult<string>> ChatCompleteAsync(ChatGPTRequest Data)
    {
        if (Data is null) throw new ArgumentNullException(nameof(Data));

        var Completion = await _openAIService.ChatCompletAsync(Data);
        _logger.LogDebug("{Completion}", Completion);
        return Completion.Choices[0].Message.Content;
    }
}