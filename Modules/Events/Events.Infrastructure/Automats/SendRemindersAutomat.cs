using Base;
using Events.Domain.Dictionaries;
using Events.Domain.Repositories;
using Events.Infrastructure.Jobs;
using SharedEvents;

namespace Events.Infrastructure.Automats
{
    public class SendRemindersAutomat : AutomationBase.SystemAutomate
    {
        private readonly IEventRepository _eventRepository;

        public SendRemindersAutomat(IEventRepository eventRepository, IJobEngine jobEngine)
            : base(jobEngine)
        {
            _eventRepository = eventRepository;
        }

        public override int Key => SystemAutomats.SentEventReminders;

        public override bool CanExecute(INotifyEvent @event)
            => @event is LogEvent logEvent && logEvent.LogId == AutomationEvents.EventCreated;

        public override async Task Execute(INotifyEvent @event)
        {
            var lastAddedEvent = _eventRepository.GetLastAdded();
            if (lastAddedEvent is null || lastAddedEvent.EventDate is null)
                return;

            var schema = CreateJobSchema($"Send reminders for event {lastAddedEvent.Title}");

            schema.AddJob(new SendReminderJob
            {
                Event = lastAddedEvent
            });

            schema.RequestDate = lastAddedEvent.EventDate.GetValueOrDefault().AddHours(-1);

            await ExecuteJobSchema(schema);
        }
    }
}
