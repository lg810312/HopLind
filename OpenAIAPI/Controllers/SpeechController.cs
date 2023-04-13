using Microsoft.AspNetCore.Mvc;
using HopLind.OpenAIService.Models;

namespace HopLind.OpenAIAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SpeechController : ControllerBase
{
    private readonly ILogger<SpeechController> _logger;

    private readonly OpenAIService.OpenAIService _openAIService;

    public SpeechController(ILogger<SpeechController> logger, OpenAIService.OpenAIService openAIService)
    {
        _logger = logger;
        _openAIService = openAIService;
    }

    /// <summary>
    /// OpenAI Whisper
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="File"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("Speech2Text")]
    public async Task<ActionResult<string>> Speech2TextAsync(WhisperRequest Data, IFormFile File)
    {
        if (Data is null) throw new ArgumentNullException(nameof(Data));
        if (File?.Length is null or 0) throw new ArgumentNullException(nameof(File));

        byte[] FileData = new byte[File.Length];
        await File.OpenReadStream().ReadAsync(FileData);
        var Whisper = await _openAIService.WhisperAsync(Data, FileData, File.FileName);
        _logger.LogDebug("{Whisper}", Whisper);
        return Whisper.Text;
    }
}