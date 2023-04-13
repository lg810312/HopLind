using System.ComponentModel;

namespace HopLind.OpenAIService.Models;

public class ChatGPTConstant
{
    public const string ChatGPTRoleSystem = "system";
    public const string ChatGPTRoleUser = "user";
    public const string ChatGPTRoleAssistant = "assistant";
}

public record ChatGPTMessage(string Role, string Content);

public enum ChoiceFinishReason
{
    stop,
    length,
    content_filter,
}
//public readonly struct ChoiceFinishReason : IEquatable<ChoiceFinishReason>
//{
//    private readonly string _value;

//    public ChoiceFinishReason(string value)
//    {
//        _value = value ?? throw new ArgumentNullException(nameof(value));
//    }

//    private const string StopValue = "stop";
//    private const string LengthValue = "length";
//    private const string ContentFilterValue = "content_filter";

//    public static ChoiceFinishReason Stop { get; } = new ChoiceFinishReason(StopValue);
//    public static ChoiceFinishReason Length { get; } = new ChoiceFinishReason(LengthValue);
//    public static ChoiceFinishReason ContentFilter { get; } = new ChoiceFinishReason(ContentFilterValue);

//    public static bool operator ==(ChoiceFinishReason left, ChoiceFinishReason right) => left.Equals(right);
//    public static bool operator !=(ChoiceFinishReason left, ChoiceFinishReason right) => !left.Equals(right);
//    public static implicit operator ChoiceFinishReason(string value) => new(value);

//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public override bool Equals(object obj) => obj is ChoiceFinishReason other && Equals(other);
//    public bool Equals(ChoiceFinishReason other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public override int GetHashCode() => _value?.GetHashCode() ?? 0;

//    public override string ToString() => _value;

//    public static class Values
//    {
//        public const string Stop = StopValue;
//        public const string Length = LengthValue;
//        public const string ContentFilter = ContentFilterValue;
//    }
//}

public readonly struct Speech2TextResponseFormat : IEquatable<Speech2TextResponseFormat>
{
    private readonly string _value;

    public Speech2TextResponseFormat(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    private const string JsonValue = "json";
    private const string TextValue = "text";
    private const string SrtValue = "srt";
    private const string VerboseJsonValue = "verbose_json";
    private const string VttValue = "vtt";

    public static Speech2TextResponseFormat Json { get; } = new Speech2TextResponseFormat(JsonValue);
    public static Speech2TextResponseFormat Text { get; } = new Speech2TextResponseFormat(TextValue);
    public static Speech2TextResponseFormat Srt { get; } = new Speech2TextResponseFormat(SrtValue);
    public static Speech2TextResponseFormat VerboseJson { get; } = new Speech2TextResponseFormat(VerboseJsonValue);
    public static Speech2TextResponseFormat Vtt { get; } = new Speech2TextResponseFormat(VttValue);

    public static bool operator ==(Speech2TextResponseFormat left, Speech2TextResponseFormat right) => left.Equals(right);
    public static bool operator !=(Speech2TextResponseFormat left, Speech2TextResponseFormat right) => !left.Equals(right);
    public static implicit operator Speech2TextResponseFormat(string value) => new(value);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => obj is Speech2TextResponseFormat other && Equals(other);
    public bool Equals(Speech2TextResponseFormat other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => _value?.GetHashCode() ?? 0;

    public override string ToString() => _value;

    public static class Values
    {
        public const string Json = JsonValue;
        public const string Text = TextValue;
        public const string Srt = SrtValue;
        public const string VerboseJson = VerboseJsonValue;
        public const string Vtt = VttValue;
    }
}