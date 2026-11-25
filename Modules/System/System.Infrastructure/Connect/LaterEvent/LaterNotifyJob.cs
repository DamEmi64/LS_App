using Base;
using Newtonsoft.Json;
using System.Reflection;

namespace System.Infrastructure.Connect.LaterEvent
{
    /// <summary>
    /// Hangfire entry point for delayed notifications. Only strings are persisted in the
    /// Hangfire invocation, so the concrete event type and its payload survive scheduling.
    /// </summary>
    public class LaterNotifyJob
    {
        private readonly Notifier _notifier;

        public LaterNotifyJob(Notifier notifier)
        {
            _notifier = notifier;
        }

        public async Task Execute(string eventTypeName, string eventData)
        {
            var eventType = Type.GetType(eventTypeName, throwOnError: true)!;
            if (!typeof(NotifyEvent).IsAssignableFrom(eventType) || eventType.IsAbstract)
            {
                throw new InvalidOperationException($"'{eventTypeName}' is not a concrete notification event type.");
            }

            var notifyEvent = JsonConvert.DeserializeObject(eventData, eventType) as NotifyEvent
                ?? throw new InvalidOperationException($"Could not deserialize notification event '{eventTypeName}'.");

            var notifyMethod = typeof(Notifier)
                .GetMethod(nameof(Notifier.Notify), BindingFlags.Instance | BindingFlags.Public)!
                .MakeGenericMethod(eventType);
            var task = (Task?)notifyMethod.Invoke(_notifier, [notifyEvent])
                ?? throw new InvalidOperationException($"Could not dispatch notification event '{eventTypeName}'.");

            await task;
        }
    }
}