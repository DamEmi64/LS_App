using Base;
using Hangfire;
using Newtonsoft.Json;

namespace System.Infrastructure.Connect.LaterEvent
{
    public class LaterEventHandler : ConnectMethod<SharedEvents.LaterEvent>
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public LaterEventHandler(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public override Task HandleAsync(SharedEvents.LaterEvent request, CancellationToken cancellationToken)
        {
            var eventType = request.Data.GetType();
            var eventTypeName = eventType.AssemblyQualifiedName
                ?? throw new InvalidOperationException($"Cannot schedule event type '{eventType.FullName}'.");
            var eventData = JsonConvert.SerializeObject(request.Data, eventType, null);

            _backgroundJobClient.Schedule<LaterNotifyJob>(
                job => job.Execute(eventTypeName, eventData),
                request.Date);

            return Task.CompletedTask;
        }
    }
}