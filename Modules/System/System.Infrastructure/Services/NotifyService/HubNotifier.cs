using Base;
using Microsoft.AspNetCore.SignalR;
using System.Infrastructure.Hubs;

namespace System.Infrastructure.Services.NotifyService
{
    public class HubNotifier : INotifierInstance
    {
        private readonly IHubContext<NotifyHub> _hub;

        public HubNotifier(IHubContext<NotifyHub> hub)
        {
            _hub = hub;
        }

        public Task Error(int messageId, params object[] args)
        {
            return _hub.Clients.All.SendAsync(NotifyHub.NotifyMethod, "error", messageId, args);
        }

        public Task Info(int messageId, params object[] args)
        {
            return _hub.Clients.All.SendAsync(NotifyHub.NotifyMethod, "info", messageId, args);
        }

        public Task Process(int messageId, params object[] args)
        {
            return _hub.Clients.All.SendAsync(NotifyHub.NotifyMethod, "process", messageId, args);
        }

        public Task ProcessError(int messageId, params object[] args)
        {
            return _hub.Clients.All.SendAsync(NotifyHub.NotifyMethod, "process-error", messageId, args);
        }

        public Task Success(int messageId, params object[] args)
        {
            return _hub.Clients.All.SendAsync(NotifyHub.NotifyMethod, "success", messageId, args);
        }

        public Task Warning(int messageId, params object[] args)
        {
            return _hub.Clients.All.SendAsync(NotifyHub.NotifyMethod, "warning", messageId, args);
        }
    }
}