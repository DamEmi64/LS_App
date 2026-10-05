namespace Base;

/// <summary>
///     Basic event for notification
/// </summary>
public record NotifyEvent;

public record MediaSavedEvent(Guid Id, string extension = "pdf", string? Owner = null) : NotifyEvent;
public record MediaDeletedEvent(Guid Id) : NotifyEvent;

public record LogEvent(int LogId, LogLevel Level, params object[] Args) : NotifyEvent;

public enum LogLevel
{
    Info,
    Warning,
    Error,
    Success,
    ProcessInfo,
    ProcessError,
}