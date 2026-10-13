using Base;
using Events.Domain.Dictionaries;
using Events.Domain.Repositories;
using Events.Infrastructure.Jobs;
using SharedEvents;

namespace Events.Infrastructure.Automats
{
    public class SendInvitationAutomat : AutomationBase.SystemAutomate
    {
        private readonly IEventRepository _eventRepository;
        private readonly IConnect _connect;

        public SendInvitationAutomat(IEventRepository eventRepository, IConnect connect, IJobEngine jobEngine)
            :base(jobEngine)
        {
            _eventRepository = eventRepository;
            _connect = connect;
        }

        public override int Key => SystemAutomats.SentEventInvitations;

        public override bool CanExecute(INotifyEvent @event)
            => @event is LogEvent logEvent && logEvent.LogId == AutomationEvents.EventCreated;

        public override async Task Execute(INotifyEvent @event)
        {
            var lastAddedEvent = _eventRepository.GetLastAdded();
            if (lastAddedEvent is null || lastAddedEvent.EventDate is null)
                return;

            var schema = CreateJobSchema($"Send invitation for event {lastAddedEvent.Title}");

            var usersResult = await _connect.Send<SharedEvents.GetUsers, List<UserData>>(new SharedEvents.GetUsers());

            if (usersResult.IsFailed)
                return;

            foreach (var user in usersResult.Value)
            {
                var job = new SendInvitationJob
                {
                    Event = lastAddedEvent,
                    Receiver = new UserData
                    {
                        UserId = user.UserId,
                        Email = user.Email,
                        Login = user.Login
                    }
                };
                schema.AddJob(job);
            }

            await ExecuteJobSchema(schema);
        }
    }
}
