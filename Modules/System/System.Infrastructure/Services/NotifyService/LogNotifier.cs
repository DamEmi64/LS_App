using Base;

namespace System.Infrastructure.Services.NotifyService
{
    public class LogNotifier : ILogNotifier
    {
        private readonly Notifier _notifier;

        public LogNotifier(Notifier notifier)
        {
            _notifier = notifier;
        }

        public Task Error(int messageId, params object[] args) => _notifier.Notify(new LogEvent(LogId: messageId, Args: args, Level: LogLevel.Error));

        public Task Info(int messageId, params object[] args) => _notifier.Notify(new LogEvent(LogId: messageId, Args: args, Level: LogLevel.Info));

        public Task Process(int messageId, params object[] args) => _notifier.Notify(new LogEvent(LogId: messageId, Args: args, Level: LogLevel.ProcessInfo));

        public Task ProcessError(int messageId, params object[] args) => _notifier.Notify(new LogEvent(LogId: messageId, Args: args, Level: LogLevel.ProcessError));

        public Task Success(int messageId, params object[] args) => _notifier.Notify(new LogEvent(LogId: messageId, Args: args, Level: LogLevel.Success));

        public Task Warning(int messageId, params object[] args) => _notifier.Notify(new LogEvent(LogId: messageId, Args: args, Level: LogLevel.Warning));
    }
}
