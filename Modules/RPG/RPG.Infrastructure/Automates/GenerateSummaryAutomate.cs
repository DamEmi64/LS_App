using Base;
using Base.Automation;
using RPG.Domain.Dictionaries;
using RPG.Infrastructure.Jobs;
using SharedEvents;

namespace RPG.Infrastructure.Automates
{
    public class GenerateSummaryAutomate : AutomationBase.SystemAutomate
    {
        private static DateTime LastEditTimeSpan = DateTime.MinValue;

        public GenerateSummaryAutomate(IJobEngine jobEngine)
            : base(jobEngine)
        {
        }

        public override int Key => SystemAutomats.GenerateSummary;

        public override bool CanExecute(INotifyEvent @event)
        {
            if (LastEditTimeSpan.AddMinutes(30) > DateTime.Now)
                return false;

            if (@event is LogEvent logEvent)
            {
                var eventId = ConvertToEventId(logEvent.LogId);

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

        private int? ConvertToEventId(int notifyTypeId)
         => true switch
         {
             true when SessionLogs.SessionSaved is { Key: var k1 } && k1 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionLogs.SessionUpdated is { Key: var k2 } && k2 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionLogs.ChapterUpdated is { Key: var k3 } && k3 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionLogs.ChapterSaved is { Key: var k4 } && k4 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionLogs.PlaceSaved is { Key: var k5 } && k5 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionLogs.PlaceUpdated is { Key: var k6 } && k6 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionLogs.HeroSaved is { Key: var k7 } && k7 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionLogs.HeroUpdated is { Key: var k8 } && k8 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             _ => null
         };

    }
}
