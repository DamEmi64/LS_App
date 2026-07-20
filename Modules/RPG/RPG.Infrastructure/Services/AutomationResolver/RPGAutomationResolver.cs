using Base;
using Base.Automation;
using Newtonsoft.Json;
using RPG.Domain.Dictionaries;
using RPG.Infrastructure.Jobs;

namespace RPG.Infrastructure.Services
{
    public class RPGAutomationResolver : IAutomationResolver
    {
        public int? ConvertToEventId(int notifyTypeId)
         => true switch
         {
             true when SessionNotifyTypes.SessionSaved is { Key: var k1 } && k1 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionNotifyTypes.SessionUpdated is { Key: var k2 } && k2 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionNotifyTypes.ChapterUpdated is { Key: var k3 } && k3 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionNotifyTypes.ChapterSaved is { Key: var k4 } && k4 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionNotifyTypes.PlaceSaved is { Key: var k5 } && k5 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionNotifyTypes.PlaceUpdated is { Key: var k6 } && k6 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionNotifyTypes.HeroSaved is { Key: var k7 } && k7 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             true when SessionNotifyTypes.HeroUpdated is { Key: var k8 } && k8 == notifyTypeId => AutomationEvents.RPGEdited?.Key,
             _ => null
         };

        public void Resolve(IProcessSchema schema, IEnumerable<AutomationTask> tasks)
        {
            var currentSchema = schema;
            foreach (var task in tasks)
            {
                if (task.Operation == Operations.GenerateSummary)
                {
                    var genJob = new GetLastEditedRPGJob();
                    var job = new GenerateSummaryJob();
                    JsonConvert.PopulateObject(task.JsonData ?? string.Empty, job);
                    currentSchema = currentSchema
                                    .AddJob(genJob)
                                    .AddChildJob(job);
                    task.Handled = true;
                }
            }
        }
    }
}
