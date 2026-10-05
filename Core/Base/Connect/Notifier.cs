namespace Base;

/// <summary>
///     Notifier
/// </summary>
public class Notifier
{
    private readonly IEnumerable<INotifyListener> _notifiers;
    public Notifier(IEnumerable<INotifyListener> notifiers)
    {
        _notifiers = notifiers.ToList();
    }

    public async Task Notify<T>(T @event) where T : NotifyEvent
    {
        foreach (var item in _notifiers)
        {
            await item.Notify(@event);
        }
    }
}

/// <summary>
///     Basic event for notification
/// </summary>
public record NotifyEvent;