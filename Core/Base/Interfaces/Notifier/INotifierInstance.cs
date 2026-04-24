namespace Base
{
    /// <summary>
    ///     Instance of notifier
    /// </summary>
    public interface INotifierInstance
    {
        Task Success(int messageId, params object[] args);

        Task Error(int messageId, params object[] args);

        Task Warning(int messageId, params object[] args);

        Task Info(int messageId, params object[] args);

        Task Process(int messageId, params object[] args);

        Task ProcessError(int messageId, params object[] args);
    }
}