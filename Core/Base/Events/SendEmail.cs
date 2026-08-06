using Base.Connect;

namespace Base;

/// <summary>
///     Sending email event
/// </summary>
/// <param name="To">Recipient</param>
/// <param name="Subject">Email subject</param>
/// <param name="Body">Email body</param>
/// <param name="From">Sender</param>
/// <param name="MessageId">Custom message Id for correlation</param>
/// <param name="Register">Indicates whether to register the email in database</param>
public record SendEmail(
    string To,
    string Subject,
    string Body,
    string? From = null,
    string? MessageId = null,
    bool Register = true
) : IEvent;
