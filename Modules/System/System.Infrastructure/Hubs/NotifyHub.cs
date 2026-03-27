using Microsoft.AspNetCore.SignalR;

namespace System.Infrastructure.Hubs
{
    public class NotifyHub : Hub
    {
        public const string NotifyMethod = "ReceiveNotification";
    }
}