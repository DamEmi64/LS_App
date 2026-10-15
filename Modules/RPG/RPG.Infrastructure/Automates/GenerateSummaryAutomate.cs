using Base;
using Base.Automation;
using RPG.Domain.Dictionaries;
using RPG.Infrastructure.Jobs;
using SharedEvents;

namespace RPG.Infrastructure.Automates
{
    public class GenerateSummaryAutomate : AutomationBase.SystemAutomate
    {
        private readonly IAutomationResolver _automationResolver;

        private static DateTime LastEditTimeSpan = DateTime.MinValue;

        public GenerateSummaryAutomate(IAutomationResolver automationResolver, IJobEngine jobEngine)
            : base(jobEngine)
        {
            _automationResolver = automationResolver;
        }

        public override int Key => SystemAutomats.GenerateSummary;

        public override bool CanExecute(INotifyEvent @event)
        {
            if (LastEditTimeSpan.AddMinutes(30) > DateTime.Now)
                return false;

            if (@event is LogEvent logEvent)
            {
                var eventId = _automationResolver.ConvertToEventId(logEvent);

                return eventId == AutomationEvents.RPGEdited;
            }

            return false;
        }

        public override async Task Execute(INotifyEvent @event, string configuration)
        {
            LastEditTimeSpan = DateTime.Now;
            var genJob = new GetLastEditedRPGJob();
            var job = new GenerateSummaryJob();
            var schema = CreateJobSchema("Generate summary for last edited RPG");
            schema = schema
                    .AddJob(genJob)
                    .AddChildJob(job);

            await ExecuteJobSchema(schema);
        }
    }
}
