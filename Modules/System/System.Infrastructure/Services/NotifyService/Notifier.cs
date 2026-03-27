using Base;

namespace System.Infrastructure.Services.NotifyService
{
    public class Notifier : INotifier
    {
        private readonly IEnumerable<INotifierInstance> _notifiers;

        public Notifier(IEnumerable<INotifierInstance> notifiers)
        {
            _notifiers = notifiers;
        }

        public async Task Error(int messageId, params object[] args)
        {
            foreach (var item in _notifiers)
            {
                await item.Error(messageId, args);
            }
        }

        public async Task Info(int messageId, params object[] args)
        {
            foreach (var item in _notifiers)
            {
                await item.Info(messageId, args);
            }
        }

        public async Task Process(int messageId, params object[] args)
        {
            foreach (var item in _notifiers)
            {
                await item.Process(messageId, args);
            }
        }

        public async Task ProcessError(int messageId, params object[] args)
        {
            foreach (var item in _notifiers)
            {
                await item.ProcessError(messageId, args);
            }
        }

        public async Task Success(int messageId, params object[] args)
        {
            foreach (var item in _notifiers)
            {
                await item.Success(messageId, args);
            }
        }

        public async Task Warning(int messageId, params object[] args)
        {
            foreach (var item in _notifiers)
            {
                await item.Warning(messageId, args);
            }
        }
    }
}
