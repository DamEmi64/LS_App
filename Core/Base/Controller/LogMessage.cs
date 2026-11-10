using System.Text.Json.Serialization;

namespace Base;

/// <summary>
/// A message dictionary ID and the arguments used to format its localized text.
/// </summary>
public sealed record LogMessage(
    [property: JsonPropertyName("messageId")] int MessageId,
    [property: JsonPropertyName("args")] object[] Args);
