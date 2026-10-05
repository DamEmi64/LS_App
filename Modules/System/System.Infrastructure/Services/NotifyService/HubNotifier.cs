using Base;
using Microsoft.AspNetCore.SignalR;
using System.Infrastructure.Hubs;

namespace System.Infrastructure.Services.NotifyService
{
    public class HubNotifier : INotifyListener
    {
        private readonly IHubContext<NotifyHub> _hub;

        public HubNotifier(IHubContext<NotifyHub> hub)
        {
            _hub = hub;
        }

        public Task Notify<T>(T @event) where T : NotifyEvent
        {
            if (@event is LogEvent logEvent)
            {
                return _hub.Clients.All.SendAsync(NotifyHub.NotifyMethod, logEvent.Level.ToString().ToLower(), logEvent.LogId, logEvent.Args);
            }

            return Task.CompletedTask;
        }
    }
}