namespace Base
{
    /// <summary>
    ///     Instance of notifier
    /// </summary>
    public interface INotifyListener
    {
        Task Notify<T>(T @event) where T : NotifyEvent;
    }
}