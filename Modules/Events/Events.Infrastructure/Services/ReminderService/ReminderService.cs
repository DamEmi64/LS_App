using Base;
using Events.Domain.Entities;
using Events.Infrastructure.Jobs;

namespace Events.Infrastructure.Services.ReminderService
{
    public class ReminderService : IReminderService
    {
        private readonly IJobEngine _jobEngine;

        public ReminderService(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }

        public async Task AddReminder(DateTimeOffset remindAt, Event eventData, UserData currentUser)
        {
            var schema = _jobEngine.Create($"Sending reminder for {eventData.Title}");
            schema.AddJob(new SendReminderJob
            {
                Event = eventData
            });
            eventData.ReminderProcess = await _jobEngine.Execute(schema, currentUser);
        }

        public Task RemoveReminder(Event eventData)
        {
            if (eventData.ReminderProcess.HasValue)
            {
                return _jobEngine.Cancel(eventData.ReminderProcess.Value);
            }
            return Task.CompletedTask;
        }
    }
}
