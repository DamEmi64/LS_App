using Base;
using Base.Automation;
using Events.Domain.Dictionaries;
using Events.Domain.Enums;
using Events.Domain.Repositories;
using Events.Infrastructure.Jobs;
using Newtonsoft.Json;

namespace Events.Infrastructure.Services.AutomationResolver
{
    public class EventAutomationResolver : IAutomationResolver
    {
        private readonly IEventRepository _eventRepository;
        private readonly IConnect _connectClient;

        public EventAutomationResolver(IEventRepository eventRepository, IConnect connectClient)
        {
            _eventRepository = eventRepository;
            _connectClient = connectClient;
        }

        public int? ConvertToEventId(int notifyTypeId)
         => true switch
         {
             true when EventNotifyTypes.EventCreated is { Key: var k1 } && k1 == notifyTypeId => AutomationEvents.EventCreated?.Key,
             true when EventNotifyTypes.EventSignIn is { Key: var k1 } && k1 == notifyTypeId => AutomationEvents.UserSignIn?.Key,
             _ => null
         };

        public void Resolve(IProcessSchema schema, IEnumerable<AutomationTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.Operation == Operations.SendReminder)
                {
                    var autoReminderSettings = JsonConvert.DeserializeObject<AutoReminderEnum>(task.JsonData ?? string.Empty);
                    var lastAddedEvent = _eventRepository.GetLastAdded();
                    if (lastAddedEvent is null || lastAddedEvent.EventDate is null)
                        continue;

                    foreach (var participate in lastAddedEvent.Participates)
                    {
                        var job = new SendReminderJob
                        {
                            Event = lastAddedEvent,
                            Receiver = new UserData
                            {
                                UserId = participate.UserId,
                                Email = participate.Email,
                                Login = participate.Login
                            },
                            RequestDate = CalcDate(lastAddedEvent.EventDate ?? DateTime.Now, autoReminderSettings)
                        };
                        schema.AddJob(job);
                    }

                    task.Handled = true;
                }
                else if (task.Operation == Operations.SendInvitation)
                {
                    var lastAddedEvent = _eventRepository.GetLastAdded();
                    if (lastAddedEvent is null || lastAddedEvent.EventDate is null)
                        continue;

                    var usersResult = _connectClient.Send<GetUsers,List<UserData>>(new GetUsers()).Result;

                    if (usersResult.IsFailed)
                        continue;

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
                            },
                            RequestDate = DateTime.Now
                        };
                        schema.AddJob(job);
                    }

                    task.Handled = true;
                }
            }
        }

        private DateTime CalcDate(DateTime eventDate, AutoReminderEnum autoReminder)
        => autoReminder switch
        {
            AutoReminderEnum.Min15 => eventDate.AddMinutes(-15),
            AutoReminderEnum.Min30 => eventDate.AddMinutes(-30),
            AutoReminderEnum.Day1 => eventDate.AddDays(-1),
            AutoReminderEnum.Week1 => eventDate.AddDays(-7),
            AutoReminderEnum.Month1 => eventDate.AddMonths(-1),
            _ => throw new ArgumentOutOfRangeException(nameof(autoReminder), autoReminder, null)
        };
    }
}
